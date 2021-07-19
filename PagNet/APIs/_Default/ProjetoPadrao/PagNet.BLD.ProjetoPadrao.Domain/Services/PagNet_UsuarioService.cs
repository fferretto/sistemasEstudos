using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class PagNet_UsuarioService : ServiceBase<PAGNET_USUARIO>, IPagNet_UsuarioService
    {
        private readonly IPagNet_UsuarioRepository _rep;

        public PagNet_UsuarioService(IPagNet_UsuarioRepository rep)
            : base(rep)
        {
            _rep = rep;
        }
        public bool ValidaLoginExistente(string _login)
        {
            var ExisteLogin = false;

            var usu = _rep.Get(x => x.LOGIN == _login && x.ATIVO == "S").FirstOrDefault();

            if (usu != null)
            {
                ExisteLogin = true;
            }

            return ExisteLogin;
        }

        public async Task<PAGNET_USUARIO> AtualizaUsuario(PAGNET_USUARIO Usuario)
        {
            _rep.Update(Usuario);

            return Usuario;
        }

        public async Task<bool> Desativa(int CodUsuario)
        {
            var usuario = GetUsuarioById(CodUsuario).Result;

            try
            {
                usuario.ATIVO = "N";
                _rep.Update(usuario);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PAGNET_USUARIO> GetAllUsuarioByCodOpe(int codOpe)
        {
            return _rep.Get(x => x.ATIVO == "S" && 
                                 x.CODOPE == codOpe && 
                                 x.VISIVEL == "S"
                           ).OrderBy(x => x.NMUSUARIO);
        }

        public async Task<PAGNET_USUARIO> GetUsuarioById(int id)
        {
            return _rep.Get(x => x.CODUSUARIO == id, "OPERADORAS")
                       .FirstOrDefault();
        }

        public async Task<PAGNET_USUARIO> IncluiUsuario(PAGNET_USUARIO Usuario)
        {
             _rep.Add(Usuario);

            return Usuario;

        }

        public IEnumerable<PAGNET_USUARIO> GetAllUsuarioByCodEmpresa(int codigoEmpresa, int codOpe)
        {
            return _rep.Get(x => x.ATIVO == "S" &&
                                 x.CODEMPRESA == codigoEmpresa &&
                                 x.CODOPE == codOpe &&
                                 x.VISIVEL == "S"
                           ).OrderBy(x => x.NMUSUARIO);
        }
    }
}
