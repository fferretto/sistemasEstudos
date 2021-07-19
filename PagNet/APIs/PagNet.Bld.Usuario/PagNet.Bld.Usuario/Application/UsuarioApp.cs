using Microsoft.AspNetCore.Http;
using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Usuario.Abstraction.Interface;
using PagNet.Bld.Usuario.Abstraction.Interface.Model;
using PagNet.Bld.Usuario.Abstraction.Model;
using PagNet.Bld.Usuario.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Telenet.BusinessLogicModel;

namespace PagNet.Bld.Usuario.Application
{
    public class UsuarioApp : Service<IContextoApp>, IUsuarioApp
    {

        //protected ITokensRepository _TokensRepository { get; }
        protected IHttpContextAccessor _ContextAccessor { get; }
        private readonly IParametrosApp _user;
        private readonly IPAGNET_CADEMPRESAService _empresa;
        private readonly IPAGNET_USUARIOService _usuarioPagNet;
        private readonly IPAGNET_USUARIO_CONCENTRADORService _usuarioConcentrador;
        private readonly IOPERADORAService _operadora;

        public UsuarioApp(IContextoApp contexto,
                                IHttpContextAccessor contextAccessor,
                                //ITokensRepository tokensRepository,
                                IParametrosApp user,
                                IPAGNET_CADEMPRESAService empresa,
                                IPAGNET_USUARIOService usuarioPagNet,
                                IPAGNET_USUARIO_CONCENTRADORService usuarioConcentrador,
                                IOPERADORAService ope
                                )
            : base(contexto)
        {
            _usuarioPagNet = usuarioPagNet;
            _usuarioConcentrador = usuarioConcentrador;
            _empresa = empresa;
            _operadora = ope;
            _user = user;
            //_TokensRepository = tokensRepository;
            _ContextAccessor = contextAccessor;
        }

        System.Text.RegularExpressions.Regex regExc = new System.Text.RegularExpressions.Regex("DA|da|DE|de|DO|do|DAS|das|DOS|dos");

