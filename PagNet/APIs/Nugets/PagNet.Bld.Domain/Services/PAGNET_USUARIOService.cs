using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_USUARIOService : ServiceBase<PAGNET_USUARIO>, IPAGNET_USUARIOService
    {
        private readonly IPAGNET_USUARIORepository _rep;

        public PAGNET_USUARIOService(IPAGNET_USUARIORepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public bool AtualizaUsuario(PAGNET_USUARIO Usuario)
        {
            try
            {
                _rep.Update(Usuario);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PAGNET_USUARIO> GetAllUsuario()
        {
            return _rep.Get(x => x.ATIVO == "S" && x.VISIVEL == "S").OrderBy(x => x.NMUSUARIO);
        }

        public PAGNET_USUARIO GetUsuarioById(int id)
        {
            return _rep.Get(x => x.CODUSUARIO == id).FirstOrDefault();
        }

        public bool IncluiUsuario(PAGNET_USUARIO Usuario)
        {
            try
            { 
                _rep.Add(Usuario);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public PAGNET_USUARIO BuscaUsuarioAleatorioByEmpresa(int codigoEmpresa)
        {
            return _rep.Get(x => x.ATIVO == "S" &&
                                 x.CODEMPRESA == codigoEmpresa &&
                                 x.VISIVEL == "S"
                           ).FirstOrDefault();
        }
    }
}
