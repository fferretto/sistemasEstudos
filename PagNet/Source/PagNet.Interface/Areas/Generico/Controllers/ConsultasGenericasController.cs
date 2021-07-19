using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Generico.Controllers
{
    [Area("Generico")]
    public class ConsultasGenericasController : ClientSessionControllerBase
    {

        private readonly IDiversosApp _app;
        private readonly IPagNetUser _user;
        private readonly IPagamentoApp _fecheCre;
        private readonly ICadastrosApp _cadastro;

        public ConsultasGenericasController(IDiversosApp app,
                                           IPagNetUser user,
                                           ICadastrosApp cadastro,
                                           IPagamentoApp fecheCre)
        {
            _app = app;
            _user = user;
            _fecheCre = fecheCre;
            _cadastro = cadastro;
        }
        public IActionResult Index()
        {
            var versaoAtual = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return View();
        }

        public IActionResult GetClassJs(string id, string subpasta, string Classe)
        {
            try
            {
                var CaminhoDefault = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "js", "Scripts");
                var versaoAtual = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                var CaminhoClasseJS = Path.Combine(CaminhoDefault, "versao_" + versaoAtual.ToString());

                if (!Directory.Exists(CaminhoClasseJS))
                {
                    DirectoryInfo dir = new DirectoryInfo(CaminhoDefault);
                    System.IO.FileInfo fi = new System.IO.FileInfo(CaminhoDefault);
                    var arquivos = new DirectoryInfo(CaminhoDefault).EnumerateFiles("*", SearchOption.AllDirectories);
                    //fi.MoveTo("caminho de destino\\" + fi.Name);

                    foreach (FileInfo file in arquivos)
                    {
                        if (!file.FullName.Contains("versao_"))
                        {
                            string caminhoArquivoOrigem = file.FullName;
                            string path = file.Name;
                            caminhoArquivoOrigem = caminhoArquivoOrigem.Replace(path, "");
                            string novoCaminho = caminhoArquivoOrigem.Replace(CaminhoDefault, CaminhoClasseJS);
                            Directory.CreateDirectory(novoCaminho);
                            System.IO.File.Copy(file.FullName, Path.Combine(novoCaminho, path), true);
                        }
                    }
                }

                string caminhoRetorno = Path.Combine(CaminhoClasseJS, id);
                if (!string.IsNullOrWhiteSpace(subpasta))
                {
                    caminhoRetorno = Path.Combine(caminhoRetorno, subpasta, Classe);
                }
                else
                {
                    caminhoRetorno = Path.Combine(caminhoRetorno, Classe);
                }

                return File(System.IO.File.ReadAllBytes(caminhoRetorno), "text/js");
            }
            catch (System.Exception ex)
            {
                throw;
            }
            
        }

        public JsonResult GetInstrucaoCobranca()
        {
            var Bancos = _app.GetInstrucaoCobranca();

            return Json(Bancos);
        }
        public JsonResult GetTiposOcorrenciasBoleto()
        {
            var Bancos = _app.GetTiposOcorrenciasBoleto();

            return Json(Bancos);
        }
        public JsonResult GetFormasLiquidacao()
        {
            var lista = _app.GetFormasLiquidacao();

            return Json(lista);
        }

        public async Task<JsonResult> BuscaEndereco(string cpf)
        {
            EnderecoVM endereco = new EnderecoVM();

            if (!string.IsNullOrWhiteSpace(cpf))
            {
                endereco = await _cadastro.RetornaEndereco(cpf);
            }

            return Json(endereco);
        }
        public JsonResult ListaJustificativaBaixaManual()
        {
            var lista = new object[][]
             {
                new object[] { "NAOINFORMADO", ""},
                new object[] { "CHEQUE", "PAGAMENTO ATRAVÉS DE CHEQUE"},
                new object[] { "COMPENSACAO", "PAGAMENTO ATRAVÉS DE COMPENSAÇÃO" },
                new object[] { "ANTECIPACAO", "ANTECIPAÇÃO" },
                new object[] { "OUTROS", "OUTROS"}
             };

            return Json(lista.ToList());
        }
        public JsonResult ListaJustificativaCancelamentoTitulo()
        {
            var lista = new object[][]
             {
                new object[] { "NAOINFORMADO", ""},
                new object[] { "CRIADO_INDEVIDAMENTE", "CRIADO INDEVIDAMENTE"},
                new object[] { "OUTROS", "OUTROS"}
             };

            return Json(lista.ToList());
        }
        public JsonResult CarregaListaRaizPlanoContas(int? id)
        {
            if (Convert.ToInt32(id) <= 0)
                id = _user.cod_empresa;

            var lista = _app.DDLPlanoContas((int)id);

            return Json(lista.ToList());
        }
        public JsonResult DDLPlanoContasPagamento(int? id)
        {
            if (Convert.ToInt32(id) <= 0)
                id = _user.cod_empresa;

            var lista = _app.DDLPlanoContasPagamento((int)id);

            return Json(lista.ToList());
        }
        public JsonResult DDLPlanoContasRecebimento(int? id)
        {
            if (Convert.ToInt32(id) <= 0)
                id = _user.cod_empresa;

            var lista = _app.DDLPlanoContasRecebimento((int)id);

            return Json(lista.ToList());
        }
        
    }
}