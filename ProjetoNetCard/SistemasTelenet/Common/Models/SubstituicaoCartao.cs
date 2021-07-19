namespace NetCard.Common.Models
{
    public class SubstituicaoCartao
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string CpfOrigem { get; set; }
        public string CpfDestino { get; set; }
        public string CartaoOrigem { get; set; }
        public string CartaoDestino { get; set; }                
        public string NomeCliente { get; set; }
        public string CodigoCliente { get; set; }
        public string BancoAutorizador { get; set; }
    }
}
