using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System.Linq;

namespace PagNet.Domain.Services
{
    public class PagNet_InstrucaoCobrancaService : ServiceBase<PAGNET_INSTRUCAOCOBRANCA>, IPagNet_InstrucaoCobrancaService
    {
        private readonly IPagNet_InstrucaoCobrancaRepository _rep;

        public PagNet_InstrucaoCobrancaService(IPagNet_InstrucaoCobrancaRepository rep)
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
