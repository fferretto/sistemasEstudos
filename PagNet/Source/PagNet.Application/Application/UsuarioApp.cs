using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using PagNet.Application.Helpers;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Domain.Entities;
using PagNet.Domain.Helpers;
using PagNet.Domain.Interface.Services;

namespace PagNet.Application.Application
{
    public class UsuarioApp : IUsuarioApp
    {
        private readonly IUsuario_NetCardService _usuNetCard;
        private readonly IPagNet_UsuarioService _usuPagNet;
        private readonly IOperadoraService _operadora;
        private readonly ICadastrosApp _cadastro;

        public UsuarioApp(IUsuario_NetCardService usuNetCard,
                          IPagNet_UsuarioService usuPagNet,
                          IOperadoraService operadora,
                          ICadastrosApp cadastro)
        {
            _usuNetCard = usuNetCard;
            _usuPagNet = usuPagNet;
            _operadora = operadora;
            _cadastro = cadastro;
        }

        public async Task<IDictionary<string, string>> Desativar(int id)
        {
            var resultado = new Dictionary<string, string>();

            bool tudoCerto = false;
            try
            {
                tudoCerto = await _usuNetCard.Desativa(id);
                tudoCerto = await _usuPagNet.Desativa(id);

            }
            catch (ArgumentException ex)
            {
                if (!resultado.ContainsKey(ex.ParamName)) resultado.Add(ex.ParamName, ex.Message);
            }
            catch (Exception ex)
            {
                if (!resultado.ContainsKey("")) resultado.Add("", ex.Message);
            }
            return resultado;
        }

        public async Task<List<ConsultaUsuario>> GetAllUsuarioByCodOpe(int id)
        {
            List<ConsultaUsuario> retorno;

            retorno = new List<ConsultaUsuario>();

            retorno = _usuPagNet.GetAllUsuarioByCodOpe(id)
                .Select(x => new ConsultaUsuario(x)).ToList();

            return retorno;
        }

        public UsuariosVm GetUsuario(int? id)
        {
            UsuariosVm retorno;

            if (id == null || id == 0)
            {
                retorno = new UsuariosVm();
            }
            else
            {
                var Reg = _usuNetCard.GetUsuarioById((int)id).Result;

                if (Reg != null)
                {

                    var nomeAbreviado = Abreviar(Reg.NMUSUARIO, true);

                    var empresa = _cadastro.RetornaDadosEmpresaByID((int)Reg.CODEMPRESA);

                    retorno = UsuariosVm.ToView(Reg, empresa.NMFANTASIA);
                }
                else
                {
                    retorno = new UsuariosVm();
                }

            }

            return retorno;
        }

