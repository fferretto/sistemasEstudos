using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Bld.PGTO.CEF.Abstraction.Interface;
using PagNet.Bld.PGTO.CEF.Web.Setup.Models;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.Bld.PGTO.CEF.Web.Setup.Controllers
{
    public class TransmissaoArquivoCEFController : ServiceController<IPagamentoApp>
    {
        public TransmissaoArquivoCEFController(IPagamentoApp service)
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
