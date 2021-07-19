using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasReceber.Controllers
{
    [Area("ContasReceber")]
    [Authorize]
    public class ConsultarBorderoController : ClientSessionControllerBase
    {
        private readonly IRecebimentoApp _recebimento;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;

        public ConsultarBorderoController(IRecebimentoApp recebimento,
                                         IPagNetUser user,
                                         IDiversosApp diversos)
        {
            _recebimento = recebimento;
            _user = user;
            _diversos = diversos;
        }
        public async Task<IActionResult> Index(string id)
        {
            var model = _recebimento.RetornaDadosInicioConstulaBordero(id, _user.cod_empresa);
            model.acessoAdmin = _user.isAdministrator;
            model.codStatus = "EM_BORDERO";
            model.nmStatus = "EM BORDERO";

            return View(model);
        }
        public async Task<ActionResult> ConsultaBordero(FiltroConsultaFaturamentoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresa) <= 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                var Dados = _recebimento.ConsultaBordero(model);

                Dados.codigoEmpresa = model.codigoEmpresa;
                Dados.dtVencimento = DateTime.Now.ToShortDateString();

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                BorderoBolVM Dados = new BorderoBolVM();
                return PartialView(Dados);
            }

        }
        public async Task<JsonResult> StatusBordero()
        {

            var lista = new object[][]
             {
                new object[] { "EM_BORDERO", "EM BORDERO" },
                new object[] { "PENDENTE_REGISTRO", "PENDENTE REGISTRO" },
                new object[] { "REGISTRADO", "REGISTRADO" },
                new object[] { "LIQUIDADO", "LIQUIDADO" }
             };


            return Json(lista.ToList());
        }
        public async Task<ActionResult> ConsultaBoletosBordero(int codBordero)
        {
            try
            {
                var dados = await _recebimento.GetAllBoletosByBordero(codBordero);

                return PartialView(dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CancelaBordero(int codBordero)
        {
            try
            {
                _recebimento.CancelaBordero(codBordero, _user.cod_usu);

                return Json(new { success = true, responseText = "Borderê cancelado com sucesso!" });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return Json(new { success = false, responseText = "Ocorreu uma falha inesperada. Favor contactar o suporte!" });
            }

        }

    }
}