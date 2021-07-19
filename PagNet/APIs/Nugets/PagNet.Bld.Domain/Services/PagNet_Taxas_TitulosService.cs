using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_TAXAS_TITULOSService : ServiceBase<PAGNET_TAXAS_TITULOS>, IPAGNET_TAXAS_TITULOSService
    {
        private readonly IPAGNET_TAXAS_TITULOSRepository _rep;


        public PAGNET_TAXAS_TITULOSService(IPAGNET_TAXAS_TITULOSRepository rep)
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
            var dados = _rep.Get(x => x.CODTITULO == codTitulo, "PAGNET_USUARIO").ToList();

            return dados;
        }

        public void IncluiTaxa(PAGNET_TAXAS_TITULOS item)
        {
            int id = _rep.BuscaProximoID();
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
