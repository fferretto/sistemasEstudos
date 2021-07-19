using Microsoft.AspNetCore.Http;
using PagNet.Api.Service.Helpers;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Api.Service.Service
{
    public class APICobrancaBordero : ApiClientBase, IAPICobrancaBordero
    {
        public APICobrancaBordero(IHttpContextAccessor contextAccessor, ITokensRepository tokensRepository)
        //: base("https://localhost:44388/", contextAccessor, tokensRepository)
        : base("CobrancaBordero/", contextAccessor, tokensRepository)
        { }
        
        public APIDadosBoletoModel BuscaFaturas(IAPIFiltroBorderoModel filtro)
        {
            var nomeBanco = ExecutaPost<APIDadosBoletoModel, IAPIFiltroBorderoModel>("BuscaFaturas", filtro).Result;

            return nomeBanco;
        }

        public RetornoModel Cancelar(int codigoBordero)
        {
            var nomeBanco = ExecutaPost<RetornoModel, int>("Cancelar", codigoBordero).Result;

            return nomeBanco;
        }

        public List<APIDadosBorderoModel> ListaBorderos(IAPIFiltroBorderoModel filtro)
        {
            var nomeBanco = ExecutaPost<List<APIDadosBorderoModel>, IAPIFiltroBorderoModel>("ListaBorderos", filtro).Result;

            return nomeBanco;
        }

        public RetornoModel Salvar(IAPIDadosBoletoModel filtro)
        {
            var nomeBanco = ExecutaPost<RetornoModel, IAPIDadosBoletoModel>("Salvar", filtro).Result;

            return nomeBanco;
        }
    }
}
