using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_CONTAEMAILService : ServiceBase<PAGNET_CONTAEMAIL>, IPAGNET_CONTAEMAILService
    {
        private readonly IPAGNET_CONTAEMAILRepository _rep;
        private readonly IPAGNET_LOGEMAILENVIADORepository _LogEmail;

        public PAGNET_CONTAEMAILService(IPAGNET_CONTAEMAILRepository rep,
                                        IPAGNET_LOGEMAILENVIADORepository LogEmail)
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
            var cod = _rep.BuscaProximoID();
            conta.CODCONTAEMAIL = cod;

            _rep.AddAsync(conta);

            return conta;
        }

        public void InseriLog(PAGNET_LOGEMAILENVIADO log)
        {
            var cod = _LogEmail.BuscaProximoID();
            log.CODLOGEMAILENVIADO = cod;

            _LogEmail.Add(log);
        }
    }
}
