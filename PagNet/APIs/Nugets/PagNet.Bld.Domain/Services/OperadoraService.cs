using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services.Common;
using PagNet.Bld.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Services
{
    public class OPERADORAService : ServiceBase<OPERADORA>, IOPERADORAService
    {
        private readonly IOPERADORARepository _rep;

        public OPERADORAService(IOPERADORARepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public IEnumerable<OPERADORA> GetAllOperadora()
        {
            return _rep.Get(x => x.FLAG_VA == "N").OrderBy(x => x.NOME);
        }

        public async Task<OPERADORA> GetOperadoraById(int id)
        {
            return _rep.Get(x => x.CODOPE == id).FirstOrDefault();
        }
        public object[][] GetHashOperadora()
        {
            return _rep.Get(x => x.FLAG_VA == "N")
                .Select(x => new object[] { x.CODOPE, x.NOME }).ToArray();
        }
    }
}
