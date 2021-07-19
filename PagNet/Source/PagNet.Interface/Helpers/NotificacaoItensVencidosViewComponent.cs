using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Interface.Helpers
{
    [ViewComponent(Name = "NotificacaoItensVencidos")]
    public class NotificacaoItensVencidosViewComponent : ViewComponent
    {
        private readonly IPagNetUser _user;
        private readonly IRecebimentoApp _recebimento;
        private readonly IPagamentoApp _pagamento;
        private readonly IDiversosApp _diversos;


        public NotificacaoItensVencidosViewComponent(IRecebimentoApp recebimento,
                                                      IPagamentoApp pag,
                                                      IDiversosApp diversos,
                                                      IPagNetUser user)
        {
            _user = user;
            _pagamento = pag;
            _recebimento = recebimento;
            _diversos = diversos;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                int quantidadeAlertas = 0;

                var DadosTitulos = _pagamento.ListaTitulosNaoLiquidado(_user.cod_empresa).Result;
                var DadosFaturamento = _recebimento.ListaFaturamentoNaoLiquidado(_user.cod_empresa).Result;

                var TotalTitulosVencido = DadosTitulos.Where(x => x.STATUS != "AGUARDANDO ARQUIVO RETORNO").Count();
                var TotalTitulosPendBaixa = DadosTitulos.Where(x => x.STATUS == "AGUARDANDO ARQUIVO RETORNO").Count();
                var TotalFaturamentoPendRegistro = DadosFaturamento.Where(x => x.Status == "PENDENTE REGISTRO").Count();
                var TotalFaturamentoVencido = DadosFaturamento.Where(x => x.Status != "PENDENTE REGISTRO" || x.Status != "REGISTRADO").Count();
                var TotalFaturamentoPendBaixa = DadosFaturamento.Where(x => x.Status == "REGISTRADO").Count();

                AlertaNotificacaoPagRecebVM model = new AlertaNotificacaoPagRecebVM();
                model.qtTotalAlerta = DadosTitulos.Count();

                ListaIAlertasVM itemAlerta = new ListaIAlertasVM();
                //Títulos Vencidos
                if (TotalTitulosVencido > 0)
                {
                    itemAlerta = new ListaIAlertasVM();
                    itemAlerta.Descricao = "Existem " + TotalTitulosVencido + " Títulos Vencidos";
                    itemAlerta.Quantidade = TotalTitulosVencido;
                    itemAlerta.linkAcesso = "FuncTotalTitulosVencido();";
                    model.listaAlertas.Add(itemAlerta);
                    quantidadeAlertas += 1;
                }

                //Títulos Aguardando Arquivo de Baixa
                if (TotalTitulosPendBaixa > 0)
                {
                    itemAlerta = new ListaIAlertasVM();
                    itemAlerta.Descricao = "Existem " + TotalTitulosPendBaixa + " Títulos Aguardando Arquivo de Baixa";
                    itemAlerta.Quantidade = TotalTitulosPendBaixa;
                    itemAlerta.linkAcesso = "FuncTotalTitulosPendBaixa();";
                    model.listaAlertas.Add(itemAlerta);

                    quantidadeAlertas += 1;
                }
                //Faturamentos Vencidos
                if (TotalFaturamentoVencido > 0)
                {
                    itemAlerta = new ListaIAlertasVM();
                    itemAlerta.Descricao = "Existem " + TotalFaturamentoVencido + " Faturamentos Vencidos";
                    itemAlerta.Quantidade = TotalFaturamentoVencido;
                    itemAlerta.linkAcesso = "FuncTotalFaturamentoVencido();";
                    model.listaAlertas.Add(itemAlerta);
                    quantidadeAlertas += 1;
                }

                //Faturamentos Aguardando Registro Junto ao Banco
                if (TotalFaturamentoPendRegistro > 0)
                {
                    itemAlerta = new ListaIAlertasVM();
                    itemAlerta.Descricao = "Existem " + TotalFaturamentoPendRegistro + " Faturamentos Aguardando Registro Junto ao Banco";
                    itemAlerta.Quantidade = TotalFaturamentoPendRegistro;
                    itemAlerta.linkAcesso = "FuncTotalFaturamentoPendRegistro();";
                    model.listaAlertas.Add(itemAlerta);
                    quantidadeAlertas += 1;
                }

                //Faturamentos Aguardando Arquivo de Liquidação
                if (TotalFaturamentoPendBaixa > 0)
                {
                    itemAlerta = new ListaIAlertasVM();
                    itemAlerta.Descricao = "Existem " + TotalFaturamentoPendBaixa + " Faturamentos Aguardando Arquivo de Liquidação";
                    itemAlerta.Quantidade = TotalFaturamentoPendBaixa;
                    itemAlerta.linkAcesso = "FuncTotalFaturamentoPendBaixa();";
                    model.listaAlertas.Add(itemAlerta);
                    quantidadeAlertas += 1;
                }

                model.qtTotalAlerta = quantidadeAlertas;

                return View("~/Views/Shared/NotificacaoItensVencidos.cshtml", model);
            }
            catch (System.Exception ex)
            {

                throw;
            }
            
        }
    }
}
