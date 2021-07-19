using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Bld.Cobranca.Bordero.Abstraction.Interface;
using PagNet.Bld.Cobranca.Bordero.Web.Setup.Models;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.Bld.Cobranca.Bordero.Web.Setup.Controllers
{
    public class BorderoController : ServiceController<IApplication>
    {
        public BorderoController(IApplication service)
            : base(service)
        { }
        [HttpPost]
        [Authorize]
        [Route("BuscaFaturas")]
        public IActionResult BuscaFaturas([FromBody] FiltroBorderoModel filtro)
        {
            if (filtro == null) return BadRequest();
            return OkResult(Service.BuscaFaturas(filtro));
        }

        [HttpPost]
        [Authorize]
        [Route("ListaBorderos")]
        public IActionResult ListaBorderos([FromBody] FiltroBorderoModel filtro)
        {
            if (filtro == null) return BadRequest();
            return OkResult(Service.ListaBorderos(filtro));
        }

        [HttpPost]
        [Authorize]
        [Route("Salvar")]
        public IActionResult Salvar([FromBody] DadosBoletoModel filtro)
        {
            if (filtro == null) return BadRequest();
            return OkResult(Service.Salvar(filtro));
        }

        [HttpPost]
        [Authorize]
        [Route("Cancelar")]
        public IActionResult Cancelar([FromBody] int codigoBordero)
        {
            if (codigoBordero == 0) return BadRequest();
            return OkResult(Service.Cancelar(codigoBordero));
        }
    }
}