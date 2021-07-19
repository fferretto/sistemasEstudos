using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_CADCLIENTERepository : RepositoryBase<PAGNET_CADCLIENTE>, IPAGNET_CADCLIENTERepository
    {
        public PAGNET_CADCLIENTERepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            var codigoRetorno = DbNetCard.PAGNET_CADCLIENTE.Select(p => p.CODCLIENTE).DefaultIfEmpty(0).Max();

            if (codigoRetorno < 100000)
            {
                codigoRetorno = 100000;
            }

            return codigoRetorno + 1;
        }

    }
    public class PAGNET_CADCLIENTE_LOGRepository : RepositoryBase<PAGNET_CADCLIENTE_LOG>, IPAGNET_CADCLIENTE_LOGRepository
    {
        public PAGNET_CADCLIENTE_LOGRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CADCLIENTE_LOG.Select(p => p.CODCLIENTE_LOG).DefaultIfEmpty(0).Max();
        }
    }
}
