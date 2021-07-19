using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCard.Bld.Relatorio.Abstraction.Interface;
using NetCard.Bld.Relatorio.Web.Setup.Models;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;


namespace NetCard.Bld.Relatorio.Web.Setup.Controllers
{
    public class RelatorioController : ServiceController<IRelatorioApp>
    {
        public RelatorioController(IRelatorioApp service)
            : base(service)
        { }

        [HttpPost]
        [Authorize]
        [Route("BuscaParametrosRelatorio")]
        public IActionResult BuscaParametrosRelatorio([FromBody] FiltroPesquisaRelModel filtro)
        {
            if (filtro == null) return BadRequest();

            return OkResult(Service.BuscaParametrosRelatorio(filtro));
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
        public IActionResult VerificaTerminoGeracaoRelatorio([FromBody] FiltroPesquisaRelModel filtro)
        {
            if (filtro == null) return BadRequest();

            return OkResult(Service.VerificaTerminoGeracaoRelatorio(filtro));
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
        [HttpPost]
        [Authorize]
        [Route("CarregaDDL")]
        public IActionResult CarregaDDL([FromBody] FiltroDDLRelModel model)
        {
            if (model == null) return BadRequest();

            return OkResult(Service.CarregaDDL(model));
        }
    }
}
