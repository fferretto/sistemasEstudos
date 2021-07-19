using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Linq;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_OCORRENCIARETPAGService : ServiceBase<PAGNET_OCORRENCIARETPAG>, IPAGNET_OCORRENCIARETPAGService
    {
        private readonly IPAGNET_OCORRENCIARETPAGRepository _rep;

        public PAGNET_OCORRENCIARETPAGService(IPAGNET_OCORRENCIARETPAGRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public string ReturnOcorrencia(string id)
        {
            var ocorrencia = _rep.Get(x => x.codOcorrenciaRetPag == id).FirstOrDefault();

            if (ocorrencia == null) return "";

            return ocorrencia.Descricao;
        }
    }
}
