using PagNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_Emissao_TitulosService : IServiceBase<PAGNET_EMISSAO_TITULOS>
    {
        void IncluiTitulo(PAGNET_EMISSAO_TITULOS Titulo);
        void IncluiLog(PAGNET_EMISSAO_TITULOS Titulo, int codUsuario, string Justificativa);

        void AtualizaTitulo(PAGNET_EMISSAO_TITULOS Titulo);
        void AtualizaStatusTituloBycodBordero(int codBordero, string NovoStatus);
        void IncluiLogByBordero(int codBordero, string status, int codUsuario, string Justificativa);
        void AtualizaPlanoContasTitulosNaoBaixados(int codEmpresa, int CodPlanoContas, int CodUsuario);

        Task<int> BuscaProximoCodTitulo();
        Task<PAGNET_EMISSAO_TITULOS> BuscaTituloByID(int CODTITULO);
        Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTituloByIDPai(int codTituloPai, int CodTitulo);
        Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTituloBySeuNumero(string SeuNumero);
        Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTransacaoAConsolidar(int codigoEmpresa, int codContaCorrente, int mes, int ano);
        Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTitulosNaoConciliados(int codigoEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim);
        Task<List<PAGNET_EMISSAO_TITULOS>> BustaTitulosByStatus(int codEmpresa, string status);
        Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTitulosByData(DateTime dtRealPGTO, int codContaCorrente);
        Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTitulosByBordero(int CodBordero);
        Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTitulosByFavorecidoDatPGTO(int codEmpresa, int codFavorecido, DateTime datPGTO);
        Task<PAGNET_EMISSAO_TITULOS> BuscaTitulosByLinhaDigitavel(string LinhaDigitavel);
        Task<List<PAGNET_EMISSAO_TITULOS>> ListaTitulosEmAberto(int codEmpresa, int codFavorecido, DateTime datInicio, DateTime datFim);
        Task<List<PAGNET_EMISSAO_TITULOS_LOG>> BuscaLog(int CODTITULO);
        Task<List<PAGNET_EMISSAO_TITULOS>> ListaTitulosNaoLiquidado(int codEmpresa);

        Task<int> RetornaquantidadeParcelasFuturas(int codigo);
        object[][] DDLTitulosAbertosByCodFavorecido(int CodigoFavorecido);

        void CancelaTituloHomologacao(int codigoContaCorrente, int codigoUsuario, string Justificativa);
        PAGNET_EMISSAO_TITULOS RetornaTituloHomologacao(int codigoContaCorrente);


    }
}
