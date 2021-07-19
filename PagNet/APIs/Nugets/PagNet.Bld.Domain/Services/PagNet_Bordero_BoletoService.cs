using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_BORDERO_BOLETOService : ServiceBase<PAGNET_BORDERO_BOLETO>, IPAGNET_BORDERO_BOLETOService
    {
        private readonly IPAGNET_BORDERO_BOLETORepository _rep;
        private readonly IPAGNET_EMISSAOBOLETOService _emissaoBoleto;

        public PAGNET_BORDERO_BOLETOService(IPAGNET_BORDERO_BOLETORepository rep,
                                            IPAGNET_EMISSAOBOLETOService emissaoBoleto)
            : base(rep)
        {
            _rep = rep;
            _emissaoBoleto = emissaoBoleto;
        }

        public void AtualizaBordero(PAGNET_BORDERO_BOLETO b)
        {
            _rep.Update(b);
        }

        public void AtualizaStatusBordero(int codBordero, string NovoStatus)
        {
            var Bordero = LocalizaBordero(codBordero);
            Bordero.STATUS = NovoStatus;

            _rep.Update(Bordero);
        }

        public List<PAGNET_BORDERO_BOLETO> BuscaBordero(string status, int codEmpresa, int codBordero)
        {

            var dados = _rep.Get(x => x.STATUS == status &&
                                       x.CODEMPRESA == codEmpresa &&
                                       ((codBordero == 0) || x.CODBORDERO == codBordero)
                                    ).ToList();

            return dados;
        }

        public List<PAGNET_BORDERO_BOLETO> BuscaBorderoByCodArquivo(int codCarquivo)
        {
            var dadosBordero = _rep.Get(x => x.CODBORDERO == 1).ToList();

            return dadosBordero;
        }

        public PAGNET_BORDERO_BOLETO InseriBordero(PAGNET_BORDERO_BOLETO b)
        {
            int codBordero = _rep.BuscaProximoID();
            b.CODBORDERO = codBordero;

            _rep.Add(b);

            return b;
        }

        public PAGNET_BORDERO_BOLETO LocalizaBordero(int codBordero)
        {
            var dados = _rep.Get(x => x.CODBORDERO == codBordero).FirstOrDefault();

            return dados;
        }
    }
}
