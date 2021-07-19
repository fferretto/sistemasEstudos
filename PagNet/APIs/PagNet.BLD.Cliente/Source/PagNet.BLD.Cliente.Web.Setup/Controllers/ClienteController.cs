using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.BLD.Cliente.Abstraction.Interface;
using PagNet.BLD.Cliente.Web.Setup.Models;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.BLD.Cliente.Web.Setup.Controllers
{
    public class ClienteController : ServiceController<IClienteApp>
    {
        public ClienteController(IClienteApp service)
            : base(service)
        { }

        [HttpPost]
        [Authorize]
        [Route("RetornaDadosClienteByID")]
        public IActionResult RetornaDadosClienteByID([FromBody] FiltroCliente model)
        {
            if (model == null) return BadRequest();

            return OkResult(Service.RetornaDadosClienteByID(model));

        }
        [HttpPost]
        [Authorize]
        [Route("RetornaDadosClienteByCPFCNPJ")]
        public IActionResult RetornaDadosClienteByCPFCNPJ([FromBody] FiltroCliente model)
        {
            if (model == null) return BadRequest();

            return OkResult(Service.RetornaDadosClienteByCPFCNPJ(model));

        }
        [HttpPost]
        [Authorize]
        [Route("RetornaDadosClienteByIDeCodEmpresa")]
        public IActionResult RetornaDadosClienteByIDeCodEmpresa([FromBody] FiltroCliente model)
        {
            if (model == null) return BadRequest();

            return OkResult(Service.RetornaDadosClienteByIDeCodEmpresa(model));

        }
        [HttpPost]
        [Authorize]
        [Route("RetornaDadosClienteByCPFCNPJeCodEmpresa")]
        public IActionResult RetornaDadosClienteByCPFCNPJeCodEmpresa([FromBody] FiltroCliente model)
        {
            if (model == null) return BadRequest();

            return OkResult(Service.RetornaDadosClienteByCPFCNPJeCodEmpresa(model));

        }
        [HttpPost]
        [Authorize]
        [Route("ConsultaTodosCliente")]
        public IActionResult ConsultaTodosCliente([FromBody] FiltroCliente model)
        {
            if (model == null) return BadRequest();

            return OkResult(Service.ConsultaTodosCliente(model));

        }
        [HttpPost]
        [Authorize]
        [Route("SalvarCliente")]
        public IActionResult SalvarCliente([FromBody] ClienteVm model)
        {
            if (model == null) return BadRequest();

            Service.SalvarCliente(model);
            return OkResult(true);
        }
        [HttpPost]
        [Authorize]
        [Route("DesativaCliente")]
        public IActionResult DesativaCliente([FromBody] FiltroCliente model)
        {
            if (model == null) return BadRequest();

            Service.DesativaCliente(model);
            return OkResult(true);
        }

    }
}
