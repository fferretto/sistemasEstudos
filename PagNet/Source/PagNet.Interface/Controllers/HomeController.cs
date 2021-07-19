using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Models;
using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Controllers
{
    [Authorize]
    public class HomeController : ClientSessionControllerBase
    {
        private readonly IValidaUsuarioApp _validaUsuario;
        private readonly IPagNetUser _user;
        private readonly IRecebimentoApp _recebimento;
        private readonly IPagamentoApp _pagamento;
        private readonly IDiversosApp _diversos;


        public HomeController(IValidaUsuarioApp validaUsuario,
                              IRecebimentoApp recebimento,
                              IPagamentoApp pag,
                              IDiversosApp diversos,
                              IPagNetUser user)
        {
            _user = user;
            _validaUsuario = validaUsuario;
            _pagamento = pag;
            _recebimento = recebimento;
            _diversos = diversos;
        }
        public IActionResult Index()
        {
            try
            {
                PaginaInicialVm vm = new PaginaInicialVm();
                vm.codigoEmpresa = _user.cod_empresa.ToString();
                //vm.nomeEmpresa = _diversos.GetnmEmpresaByID(_user.cod_empresa);
                vm.acessoAdmin = _user.isAdministrator;

                
                return View(vm);
            }
            catch (Exception EX)
            {
                var msg = EX.Message;
                return RedirectToAction("logout", "Autenticacao", new { area = "Identificacao" });
            }

        }

        public async Task<ActionResult> ListaTitulosNaoLiquidado(int? codEmpresa, string status)
        {
            try
            {
                if(Convert.ToInt32(codEmpresa) <= 0)
                {
                    codEmpresa = _user.cod_empresa;
                }

                var Dados = await _pagamento.ListaTitulosNaoLiquidado((int)codEmpresa);

                if (status == "VENCIDO")
                {
                    Dados = Dados.Where(x => x.STATUS != "AGUARDANDO ARQUIVO RETORNO").ToList();
                }
                else
                {
                    Dados = Dados.Where(x => x.STATUS == "AGUARDANDO ARQUIVO RETORNO").ToList();
                }
                
                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";          
                return PartialView(null);
            }

        }
        public async Task<ActionResult> ListaFaturamentoNaoLiquidado(int? codEmpresa, string status)
        {
            try
            {
                if (Convert.ToInt32(codEmpresa) <= 0)
                {
                    codEmpresa = _user.cod_empresa;
                }

                var Dados = await _recebimento.ListaFaturamentoNaoLiquidado((int)codEmpresa);

                if(status == "VENCIDO")
                {
                    Dados = Dados.Where(x => x.Status != "PENDENTE REGISTRO" || x.Status != "REGISTRADO").ToList();
                }
                else if(status == "PENDENTEREGISTRO")
                {
                    Dados = Dados.Where(x => x.Status == "PENDENTE REGISTRO").ToList();
                }
                else
                {
                    Dados = Dados.Where(x => x.Status != "REGISTRADO").ToList();
                }
                               
                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public ActionResult Master()
        //{

        //    var menu = _validaUsuario.CarregaMenus();

        //    return View("~/Views/Shared/Master.cshtml", menu);

        //}

    }
}
