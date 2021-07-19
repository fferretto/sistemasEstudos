using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Services.Common;
using PagNet.Bld.Domain.Interface.Repository.Procedures;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_BORDERO_PAGAMENTOService : ServiceBase<PAGNET_BORDERO_PAGAMENTO>, IPAGNET_BORDERO_PAGAMENTOService
    {
        private readonly IPAGNET_BORDERO_PAGAMENTORepository _rep;
        private readonly IPROC_PAGNET_CONS_BORDERORepository _PBordero;

        public PAGNET_BORDERO_PAGAMENTOService(IPAGNET_BORDERO_PAGAMENTORepository rep,
                                               IPROC_PAGNET_CONS_BORDERORepository PBordero)
            : base(rep)
        {
            _rep = rep;
            _PBordero = PBordero;
        }

        public void AtualizaBordero(PAGNET_BORDERO_PAGAMENTO b)
        {
            _rep.Update(b);
        }

        public void AtualizaStatusBordero(int codBordero, string NovoStatus)
        {
            var Bordero = LocalizaBordero(codBordero);
            Bordero.STATUS = NovoStatus;

            _rep.Update(Bordero);
        }

        public List<PAGNET_BORDERO_PAGAMENTO> BuscaBordero(string status, string codBanco, int codEmpresa, string codFormaPGTO, int codBordero)
        {

            var dados = _rep.Get(x =>  x.STATUS == status &&
                                       ((codEmpresa == 0) || x.CODEMPRESA == codEmpresa) &&
                                       ((codBordero == 0) || x.CODBORDERO == codBordero)
                                    ).ToList();

            return dados;
        }

        public PAGNET_BORDERO_PAGAMENTO InseriBordero(PAGNET_BORDERO_PAGAMENTO b)
        {
            int codBordero = _rep.BuscaProximoID();
            b.CODBORDERO = codBordero;

            _rep.Add(b);

            return b;
        }

        public PAGNET_BORDERO_PAGAMENTO LocalizaBordero(int codBordero)
        {
            var dados = _rep.Get(x => x.CODBORDERO == codBordero).FirstOrDefault();

            return dados;
        }

        public async Task<List<PROC_PAGNET_CONS_BORDERO>> Proc_ConsultaBorderoPagamento(int codEmpresa, int codBordero, int codContaCorrente,  string Status)
        {
            var dados = await _PBordero.ConsultaBorderoPagamento(codEmpresa, codBordero, codContaCorrente, Status);

            return dados.ToList();
        }
    }
}
