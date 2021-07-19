using Boleto.Models;
using BoletoNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Boleto.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View(new HomeIndex());
        }

        public ActionResult GerarBoleto(string dropBanco, string dropCarteira, string txtValor, string txtVencimento)
        {

            switch (dropBanco)
            {
                case "001":
                    ViewBag.Boleto = GerarBoletoBB(dropCarteira, txtValor, txtVencimento);
                    break;
                case "341":
                    ViewBag.Boleto = GerarBoletoItau(dropCarteira, txtValor, txtVencimento);
                    break;    
                default:
                    break;
            }


            return View();

        }

        protected string GerarBoletoBB(string dropCarteira, string txtValor, string txtVencimento)
        {

            var model = new HomeIndex();

            BoletoBancario boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 001;

            ContaBancaria cb = new ContaBancaria();

            cb.Agencia = "2906";
            cb.Conta = "23935";

            Cedente c = new Cedente();
            c.CPFCNPJ = "20590201000101";
            c.Nome = "CONDOMINIO HORIZONTE VERDE";
            c.Carteira = dropCarteira;
            c.ContaBancaria = cb;
            c.Codigo = "120";
            c.Convenio = int.Parse(c.Codigo);

            BoletoNet.Boleto b = new BoletoNet.Boleto(Convert.ToDateTime(txtVencimento), Convert.ToDecimal(txtValor), dropCarteira, "12345678", c);

            b.NumeroDocumento = "12345678";
            b.DataProcessamento = DateTime.Now;
            b.DataDocumento = Convert.ToDateTime(txtVencimento);
            b.JurosMora = 2;

            BoletoNet.Endereco end = new BoletoNet.Endereco();

            end.Bairro = "";
            end.End = "AV 3, 776 16/302 GAVEA II";
            end.CEP = "33200000";
            end.Cidade = "VESPASIANO";
            end.Complemento = "";
            end.Email = "";
            end.Logradouro = "";
            end.Numero = "";
            end.UF = "MG";

            BoletoNet.Sacado s = new BoletoNet.Sacado("08553701654", "THIAGO HENRIQUE DE SOUZA", end);
            b.Sacado = s;

            //Aqui passa o codigo do banco bradesco
            //b.EspecieDocumento = new EspecieDocumento_Santander("17");
            b.EspecieDocumento = new EspecieDocumento_Itau("001");

            Instrucao objInst1 = new Instrucao(001);
            objInst1.Descricao = "teste 1";
            Instrucao objInst2 = new Instrucao(001);
            objInst2.Descricao = "teste 2";
            Instrucao objInst3 = new Instrucao(001);
            objInst3.Descricao = "teste 3";
            Instrucao objInst4 = new Instrucao(001);
            objInst4.Descricao = "teste 4";

            b.Instrucoes.Add(objInst1);
            b.Instrucoes.Add(objInst2);
            b.Instrucoes.Add(objInst3);
            b.Instrucoes.Add(objInst4);

            //Importante para o calculo do digito verificador
            //b.PercentualIOS = 0;

            boletoBancario.Boleto = b;
            boletoBancario.MostrarCodigoCarteira = true;
            boletoBancario.Boleto.Valida();

            BoletoNet.BoletoBancario bolBanc = boletoBancario;

            //HTML que monta o boleto.
            // Aqui dá o erro falando que não foi passado o valor pro objeto BoletoNet.Conta
            var htmlBoleto = bolBanc.MontaHtmlEmbedded();// MontaHtml();
            //htmlBoleto = htmlBoleto
            //.Replace(Request.ServerVariables["APPL_PHYSICAL_PATH"], "\\") // convertendo o caminho absoluto para relativo
            //.Replace(System.IO.Path.GetTempPath(), Url.Action("Temporario", "Faturas") + "/?filename=") // convertendo o caminho temporário em relativo
            //.Replace(".w666{width:666px}", ".w666{width:21cm}")
            //.Replace("<body>", "<body style=\"height:29cm\">")
            //    //.Replace("</html>", "<a length=\"0\" href=\"/Faturas/PrintBoleto?Fatura=subsFatura\" >Exportar PDF</a></html>")
            ////.Replace("</html>", "<a length=\"0\" href=\"#\" onclick=\"window.print();\">Imprimir/Exportar PDF</a></html>")
            ////.Replace("subsFatura", Fatura.Replace("/", "%2F"))
            //    //.Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">", "")
            //    //.Replace("Imprimir em impressora jato de tinta (ink jet) ou laser em qualidade normal. (Não use modo econômico).<br>Utilize folha A4 (210 x 297 mm) ou Carta (216 x 279 mm) - Corte na linha indicada<br>","")
            //    //.Replace("&nbsp;","")
            //    ;

            return htmlBoleto.ToString(); ;
        }

        protected string GerarBoletoItau(string dropCarteira, string txtValor, string txtVencimento)
        {

            var model = new HomeIndex();

            BoletoBancario boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 341;

            ContaBancaria cb = new ContaBancaria();

            cb.Agencia = "2906";
            cb.Conta = "23935";

            Cedente c = new Cedente();
            c.CPFCNPJ = "20590201000101";
            c.Nome = "CONDOMINIO HORIZONTE VERDE";
            c.Carteira = dropCarteira;
            c.ContaBancaria = cb;
            c.Codigo = "120";
            c.Convenio = int.Parse(c.Codigo);

            BoletoNet.Boleto b = new BoletoNet.Boleto(DateTime.Now, Convert.ToDecimal(txtValor), dropCarteira, "12345678", c);

            b.NumeroDocumento = "12345678";
            b.DataProcessamento = DateTime.Now;
            b.DataDocumento = DateTime.Now;
            b.JurosMora = 2;

            BoletoNet.Endereco end = new BoletoNet.Endereco();

            end.Bairro = "";
            end.End = "AV 3, 776 16/302 GAVEA II";
            end.CEP = "33200000";
            end.Cidade = "VESPASIANO";
            end.Complemento = "";
            end.Email = "";
            end.Logradouro = "";
            end.Numero = "";
            end.UF = "MG";

            BoletoNet.Sacado s = new BoletoNet.Sacado("08553701654", "THIAGO HENRIQUE DE SOUZA", end);
            b.Sacado = s;

            //Aqui passa o codigo do banco bradesco
            //b.EspecieDocumento = new EspecieDocumento_Santander("17");
            b.EspecieDocumento = new EspecieDocumento_Itau("341");

            Instrucao objInst1 = new Instrucao(001);
            objInst1.Descricao = "teste 1";
            Instrucao objInst2 = new Instrucao(001);
            objInst2.Descricao = "teste 2";
            Instrucao objInst3 = new Instrucao(001);
            objInst3.Descricao = "teste 3";
            Instrucao objInst4 = new Instrucao(001);
            objInst4.Descricao = "teste 4";

            b.Instrucoes.Add(objInst1);
            b.Instrucoes.Add(objInst2);
            b.Instrucoes.Add(objInst3);
            b.Instrucoes.Add(objInst4);

            //Importante para o calculo do digito verificador
            //b.PercentualIOS = 0;

            boletoBancario.Boleto = b;
            boletoBancario.MostrarCodigoCarteira = true;
            boletoBancario.Boleto.Valida();

            BoletoNet.BoletoBancario bolBanc = boletoBancario;

            //HTML que monta o boleto.
            // Aqui dá o erro falando que não foi passado o valor pro objeto BoletoNet.Conta
            var htmlBoleto = bolBanc.MontaHtmlEmbedded();// MontaHtml();
            //htmlBoleto = htmlBoleto
            //.Replace(Request.ServerVariables["APPL_PHYSICAL_PATH"], "\\") // convertendo o caminho absoluto para relativo
            //.Replace(System.IO.Path.GetTempPath(), Url.Action("Temporario", "Faturas") + "/?filename=") // convertendo o caminho temporário em relativo
            //.Replace(".w666{width:666px}", ".w666{width:21cm}")
            //.Replace("<body>", "<body style=\"height:29cm\">")
            //    //.Replace("</html>", "<a length=\"0\" href=\"/Faturas/PrintBoleto?Fatura=subsFatura\" >Exportar PDF</a></html>")
            ////.Replace("</html>", "<a length=\"0\" href=\"#\" onclick=\"window.print();\">Imprimir/Exportar PDF</a></html>")
            ////.Replace("subsFatura", Fatura.Replace("/", "%2F"))
            //    //.Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">", "")
            //    //.Replace("Imprimir em impressora jato de tinta (ink jet) ou laser em qualidade normal. (Não use modo econômico).<br>Utilize folha A4 (210 x 297 mm) ou Carta (216 x 279 mm) - Corte na linha indicada<br>","")
            //    //.Replace("&nbsp;","")
            //    ;

            return htmlBoleto.ToString();
        }

    }
}
