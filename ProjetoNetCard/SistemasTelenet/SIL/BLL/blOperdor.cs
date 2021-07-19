using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TELENET.SIL.PO;
using TELENET.SIL.DA;
using SIL.BLL;


namespace TELENET.SIL.BL
{
	public class blOperador
	{
		OPERADORA FOperador; 

		public blOperador(){}

		public string Autenticar(string Login, string Senha)
		{
			string MsgErro;
			MsgErro = string.Empty;

			daOperador daOperador = new daOperador();

			//daOperador.AutenticarUsuario(login:Login, senha: Senha);

			FOperador = daOperador.Autenticar(Login, Senha, out MsgErro);

			if (!FOperador.AUTENTICADO)
				MsgErro = "Erro de autenticacao: " + MsgErro;

			return MsgErro;
		}

        public string Autenticar2(string Login, string Senha, out string Acao)
        {            
            var MsgErro = string.Empty;
            Acao = string.Empty;
            Senha = Senha.Length > 8 ? Senha.Substring(0, 8) : Senha;            
            daOperador daOperador = new daOperador();
            FOperador = daOperador.Autenticar2(Login, Senha, out MsgErro, out Acao);            
            return MsgErro;
        }

        public string AlterarSenha(string Login, string Senha, string NovaSenha)
        {
            var MsgErro = string.Empty;
            Senha = Senha.Length > 8 ? Senha.Substring(0, 8) : Senha;
            NovaSenha = NovaSenha.Length > 8 ? NovaSenha.Substring(0, 8) : NovaSenha;

            daOperador daOperador = new daOperador();
            FOperador = daOperador.AlterarSenha(Login, Senha, NovaSenha, out MsgErro);
            MsgErro = MsgErro == "OK" ? string.Empty : MsgErro;
            return MsgErro;
        }

		public OPERADORA Operador
		{
			get {return FOperador;}
		}
	}
}
