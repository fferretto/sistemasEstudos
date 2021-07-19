using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using System.Linq;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class PagNet_OcorrenciaRetPagService : ServiceBase<PAGNET_OCORRENCIARETPAG>, IPagNet_OcorrenciaRetPagService
    {
        private readonly IPagNet_OcorrenciaRetPagRepository _rep;

        public PagNet_OcorrenciaRetPagService(IPagNet_OcorrenciaRetPagRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public string ReturnOcorrencia(string id)
        {
            var ocorrencia = _rep.Get(x => x.codOcorrenciaRetPag == id).FirstOrDefault();

            if (ocorrencia == null) return "";

            return ocorrencia.Descricao;
        }
    }
}
