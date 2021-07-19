using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.ContasPagar.Models;
using PagNet.Interface.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasPagar.Controllers
{
    [Area("ContasPagar")]
    [Authorize]
    public class ArquivoRetornoController : ClientSessionControllerBase
    {
        private readonly IPagamentoApp _fecheCre;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly IAPITrasmissaoPagamento _ApiEmissaoArquivoPgto;

        public ArquivoRetornoController(IPagNetUser user,
                                        IPagamentoApp fecheCre,
                                        IAPITrasmissaoPagamento ApiEmissaoArquivoPgto,
                                        IDiversosApp diversos)
        {
            _fecheCre = fecheCre;
            _user = user;
            _diversos = diversos;
            _ApiEmissaoArquivoPgto = ApiEmissaoArquivoPgto;
        }

        public async Task<IActionResult> Index(string id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadHomeReport()
        {
            try
            {
                string novoCaminho = "";
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);

                        var path = Path.Combine(CaminhoPadrao, "ArquivoRetornoPagamento");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        string NewFileName = DateTime.Now.ToString("ddMMyyHHmmssffff") + ".txt";

                        novoCaminho = Path.Combine(path, NewFileName);

                        formFile.SaveTo(novoCaminho);
                    }
                }
                return Json(new { success = true, responseText = novoCaminho });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }

        }
        public IActionResult ValidaArquivo(string caminho)
        {
            try
            {

                string[] LinhaArquivo = System.IO.File.ReadAllLines(caminho);
                string codigoBanco = LinhaArquivo[0].Substring(0, 3);
                List<RetornoArquivoBancoVM> listaRetorno = new List<RetornoArquivoBancoVM>();

                var dados = _ApiEmissaoArquivoPgto.ProcessaArquivoRetorno(caminho);
                listaRetorno = RetornoArquivoBancoVM.ToListView(dados).ToList();

                if (listaRetorno.Count == 0)
                {
                    RetornoArquivoBancoVM mdTemp = new RetornoArquivoBancoVM();
                    mdTemp.mensagemRetorno = "Não foi identificado nenhum título originado do sistema PagNet dentro do arquivo de retorno.";
                    listaRetorno.Add(mdTemp);
                }

                return PartialView(listaRetorno);
            }
            catch (Exception ex)
            {
                List<RetornoArquivoBancoVM> DadosRet = new List<RetornoArquivoBancoVM>();
                DadosRet.Add(new RetornoArquivoBancoVM()
                {
                    mensagemRetorno = "Falha ao ler o arquivo. Favor Tentar novamente, mas se o problema persistir, favor contactar o suporte: "
                });
                return PartialView(DadosRet);
            }

        }

    }
}