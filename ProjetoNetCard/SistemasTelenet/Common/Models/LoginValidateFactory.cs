using NetCard.Common.Util;

namespace NetCard.Common.Models
{
    public static class LoginValidateFactory
    {
        public static ILoginValidate CreateValidator(ObjConn objConexao, string tipoAcesso)
        {
            switch (tipoAcesso)
            {
                case Constantes.cliente:
                    return new ClienteLoginValidate(objConexao);
                case Constantes.credenciado:
                    return new CredenciadoLoginValidate(objConexao);
                case Constantes.fidelidade:
                    return new FidelidateLoginValidate(objConexao);
                case Constantes.parceria:
                    return new ParceriaLoginValidate(objConexao);
                case Constantes.usuario:
                default:
                    return new UsuarioLoginValidate(objConexao);
            }
        }
    }
}
