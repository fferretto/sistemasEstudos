using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_CADFAVORECIDOService : IServiceBase<PAGNET_CADFAVORECIDO>
    {
        List<PAGNET_CADFAVORECIDO> BuscaAllFavorecidosPagamento(int codigoEmpresa);
        List<PAGNET_CADFAVORECIDO> BuscaAllFavorecidosFornecedor(int codigoEmpresa);
        List<PAGNET_CADFAVORECIDO> BuscaAllByCentralizadora();
        List<PAGNET_CADFAVORECIDO> BuscaFavorecidosByBanco(string codigoBanco);
        PAGNET_CADFAVORECIDO BuscaFavorecidosByCNPJ(string CNPJ);
        PAGNET_CADFAVORECIDO BuscaFavorecidosByID(int codigoFavorecido);
        PAGNET_CADFAVORECIDO BuscaFavorecidosByCodCen(int codigocentralizadora);

        void IncluiFavorito(PAGNET_CADFAVORECIDO dados);
        void AtualizaFavorito(PAGNET_CADFAVORECIDO dados);

        void AtualizaFavoritoConfig(PAGNET_CADFAVORECIDO_CONFIG dados);
        void InseriFavoritoConfig(PAGNET_CADFAVORECIDO_CONFIG dados);
        void DeletaFavoritoConfig(int codigoFavorecido, int codigoEmpresa);

        PAGNET_CADFAVORECIDO_CONFIG BuscaConfigFavorecido(int codigoFavorecido, int codigoEmpresa);

        List<PAGNET_CADFAVORECIDO_LOG> ConsultaLog(int codigoFavorecido);
        void InsertLog(PAGNET_CADFAVORECIDO favorecido, int codigoUsuario, string Justificativa);

    }
}
