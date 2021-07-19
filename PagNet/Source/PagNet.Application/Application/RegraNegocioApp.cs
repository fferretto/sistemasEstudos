using PagNet.Application.Interface;
using PagNet.Domain.Interface.Services;

namespace PagNet.Application.Application
{
    public class RegraNegocioApp : IRegraNegocioApp
    {
        private readonly IPagNet_FavoritoPagamentoService _favorecido;

        public RegraNegocioApp(IPagNet_FavoritoPagamentoService favorecido)
        {
            _favorecido = favorecido;
        }
    }
}
