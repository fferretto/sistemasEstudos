namespace PagNet.Bld.Domain.Interface
{
    public interface IParametrosApp
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
        bool PossuiNetCard { get; }
        string GetConnectionStringPagNet();
        string GetConnectionStringConcentrador();
    }
}
