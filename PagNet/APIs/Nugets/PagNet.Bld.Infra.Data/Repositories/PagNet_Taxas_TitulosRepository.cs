using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_TAXAS_TITULOSRepository : RepositoryBase<PAGNET_TAXAS_TITULOS>, IPAGNET_TAXAS_TITULOSRepository
    {
        public PAGNET_TAXAS_TITULOSRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_TAXAS_TITULOS.Select(p => p.CODTAXATITULO).DefaultIfEmpty(0).Max();
        }
    }
}
