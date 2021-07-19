using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IUsuario_NetCardService : IServiceBase<USUARIO_NETCARD>
    {
        Task<USUARIO_NETCARD> GetUsuarioById(int id);
        Task<bool> IncluiUsuario(USUARIO_NETCARD Usuario);
        Task<bool> AtualizaUsuario(USUARIO_NETCARD Usuario);
        IEnumerable<USUARIO_NETCARD> GetAllUsuario();
        Task<bool> Desativa(int CodUsuario);
    }
}
