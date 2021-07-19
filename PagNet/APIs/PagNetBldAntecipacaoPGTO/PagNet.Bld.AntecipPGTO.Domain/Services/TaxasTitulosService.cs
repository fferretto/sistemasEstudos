using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Repository;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Services;
using PagNet.Bld.AntecipPGTO.Domain.Services.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Bld.AntecipPGTO.Domain.Services
{
    public class TaxasTitulosService : ServiceBase<PAGNET_TAXAS_TITULOS>, ITaxasTitulosService
    {
        private readonly IPagNet_Taxas_TitulosRepository _rep;


        public TaxasTitulosService(IPagNet_Taxas_TitulosRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public void AtualizaTaxa(PAGNET_TAXAS_TITULOS item)
        {
            _rep.Update(item);
        }

        public async Task<PAGNET_TAXAS_TITULOS> buscaTaxasbyID(int id)
        {
            var dados = _rep.Get(x => x.CODTAXATITULO == id).FirstOrDefault();

            return dados;
        }

        public async Task<List<PAGNET_TAXAS_TITULOS>> buscaTodasTaxasbyCodTitulo(int codTitulo)
        {
            var dados = _rep.Get(x => x.CODTITULO == codTitulo).ToList();

            return dados;
        }

        public void IncluiTaxa(PAGNET_TAXAS_TITULOS item)
        {
            int id = _rep.GetMaxKey();
            item.CODTAXATITULO = id;
            _rep.Add(item);
        }

        public void RemoveTaxa(int id)
        {
            var taxa = _rep.Get(x => x.CODTAXATITULO == id).FirstOrDefault();

            _rep.Remove(taxa);
        }
    }
}
