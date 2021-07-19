namespace NetCard.Common.Models
{
    public class SubstituirCartaoUsuario
    {
        public string CodigoUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string Cpf { get; set; }
        public string Filial { get; set; }
        public string Setor { get; set; }
        public string Matricula { get; set; }
        public decimal Limite { get; set; }
        public string NumeroCartao { get; set; }
        public string CartaoMasc { get; set; }
        public string SaldoCartao { get; set; }
        public string StatusCartao { get; set; }
        public bool Existe { get; set; }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
    }
}
