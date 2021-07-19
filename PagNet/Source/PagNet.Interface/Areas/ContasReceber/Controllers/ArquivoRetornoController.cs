using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.ContasReceber.Models;
using PagNet.Interface.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;


namespace PagNet.Interface.Areas.ContasReceber.Controllers
{
    [Area("ContasReceber")]
    [Authorize]
    public class ArquivoRetornoController : ClientSessionControllerBase
    {
        private readonly IPagamentoApp _pag;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ICadastrosApp _cadastro;
        private readonly IAPIClienteAPP _APICliente;
        private readonly IAPITransmissaoCobrancaBancaria _ApiCobrancaBancaria;

        public ArquivoRetornoController(IPagamentoApp pag,
                                        IAPIClienteAPP APICliente,
                                        IPagNetUser user,
                                        ICadastrosApp cadastro,
                                        IDiversosApp diversos, 
                                        IAPITransmissaoCobrancaBancaria ApiCobrancaBancaria
                                )
        {
            _pag = pag;
            _user = user;
            _diversos = diversos;
            _cadastro = cadastro;
            _APICliente = APICliente;
            _ApiCobrancaBancaria = ApiCobrancaBancaria;
        }

        public async Task<IActionResult> Index(int? id)
        {
            FiltroArquivoRetornoVM model = new FiltroArquivoRetornoVM();
            
            var empresa = _cadastro.RetornaDadosEmpresaByID(_user.cod_empresa);
            model.codigoEmpresa = empresa.CODEMPRESA.ToString();
            model.nomeEmpresa = empresa.NMFANTASIA.ToString();

            model.acessoAdmin = _user.isAdministrator;

            return View(model);
        }
        [HttpPost]
        public ActionResult UploadArquivoValidacaoRemessa()
        {
            try
            {
                string CaminhoArquivo = "";
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {

                        var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);
                        string nmOperadora = _user.nmPerfilOperadora;

                        var path = Path.Combine(CaminhoPadrao, "ArquivosVisualizados", nmOperadora);

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);


                        string NewFileName = DateTime.Now.ToString("ddMMyyHHmmssffff") + ".txt";

                        CaminhoArquivo = Path.Combine(path, NewFileName);
                        formFile.SaveTo(CaminhoArquivo);
                    }
                }


                return Json(new { success = true, responseText = CaminhoArquivo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }

        }
        public JsonResult BuscarCliente(string filtro, string codempresa)
        {
            ClienteArquivoRetornoRecModel model = new ClienteArquivoRetornoRecModel();

            if (Convert.ToInt32(codempresa) <= 0)
                codempresa = _user.cod_empresa.ToString();

            if (filtro.Length < 11)
            {
                model = ClienteArquivoRetornoRecModel.ToView(_APICliente.RetornaDadosClienteByIDeCodEmpresa(Convert.ToInt32(filtro), Convert.ToInt32(codempresa)));
            }
            else
            {
                model = ClienteArquivoRetornoRecModel.ToView(_APICliente.RetornaDadosClienteByCPFCNPJeCodEmpresa(filtro, Convert.ToInt32(codempresa)));
            }

            return Json(model);
        }
        public async Task<ActionResult> ConsultaClientesEmpresa(string codempresa)
        {
            try
            {
                if (Convert.ToInt32(codempresa) <= 0)
                    codempresa = _user.cod_empresa.ToString();

                var dados = await _APICliente.ConsultaTodosCliente(Convert.ToInt32(codempresa), "J");
                var ListaCliente = ClienteArquivoRetornoRecModel.ToListView(dados).ToList();

                return PartialView(ListaCliente);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }
        public IActionResult ValidaArquivo(string CaminhoArquivo)
        {
            try
            {
                //string nmOperadora = _user.nmPerfilOperadora;
                var Dados = _ApiCobrancaBancaria.CarregaDadosArquivoRetorno(CaminhoArquivo);
                var dadosModel = APISolicitacaoBoletoModel.ToListView(Dados).ToList();

                return PartialView(dadosModel);
            }
            catch (Exception ex)
            {
                List<APISolicitacaoBoletoModel> DadosRet = new List<APISolicitacaoBoletoModel>();
                DadosRet.Add(new APISolicitacaoBoletoModel()
                {
                    MsgRetorno = "Falha ao ler o arquivo. Favor Tentar novamente, mas se o problema persistir, favor contactar o suporte: "
                });
                return PartialView(DadosRet);
            }

        }
        [HttpPost]
        public ActionResult UploadArquivoCliente()
        {
            try
            {
                string CaminhoArquivo = "";
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {

                        var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);
                        string nmOperadora = _user.nmPerfilOperadora;

                        var path = Path.Combine(CaminhoPadrao, "ArquivosVisualizados", nmOperadora);

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);


                        string NewFileName = DateTime.Now.ToString("ddMMyyHHmmssffff") + ".txt";

                        CaminhoArquivo = Path.Combine(path, NewFileName);
                        formFile.SaveTo(CaminhoArquivo);
                    }
                }


