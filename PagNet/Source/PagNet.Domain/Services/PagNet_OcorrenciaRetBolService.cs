using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System.Linq;

namespace PagNet.Domain.Services
{
    public class PagNet_OcorrenciaRetBolService : ServiceBase<PAGNET_OCORRENCIARETBOL>, IPagNet_OcorrenciaRetBolService
    {
        private readonly IPagNet_OcorrenciaRetBolRepository _rep;

        public PagNet_OcorrenciaRetBolService(IPagNet_OcorrenciaRetBolRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public string ReturnOcorrencia(string id)
        {
            var ocorrencia = _rep.Get(x => x.codOcorrenciaRetBol == id).FirstOrDefault();

            return ocorrencia.Descricao;
        }
    }
}

