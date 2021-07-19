using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class Usuario_NetCardService : ServiceBase<USUARIO_NETCARD>, IUsuario_NetCardService
    {
        private readonly IUsuario_NetCardRepository _rep;

        public Usuario_NetCardService(IUsuario_NetCardRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public async Task<bool> AtualizaUsuario(USUARIO_NETCARD Usuario)
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

        public IEnumerable<USUARIO_NETCARD> GetAllUsuario()
        {
            return _rep.Get(x => x.ATIVO == "S" && x.VISIVEL == "S").OrderBy(x => x.NMUSUARIO);
        }

        public async Task<USUARIO_NETCARD> GetUsuarioById(int id)
        {
            return _rep.Get(x => x.CODUSUARIO == id).FirstOrDefault();
        }

        public async Task<bool> IncluiUsuario(USUARIO_NETCARD Usuario)
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
    }
}
