using PagNet.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_UsuarioService : IServiceBase<PAGNET_USUARIO>
    {
        Task<PAGNET_USUARIO> GetUsuarioById(int id);
        Task<PAGNET_USUARIO> IncluiUsuario(PAGNET_USUARIO Usuario);
        Task<PAGNET_USUARIO> AtualizaUsuario(PAGNET_USUARIO Usuario);
        Task<bool> Desativa(int CodUsuario);
        bool ValidaLoginExistente(string login);
        IEnumerable<PAGNET_USUARIO> GetAllUsuarioByCodOpe(int id);
        IEnumerable<PAGNET_USUARIO> GetAllUsuarioByCodEmpresa(int codigoEmpresa, int codOpe);
        PAGNET_USUARIO BuscaUsuarioAleatorioByEmpresa(int codigoEmpresa);


    }
}
