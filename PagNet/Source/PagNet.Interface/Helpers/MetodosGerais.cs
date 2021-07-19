using PagNet.Api.Service.Helpers;
using PagNet.Api.Service.Interface;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Interface.Helpers
{
    public class MetodosGerais
    {
        public MetodosGerais(IAPIFavorecido apiFavorecido,
                             IPagNetUser user
                            )
        {
            _apiFavorecido = apiFavorecido;
            _user = user;
        }
        private readonly IAPIFavorecido _apiFavorecido;
        private readonly IPagNetUser _user;

        public IDictionary<int, string> RetornaFavorecido(string filtro, int? codigoEmpresa)
        {
            Dictionary<int, string> valorRetorno = new Dictionary<int, string>();
            if (Convert.ToInt32(codigoEmpresa) <= 0)
            {
                codigoEmpresa = _user.cod_empresa;
            }
            APIFavorecidoVM model = new APIFavorecidoVM();

            filtro = Util.RemoveCaracteres(filtro);

            if (filtro.Length <= 10)
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByID(Convert.ToInt32(filtro), Convert.ToInt32(codigoEmpresa)));
            }
            else
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByCPFCNPJ(filtro, Convert.ToInt32(codigoEmpresa)));
            }
            valorRetorno.Add(model.codigoFavorecido, model.nomeFavorecido);

            return valorRetorno;
        }
    }
}
