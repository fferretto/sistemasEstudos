using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;

namespace PagNet.Domain.Services
{
    public class PagNet_Bordero_BoletoService : ServiceBase<PAGNET_BORDERO_BOLETO>, IPagNet_Bordero_BoletoService
    {
        private readonly IPagNet_Bordero_BoletoRepository _rep;
        private readonly IPagNet_EmissaoBoletoService _emissaoBoleto;

        public PagNet_Bordero_BoletoService(IPagNet_Bordero_BoletoRepository rep,
            IPagNet_EmissaoBoletoService emissaoBoleto)
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
            int codBordero = _rep.GetMaxKey();
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
