using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Procedures
{
    public interface IProc_PagNet_Cons_Tran_CreRepository : IRepositoryBase<PROC_PAGNET_CONS_TITULOS_BORDERO>
    {
        IEnumerable<PROC_PAGNET_CONS_TITULOS_BORDERO> ConsultaTitulosEmAbertos(DateTime dtInicio, DateTime dtFim, int codFavorito, int codEmpresa, int codContaCorrente, string codBanco);
    }
    public interface IPROC_PAGNET_CONS_TITULOS_VENCIDOSRepository : IRepositoryBase<PROC_PAGNET_CONS_TITULOS_VENCIDOS>
    {
        IEnumerable<PROC_PAGNET_CONS_TITULOS_VENCIDOS> ConsultaTitulosVencidos(int codEmpresa, DateTime dtInicio, DateTime dtFim);
    }
    public interface IProc_PagNet_Indicador_Pagamento_DiaRepository : IRepositoryBase<PROC_PAGNET_IND_PAG_PREVISTO_DIA>
    {
        IEnumerable<PROC_PAGNET_IND_PAG_PREVISTO_DIA> ConsultaValorPagoDia(DateTime dtInicio, DateTime dtFim, int codSubRede);
    }
    public interface IPROC_PAGNET_IND_PAG_ANORepository : IRepositoryBase<PROC_PAGNET_IND_PAG_ANO>
    {
        IEnumerable<PROC_PAGNET_IND_PAG_ANO> ConsultaPagRealizadoAno(int codSubRede, string codBanco, int codCre);
    }
    public interface IPROC_PAGNET_IND_PAG_REALIZADO_DIARepository : IRepositoryBase<PROC_PAGNET_IND_PAG_REALIZADO_DIA>
    {
        IEnumerable<PROC_PAGNET_IND_PAG_REALIZADO_DIA> ConsultaPagRealizadoDia(DateTime dtInicio, DateTime dtFim, int codSubRede, string codBanco, int codCre);
    }
    public interface IPROC_PAGNET_CONSULTA_TITULOSRepository : IRepositoryBase<PROC_PAGNET_CONSULTA_TITULOS>
    {
        Task<IEnumerable<PROC_PAGNET_CONSULTA_TITULOS>> ConsultaTitulos(DateTime dtInicio, DateTime dtFim, int codFavorecido, int codEmpresa, string status, int codigoTitulo);
    }
    public interface IPROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANORepository : IRepositoryBase<PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO>
    {
        IEnumerable<PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO> ConsultaIndicadorEntradaSaidaAno(int codSubRede);
    }

    public interface IPROC_PAGNET_INDI_RECEB_PREVISTA_DIARepository : IRepositoryBase<PROC_PAGNET_INDI_RECEB_PREVISTA_DIA>
    {
        IEnumerable<PROC_PAGNET_INDI_RECEB_PREVISTA_DIA> ConsultaIndicadorReceitaDia(DateTime dtInicio, DateTime dtFim, int codSubRede);
    }

    public interface IPROC_PAGNET_CONSCARGA_PGTORepository : IRepositoryBase<PROC_PAGNET_CONSCARGA_PGTO>
    {
        Task<IEnumerable<PROC_PAGNET_CONSCARGA_PGTO>> ConsultaCargaPendenteBoleto(int codCli, int codSubRede, DateTime dtInicio, DateTime dtFim, int numCarga);
    }
    public interface IPROC_PAGNET_CONS_BORDERORepository : IRepositoryBase<PROC_PAGNET_CONS_BORDERO>
    {
        Task<IEnumerable<PROC_PAGNET_CONS_BORDERO>> ConsultaBorderoPagamento(int codEmpresa, int codBordero, int codContaCorrente, string Status);
    }

    public interface IPROC_PAGNET_SOLICITACAOBOLETORepository : IRepositoryBase<PROC_PAGNET_SOLICITACAOBOLETO>
    {
        IEnumerable<PROC_PAGNET_SOLICITACAOBOLETO> ConsultaEmissaoBoleto(DateTime dtInicio, DateTime dtFim, int codCliente, int codEmpresa, out int ERRO, out string MSG_ERRO);
    }
    public interface IPROC_PAGNET_CONSULTABOLETORepository : IRepositoryBase<PROC_PAGNET_CONSULTABOLETO>
    {
        IEnumerable<PROC_PAGNET_CONSULTABOLETO> ConsultaBoletos(DateTime dtInicio, DateTime dtFim, int codCliente, int codEmpresa, string status, out int ERRO, out string MSG_ERRO);
    }
    public interface IPROC_PAGNET_DETALHAMENTO_COBRANCARepository : IRepositoryBase<PROC_PAGNET_DETALHAMENTO_COBRANCA>
    {
        IEnumerable<PROC_PAGNET_DETALHAMENTO_COBRANCA> DetalhamentoCobranca(int codEmissaoFaturamento);
    }

    public interface IPROC_PAGNET_REG_CARGA_CLI_NETCARDRepository : IRepositoryBase<PROC_PAGNET_REG_CARGA_CLI_NETCARD>
    {
        void AtualizaSolicitacaoCarga(DateTime datRef, out int ERRO, out string MSG_ERRO);
    }
    public interface IPROC_CONFIRMAPGTOCARGARepository : IRepositoryBase<PROC_CONFIRMAPGTOCARGA>
    {
        void RegistraPagamentoCargaNetCard(int codigoCliente, int NumeroCarga);
    }
    public interface IPROC_PAGNET_INC_FAVORECIDO_USUARIO_NCRepository : IRepositoryBase<PROC_PAGNET_INC_FAVORECIDO_USUARIO_NC>
    {
        void IncluiUsuarioNC(string CPF, int sistema, int codEmpresa);
    }    
    public interface IPROC_PAGNET_MAIORES_RECEITASRepository : IRepositoryBase<PROC_PAGNET_MAIORES_RECEITAS>
    {
        Task<IEnumerable<PROC_PAGNET_MAIORES_RECEITAS>> Listar_MAIORES_RECEITAS(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim);
    }
    public interface IPROC_PAGNET_MAIORES_DESPESASRepository : IRepositoryBase<PROC_PAGNET_MAIORES_DESPESAS>
    {
        Task<IEnumerable<PROC_PAGNET_MAIORES_DESPESAS>> Listar_MAIORES_DESPESAS(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim);
    }
    public interface IPROC_PAGNET_INFO_CONTA_CORRENTERepository : IRepositoryBase<PROC_PAGNET_INFO_CONTA_CORRENTE>
    {
        Task<PROC_PAGNET_INFO_CONTA_CORRENTE> Consultar_INFO_CONTA_CORRENTE(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim);
    }
    public interface IPROC_PAGNET_EXTRATO_BANCARIORepository : IRepositoryBase<PROC_PAGNET_EXTRATO_BANCARIO>
    {
        Task<IEnumerable<PROC_PAGNET_EXTRATO_BANCARIO>> Listar_EXTRATO_BANCARIO(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim);
    }
    public interface IPROC_PAGNET_BUSCA_USUARIO_NCRepository : IRepositoryBase<PROC_PAGNET_BUSCA_USUARIO_NC>
    {
        Task<PROC_PAGNET_BUSCA_USUARIO_NC> BuscaDadosUsuarioNC(string Matricula, int Sistema, int codCliente);
    }
    public interface IPROC_PAGNET_INC_CLIENTE_USUARIO_NCRepository : IRepositoryBase<PROC_PAGNET_INC_CLIENTE_USUARIO_NC>
    {
        Task<PROC_PAGNET_INC_CLIENTE_USUARIO_NC> IncluiClienteUsuarioNC(string CPF, int Sistema, int codEmpresa, int codUsuario);
    }
    
}
