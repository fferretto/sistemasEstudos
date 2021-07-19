#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telenet.Carga.Abstractions;
using Telenet.Carga.Properties;
using Telenet.Core.Data;

namespace Telenet.Carga
{
    internal static class AcessoDadosExtension
    {
        private const string FalhaInternaJob = "Falha inesperada na execução da solicitação de carga. O processo foi finalizado. [{0}]";
        private const int CodigoFalhaInternaJob = 666666;

        private static string ConfiguraCampoConsultaCartao(string campo)
        {
            return campo == "NOMECLIENTE"
                ? "NOMUSU"
                : campo == "NUMEROCARTAO"
                    ? "CODCRT"
                    : campo == "CPFUSUARIO"
                        ? "CPF"
                        : campo;
        }

        private static bool EStatusSolicitacao(this EtapaCarga etapaCarga)
        {
            return etapaCarga == EtapaCarga.CargaFinalizada
                || etapaCarga == EtapaCarga.SolicitacaoCargaErro
                || etapaCarga == EtapaCarga.SolicitacaoCargaOk;
        }

        private static bool EStatusValidacao(this EtapaCarga etapaCarga)
        {
            return etapaCarga == EtapaCarga.ValidacaoLayoutErro
                || etapaCarga == EtapaCarga.ValidacaoLayoutOk
                || etapaCarga == EtapaCarga.VerificacaoDadosErro
                || etapaCarga == EtapaCarga.VerificacaoDadosOk;
        }

