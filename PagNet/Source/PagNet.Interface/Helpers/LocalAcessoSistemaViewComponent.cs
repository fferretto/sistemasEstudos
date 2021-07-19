using Microsoft.AspNetCore.Mvc;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Models;
using System.Threading.Tasks;

namespace PagNet.Interface.Helpers
{
    [ViewComponent(Name= "LocalAcessoSistema")]
    public class LocalAcessoSistemaViewComponent : ViewComponent
    {
        IPagNetUser _user;

        public LocalAcessoSistemaViewComponent(IPagNetUser user)
        {
            _user = user;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            LocalAcessoSistemaModel model = new LocalAcessoSistemaModel();

            model.nomeBanco = _user.BdNetCard;
            model.nomeServidor = _user.ServidorDadosNetCard;
            model.baseHomologacao = (_user.ServidorDadosNetCard.ToUpper() != "ZEUS");
           
            return View("~/Views/Shared/LocalAcessoSistema.cshtml", model);
        }
    }
}
