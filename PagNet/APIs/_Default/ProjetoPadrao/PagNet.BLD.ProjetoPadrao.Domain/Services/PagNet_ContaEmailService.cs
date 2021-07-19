using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class PagNet_ContaEmailService : ServiceBase<PAGNET_CONTAEMAIL>, IPagNet_ContaEmailService
    {
        private readonly IPagNet_ContaEmailRepository _rep;
        private readonly IPagNet_LogEmailEnviadoRepository _LogEmail;

        public PagNet_ContaEmailService(IPagNet_ContaEmailRepository rep,
                                        IPagNet_LogEmailEnviadoRepository LogEmail)
            : base(rep)
        {
            _rep = rep;
            _LogEmail = LogEmail;
        }

        public void AtualizaEmail(PAGNET_CONTAEMAIL conta)
        {
            _rep.Update(conta);

        }

        public bool ExisteContaPrincipalCadastrada()
        {
            var dados = _rep.Get(x => x.ATIVO == "S" && x.EMAILPRINCIPAL == "S").ToList();

            return (dados.Count > 0);
        }

        public async Task<List<PAGNET_CONTAEMAIL>> GetallEmailAtivos()
        {
            return _rep.Get(x => x.ATIVO == "S").ToList();
        }

        public async Task<PAGNET_CONTAEMAIL> GetEmailById(int id)
        {
            return _rep.Get(x => x.CODCONTAEMAIL == id).FirstOrDefault();
        }

        public object[][] GetHashEmail(int codSubRede)
        {
            return _rep.Get(x => x.ATIVO == "S")
                .Select(x => new object[] { x.CODCONTAEMAIL, x.NMCONTAEMAIL }).ToArray();
        }

        public async Task<PAGNET_CONTAEMAIL> IncluirEmail(PAGNET_CONTAEMAIL conta)
        {
            var cod = _rep.GetMaxKey();
            conta.CODCONTAEMAIL = cod;

            _rep.AddAsync(conta);

            return conta;
        }

        public void InseriLog(PAGNET_LOGEMAILENVIADO log)
        {
            var cod = _LogEmail.GetMaxKey();
            log.CODLOGEMAILENVIADO = cod;

            _LogEmail.Add(log);
        }
    }
}
