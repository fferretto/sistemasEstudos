using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Bld.PGTO.Santander.Abstraction.Interface;
using PagNet.Bld.PGTO.Santander.Web.Setup.Models;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.Bld.PGTO.Santander.Web.Setup.Controllers
{
    public class PagamentoController : ServiceController<IPagamentoApp>
    {
        public PagamentoController(IPagamentoApp service)
            : base(service)
        { }
        [HttpPost]
        [Authorize]
        [Route("GeraArquivoRemessa")]
        public IActionResult GeraArquivoRemessa([FromBody] FiltroTransmissaoBancoVM model)
        {
            if (model == null) return BadRequest();

            return OkResult(Service.GeraArquivoRemessa(model));

        }

    }
}
