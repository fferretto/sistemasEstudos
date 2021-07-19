using System.Linq;
using System.Threading.Tasks;
using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;

namespace PagNet.Domain.Services
{
    public class SubRedeService : ServiceBase<SUBREDE>, ISubRedeService
    {
        private readonly ISubRedeRepository _rep;

        public SubRedeService(ISubRedeRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public object[][] GetSubRede()
        {
            var dados = _rep.Get().Select(x => new object[] { x.CODSUBREDE, x.NOMSUBREDE }).ToArray();
            return dados;
        }
        public async Task<SUBREDE> GetSubRedeByID(int codSubRede)
        {
            return _rep.Get(x => x.CODSUBREDE == codSubRede).FirstOrDefault();
        }

        public object[][] BuscaSubRedeByID(int codSubRede)
        {
            return _rep.Get(x => x.CODSUBREDE == codSubRede).Select(x => new object[] { x.CODSUBREDE, x.NOMSUBREDE }).ToArray();
        }
    }
}
