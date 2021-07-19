using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_USUARIOService : IServiceBase<PAGNET_USUARIO>
    {
        PAGNET_USUARIO GetUsuarioById(int id);
        bool IncluiUsuario(PAGNET_USUARIO Usuario);
        bool AtualizaUsuario(PAGNET_USUARIO Usuario);
        IEnumerable<PAGNET_USUARIO> GetAllUsuario();
        bool Desativa(int CodUsuario);
        PAGNET_USUARIO BuscaUsuarioAleatorioByEmpresa(int codigoEmpresa);

    }
}
