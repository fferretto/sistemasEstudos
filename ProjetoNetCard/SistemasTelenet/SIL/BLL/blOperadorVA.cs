using System;
using System.Collections.Generic;
using TELENET.SIL.PO;
using TELENET.SIL.DA;


namespace TELENET.SIL.BL
{
    public class blOPERADORVA
    {
        readonly OPERADORA FOperador;
        public blOPERADORVA(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        #region Selecao
        public OPERADORVA GetOPERADORVA(int ID)
        {
            try
            {
                var OPERADORDAL = new daOPERADORVA(FOperador);
                return OPERADORDAL.GetOperadorVA(ID);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("Erro Camada BLL [GetOperadorVA] : {0}", err.Message));
            }
        }

        public List<OPERADORVA> GetOPERADORVA()
        {
            try
            {
                var OperadorDAL = new daOPERADORVA(FOperador);
                return OperadorDAL.GetOperadorVA();
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("Erro Camada BLL [GerOparadorVA] : {0}", err.Message));
            }
        }

        public List<OPERADOR_VA> GetColecaoOperadorVA(string filtro)
        {
            try
            {
                var OperadorDAL = new daOPERADORVA(FOperador);
                return OperadorDAL.GetColecaoOperadorVA(filtro);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("Erro Camada BLL [GerOparadorVA] : {0}", err.Message));
            }
        }

        public int ProxCodigo()
        {
            var oper = new daOPERADORVA(FOperador);
            var ColecaoOperadorVA = oper.GetOperadorVA();
            var fProxCodigo = 0;
            if (ColecaoOperadorVA.Count > 0)
            {
                var OperadorVA = ColecaoOperadorVA[0];
                fProxCodigo = OperadorVA.CODOPE;
            }
            return fProxCodigo + 1;
        }
        #endregion

        #region Inclusao
        public bool Incluir(OPERADORVA OperadorVA)
        {
            if (OperadorVA.NOMOPE == string.Empty)
            {
                throw new Exception("Favor informar Nome do Operador");
            }

            //Persistir
            try
            {
                var OperadorDAL = new daOPERADORVA(FOperador);
                return OperadorDAL.Inserir(OperadorVA);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("Erro persistir INCLUSAO: {0}", err.Message));
            }
        }
        #endregion

        #region Alteracao

        public bool Alterar(OPERADORVA OperadorVA)
        {
            if (OperadorVA.NOMOPE == string.Empty)
            {
                throw new Exception("Favor informar Nome do Operador");
            }

            // Persistir
            try
            {
                var OperadorDAL = new daOPERADORVA(FOperador);
                return OperadorDAL.Alterar(OperadorVA);
            }
            catch (Exception err)
            {
                throw new Exception(string.Format("Erro Camada BLL [Alterar] : {0}", err.Message));
            }
        }

        #endregion

        #region Exclusao
        public bool Excluir(OPERADORVA OperadorVA)
        {
            //Persistir
            try
            {
                var OperadorDAL = new daOPERADORVA(FOperador);
                return OperadorDAL.Excluir(OperadorVA);

            }
            catch (Exception err)
            {
                throw new Exception(string.Format("Erro Camada BLL [Exclusao] : {0}", err.Message));
            }
        }
        #endregion

    }
}