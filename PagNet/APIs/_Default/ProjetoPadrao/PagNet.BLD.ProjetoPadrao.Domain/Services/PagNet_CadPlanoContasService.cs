using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class PagNet_CadPlanoContasService : ServiceBase<PAGNET_CADPLANOCONTAS>, IPagNet_CadPlanoContasService
    {
        private readonly IPagNet_CadPlanoContasRepository _rep;

        public PagNet_CadPlanoContasService(IPagNet_CadPlanoContasRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public void AtualizaPlanoContas(PAGNET_CADPLANOCONTAS dados)
        {
            _rep.Update(dados);
        }

        public async Task<List<PAGNET_CADPLANOCONTAS>> BuscaPlanosContasDefault()
        {
            var lista = _rep.Get(x => x.PLANOCONTASDEFAULT == "S" && x.ATIVO == "S");
            return lista.ToList();
        }

        public async Task<PAGNET_CADPLANOCONTAS> BuscaDefaultPlanosContasPagamento(int codEmpresa)
        {
            var planocontas = _rep.Get(x => x.CODEMPRESA == codEmpresa && x.ATIVO == "S" && x.DEFAULTPAGAMENTO == "S");
            return planocontas.FirstOrDefault();
        }

        public async Task<PAGNET_CADPLANOCONTAS> BuscaDefaultPlanosContasRecebimento(int codEmpresa)
        {
            var planocontas = _rep.Get(x => x.CODEMPRESA == codEmpresa && x.ATIVO == "S" && x.DEFAULTRECEBIMENTO == "S");
            return planocontas.FirstOrDefault();
        }
        public async Task<List<PAGNET_CADPLANOCONTAS>> BuscaPlanosContasPagamento(int codEmpresa)
        {
            var planocontas = _rep.Get(x => x.CODEMPRESA == codEmpresa && x.ATIVO == "S" && x.DESPESA == "S");
            return planocontas.ToList();
        }

        public async Task<List<PAGNET_CADPLANOCONTAS>> BuscaPlanosContasRecebimento(int codEmpresa)
        {
            var planocontas = _rep.Get(x => x.CODEMPRESA == codEmpresa && x.ATIVO == "S" && x.DESPESA == "N");
            return planocontas.ToList();
        }

        public async Task<List<PAGNET_CADPLANOCONTAS>> BuscaTodosPlanosContas(int codEmpresa)
        {
            var lista = _rep.Get(x => x.CODEMPRESA == codEmpresa && x.ATIVO == "S");
            return lista.ToList();
        }

        public async Task<PAGNET_CADPLANOCONTAS> BuscaTodosPlanosContasByID(int codPlanoContas)
        {
            var lista = _rep.Get(x => x.CODPLANOCONTAS == codPlanoContas);
            return lista.FirstOrDefault();
        }

        public async Task<List<PAGNET_CADPLANOCONTAS>> BuscaTodosPlanosContasFilhoByIDPai(int codPlanoContas)
        {
            var lista = _rep.Get(x => x.CODPLANOCONTAS_PAI == codPlanoContas && x.ATIVO == "S");
            return lista.ToList();
        }

        public void IncluiPlanoContas(PAGNET_CADPLANOCONTAS dados)
        {
            dados.CODPLANOCONTAS = _rep.GetMaxKey();
            _rep.Add(dados);
        }

        public async Task<List<PAGNET_CADPLANOCONTAS>> BuscaPlanosContasDefaultPagamento()
        {
            var lista = _rep.Get(x => x.PLANOCONTASDEFAULT == "S" && x.ATIVO == "S" && x.DESPESA == "S");
            return lista.ToList();
        }

        public async Task<List<PAGNET_CADPLANOCONTAS>> BuscaPlanosContasDefaultRecebimento()
        {
            var lista = _rep.Get(x => x.PLANOCONTASDEFAULT == "S" && x.ATIVO == "S" && x.DESPESA == "N");
            return lista.ToList();
        }
    }
}
