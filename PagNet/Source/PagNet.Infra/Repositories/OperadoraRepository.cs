using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;

namespace PagNet.Infra.Data.Repositories
{
    public class OperadoraRepository : RepositoryBase<OPERADORA>, IOperadoraRepository
    {
        public OperadoraRepository(ContextConcentrador context)
            : base(context)
        { }
    }
}
