using System;
using System.Collections.Generic;
using TELENET.SIL.PO;
using TELENET.SIL.DA;

namespace TELENET.SIL.BL
{
    public class blBeneficios
    {
        readonly OPERADORA FOperador;

        public blBeneficios(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        public int BuscarProximoCodigo()
        {
            return new daBeneficios(FOperador).BuscarProximoCodigo();
        }

        public List<BENEFICIO> ColecaoBeneficios(string Filtro)
        {
            var da = new daBeneficios(FOperador);
            return da.ColecaoBeneficios(Filtro);
        }

        public BENEFICIO GetBeneficio(int idBeneficios)
        {
            var da = new daBeneficios(FOperador);
            return da.GetBenefico(idBeneficios);
        }

        public List<STATUS> ColecaoStatus()
        {
            var da = new daBeneficios(FOperador);
            return da.ColecaoStatus();
        }

        public List<BENEFICIOS_CONSULTA> ColecaoBeneficiosConsulta(string Filtro)
        {
            var da = new daBeneficios(FOperador);
            return da.ColecaoBeneficiosConsulta(Filtro);
        }        

        public void Incluir(BENEFICIO beneficio)
        {
            var da = new daBeneficios(FOperador);
            if (da.VerificaAbrev(beneficio.ABREVBENEF))
                throw new Exception(string.Format("Abreviatura {0} incorreta ou ja existente no sistema!", beneficio.ABREVBENEF));
            da.Inserir(beneficio);
        }

        public void Alterar(BENEFICIO beneficio, string AbrevAtual)
        {
            var da = new daBeneficios(FOperador);
            if (beneficio.ABREVBENEF != AbrevAtual)
            {
                if (da.VerificaAbrev(beneficio.ABREVBENEF))
                    throw new Exception(string.Format("Login {0} ja existe!", beneficio.ABREVBENEF));
            }
            da.Alterar(beneficio);
        }

        public void Excluir(BENEFICIO beneficio)
        {
            var da = new daBeneficios(FOperador);
            da.Excluir(beneficio);
        }

        public bool AssociaBeneficioCliente(BENEF_CLIENTE beneficio, int idOperador, out string msgRetorno)
        {
            return new daBeneficios(FOperador).AssociaBeneficioCliente(beneficio, idOperador, out msgRetorno);
        }

        public bool DesassociaBeneCli(BENEF_CLIENTE beneficio, int idOperador)
        {
            try
            {
                var daBeneficios = new daBeneficios(FOperador);
                return daBeneficios.DesassociaBeneCli(beneficio, idOperador);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        
        public List<BENEF_CLIENTE> BuscarBeneficiosClientes(int codcli)
        {
            return new daBeneficios(FOperador).BuscarBeneficiosClientes(codcli);
        }
    }
}