        public async Task<IDictionary<string, string>> Salvar(UsuariosVm model)
        {

            bool tudoCerto = false;

            var resultado = new Dictionary<string, string>();

            USUARIO_NETCARD usu_netCard;
            PAGNET_USUARIO usu_Concentrador;
            bool usuNetCardNovo = false;
            string Acao = "";

            if (model.CodUsuario == 0)
            {
                Acao = "INCLUSAO";
                PAGNET_USUARIO Newusu_Concentrador = new PAGNET_USUARIO();
                usu_netCard = new USUARIO_NETCARD();
                usuNetCardNovo = true;

                //Valida se já existe este login cadastrado e ativo
                var bValidaLogin = _usuPagNet.ValidaLoginExistente(model.Login + model.PerfilOperadora);
                if (bValidaLogin)
                {
                    resultado.Add("LOGIN", "Já existe um usuário cadastrado com este login");
                    return resultado;
                }

                //INCLUI DADOS NA TABELA DE USUÁRIO DO SERVIDOR DO CONCENTRADOR
                Newusu_Concentrador = UsuariosVm.ToEntity_PAGNET(model);

                usu_Concentrador = await _usuPagNet.IncluiUsuario(Newusu_Concentrador);
            }
            else
            {
                Acao = "ALTERACAO";
                var usu = _usuPagNet.GetUsuarioById(model.CodUsuario).Result;

                usu.NMUSUARIO = model.nmUsuario ;

                //caso esteja editando os campos do usuário com exceção do campo senha. 
                if (!String.IsNullOrEmpty(model.Password))
                    usu.SENHA = Help.CriptografarSenha(model.Password);


                usu_Concentrador = await _usuPagNet.AtualizaUsuario(usu);

            }
            
            /*
             * CASO O USUÁRIO NÃO SEJA UM USUÁRIO NOVO, FOI CRIADA A VERIFICAÇÃO A BAIXO PARA VALIDAR SE O USUARIO ESTÁ CADASTRADO
             * NO AMBIENTE DO NET CARD TAMBÉM. ISSO É NECESSÁRIO PARA O PRIMEIRO USUÁRIO DO SISTEMA. POIS ASSIM QUE O SISITEMA E VENDIDO, 
             * O PRIMEIRO USUÁRIO É DIRETO NO BANCO APENAS NA TABELA DE CONCENTRADOR.
             */
            if (model.CodUsuario != 0)
            {
                usu_netCard = _usuNetCard.GetUsuarioById(usu_Concentrador.CODUSUARIO).Result;

                if (usu_netCard == null)
                {
                    usuNetCardNovo = true;
                    usu_netCard.CODUSUARIO = usu_Concentrador.CODUSUARIO;
                    usu_netCard.CPF = usu_Concentrador.CPF;
                    usu_netCard.LOGIN = usu_Concentrador.LOGIN;
                    usu_netCard.CODEMPRESA = Convert.ToInt32(usu_Concentrador.CODEMPRESA);
                    usu_netCard.EMAIL = usu_Concentrador.EMAIL;
                    usu_netCard.ADMINISTRADOR = usu_Concentrador.ADMINISTRADOR;
                    usu_netCard.ATIVO = "S";
                    usu_netCard.VISIVEL = "S";
                }

                usu_netCard.NMUSUARIO = usu_Concentrador.NMUSUARIO;
                usu_netCard.SENHA = usu_Concentrador.SENHA;

            }
            else
            {
                usu_netCard = UsuariosVm.ToEntity_NetCard(usu_Concentrador);
            }

            try
            {
                if (usuNetCardNovo)
                    tudoCerto = await _usuNetCard.IncluiUsuario(usu_netCard);
                else
                    tudoCerto = await _usuNetCard.AtualizaUsuario(usu_netCard);


                EnviaDadosAcessoEmail(Acao, model);
                
            }        
             catch (ArgumentException ex)
             {
                if (!resultado.ContainsKey(ex.ParamName))
                    resultado.Add(ex.ParamName, ex.Message);
            }


             return resultado;
         }
        private string Abreviar(string s, bool nomesDoMeio)
        {

            // Quebro os nomes...
            string[] nomes = s.Split(' ');
            int inicio = 0;
            int fim = nomes.Length - 1;

            // Se eu não quiser abreviar o primeiro e o ultimo nome
            if (nomesDoMeio)
            {
                inicio = 1;                
                fim = nomes.Length - 2;
            }

            // Monto o retorno
            string retorno = "";

            for (int i = 0; i < nomes.Length; i++)
            {
                if (!PalavrasExcecoes(nomes[i]) && i >= inicio && i <= fim)
                    retorno += nomes[i][0] + ". ";
                else
                    retorno += nomes[i] + " ";
            }

            return retorno.Trim();
        }

        System.Text.RegularExpressions.Regex regExc = new System.Text.RegularExpressions.Regex("DA|da|DE|de|DO|do|DAS|das|DOS|dos");

        private bool PalavrasExcecoes(string palavra)
        {
            return regExc.Match(palavra).Success;
        }

        public async Task<List<ConsultaUsuario>> GetAllUsuarioByCodEmpresa(int CodEmpresa, int codOpe)
        {
            List<ConsultaUsuario> retorno;

            retorno = new List<ConsultaUsuario>();

            retorno = _usuPagNet.GetAllUsuarioByCodEmpresa(CodEmpresa, codOpe)
                .Select(x => new ConsultaUsuario(x)).ToList();

            return retorno;
        }
        private void EnviaDadosAcessoEmail(string Acao, UsuariosVm modal)
        {           
            var msg = "<b>Email enviado via sistema Pagnet</b> <br />";
            msg += "<b>Dados de acesso do sistema</b> <br /><br /><br />";
            msg += "<b>Solicitante: </b> " + modal.nmUsuario + " <br />";
            msg += "<b>Email para contato: </b> " + modal.Email + " <br />";
            msg += "<b>Login: </b> " + modal.Login + " <br />";
            msg += "<b>Senha: </b> " + modal.Password + " <br />";

            var smtpClient = new SmtpClient
            {
                Host = "smtp.task.com.br", // set your SMTP server name here
                Port = 587, // Port 
                EnableSsl = false,
                Credentials = new NetworkCredential("sistemapagnet@tln.com.br", "P@gn3t3lenet$#")
            };

            var message = new MailMessage();

            message.To.Add(modal.Email);

            message.From = new MailAddress("sistemapagnet@tln.com.br");
            if (Acao == "INCLUSAO")
            {
                message.Subject = "PagNet-Inclusão de usuário no sistema PagNet";
            }
            else
            {
                message.Subject = "PagNet-Alteracao de usuário no sistema PagNet";
            }
            message.IsBodyHtml = true;
            message.Body = msg;

            smtpClient.SendMailAsync(message);
        }
    }
}
