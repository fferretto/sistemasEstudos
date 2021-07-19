using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_CADPLANOCONTASService : ServiceBase<PAGNET_CADPLANOCONTAS>, IPAGNET_CADPLANOCONTASService
    {
        private readonly IPAGNET_CADPLANOCONTASRepository _rep;

        public PAGNET_CADPLANOCONTASService(IPAGNET_CADPLANOCONTASRepository rep)
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
            var planocontas = _rep.Get(x => x.CODEMPRESA == codEmpresa && x.ATIVO == "S" && x.DEFAULTPAGAMENTO == "S").FirstOrDefault();

            if (planocontas == null)
            {
                planocontas = _rep.Get(x => x.ATIVO == "S" && x.DEFAULTPAGAMENTO == "S" && x.PLANOCONTASDEFAULT == "S").FirstOrDefault();
            }
            return planocontas;
        }

        public async Task<PAGNET_CADPLANOCONTAS> BuscaDefaultPlanosContasRecebimento(int codEmpresa)
        {
            var planocontas = _rep.Get(x => x.CODEMPRESA == codEmpresa && x.ATIVO == "S" && x.DEFAULTRECEBIMENTO == "S").FirstOrDefault();

            if (planocontas == null)
            {
                planocontas = _rep.Get(x => x.ATIVO == "S" && x.DEFAULTRECEBIMENTO == "S" && x.PLANOCONTASDEFAULT == "S").FirstOrDefault();
            }
            return planocontas;
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
            dados.CODPLANOCONTAS = _rep.BuscaProximoID();
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
