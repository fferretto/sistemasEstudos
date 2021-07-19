using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasPagar.Controllers
{
    [Area("ContasPagar")]
    [Authorize]
    public class ConsultaBorderoController : ClientSessionControllerBase
    {
        private readonly IPagamentoApp _fecheCre;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ICadastrosApp _cadastro;

        public ConsultaBorderoController(IPagamentoApp fecheCre,
                                         IPagNetUser user,
                                         ICadastrosApp cadastro,
                                         IDiversosApp diversos)
        {
            _fecheCre = fecheCre;
            _user = user;
            _diversos = diversos;
            _cadastro = cadastro;
        }

        public async Task<IActionResult> Index(string id)
        {
            FiltroConsultaBorderoPagVM model = new FiltroConsultaBorderoPagVM();

            var empresa = _cadastro.RetornaDadosEmpresaByID(_user.cod_empresa);
            model.codEmpresa = empresa.CODEMPRESA.ToString();
            model.nmEmpresa = empresa.NMFANTASIA;

            model.acessoAdmin = _user.isAdministrator;
            model.codStatus = "EM_BORDERO";
            model.nmStatus = "EM BORDERO";

            return View(model);
        }
        public async Task<ActionResult> ConsultaBordero(FiltroConsultaBorderoPagVM model)
        {
            try
            {
                if (Convert.ToInt32(model.CodFormaPagamento) <= 0)
                    model.CodFormaPagamento = "";

                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                var Dados = _fecheCre.ConsultaBordero(model);

                Dados.codigoEmpresa = model.codEmpresa;
                Dados.codigoBanco = model.codBanco;

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                GridTitulosVM Dados = new GridTitulosVM();
                return PartialView(Dados);
            }

        }
        public async Task<JsonResult> StatusBordero()
        {

            var lista = new object[][]
             {
                new object[] { "EM_BORDERO", "EM BORDERO" },
                new object[] { "AGUARDANDO_ARQUIVO_RETORNO", "AGUARDANDO ARQUIVO RETORNO" },
                new object[] { "BAIXADO", "BAIXADO"}
             };

            return Json(lista.ToList());
        }
        public JsonResult BuscaBanco(string filtro)
        {
            FiltroTitulosPagamentoVM model;


                model = _fecheCre.FindBancoByID(Convert.ToInt32(filtro));
     

            var retorno = model.filtroCodBanco + "/" + model.FiltroNmBanco + "/" + model.codBanco;

            return Json(retorno);
        }

        public async Task<ActionResult> ConsultaFechamentosBordero(int codBordero)
        {
            try
            {
                var dados = await _fecheCre.GetAllPagamentosBordero(codBordero);

                return PartialView(dados);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> CancelaBordero(int codBordero)
        {
            try
            {
                _fecheCre.CancelaBordero(codBordero, _user.cod_usu);

                return Json(new { success = true, responseText = "Borderô cancelado com sucesso!" });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return Json(new { success = false, responseText = "Ocorreu uma falha inesperada. Favor contactar o suporte!" });
            }

        }
    }
}