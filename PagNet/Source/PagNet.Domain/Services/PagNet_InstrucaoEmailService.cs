using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Domain.Services
{
    public class PagNet_InstrucaoEmailService : ServiceBase<PAGNET_INSTRUCAOEMAIL>, IPagNet_InstrucaoEmailService
    {
        private readonly IPagNet_InstrucaoEmailRepository _rep;

        public PagNet_InstrucaoEmailService(IPagNet_InstrucaoEmailRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public void AtualizaEmail(PAGNET_INSTRUCAOEMAIL conta)
        {
            _rep.Update(conta);
        }

        public async Task<PAGNET_INSTRUCAOEMAIL> IncluirEmail(PAGNET_INSTRUCAOEMAIL conta)
        {
            var cod = _rep.GetMaxKey();
            conta.CODINSTRUCAOEMAIL = cod;

            _rep.AddAsync(conta);

            return conta;
        }

        public async Task<PAGNET_INSTRUCAOEMAIL> GetInstrucaoById(int id)
        {
            return _rep.Get(x => x.CODINSTRUCAOEMAIL == id).FirstOrDefault();
        }

        public async Task<List<PAGNET_INSTRUCAOEMAIL>> ConsultaInstrucaoById(int codEmpresa)
        {
            var instrucao = _rep.Get(x => x.CODEMPRESA == codEmpresa).ToList();

            return instrucao;
        }

    }
}
