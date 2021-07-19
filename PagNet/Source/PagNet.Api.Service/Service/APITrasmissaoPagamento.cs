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
    public class APITrasmissaoPagamento : ApiClientBase, IAPITrasmissaoPagamento
    {
        public APITrasmissaoPagamento(IHttpContextAccessor contextAccessor, ITokensRepository tokensRepository)
        //: base("https://localhost:44368/", contextAccessor, tokensRepository)
        : base("MontaArquivoRemessaPGTO/", contextAccessor, tokensRepository)
        { }

        public List<APIRetornoArquivoBancoVM> ProcessaArquivoRetorno(string CaminhoArquivo)
        {
            APIFiltroRetorno parametro = new APIFiltroRetorno();
            parametro.caminhoArquivo = CaminhoArquivo;
            try
            {
                var ResultadoTransmissao = ExecutaPost<List<APIRetornoArquivoBancoVM>, APIFiltroRetorno>("ProcessaArquivoRetorno", parametro).Result;

                return ResultadoTransmissao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<string, string> TransmiteArquivoBanco(IAPITransmissaoArquivoModal model)
        {
            var Resultado = new Dictionary<string, string>();
            try
            {
                var ResultadoTransmissao = ExecutaPost<ResultadoTransmissaoArquivo, IAPITransmissaoArquivoModal>("TransmiteArquivoBanco", model).Result;

                Resultado.Add(ResultadoTransmissao.nomeArquivo, ResultadoTransmissao.msgResultado);

                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public RetornoModel CancelaArquivoRemessa(int codigoArquivo)
        {
            var Resultado = new Dictionary<string, string>();
            try
            {
                var Result = ExecutaPost<RetornoModel, int>("CancelaArquivoRemessa", codigoArquivo).Result;
                
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
