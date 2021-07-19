using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Services;
using PagNet.Domain.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PagNet.Application.Application
{
    public class ContaEmailApp : IContaEmailApp
    {
        private readonly IPagNet_ContaEmailService _email;
        private readonly IOperadoraService _ope;
        private readonly IPagNet_EmissaoBoletoService _emissaoBoleto;
        private readonly IRecebimentoApp _recebimento;
        private readonly IPagNet_CadEmpresaService _empresa;
        private readonly IPagNet_InstrucaoEmailService _instrucao;

        public ContaEmailApp(IPagNet_ContaEmailService email,
                             IPagNet_EmissaoBoletoService emissaoBoleto,
                             IRecebimentoApp recebimento,
                             IOperadoraService ope,
                             IPagNet_CadEmpresaService empresa,
                             IPagNet_InstrucaoEmailService instrucao)
        {
            _email = email;
            _ope = ope;
            _emissaoBoleto = emissaoBoleto;
            _recebimento = recebimento;
            _empresa = empresa;
            _instrucao = instrucao;
         }

        public async Task<IDictionary<bool, string>> EnviaEmailBoleto(int codEmissaoBoleto, int cod_ope, int codUsuario, int codEmpresa)
        {
            var resultado = new Dictionary<bool, string>();
            var DadosBoleto = await _emissaoBoleto.BuscaBoletoByID(codEmissaoBoleto);
            try
            {
                bool EmailEnviadoSucesso = false;

                var CaminhoPadrao = await _ope.GetOperadoraById(cod_ope);
                string nmCaminhoPadrao = Path.Combine(CaminhoPadrao.CAMINHOARQUIVO, CaminhoPadrao.NOMOPERAFIL, codEmpresa.ToString());

                var BoletoFullName = Path.Combine(nmCaminhoPadrao, "PDFBoleto", DadosBoleto.nmBoletoGerado + ".pdf");

                //VALIDA SE O PDF DO BOLETO JÁ FOI GERADO, CASO CONTRÁRIO, O SISTEMA DEVERÁ GERAR ELE AUTOMATICAMENTE
                if (!System.IO.File.Exists(BoletoFullName))
                {
                    _recebimento.GeraBoletoPDF(Path.Combine(nmCaminhoPadrao, "PDFBoleto"), Convert.ToInt32(codEmissaoBoleto), codUsuario);
                }

                var DadosEmail = await _email.GetallEmailAtivos();
                var DadosEmailPrincipal = DadosEmail.Where(x => x.EMAILPRINCIPAL == "S").FirstOrDefault();

                var EmailPara = RemoveEmailsDupicados(DadosBoleto.PAGNET_CADCLIENTE.EMAIL);

                DadosEnvioEmailVm envioEmail = new DadosEnvioEmailVm();

                if (DadosEmailPrincipal == null)
                {
                    resultado.Add(false, "Necessário cadastrar ao menos 1 Email Principal para prosseguir com esta ação.");
                    return resultado;
                }


                envioEmail.ServidorSMTP = DadosEmailPrincipal.SERVIDOR;
                envioEmail.Porta = Convert.ToInt32(DadosEmailPrincipal.PORTA);
                envioEmail.Criptografia = DadosEmailPrincipal.CRIPTOGRAFIA;
                envioEmail.Usuario = DadosEmailPrincipal.EMAIL;
                envioEmail.Senha = DadosEmailPrincipal.SENHA;                                   
                envioEmail.Para = EmailPara;
                envioEmail.TituloEmail = "Boleto para Pagamento";
                envioEmail.CorpoEmail = "Segue em anexo o boleto para pagamento.";
                envioEmail.CaminhoArquivoAnexo = BoletoFullName;

                envioEmail.CODUSUARIO = codUsuario;
                envioEmail.CODCONTAEMAIL = DadosEmailPrincipal.CODCONTAEMAIL;
                envioEmail.CODEMISSAOBOLETO = DadosBoleto.codEmissaoBoleto;

                var resultadoEnvio = await enviaEmailComAnexo(envioEmail);

                if (!resultadoEnvio)
                {
                    var DadosEmailSecundario = DadosEmail.Where(x => x.EMAILPRINCIPAL == "N").ToList();
                    if (DadosEmailSecundario.Count == 0)
                    {
                        resultado.Add(false, "Falha ao enviar o email. Favor contactar o suporte.");
                        return resultado;
                    }

                    foreach (var emalsecundario in DadosEmailSecundario)
                    {
                        envioEmail.ServidorSMTP = emalsecundario.SERVIDOR;
                        envioEmail.Porta = Convert.ToInt32(emalsecundario.PORTA);
                        envioEmail.Criptografia = emalsecundario.CRIPTOGRAFIA;
                        envioEmail.Usuario = emalsecundario.EMAIL;
                        envioEmail.Senha = emalsecundario.SENHA;
                        envioEmail.CODCONTAEMAIL = emalsecundario.CODCONTAEMAIL;

                        var EnvioSecundario = await enviaEmailComAnexo(envioEmail);

                        if (EnvioSecundario)
                        {
                            EmailEnviadoSucesso = true;
                            break;
                        }
                    }
                }
                else
                {
                    EmailEnviadoSucesso = true;
                }

                if (!EmailEnviadoSucesso)
                {
                    resultado.Add(false, "Falha ao enviar o email. Favor contactar o suporte.");
                }
                else
                {
                    DadosBoleto.BOLETOENVIADO = "S";
                    _emissaoBoleto.AtualizaBoleto(DadosBoleto);
                    resultado.Add(true, "Email enviado com sucesso.");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                DadosBoleto.BOLETOENVIADO = "N";
                _emissaoBoleto.AtualizaBoleto(DadosBoleto);
                throw ex;
            }
        }

        public async Task<IDictionary<bool, string>> EnviarBoletoOutroEmail(int codEmissaoBoleto, string email, int cod_ope, int codUsuario, int codEmpresa)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                bool EmailEnviadoSucesso = false;

                var CaminhoPadrao = await _ope.GetOperadoraById(cod_ope);
                var DadosBoleto = await _emissaoBoleto.BuscaBoletoByID(codEmissaoBoleto);
                string nmCaminhoPadrao = Path.Combine(CaminhoPadrao.CAMINHOARQUIVO, CaminhoPadrao.NOMOPERAFIL, codEmpresa.ToString());

                var BoletoFullName = Path.Combine(nmCaminhoPadrao, "PDFBoleto", DadosBoleto.nmBoletoGerado + ".pdf");

                //VALIDA SE O PDF DO BOLETO JÁ FOI GERADO, CASO CONTRÁRIO, O SISTEMA DEVERÁ GERAR ELE AUTOMATICAMENTE
                if (!System.IO.File.Exists(BoletoFullName))
                {
                    _recebimento.GeraBoletoPDF(Path.Combine(nmCaminhoPadrao, "PDFBoleto"), Convert.ToInt32(codEmissaoBoleto), codUsuario);
                }

                var DadosEmail = await _email.GetallEmailAtivos();
                var DadosEmailPrincipal = DadosEmail.Where(x => x.EMAILPRINCIPAL == "S").FirstOrDefault();

                var EmailPara = RemoveEmailsDupicados(email);

                DadosEnvioEmailVm envioEmail = new DadosEnvioEmailVm();

                if (DadosEmailPrincipal == null)
                {
                    resultado.Add(false, "Necessário cadastrar ao menos 1 Email Principal para prosseguir com esta ação.");
                    return resultado;
                }


                envioEmail.ServidorSMTP = DadosEmailPrincipal.SERVIDOR;
                envioEmail.Porta = Convert.ToInt32(DadosEmailPrincipal.PORTA);
                envioEmail.Criptografia = DadosEmailPrincipal.CRIPTOGRAFIA;
                envioEmail.Usuario = DadosEmailPrincipal.EMAIL;
                envioEmail.Senha = DadosEmailPrincipal.SENHA;
                envioEmail.Para = EmailPara;
                envioEmail.TituloEmail = "Boleto para Pagamento";
                envioEmail.CorpoEmail = "Segue em anexo o boleto para pagamento.";
                envioEmail.CaminhoArquivoAnexo = BoletoFullName;

                envioEmail.CODUSUARIO = codUsuario;
                envioEmail.CODCONTAEMAIL = DadosEmailPrincipal.CODCONTAEMAIL;
                envioEmail.CODEMISSAOBOLETO = DadosBoleto.codEmissaoBoleto;

                var resultadoEnvio = await enviaEmailComAnexo(envioEmail);

                if (!resultadoEnvio)
                {
                    var DadosEmailSecundario = DadosEmail.Where(x => x.EMAILPRINCIPAL == "N").ToList();
                    if (DadosEmailSecundario.Count == 0)
                    {
                        resultado.Add(false, "Falha ao enviar o email. Favor contactar o suporte.");
                        return resultado;
                    }

                    foreach (var emalsecundario in DadosEmailSecundario)
                    {
                        envioEmail.ServidorSMTP = emalsecundario.SERVIDOR;
                        envioEmail.Porta = Convert.ToInt32(emalsecundario.PORTA);
                        envioEmail.Criptografia = emalsecundario.CRIPTOGRAFIA;
                        envioEmail.Usuario = emalsecundario.EMAIL;
                        envioEmail.Senha = emalsecundario.SENHA;
                        envioEmail.CODCONTAEMAIL = emalsecundario.CODCONTAEMAIL;

                        var EnvioSecundario = await enviaEmailComAnexo(envioEmail);

                        if (EnvioSecundario)
                        {
                            EmailEnviadoSucesso = true;
                            break;
                        }
                    }
                }
                else
                {
                    EmailEnviadoSucesso = true;
                }

                if (!EmailEnviadoSucesso)
                {
                    resultado.Add(false, "Falha ao enviar o email. Favor contactar o suporte.");
                }
                else
                {
                    DadosBoleto.BOLETOENVIADO = "S";
                    _emissaoBoleto.AtualizaBoleto(DadosBoleto);
                    resultado.Add(true, "Email enviado com sucesso.");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ConsultaContaEmailVM>> GetAllContasEmail(int codEmpresa)
        {
            try
            {
                List<ConsultaContaEmailVM> Emails = new List<ConsultaContaEmailVM>();

                Emails = _email.GetAll().Where(x => x.ATIVO == "S" && x.CODEMPRESA == codEmpresa)
                               .Select(y => new ConsultaContaEmailVM(y)).ToList();

                return Emails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ContaEmailVM GetContaEmailById(int? id, int codEmpresa)
        {
            try
            {
                ContaEmailVM DadosEmail = new ContaEmailVM();

                var codEmail = id ?? 0;
                var empresa = _empresa.ConsultaEmpresaById(codEmpresa).Result;

                if (codEmail > 0)
                {
                    DadosEmail = ContaEmailVM.ToView(_email.GetEmailById((int)id).Result);
                    DadosEmail.nmEmpresa = empresa.NMFANTASIA;
                }
                else
                {
                    DadosEmail.codEmpresa = empresa.CODEMPRESA.ToString();
                    DadosEmail.nmEmpresa = empresa.NMFANTASIA;
                }
                return DadosEmail;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IDictionary<string, string>> RemoveConta(int codContaEmail)
        {

            var resultado = new Dictionary<string, string>();

            try
            {
                var conta = _email.GetEmailById(codContaEmail).Result;

                conta.ATIVO = "N";

                _email.AtualizaEmail(conta);
                resultado.Add("Sucesso", "Email Removido com Sucesso!");


            }
            catch (ArgumentException ex)
            {
                string[] lines = Regex.Split(ex.Message, "\r\n");
                resultado.Add(ex.ParamName, lines[0]);
            }


            return resultado;
        }

        public async Task<IDictionary<string, string>> SalvaConta(ContaEmailVM email)
        {
            var resultado = new Dictionary<string, string>();

            try
            {               

                PAGNET_CONTAEMAIL conta;
                if (email.CODCONTAEMAIL == 0)
                {
                    //valida se já existe uma conta principal cadastrada, pois só pode haver uma conta principal,
                    //mas contas secundárias podem haver várias.
                    if (email.EMAILPRINCIPAL)
                    {
                        if (_email.ExisteContaPrincipalCadastrada())
                        {
                            resultado.Add("Falha", "Não é possível cadastrar mais de uma conta principal.");
                            return resultado;
                        }
                    }

                    conta  = new PAGNET_CONTAEMAIL();
                    conta = ContaEmailVM.ToEntity(email);

                    var dadosRetorno = await _email.IncluirEmail(conta);
                }
                else
                {
                    conta = _email.GetEmailById(email.CODCONTAEMAIL).Result;
                    if (string.IsNullOrWhiteSpace(email.SENHA)) email.SENHA = conta.SENHA;

                    conta.NMCONTAEMAIL = email.NMCONTAEMAIL;
                    conta.EMAIL = email.EMAIL;
                    conta.SENHA = email.SENHA;
                    conta.SERVIDOR = email.SERVIDOR;
                    conta.ENDERECOSMTP = email.SERVIDOR;
                    conta.PORTA = email.PORTA;
                    conta.CRIPTOGRAFIA = email.CRIPTOGRAFIA;
                    conta.EMAILPRINCIPAL = (email.EMAILPRINCIPAL) ? "S" : "N";
                    conta.CODEMPRESA = Convert.ToInt32(email.codEmpresa);

                    _email.AtualizaEmail(conta);
                }

                resultado.Add("Sucesso", "Email Cadastrado com Sucesso!");

            }
            catch (Exception ex)
            {
                string[] lines = Regex.Split(ex.Message, "\r\n");
                //resultado.Add(ex.ParamName, lines[0]);
            }


            return resultado;
        }

        public bool ValidaContaEmail(ContaEmailVM _email)
        {
            try
            {

                DadosEnvioEmailVm envioEmail = new DadosEnvioEmailVm();

                envioEmail = DadosEnvioEmailVm.ToEmailTeste(_email);

                var resultadoEnvio = enviaEmailTeste(envioEmail).Result;

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> enviaEmailTeste(DadosEnvioEmailVm _emailTeste)
        {
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = _emailTeste.ServidorSMTP, // set your SMTP server name here
                    Port = _emailTeste.Porta, // Port 
                    UseDefaultCredentials = true,
                    EnableSsl = (_emailTeste.Criptografia == "SSL"),
                    Credentials = new NetworkCredential(_emailTeste.Usuario, _emailTeste.Senha)
                };

                using (var message = new MailMessage(_emailTeste.Usuario, _emailTeste.Para)
                {
                    Subject = _emailTeste.TituloEmail,
                    Body = _emailTeste.CorpoEmail
                })
                {
                    await smtpClient.SendMailAsync(message);
                }
                return true;

            }
            catch (ArgumentException ar)
            {
                var valor = ar.Message;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> enviaEmailComAnexo(DadosEnvioEmailVm Email)
        {
            try
            {
                PAGNET_CONTAEMAIL conta;
                var smtpClient = new SmtpClient
                {
                    Host = Email.ServidorSMTP, // set your SMTP server name here
                    Port = Email.Porta, // Port 
                    EnableSsl = (Email.Criptografia == "SSL"),
                    Credentials = new NetworkCredential(Email.Usuario, Email.Senha)
                };

                var message = new MailMessage();

                var listaEmailsTo = Email.Para.Split(';');
                foreach(var email in listaEmailsTo)
                {
                    if(!string.IsNullOrWhiteSpace(email))
                        message.To.Add(email);
                }

                if (!string.IsNullOrWhiteSpace(Email.CC))
                {
                    var listaEmailsCC = Email.CC.Split(';');
                    foreach (var email in listaEmailsCC)
                    {
                        if (!string.IsNullOrWhiteSpace(email))
                            message.CC.Add(email);
                    }
                }
                               
                message.From = new MailAddress(Email.Usuario);
                message.Subject = Email.TituloEmail;
                message.Body = Email.CorpoEmail;
                conta = _email.GetEmailById(Email.CODCONTAEMAIL).Result;
                var DadosInstrucao = await _instrucao.ConsultaInstrucaoById(conta.CODEMPRESA);
                var instrucao = DadosInstrucao.ToList();

                foreach (var valor in instrucao)
                {
                    message.Subject = valor.ASSUNTO;
                    message.Body = valor.MENSAGEM;
                }
                
                if (!string.IsNullOrWhiteSpace(Email.CaminhoArquivoAnexo))
                {
                    Attachment att = new Attachment(Email.CaminhoArquivoAnexo);
                    message.Attachments.Add(att);
                }

                await smtpClient.SendMailAsync(message);

                //Registra o log 
                var logEmail = DadosEnvioEmailVm.ToLogEmail(Email);
                _email.InseriLog(logEmail);


                return true;

            }
            catch (ArgumentException ar)
            {
                var valor = ar.Message;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string RemoveEmailsDupicados(string EmailOriginal)
        {
            string emailRetornor = "";

            EmailOriginal = EmailOriginal.Replace(",", ";").ToUpper();
            var listaEmails = EmailOriginal.Split(';').ToList().Distinct();

            foreach (var email in listaEmails)
            {
                emailRetornor += email + ";";
            }

            emailRetornor = emailRetornor.Substring(0, emailRetornor.Length - 1);

            return emailRetornor;

        }
        private string FormatMultipleEmailAddresses(string emailAddresses)
        {
            var delimiters = new[] { ',', ';' };

            var addresses = emailAddresses.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            return string.Join(",", addresses);
        }

        public async Task<IDictionary<bool, string>> EnviaEmailDetalhamentoFatura(int codEmissaoBoleto, string caminhoFatura, string emailDestino, int codUsuario)
        {

            var resultado = new Dictionary<bool, string>();
            try
            {
                bool EmailEnviadoSucesso = false;
                
                var DadosEmail = await _email.GetallEmailAtivos();
                var DadosEmailPrincipal = DadosEmail.Where(x => x.EMAILPRINCIPAL == "S").FirstOrDefault();

                if (DadosEmailPrincipal == null)
                {
                    resultado.Add(false, "Necessário cadastrar ao menos 1 Email Principal para prosseguir com esta ação.");
                    return resultado;
                }

                var EmailPara = RemoveEmailsDupicados(emailDestino);

                DadosEnvioEmailVm envioEmail = new DadosEnvioEmailVm();

                envioEmail.ServidorSMTP = DadosEmailPrincipal.SERVIDOR;
                envioEmail.Porta = Convert.ToInt32(DadosEmailPrincipal.PORTA);
                envioEmail.Criptografia = DadosEmailPrincipal.CRIPTOGRAFIA;
                envioEmail.Usuario = DadosEmailPrincipal.EMAIL;
                envioEmail.Senha = DadosEmailPrincipal.SENHA;
                envioEmail.Para = EmailPara;
                envioEmail.TituloEmail = "Detalhamento da cobrança";
                envioEmail.CorpoEmail = "Segue em anexo o detalhamento dos valores cobrados.";
                envioEmail.CaminhoArquivoAnexo = caminhoFatura;

                envioEmail.CODUSUARIO = codUsuario;
                envioEmail.CODCONTAEMAIL = DadosEmailPrincipal.CODCONTAEMAIL;
                envioEmail.CODEMISSAOBOLETO = codEmissaoBoleto;

                var resultadoEnvio = await enviaEmailComAnexo(envioEmail);

                if (!resultadoEnvio)
                {
                    var DadosEmailSecundario = DadosEmail.Where(x => x.EMAILPRINCIPAL == "N").ToList();
                    if (DadosEmailSecundario.Count == 0)
                    {
                        resultado.Add(false, "Falha ao enviar o email. Favor contactar o suporte.");
                        return resultado;
                    }

                    foreach (var emalsecundario in DadosEmailSecundario)
                    {
                        envioEmail.ServidorSMTP = emalsecundario.SERVIDOR;
                        envioEmail.Porta = Convert.ToInt32(emalsecundario.PORTA);
                        envioEmail.Criptografia = emalsecundario.CRIPTOGRAFIA;
                        envioEmail.Usuario = emalsecundario.EMAIL;
                        envioEmail.Senha = emalsecundario.SENHA;
                        envioEmail.CODCONTAEMAIL = emalsecundario.CODCONTAEMAIL;

                        var EnvioSecundario = await enviaEmailComAnexo(envioEmail);

                        if (EnvioSecundario)
                        {
                            EmailEnviadoSucesso = true;
                            break;
                        }
                    }
                }
                else
                {
                    EmailEnviadoSucesso = true;
                }

                if (!EmailEnviadoSucesso)
                {
                    resultado.Add(false, "Falha ao enviar o email. Favor contactar o suporte.");
                }
                else
                {
                    resultado.Add(true, "Email enviado com sucesso.");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IDictionary<bool, string> EnviaEmailBoletoEmMassa(DadosEnvioEmailMassModel dadosEmail, int cod_ope, int codUsuario)
        {
            var resultado = new Dictionary<bool, string>();
            foreach (var email in dadosEmail.ListaBoleto)
            {
                var resultEnvio = EnviaEmailBoleto(email.codEmissaoBoleto, cod_ope, codUsuario, dadosEmail.codigoEmpresa).Result;
            }
            resultado.Add(true, "Processo Finalizado com sucesso!");

            return resultado;
        }
    }
}
