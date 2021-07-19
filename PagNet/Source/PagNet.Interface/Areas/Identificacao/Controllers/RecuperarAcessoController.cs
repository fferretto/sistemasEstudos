using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Application.Interface;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Identificacao.Models;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Identificacao.Controllers
{
    [Area("Identificacao")]
    public class RecuperarAcessoController : Controller
    {
        IAPIUsuario _validaUsuario;
        IPagNetUser _user;
        public RecuperarAcessoController(IAPIUsuario validaUsuario)
        {
            _validaUsuario = validaUsuario;
        }
        public IActionResult Index()
        {
            FiltroRecuperaSenhaModel model = new FiltroRecuperaSenhaModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult EnviaLinkRecuperaAcesso(FiltroRecuperaSenhaModel model)
        {
            RecuperaSenhaModel filtro = new RecuperaSenhaModel();
            if(model.EmailLoginUsuario.IndexOf(".com") > 0)
            {
                filtro.EmailUsuario = model.EmailLoginUsuario;
            }
            else
            {
                filtro.Login = model.EmailLoginUsuario;
            }
            var resultadoValidacao = _validaUsuario.ValidaLoginRecuperarSenha(filtro);

            model.Sucesso = resultadoValidacao.Sucesso;
            model.MensagemRetorno = resultadoValidacao.msgResultado;

            return View(model);
        }
        public ActionResult AlterarSenha()
        {
            NovaSenhaModel model = new NovaSenhaModel();
            model.codigoEnviado = "123456";
            return View(model);
        }
    }
}