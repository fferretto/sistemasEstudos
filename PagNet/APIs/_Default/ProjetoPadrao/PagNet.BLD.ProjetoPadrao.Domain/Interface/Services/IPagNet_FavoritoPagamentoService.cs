using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_FavoritoPagamentoService : IServiceBase<PAGNET_CADFAVORECIDO>
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
