#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Telenet.Carga.Abstractions;
using Telenet.Core.Data;

namespace Telenet.Carga
{
    public class SolicitacaoCargaArquivo : SolicitacaoCargaBase<IContextoCarga>, ISolicitacaoCargaArquivo
    {
        public SolicitacaoCargaArquivo(IContextoCarga contexto, IAcessoDados acessoDados)
            : base(contexto, acessoDados)
        { }

        private void GeraLog(string idProcesso, string nomeArquivoCarga, int nivel, EtapaCarga etapaCarga)
        {
            if (nivel == 100
                && (etapaCarga == EtapaCarga.ValidacaoLayoutErro
                || etapaCarga == EtapaCarga.VerificacaoDadosErro
                || etapaCarga == EtapaCarga.SolicitacaoCargaErro
                || etapaCarga == EtapaCarga.SolicitacaoCargaOk))
            {
                new ObterLogCargaCartao(
                        Contexto.PastaArquivosImportacao,
                        Contexto.NomeOperadora,
                        nomeArquivoCarga,
                        AcessoDados.ObterResumoCarga(idProcesso)
                    ).SalvarLog();
            }
        }

        private static void ValidaExisteCarga(EtapaCarga etapaCarga)
        {
            if (etapaCarga == EtapaCarga.SemCargaEmExecucao)
            {
                throw new ApplicationException("NAO_EXISTE_CARGA_EM_ANDAMENTO");
            }
        }

        private static void ValidaEtapaCorretaOperacao(EtapaCarga etapaCarga, EtapaCarga etapaOperacao)
        {
            if (etapaCarga != etapaOperacao)
            {
                throw new ApplicationException("STATUS_NAO_PERMITE_OPERACAO");
            }
        }

        public void CancelarCarga()
        {
            var statusCarga = ObterStatusCarga();

            ValidaExisteCarga(statusCarga.EtapaCarga);

            if (statusCarga.Erro > 100)
            {
                AcessoDados.EncerrarStatus(Contexto.CodigoOperadora, statusCarga.Erro, statusCarga.IdProcesso);
            }
            else
            {
                int erro;
                string mensagem;
                AcessoDados.CancelarSolicitacao(Contexto.CodigoOperadora, statusCarga.EtapaCarga, statusCarga.IdProcesso, out erro, out mensagem);

                if (erro > 0)
                {
                    throw new ApplicationException(mensagem);
                }
            }
        }

        public void FinalizarCarga()
        {
            var statusCarga = ObterStatusCarga();

            ValidaExisteCarga(statusCarga.EtapaCarga);
            ValidaEtapaCorretaOperacao(statusCarga.EtapaCarga, EtapaCarga.SolicitacaoCargaOk);

            var dadosCarga = ObterDadosCarga();

            if (!dadosCarga.BaixouLog)
            {
                AcessoDados.EncerrarStatus(Contexto.CodigoOperadora, statusCarga.Erro, dadosCarga.IdProcesso);
            }
        }

        public IDadosCarga ObterDadosCarga()
        {
            var statusCarga = ObterStatusCarga();
            ValidaExisteCarga(statusCarga.EtapaCarga);

            return AcessoDados.ObterDadosCarga(Contexto.Login, Contexto.CodigoOperadora);
        }

        public IEnumerable<IResumoCarga> ObterResumoCarga()
        {
            var statusCarga = ObterStatusCarga();
            ValidaExisteCarga(statusCarga.EtapaCarga);

            return AcessoDados.ObterResumoCarga(statusCarga.IdProcesso);
        }

        public IStatusCarga ObterStatusCarga()
        {
            var statusCarga = AcessoDados.ObterStatusCarga(Contexto.Login, Contexto.CodigoOperadora);

            if (string.IsNullOrEmpty(statusCarga.IdProcesso))
            {
                return new StatusCarga();
            }

            var dadosCarga = AcessoDados.ObterDadosCarga(Contexto.Login, Contexto.CodigoOperadora);

            if (statusCarga.Nivel == 100 && statusCarga.Erro > 0)
            {
                GeraLog(statusCarga.IdProcesso, dadosCarga.NomeArquivoCarga, statusCarga.Nivel, statusCarga.EtapaCarga);
                return statusCarga;
            }

            //statusCarga = AcessoDados.ObterStatusSemaforo(Contexto.Login, Contexto.CodigoOperadora);
            statusCarga = AcessoDados.ObterStatusCarga(Contexto.Login, Contexto.CodigoOperadora);
            GeraLog(statusCarga.IdProcesso, dadosCarga.NomeArquivoCarga, statusCarga.Nivel, statusCarga.EtapaCarga);
            return statusCarga;
        }

        public void SolicitarCarga()
        {
            var statusCarga = ObterStatusCarga();

            ValidaExisteCarga(statusCarga.EtapaCarga);
            ValidaEtapaCorretaOperacao(statusCarga.EtapaCarga, EtapaCarga.VerificacaoDadosOk);
            AcessoDados.ExecutaJobCargaSolicita(Contexto.CodigoOperadora, statusCarga.IdProcesso, Contexto.Login);
        }

        public void ValidarArquivo(DateTime dataProgramacao, string nomeCompletoArquivoCarga, string nomeOriginalArquivo, bool validarCpf)
        {
            var idProcesso = string.Empty;

            try
            {
                idProcesso = AcessoDados.InserirProcesso(Contexto.Login, Contexto.CodigoOperadora);
            }
            catch (SqlException)
            {
                throw new ApplicationException("254");
            }

            if (!File.Exists(nomeCompletoArquivoCarga))
                throw new FileNotFoundException("ARQUIVO_CARGA_NAO_ENCONTRADO");

            AcessoDados.ExecutaJobCargaValidaArquivo(dataProgramacao, idProcesso, Contexto.IpMaquinaSolicitante, Contexto.Login, Contexto.CodigoOperadora, Contexto.IdOperador, Contexto.CodigoCliente, validarCpf,
                Contexto.SistemaOrigem, nomeCompletoArquivoCarga, nomeOriginalArquivo);
        }
    }
}

#pragma warning restore 1591

