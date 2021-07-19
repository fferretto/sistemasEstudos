using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Bld.PGTO.Favorecido.Abstraction.Interface;
using PagNet.Bld.PGTO.Favorecido.Web.Setup.Models;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.Bld.PGTO.Favorecido.Web.Setup.Controllers
{
    public class FavorecidoController : ServiceController<ICadastroApp>
    {
        public FavorecidoController(ICadastroApp service)
            : base(service)
        { }
        [HttpPost]
        [Authorize]
        [Route("RetornaDadosFavorecidoByID")]
        public IActionResult RetornaDadosFavorecidoByID([FromBody] FiltroPesquisaModel model)
        {
            if (model.codigoFavorecido == 0) return BadRequest();
            if (model.codigoEmpresa == 0) return BadRequest();

            return OkResult(Service.RetornaDadosFavorecidoByID(model.codigoFavorecido, model.codigoEmpresa));
        }
        [HttpPost]
        [Authorize]
        [Route("RetornaDadosFavorecidoByCodCen")]
        public IActionResult RetornaDadosFavorecidoByCodCen([FromBody] FiltroPesquisaModel model)
        {
            if (model.codigoFavorecido == 0) return BadRequest();
            if (model.codigoEmpresa == 0) return BadRequest();

            return OkResult(Service.RetornaDadosFavorecidoByCodCen(model.codigoFavorecido, model.codigoEmpresa));
        }
        [HttpPost]
        [Authorize]
        [Route("RetornaDadosFavorecidoByCPFCNPJ")]
        public IActionResult RetornaDadosFavorecidoByCPFCNPJ([FromBody] FiltroPesquisaModel model)
        {
            if (model.CPFCNPJ == "") return BadRequest();

            return OkResult(Service.RetornaDadosFavorecidoByCPFCNPJ(model.CPFCNPJ, model.codigoEmpresa));
        }
        [HttpPost]
        [Authorize]
        [Route("ConsultaTodosFavorecidosCentralizadora")]
        public IActionResult ConsultaTodosFavorecidosCentralizadora([FromBody] FiltroPesquisaModel model)
        {
            return OkResult(Service.ConsultaTodosFavorecidosCentralizadora(model.codigoEmpresa));
        }
        [HttpPost]
        [Authorize]
        [Route("ConsultaTodosFavorecidosFornecedores")]
        public IActionResult ConsultaTodosFavorecidosFornecedores([FromBody] FiltroPesquisaModel model)
        {
            if (model.codigoEmpresa == 0) return BadRequest();

            return OkResult(Service.ConsultaTodosFavorecidosFornecedores(model.codigoEmpresa));
        }
        [HttpPost]
        [Authorize]
        [Route("ConsultaTodosFavorecidosPAG")]
        public IActionResult ConsultaTodosFavorecidosPAG([FromBody] FiltroPesquisaModel model)
        {
            if (model.codigoEmpresa == 0) return BadRequest();

            return OkResult(Service.ConsultaTodosFavorecidosPAG(model.codigoEmpresa));
        }
        [HttpPost]
        [Authorize]
        [Route("SalvarFavorecido")]
        public IActionResult SalvarFavorecido([FromBody] FiltroFavorecidosModel filtro)
        {
            if (filtro == null) return BadRequest();

            return OkResult(Service.SalvarFavorecido(filtro));
        }
        [HttpPost]
        [Authorize]
        [Route("DesativaFavorecido")]
        public IActionResult DesativaFavorecido([FromBody] int codigoFavorecido)
        {
            if (codigoFavorecido == 0) return BadRequest();

            return OkResult(Service.DesativaFavorecido(codigoFavorecido));
        }
        [HttpPost]
        [Authorize]
        [Route("ConsultaLog")]
        public IActionResult ConsultarLog([FromBody] int codigoFavorecido)
        {
            if (codigoFavorecido == 0) return BadRequest();

            return OkResult(Service.ConsultaLog(codigoFavorecido));
        }
    }
}
