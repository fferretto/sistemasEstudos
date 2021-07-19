namespace PagNet.Infra.Data.Context
{
    public interface IPagNetUser
    {
        string ServidorDadosAutorizador { get; }
        string BdAutorizador { get; }
        string ServidorDadosNetCard { get; }
        string BdNetCard { get; }
        string nmPerfilOperadora { get; }
        string login_usu { get; }
        string nmUsuario { get; }

        int cod_usu { get; }
        int cod_ope { get; }
        int cod_empresa { get; }

        bool isAdministrator { get; }
        bool isTelenet { get; }
        bool PossuiNetCard { get; }

        string GetConnectionStringNetCard();
        string GetConnectionStringAutorizador();

    }
}
