using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
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
