using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_InstrucaoEmailRepository : RepositoryBase<PAGNET_INSTRUCAOEMAIL>, IPagNet_InstrucaoEmailRepository
    {
        public PagNet_InstrucaoEmailRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_INSTRUCAOEMAIL.Select(p => p.CODINSTRUCAOEMAIL).DefaultIfEmpty(0).Max();
        }
    }
}
