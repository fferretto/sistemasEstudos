using PagNet.Application.Models;
using PagNet.Domain.Interface.Services;
using System;
using PagNet.Application.Interface.ProcessoCnab;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace PagNet.Application.Cnab.BancoBs2
{
    public class ProcessaDadosBancoBs2 : IProcessaDadosBancoBs2
    {

        private readonly IPagNet_ContaCorrenteService _conta;
        private readonly IPagNet_ArquivoService _arquivo;
        private readonly IPagNet_OcorrenciaRetPagService _ocorrencia;
        private readonly IPagNet_Titulos_PagosService _pag;
        private readonly IPagNet_CadFavorecidoService _favorito;
        private readonly IPagNet_CadEmpresaService _empresa;

        public ProcessaDadosBancoBs2(IPagNet_OcorrenciaRetPagService ocorrencia,
                                     IPagNet_ContaCorrenteService conta,
                                     IPagNet_ArquivoService arquivo,
                                     IPagNet_Titulos_PagosService pag,
                                     IPagNet_CadFavorecidoService favorito,
                                     IPagNet_CadEmpresaService empresa)
        {
            _conta = conta;
            _arquivo = arquivo;
            _pag = pag;
            _ocorrencia = ocorrencia;
            _favorito = favorito;
            _empresa = empresa;
        }

        public async Task<IDictionary<bool, string>> EnviaPagamentoBanco(BorderoPagVM model, int codArquivo)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {

                if (model.ListaBordero.Count > 0)
                {
                    string nmArquivo;
                    string parth = "";

                    var contaCorrente = await _conta.GetContaCorrenteById(Convert.ToInt32(model.codContaCorrente));

                    var vm = mdCedente.ToView(contaCorrente);
                    vm.NumSeq = codArquivo;
                    EnviarTitulosBanco(codArquivo, model, vm);

                    resultado.Add(true, "Títulos Enviados para o banco com sucesso.");

                }
            }
            catch (Exception)
            {

                throw;
            }
            return resultado;
        }
        public void EnviarTitulosBanco(int codArquivo, BorderoPagVM model, mdCedente cedente)
        {
            //try
            //{
            //    string Arquivo = "";
            //    string Lote = "";
            //    string header;
            //    string Trailerheader = "";


            //    int totalRegistroInserido = 0;
            //    int NumLot = 0;//numero do lote
            //    int NumTotalRegistro = 0;//Total de Registro


            //    // Header do Arquivo
            //    NumTotalRegistro = 1;
            //    header = GA.HeaderArquivo(cedente) + Environment.NewLine;

            //    var TitulosPagamento = _pag.GetPagamentosByCodArquivo(codArquivo);
            //    var TitulosTransferencias = TitulosPagamento.Where(x => x.TIPOTITULO == "TEDDOC").ToList();

            //    var ListaTitulosTED = TitulosTransferencias.Where(x => x.PAGNET_CADFAVORECIDO.BANCO != "237" && x.TIPOTITULO == "TEDDOC").ToList();
            //    //var ListaTitulosDOC = TitulosTransferencias.Where(x => x.FormaPGTO == "DOC" && x.CODBANCO != "0237" && x.TIPOTITULO == "TEDDOC").ToList();
            //    var ListaTitulosCC = TitulosTransferencias.Where(x => x.PAGNET_CADFAVORECIDO.BANCO == "237" && x.TIPOTITULO == "TEDDOC").ToList();
            //    var ListaTitulosBoleto = TitulosPagamento.Where(x => x.TIPOTITULO == "BOLETO").ToList();

            //    //MONTA LOTE PARA PAGAMENTO DE BOLETOS BANCÁRIOS
            //    if (ListaTitulosBoleto.Count > 0)
            //    {
            //        NumLot += 1;
            //        Lote += GeraTrailerLoteBoleto(cedente, model, ListaTitulosBoleto, "BOLETO", NumLot, out totalRegistroInserido);

            //        if (totalRegistroInserido > 0)
            //        {
            //            NumTotalRegistro += totalRegistroInserido;
            //        }
            //    }
            //    //MONTA LOTE PARA PAGAMENTO COMO CRÉDITO EM CONTA
            //    if (ListaTitulosCC.Count > 0)
            //    {
            //        NumLot += 1;
            //        Lote += GeraTrailerLote(cedente, model, ListaTitulosCC, "CC", NumLot, out totalRegistroInserido);

            //        if (totalRegistroInserido > 0)
            //        {
            //            NumTotalRegistro += totalRegistroInserido;
            //        }
            //    }
            //    //MONTA LOTE PARA PAGAMENTO COMO TED
            //    if (ListaTitulosTED.Count > 0)
            //    {
            //        NumLot += 1;
            //        Lote += GeraTrailerLote(cedente, model, ListaTitulosTED, "TED", NumLot, out totalRegistroInserido);

            //        if (totalRegistroInserido > 0)
            //        {
            //            NumTotalRegistro += totalRegistroInserido;
            //        }
            //    }

            //    NumTotalRegistro += 1;
            //    Trailerheader = GA.TrailerArquivo(NumLot, NumTotalRegistro, 0);


            //    Arquivo = header + Lote;

            //    Arquivo += Trailerheader;

            //    arquivoRemessa.WriteLine(Arquivo);

            //    arquivoRemessa.Close();
            //    arquivoRemessa.Dispose();
            //    arquivoRemessa = null;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }
        public string Autenticacao(int codContaCorrente)
        {

            using (var client = new HttpClient())
            {
                string caminho = "auth/oauth/v2/token";

                if (client.BaseAddress == null)
                {
                    client.BaseAddress = new Uri("https://api.bs2.com:8443/");
                }

                var content = new StringContent(JsonConvert.SerializeObject(""),
                                                Encoding.UTF8,
                                                "application/json");

                HttpResponseMessage response = client.PostAsync(caminho, content).Result;
                response.EnsureSuccessStatusCode();
                var msgretorno = response.ReasonPhrase;

                if (msgretorno != "Sucesso")
                {
                    throw new Exception(msgretorno);
                }
            }
            return "";
        }

    }
}
