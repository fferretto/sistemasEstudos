using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using PagNet.Interface.Helpers;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasReceber.Controllers
{
    [Area("ContasReceber")]
    [Authorize]
    public class GerarBorderoController : ClientSessionControllerBase
    {
        private readonly IRecebimentoApp _recebimento;
        private readonly IPagNetUser _user;
        private readonly IAPIClienteAPP _APICliente;

        public GerarBorderoController(IRecebimentoApp recebimento,
                                      IAPIClienteAPP APICliente,
                                      IPagNetUser user)
        {
            _recebimento = recebimento;
            _user = user;
            _APICliente = APICliente;
        }

        public async Task<IActionResult> Index(string id)
        {
            var model = _recebimento.RetornaDadosInicio(id, _user.cod_empresa);
            model.acessoAdmin = _user.isAdministrator;

            return View(model);
        }
        public async Task<ActionResult> ConsultaSolicitacaoBoleto(FiltroConsultaFaturamentoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresa) <= 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                var Dados = await _recebimento.ConsultaSolicitacoesBoletos(model);

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.ERRO"] = ex.Message;
                DadosBoletoVM aux = new DadosBoletoVM();
                return PartialView(aux);
            }
        }

        public JsonResult BuscaCliente(string filtro, string codempresa)
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

            return Json(model);
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

        public JsonResult ReturnInstrucaoCobranca()
        {
            var Bancos = _recebimento.GetInstrucaoCobranca();

            return Json(Bancos);
        }
        [HttpPost]
        [RequestFormSizeLimit(valueCountLimit: 9000)]
        public async Task<IActionResult> SalvaBordero(DadosBoletoVM model)
        {
            try
            {
                model.codUsuario = _user.cod_usu;

                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa;


                var result = await _recebimento.SalvaBordero(model);

                return Json(new { success = true, responseText = result });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }

        }
    }
}