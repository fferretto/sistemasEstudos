using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class SUBREDEService : ServiceBase<SUBREDE>, ISUBREDEService
    {
        private readonly ISUBREDERepository _rep;

        public SUBREDEService(ISUBREDERepository rep)
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
