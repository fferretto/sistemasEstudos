using System;
using System.Collections.Generic;
using TELENET.SIL.PO;
using TELENET.SIL.DA;

namespace TELENET.SIL.BL
{
    public class blComponentesAcessoVA
    {
        readonly OPERADORA FOperador;

        public blComponentesAcessoVA(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        public List<COMPONENTESACESSOVA> ColecaoFormularios(Int32 IdPerfil)
        {
            try
            {
                var FormulariosDAL = new daComponentesAcessoVA(FOperador);
                return FormulariosDAL.ColecaoFormularios(IdPerfil);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("Erro Camada BLL [ColecaoFormularios] : {0}", err.Message));
            }
        }

        public List<COMPONENTESACESSOVA> ColecaoComponentes(string Form, Int32 IdPerfil)
        {
            try
            {
                var ComponentesDAL = new daComponentesAcessoVA(FOperador);
                return ComponentesDAL.ColecaoComponentes(Form, IdPerfil);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("Erro Camada BLL [ColecaoComponentes] : {0}", err.Message));
            }
        }

    }
}
