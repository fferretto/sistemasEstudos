using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Model;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasPagar.Controllers
{
    [Area("ContasPagar")]
    [Authorize]
    public class ConsultarTitulosController : ClientSessionControllerBase
    {
        private readonly IPagamentoApp _pagamento;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ICadastrosApp _cadastro;
        private readonly IAPIAntecipacaoPGTO _antecipacaoClient;
        private readonly IAPIFavorecido _apiFavorecido;

        public ConsultarTitulosController(IPagamentoApp pagamento,
                                          IPagNetUser user,
                                          ICadastrosApp cadastro,
                                          IDiversosApp diversos,
                                          IAPIFavorecido apiFavorecido,
                                          IAPIAntecipacaoPGTO antecipacaoClient)
        {
            _pagamento = pagamento;
            _user = user;
            _diversos = diversos;
            _cadastro = cadastro;
            _antecipacaoClient = antecipacaoClient;
            _apiFavorecido = apiFavorecido;
        }

        public IActionResult Index(string id)
        {

            ConsultaFechamentoCredVm model = new ConsultaFechamentoCredVm();


            if (!string.IsNullOrWhiteSpace(id))
            {
                var param = id.Split(";");
                model.dtInicio = param[0];
                model.dtFim = param[1];
                model.codFavorecido = Convert.ToInt32(param[3]);
            }
            else
            {
                DateTime data = DateTime.Now;

                model.dtInicio = data.ToShortDateString();
                model.dtFim = data.ToShortDateString();
            }

            var empresa = _cadastro.RetornaDadosEmpresaByID(_user.cod_empresa);
            model.codEmpresa = empresa.CODEMPRESA.ToString();
            model.nmEmpresa = empresa.NMFANTASIA.ToString();

            model.codStatus = "TODOS";
            model.nmStatus = "TODOS";

            model.acessoAdmin = _user.isAdministrator;

            return View(model);
        }
        public async Task<ActionResult> CarregaGrigTitulos(ConsultaFechamentoCredVm model)
        {
            try
            {

                if (Convert.ToInt32(model.codEmpresa) < 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                if (model.codStatus == "TODOS" || model.codStatus == "-1" || model.codStatus == null)
                    model.codStatus = "";

                var Dados = await _pagamento.ConsultaTransacaoPagamento(model);
                decimal valorTotal = Dados.Sum(x => Convert.ToDecimal(x.ValorTotalPagar));
                decimal TotalTaxas = Dados.Sum(x => Convert.ToDecimal(x.VALORTAXA.Replace("R$ ", "")));
                decimal TotalBruto = Dados.Sum(x => Convert.ToDecimal(x.VALORBRUTO.Replace("R$ ", "")));
                for (int i = 0; i < Dados.Count; i++)
                {
                    Dados[i].ValorTotalPagar = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valorTotal).Replace("R$", "");
                    Dados[i].ValorTotalBruto = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", TotalBruto).Replace("R$", "");
                    Dados[i].ValorTotalTaxas = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", TotalTaxas).Replace("R$", "");
                }


                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                return PartialView(null);
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
        public JsonResult BuscaFavorecido(string filtro, int? codEmpresa)
        {
            APIFavorecidoVM model = new APIFavorecidoVM();

            if (Convert.ToInt32(codEmpresa) <= 0)
            {
                codEmpresa = _user.cod_empresa;
            }

            if (filtro.Length <= 10)
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByID(Convert.ToInt32(filtro), Convert.ToInt32(codEmpresa)));
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
        public async Task<ActionResult> ConsultaLog(string CODTITULO)
        {
            try
            {
                var dados = await _pagamento.ConsultaLog(Convert.ToInt32(CODTITULO));

                return PartialView(dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return PartialView(null);
            }
        }
        public async Task<ActionResult> DetalharTaxas(string codtitulo)
        {
            try
            {
                var dados = await _pagamento.ListarTaxasTitulo(Convert.ToInt32(codtitulo));

                return PartialView(dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return PartialView(null);
            }
        }
        public JsonResult CarregaListaAcoes()
        {

            var lista = new object[][]
             {
                new object[] { "RealizarAcao", "CONSULTARLOG", "Consultar Log"},
                new object[] { "RealizarAcao", "EDITARTITULO", "Editar Titulo" },
                new object[] { "RealizarAcao", "AJUSTARVALORTITULO", "Ajustar Valor" },
                new object[] { "RealizarAcao", "ANTECIPARPGTO", "Antecipar PGTO" },
                new object[] { "RealizarAcao", "BAIXAMANUAL", "Baixar Manualmente"},
                new object[] { "RealizarAcao", "DESVINCULARTITULO", "Desvincular Titulo do Arquivo"},
                new object[] { "RealizarAcao", "CANCELARTITULO", "Cancelar Título" }
             };
            

            return Json(lista.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> BaixaManual(FiltroBorderoPagVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                model.codUsuario = _user.cod_usu;

                var result = await _pagamento.BaixaManualByID(model);

                return Json(new { success = true, responseText = result });
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return Json(ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> CancelarTitulo(FiltroBorderoPagVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                model.codUsuario = _user.cod_usu;

                var result = await _pagamento.CancelarTitulo(model);

                return Json(new { success = true, responseText = result.FirstOrDefault().Value });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Falha ao cancelar o título. Favor contactar o suporte!" });
            }

        }
        public ActionResult JustificativaDesvincularTitulo()
        {
            JustificativaVm vm = new JustificativaVm();

            return PartialView(vm);

        }
        public ActionResult JustificativaBaixaManual()
        {
            JustificativaVm vm = new JustificativaVm();

            return PartialView(vm);

        }
        public ActionResult JustificativaCancelarTitulo()
        {
            JustificativaVm vm = new JustificativaVm();

            return PartialView(vm);

        }
        public async Task<ActionResult> EdicaoTitulo(int CODTITULO)
        {
            try
            {
                var dados = await _pagamento.RetornaDadosTitulo(CODTITULO);

                return PartialView(dados);
            }
            catch (Exception)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                DadosTituloVm VmVazio = new DadosTituloVm();
                return PartialView(VmVazio);
            }
        }
        public async Task<ActionResult> AjustarValorTitulo(int CODTITULO)
        {
            try
            {
                AjustarValorTitulo model = new AjustarValorTitulo();

                var dados = await _pagamento.RetornaDadosTitulo(CODTITULO);
                model.codigoTituloAjusteValor = dados.CODIGOTITULO;
                model.nomeFavorecido = dados.NMFAVORECIDO;
                model.cpfCnpj = dados.CNPJ;
                model.codigoFavorecido = dados.CODFAVORECIDO;
                model.ValorAtual = dados.VALLIQ;
                model.NovoValor = dados.VALLIQ;
                model.valorConcedido = "0,00";
                model.Descricao = "";
                model.Desconto = true;

                return PartialView(model);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                DadosTituloVm VmVazio = new DadosTituloVm();
                return PartialView(VmVazio);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SalvarAjusteValor(AjustarValorTitulo model)
        {
            try
            {
                model.codigoUsuarioAjusteValor = _user.cod_usu;

                var result = await _pagamento.SalvarAjusteValorTitulo(model);

                return Json(new { success = result.FirstOrDefault().Key, responseText = result.FirstOrDefault().Value });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Falha ao ajustar o valor do título. Favor contactar o suporte!" });
            }
        }
        public ActionResult AnteciparPGTO(int CODTITULO)
        {
            try
            {
                FiltroAntecipacaoPGTOVm model = new FiltroAntecipacaoPGTOVm();
                model.dtAntecipacao = DateTime.Now.AddDays(1).ToShortDateString();
                model.codigoTitulo = CODTITULO.ToString();
                model.acessoAdmin = _user.isAdministrator;

                return PartialView(model);
            }
            catch (Exception)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                DadosTituloVm VmVazio = new DadosTituloVm();
                return PartialView(VmVazio);
            }
        }
        public async Task<ActionResult> CalculaTaxaAntecipacaoPGTO(FiltroAntecipacaoPGTOVm model)
        {
            try
            {
                APIFiltroAntecipacaoVM filtro = new APIFiltroAntecipacaoVM();
                filtro.codigoTitulo = Convert.ToInt32(model.codigoTitulo);
                filtro.NovaDataPGTO = Convert.ToDateTime(model.dtAntecipacao);

                var ApiResult = _antecipacaoClient.CalculaTaxaAntecipacaoPGTO(filtro);

                return PartialView(ApiResult);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = msg;
                AntecipacaoPGTOVm Dados = new AntecipacaoPGTOVm();
                return PartialView(Dados);
            }

        }
        public async Task<IActionResult> SalvarAntecipacaoPGTO(FiltroAntecipacaoPGTOVm model)
        {
            try
            {
                APIFiltroAntecipacaoVM filtro = new APIFiltroAntecipacaoVM();
                filtro.codigoTitulo = Convert.ToInt32(model.codigoTitulo);
                filtro.NovaDataPGTO = Convert.ToDateTime(model.dtAntecipacao);

                var ApiResult = _antecipacaoClient.SalvarAntecipacaoPGTO(filtro);

                return Json(new { success = ApiResult, responseText = "Antecipação realizada com sucesso." });
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return Json(ex.Message);
            }

        }
        public JsonResult GetJustificativaEditTitulo()
        {

            var lista = new object[][]
             {
                new object[] { "NAOINFORMADO", ""},
                new object[] { "TITULO_VENCIDO", "TITULO VENCIDO"},
                new object[] { "DADOS_BANCARIOS", "DADOS BANCÁRIOS INCORRETOS" },
                new object[] { "CONCEDER_DESCONTO", "CONCEDER DESCONTO" },
                new object[] { "OUTROS", "OUTROS"}
             };

            return Json(lista.ToList());
        }
        public JsonResult GetJustificativaDesvincularTitulo()
        {

            var lista = new object[][]
             {
                new object[] { "NAOINFORMADO", ""},
                new object[] { "NAO_AUTORIZADO", "TÍTULO NÃO AUTORIZADO PARA PAGAMENTO"},
                new object[] { "OUTROS", "OUTROS"}
             };

            return Json(lista.ToList());
        }
        public async Task<JsonResult> StatusTransacao()
        {

            var lista = new object[][]
             {
                new object[] { "TODOS", "TODOS"},
                new object[] { "EM_ABERTO", "EM ABERTO"},
                new object[] { "EM_BORDERO", "EM BORDERO" },
                new object[] { "AGUARDANDO_ARQUIVO_RETORNO", "AGUARDANDO ARQUIVO RETORNO" },
                new object[] { "BAIXADO", "BAIXADO"},
                new object[] { "BAIXADO_MANUALMENTE", "BAIXADO MANUALMENTE" },
                new object[] { "RECUSADO", "RECUSADO" }
             };

            return Json(lista.ToList());
        }
        [HttpPost]
        public IActionResult SalvaEdicaoTitulo(DadosTituloVm model)
        {
            try
            {
                model.CODUSUARIO = _user.cod_usu;
                model.CODOPERADORA = _user.cod_ope;

                if (Convert.ToInt32(model.CODEMPRESA) <= 0)
                    model.CODEMPRESA = _user.cod_empresa;

                var result = _pagamento.SalvaEdicaoTitulo(model).Result;

                return Json(new { success = true, responseText = result.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult DesvinculaTitulo(FiltroBorderoPagVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                model.codUsuario = _user.cod_usu;

                var TextoRetorno = _pagamento.DesvinculaTitulo(model).Result;

                return Json(new { success = true, responseText = TextoRetorno });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }

    }
}