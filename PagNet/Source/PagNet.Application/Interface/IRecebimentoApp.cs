using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface
{
    public interface IRecebimentoApp
    {

        Task<EmissaoBoletoVM> DadosInicioEmissaoBoletoAvulso(int codEmissaoBoleto, int codEmpresa);
        Task<IDictionary<bool, string>> ValidapedidosFaturamento(List<SolicitacaoBoletoVM> listaPedidos, int CodUsuario);
        Task<IDictionary<bool, string>> IncluiNovoPedidoFaturamento(EmissaoBoletoVM bol);
        Task<IDictionary<bool, string>> CancelarFatura(EmissaoBoletoVM bol);
        Task<IDictionary<bool, string>> LiquidacaoManual(EmissaoBoletoVM bol);
        Task<IDictionary<bool, string>> SalvarParcelamentoFatura(ParcelamentoFaturaVm model);
        Task<IDictionary<string, string>> CancelaArquivoRemessaByID(int codCarquivo, int codUsuario);

        Task<List<ConstulaEmissao_BoletoVM>> BustaTodosBoletosParaEdicao(int codigoEmpresa);
        Task<List<ConstulaLogFaturaVM>> VisualizarLog(int CodFatura);

        Task<DadosBoletoVM> ConsultaSolicitacoesBoletos(FiltroConsultaFaturamentoVM model);
        Task<List<SolicitacaoBoletoVM>> CarregaListaBoletos(FiltroConsultaFaturamentoVM model);

        FiltroConsultaFaturamentoVM RetornaDadosInicio(string dados, int codEmpresa);


        Task<List<ConsultaClienteVM>> ConsultaClientes();
        
        Task<List<ConsultaArquivosRemessaVM>> CarregaGridArquivosRemessa(FiltroDownloadArquivoVm model);
        Task<List<ListaClienteUsuarioVm>> ConsolidaArquivoRetornoCliente(string CaminhoArquivo, string nmOperadora, int codFatura);
        Task<IDictionary<bool, string>> ValidaFaturamentoViaArquivo(List<ListaClienteUsuarioVm> model, int cod_usu, int cod_empresa);

        Task<List<ListaBoletosVM>> ConsultaBoletosGerados(FiltroConsultaFaturamentoVM model);
        void GeraBoletoPDF(string CaminhoPadrao, int codEmissaoBoleto, int codigoUsuario);
        Task<string> RetornaNomeBoletoByID(int codFaturamento);

        Task<string> SalvaBordero(DadosBoletoVM model);

        object[][] GetInstrucaoCobranca();
        object[][] CarregaListaFaturas(int codigoCliente);

        //Métodos da tela de consulta borderô
        FiltroConsultaFaturamentoVM RetornaDadosInicioConstulaBordero(string dados, int codSubRede);
        BorderoBolVM ConsultaBordero(FiltroConsultaFaturamentoVM model);
        Task<List<ConstulaEmissao_BoletoVM>> GetAllBoletosByBordero(int CodBordero);
        Task<List<ConstulaEmissao_BoletoVM>> GetAllBoletosByCodArquivo(int codArquivo, int codOpe);
        void CancelaBordero(int codBordero, int codigoUsuario);

        Task<List<SolicitacaoBoletoVM>> BoletosRegistradosNaoLiquidados(FiltroConsultaFaturamentoVM model);

        //Métodos da tela de Inclusão de Arquivo Remessa
        //Task<string> GeraArquivoRemessaAsync(BorderoBolVM model);
        //Task<List<SolicitacaoBoletoVM>> CarregaDadosArquivoRetorno(string CaminhoArquivo, int CodOpe, int CodUsuario);

        void AtualizaSolicitacaoCarga();

        Task<List<ConstulaEmissao_BoletoVM>> ListaFaturamentoNaoLiquidado(int codEmpresa);
        Task<DetalhamentoFaturaReembolsoVm> RetornaDadosDetalhamentoCobranca(int codEmissaoFaturamento);

    }
}