        private static string DescricaoJob(string idProcesso, string login, int codigoOperadora)
        {
            return string.Format(@"Job de solicitação de carga de cartões

Processo: {0}
Login: {1}
Operadora: {2}", idProcesso, login, codigoOperadora);
        }

        private static string NomeJobSolicitaCarga(int codigoOperadora, string idProcesso)
        {
            return string.Format("CARGA_SOLICITA - {0} - {1}", codigoOperadora, idProcesso);
        }

        private static string NomeJobValidaArquivo(int codigoOperadora, string idProcesso)
        {
            return string.Format("CARGA_VALIDA_ARQUIVO - {0} - {1}", codigoOperadora, idProcesso);
        }

        public static void CancelarSolicitacao(this IAcessoDados acessoDados, int codigoOperadora, EtapaCarga etapaAtual, string idProcesso, out int erro, out string mensagem)
        {
            var retorno = acessoDados.NetCard
                .StoredProcedure("CARGA_CANCELA_SOLICITACAO @ID_PROCESSO")
                .GetData<RetornoProcCarga>(idProcesso)
                .FirstOrDefault();

            erro = retorno.Erro;
            mensagem = retorno.Mensagem;
        }

        public static void EncerrarStatus(this IAcessoDados acessoDados, int codigoOperadora, int erro, string idProcesso)
        {
            acessoDados.NetCard
                .StoredProcedure("CARGA_ENCERRA_STATUS @ID_PROCESSO")
                .Execute(idProcesso);
        }

        public static void ExecutaJobCargaSolicita(this IAcessoDados acessoDados, int codigoOperadora, string idProcesso, string login)
        {
            idProcesso = idProcesso.ToUpper();

            acessoDados.NetCard
                .Command("UPDATE CARGA_STATUS SET ERRO = 0, MSG_ERRO = NULL, ETAPA_PROC = 4, NIVEL = 0 WHERE ID_PROCESSO = @ID_PROCESSO")
                .Execute(idProcesso);

            var script = Resources.ScriptSolicitaCarga
                .Replace("_ID_PROCESSO_", idProcesso)
                .Replace("_NOME_BANCO_", acessoDados.ObterNomeBancoNetCard(codigoOperadora));

            var job = acessoDados.NetCard
                .Job(NomeJobSolicitaCarga(codigoOperadora, idProcesso), DescricaoJob(idProcesso, login, codigoOperadora))
                .Create(script)
                .Start();
        }

        public static void ExecutaJobCargaValidaArquivo(this IAcessoDados acessoDados, DateTime dataProgramacao, string idProcesso, string ipCliente, string login,
            int codigoOperadora, int idOperador, int codigoCliente, bool validarCpf, string sistemaOrigem, string nomeCompletoArquivoCarga, string nomeOriginalArquivo)
        {
            idProcesso = idProcesso.ToUpper();

            acessoDados.NetCard
                .Command("UPDATE CARGA_STATUS SET ERRO = 0, MSG_ERRO = NULL, ETAPA_PROC = 0, NIVEL = 0 WHERE ID_PROCESSO = @ID_PROCESSO")
                .Execute(idProcesso);

            var script = Resources.ScriptValidaArquivo
                .Replace("_NOME_BANCO_", acessoDados.ObterNomeBancoNetCard(codigoOperadora))
                .Replace("_ID_PROCESSO_", idProcesso)
                .Replace("_DIRETORIO_", Path.GetDirectoryName(nomeCompletoArquivoCarga))
                .Replace("_NOME_ARQUIVO_", Path.GetFileName(nomeCompletoArquivoCarga))
                .Replace("_CLIENTE_WEB_", codigoCliente == 0 ? "null" : codigoCliente.ToString())
                .Replace("_VALIDA_CPF_", validarCpf ? "S" : "N")
                .Replace("_NOME_USUARIO_", login)
                .Replace("_CODIGO_OPERADORA_", codigoOperadora.ToString())
                .Replace("_NOME_ORIGINAL_ARQUIVO_", nomeOriginalArquivo)
                .Replace("_ORIGEM_", sistemaOrigem)
                .Replace("_DT_PROG_", dataProgramacao.Date == DateTime.Now.Date ? "" : dataProgramacao.ToString("yyyyMMdd"))
                .Replace("_ID_OPERADOR_", idOperador.ToString())
                .Replace("_IP_MAQUINA_", ipCliente)
                .Replace("_TIPO_CARGA_", "0");

            var job = acessoDados.NetCard
                .Job(NomeJobValidaArquivo(codigoOperadora, idProcesso), DescricaoJob(idProcesso, login, codigoOperadora))
                .Create(script)
                .Start();
        }

        public static string InserirProcesso(this IAcessoDados acessoDados, string login, int codigoOperadora)
        {
            const string EsteProcessoFoiIniciadoPorOutroIdLogin = "ESTE_PROCESSO_FOI_INICIADO_POR_OUTRO_ID_LOGIN";
            const string JaExisteProcessoParaEsteIdLogin = "JA_EXISTE_PROCESSO_PARA_ESTE_ID_LOGIN";

            var resultado = acessoDados.NetCard
                .StoredProcedure("CARGA_INSERE_STATUS @LOGINOPE, @CODOPE, @ID_LOGIN")
                .GetData<NovaCarga>(login, codigoOperadora, Guid.NewGuid().ToString())
                .FirstOrDefault();

            if (resultado.Mensagem == EsteProcessoFoiIniciadoPorOutroIdLogin
                || resultado.Mensagem == JaExisteProcessoParaEsteIdLogin)
            {
                throw new ApplicationException(resultado.Mensagem);
            }

            return resultado.Id;
        }

        public static IDadosCarga ObterDadosCarga(this IAcessoDados acessoDados, string login, int codigoOperadora)
        {
            return acessoDados.NetCard
                .Query("CARGA_RECUPERA_DADOS @LOGINOPE, @CODOPE")
                .GetData<DadosCarga>(login, codigoOperadora)
                .FirstOrDefault();
        }

        public static IEnumerable<IResumoCarga> ObterResumoCarga(this IAcessoDados acessoDados, string idProcesso)
        {
            return acessoDados.NetCard
                .Query("SELECT ID, CODCLI, NUM_CARGA, REGISTRO_LOG, TIPO_REG FROM CARGA_CTRL_TABS_RESUMO WHERE ID_PROCESSO = @ID_PROCESSO ORDER BY ID_LOG")
                .GetData<ResumoCarga>(idProcesso)
                .Cast<IResumoCarga>();
        }

        private static IEnumerable<JobHistory> ObterJobCarga(this IAcessoDados acessoDados, int codigoOperadora, EtapaCarga etapaCarga, string idProcesso)
        {
            var nomeJob = string.Empty;

            switch (etapaCarga)
            {
                case EtapaCarga.ValidacaoLayoutErro:
                case EtapaCarga.ValidacaoLayoutOk:
                case EtapaCarga.VerificacaoDadosErro:
                case EtapaCarga.VerificacaoDadosOk:
                    nomeJob = NomeJobValidaArquivo(codigoOperadora, idProcesso);
                    break;
                case EtapaCarga.SolicitacaoCargaErro:
                case EtapaCarga.SolicitacaoCargaOk:
                    nomeJob = NomeJobSolicitaCarga(codigoOperadora, idProcesso);
                    break;
                case EtapaCarga.CargaFinalizada:
                case EtapaCarga.SemCargaEmExecucao:
                default:
                    break;
            }

            return acessoDados.NetCard
                .Job(nomeJob)
                .History();
        }

        //public static IStatusCarga ObterStatusCarga(this IAcessoDados acessoDados, string login, int codigoOperadora)
        //{
        //    var statusCarga = acessoDados.NetCard
        //        .Query("SELECT ERRO, NIVEL, MSG_ERRO, ETAPA_PROC, ID_PROCESSO FROM CARGA_STATUS WHERE LOGINOPE = @LOGINOPE AND CODOPE = @CODOPE")
        //        .GetData<StatusCarga>(login, codigoOperadora)
        //        .FirstOrDefault() ?? new StatusCarga();

        //    var history = acessoDados.ObterJobCarga(codigoOperadora, statusCarga.EtapaCarga, statusCarga.IdProcesso);

        //    if (history.Count() > 0 && history.Any(h => h.StepId == 1 && h.Status == JobStatus.Failed))
        //    {
        //        return new StatusCarga(CodigoFalhaInternaJob, 
        //            string.Format(FalhaInternaJob, statusCarga.IdProcesso), 
        //            statusCarga.EtapaCarga, 
        //            statusCarga.IdProcesso, 
        //            100);
        //    }

        //    return statusCarga;
        //}

        //public static IStatusCarga ObterStatusSemaforo(this IAcessoDados acessoDados, string login, int codigoOperadora)
        public static IStatusCarga ObterStatusCarga(this IAcessoDados acessoDados, string login, int codigoOperadora)
        {
            var comando = acessoDados.NetCard
                .StoredProcedure("CARGA_CONSULTA_SEMAFORO @LOGINOPE, @CODOPE, @ID_PROCESSO VARCHAR(36) OUTPUT, @ETAPA_PROC INT OUTPUT, @NIVEL INT OUTPUT, @ERRO INT OUTPUT, @MSG_ERRO VARCHAR(255) OUTPUT");

            comando.Execute(login, codigoOperadora);
            var outputs = comando.GetOutputs();
            var etapaCarga = outputs.Get<EtapaCarga>("@ETAPA_PROC");
            var idProcesso = outputs.Get<string>("@ID_PROCESSO");

            var nomeJob = etapaCarga.EStatusSolicitacao()
                ? NomeJobSolicitaCarga(codigoOperadora, idProcesso)
                : etapaCarga.EStatusValidacao()
                    ? NomeJobValidaArquivo(codigoOperadora, idProcesso)
                    : "";

            var statusJob = acessoDados.NetCard.Job(nomeJob).Status();

            if (!string.IsNullOrEmpty(nomeJob) && statusJob == JobStatus.Running)
            {
                return new StatusCarga(0, outputs.Get<string>("@MSG_ERRO"), etapaCarga, idProcesso, outputs.Get<int>("@NIVEL") - 1);
            }

            if (statusJob == JobStatus.Failed)
            {
                var outValues = outputs.Get();
                var history = acessoDados.NetCard.Job(nomeJob).History().First();
                outValues["@ERRO"] = history.MessageId;
                outValues["@MSG_ERRO"] = string.Format("Erro inesperado no processo de execução da carga {0}. Código do Erro: {1}.",
                    idProcesso, history.MessageId);

                return new StatusCarga(new OutputValues(outValues));
            }

            return new StatusCarga(outputs);
        }

        public static IEnumerable<ICargaSolicitada> ObterCargasSolicitadas(this IAcessoDados acessoDados, int codigoCliente)
        {
            return acessoDados.NetCard
                .StoredProcedure("CARGA_AGUARDANDO_LIBERACAO @CODCLI")
                .GetData<CargaSolicitada>(codigoCliente)
                .Cast<ICargaSolicitada>();
        }

        public static ICartaoUsuario ObterCartaoUsuario(this IAcessoDados acessoDados, int codigoCliente, string campo, string valor)
        {
            valor = valor.Trim();

            campo = ConfiguraCampoConsultaCartao(campo);

            if (campo == "NOMUSU")
            {
                campo += " LIKE";
                valor += "%";
            }
            else
            {
                campo += " =";
            }

            var cartao = acessoDados.NetCard
                .Query(string.Format("SELECT NOMUSU, CPF, MAT, CODFIL, CODSET, CODCRT FROM VUSUARIOVA WHERE CODCLI = @p1 AND {0} @p2", campo))
                .GetData<CartaoUsuario>(codigoCliente, valor)
                .FirstOrDefault();

            return cartao;
        }

        public static bool ClientePodeTrocarCpf(this IAcessoDados acessoDados, int codigoCliente)
        {
            var comando = acessoDados.NetCard.StoredProcedure("CARGA_CLIENTE_PODE_TROCAR_CPF @SISTEMA, @CODCLI, @CLIENTE_PODE_TROCAR_CPF CHAR(1) OUTPUT");
            comando.Execute(1, codigoCliente);

            var outputs = comando.GetOutputs();
            return outputs.Get<string>("@CLIENTE_PODE_TROCAR_CPF") == "S";
        }

        public static bool CpfValidoParaTrocar(this IAcessoDados acessoDados, int codigoCliente, string cpf, out int erro, out string mensagem)
        {
            erro = 0;
            mensagem = string.Empty;
            var comando = acessoDados.NetCard.StoredProcedure("CARGA_VALIDA_CPF_PARA_TROCA @SISTEMA, @CODCLI, @CPF, @CPF_VALIDO_PARA_TROCA CHAR(1) OUTPUT, @ERRO INT OUTPUT, @MSG_ERRO VARCHAR(512) OUTPUT");
            comando.Execute(1, codigoCliente, cpf);

            var outputs = comando.GetOutputs();

            erro = outputs.Get<int>("@ERRO");
            if (erro != 0)
                mensagem = outputs.Get<string>("@MSG_ERRO");

            return outputs.Get<string>("@CPF_VALIDO_PARA_TROCA") == "S";
        }

        public static bool CpfTemporarioValidoParaTrocar(this IAcessoDados acessoDados, int codigoCliente, string cpf, out int erro, out string mensagem)
        {
            erro = 0;
            mensagem = string.Empty;
            var comando = acessoDados.NetCard.StoredProcedure("CARGA_VALIDA_CPF_TEMPORARIO_PARA_TROCA @SISTEMA, @CODCLI, @CPF_TEMPORARIO, @CPF_TEMP_VALIDO_PARA_TROCA CHAR(1) OUTPUT, @ERRO INT OUTPUT, @MSG_ERRO VARCHAR(512) OUTPUT");
            comando.Execute(1, codigoCliente, cpf);

            var outputs = comando.GetOutputs();

            erro = outputs.Get<int>("@ERRO");
            if (erro != 0)
                mensagem = outputs.Get<string>("@MSG_ERRO");

            return outputs.Get<string>("@CPF_TEMP_VALIDO_PARA_TROCA") == "S";
        }

        public static IEnumerable<string> LookupPara(this IAcessoDados acessoDados, int codigoCliente, string campo, string valor)
        {
            valor = valor.Trim();
            campo = ConfiguraCampoConsultaCartao(campo);

            if (!string.IsNullOrEmpty(valor))
            {
                valor += "%";
            }

            return acessoDados.NetCard
                .Query(string.Format("SELECT TOP 100 NOMUSU, CPF, MAT, CODFIL, CODSET, CODCRT FROM VUSUARIOVA WHERE CODCLI = @p1 AND {0} LIKE @p2", campo))
                .GetData<CartaoUsuario>(codigoCliente, valor).Select(c => campo == "NOMUSU"
                    ? c.Nome
                    : campo == "CPF"
                        ? c.Cpf
                        : c.Numero);
        }

        public static string ObterNomeBancoNetCard(this IAcessoDados acessoDados, int codigoOperadora)
        {
            return acessoDados.Concentrador
                .Query("SELECT BD_NC FROM OPERADORA WHERE CODOPE = @CODOPE")
                .GetScalar<string>(codigoOperadora);
        }

        public static ICommandAsyncResult ObterResultadoAsync(this IAcessoDados acessoDados, string idProcesso)
        {
            return acessoDados.NetCard
                .GetAsyncResult(idProcesso);
        }

        public static void SolicitarCarga(this IAcessoDados acessoDados, string idProcesso, out int erro, out string mensagem)
        {
            erro = 0;
            mensagem = null;

            var comando = acessoDados.NetCard
                .StoredProcedure("CARGA_SOLICITA @ID_PROCESSO, @ERRO INT OUTPUT, @MSG_ERRO VARCHAR(512) OUTPUT");

            comando.Execute(idProcesso);

            var outputs = comando.GetOutputs();

            erro = outputs.Get<int>("@ERRO");
            mensagem = outputs.Get<string>("@MSG_ERRO");
        }

        public static void ValidarArquivo(this IAcessoDados acessoDados, DateTime dataProgramacao, string idProcesso, string login, int codigoOperadora,
            int idOperador, int codigoCliente, string sistemaOrigem, string nomeCompletoArquivoCarga, string nomeOriginalArquivo, bool validaCpf,
            out int erro, out string mensagem)
        {
            var nomeArquivoCarga = Path.GetFileName(nomeCompletoArquivoCarga);

            erro = 0;
            mensagem = null;

            var comando = acessoDados.NetCard
                .StoredProcedure(@"CARGA_VALIDA_ARQUIVO @DIRETORIO, @ARQUIVO, @CLIENTE_WEB, @VALIDA_CPF, @LOGINOPE, @CODOPE, @ORIGEM, @NOME_ARQUIVO, @ID_PROCESSO, 
@TIPO_CARGA, @DT_PROG, @ID_OPERADOR, @ERRO INT OUTPUT, @NUM_CARGA INT OUTPUT, @VALOR_DA_CARGA NUMERIC(15,2) OUTPUT, @CNPJ CHAR(14) OUTPUT, @NOME_TABLE VARCHAR(128) OUTPUT,
@CODCLI INT OUTPUT, @VALIDA_CPF_OUT CHAR(1) OUTPUT, @MSG_ERRO VARCHAR(512) OUTPUT");

            comando.Execute(Path.GetDirectoryName(nomeCompletoArquivoCarga),
                nomeArquivoCarga,
                codigoCliente == 0 ? DBNull.Value : (object)codigoCliente,
                validaCpf ? 'S' : 'N',
                login,
                codigoOperadora,
                sistemaOrigem,
                nomeOriginalArquivo,
                idProcesso,
                1,
                dataProgramacao == DateTime.MinValue ? "" : dataProgramacao.ToString("yyyyMMdd"),
                idOperador);

            var outputs = comando.GetOutputs();

            erro = outputs.Get<int>("@ERRO");
            mensagem = outputs.Get<string>("@MSG_ERRO");
        }
    }
}

#pragma warning restore 1591
