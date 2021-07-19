using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class PagNet_Taxas_TitulosService : ServiceBase<PAGNET_TAXAS_TITULOS>, IPagNet_Taxas_TitulosService
    {
        private readonly IPagNet_Taxas_TitulosRepository _rep;


        public PagNet_Taxas_TitulosService(IPagNet_Taxas_TitulosRepository rep)
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
            var dados = _rep.Get(x => x.CODTITULO == codTitulo, "USUARIO_NETCARD").ToList();

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
