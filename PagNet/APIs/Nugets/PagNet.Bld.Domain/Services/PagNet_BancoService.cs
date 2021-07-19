using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_BANCOService : ServiceBase<PAGNET_BANCO>, IPAGNET_BANCOService
    {
        private readonly IPAGNET_BANCORepository _rep;

        public PAGNET_BANCOService(IPAGNET_BANCORepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public object[][] GetBancoAtivos()
        {
            return _rep.Find(x => x.ATIVO == "S").Select(x => new object[] { x.CODBANCO, x.NMBANCO }).ToArray();
        }

        public async Task<PAGNET_BANCO> getBancoByID(string codBanco)
        {
            return _rep.Get(x => x.CODBANCO == codBanco).FirstOrDefault();
        }
    }
}
