using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_EmissaoBoletoService : IServiceBase<PAGNET_EMISSAOBOLETO>
    {
        Task<PAGNET_EMISSAOBOLETO> BuscaBoletoByID(int id);
        Task<List<PAGNET_EMISSAOBOLETO>> BuscaTodosBoletoByCodArquivo(int CodArquivo);
        Task<List<PAGNET_EMISSAOBOLETO>> BustaTodosBoletosParaEdicao(int codEmpresa);
        Task<List<PAGNET_EMISSAOBOLETO>> BustaTodosBoletosVencidos();
        Task<PAGNET_EMISSAOBOLETO> BuscaBoletoBySeuNumero(string seuNumero);
        Task<PAGNET_EMISSAOBOLETO> BuscaBoletoByNossoNumero(string NossoNumero);
        Task<List<PAGNET_EMISSAOBOLETO>> BuscaBoletoByCodCliente(int CodCliente);
        
        void IncluiBoleto(PAGNET_EMISSAOBOLETO bol);
        void AtualizaBoleto(PAGNET_EMISSAOBOLETO bol);

        Task<PAGNET_EMISSAOFATURAMENTO> BuscaFaturamentoByID(int id);
        Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaFaturamentosByData(DateTime dtRealPGTO, int codContaCorrente);
        Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaFaturamentoByIDPai(int codPai, int codFilho);
        Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaTodosFaturamentoByCodBordero(int CodBordero);
        Task<List<PAGNET_EMISSAOFATURAMENTO>> BustaTodosFaturamentosParaEdicao(int codEmpresa);
        Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaFaturamentoBySeuNumero(string seuNumero);
        Task<PAGNET_EMISSAOFATURAMENTO> BuscaFaturamentoByNroDocumento(string nroDocumento);        
        Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaFaturamentoByCodCliente(int CodCliente);
        Task<List<PAGNET_EMISSAOFATURAMENTO>> GetAllFaturas(string status, int codEmpresa, int codCli, DateTime dtInicio, DateTime dtFim);
        Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaTransacaoAConsolidar(int codigoEmpresa, int codContaCorrente, int mes, int ano);
        Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaFaturamentosNaoConciliados(int codigoEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim);

        void IncluiFaturamento(PAGNET_EMISSAOFATURAMENTO bol);
        void AtualizaFaturamento(PAGNET_EMISSAOFATURAMENTO bol);
        void AtualizaPlanoContasFaturamentosNaoLiquidados(int codigoEmpresa, int codigoPlanoContas, int codigousuario);

        void IncluiLog(PAGNET_EMISSAOFATURAMENTO faturamento, int codUsuario, string Justificativa);
        void IncluiLogByBordero(int codBordero, string status, int codUsuario, string Justificativa);
        void IncluiLogBySeuNumero(string seuNumero, string status, int codUsuario, string Justificativa);
        Task<List<PAGNET_EMISSAOFATURAMENTO_LOG>> BuscaLog(int codEmissaoBoleto);

        Task<List<PAGNET_EMISSAOBOLETO>> GetAllBoletosNaoVencidos(int codEmpresa, int codCli);
        Task<List<PAGNET_EMISSAOBOLETO>> GetAllBoletos(string status, int codEmpresa, int codCli, DateTime dtInicio, DateTime dtFim);
        Task<List<PAGNET_EMISSAOBOLETO>> GetBoletosRegistradosNaoLiquidados(int codEmpresa, int CodContaCorrente, int codCli, DateTime dtInicio, DateTime dtFim);

        void AtualizaStatusBycodBordero(int codBordero, string NovoStatus);
        void AtualizaEmissaoBoletoVencido();
        int BuscaNovoIDEmissaoBoleto();
        int BuscaNovoIDEmissaoFaturamento();

        Task<List<PAGNET_EMISSAOFATURAMENTO>> ListaFaturamentoNaoLiquidado(int codEmpresa);
        Task<int> RetornaquantidadeParcelasFuturas(int codigo);


        object[][] CarregaListaFaturas(int codigoCliente);
    }
}
