using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.BLD.DescontoFolha.Abstraction.Interface;
using PagNet.BLD.DescontoFolha.Web.Setup2.Models;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.BLD.DescontoFolha.Web.Setup2.Controllers
{
    public class DescontoFolhaController : ServiceController<IDescontoFolhaApp>
    {
        public DescontoFolhaController(IDescontoFolhaApp service)
            : base(service)
        { }

        [HttpPost]
        [Authorize]
        [Route("BuscaDadosUsuarioByCPF")]
        public IActionResult BuscaDadosUsuarioByCPF([FromBody] string CPF)
        {
            if (CPF == null) return BadRequest();

            return OkResult(Service.BuscaDadosUsuarioByCPF(CPF));
        }

        [HttpPost]
        [Authorize]
        [Route("BuscaDadosUsuarioByMatricula")]
        public IActionResult BuscaDadosUsuarioByMatricula([FromBody] string Matricula)
        {
            if (Matricula == null) return BadRequest();

            return OkResult(Service.BuscaDadosUsuarioByMatricula(Matricula));
        }
        [HttpPost]
        [Authorize]
        [Route("BuscaConfiguracaoByCliente")]
        public IActionResult BuscaConfiguracaoByCliente([FromBody]  FiltroDescontoFolhaVM filtro)
        {
            if (filtro == null) return BadRequest();
            //FiltroDescontoFolhaVM filtro = new FiltroDescontoFolhaVM();

            return OkResult(Service.BuscaConfiguracaoByCliente(filtro));
        }
        [HttpPost]
        [Authorize]
        [Route("SalvarParamLeituraArquivo")]
        public IActionResult SalvarParamLeituraArquivo([FromBody] ConfigParamLeituraArquivoVM model)
        {
            if (model == null) return BadRequest();

            Service.SalvarParamLeituraArquivo(model);

            return OkResult(true);
        }
        [HttpPost]
        [Authorize]
        [Route("ConsolidaArquivoRetornoCliente")]
        public IActionResult ConsolidaArquivoRetornoCliente([FromBody] FiltroDescontoFolhaVM filtro)
        {
            if (filtro == null) return BadRequest();
                       

            return OkResult(Service.ConsolidaArquivoRetornoCliente(filtro));
        }
        [HttpPost]
        [Authorize]
        [Route("ExecutaProcessoDescontoFolha")]
        public IActionResult ExecutaProcessoDescontoFolha([FromBody] UsuariosArquivoRetornoVm lista)
        {
            if (lista == null) return BadRequest();

            Service.ExecutaProcessoDescontoFolha(lista);

            return OkNoResult();
        }
        [HttpPost]
        [Authorize]
        [Route("CarregaListaFaturasAbertas")]
        public IActionResult CarregaListaFaturasAbertas([FromBody] int codigoCliente)
        {
            if (codigoCliente == 0) return BadRequest();


            return OkResult(Service.CarregaListaFaturasAbertas(codigoCliente));
        }
        [HttpPost]
        [Authorize]
        [Route("BuscaFechamentosNaoDescontados")]
        public IActionResult BuscaFechamentosNaoDescontados([FromBody] FiltroDescontoFolhaVM filtro)
        {
            if (filtro == null) return BadRequest();

            return OkResult(Service.BuscaFechamentosNaoDescontados(filtro));
        }

        
    }
}
