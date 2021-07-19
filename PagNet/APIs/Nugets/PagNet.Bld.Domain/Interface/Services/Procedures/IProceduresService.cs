using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services.Procedures
{
    public interface IProceduresService : IServiceBase<PAGNET_TITULOS_PAGOS>
    {
        List<PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO> ConsultaIndicadorEntradaSaidaAno(int codSubRede);
        List<PROC_PAGNET_INDI_RECEB_PREVISTA_DIA> ConsultaIndicadorReceitaDia(DateTime dtInicio, DateTime dtFim, int codSubRede);
        List<PROC_PAGNET_IND_PAG_REALIZADO_DIA> ConsultaPagRealizadoDia(DateTime dtInicio, DateTime dtFim, int codSubRede, string codBanco, int codCre);
        List<PROC_PAGNET_IND_PAG_ANO> ConsultaPagRealizadoAno(int codSubRede, string codBanco, int codCre);
        List<PROC_PAGNET_CONS_TITULOS_VENCIDOS> RetornaTitulosVencidos(int codEmpresa,  DateTime dtInicio, DateTime dtFim);
        Task<MW_CONSCEP> ConsultaEndereco(string CEP);
        List<PROC_PAGNET_CONS_TITULOS_BORDERO> BuscaListaTitulosEmAberto(DateTime dtInicio, DateTime dtFim, int codFavorito,
                                                int codEmpresa,  int codContaCorrente, string codBanco);

        Task<List<PROC_PAGNET_CONSULTA_TITULOS>> ConsultaTitulos(DateTime dtInicio, DateTime dtFim, int codFavorecido, int codEmpresa, string status, int codigoTitulo);
        Task<IEnumerable<PROC_PAGNET_SOLICITACAOBOLETO>> ConsultaEmissaoBoleto(DateTime dtInicio, DateTime dtFim, int codCliente, int codEmpresa);
        Task<IEnumerable<PROC_PAGNET_CONSULTABOLETO>> ConsultaBoletos(DateTime dtInicio, DateTime dtFim, int codCliente, int codEmpresa, string status);
        Task<IEnumerable<PROC_PAGNET_DETALHAMENTO_COBRANCA>> DetalhamentoCobranca(int codEmissaoFaturamento);

        Task<IEnumerable<PROC_PAGNET_MAIORES_RECEITAS>> Listar_MAIORES_RECEITAS(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<PROC_PAGNET_MAIORES_DESPESAS>> Listar_MAIORES_DESPESAS(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim);
        Task<PROC_PAGNET_INFO_CONTA_CORRENTE> Consultar_INFO_CONTA_CORRENTE(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<PROC_PAGNET_EXTRATO_BANCARIO>> Listar_EXTRATO_BANCARIO(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim);
               
        void AtualizaSolicitacaoCarga(DateTime datRef);
        void RegistraPagamentoCargaNetCard(int codigoCliente, int NumeroCarga);
        void IncluiUsuarioNC(string CPF, int sistema, int codEmpresa);
        Task<PROC_PAGNET_BUSCA_USUARIO_NC> BuscaDadosUsuarioNC(string Matricula, int sistema, int codCliente);
        Task<PROC_PAGNET_INC_CLIENTE_USUARIO_NC> IncluiClienteUsuarioNC(string CPF, int Sistema, int codEmpresa, int codUsuario);
        PROC_BAIXA_LOTE_CPF ExecutarProcedure(int codigoCliente, int Lote, string CPF, string ResultSet);
        /// <summary>
        /// Processo para atualizar a tabela de gestão DF, atualizar valor da fatura e liberar saldo usuário. 
        /// A "ListaCPF" é utilizada como excessão. ou seja, irá atualizar todos os usuários com excessão destes cpfs.
        /// o formado dos cpfs deve ser da seguinte forma '12365478910','12345678985','12365874523'
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <param name="codigoFatura"></param>
        /// <param name="nroLote"></param>
        /// <param name="listaCPF"></param>
        /// <returns></returns>
        PROC_PAGNET_LIBERA_DESCONTOFOLHA ExecutarDescontoFolha(int codigoCliente, int codigoFatura, int nroLote, string listaCPF, int codUsuarioPN);
        List<PROC_PAGNET_CONS_FATURAS_BORDERO> ExecutarConsFaturaBordero(DateTime dtInicio, DateTime dtFim, int codCliente, int codEmpresa, int codContaCorrente);

    }
}