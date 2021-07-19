using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface INETCARD_USUARIOPOSService : IServiceBase<NETCARD_USUARIOPOS>
    {
        NETCARD_USUARIOPOS BuscaUsuarioPosByCPF(string cpf);
        NETCARD_USUARIOPOS BuscaUsuarioPosByMatricula(string matricula);
        List<NETCARD_USUARIOPOS> BuscaUsuarioTodosPosByCliente(int codigoCliente);
    }
    public interface INETCARD_USUARIOPREService : IServiceBase<NETCARD_USUARIOPRE>
    {
        NETCARD_USUARIOPRE BuscaUsuarioPreByCPF(string cpf);

        NETCARD_USUARIOPRE BuscaUsuarioPreByMatricula(string matricula);
    }
}
