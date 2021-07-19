using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Bld.PGTO.Brasil.Abstraction.Interface;
using PagNet.Bld.PGTO.Brasil.Web.Setup.Models;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.Bld.PGTO.Brasil.Web.Setup.Controllers
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
