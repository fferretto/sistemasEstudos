using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_CADEMPRESAService : ServiceBase<PAGNET_CADEMPRESA>, IPAGNET_CADEMPRESAService
    {
        private readonly IPAGNET_CADEMPRESARepository _rep;

        public PAGNET_CADEMPRESAService(IPAGNET_CADEMPRESARepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public void AtualizaEmpresa(PAGNET_CADEMPRESA emp)
        {
            _rep.Update(emp);
        }

        public async Task<PAGNET_CADEMPRESA> ConsultaEmpresaByCNPJ(string CNPJ)
        {
            return _rep.Get(x => x.CNPJ == CNPJ).FirstOrDefault();
        }

        public async Task<PAGNET_CADEMPRESA> ConsultaEmpresaById(int CodEmpresa)
        {
            return _rep.Get(x => x.CODEMPRESA == CodEmpresa).FirstOrDefault();
        }

        public async Task<PAGNET_CADEMPRESA> ConsultaEmpresaBySubRede(int codSubRede)
        {
            return _rep.Get(x => x.CODSUBREDE == codSubRede).FirstOrDefault();
        }

        public async Task<List<PAGNET_CADEMPRESA>> GetAllempresas()
        {
            return _rep.GetAll().ToList();
        }

        public object[][] GetEmpresa()
        {
            return _rep.Get()
                .Select(x => new object[] { x.CODEMPRESA, x.NMFANTASIA }).ToArray();
        }

        public void InserirEmpresa(PAGNET_CADEMPRESA emp)
        {
            var id = _rep.BuscaProximoID();
            emp.CODEMPRESA = id;

            _rep.Add(emp);
        }
    }
}
