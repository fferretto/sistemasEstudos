using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Helpers;
using Telenet.AspNetCore.Mvc.Authentication;
using PagNet.Application.Helpers;
using PagNet.Api.Service.Interface;
using PagNet.Interface.Areas.Cadastros.Models;

namespace PagNet.Interface.Areas.ContasPagar.Controllers
{
    [Area("ContasPagar")]
    [Authorize]
    public class GeraBorderoController : ClientSessionControllerBase
    {
        private readonly IAPIContaCorrente _APIContaCorrente;
        private readonly IPagamentoApp _pagamento;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ICadastrosApp _cadastro;
        private readonly IAPIFavorecido _apiFavorecido;

        public GeraBorderoController(IPagamentoApp pagamento,
                                                IAPIContaCorrente APIContaCorrente,
                                                ICadastrosApp cadastro,
                                                IPagNetUser user,
                                                IAPIFavorecido apiFavorecido,
                                                IDiversosApp diversos)
        {
            _pagamento = pagamento;
            _APIContaCorrente = APIContaCorrente;
            _user = user;
            _diversos = diversos;
            _cadastro = cadastro;
            _apiFavorecido = apiFavorecido;
        }
        public async Task<IActionResult> Index(string id)
        {
            FiltroTitulosPagamentoVM model = new FiltroTitulosPagamentoVM();

            if (!string.IsNullOrWhiteSpace(id))
            {
                var param = id.Split(';');
                model.dtInicio = param[0];
                model.dtFim = param[1];
                model.codFavorecido = Convert.ToInt32(param[3]);
                model.CaminhoArquivoDownload = param[4];
            }
            else
            {
                DateTime data = DateTime.Now;

                model.dtInicio = data.ToShortDateString();
                model.dtFim = data.ToShortDateString();
            }


            var empresa = _cadastro.RetornaDadosEmpresaByID(_user.cod_empresa);
            model.codEmpresa = empresa.CODEMPRESA.ToString();
            model.nmEmpresa = empresa.NMFANTASIA;

            //var model = _pagamento.RetornaDadosInicio(id, _user.cod_empresa);
            model.acessoAdmin = _user.isAdministrator;

            return View(model);
        }
        public JsonResult BuscaBanco(string filtro)
        {
            FiltroTitulosPagamentoVM model;

            model = _pagamento.FindBancoByID(Convert.ToInt32(filtro));
        
            var retorno = model.filtroCodBanco + "/" + model.FiltroNmBanco + "/" + model.codBanco;

            return Json(retorno);
        }
        public JsonResult RetornaCodigoBancoByCC(string id)
        {
            var banco = _APIContaCorrente.GetBancoByCodContaCorrente(Convert.ToInt32(id));

            return Json(banco.CODBANCO);
        }
        public ActionResult JustificativaBaixaManual()
        {
            JustificativaVm vm = new JustificativaVm();
            
            return PartialView(vm);

        }
        [HttpPost]
        [RequestFormSizeLimit(valueCountLimit: 9000)]
        public async Task<IActionResult> BaixaManual(FiltroBorderoPagVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                model.codUsuario = _user.cod_usu;

                var result = _pagamento.PagamentoManualAsync(model);

                return Json(new { success = true, responseText = result.Result });
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return Json(ex.Message);
            }

        }
        public JsonResult BuscaFavorecido(string filtro, int? codEmpresa)
        {

            filtro = Geral.RemoveCaracteres(filtro);

            if (Convert.ToInt32(codEmpresa) <= 0)
            {
                codEmpresa = _user.cod_empresa;
            }
            APIFavorecidoVM model = new APIFavorecidoVM();
            
            if(!Geral.IsNumeric(filtro))
            {
                return Json(filtro + "/O filtro informado é inválido./0");
            }

            if (filtro.Length <= 10)
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByID(Convert.ToInt32(filtro), Convert.ToInt32(codEmpresa)));
            }
            else
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByCPFCNPJ(filtro, Convert.ToInt32(codEmpresa)));
            }

            var retorno = model.codigoFavorecido + "/" + model.nomeFavorecido + "/" + model.codigoFavorecido;

            if (model.codigoEmpresa != "" && model.codigoEmpresa != codEmpresa.ToString())
            {
                retorno = filtro + "/Favorecido não encontrado/0";
            }

            return Json(retorno);
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
        public async Task<ActionResult> ListaTitulos(FiltroTitulosPagamentoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                var Dados = _pagamento.CarregaGridTitulos(model);

                Dados.codEmpresa = model.codEmpresa;
                Dados.codigoBanco = model.codBanco;
                Dados.codigoFormaPGTO = model.CodFormaPagamento;

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }
        public async Task<ActionResult> ConsultaTitulosPGTO(int codEmpresa, int codFavorecido, string datPGTO)
        {
            try
            {
                if (Convert.ToInt32(codEmpresa) <= 0)
                    codEmpresa = _user.cod_empresa;

                
                DateTime dataPagamento = Convert.ToDateTime(datPGTO);

                var Dados = await _pagamento.ConsultaTitulosByFavorecidoDatPGTO(codEmpresa, codFavorecido, dataPagamento);


                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }
        [HttpPost]
        [RequestFormSizeLimit(valueCountLimit: 9000)]
        public async Task<IActionResult> SalvaBordero(FiltroBorderoPagVM model)
        {
            try
            {

                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                model.codUsuario = _user.cod_usu;
                model.codOpe = _user.cod_ope;

                var result = await _pagamento.SalvaBordero(model);
                
                return Json(new {success = true, responseText = result});

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }

        }
        public JsonResult GetJustificativaBaixaManual()
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

    }
}