
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PagNet.Bld.AntecipPGTO.Abstraction.Interface;
using PagNet.Bld.AntecipPGTO.Web.Setup.Models;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.Bld.AntecipPGTO.Web.Setup.Controllers
{
    public class AntecipacaoPGTOController : ServiceController<IAntecipacaoApp>
    {
        public AntecipacaoPGTOController(IAntecipacaoApp service)
            : base(service)
        { }
        [HttpPost]
        [Authorize]
        [Route("ListaTitulosParaAntecipacao")]
        public IActionResult ListaTitulosParaAntecipacao([FromBody] FiltroAntecipacaoModel model)
        {
            if (model == null) return BadRequest();
            return OkResult(Service.ListaTitulosValidosAntecipacao(model));
        }
        [HttpPost]
        [Authorize]
        [Route("CalculaTaxaAntecipacaoPGTO")]
        public IActionResult CalculaTaxaAntecipacaoPGTO([FromBody] FiltroAntecipacaoModel model)
        {
            if (model == null) return BadRequest();
            return OkResult(Service.CalculaTaxaAntecipacaoPGTO(model));
        }
        [HttpPost]
        [Authorize]
        [Route("SalvarAntecipacaoPGTO")]
        public IActionResult SalvarAntecipacaoPGTO([FromBody] FiltroAntecipacaoModel model)
        {
            if (model == null) return BadRequest();
            var Sucesso = Service.SalvarAntecipacaoPGTO(model);

            if (!Sucesso)
                return Conflict("Não foi possível salvar a solicitação. Favor contactar o suporte.");

            return OkResult(Sucesso);
        }
        

    }
}