        public UsuarioModel GetUsuario(IFiltroModel filtro)
        {
            UsuarioModel retorno = new UsuarioModel();

            if (filtro.codigoUsuario > 0)
            {
                var Reg = _usuarioPagNet.GetUsuarioById(filtro.codigoUsuario);

                if (Reg != null)
                {
                    var empresa = _empresa.ConsultaEmpresaById((int)Reg.CODEMPRESA).Result;
                    
                    retorno.codigoUsuario = Reg.CODUSUARIO;
                    retorno.nomeUsuario = Reg.NMUSUARIO;
                    retorno.Login = Reg.LOGIN;
                    retorno.Cpf = Helper.FormataCPFCnPj(Reg.CPF);
                    retorno.Email = Reg.EMAIL.Trim();
                    retorno.Administrador = (Reg.ADMINISTRADOR == "S") ? true : false;
                    retorno.codigoEmpresa = Convert.ToString(Reg.CODEMPRESA);
                    retorno.nomeEmpresa = empresa.NMFANTASIA;
                    retorno.Password = "";
                    retorno.ConfirmPassword = "";
                }

            }

            return retorno;
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
        private bool PalavrasExcecoes(string palavra)
        {
            return regExc.Match(palavra).Success;
        }
        public RetornoModel Salvar(IUsuarioModel model)
        {
            bool tudoCerto = false;

            RetornoModel resultado = new RetornoModel();

            PAGNET_USUARIO usu_netCard;
            PAGNET_USUARIO_CONCENTRADOR usu_Concentrador;
            bool usuarioPagNetNovo = false;
            string Acao = "";

            if (model.codigoUsuario == 0)
            {
                Acao = "INCLUSAO";
                PAGNET_USUARIO_CONCENTRADOR Newusu_Concentrador = new PAGNET_USUARIO_CONCENTRADOR();
                usu_netCard = new PAGNET_USUARIO();
                usuarioPagNetNovo = true;

                //Valida se já existe este login cadastrado e ativo
                var bValidaLogin = _usuarioConcentrador.ValidaLoginExistente(model.Login);
                if (bValidaLogin)
                {
                    resultado.Sucesso = false;
                    resultado.msgResultado = "Já existe um usuário cadastrado com este login";
                    return resultado;
                }

                //INCLUI DADOS NA TABELA DE USUÁRIO DO SERVIDOR DO CONCENTRADOR

                Newusu_Concentrador.CODUSUARIO = model.codigoUsuario;
                Newusu_Concentrador.NMUSUARIO = model.nomeUsuario;
                Newusu_Concentrador.LOGIN = model.Login;
                Newusu_Concentrador.SENHA = Helper.CriptografarSenha(model.Password);
                Newusu_Concentrador.CPF = model.Cpf;
                Newusu_Concentrador.CODEMPRESA = Convert.ToInt32(model.codigoEmpresa);
                Newusu_Concentrador.EMAIL = model.Email;
                Newusu_Concentrador.CODOPE = Convert.ToInt16(_user.cod_ope);
                Newusu_Concentrador.ADMINISTRADOR = (model.Administrador) ? "S" : "N";
                Newusu_Concentrador.VISIVEL = "S";
                Newusu_Concentrador.ATIVO = "S";

                usu_Concentrador = _usuarioConcentrador.IncluiUsuario(Newusu_Concentrador);
            }
            else
            {
                Acao = "ALTERACAO";
                var usu = _usuarioConcentrador.GetUsuarioById(model.codigoUsuario);

                usu.NMUSUARIO = model.nomeUsuario;
                usu.LOGIN = model.Login;

                //caso esteja editando os campos do usuário com exceção do campo senha. 
                if (!String.IsNullOrEmpty(model.Password))
                    usu.SENHA = Helper.CriptografarSenha(model.Password);

                usu.CPF = Helper.RemoveCaracteres(model.Cpf);
                usu.CODOPE = Convert.ToInt16(_user.cod_ope);
                usu.CODEMPRESA = Convert.ToInt32(model.codigoEmpresa);
                usu.EMAIL = model.Email;
                usu.ADMINISTRADOR = (model.Administrador) ? "S" : "N";


                usu_Concentrador =  _usuarioConcentrador.AtualizaUsuario(usu);

            }

            /*
             * CASO O USUÁRIO NÃO SEJA UM USUÁRIO NOVO, FOI CRIADA A VERIFICAÇÃO A BAIXO PARA VALIDAR SE O USUARIO ESTÁ CADASTRADO
             * NO AMBIENTE DO NET CARD TAMBÉM. ISSO É NECESSÁRIO PARA O PRIMEIRO USUÁRIO DO SISTEMA. POIS ASSIM QUE O SISITEMA E VENDIDO, 
             * O PRIMEIRO USUÁRIO É DIRETO NO BANCO APENAS NA TABELA DE CONCENTRADOR.
             */
            if (model.codigoUsuario != 0)
            {
                usu_netCard = _usuarioPagNet.GetUsuarioById(usu_Concentrador.CODUSUARIO);

                if (usu_netCard == null)
                {
                    usuarioPagNetNovo = true;
                    usu_netCard.CODUSUARIO = usu_Concentrador.CODUSUARIO;
                }

                usu_netCard.NMUSUARIO = usu_Concentrador.NMUSUARIO;
                usu_netCard.LOGIN = usu_Concentrador.LOGIN;
                usu_netCard.SENHA = usu_Concentrador.SENHA;
                usu_netCard.CPF = usu_Concentrador.CPF;
                usu_netCard.CODEMPRESA = Convert.ToInt32(usu_Concentrador.CODEMPRESA);
                usu_netCard.EMAIL = usu_Concentrador.EMAIL;
                usu_netCard.ADMINISTRADOR = usu_Concentrador.ADMINISTRADOR;
                usu_netCard.ATIVO = "S";
                usu_netCard.VISIVEL = "S";

            }
            else
            {
                usu_netCard = new PAGNET_USUARIO();
                usu_netCard.CODUSUARIO = usu_Concentrador.CODUSUARIO;
                usu_netCard.NMUSUARIO = usu_Concentrador.NMUSUARIO;
                usu_netCard.LOGIN = usu_Concentrador.LOGIN;
                usu_netCard.SENHA = usu_Concentrador.SENHA;
                usu_netCard.CPF = usu_Concentrador.CPF;
                usu_netCard.CODEMPRESA = Convert.ToInt32(usu_Concentrador.CODEMPRESA);
                usu_netCard.EMAIL = usu_Concentrador.EMAIL;
                usu_netCard.ADMINISTRADOR = usu_Concentrador.ADMINISTRADOR;
                usu_netCard.VISIVEL = "S";
                usu_netCard.ATIVO = "S";
            }

            try
            {
                if (usuarioPagNetNovo)
                    tudoCerto = _usuarioPagNet.IncluiUsuario(usu_netCard);
                else
                    tudoCerto = _usuarioPagNet.AtualizaUsuario(usu_netCard);


                EnviaDadosAcessoEmail(Acao, model);

                resultado.Sucesso = true;
                resultado.msgResultado = "Usuário salvo com sucesso!";
                return resultado;


            }
            catch (ArgumentException ex)
            {
                resultado.Sucesso = false;
                resultado.msgResultado = ex.Message;
                return resultado;
            }
        }
        public RetornoModel Desativar(IFiltroModel filtro)
        {
            RetornoModel resultado = new RetornoModel();

            bool tudoCerto = false;
            try
            {
                tudoCerto = _usuarioPagNet.Desativa(filtro.codigoUsuario);
                tudoCerto = _usuarioConcentrador.Desativa(filtro.codigoUsuario);

                resultado.Sucesso = true;
                resultado.msgResultado = "Usuário desativado com sucesso.";
            }
            catch (ArgumentException ex)
            {
                resultado.Sucesso = false;
                resultado.msgResultado = ex.Message;
            }
            catch (Exception ex)
            {
                resultado.Sucesso = false;
                resultado.msgResultado = ex.Message;
            }
            return resultado;
        }
        public List<UsuarioModel> GetAllUsuarioByCodOpe(IFiltroModel filtro)
        {
            List<UsuarioModel> retorno = new List<UsuarioModel>();

            var dados = _usuarioConcentrador.GetAllUsuarioByCodOpe(filtro.codigoOperadora);

            UsuarioModel item = new UsuarioModel();
            foreach (var x in dados)
            {
                item = new UsuarioModel();
                item.codigoUsuario = x.CODUSUARIO;
                item.nomeUsuario = x.NMUSUARIO.Trim();
                item.Login = x.LOGIN.Trim();
                item.Email = x.EMAIL.Trim();
                retorno.Add(item);
            }

            return retorno;
        }
        public List<UsuarioModel> GetAllUsuarioByCodEmpresa(IFiltroModel filtro)
        {
            List<UsuarioModel> retorno = new List<UsuarioModel>();

            var dados = _usuarioConcentrador.GetAllUsuarioByCodEmpresa(filtro.codigoEmpresa ,filtro.codigoOperadora);

            UsuarioModel item = new UsuarioModel();
            foreach (var x in dados)
            {
                item = new UsuarioModel();
                item.codigoUsuario = x.CODUSUARIO;
                item.nomeUsuario = x.NMUSUARIO.Trim();
                item.Login = x.LOGIN.Trim();
                item.Email = x.EMAIL.Trim();
                retorno.Add(item);
            }

            return retorno;
        }
        public RetornoValidaUsuario ValidaLoginRecuperarSenha(IFiltroModel filtro)
        {
            RetornoValidaUsuario retorno = new RetornoValidaUsuario();
            if (!string.IsNullOrWhiteSpace(filtro.Login))
            {
                var dadosUsu = _usuarioConcentrador.BuscaUsuarioByLogin(filtro.Login);
                if(dadosUsu != null)
                {
                    retorno.Sucesso = true;
                    retorno.Login = dadosUsu.LOGIN;
                    retorno.Email = dadosUsu.EMAIL;
                    retorno.msgResultado = "Usuário encontrado!";
                }
                else
                {
                    retorno.Sucesso = false;
                    retorno.msgResultado = "Usuário não encontrado!";
                }
            }
            else if (!string.IsNullOrWhiteSpace(filtro.EmailUsuario))
            {
                var dadosUsu = _usuarioConcentrador.BuscaUsuarioByEmail(filtro.EmailUsuario);
                if (dadosUsu != null)
                {
                    retorno.Sucesso = true;
                    retorno.Login = dadosUsu.LOGIN;
                    retorno.Email = dadosUsu.EMAIL;
                    retorno.msgResultado = "Usuário encontrado!";
                }
                else
                {
                    retorno.Sucesso = false;
                    retorno.msgResultado = "Usuário não encontrado!";
                }
            }
            else
            {
                retorno.Sucesso = false;
                retorno.msgResultado = "Usuário não localizado";
            }

            return retorno;
        }
        public RetornoModel SalvaAlteracaoSenha(IUsuarioModel model)
        {
            bool tudoCerto = false;

            RetornoModel resultado = new RetornoModel();

            PAGNET_USUARIO usu_netCard;
            PAGNET_USUARIO_CONCENTRADOR usu_Concentrador;
            string Acao = "";

                Acao = "ALTERACAO";
                var usu = _usuarioConcentrador.GetUsuarioById(model.codigoUsuario);
            
                //caso esteja editando os campos do usuário com exceção do campo senha. 
                usu.SENHA = Helper.CriptografarSenha(model.Password);
                usu_Concentrador = _usuarioConcentrador.AtualizaUsuario(usu);            

            /*
             * CASO O USUÁRIO NÃO SEJA UM USUÁRIO NOVO, FOI CRIADA A VERIFICAÇÃO A BAIXO PARA VALIDAR SE O USUARIO ESTÁ CADASTRADO
             * NO AMBIENTE DO NET CARD TAMBÉM. ISSO É NECESSÁRIO PARA O PRIMEIRO USUÁRIO DO SISTEMA. POIS ASSIM QUE O SISITEMA E VENDIDO, 
             * O PRIMEIRO USUÁRIO É DIRETO NO BANCO APENAS NA TABELA DE CONCENTRADOR.
             */
            
                usu_netCard = _usuarioPagNet.GetUsuarioById(usu_Concentrador.CODUSUARIO);
                usu_netCard.SENHA = usu_Concentrador.SENHA;
                

            try
            {
                tudoCerto = _usuarioPagNet.AtualizaUsuario(usu_netCard);

                EnviaDadosAcessoEmail(Acao, model);

                resultado.Sucesso = true;
                resultado.msgResultado = "Senha alterada com sucesso!";
                return resultado;


            }
            catch (ArgumentException ex)
            {
                resultado.Sucesso = false;
                resultado.msgResultado = ex.Message;
                return resultado;
            }
        }
        private void EnviaDadosAcessoEmail(string Acao, IUsuarioModel modal)
        {
            var msg = "<b>Email enviado via sistema Pagnet</b> <br />";
            msg += "<b>Dados de acesso do sistema</b> <br /><br /><br />";
            msg += "<b>Solicitante: </b> " + modal.nomeUsuario + " <br />";
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
