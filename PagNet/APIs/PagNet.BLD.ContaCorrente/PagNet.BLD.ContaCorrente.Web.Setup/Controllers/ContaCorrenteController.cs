using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.BLD.ContaCorrente.Abstraction.Interface;
using PagNet.BLD.ContaCorrente.Web.Setup.Models;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.BLD.ContaCorrente.Web.Setup.Controllers
{
    public class ContaCorrenteController : ServiceController<IContaCorrenteApp>
    {
        public ContaCorrenteController(IContaCorrenteApp service)
            : base(service)
        { }
        [HttpPost]
        [Authorize]
        [Route("DDLBuscaContaCorrente")]
        public IActionResult DDLBuscaContaCorrente([FromBody] int codigoEmpresa)
        {
            if (codigoEmpresa == 0) return BadRequest();
            return OkResult(Service.GetContaCorrente(codigoEmpresa));
        }
        [HttpPost]
        [Authorize]
        [Route("DDLBuscaContaCorrentePagamento")]
        public IActionResult DDLBuscaContaCorrentePagamento([FromBody] int codigoEmpresa)
        {
            if (codigoEmpresa == 0) return BadRequest();
            return OkResult(Service.GetContaCorrentePagamento(codigoEmpresa));
        }
        [HttpPost]
        [Authorize]
        [Route("DDLBuscaContaCorrenteBoleto")]
        public IActionResult DDLBuscaContaCorrenteBoleto([FromBody] int codigoEmpresa)
        {
            if (codigoEmpresa == 0) return BadRequest();
            return OkResult(Service.GetContaCorrenteBoleto(codigoEmpresa));
        }
        [HttpPost]
        [Authorize]
        [Route("DDLBuscaBanco")]
        public IActionResult DDLBuscaBanco()
        {
            return OkResult(Service.GetBanco());
        }
        [HttpPost]
        [Authorize]
        [Route("BuscaBancoByContaCorrente")]
        public IActionResult BuscaBancoByContaCorrente([FromBody] int codigoContaCorrente)
        {
            if (codigoContaCorrente == 0) return BadRequest();
            return OkResult(Service.GetBancoByCodContaCorrente(codigoContaCorrente));
        }
        [HttpPost]
        [Authorize]
        [Route("BuscaBancoByID")]
        public IActionResult BuscaBancoByID([FromBody] string codigoBanco)
        {
            if (string.IsNullOrWhiteSpace(codigoBanco)) return BadRequest();

            return OkResult(Service.BuscaBancoByID(codigoBanco));
        }
        [HttpPost]
        [Authorize]
        [Route("ExisteArquivoRemessaBoletoCriado")]
        public IActionResult ExisteArquivoRemessaBoletoCriado([FromBody] int codigoContaCorrente)
        {
            if (codigoContaCorrente == 0) return BadRequest();

            return OkResult(Service.ExisteArquivoRemessaBoletoCriado(codigoContaCorrente));
        }
        [HttpPost]
        [Authorize]
        [Route("GeraArquivoRemessaHomologacao")]
        public IActionResult GeraArquivoRemessaHomologacao([FromBody] APIDadosHomologarContaCorrenteVm filtro)
        {
            if (filtro == null) return BadRequest();

            return OkResult(Service.GeraArquivoRemessaHomologacao(filtro));
        }
        [HttpPost]
        [Authorize]
        [Route("GeraBoletoPDFHomologacao")]
        public IActionResult GeraBoletoPDFHomologacao([FromBody] APIDadosHomologarContaCorrenteVm filtro)
        {
            if (filtro == null) return BadRequest();

            return OkResult(Service.GeraBoletoPDFHomologacao(filtro));
        }
        [HttpPost]
        [Authorize]
        [Route("BuscaTodasContaCorrente")]
        public IActionResult BuscaTodasContaCorrente([FromBody] int codigoEmpresa)
        {
            if (codigoEmpresa == 0) return BadRequest();

            return OkResult(Service.GetAllContaCorrente(codigoEmpresa));
        }
        [HttpPost]
        [Authorize]
        [Route("BuscaContaCorrenteByID")]
        public IActionResult BuscaContaCorrenteByID([FromBody] APIFiltro filtro)
        {
            return OkResult(Service.GetContaCorrenteById(filtro.codigoContaCorrente, filtro.codigoEmpresa));
        }
        [HttpPost]
        [Authorize]
        [Route("SalvarContaCorrente")]
        public IActionResult SalvarContaCorrente([FromBody] APIContaCorrenteVm filtro)
        {
            if (filtro == null) return BadRequest();

            return OkResult(Service.Salvar(filtro));
        }
        [HttpPost]
        [Authorize]
        [Route("DesativarContaCorrente")]
        public IActionResult DesativarContaCorrente([FromBody] int codigoContaCorrente)
        {
            if (codigoContaCorrente == 0) return BadRequest();
            Service.Desativar(codigoContaCorrente);
            return OkNoResult();
        }
    }
}