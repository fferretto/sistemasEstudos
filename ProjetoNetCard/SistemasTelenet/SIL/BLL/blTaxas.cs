using System;
using System.Collections.Generic;
using TELENET.SIL.PO;
using TELENET.SIL.DA;

namespace TELENET.SIL.BL
{
    public class blTaxas
    {
        readonly OPERADORA FOperador;

        public blTaxas(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        public List<TAXAVA> ColecaoTaxas(string Filtro)
        {
            var TaxasDAL = new daTaxas(FOperador);
            return TaxasDAL.ColecaoTaxas(Filtro);
        }

        public List<TAXAVAPJ> ColecaoTaxasVaPj(string Filtro)
        {
            var TaxasDAL = new daTaxas(FOperador);
            return TaxasDAL.ColecaoTaxasVaPj(Filtro);
        }

        public TAXAVA GetTaxa(int idTaxa)
        {
            var TaxasDAL = new daTaxas(FOperador);
            return TaxasDAL.GetTaxa(idTaxa);
        }

        public List<STATUS> ColecaoStatus()
        {
            var TaxasDAL = new daTaxas(FOperador);
            return TaxasDAL.ColecaoStatus();
        }

        public List<TAXAS_CONSULTA> ColecaoTaxasConsulta(string Filtro)
        {
            var TaxasDAL = new daTaxas(FOperador);
            return TaxasDAL.ColecaoTaxasConsulta(Filtro);
        }

        public List<TAXAS_CONSULTA> ColecaoTaxasConsultaVaPj(string Filtro)
        {
            var TaxasDAL = new daTaxas(FOperador);
            return TaxasDAL.ColecaoTaxasConsultaVaPj(Filtro);
        }

        public void Incluir(TAXAVA taxava)
        {
            var TaxasDAL = new daTaxas(FOperador);
            if (TaxasDAL.VerificaAbrev(taxava.ABREVTAXA))
                throw new Exception(string.Format("Abreviatura {0} incorreta ou ja existente no sistema!", taxava.ABREVTAXA));
            TaxasDAL.Inserir(taxava);
        }

        public bool Incluir(TAXAVAPJ taxa, out string mensagem)
        {
            var TaxasDAL = new daTaxas(FOperador);
            if (TaxasDAL.VerificaAbrev(taxa.ABREVTAXA))
                throw new Exception(string.Format("Abreviatura {0} incorreta ou ja existente no sistema!", taxa.ABREVTAXA));
            return TaxasDAL.Inserir(taxa, out mensagem);
        }

        public void Alterar(TAXAVAPJ taxa)
        {
            var TaxasDAL = new daTaxas(FOperador);
            if (TaxasDAL.VerificaAbrev(taxa.ABREVTAXA))
                throw new Exception(string.Format("Abreviatura {0} incorreta ou ja existente no sistema!", taxa.ABREVTAXA));
            TaxasDAL.Alterar(taxa);
        }

        public bool Excluir(TAXAVAPJ taxa, out string mensagem)
        {
            var TaxasDAL = new daTaxas(FOperador);
           
            return TaxasDAL.Excluir(taxa, out mensagem);
        }

        public void Alterar(TAXAVA taxava, string AbrevAtual)
        {
            var TaxasDAL = new daTaxas(FOperador);
            if (taxava.ABREVTAXA != AbrevAtual)
            {
                if (TaxasDAL.VerificaAbrev(taxava.ABREVTAXA))
                    throw new Exception(string.Format("Login {0} ja existe!", taxava.ABREVTAXA));
            }
            TaxasDAL.Alterar(taxava);
        }

        public void Excluir(TAXAVA taxava)
        {
            var TaxasDAL = new daTaxas(FOperador);
            TaxasDAL.Excluir(taxava);
        }
    }
}
