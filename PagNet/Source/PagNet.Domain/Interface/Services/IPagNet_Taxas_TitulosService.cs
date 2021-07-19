using PagNet.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_Taxas_TitulosService : IServiceBase<PAGNET_TAXAS_TITULOS>
    {
        Task<PAGNET_TAXAS_TITULOS> buscaTaxasbyID(int id);
        Task<List<PAGNET_TAXAS_TITULOS>> buscaTodasTaxasbyCodTitulo(int codTitulo);

        void IncluiTaxa(PAGNET_TAXAS_TITULOS item);
        void AtualizaTaxa(PAGNET_TAXAS_TITULOS item);
        void RemoveTaxa(int id);
    }
}
