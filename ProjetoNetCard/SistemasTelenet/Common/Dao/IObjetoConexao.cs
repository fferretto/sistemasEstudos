
namespace NetCard.Common.Models
{
    public interface IObjetoConexao
    {
        string Acesso { get; }
        string CodCli { get; }
        string CodCre { get; }
        string CodCrt { get; }
        int CodOpe { get; }
        string Cpf { get; }
        string BancoAutorizador { get; }
        string BancoConcentrador { get; }
        string BancoNetcard { get; }
        string LibDelphiAutorizador { get; }
        string LibDelphiNetcard { get; }
        string LoginWeb { get; }
        string NomOperadora { get; }
        string ServAutorizador { get; }
        string ServConcentrador { get; }
        string ServNertCard { get; }
        string StaCli { get; }
    }
}
