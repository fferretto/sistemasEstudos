using NetCard.Common.Util;

namespace NetCard.Common.Models
{
    public class DetalheMovimento
    {
        public DetalheMovimento(int titular, string cartao, string data, string autorizacao, decimal valor,
                                string tipo, string estabelecimento)
        {
            Titular = (titular == 0 ? "Titular" : "Dependente");
            Cartao = cartao;
            Data = data;
            Autorizacao = autorizacao;
            Valor = valor;
            Tipo = tipo;
            Estabelecimento = estabelecimento;
        }

        public DetalheMovimento(int titular, string cartao, string data, int lote, string autorizacao, decimal valor,
                                string tipo, string codEstab, string estabelecimento)
        {
            Titular = (titular == 0 ? "Titular" : "Dependente");
            Cartao = cartao;
            Data = data;
            Lote = lote;
            Autorizacao = autorizacao;
            Valor = valor;
            Tipo = tipo;
            CodEstab = codEstab;
            Estabelecimento = estabelecimento;
        }

        public DetalheMovimento(int titular, string cartao, string data, string autorizacao, decimal valor,
                               string tipo, string trans, string codEstab, string estabelecimento)
        {
            Titular = (titular == 0 ? "Titular" : "Dependente");
            Cartao = cartao;
            Data = data;
            Autorizacao = autorizacao;
            Valor = valor;
            Tipo = tipo;
            Trans = trans;
            CodEstab = codEstab;
            Estabelecimento = estabelecimento;
        }

        public string Titular { get; set; }
        public string Cartao { get; set; }
        public string CodCrtMask { get { return Cartao != null ? Utils.MascaraCartao(Cartao, 17) : string.Empty; } }
        public string Data { get; set; }
        public int Lote { get; set; }
        public string Autorizacao { get; set; }
        public decimal Valor { get; set; }
        public string Tipo { get; set; }
        public string Trans { get; set; }
        public string Estabelecimento { get; set; }
        public string Descricao { get; set; }
        public string CodEstab { get; set; }
    }
}
