using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_USUARIO_CONCENTRADORService : ServiceBase<PAGNET_USUARIO_CONCENTRADOR>, IPAGNET_USUARIO_CONCENTRADORService
    {
        private readonly IPAGNET_USUARIO_CONCENTRADORRepository _rep;

        public PAGNET_USUARIO_CONCENTRADORService(IPAGNET_USUARIO_CONCENTRADORRepository rep)
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

        public PAGNET_USUARIO_CONCENTRADOR AtualizaUsuario(PAGNET_USUARIO_CONCENTRADOR Usuario)
        {
            _rep.Update(Usuario);

            return Usuario;
        }

        public bool Desativa(int CodUsuario)
        {
            var usuario = GetUsuarioById(CodUsuario);

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

        public IEnumerable<PAGNET_USUARIO_CONCENTRADOR> GetAllUsuarioByCodOpe(int codOpe)
        {
            return _rep.Get(x => x.ATIVO == "S" && 
                                 x.CODOPE == codOpe && 
                                 x.VISIVEL == "S"
                           ).OrderBy(x => x.NMUSUARIO);
        }

        public PAGNET_USUARIO_CONCENTRADOR GetUsuarioById(int id)
        {
            return _rep.Get(x => x.CODUSUARIO == id, "OPERADORAS")
                       .FirstOrDefault();
        }

        public PAGNET_USUARIO_CONCENTRADOR IncluiUsuario(PAGNET_USUARIO_CONCENTRADOR Usuario)
        {
             _rep.Add(Usuario);

            return Usuario;

        }

        public IEnumerable<PAGNET_USUARIO_CONCENTRADOR> GetAllUsuarioByCodEmpresa(int codigoEmpresa, int codOpe)
        {
            return _rep.Get(x => x.ATIVO == "S" &&
                                 x.CODEMPRESA == codigoEmpresa &&
                                 x.CODOPE == codOpe &&
                                 x.VISIVEL == "S"
                           ).OrderBy(x => x.NMUSUARIO);
        }
        public PAGNET_USUARIO_CONCENTRADOR BuscaUsuarioAleatorioByEmpresa(int codigoEmpresa)
        {
            return _rep.Get(x => x.ATIVO == "S" &&
                                 x.CODEMPRESA == codigoEmpresa &&
                                 x.VISIVEL == "S"
                           ).FirstOrDefault();
        }

        public PAGNET_USUARIO_CONCENTRADOR BuscaUsuarioByEmail(string email)
        {
            return _rep.Get(x => x.ATIVO == "S" &&
                                 x.EMAIL == email &&
                                 x.VISIVEL == "S"
                           ).FirstOrDefault();
        }

        public PAGNET_USUARIO_CONCENTRADOR BuscaUsuarioByLogin(string login)
        {
            return _rep.Get(x => x.ATIVO == "S" &&
                                 x.LOGIN == login &&
                                 x.VISIVEL == "S"
                           ).FirstOrDefault();
        }
    }
}
