using Microsoft.AspNetCore.Http;
using PagNet.Api.Service.Helpers;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Json;
using System.Text;
using Telenet.AspNetCore.Mvc.Authentication;


namespace PagNet.Api.Service.Service
{
    public class APITransmissaoCobrancaBancaria : ApiClientBase, IAPITransmissaoCobrancaBancaria
    {
        public APITransmissaoCobrancaBancaria(IHttpContextAccessor contextAccessor, ITokensRepository tokensRepository)
        //: base("https://localhost:44314/", contextAccessor, tokensRepository)
        : base("CobrancaBancaria/", contextAccessor, tokensRepository)
        { }

        public List<APISolicitacaoBoletoVM> CarregaDadosArquivoRetorno(string _caminhoArquivo)
        {
            //dynamic FiltroEnvio = new JsonObject();
            //FiltroEnvio.CaminhoArquivo = _caminhoArquivo;
            APIFiltroCobranca parametros = new APIFiltroCobranca();
            parametros.caminhoArquivo = _caminhoArquivo;

            try
            {
                var ResultadoTransmissao = ExecutaPost<List<APISolicitacaoBoletoVM>, APIFiltroCobranca>("CarregaDadosArquivoRetorno", parametros).Result;

                return ResultadoTransmissao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GeraArquivoRemessa(IAPIBorderoBolVM filtro)
        {
            try
            {
                var ResultadoTransmissao = ExecutaPost<ResultadoTransmissaoArquivo, IAPIBorderoBolVM>("TransmissaoArquivoRemessa", filtro).Result;

                return ResultadoTransmissao.CaminhoCompletoArquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GeraBoletoPDF(IAPIFiltroCobranca filtro)
        {
            try
            {
                var ResultadoTransmissao = ExecutaPost<bool, IAPIFiltroCobranca>("TransmissaoArquivoRemessa", filtro).Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
