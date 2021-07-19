using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Infra.Data.Context;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Configuracao.Controllers
{
    [Area("Configuracao")]
    [Authorize]
    public class TaxasController : ClientSessionControllerBase
    {
        private readonly IConfiguracaoApp _config;
        private readonly IPagNetUser _user;

        public TaxasController(IConfiguracaoApp config,
                               IPagNetUser user)
        {
            _config = config;
            _user = user;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}