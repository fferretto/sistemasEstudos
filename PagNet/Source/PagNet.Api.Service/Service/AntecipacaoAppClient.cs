using Microsoft.AspNetCore.Http;
using PagNet.Api.Service.Helpers;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Model;
using PagNet.Bld.AntecipPGTO.Abstraction.Interface.Model;
using System;
using System.Collections.Generic;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Api.Service.Service
{
    public class AntecipacaoAppClient : ApiClientBase, IAPIAntecipacaoPGTO
    {
        public AntecipacaoAppClient(IHttpContextAccessor contextAccessor, ITokensRepository tokensRepository)
            //: base("https://localhost:44370/", contextAccessor, tokensRepository)
            : base("AntecipacaoPGTO/", contextAccessor, tokensRepository)
        { }

        public APIRegraNegocioPGTOResult BuscaRegraAtivaPagamento()
        {
            throw new NotImplementedException();
        }

        public APIRegraNegocioPGTOResult BuscaRegraPagamentoByID(int codRegra)
        {
            throw new NotImplementedException();
        }

        public APIAntecipacaoPGTOResult CalculaTaxaAntecipacaoPGTO(IFiltroAntecipacaoPGTOModel filtro)
        {
            var ApiResult = ExecutaPost<AntecipacaoPGTOModel, IFiltroAntecipacaoPGTOModel>("CalculaTaxaAntecipacaoPGTO", filtro).Result;

            return APIAntecipacaoPGTOResult.ToView(ApiResult);
        }

        public IList<APIAntecipacaoPGTOResult> ListaTitulosValidosAntecipacao(IFiltroAntecipacaoPGTOModel filtro)
        {
            var ApiResult = ExecutaPost<List<AntecipacaoPGTOModel>, IFiltroAntecipacaoPGTOModel>("ListaTitulosParaAntecipacao", filtro).Result;

            return APIAntecipacaoPGTOResult.ToListView(ApiResult);
        }

        public bool SalvarAntecipacaoPGTO(IFiltroAntecipacaoPGTOModel filtro)
        {
            return ExecutaPost<bool, IFiltroAntecipacaoPGTOModel>("SalvarAntecipacaoPGTO", filtro).Result;
        }
    }
}
