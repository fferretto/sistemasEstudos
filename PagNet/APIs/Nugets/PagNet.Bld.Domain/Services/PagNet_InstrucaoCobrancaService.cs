using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Linq;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_INSTRUCAOCOBRANCAService : ServiceBase<PAGNET_INSTRUCAOCOBRANCA>, IPAGNET_INSTRUCAOCOBRANCAService
    {
        private readonly IPAGNET_INSTRUCAOCOBRANCARepository _rep;

        public PAGNET_INSTRUCAOCOBRANCAService(IPAGNET_INSTRUCAOCOBRANCARepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public object[][] GetHashInstrucaoCobranca()
        {
            return _rep.Get(x => x.bAtivo)
                .Select(x => new object[] { x.codInstrucaoCobranca, x.nmInstrucaoCobranca }).ToArray();
        }

        public string GetInstrucaoCobrancaById(int codInstrucaoCobranca)
        {
            var instrucao = _rep.Get(x => x.codInstrucaoCobranca == codInstrucaoCobranca).FirstOrDefault();

            return instrucao.nmInstrucaoCobranca;
        }
    }
}
