using System;
using TELENET.SIL.PO;
using TELENET.SIL.DA;
using System.Collections.Generic;

namespace TELENET.SIL.BL
{
    public class blControleAcesso
    {
        readonly OPERADORA FOperador;

        public blControleAcesso(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        public List<PERMISSAOACESSO> ColecaoControleAcesso(Int16 Perfil, string FlgForm)
        {
            var ControleAcessoDAL = new daControleAcesso(FOperador);
            return ControleAcessoDAL.ColecaoPermissoes(Perfil, FlgForm, String.Empty);
        }

        public List<PERMISSAOACESSO> ColecaoPermissoes(Int16 Perfil)
        {
            var ControleAcessoDAL = new daControleAcesso(FOperador);
            return ControleAcessoDAL.ColecaoPermissoes2(Perfil, String.Empty, String.Empty);
        }

        public List<PERMISSAOACESSO> ColecaoControleAcesso(string FlgForm, string Form)
        {
            var ControleAcessoDAL = new daControleAcesso(FOperador);
            return ControleAcessoDAL.ColecaoPermissoes(FOperador.IDPERFIL, FlgForm, Form);
        }

        public void Incluir(CONTROLEACESSO ControleAcesso)
        {
            var ControleAcessoDAL = new daControleAcesso(FOperador);
            if (ControleAcessoDAL.DuplicidadeCodigos(ControleAcesso.IDPERFIL, ControleAcesso.IDCOMP))
                throw new Exception("Permissão ja inserida para Perfil selecionado.");
            ControleAcessoDAL.Inserir(ControleAcesso);
        }

        public void Incluir(int idPerfil, List<object> listAcao)
        {            
            var ControleAcessoDAL = new daControleAcesso(FOperador);            
            foreach (var item in listAcao)
            {
                if (ControleAcessoDAL.DuplicidadeCodigos(idPerfil, item as string))
                    throw new Exception("Permissão ja inserida para Perfil selecionado.");

                if (!ControleAcessoDAL.FormPaiLiberado(idPerfil, item as string))
                    ControleAcessoDAL.InserirFormPai(idPerfil, item as string);
                ControleAcessoDAL.Inserir(idPerfil, item as string);
            }
        }

        public void Excluir(int idPerfil, string acao)
        {
            //Persistir
            var ControleAcessoDAL = new daControleAcesso(FOperador);
            ControleAcessoDAL.Excluir(idPerfil, acao);
        }
    }
}
