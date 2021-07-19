using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class PagNet_CadEmpresaService : ServiceBase<PAGNET_CADEMPRESA>, IPagNet_CadEmpresaService
    {
        private readonly IPagNet_CadEmpresaRepository _rep;

        public PagNet_CadEmpresaService(IPagNet_CadEmpresaRepository rep)
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
            var id = _rep.GetMaxKey();
            emp.CODEMPRESA = id;

            _rep.Add(emp);
        }
    }
}
