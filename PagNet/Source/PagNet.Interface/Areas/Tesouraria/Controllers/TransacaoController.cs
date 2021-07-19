using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using Telenet.AspNetCore.Mvc.Authentication;


namespace PagNet.Interface.Areas.Tesouraria.Controllers
{
    [Area("Tesouraria")]
    [Authorize]
    public class TransacaoController : ClientSessionControllerBase
    {
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ITesourariaApp _tesouraria;

        public TransacaoController(ITesourariaApp tesouraria,
                                            IDiversosApp diversos,
                                            IPagNetUser user)
        {
            _tesouraria = tesouraria;
            _user = user;
            _diversos = diversos;
        }
        public IActionResult Index()
        {
            var model = _tesouraria.CarregaDadosInicio(_user.cod_empresa).Result;
            model.acessoAdmin = _user.isAdministrator;

            return View(model);
        }

        public async Task<ActionResult> ListagemTransacao(FiltroTransacaoVM model)
        {
            try
            {

                if (Convert.ToInt32(model.codigoEmpresa) < 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                var Dados = await _tesouraria.ListaTransacoesAConsolidar(model);

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                return PartialView(null);
            }

        }
        public async Task<ActionResult> IncluirTransacao(int CodTransacao, string TipoTransacao)
        {
            try
            {
                IncluiTransacaoVM model = new IncluiTransacaoVM();
                model.acessoAdminIncTransacao = _user.isAdministrator;
                model.codigoEmpresaIncTransacao = _user.cod_empresa.ToString();
                model.nomeEmpresaIncTransacao = _diversos.GetnmEmpresaByID(_user.cod_empresa);
                model.dtIncTransacao = DateTime.Now.ToShortDateString();

                return PartialView(model);
            }
            catch (Exception)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                DadosTituloVm VmVazio = new DadosTituloVm();
                return PartialView(VmVazio);
            }

        }
        public async Task<ActionResult> EdicaoTransacao(int CodTransacao, string TipoTransacao)
        {
            try
            {
                var dados = await _tesouraria.ConsultaTransacao(CodTransacao, TipoTransacao);

                return PartialView(dados);
            }
            catch (Exception)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                DadosTituloVm VmVazio = new DadosTituloVm();
                return PartialView(VmVazio);
            }

        }
        public JsonResult CarregaListaAcoes()
        {

            var lista = new object[][]
             {
                new object[] { "RealizarAcao", "INCLUIRTRANSACAO", "Incluir" },
                new object[] { "RealizarAcao", "EDITATRANSACAO", "Editar" },
                new object[] { "RealizarAcao", "CONSOLIDATRANSACAO", "Consolidar"},
                new object[] { "RealizarAcao", "CANCELATRANSACAO", "Cancelar"}
             };

            return Json(lista.ToList());
        }
        [HttpPost]
        public async Task<ActionResult> IncluirTransacao(IncluiTransacaoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresaIncTransacao) <= 0)
                    model.codigoEmpresaIncTransacao = _user.cod_empresa.ToString();

                model.codigoUsuarioIncTransacao = _user.cod_usu;

                if (ModelState.IsValid)
                {
                    IDictionary<bool, string> retorno;
                    if (model.TipoIncTransacao == "SAIDA")
                    {
                        retorno = await _tesouraria.IncluirNovaTransacaoSAIDA(model);
                    }
                    else
                    {
                        retorno = await _tesouraria.IncluirNovaTransacaoENTRADA(model);
                    }

                    return Json(new { success = true, responseText = retorno.FirstOrDefault().Value });

                }

                return View("Index", model);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Falha ao cancelar o título. Favor contactar o suporte!" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CancelarTransacao(EditarTransacaoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresaTransacao) <= 0)
                    model.codigoEmpresaTransacao = _user.cod_empresa.ToString();

                model.codigoUsuario= _user.cod_usu;

                var result = await _tesouraria.CancelaTransacao(model);

                return Json(new { success = true, responseText = result.FirstOrDefault().Value });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Falha ao cancelar o título. Favor contactar o suporte!" });
            }

        }
        [HttpPost]
        public async Task<IActionResult> ConsolidaTransacao(EditarTransacaoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresaTransacao) <= 0)
                    model.codigoEmpresaTransacao = _user.cod_empresa.ToString();

                model.codigoUsuario = _user.cod_usu;

                var result = await _tesouraria.ConsolidaTransacao(model);

                return Json(new { success = true, responseText = result.FirstOrDefault().Value });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Falha ao cancelar o título. Favor contactar o suporte!" });
            }

        }
        public async Task<IActionResult> VerificaParcelasFuturas(int CodTransacao, string TipoTransacao)
        {
            try
            {
                var PossuiParcelasFuturas = await _tesouraria.VerificaParcelasFuturas(CodTransacao, TipoTransacao);

                return Json(new { resultado = PossuiParcelasFuturas });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Falha ao cancelar o título. Favor contactar o suporte!" });
            }

        }
        [HttpPost]
        public IActionResult SalvaEdicaoTransacao(EditarTransacaoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresaTransacao) <= 0)
                    model.codigoEmpresaTransacao = _user.cod_empresa.ToString();

                model.codigoUsuario = _user.cod_usu;

                IDictionary<bool, string> resultado;
                if (model.TipoTransacao == "ENTRADA")
                {
                    resultado = _tesouraria.AtualizaTransacaoENTRADA(model).Result;
                }
                else
                {
                    resultado = _tesouraria.AtualizaTransacaoSAIDA(model).Result;
                }


                return Json(new { success = true, responseText = resultado.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }
    }
}