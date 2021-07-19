using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using Rotativa.AspNetCore.Options;
using Rotativa.AspNetCore;
using Telenet.AspNetCore.Mvc.Authentication;
using System;
using PagNet.Api.Service.Interface;
using PagNet.Interface.Areas.Relatorios.Models;
using PagNet.Api.Service.Model;

namespace PagNet.Interface.Areas.Generico.Controllers
{
    [Area("Generico")]
    public class VisualizadorRelatorioPDFController : ClientSessionControllerBase
    {
        private readonly IAPIRelatorioApp _relatorio;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;

        public VisualizadorRelatorioPDFController(IAPIRelatorioApp relatorio,
                                                  IDiversosApp diversos,
                                                  IPagNetUser user)
        {
            _relatorio = relatorio;
            _diversos = diversos;
            _user = user;
        }
        public IActionResult RelPDF(string id)
        {
            RelatoriosModel model = new RelatoriosModel();

            var parametros = id.Split('&');
            foreach (var par in parametros)
            {
                var parAux = par.Split(':');
                if (parAux[0] == "nomproc")
                {
                    model.nmProc = parAux[1];
                }
                else if (parAux[0] == "nmTela")
                {
                    model.nmTela = parAux[1];                    
                }
                else
                {
                    APIParametrosRelatorioModel aux = new APIParametrosRelatorioModel();
                    aux.NOMPAR = parAux[0];
                    aux.VALCAMPO = parAux[1];
                    aux.TIPO = parAux[2];

                    if (aux.NOMPAR == "CODEMPRESA" && Convert.ToInt32(aux.VALCAMPO) <= 0)
                    {
                        aux.VALCAMPO = _user.cod_empresa.ToString();
                    }

                    model.listaCampos.Add(aux);
                }
            }

            var dadosRel = _relatorio.RelatorioPDF(model);

            dadosRel.nmRelatorio = model.nmTela;

            var pdf = new ViewAsPdf("RelPDF", dadosRel)
            {
                Model = dadosRel,
                PageSize = Size.A4,
                IsLowQuality = true,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(2, 6, 6, 5)
            };

            return pdf;
            //return View(dadosRel);
        }

        public IActionResult DetalhamentoFaturaReembolso(string id, string nomePDF, string caminho)
        {
            DetalhamentoFaturaReembolsoVm model = new DetalhamentoFaturaReembolsoVm();

            model.nroDocumento = "000236";
            model.datEmissao = "01/11/2019";
            model.datVencimento = "20/11/2019";
            model.vlTotal = "R$ 1.563,45";

            //Dados Credor
            model.Credor = "Luiz Felipe Ferretto";
            model.CNPJCredor = "079.575.066-82";
            model.CEPCredor = "31550-430";
            model.EnderecoCredor = "Rua alguma coisa";
            model.nroCredor = "532";
            model.ComplementoCredor = "Apto 103";
            model.BairroCredor = "Bairro Sei lá";
            model.CidadeCredor = "Cidade Quaquer";
            model.EstadoCredor = "MG";
            model.TelefoneCredor = "(31) 98564-4562";

            //Dados Devedor
            model.Devedor = "Rodolfo que Deve Todo Mundo";
            model.CNPJDevedor = "03.337.423/0001-90";
            model.CEPDevedor = "31565-524";
            model.EnderecoDevedor = "Rua Rodolfo";
            model.nroDevedor = "666";
            model.ComplementoDevedor = "Sala 5";
            model.BairroDevedor = "Bairro Rodolfo";
            model.CidadeDevedor = "Cidade Rodolfo";
            model.EstadoDevedor = "SP";
            model.TelefoneDevedor = "(11) 3654-4562";

            DetalhamentoValoresCobradosFaturaVm detalhe = new DetalhamentoValoresCobradosFaturaVm();
            detalhe.Descricao = "Compras";
            detalhe.Valor = "R$ 568,12";
            model.Detalhamento.Add(detalhe);

            detalhe = new DetalhamentoValoresCobradosFaturaVm();
            detalhe.Descricao = "Taxa";
            detalhe.Valor = "R$ 1.000,00";
            model.Detalhamento.Add(detalhe);


            var pdf = new ViewAsPdf("DetalhamentoFaturaReembolso", model)
            {
                Model = model,
                PageSize = Size.A4,
                IsLowQuality = true,

                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(2, 6, 6, 5),
                SaveOnServerPath = caminho // preecha caminho com o diretório a ser salvo
            };

            return pdf;
            //return View(model);
        }
        public void GravaPDF(string codEmissaoBoleto, string nomeArquivoPDF, string caminhoArquivo)
        {
            RedirectToAction("DetalhamentoFaturaReembolso", new { id = codEmissaoBoleto, nomePDF = nomeArquivoPDF, caminho = caminhoArquivo });
        }
    }
}