using PagNet.Bld.AntecipPGTO.Abstraction.Interface.Model;
using PagNet.Bld.AntecipPGTO.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PagNet.Bld.AntecipPGTO.Abstraction.Interface
{
    public interface IAntecipacaoApp
    {
        RegraNegocioPagamentoVm BuscaRegraPagamentoByID(int codRegra);
        RegraNegocioPagamentoVm BuscaRegraAtivaPagamento();
        List<AntecipacaoPGTOModel> ListaTitulosValidosAntecipacao(IFiltroAntecipacaoModel filtro);
        AntecipacaoPGTOModel CalculaTaxaAntecipacaoPGTO(IFiltroAntecipacaoModel filtro);
        bool SalvarAntecipacaoPGTO(IFiltroAntecipacaoModel titulo);


    }
}
