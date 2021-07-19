using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Bld.PGTO.Safra.Abstraction.Interface;
using PagNet.Bld.PGTO.Safra.Web.Setup.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.Bld.PGTO.Safra.Web.Setup.Controllers
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
