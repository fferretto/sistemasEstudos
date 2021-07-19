using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasReceber.Controllers
{
    [Area("ContasReceber")]
    [Authorize]
    public class IncluirNovoBoletoController : ClientSessionControllerBase
    {
        private readonly IRecebimentoApp _recebimento;
        private readonly IPagNetUser _user;
        private readonly IConfiguracaoApp _cofig;
        private readonly IAPIClienteAPP _APICliente;

        public IncluirNovoBoletoController(IRecebimentoApp recebimento,
                                           IConfiguracaoApp cofig,
                                           IAPIClienteAPP APICliente,
                                           IPagNetUser user)
        {
            _recebimento = recebimento;
            _user = user;
            _cofig = cofig;
            _APICliente = APICliente;
        }

        public async Task<IActionResult> Index(int? id)
        {
            try
            {
                id = id ?? 0;
                var model = await _recebimento.DadosInicioEmissaoBoletoAvulso((int)id, _user.cod_empresa);
                model.acessoAdmin = _user.isAdministrator;
                if (model.codigoEmissaoBoleto == 0)
                {
                    model.dataVencimento = DateTime.Now.ToShortDateString();
                }

                return View(model);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ActionResult> BustaTodosBoletosParaEdicao(int codempresa)
        {
            try
            {

                if (Convert.ToInt32(codempresa) <= 0)
                    codempresa = _user.cod_empresa;

                var dados = await _recebimento.BustaTodosBoletosParaEdicao(codempresa);

                return PartialView(dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }
        public async Task<ActionResult> ConsultaTodosClientes(string codempresa)
        {
            try
            {
                if (Convert.ToInt32(codempresa) <= 0)
                    codempresa = _user.cod_empresa.ToString();

                var dados = await _APICliente.ConsultaTodosCliente(Convert.ToInt32(codempresa), "");
                var ListaCliente = APIClienteVm.ToListView(dados).ToList();

                return PartialView(ListaCliente);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }
        
        public JsonResult BuscarCliente(string filtro, string codempresa)
        {
            APIClienteVm model = new APIClienteVm();

            if (Convert.ToInt32(codempresa) <= 0)
                codempresa = _user.cod_empresa.ToString();

            if (filtro.Length < 11)
            {
                model = APIClienteVm.ToView(_APICliente.RetornaDadosClienteByIDeCodEmpresa(Convert.ToInt32(filtro), Convert.ToInt32(codempresa)));
            }
            else
            {
                model = APIClienteVm.ToView(_APICliente.RetornaDadosClienteByCPFCNPJeCodEmpresa(filtro, Convert.ToInt32(codempresa)));
            }

            if (model.REGRAPROPRIA == "NÃO")
            {
                var RegraDefault = _cofig.BuscaRegraAtiva(Convert.ToInt32(codempresa)).Result;
                if (RegraDefault.CODREGRA > 0)
                {
                    model.NMPRIMEIRAINSTCOBRA = RegraDefault.NMPRIMEIRAINSTCOBRA.Trim();
                    model.NMSEGUNDAINSTCOBRA = RegraDefault.NMSEGUNDAINSTCOBRA.Trim();
                    model.TAXAEMISSAOBOLETO = RegraDefault.TAXAEMISSAOBOLETO;
                    model.VLJUROSDIAATRASO = RegraDefault.VLJUROSDIAATRASO;
                    model.VLMULTADIAATRASO = RegraDefault.VLMULTADIAATRASO;
                    model.PERCJUROS = RegraDefault.PERCJUROS;
                    model.PERCMULTA = RegraDefault.PERCJUROS;
                    model.COBRAJUROS = RegraDefault.COBRAJUROS;
                    model.COBRAMULTA = RegraDefault.COBRAMULTA;
                }
            }

            return Json(model);
        }

        [HttpPost]
        public async Task<ActionResult> Salvar(EmissaoBoletoVM model)
        {
            try
            {

                if (Convert.ToInt32(model.codigoFormaFaturamento) <= 0)
                    model.codigoFormaFaturamento = "1";

                if (Convert.ToInt32(model.codigoEmpresa) <= 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();


                model.codigoUsuario = _user.cod_usu;

                if (ModelState.IsValid)
                {
                    var result = await _recebimento.IncluiNovoPedidoFaturamento(model);
                    if (result.FirstOrDefault().Key)
                    {
                        TempData["Avis.Sucesso"] = result.FirstOrDefault().Value;
                    }
                    else
                    {
                        TempData["Avis.Erro"] = result.FirstOrDefault().Value;
                    }
                    return RedirectToAction("Index", new { id = 0 });

                }

                return RedirectToAction("Index", model);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return View("Index", model);
            }
        }

        public ActionResult JustificarCancelamentoPedidoFaturamento()
        {
            EmissaoBoletoVM vm = new EmissaoBoletoVM();

            return PartialView(vm);

        }
        public ActionResult JustificarEdicaoPedidoFaturamento()
        {
            EmissaoBoletoVM vm = new EmissaoBoletoVM();

            return PartialView(vm);

        }
        [HttpPost]
        public async Task<ActionResult> CancelarFatura(EmissaoBoletoVM model)
        {
            try
            {
                model.codigoUsuario = _user.cod_usu;

                var retorno = await _recebimento.CancelarFatura(model);

                return Json(new { success = retorno.FirstOrDefault().Key, responseText = retorno.FirstOrDefault().Value });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Ocorreu uma falha durante o processo de cancelamento. Favor contactar o suporte!" });
            }
        }
        [HttpPost]
        public async Task<ActionResult> LiquidacaoManual(EmissaoBoletoVM model)
        {
            try
            {
                model.codigoUsuario = _user.cod_usu;

                if (ModelState.IsValid)
                {
                    var result = await _recebimento.LiquidacaoManual(model);
                    if (result.FirstOrDefault().Key)
                    {
                        TempData["Avis.Sucesso"] = result.FirstOrDefault().Value;
                    }
                    else
                    {
                        TempData["Avis.Erro"] = result.FirstOrDefault().Value;
                    }
                    return RedirectToAction("Index");

                }

                return RedirectToAction("Index", model);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return View("Index", model);
            }
        }


    }
}