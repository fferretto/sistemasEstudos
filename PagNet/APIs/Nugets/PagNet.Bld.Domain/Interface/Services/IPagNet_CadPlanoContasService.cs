using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_CADPLANOCONTASService : IServiceBase<PAGNET_CADPLANOCONTAS>
    {
        Task<List<PAGNET_CADPLANOCONTAS>> BuscaTodosPlanosContas(int codEmpresa);
        Task<PAGNET_CADPLANOCONTAS> BuscaTodosPlanosContasByID(int codPlanoContas);
        Task<List<PAGNET_CADPLANOCONTAS>> BuscaTodosPlanosContasFilhoByIDPai(int codPlanoContas);

        Task<PAGNET_CADPLANOCONTAS> BuscaDefaultPlanosContasPagamento(int codEmpresa);
        Task<PAGNET_CADPLANOCONTAS> BuscaDefaultPlanosContasRecebimento(int codEmpresa);
        Task<List<PAGNET_CADPLANOCONTAS>> BuscaPlanosContasPagamento(int codEmpresa);
        Task<List<PAGNET_CADPLANOCONTAS>> BuscaPlanosContasRecebimento(int codEmpresa);
        Task<List<PAGNET_CADPLANOCONTAS>> BuscaPlanosContasDefault();
        Task<List<PAGNET_CADPLANOCONTAS>> BuscaPlanosContasDefaultPagamento();
        Task<List<PAGNET_CADPLANOCONTAS>> BuscaPlanosContasDefaultRecebimento();

        void IncluiPlanoContas(PAGNET_CADPLANOCONTAS dados);
        void AtualizaPlanoContas(PAGNET_CADPLANOCONTAS dados);

    }
}
