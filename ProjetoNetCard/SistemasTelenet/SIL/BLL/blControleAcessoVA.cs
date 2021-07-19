using System;
using TELENET.SIL.PO;
using TELENET.SIL.DA;
using System.Collections.Generic;

namespace TELENET.SIL.BL
{
    public class blControleAcessoVA
    {
        readonly OPERADORA FOperador;

        public blControleAcessoVA(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        public bool HabilitaAgrupamento()
        {
            var ControleAcessoVADAL = new daControleAcessoVA(FOperador);
            return ControleAcessoVADAL.HabilitaAgrupamento();
        }

        public bool NovaCargaAtiva()
        {
            var ControleAcessoVADAL = new daControleAcessoVA(FOperador);
            return ControleAcessoVADAL.NovaCargaAtiva();
        }

        public int NumRegConsultaTransacao()
        {
            var ControleAcessoVADAL = new daControleAcessoVA(FOperador);
            return ControleAcessoVADAL.NumRegConsultaTransacao();
        }

        public int ConsultaAgrupamento(int IdPerfil)
        {
            var ControleAcessoVADAL = new daControleAcessoVA(FOperador);
            return ControleAcessoVADAL.ConsultaAgrupamento(IdPerfil);
        }

        public List<RESTRICAOACESSO> ColecaoControleAcessoVA(Int16 Perfil, string FlgForm)
        {
            var ControleAcessoVADAL = new daControleAcessoVA(FOperador);
            return ControleAcessoVADAL.ColecaoRestricoes(Perfil, FlgForm, String.Empty);
        }

        public List<RESTRICAOACESSO> ColecaoControleAcessoVA(Int16 Perfil)
        {
            var ControleAcessoVADAL = new daControleAcessoVA(FOperador);
            return ControleAcessoVADAL.ColecaoRestricoes(Perfil, String.Empty, String.Empty);
        }

        public List<RESTRICAOACESSO> ColecaoControleAcessoVA(string FlgForm, string Form)
        {
            var ControleAcessoVADAL = new daControleAcessoVA(FOperador);
            return ControleAcessoVADAL.ColecaoRestricoes(FOperador.IDPERFIL, FlgForm, Form);
        }

        public void Incluir(CONTROLEACESSOVA ControleAcessoVA)
        {
            var ControleAcessoVADAL = new daControleAcessoVA(FOperador);
            if (ControleAcessoVADAL.DuplicidadeCodigos(ControleAcessoVA.IDPERFIL, ControleAcessoVA.IDCOMP))
                throw new Exception("Restricao ja inserida para Perfil selecionado.");
            ControleAcessoVADAL.Inserir(ControleAcessoVA);
        }

        public void Incluir(List<CONTROLEACESSOVA> ControleAcessoVAList, List<object> listIDComp)
        {
            var ControleAcessoVADAL = new daControleAcessoVA(FOperador);
            if (ControleAcessoVADAL.DuplicidadeCodigos(ControleAcessoVAList[0].IDPERFIL, listIDComp))
                throw new Exception("Restricao ja inserida para Perfil selecionado.");
            ControleAcessoVADAL.Inserir(ControleAcessoVAList);
        }

        public void Excluir(CONTROLEACESSOVA ControleAcessoVA)
        {
            //Persistir
            var ControleAcessoVADAL = new daControleAcessoVA(FOperador);
            ControleAcessoVADAL.Excluir(ControleAcessoVA);
        }

        public void ExcluirPerfil(Int32 IdPerfil)
        {
            //Persistir
            var ControleAcessoVADAL = new daControleAcessoVA(FOperador);
            ControleAcessoVADAL.ExcluirPerfil(IdPerfil);
        }
    }
}
