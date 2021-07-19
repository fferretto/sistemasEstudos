using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public sealed class LoginValidation
    {
        public LoginValidation(bool resultado, string mensagem, string acao, IList<DadosAcesso> dadosAcesso)
        {
            Resultado = resultado;
            Mensagem = mensagem;
            Acao = acao;
            DadosAcesso = dadosAcesso;
        }

        public bool Resultado { get; private set; }
        public string Mensagem { get; private set; }
        public string Acao { get; private set; }
        public IList<DadosAcesso> DadosAcesso { get; private set; }
    }
}
