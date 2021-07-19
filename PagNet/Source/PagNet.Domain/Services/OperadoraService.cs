using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Domain.Services
{
    public class OperadoraService : ServiceBase<OPERADORA>, IOperadoraService
    {
        private readonly IOperadoraRepository _rep;

        public OperadoraService(IOperadoraRepository rep)
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
