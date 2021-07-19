using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Linq;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_OCORRENCIARETBOLService : ServiceBase<PAGNET_OCORRENCIARETBOL>, IPAGNET_OCORRENCIARETBOLService
    {
        private readonly IPAGNET_OCORRENCIARETBOLRepository _rep;

        public PAGNET_OCORRENCIARETBOLService(IPAGNET_OCORRENCIARETBOLRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public string ReturnOcorrencia(string id)
        {
            var ocorrencia = _rep.Get(x => x.codOcorrenciaRetBol == id).FirstOrDefault();

            return ocorrencia.Descricao;
        }
    }
}

