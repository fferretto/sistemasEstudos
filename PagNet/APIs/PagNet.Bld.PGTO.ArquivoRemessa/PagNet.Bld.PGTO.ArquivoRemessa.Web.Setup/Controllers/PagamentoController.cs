using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Bld.PGTO.ArquivoRemessa.Abrstraction.Interface;
using PagNet.Bld.PGTO.ArquivoRemessa.Abrstraction.Model;
using PagNet.Bld.PGTO.ArquivoRemessa.Web.Setup.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.Bld.PGTO.ArquivoRemessa.Web.Setup.Controllers
{
    public class PagamentoController : ServiceController<IArquivoRemessa>
    {
        public PagamentoController(IArquivoRemessa service)
            : base(service)
        { }
        [HttpPost]
        [Authorize]
        [Route("TransmiteArquivoBanco")]
        public IActionResult TransmiteArquivoBanco([FromBody] FiltroTransmissaoBancoVM model)
        {
            if (model == null) return BadRequest();

            return OkResult(Service.TransmiteArquivoBanco(model));

        }
        [HttpPost]
        [Authorize]
        [Route("ProcessaArquivoRetorno")]
        public IActionResult ProcessaArquivoRetorno([FromBody] APIFiltroRetorno api)
        {
            if (string.IsNullOrWhiteSpace(api.caminhoArquivo)) return BadRequest();

            return OkResult(Service.ProcessaArquivoRetorno(api.caminhoArquivo));

        }
        [HttpPost]
        [Authorize]
        [Route("CancelaArquivoRemessa")]
        public IActionResult CancelaArquivoRemessa([FromBody] int codigoArquivo)
        {
            if (codigoArquivo <= 0) return BadRequest();

            return OkResult(Service.CancelaArquivoRemessaByID(codigoArquivo));

        }

        
    }
}
