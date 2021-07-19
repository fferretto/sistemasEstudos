using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_EMISSAOBOLETOService : IServiceBase<PAGNET_EMISSAOBOLETO>
    {
        PAGNET_EMISSAOBOLETO BuscaBoletoByID(int id);
        List<PAGNET_EMISSAOBOLETO> BuscaTodosBoletoByCodArquivo(int CodArquivo);
        List<PAGNET_EMISSAOBOLETO> BustaTodosBoletosParaEdicao(int codEmpresa);
        List<PAGNET_EMISSAOBOLETO> BustaTodosBoletosVencidos();
        PAGNET_EMISSAOBOLETO BuscaBoletoBySeuNumero(string seuNumero);
        PAGNET_EMISSAOBOLETO BuscaBoletoByNossoNumero(string NossoNumero);
        List<PAGNET_EMISSAOBOLETO> BuscaBoletoByCodCliente(int CodCliente);
        
        void IncluiBoleto(PAGNET_EMISSAOBOLETO bol);
        void AtualizaBoleto(PAGNET_EMISSAOBOLETO bol);

        PAGNET_EMISSAOFATURAMENTO BuscaFaturamentoByID(int id);
        List<PAGNET_EMISSAOFATURAMENTO> BuscaFaturamentosByData(DateTime dtRealPGTO, int codContaCorrente);
        List<PAGNET_EMISSAOFATURAMENTO> BuscaFaturamentoByIDPai(int codPai, int codFilho);
        List<PAGNET_EMISSAOFATURAMENTO> BuscaTodosFaturamentoByCodBordero(int CodBordero);
        List<PAGNET_EMISSAOFATURAMENTO> BustaTodosFaturamentosParaEdicao(int codEmpresa);
        List<PAGNET_EMISSAOFATURAMENTO> BuscaFaturamentoBySeuNumero(string seuNumero);
        PAGNET_EMISSAOFATURAMENTO BuscaFaturamentoByNroDocumento(string nroDocumento);        
        List<PAGNET_EMISSAOFATURAMENTO> BuscaFaturamentoByCodCliente(int CodCliente);
        List<PAGNET_EMISSAOFATURAMENTO> GetAllFaturas(string status, int codEmpresa, int codCli, DateTime dtInicio, DateTime dtFim);
        List<PAGNET_EMISSAOFATURAMENTO> BuscaTransacaoAConsolidar(int codigoEmpresa, int codContaCorrente, int mes, int ano);
        List<PAGNET_EMISSAOFATURAMENTO> BuscaFaturamentosNaoConciliados(int codigoEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim);

        void IncluiFaturamento(PAGNET_EMISSAOFATURAMENTO bol);
        void AtualizaFaturamento(PAGNET_EMISSAOFATURAMENTO bol);
        void AtualizaPlanoContasFaturamentosNaoLiquidados(int codigoEmpresa, int codigoPlanoContas, int codigousuario);

        void IncluiLog(PAGNET_EMISSAOFATURAMENTO faturamento, int codUsuario, string Justificativa);
        void IncluiLogByBordero(int codBordero, string status, int codUsuario, string Justificativa);
        void IncluiLogBySeuNumero(string seuNumero, string status, int codUsuario, string Justificativa);
        List<PAGNET_EMISSAOFATURAMENTO_LOG> BuscaLog(int codEmissaoBoleto);

        List<PAGNET_EMISSAOBOLETO> GetAllBoletosNaoVencidos(int codEmpresa, int codCli);
        List<PAGNET_EMISSAOBOLETO> GetAllBoletos(string status, int codEmpresa, int codCli, DateTime dtInicio, DateTime dtFim);
        List<PAGNET_EMISSAOBOLETO> GetBoletosRegistradosNaoLiquidados(int codEmpresa, int CodContaCorrente, int codCli, DateTime dtInicio, DateTime dtFim);

        void AtualizaStatusBycodBordero(int codBordero, string NovoStatus);
        void AtualizaEmissaoBoletoVencido();
        int BuscaNovoIDEmissaoBoleto();
        int BuscaNovoIDEmissaoFaturamento();

        List<PAGNET_EMISSAOFATURAMENTO> ListaFaturamentoNaoLiquidado(int codEmpresa);
        int RetornaquantidadeParcelasFuturas(int codigo);

        List<PAGNET_EMISSAOFATURAMENTO> BuscaFaturas(int codigoCliente, DateTime dataVencimento);
        object[][] CarregaListaFaturas(int codigoCliente);
        void CancelaFaturaHomologacao(int codigoContaCorrente, int codigoUsuario, string Justificativa);
        PAGNET_EMISSAOFATURAMENTO RetornaFaturaHomologacao(int codigoContaCorrente);
    }
}
