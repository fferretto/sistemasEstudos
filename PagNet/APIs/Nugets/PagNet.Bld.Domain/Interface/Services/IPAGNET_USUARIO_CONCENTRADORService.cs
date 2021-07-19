using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_USUARIO_CONCENTRADORService : IServiceBase<PAGNET_USUARIO_CONCENTRADOR>
    {
        PAGNET_USUARIO_CONCENTRADOR GetUsuarioById(int id);
        PAGNET_USUARIO_CONCENTRADOR IncluiUsuario(PAGNET_USUARIO_CONCENTRADOR Usuario);
        PAGNET_USUARIO_CONCENTRADOR AtualizaUsuario(PAGNET_USUARIO_CONCENTRADOR Usuario);
        bool Desativa(int CodUsuario);
        bool ValidaLoginExistente(string login);
        IEnumerable<PAGNET_USUARIO_CONCENTRADOR> GetAllUsuarioByCodOpe(int id);
        IEnumerable<PAGNET_USUARIO_CONCENTRADOR> GetAllUsuarioByCodEmpresa(int codigoEmpresa, int codOpe);
        PAGNET_USUARIO_CONCENTRADOR BuscaUsuarioAleatorioByEmpresa(int codigoEmpresa);

        PAGNET_USUARIO_CONCENTRADOR BuscaUsuarioByEmail(string email);
        PAGNET_USUARIO_CONCENTRADOR BuscaUsuarioByLogin(string login);


    }
}
