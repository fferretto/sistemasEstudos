using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System.Linq;

namespace PagNet.Domain.Services
{
    public class PagNet_CodigoOcorrenciaService : ServiceBase<PAGNET_CODIGOOCORRENCIA>, IPagNet_CodigoOcorrenciaService
    {
        private readonly IPagNet_CodigoOcorrenciaRepository _rep;

        public PagNet_CodigoOcorrenciaService(IPagNet_CodigoOcorrenciaRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public object[][] GetTiposOcorrencias()
        {
            return _rep.Get(x => x.ATIVO == "S")
                .Select(x => new object[] { x.codOcorrencia, x.nmOcorrencia }).ToArray();
        }
    }
}
