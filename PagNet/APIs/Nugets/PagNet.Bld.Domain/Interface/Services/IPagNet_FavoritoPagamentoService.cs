using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_FAVORITOPAGAMENTOService : IServiceBase<PAGNET_CADFAVORECIDO>
    {
        Task<List<PAGNET_CADFAVORECIDO>> BuscaAllFavorecidos(int CodEmpresa);
        Task<List<PAGNET_CADFAVORECIDO>> BuscaAllCentralizadora(int CodEmpresa);
        Task<List<PAGNET_CADFAVORECIDO>> BuscaFavorecidosByBanco(int CodEmpresa, string CodBanco);
        Task<PAGNET_CADFAVORECIDO> BuscaFavorecidosByCNPJ(int CodEmpresa, string CNPJ);
        Task<PAGNET_CADFAVORECIDO> BuscaFavorecidosByCodCen(int CodEmpresa, int CodCen);
        Task<PAGNET_CADFAVORECIDO> BuscaFavorecidosByID(int CodFavorito);

        void IncluiFavorito(PAGNET_CADFAVORECIDO dados);
        void AtualizaFavorito(PAGNET_CADFAVORECIDO dados);

    }
}
