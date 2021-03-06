using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;


namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_CadClienteRepository : RepositoryBase<PAGNET_CADCLIENTE>, IPagNet_CadClienteRepository
    {
        public PagNet_CadClienteRepository(ContextNetCard context)
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
    public class PagNet_CadCliente_LogRepository : RepositoryBase<PAGNET_CADCLIENTE_LOG>, IPagNet_CadCliente_LogRepository
    {
        public PagNet_CadCliente_LogRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CADCLIENTE_LOG.Select(p => p.CODCLIENTE_LOG).DefaultIfEmpty(0).Max();
        }
    }
}
