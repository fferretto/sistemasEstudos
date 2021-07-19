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
    public class APIRelatorioApp : ApiClientBase, IAPIRelatorioApp
    {
        public APIRelatorioApp(IHttpContextAccessor contextAccessor, ITokensRepository tokensRepository)
        //: base("https://localhost:44336/", contextAccessor, tokensRepository)
        : base("Relatorio/", contextAccessor, tokensRepository)
        { }

        public APIRelatorioModel BuscaParametrosRelatorio(int codRel)
        {
            var dadosRetorno = ExecutaPost<APIRelatorioModel, int>("BuscaParametrosRelatorio", codRel).Result;
            return dadosRetorno;
        }
        public IDictionary<bool, string> ExportaExcel(IAPIRelatorioModel model)
        {
            Dictionary<bool, string> retorno = new Dictionary<bool, string>();
            var dadosRetorno = ExecutaPost<RetornoModel, IAPIRelatorioModel>("ExportaExcel", model).Result;
            retorno.Add(dadosRetorno.Sucesso, dadosRetorno.msgResultado);

            return retorno;
        }

        public RetornoModel GeraRelatorioViaJob(IAPIRelatorioModel model)
        {
            var dadosRetorno = ExecutaPost<RetornoModel, IAPIRelatorioModel>("GeraRelatorioViaJob", model).Result;
            return dadosRetorno;
        }

        public APIModRelPDFModel RelatorioPDF(IAPIRelatorioModel model)
        {
            var dadosRetorno = ExecutaPost<APIModRelPDFModel, IAPIRelatorioModel>("RelatorioPDF", model).Result;
            return dadosRetorno;
        }

        public APIModRelPDFModel RetornoRelatornoViaJob(IAPIRelatorioModel model)
        {
            var dadosRetorno = ExecutaPost<APIModRelPDFModel, IAPIRelatorioModel>("RetornoRelatornoViaJob", model).Result;
            return dadosRetorno;
        }

        public bool VerificaTerminoGeracaoRelatorio(int codigoRelatorio)
        {
            var dadosRetorno = ExecutaPost<bool, int>("VerificaTerminoGeracaoRelatorio", codigoRelatorio).Result;
            return dadosRetorno;
        }
    }
}
