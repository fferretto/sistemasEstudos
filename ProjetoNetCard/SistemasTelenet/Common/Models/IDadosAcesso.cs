using System;

namespace NetCard.Common.Models
{
    public interface IDadosAcesso
    {
        string Acesso { get; }
        int SistemaAcessado { get; }
        Sistema Sistema { get; }
        string Nome { get; }
        int Codigo { get; }
        string Login { get; }
        string Cnpj { get; }
        int Id { get; }
        int TipProd { get; }
        int CodParceria { get; }
        string TipoAcesso { get; }
        string CnpjOpe { get; }
        string RazaoOpe { get; }
        decimal Saldo { get; }
        int CodCen { get; }
        string CodCrt { get; }
        int NumDep { get; }
        string CodCrtMask { get; }
        string Cpf { get; }
        string Bairro { get; }
        string Cep { get; }
        string Cidade { get; }
        string Endereco { get; }
        string Complemento { get; }
        string Email { get; }
        string Uf { get; }
        string Matricula { get; }
        DateTime DataNascimento { get; }
        string Telefone { get; }
        string Recarga { get; }
        string Status { get; }
        string CentroCusto { get; }
        string FlagVA { get; }
        string FlagPJ { get; }
        string HabMaxParc { get; }
        string HabPremio { get; }
        string HabLimDep { get; }
        string HabIncDep { get; }
        string HabExcDep { get; }
        string HabCancMass { get; }
        string HabTransfSal { get; }
        string HabBeneficios { get; }
        string HabMobile { get; }
        bool HabTokencomCad { get; }
        bool HabFidelidade { get; }
        bool AdesaoPontuacao { get; }
        bool HasPergSecreta { get; }
        DateTime DtAdesaoFidelidade { get; }
        string HorIniAltLim { get; }
        string QtMaxAltLim { get; }
        string PcMaxAltLim { get; }
        string HorFimAltLim { get; }
        string Conpmo { get; }
        string UltAcesso { get; }
        bool TemSenha { get; }
    }
}
