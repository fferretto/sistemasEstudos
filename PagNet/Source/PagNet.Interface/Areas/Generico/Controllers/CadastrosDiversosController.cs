using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Generico.Controllers
{
    [Area("Generico")]
    public class CadastrosDiversosController : ClientSessionControllerBase
    {

        private readonly IAPIContaCorrente _APIContaCorrente;
        private readonly IAPIFavorecido _apiFavorecido;
        private readonly IDiversosApp _app;
        private readonly IPagNetUser _user;
        private readonly IPagamentoApp _fecheCre;
        private readonly ICadastrosApp _cadastro;

        public CadastrosDiversosController(IDiversosApp app,
                                           IAPIContaCorrente APIContaCorrente,
                                           IAPIFavorecido apiFavorecido,
                                           IPagNetUser user,
                                           ICadastrosApp cadastro,
                                           IPagamentoApp fecheCre)
        {
            _app = app;
            _APIContaCorrente = APIContaCorrente;
            _apiFavorecido = apiFavorecido;
            _user = user;
            _fecheCre = fecheCre;
            _cadastro = cadastro;
        }
        public IActionResult Index()
        {
            return View();
        }
        //Carrega o dropdown com a operadora
        public async Task<JsonResult> GetOperadora()
        {
            var lista = _app.GetOperadoras();

            return Json(lista);
        }
        //Carrega o dropdown com os bancos
        public async Task<JsonResult> GetBanco()
        {
            try
            {
                var Bancos = _APIContaCorrente.GetBanco();

                return Json(Bancos);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return Json("");
            }
        }
        //Carrega o dropdown com a sub rede
        public async Task<JsonResult> GetSubRede()
        {
            var lista = _app.GetSubRede();

            return Json(lista);
        }
        //Carrega o dropdown com as empresaas cadastradas
        public async Task<JsonResult> GetEmpresa()
        {
            var codEmpresa = 0;
            if (!_user.isAdministrator) codEmpresa = _user.cod_empresa;

             var lista = _app.GetEmpresa(codEmpresa);

            return Json(lista);
        }
        //Carrega o dropdown com todas as contas correntes ativas
        public async Task<JsonResult> GetContaCorrente(int? id)
        {
            int codigoEmpresa = _user.cod_empresa;

            if (Convert.ToInt32(id) > 0)
            {
                codigoEmpresa = (int)id;
            }
            var dados = _APIContaCorrente.GetContaCorrente(codigoEmpresa);

            return Json(dados);
        }
       
        
        //Carrega o dropdown com a conta corrente configurada para pagamento
        public async Task<JsonResult> GetContaCorrentePagamento(int? id)
        {
            int codigoEmpresa = _user.cod_empresa;

            if (Convert.ToInt32(id) > 0)
            {
                codigoEmpresa = (int)id;
            }
            var dados = _APIContaCorrente.GetContaCorrentePagamento(codigoEmpresa);

            return Json(dados);
        }
        //Carrega o dropdown com a conta corrente configurada para geração de boleto
        public async Task<JsonResult> GetContaCorrenteBoleto(int? id)
        {
            int codigoEmpresa = _user.cod_empresa;

            if (Convert.ToInt32(id) > 0)
            {
                codigoEmpresa = (int)id;
            }
            var dados = _APIContaCorrente.GetContaCorrenteBoleto(codigoEmpresa);

            return Json(dados);
        }
        public JsonResult GetBancoByContaCorrente(int codContaCorrente)
        {
            var vm = _APIContaCorrente.GetBancoByCodContaCorrente(codContaCorrente);

            return Json(new { success = true, codBanco = vm.CODBANCO, nmBanco = vm.NMBANCO });

        }
        public JsonResult BuscaBanco(string filtro)
        {
            FiltroTitulosPagamentoVM model;


                model = _fecheCre.FindBancoByID(Convert.ToInt32(filtro));


            var retorno = model.filtroCodBanco + "/" + model.FiltroNmBanco + "/" + model.codBanco;

            return Json(retorno);
        }
        public JsonResult BuscaCredenciado(string filtro, int codigoEmpresa)
        {
            APIFavorecidoVM model = new APIFavorecidoVM(); ;

            if (filtro.Length <= 10)
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByID(Convert.ToInt32(filtro), Convert.ToInt32(codigoEmpresa)));
            }
            else
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByCPFCNPJ(filtro, Convert.ToInt32(codigoEmpresa)));
            }

            var retorno = model.CPFCNPJ + "/" + model.nomeFavorecido + "/" + model.codigoFavorecido;

            return Json(retorno);
        }

        public JsonResult BuscaFavorecido(string filtro, int codigoEmpresa)
        {
            APIFavorecidoVM model;

            if (filtro.Length <= 6)
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByCodCen(Convert.ToInt32(filtro), codigoEmpresa));
            }
            else if(filtro.Length >= 7 && filtro.Length <= 10)
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByID(Convert.ToInt32(filtro), codigoEmpresa));
            }
            else
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByCPFCNPJ(filtro, codigoEmpresa));
            }

            var retorno = model.CPFCNPJ + "]" + model.nomeFavorecido + "]" + model.codigoFavorecido;

            return Json(retorno);
        }
    }
}

