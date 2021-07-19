using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services.Common;
using System.Linq;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_API_PGTOService : ServiceBase<PAGNET_API_PGTO>, IPAGNET_API_PGTOService
    {
        private readonly IPAGNET_API_PGTORepository _rep;

        public PAGNET_API_PGTOService(IPAGNET_API_PGTORepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public string RetornaCaminhoAPI(string codigoBanco)
        {
            string caminho = "";
            var dados = _rep.Get(x => x.CODIGOBANCO == codigoBanco).FirstOrDefault();

            if(dados != null)
            {
                caminho = dados.CAMINHO;
            }

            return caminho;

        }
    }
}
