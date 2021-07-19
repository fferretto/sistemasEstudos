using PagNet.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_CadFavorecidoService : IServiceBase<PAGNET_CADFAVORECIDO>
    {
        Task<List<PAGNET_CADFAVORECIDO>> BuscaAllFavorecidosPagamento(int codEmpresa);
        Task<List<PAGNET_CADFAVORECIDO>> BuscaAllFavorecidosFornecedor(int codEmpresa);
        Task<List<PAGNET_CADFAVORECIDO>> BuscaAllByCentralizadora();
        Task<List<PAGNET_CADFAVORECIDO>> BuscaFavorecidosByBanco(string CodBanco);
        Task<PAGNET_CADFAVORECIDO> BuscaFavorecidosByCNPJ(string CNPJ);
        Task<PAGNET_CADFAVORECIDO> BuscaFavorecidosByID(int CodFavorito);
        Task<PAGNET_CADFAVORECIDO> BuscaFavorecidosByCodCen(int CodCen);

        void IncluiFavorito(PAGNET_CADFAVORECIDO dados);
        void AtualizaFavorito(PAGNET_CADFAVORECIDO dados);

        Task<List<PAGNET_CADFAVORECIDO_LOG>> ConsultaLog(int CODFAVORECIDO);
        void InsertLog(PAGNET_CADFAVORECIDO favorito, int codUsuario, string Justificativa);

    }
}
