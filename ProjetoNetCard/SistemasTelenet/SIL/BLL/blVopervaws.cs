using System;
using System.Collections.Generic;
using TELENET.SIL.PO;
using TELENET.SIL.DA;

namespace TELENET.SIL.BL
{
    public class blVopervaws
    {
        readonly OPERADORA FOperador;

        public blVopervaws(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        public List<VOPERVAWS> ColecaoVopervaws(string Filtro)
        {
            var VopervawsDAL = new daVopervaws(FOperador);
            return VopervawsDAL.ColecaoVopervaws(Filtro);
        }

        public VOPERVAWS GetVopervaws(int idFunc)
        {
            var VopervawsDAL = new daVopervaws(FOperador);
            return VopervawsDAL.GetVopervaws(idFunc);
        }

        public List<VOPERVAWS_CONSULTA> ColecaoVopervawsConsulta(string Filtro)
        {
            var VopervawsDAL = new daVopervaws(FOperador);
            return VopervawsDAL.ColecaoVopervawsConsulta(Filtro);
        }

        public void Incluir(VOPERVAWS Vopervaws)
        {
            //Persistir
            var VopervawsDAL = new daVopervaws(FOperador);
            if (VopervawsDAL.VerificaLogin(Vopervaws.LOGIN))
                throw new Exception(string.Format("Login incorreto ou ja existente no sistema!"));
            VopervawsDAL.Inserir(Vopervaws);
        }

        public void Alterar(VOPERVAWS Vopervaws, string loginAtual)
        {
            //Persistir
            var VopervawsDAL = new daVopervaws(FOperador);
            if (Vopervaws.LOGIN != loginAtual)
            {
                if (VopervawsDAL.VerificaLogin(Vopervaws.LOGIN))
                    throw new Exception(string.Format("Login {0} ja existe!", Vopervaws.LOGIN));
            }
            VopervawsDAL.Alterar(Vopervaws);
        }

        public void Excluir(VOPERVAWS Vopervaws)
        {
            //Persistir
            var VopervawsDAL = new daVopervaws(FOperador);
            VopervawsDAL.Excluir(Vopervaws);
        }

        public string AlteraSenha(VOPERVAWS operador)
        {
            var VopervawsDAL = new daVopervaws(FOperador);
            return VopervawsDAL.AlterarSenha(operador);
        }

        public string AlteraSenha(string NovaSenha)
        {
            var VopervawsDAL = new daVopervaws(FOperador);
            return VopervawsDAL.AlterarSenha(NovaSenha);
        }

        public Boolean VerificaPerfil(Int32 IdPerfil)
        {
            //Persistir
            var VopervawsDAL = new daVopervaws(FOperador);
            return VopervawsDAL.VerificaPerfil(IdPerfil);
        }
    }
}