                return Json(new { success = true, responseText = CaminhoArquivo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }

        }
        //[HttpPost]
        //public async Task<IActionResult> UploadArquivoCliente(List<IFormFile> arquivos)
        //{
        //    try
        //    {

        //        string CaminhoArquivo = "";
        //        long tamanhoArquivos = arquivos.Sum(f => f.Length);
        //        // caminho completo do arquivo na localização temporária
        //        var caminhoArquivo = Path.GetTempFileName();
        //        // processa os arquivo enviados
        //        //percorre a lista de arquivos selecionados
        //        foreach (var arquivo in arquivos)
        //        {
        //            //verifica se existem arquivos 
        //            if (arquivo == null || arquivo.Length == 0)
        //            {
        //                //retorna a viewdata com erro
        //                ViewData["Erro"] = "Error: Arquivo(s) não selecionado(s)";
        //                return View(ViewData);
        //            }
        //            // < define a pasta onde vamos salvar os arquivos >
        //            string pasta = "Arquivos_Usuario";
        //            // Define um nome para o arquivo enviado incluindo o sufixo obtido de milesegundos
        //            string nomeArquivo = "Usuario_arquivo_" + DateTime.Now.Millisecond.ToString();
        //            //verifica qual o tipo de arquivo : jpg, gif, png, pdf ou tmp
        //            if (arquivo.FileName.Contains(".jpg"))
        //                nomeArquivo += ".jpg";
        //            else if (arquivo.FileName.Contains(".gif"))
        //                nomeArquivo += ".gif";
        //            else if (arquivo.FileName.Contains(".png"))
        //                nomeArquivo += ".png";
        //            else if (arquivo.FileName.Contains(".pdf"))
        //                nomeArquivo += ".pdf";
        //            else
        //                nomeArquivo += ".tmp";
        //            //< obtém o caminho físico da pasta wwwroot >
        //            string caminho_WebRoot = _appEnvironment.WebRootPath;
        //            // monta o caminho onde vamos salvar o arquivo : 
        //            // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos
        //            string caminhoDestinoArquivo = caminho_WebRoot + "\\Arquivos\\" + pasta + "\\";
        //            // incluir a pasta Recebidos e o nome do arquivo enviado : 
        //            // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos\
        //            string caminhoDestinoArquivoOriginal = caminhoDestinoArquivo + "\\Recebidos\\" + nomeArquivo;
        //            //copia o arquivo para o local de destino original
        //            using (var stream = new FileStream(caminhoDestinoArquivoOriginal, FileMode.Create))
        //            {
        //                await arquivo.CopyToAsync(stream);
        //            }
        //        }

        //            return Json(new { success = true, responseText = CaminhoArquivo });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, responseText = ex.Message });
        //    }

        //}

    }
}