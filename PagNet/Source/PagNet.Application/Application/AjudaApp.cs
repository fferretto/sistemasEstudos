using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PagNet.Application.Application
{
    public class AjudaApp : IAjudaApp
    {
        private readonly IPagNet_ContaCorrenteService _conta;



        public AjudaApp(IPagNet_ContaCorrenteService conta)
        {
            _conta = conta;
        }

        public async Task<IDictionary<bool, string>> EnviarEmail(ContatoViaEmailVM modal)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                modal.Mensagem = modal.Mensagem.Replace("\n", "<br />");
                var msg = "<b>Email enviado via sistema Pagnet</b> <br /><br /><br />";
                msg += "<b>Solicitante: </b> " + modal.nmSolicitante + " <br />";
                msg += "<b>Empresa: </b> " + modal.nmEmpresaSolicitante + " <br />";
                msg += "<b>Email para contato: </b> " + modal.EmailSolicitente + " <br />";
                msg += "<b>Telefone para contato: </b> " + modal.TelefoneSolicitante + " <br />";
                msg += "<b>Mensagem Enviada: </b>  <br />";
                msg += modal.Mensagem;

               var smtpClient = new SmtpClient
                {
                    Host = "smtp.task.com.br", // set your SMTP server name here
                    Port = 587, // Port 
                    EnableSsl = false,
                    Credentials = new NetworkCredential("sistemapagnet@tln.com.br", "P@gn3t3lenet$#")
                };

                var message = new MailMessage();

                message.To.Add("suporte@tln.com.br");
                //message.To.Add("luiz@tln.com.br");

                message.From = new MailAddress("sistemapagnet@tln.com.br");
                message.Subject = "PagNet-" + modal.Assunto;
                message.IsBodyHtml = true;
                message.Body = msg;
                if (modal.Anexo != null)
                {
                    if (modal.Anexo.Count > 0)
                    {
                        foreach (var item in modal.Anexo)
                        {
                            var path = Path.Combine(modal.CaminhoArquivoPadrao, item.NovoNomeArquivo);
                            Attachment att = new Attachment(path);
                            message.Attachments.Add(att);
                        }
                    }
                }

                await smtpClient.SendMailAsync(message);

                resultado.Add(true, "Email enviado com sucesso!");
            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
            }
            return resultado;
        }
    }
}
