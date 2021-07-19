using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Interface;
using PagNet.Bld.PGTO.CobrancaBancaria.Web.Setup.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.Bld.PGTO.CobrancaBancaria.Web.Setup.Controllers
{
    public class CobrancaBancariaController : ServiceController<ICobrancaBancariaApp>
    {
        public CobrancaBancariaController(ICobrancaBancariaApp service)
            : base(service)
        { }
        [HttpPost]
        [Authorize]
        [Route("TransmissaoArquivoRemessa")]
        public IActionResult TransmissaoArquivoRemessa([FromBody] FiltroEmissaoBoletoModel filtro)
        {
            if (filtro == null) return BadRequest();
            return OkResult(Service.GeraArquivoRemessa(filtro));
        }
        [HttpPost]
        [Authorize]
        [Route("CarregaDadosArquivoRetorno")]
        public IActionResult CarregaDadosArquivoRetorno([FromBody] FiltroCobranca filtro)
        {
            if (filtro == null) return BadRequest();

            return OkResult(Service.CarregaDadosArquivoRetorno(filtro));

        }
        [HttpPost]
        [Authorize]
        [Route("GeraBoletoPDF")]
        public IActionResult GeraBoletoPDF([FromBody] FiltroCobranca filtro)
        {
            if (filtro == null) return BadRequest();
            Service.GeraBoletoPDF(filtro);
            return OkResult(true);

        }

        
    }
}
