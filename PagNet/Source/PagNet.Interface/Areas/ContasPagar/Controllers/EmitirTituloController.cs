using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Application.Helpers;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using PagNet.Interface.Helpers;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasPagar.Controllers
{
    [Area("ContasPagar")]
    [Authorize]
    public class EmitirTituloController : ClientSessionControllerBase
    {
        private readonly IAPIContaCorrente _APIContaCorrente;
        private readonly IPagamentoApp _pag;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ICadastrosApp _cadastro;
        private readonly IAPIFavorecido _apiFavorecido;

        public EmitirTituloController(IPagamentoApp pag,
                                      IAPIContaCorrente APIContaCorrente,
                                      IPagNetUser user,
                                      ICadastrosApp cadastro,
                                      IAPIFavorecido apiFavorecido,
                                      IDiversosApp diversos)
        {
            _pag = pag;
            _APIContaCorrente = APIContaCorrente;
            _user = user;
            _diversos = diversos;
            _cadastro = cadastro;
            _apiFavorecido = apiFavorecido;
        }
        public IActionResult Index(int? id)
        {
            EmissaoTituloAvulsto model = new EmissaoTituloAvulsto();

            var emp = _cadastro.RetornaDadosEmpresaByID(_user.cod_empresa);

            model.acessoAdmin = _user.isAdministrator;
            model.codUsuario = _user.cod_usu;
            model.CODEMPRESA = emp.CODEMPRESA.ToString();
            model.NMEMPRESA = emp.NMFANTASIA;
            model.PossuiNetCard = _user.PossuiNetCard;

            return View(model);
        }

        public ActionResult VisualizaTituloMassa()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                return PartialView(null);
            }

        }
        public JsonResult GetTipoTitulo()
        {
            var lista = new object[][]
             {
                new object[] { "0", ""},
                new object[] { "TEDDOC", "TRANSFERÊNCIA"},
                new object[] { "BOLETO", "PAGAMENTO DE BOLETO"}
             };

            return Json(lista.ToList());
        }

        public JsonResult BuscaFavorecido(string filtro, int? codEmpresa)
        {
            if (Convert.ToInt32(codEmpresa) <= 0)
            {
                codEmpresa = _user.cod_empresa;
            }
            APIFavorecidoVM model = new APIFavorecidoVM();

            if (filtro.Length <= 10)
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByID(Convert.ToInt32(filtro),Convert.ToInt32(codEmpresa)));
            }
            else
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByCPFCNPJ(filtro, Convert.ToInt32(codEmpresa)));
            }

            var retorno = model.codigoFavorecido + "/" + model.nomeFavorecido;

            if (model.codigoEmpresa != "" && model.codigoEmpresa != codEmpresa.ToString())
            {
                retorno = filtro + "/Favorecido não encontrado";
            }

            return Json(retorno);
        }

        [HttpPost]
        public async Task<ActionResult> Salvar(EmissaoTituloAvulsto model)
        {
            try
            {

                if (Convert.ToInt32(model.CODEMPRESA) <= 0)
                    model.CODEMPRESA = _user.cod_empresa.ToString();


                model.codUsuario = _user.cod_usu;

                if (ModelState.IsValid)
                {
                    var result = await _pag.IncluirNovoTituloPGTO(model);

                    TempData["Avis.Sucesso"] = result.FirstOrDefault().Value;
                    return RedirectToAction("Index", new { id = 0 });

                }

                return View("Index", model);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return View("Index", model);
            }
        }
        public JsonResult ValidaLinhaDigitavel(string linhadigitavel)
        {
            try
            {
                EmissaoTituloAvulsto model = new EmissaoTituloAvulsto();
                string codBanco;
                string codigoMoeda;
                string dataVencimento;
                long valAux = 0;
                decimal ValorBoleto = 0;
                DateTime database = Convert.ToDateTime("07/10/1997");

                if (!string.IsNullOrWhiteSpace(linhadigitavel)) //boleto de cobranca
                {
                    if (linhadigitavel.Substring(0, 1) != "8")
                    {
                        linhadigitavel = Geral.RemoveCaracteres(linhadigitavel);

                        codBanco = linhadigitavel.Substring(0, 3);
                        codigoMoeda = linhadigitavel.Substring(3, 1);
                        dataVencimento = linhadigitavel.Substring(32, 4);
                        valAux = Convert.ToInt64(linhadigitavel.Substring(37, 10));
                        string val1 = valAux.ToString().Substring(0, valAux.ToString().Length - 2);
                        string val2 = valAux.ToString().Substring(valAux.ToString().Length - 2, 2);
                        ValorBoleto = Convert.ToDecimal((val1 + "," + val2));

                        model.BANCO = codBanco + " - " + _APIContaCorrente.BuscaBancoByID(codBanco);
                        model.DATVENCIMENTOBOLETO = database.AddDays(Convert.ToInt32(dataVencimento)).ToShortDateString();
                        model.VALORBOLETO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (ValorBoleto)).Replace("R$ ", "");
                        //model.DATREALPGTOBOLETO = DateTime.Now.ToShortDateString();

                    }
                    else //Boleto de consumo (agua, luz, telefone, etc...)
                    {
                        linhadigitavel = Geral.RemoveCaracteres(linhadigitavel);
                        string valbolParte1 = linhadigitavel.Substring(4, 6);
                        string valbolParte2 = linhadigitavel.Substring(12, 4);
                        string valorsemformatacao = valbolParte1 + valbolParte2;
                        valAux = Convert.ToInt64(valorsemformatacao);

                        string val1 = valAux.ToString().Substring(0, valAux.ToString().Length - 2);
                        string val2 = valAux.ToString().Substring(valAux.ToString().Length - 2, 2);
                        ValorBoleto = Convert.ToDecimal((val1 + "," + val2));
                        model.VALORBOLETO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (ValorBoleto)).Replace("R$ ", "");

                        //model.DATVENCIMENTOBOLETO = DateTime.Now.ToShortDateString();
                        //model.DATREALPGTOBOLETO = DateTime.Now.ToShortDateString();

                        model.TIPOBOLETO = Geral.TipoSegmentoBoletoArrecadacao(linhadigitavel.Substring(1, 1));
                    }
                }



                return Json(model);

            }
            catch (Exception ex)
            {
                EmissaoTituloAvulsto model = new EmissaoTituloAvulsto();
                return Json(model);
            }
        }

        public async Task<ActionResult> ConsultaFavorecido(int? codEmpresa)
        {
            try
            {
                if (Convert.ToInt32(codEmpresa) <= 0)
                {
                    codEmpresa = _user.cod_empresa;
                }
                var dados = _apiFavorecido.ConsultaTodosFavorecidosPAG(Convert.ToInt32(codEmpresa));
                var ListaFavorecido = APIFavorecidoVM.ToListView(dados);

                return PartialView(ListaFavorecido);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }
        public async Task<FileResult> DownloadLayoutTitulosUsuaro(string tipoArquivo)
        {
            string nomeArquivo = "";

            if (tipoArquivo == "NC")
            {
                nomeArquivo = "Layout_Tit_Usuarios_NetCard_Massa.csv";
            }
            else if (tipoArquivo == "TM")
            {
                nomeArquivo = "Layout_Tit_Inclusao_Massa.csv";
            }
            var cam = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope);

            var caminho = Path.Combine(cam,"Layout_Arquivos", nomeArquivo);

            var path = Path.GetFullPath(caminho);

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
        public ActionResult uploadArquivo(string tipoArquivo)
        {
            bool ArquivoValido = true;
            string Descricao = "";
            try
            {
                string novoCaminho = "";
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);

                        var path = Path.Combine(CaminhoPadrao, "InclusaoTitulosMassa");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        string NewFileName = DateTime.Now.ToString("ddMMyyHHmmssffff") + ".csv";

                        novoCaminho = Path.Combine(path, NewFileName);

                        formFile.SaveTo(novoCaminho);
                    }
                }
                if (tipoArquivo == "NC")
                {
                    ArquivoValido = ValidaArquivoInclusaoMassaNC(novoCaminho);
                    if (!ArquivoValido)
                    {
                        Descricao = "Arquivo Inválido";
                    }
                }
                else if (tipoArquivo == "TM")
                {
                    ArquivoValido = ValidaArquivoInclusaoMassa(novoCaminho);
                    if (!ArquivoValido)
                    {
                        Descricao = "Arquivo Inválido";
                    }
                }

                return Json(new { success = true, responseText = novoCaminho, arquivovalido = ArquivoValido, descricao = Descricao });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message, arquivovalido = ArquivoValido, descricao = Descricao });
            }

        }

        public async Task<IActionResult> LogProcessoInclusaoMassa(string caminho, string tipoArquivo)
        {
            try
            {
                List<ListaTitulosInclusaoMassa> lista = new List<ListaTitulosInclusaoMassa>();

                if (tipoArquivo == "NC")
                {
                    lista = await _pag.CarregaArquivoTituloMassaNC(caminho, _user.cod_empresa, _user.cod_usu);
                }
                else if (tipoArquivo == "TM")
                {
                    lista = await _pag.CarregaArquivoTituloMassa(caminho, _user.cod_empresa, _user.cod_usu);
                }

                return PartialView(lista);
            }
            catch (Exception ex)
            {
                List<ListaTitulosInclusaoMassa> DadosRet = new List<ListaTitulosInclusaoMassa>();

                return PartialView(DadosRet);
            }
        }
        private bool ValidaArquivoInclusaoMassaNC(string CaminhoArquivo)
        {
            try
            {
                bool sucesso = true;
                string[] lines = System.IO.File.ReadAllLines(CaminhoArquivo);

                foreach (string line in lines)
                {
                    var Colunas = line.Split(';');
                    if (Colunas.Length > 7)
                    {
                        sucesso = false;
                    }
                    if (Colunas[0].IndexOf("USUARIO DE CARTAO") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[1].IndexOf("CPF") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[2].IndexOf("DATA PGTO") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[3].IndexOf("VALOR") < 0)
                    {
                        sucesso = false;
                    }
                    break;
                }

                return sucesso;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool ValidaArquivoInclusaoMassa(string CaminhoArquivo)
        {
            try
            {
                bool sucesso = true;
                string[] lines = System.IO.File.ReadAllLines(CaminhoArquivo);

                foreach (string line in lines)
                {
                    var Colunas = line.Split(';');
                    if (Colunas.Length > 17)
                    {
                        sucesso = false;
                    }
                    if (Colunas[0].IndexOf("CPF") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[1].IndexOf("Nome do Favorecido") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[2].IndexOf("Data Pagamento") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[3].IndexOf("Valor") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[4].IndexOf("CEP") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[5].IndexOf("Logradouro") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[6].IndexOf("Numero") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[7].IndexOf("Complemento") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[8].IndexOf("Bairro") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[9].IndexOf("Cidade") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[10].IndexOf("UF") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[11].IndexOf("Banco") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[12].IndexOf("Agencia") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[13].IndexOf("DV Agencia") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[14].IndexOf("ope") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[15].IndexOf("Conta") < 0)
                    {
                        sucesso = false;
                    }
                    if (Colunas[16].IndexOf("DV Conta") < 0)
                    {
                        sucesso = false;
                    }
                    break;
                }

                return sucesso;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<ActionResult> IncluirTituloEmMassa(ListaTitulosInclusaoMassa model)
        {
            try
            {
                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa;

                EmissaoTituloAvulsto tit = new EmissaoTituloAvulsto();
                tit.CODEMPRESA = model.codEmpresa.ToString();
                tit.codUsuario = _user.cod_usu;
                tit.CODFORNECEDOR = model.codFavorecidoNC;
                tit.VALOR = model.vlLiquido;
                tit.DATREALPGTO = model.DataPGTO;
                tit.TIPOTITULO = "TEDDOC";
                tit.LINHADIGITAVEL = "";

                var result = await _pag.IncluirNovoTituloPGTO(tit);

                return Json(new { success = true, responseText = result.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }
    }
}