using NetCard.Common.Util;
using System;

namespace NetCard.Common.Models
{
    public class DadosAcesso : IDadosAcesso
    {
        public string Acesso { get; set; }
        public int SistemaAcessado { get; set; }
        public Sistema Sistema { get; set; }
        public string Nome { get; set; }
        public int Codigo { get; set; }
        public string Login { get; set; }
        public string Cnpj { get; set; }
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int NumTit { get; set; }
        public int TipProd { get; set; }
        public int CodParceria { get; set; }
        public string TipoAcesso { get; set; }
        public string CnpjOpe { get; set; }
        public string RazaoOpe { get; set; }
        public decimal Saldo { get; set; }
        public int CodCen { get; set; }
        public string CodCrt { get; set; }
        public int NumDep { get; set; }
        public string CodCrtMask { get { return CodCrt != null ? Utils.MascaraCartao(CodCrt, 17) : string.Empty; } }
        public string Cpf { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public string Email { get; set; }
        public string Uf { get; set; }
        public string Matricula { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Recarga { get; set; }
        public string Status { get; set; }
        public string CentroCusto { get; set; }
        public string FlagVA { get; set; }
        public string FlagPJ { get; set; }
        public string HabMaxParc { get; set; }
        public string HabPremio { get; set; }
        public string HabLimDep { get; set; }
        public string HabIncDep { get; set; }
        public string HabExcDep { get; set; }
        public string HabCancMass { get; set; }
        public string HabTransfSal { get; set; }
        public string HabBeneficios { get; set; }
        public string HabMobile { get; set; }
        public bool HabTokencomCad { get; set; }
        public bool HabFidelidade { get; set; }
        public bool AceiteTermoConsentimento { get; set; }
        public bool AdesaoPontuacao { get; set; }
        public bool HasPergSecreta { get; set; }
        public DateTime DtAdesaoFidelidade { get; set; }
        public string HorIniAltLim { get; set; }
        public string QtMaxAltLim { get; set; }
        public string PcMaxAltLim { get; set; }
        public string HorFimAltLim { get; set; }        
        public string Conpmo { get; set; }
        public string UltAcesso { get; set; }
        public bool TemSenha { get; set; }
        
    }
}