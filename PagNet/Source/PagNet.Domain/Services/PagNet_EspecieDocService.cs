using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;

namespace PagNet.Domain.Services
{
    public class PagNet_EspecieDocService : ServiceBase<PAGNET_ESPECIEDOC>, IPagNet_EspecieDocService
    {
        private readonly IPagNet_EspecieDocRepository _rep;

        public PagNet_EspecieDocService(IPagNet_EspecieDocRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

    }
}
