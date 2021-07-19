using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_TITULOS_PAGOSService : IServiceBase<PAGNET_TITULOS_PAGOS>
    {
        Task<PAGNET_TITULOS_PAGOS> GetTransacaoById(int id);
        Task<PAGNET_TITULOS_PAGOS> GetTransacaoBySeuNumero(string seuNumero);
        PAGNET_TITULOS_PAGOS IncluiTransacao(PAGNET_TITULOS_PAGOS pag);
        int RetornaProximoNumeroTitulosPagos();
        Task<bool> AtualizaTransacao(PAGNET_TITULOS_PAGOS pag);
        Task<bool> AtualizaStatus(string status, int CodTransacao);

        List<PAGNET_TITULOS_PAGOS> getTransacaoPagamento(DateTime dtInicio, DateTime dtFim, int codCred, int codSubRede);
        List<PAGNET_TITULOS_PAGOS> getTransacaoPagamentoByBordero(int codBordero);
        List<PAGNET_TITULOS_PAGOS> GetPagamentosAtivosByCodArquivo(int codCarquivo);
        List<PAGNET_TITULOS_PAGOS> GetPagamentosByCodArquivo(int codCarquivo);
        Task<List<PAGNET_TITULOS_PAGOS>> BuscaTitulosByData(DateTime dtRealPGTO, int codContaCorrente);

    }
}
