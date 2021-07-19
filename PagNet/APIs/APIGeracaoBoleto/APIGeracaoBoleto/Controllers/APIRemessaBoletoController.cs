using APIGeracaoBoleto.Models;
using APIGeracaoBoleto.Processamento;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIGeracaoBoleto.Controllers
{
    public class APIRemessaBoletoController : ApiController
    {
        // GET: api/APIRemessaBoleto/5
        [HttpPost()]
        [Route("RetornaDadosArquivo")]
        public APIBoletoVM RetornaDadosArquivo([FromBody] APIDadosRetArquivo model)
        {
            ProcessaBoleto PB = new ProcessaBoleto();

            var dadosRet = PB.GetArquivoRetorno(model);

            return dadosRet;
        }

        // POST: api/APIRemessaBoleto/5
        [HttpPost()]
        [Route("CriaArquivoRemessa")]
        public HttpResponseMessage CriaArquivoRemessa([FromBody] APIBoletoVM model)
        {
            try
            {
                ProcessaBoleto PB = new ProcessaBoleto();

                var retornoProcessamento = PB.GeraArquivo(model);

                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(retornoProcessamento)
                };
                
                return resp;
            }
            catch(Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Falha no processo")),
                    ReasonPhrase = "Ocorreu uma falha durante o processo de geração de arquivo."
                };
                throw new HttpResponseException(resp);
            }
        }
    }
}
