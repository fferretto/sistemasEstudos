using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Configuracao.Controllers
{
    [Area("Configuracao")]
    [Authorize]
    public class PerfisAcessoController : ClientSessionControllerBase
    {
        private readonly IConfiguracaoApp _config;
        private readonly IPagNetUser _user;

        public PerfisAcessoController(IConfiguracaoApp config,
                                      IPagNetUser user)
        {
            _config = config;
            _user = user;
        }
        public async Task<IActionResult> Index()
        {
            PerfilAcessoVM model = new PerfilAcessoVM();


            return View(model);
        }

        public async Task<ActionResult> AbrirJanelaModal()
        {
            PerfilAcessoVM dados = new PerfilAcessoVM();

            return PartialView(dados);

        }
    }
}