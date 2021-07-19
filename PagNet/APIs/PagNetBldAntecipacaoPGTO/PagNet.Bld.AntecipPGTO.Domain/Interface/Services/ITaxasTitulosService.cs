using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace PagNet.Bld.AntecipPGTO.Domain.Interface.Services
{
    public interface ITaxasTitulosService : IServiceBase<PAGNET_TAXAS_TITULOS>
    {
        Task<PAGNET_TAXAS_TITULOS> buscaTaxasbyID(int id);
        Task<List<PAGNET_TAXAS_TITULOS>> buscaTodasTaxasbyCodTitulo(int codTitulo);

        void IncluiTaxa(PAGNET_TAXAS_TITULOS item);
        void AtualizaTaxa(PAGNET_TAXAS_TITULOS item);
        void RemoveTaxa(int id);
    }
}
