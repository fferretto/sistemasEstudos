using System;
using System.Collections.Generic;
using TELENET.SIL.PO;
using TELENET.SIL.DA;

namespace TELENET.SIL.BL
{
    public class blComponentesAcesso
    {
        readonly OPERADORA FOperador;

        public blComponentesAcesso(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        public List<COMPONENTESACESSO> ColecaoFormularios(Int32 IdPerfil)
        {
            try
            {
                var FormulariosDAL = new daComponentesAcesso(FOperador);
                return FormulariosDAL.ColecaoFormularios(IdPerfil);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("Erro Camada BLL [ColecaoFormularios] : {0}", err.Message));
            }
        }

        public List<COMPONENTESACESSO> ColecaoComponentes(string Form, Int32 IdPerfil)
        {
            try
            {
                var ComponentesDAL = new daComponentesAcesso(FOperador);
                return ComponentesDAL.ColecaoComponentes(Form, IdPerfil);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("Erro Camada BLL [ColecaoComponentes] : {0}", err.Message));
            }
        }

    }
}
