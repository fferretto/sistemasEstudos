using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_TAXAS_TITULOSService : IServiceBase<PAGNET_TAXAS_TITULOS>
    {
        Task<PAGNET_TAXAS_TITULOS> buscaTaxasbyID(int id);
        Task<List<PAGNET_TAXAS_TITULOS>> buscaTodasTaxasbyCodTitulo(int codTitulo);

        void IncluiTaxa(PAGNET_TAXAS_TITULOS item);
        void AtualizaTaxa(PAGNET_TAXAS_TITULOS item);
        void RemoveTaxa(int id);
    }
}
