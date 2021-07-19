using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasPagar.Controllers
{
    [Area("ContasPagar")]
    [Authorize]
    public class ReagendarTituloController : ClientSessionControllerBase
    {
        private readonly IPagamentoApp _pagamento;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ICadastrosApp _cadastro;

        public ReagendarTituloController(IPagamentoApp pagamento,
                                         IPagNetUser user,
                                         ICadastrosApp cadastro,
                                         IDiversosApp diversos)
        {
            _pagamento = pagamento;
            _user = user;
            _diversos = diversos;
            _cadastro = cadastro;
        }
        public async Task<IActionResult> Index(string id)
        {
            FiltroTitulosPagamentoVM vm = new FiltroTitulosPagamentoVM();

            var empresa = _cadastro.RetornaDadosEmpresaByID(_user.cod_empresa);
            vm.codEmpresa = _user.cod_empresa.ToString();
            vm.nmEmpresa = empresa.NMFANTASIA.ToString();
            vm.dtInicio = DateTime.Now.AddDays(-1).ToShortDateString();
            vm.dtFim = DateTime.Now.AddDays(-1).ToShortDateString();


            vm.acessoAdmin = _user.isAdministrator;

            return View(vm);
        }
        public async Task<ActionResult> CarregaGridTitulosVencidos(FiltroTitulosPagamentoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                var Dados = _pagamento.GetAllTitulosVencidos(model);

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                GridTitulosVM list = new GridTitulosVM();
                return PartialView(list);
            }

        }

        public ActionResult JustificarTitulosEmMassa()
        {
            JustificativaVm vm = new JustificativaVm();

            return PartialView(vm);

        }
        public JsonResult GetTiposJustificativas()
        {
            var lista = new object[][]
             {
                new object[] { "NAOINFORMADO", ""},
                new object[] { "CHEQUE", "TÍTULO VENCIDO"},
                new object[] { "OUTROS", "OUTROS"}
             };

            return Json(lista.ToList());
        }

        [HttpPost]
        [RequestFormSizeLimit(valueCountLimit: 9000)]
        public async Task<IActionResult> AlteraTituloEmMassa(GridTitulosVM model)
        {
            try
            {
                model.codUsuario = _user.cod_usu;

                var result = await _pagamento.AlteraDataPGTOEmMassa(model);

                return Json(new { success = true, responseText = result.Values });
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return Json(ex.Message);
            }

        }
        
    }
}