using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Domain.Services
{
    public class PagNet_BancoService : ServiceBase<PAGNET_BANCO>, IPagNet_BancoService
    {
        private readonly IPagNet_BancoRepository _rep;

        public PagNet_BancoService(IPagNet_BancoRepository rep)
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
