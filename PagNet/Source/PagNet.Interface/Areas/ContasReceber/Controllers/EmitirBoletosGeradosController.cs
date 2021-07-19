using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasReceber.Controllers
{
    [Area("ContasReceber")]
    [Authorize]
    public class EmitirBoletosGeradosController : ClientSessionControllerBase
    {
        private readonly IRecebimentoApp _recebimento;
        private readonly IPagNetUser _user;
        private readonly IConfiguracaoApp _cofig;
        private readonly IDiversosApp _diversos;
        private readonly IContaEmailApp _email;
        private readonly IAPIClienteAPP _APICliente;

        public EmitirBoletosGeradosController(IRecebimentoApp recebimento,
                                              IConfiguracaoApp cofig,
                                              IDiversosApp diversos,
                                              IContaEmailApp email,
                                              IAPIClienteAPP APICliente,
                                              IPagNetUser user)
        {
            _recebimento = recebimento;
            _user = user;
            _cofig = cofig;
            _diversos = diversos;
            _email = email;
            _APICliente = APICliente;
        }
        public async Task<IActionResult> Index(string id)
        {
            var model = _recebimento.RetornaDadosInicioConstulaBordero(id, _user.cod_empresa);
            model.acessoAdmin = _user.isAdministrator;
            model.codStatus = "REGISTRADO";
            model.nmStatus = "REGISTRADO";
            model.dtInicio = DateTime.Now.ToShortDateString();
            model.dtFim = DateTime.Now.AddDays(7).ToShortDateString();

            return View(model);
        }
        public async Task<JsonResult> StatusBoleto()
        {

            var lista = new object[][]
             {
                new object[] { "PENDENTE", "PENDENTE" },
                new object[] { "REGISTRADO", "REGISTRADO" }
             };

            return Json(lista.ToList());
        }
        public async Task<IActionResult> CarregaListaBoletos(FiltroConsultaFaturamentoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresa) <= 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                var Dados = await _recebimento.ConsultaBoletosGerados(model);
                for(int i = 0; i< Dados.Count; i++)
                {
                    Dados[i].codigoEmpresaBoleto = Convert.ToInt32(model.codigoEmpresa);
                }
                
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
        public async Task<ActionResult> EnvioEmailEmMassa()
        {
            return PartialView();
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
        [HttpPost]
        public async Task<IActionResult> EnviarBoletoEmail(int codEmissaoBoleto)
        {
            try
            {
                var retorno = await _email.EnviaEmailBoleto(codEmissaoBoleto, _user.cod_ope, _user.cod_usu, _user.cod_empresa);

                return Json(new { success = retorno.FirstOrDefault().Key, responseText = retorno.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult EnviarBoletoEmailEmMassa(DadosEnvioEmailMassModel listaBol)
        {
            try
            {
                var retorno = _email.EnviaEmailBoletoEmMassa(listaBol, _user.cod_ope, _user.cod_usu);

                return Json(new { success = retorno.FirstOrDefault().Key, responseText = retorno.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> EnviarBoletoOutroEmail(int codEmissaoBoleto, string email)
        {
            try
            {
                var retorno = await _email.EnviarBoletoOutroEmail(codEmissaoBoleto, email, _user.cod_ope, _user.cod_usu, _user.cod_empresa);

                return Json(new { success = retorno.FirstOrDefault().Key, responseText = retorno.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }

        public async Task<FileResult> DownloadBoleto(string id, string codigo, int codigoempresa)
        {
            if(codigoempresa <= 0)
            {
                codigoempresa = _user.cod_empresa;
            }

            var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, codigoempresa);
            
            var url = Path.Combine(CaminhoPadrao, "PDFBoleto", id + ".pdf");

            if(!System.IO.File.Exists(url))
            {
                _recebimento.GeraBoletoPDF(Path.Combine(CaminhoPadrao, "PDFBoleto"), Convert.ToInt32(codigo), _user.cod_usu);
            }
            
            var path = Path.GetFullPath(url);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }       
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".rem", "text/rem"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        public JsonResult DDLBoletosEntregues()
        {
            var lista = new object[][]
             {
                new object[] { "T", "TODOS"},
                new object[] { "S", "SIM"},
                new object[] { "N", "NÃO" }
             };

            return Json(lista.ToList());
        }
        
    }
}