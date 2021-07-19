using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Application.Interface;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using PagNet.Interface.Areas.Configuracao.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Configuracao.Controllers
{
    [Area("Configuracao")]
    [Authorize]
    public class ConfigParamLeituraArqDFController : ClientSessionControllerBase
    {
        private readonly IPagNetUser _user;
        private readonly IAPIGestaoDescontoFolhaAppClient _descontoFolha;
        private readonly ICadastrosApp _cadastro;
        private readonly IAPIClienteAPP _APICliente;

        public ConfigParamLeituraArqDFController(IPagNetUser user,
                                                 ICadastrosApp cadastro,
                                                 IAPIClienteAPP APICliente,
                                                 IAPIGestaoDescontoFolhaAppClient descontoFolha)
        {
            _user = user;
            _descontoFolha = descontoFolha;
            _cadastro = cadastro;
            _APICliente = APICliente;
        }

        public IActionResult Index(int? id)
        {
            FiltroDescontoFolhaVM filtro = new FiltroDescontoFolhaVM();
            filtro.codigoCliente = Convert.ToInt32(id);

            var ModelAPI = _descontoFolha.BuscaConfiguracaoByCliente(filtro);
            var model = ConfigParamLeituraArquivoVM.ToView(ModelAPI);
            model.acessoAdmin = _user.isAdministrator;
            if (model.codigoEmpresa == null)
            {
                var emp = _cadastro.RetornaDadosEmpresaByID(_user.cod_empresa);
                model.codigoEmpresa = emp.CODEMPRESA.ToString();
                model.nmEmpresa = emp.NMFANTASIA;
            }
            return View(model);
        }

        public JsonResult CarregaTipoExtensao()
        {

            var lista = new object[][]
             {
                new object[] { "CSV", "CSV"},
                new object[] { "TXT", "TXT" },
                new object[] { "XLS", "XLS" },
                new object[] { "XLSX", "XLSX"}
             };

            return Json(lista.ToList());
        }
        public JsonResult CarregaFormaLerArquivo()
        {
            var lista = new object[][]
             {
                new object[] { "1", "No Arquivo de retorno possui apenas usuários que NÃO CONSEGUIRAM descontar o valor"},
                new object[] { "2", "No Arquivo de retorno possui apenas usuários que CONSEGUIRAM descontar o valor" },
                new object[] { "3", "No Arquivo de retorno possui todos os usuários e os que eles conseguiram descontar o campo de valor está vazio" }
             };

            return Json(lista.ToList());
        }

        [HttpPost]
        public JsonResult BuscaCliente(int codigoCliente)
        {
            FiltroDescontoFolhaVM filtro = new FiltroDescontoFolhaVM();
            filtro.codigoCliente = codigoCliente;

            var model = _descontoFolha.BuscaConfiguracaoByCliente(filtro);

            return Json(model);
        }
        public ActionResult ConsultaTodosClientes(string codempresa)
        {
            try
            {
                if (Convert.ToInt32(codempresa) <= 0)
                    codempresa = _user.cod_empresa.ToString();

                var dados =  _APICliente.ConsultaTodosCliente(Convert.ToInt32(codempresa), "").Result;
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
        [HttpPost]
        public async Task<IActionResult> SalvarParamLeituraArquivo(ConfigParamLeituraArquivoVM model)
        {
            try
            {
                model.extensaoArquivoRET = model.extensaoArquivo;
                model.codigoFormaVerificacao = Convert.ToInt32(model.codigoFormaVerificacaoArq);
                var Resultado = _descontoFolha.SalvarParamLeituraArquivo(model).FirstOrDefault();

                return Json(new { success = Resultado.Key, responseText = Resultado.Value });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }

        }

    }
}