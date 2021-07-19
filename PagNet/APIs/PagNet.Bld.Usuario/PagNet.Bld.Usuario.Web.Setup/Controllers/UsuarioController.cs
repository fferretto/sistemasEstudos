using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Bld.Usuario.Abstraction.Interface;
using PagNet.Bld.Usuario.Web.Setup.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.Bld.Usuario.Web.Setup.Controllers
{
    public class UsuarioController : ServiceController<IUsuarioApp>
    {
        public UsuarioController(IUsuarioApp service)
            : base(service)
        { }
        [HttpPost]
        [Authorize]
        [Route("GetUsuario")]
        public IActionResult GetUsuario([FromBody] FiltroModel filtro)
        {
            if (filtro == null) return BadRequest();
            return OkResult(Service.GetUsuario(filtro));

        }
        [HttpPost]
        [Authorize]
        [Route("Salvar")]
        public IActionResult Salvar([FromBody] UsuarioModel filtro)
        {
            if (filtro == null) return BadRequest();
            return OkResult(Service.Salvar(filtro));
        }
        [HttpPost]
        [Authorize]
        [Route("Desativar")]
        public IActionResult Desativar([FromBody] FiltroModel filtro)
        {
            if (filtro == null) return BadRequest();
            return OkResult(Service.Desativar(filtro));
        }
        [HttpPost]
        [Authorize]
        [Route("GetAllUsuarioByCodOpe")]
        public IActionResult GetAllUsuarioByCodOpe([FromBody] FiltroModel filtro)
        {
            if (filtro == null) return BadRequest();
            return OkResult(Service.GetAllUsuarioByCodOpe(filtro));
        }
        [HttpPost]
        [Authorize]
        [Route("GetAllUsuarioByCodEmpresa")]
        public IActionResult GetAllUsuarioByCodEmpresa([FromBody] FiltroModel filtro)
        {
            if (filtro == null) return BadRequest();
            return OkResult(Service.GetAllUsuarioByCodEmpresa(filtro));
        }
        [HttpPost]
        [Route("ValidaLoginRecuperarSenha")]
        public IActionResult ValidaLoginRecuperarSenha([FromBody] FiltroModel filtro)
        {
            if (filtro == null) return BadRequest();
            return OkResult(Service.ValidaLoginRecuperarSenha(filtro));
        }
        [HttpPost]
        [Route("SalvaAlteracaoSenha")]
        public IActionResult SalvaAlteracaoSenha([FromBody] UsuarioModel filtro)
        {
            if (filtro == null) return BadRequest();
            return OkResult(Service.SalvaAlteracaoSenha(filtro));
        }
    }
}