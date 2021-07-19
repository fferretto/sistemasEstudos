using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.BLD.Relatorio.Abstraction.Interface;
using PagNet.BLD.Relatorio.Web.Setup.Models;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.BLD.Relatorio.Web.Setup.Controllers
{
    public class RelatorioController : ServiceController<IRelatorioApp>
    {
        public RelatorioController(IRelatorioApp service)
            : base(service)
        { }

        [HttpPost]
        [Authorize]
        [Route("BuscaParametrosRelatorio")]
        public IActionResult BuscaParametrosRelatorio([FromBody] int codRel)
        {
            if (codRel == 0) return BadRequest();
            
            return OkResult(Service.BuscaParametrosRelatorio(codRel));
        }
        [HttpPost]
        [Authorize]
        [Route("RelatorioPDF")]
        public IActionResult RelatorioPDF([FromBody] ParametrosRel model)
        {
            if (model == null) return BadRequest();

            return OkResult(Service.RelatorioPDF(model));
        }
        [HttpPost]
        [Authorize]
        [Route("ExportaExcel")]
        public IActionResult ExportaExcel([FromBody] ParametrosRel model)
        {
            if (model == null) return BadRequest();

            return OkResult(Service.ExportaExcel(model));
        }
        [HttpPost]
        [Authorize]
        [Route("VerificaTerminoGeracaoRelatorio")]
        public IActionResult VerificaTerminoGeracaoRelatorio([FromBody] int codigoRelatorio)
        {
            if (codigoRelatorio == 0) return BadRequest();

            return OkResult(Service.VerificaTerminoGeracaoRelatorio(codigoRelatorio));
        }
        [HttpPost]
        [Authorize]
        [Route("GeraRelatorioViaJob")]
        public IActionResult GeraRelatorioViaJob([FromBody] ParametrosRel model)
        {
            if (model == null) return BadRequest();

            return OkResult(Service.GeraRelatorioViaJob(model));
        }
        [HttpPost]
        [Authorize]
        [Route("RetornoRelatornoViaJob")]
        public IActionResult RetornoRelatornoViaJob([FromBody] ParametrosRel model)
        {
            if (model == null) return BadRequest();

            return OkResult(Service.RetornoRelatornoViaJob(model));
        }

    }
}
