#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Telenet.Carga.Abstractions;
using Telenet.Core.Data;

namespace Telenet.Carga
{
    public class SolicitacaoCargaManual : SolicitacaoCargaBase<IContextoCargaManual>, ISolicitacaoCargaManual
    {
        public SolicitacaoCargaManual(IContextoCargaManual contexto, IAcessoDados acessoDados)
            : base(contexto, acessoDados)
        { }

        private static bool EstaTrocandoCpf(bool permiteTrocaCpf, string cpfOrigem, string cpf)
        {
            // Verifica se a cara que está sendo realizada para o cartão é uma troca de CPF Temporário.
            return permiteTrocaCpf // Cliente tem que poder trocar CPF
                && cpf != cpfOrigem // CPF e CPF Origem devem ser diferentes
                && (cpfOrigem != null && cpfOrigem.Trim().EndsWith("FC")); // CPF Origem deve ser temporário.
        }

        private string CriaArquivoCarga(IEnumerable<IDadosCargaCartao> cartoes)
        {
            var permiteTrocaCpf = AcessoDados.ClientePodeTrocarCpf(Contexto.CodigoCliente);
            var nomeArquivo = CriaNomeArquivo();
            var diretorio = Path.GetDirectoryName(nomeArquivo);

            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }

            using (var writer = new StreamWriter(nomeArquivo, false, Encoding.ASCII))
            {
                writer.WriteLine(string.Format("NOME;CPF;VALOR;MATRICULA;FILIAL;SETOR;CCUSTO{0}", permiteTrocaCpf ? ";CPF_TEMPORARIO" : ""));

                foreach (var cartao in cartoes)
                {
                    writer.WriteLine(string.Concat(
                        string.IsNullOrEmpty(cartao.Nome) ? "" : cartao.Nome.Trim(), ";",
                        string.IsNullOrEmpty(cartao.Cpf) ? "" : cartao.Cpf.Trim(), ";",
                        cartao.Valor, ";",
                        string.IsNullOrEmpty(cartao.Matricula) ? "" : cartao.Matricula.Trim(), ";",
                        cartao.Filial <= 0 ? "" : cartao.Filial.ToString(), ";",
                        string.IsNullOrEmpty(cartao.Setor) ? "" : cartao.Setor.Trim(), ";",
                        string.IsNullOrEmpty(cartao.CentroCusto) ? "" : cartao.CentroCusto.Trim(), ";",
                        EstaTrocandoCpf(permiteTrocaCpf, cartao.CpfOrigem, cartao.Cpf)
                            ? string.IsNullOrEmpty(cartao.CpfOrigem) ? "" : cartao.CpfOrigem.Trim() + ";" : ""
                    ));
                }

                writer.Flush();
            }

            return nomeArquivo;
        }

        private string CriaNomeArquivo()
        {
            var codigoCliente = Contexto.CodigoCliente.ToString();

            if (codigoCliente.Length < 5)
            {
                codigoCliente = codigoCliente.PadLeft(5, '0');
            }

            return Path.Combine(Path.Combine(Contexto.PastaArquivosImportacao, Contexto.NomeOperadora),
                string.Concat(codigoCliente, "_", Contexto.CnpjCliente, "_", DateTime.Now.ToString("ddMMyyyy"), ".csv"))
                .Replace(" ", "_");
        }

        private void PreparaCarga(DateTime dataProgramacao, IEnumerable<IDadosCargaCartao> cartoes, out string idProcesso, out int erro, out string mensagem)
        {
            erro = 0;
            mensagem = string.Empty;
            idProcesso = string.Empty;

            if (cartoes.Count() > Contexto.MaximoCartoesPorSolicitacao)
                throw new ArgumentOutOfRangeException("MAXIMO_CARTOES_CARGA_EXCEDIDO");

            try
            {
                idProcesso = AcessoDados.InserirProcesso(Contexto.Login, Contexto.CodigoOperadora);
            }
            catch (SqlException)
            {
                throw new ApplicationException("254");
            }

            var nomeArquivoCarga = CriaArquivoCarga(cartoes);

            if (!File.Exists(nomeArquivoCarga))
                throw new FileNotFoundException("ARQUIVO_CARGA_MANUAL_NAO_ENCONTRADO");

            AcessoDados.ValidarArquivo(dataProgramacao.Date == DateTime.Now.Date ? DateTime.MinValue : dataProgramacao.Date,
                idProcesso,
                Contexto.Login,
                Contexto.CodigoOperadora,
                Contexto.IdOperador,
                Contexto.CodigoCliente,
                Contexto.SistemaOrigem,
                nomeArquivoCarga, Path.GetFileName(nomeArquivoCarga),
                false,
                out erro,
                out mensagem);
        }

        public void FinalizarCarga()
        {
            var dadosCarga = ObterDadosCarga();

            if (dadosCarga.EtapaCarga != EtapaCarga.SemCargaEmExecucao)
            {
                AcessoDados.EncerrarStatus(Contexto.CodigoOperadora, dadosCarga.Erro, dadosCarga.IdProcesso);
            }
        }

        public IDadosCarga ObterDadosCarga()
        {
            return AcessoDados.ObterDadosCarga(Contexto.Login, Contexto.CodigoOperadora);
        }

        public IEnumerable<IResumoCarga> ObterResumoCarga()
        {
            var status = AcessoDados.ObterStatusCarga(Contexto.Login, Contexto.CodigoOperadora);
            var resumo = AcessoDados.ObterResumoCarga(status.IdProcesso);
            return resumo;
        }

        public void SolicitarCarga(DateTime dataProgramacao, IEnumerable<IDadosCargaCartao> cartoes, out int erro, out string mensagem)
        {
            var idProcesso = string.Empty;
            erro = 0;
            mensagem = string.Empty;

            PreparaCarga(dataProgramacao, cartoes, out idProcesso, out erro, out mensagem);

            if (erro == 0)
            {
                AcessoDados.SolicitarCarga(idProcesso, out erro, out mensagem);
            }

            var resumoCarga = ObterResumoCarga();

            if (erro <= 100)
            {
                // Existem registros de log do tipo "L" na tabela CARGA_CTRL_TABS_RESUMO. Estes registros devem compor um arquivo de log a 
                // ser baixado pelo usuário.
                mensagem = string.Empty;
                new ObterLogCargaCartao(
                        Contexto.PastaArquivosImportacao,
                        Contexto.NomeOperadora,
                        ObterDadosCarga().NomeArquivoCarga,
                        resumoCarga
                    ).SalvarLog();
            }
            else 
            { 
                // Não existem dados para formação de um arquivo de log. Recuperamos os registros do tipo "E" e montamos uma mensagem de erro.
                FinalizarCarga();

                var erros = string.Join(
                    "\n", 
                    resumoCarga
                        .Where(r => r.TipoRegistro == 'E')
                        .Select(r => r.RegistroLog)
                        .ToArray());
            }
        }
    }
}

#pragma warning restore 1591
