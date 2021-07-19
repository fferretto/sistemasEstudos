using System.Collections.Generic;
using TELENET.SIL.PO;
using TELENET.SIL.DA;

namespace TELENET.SIL.BL
{
    public class blPerfilAcessoVA
    {
        readonly OPERADORA FOperador;

        public blPerfilAcessoVA(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        public List<PERFILACESSOVA> ColecaoPerfilAcessoVA(string Filtro)
        {
            var PerfilAcessoVADAL = new daPerfilAcessoVA(FOperador);
            return PerfilAcessoVADAL.ColecaoPerfilAcessoVA(Filtro);
        }

        public List<PERFILACESSOVA> ColecaoPerfilAcessoVA()
        {
            var PerfilAcessoVADAL = new daPerfilAcessoVA(FOperador);
            return PerfilAcessoVADAL.ColecaoPerfilAcessoVA();
        }

        public PERFILACESSOVA GetPerfilAcessoVA(int id)
        {
            var PerfilAcessoVADAL = new daPerfilAcessoVA(FOperador);
            return PerfilAcessoVADAL.GetPerfilAcessoVA(id);
        }

        public void Incluir(PERFILACESSOVA PerfilAcessoVA)
        {
            //Persistir
            var PerfilAcessoDAL = new daPerfilAcessoVA(FOperador);
            PerfilAcessoDAL.Inserir(PerfilAcessoVA);
        }

        public void Alterar(PERFILACESSOVA PerfilAcessoVA)
        {
            //Persistir
            var PerfilAcessoDAL = new daPerfilAcessoVA(FOperador);
            PerfilAcessoDAL.Alterar(PerfilAcessoVA);
        }

        public bool ValidaNomePerfil(PERFILACESSOVA PerfilAcessoVA)
        {
            //Persistir
            var PerfilAcessoDAL = new daPerfilAcessoVA(FOperador);
            return PerfilAcessoDAL.ValidaNomePerfil(PerfilAcessoVA);
        }

        public void Excluir(PERFILACESSOVA PerfilAcessoVA)
        {
            //Persistir
            var PerfilAcessoDAL = new daPerfilAcessoVA(FOperador);
            PerfilAcessoDAL.Excluir(PerfilAcessoVA);
        }

        public bool OperadorTelenet(int idPerfil)
        {
            return new daPerfilAcessoVA(FOperador).OperadorTelenet(idPerfil);
        }
    }
}
