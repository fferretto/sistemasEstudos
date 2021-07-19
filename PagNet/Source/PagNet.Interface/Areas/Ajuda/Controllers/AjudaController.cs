using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Infra.Data.Context;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Ajuda.Controllers
{
    [Area("Ajuda")]
    [Authorize]
    public class AjudaController : ClientSessionControllerBase
    {
        private readonly IPagNetUser _user;

        public AjudaController(IPagNetUser user)
        {
            _user = user;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}