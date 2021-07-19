using PagNet.Api.Service.Model;
using PagNet.Bld.AntecipPGTO.Abstraction.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface
{
    public interface IAPIAntecipacaoPGTO
    {
        APIRegraNegocioPGTOResult BuscaRegraAtivaPagamento();
        APIRegraNegocioPGTOResult BuscaRegraPagamentoByID(int codRegra);
        APIAntecipacaoPGTOResult CalculaTaxaAntecipacaoPGTO(IFiltroAntecipacaoPGTOModel filtro);
        IList<APIAntecipacaoPGTOResult> ListaTitulosValidosAntecipacao(IFiltroAntecipacaoPGTOModel filtro);
        bool SalvarAntecipacaoPGTO(IFiltroAntecipacaoPGTOModel titulo);
    }
}
