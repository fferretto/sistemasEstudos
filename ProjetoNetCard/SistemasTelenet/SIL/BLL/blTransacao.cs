using System;
using System.Collections.Generic;
using TELENET.SIL.DA;
using TELENET.SIL.PO;
using SIL.DAL;
using System.Collections;

namespace SIL.BLL
{
    public class blTransacao
    {
        OPERADORA FOperador;
        public blTransacao(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        public long ObterNumeroTransacoes(CONSULTA_VA filtros)
        {
            if (filtros.FILTROVAZIO)
            {
                return 0;
            }

            return new daTransacao(FOperador).ObterNumeroTransacoes(filtros);
        }

        public List<CTTRANSVA> GerarConsultaTransacao(CONSULTA_VA filtros)
        {
            //Sem filtros nao ha consulta (muito grande e causa erros) Apesar de que dependendo dos filtros o tamanho fica grande
            //e causa erros.
            if (filtros.FILTROVAZIO)
            {
                return null;
            }

            return new daTransacao(FOperador).GeraConsultaTransacao(filtros);
        }

        public List<SUBREDE> ListaSubRede()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarSubrede();
        }

        public List<REDECAPTURA> ListaRedeCaptura()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarRedeCaptura();
        }

        public List<TIPTRANS> ListaTipoTrans()
        {
            return new daTransacao(FOperador).ListaTipoTrans();
        }

        public List<TRANSACAO_VA> ListaTotalTrans(Hashtable filtros)
        {
            if (filtros != null && filtros["Ordem"] != null)
                return new daTransacao(FOperador).ListaTotalTransSintetico(filtros);
            return new daTransacao(FOperador).ListaTotalTransAnalitico(filtros);
        }

        public string AlterarValTrans(int sistema, string dattra, string numHost, string nsuAut, decimal valor)
        {
            if (string.IsNullOrEmpty(dattra) || string.IsNullOrEmpty(numHost) || string.IsNullOrEmpty(nsuAut))
                return string.Empty;

            return new daTransacao(FOperador).AlterarValTrans(sistema, dattra, numHost, nsuAut, valor);
        }

        public string CancelarTrans(int sistema, string dattra, string numHost, string nsuAut, string justific)
        {
            if (string.IsNullOrEmpty(dattra) || string.IsNullOrEmpty(numHost) || string.IsNullOrEmpty(nsuAut) || string.IsNullOrEmpty(justific))
                return string.Empty;

            return new daTransacao(FOperador).CancelarTrans(sistema, dattra, numHost, nsuAut, justific);
        }

        public string AlteraTrans(int sistema, string dattra, string numHost, string nsuAut)
        {
            if (string.IsNullOrEmpty(dattra) || string.IsNullOrEmpty(numHost) || string.IsNullOrEmpty(nsuAut))
                return string.Empty;

            return new daTransacao(FOperador).AlteraTrans(sistema, dattra, numHost, nsuAut);
        }

        public string ConfirmaTrans(int sistema, string dattra, string numHost, string nsuAut, string justific)
        {
            if (string.IsNullOrEmpty(dattra) || string.IsNullOrEmpty(numHost) || string.IsNullOrEmpty(nsuAut) || string.IsNullOrEmpty(justific))
                return string.Empty;

            return new daTransacao(FOperador).ConfirmaTrans(sistema, dattra, numHost, nsuAut, justific);
        }
    }
}
