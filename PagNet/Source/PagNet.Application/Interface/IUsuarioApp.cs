using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface
{
    public interface IUsuarioApp
    {

        UsuariosVm GetUsuario(int? id);
        Task<IDictionary<string, string>> Salvar(UsuariosVm model);
        Task<IDictionary<string, string>> Desativar(int id);
        Task<List<ConsultaUsuario>> GetAllUsuarioByCodOpe(int CodOpe);
        Task<List<ConsultaUsuario>> GetAllUsuarioByCodEmpresa(int CodEmpresa, int codOpe);
    }
}
