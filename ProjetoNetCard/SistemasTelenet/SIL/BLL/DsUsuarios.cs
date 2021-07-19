using System;
using System.Collections.Generic;
using System.Linq;
using TELENET.SIL.PO;

namespace TELENET.SIL.BL
{
    public class DsUsuarios
    {
        public DsUsuarios(OPERADORA operadora)
        {
            _operadora = operadora;
        }

        //private blUsuarioVANovo _blUsuarioVANovo;
        public List<VUSUARIODEP> _dependentes { get; set; }
        private OPERADORA _operadora;
        private int _idUsuario;
        private int _codCli;

        private List<VUSUARIODEP> InternalGetUsuariosDep(int id_Usuario, int codCli, string cpf, bool removeExcluidos)
        {
            if (_idUsuario != id_Usuario || _codCli != codCli)
            {
                _dependentes = new blUsuarioVANovo(_operadora).GetUsuariosDep(codCli, cpf);
                _idUsuario = id_Usuario;
                _codCli = codCli;
            }
            return removeExcluidos ? _dependentes.Where(d => !d.EXCLUIDO).ToList() : _dependentes;
        }


        public List<VUSUARIODEP> GetUsuariosDep(int id_Usuario, int codCli, string cpf)
        {
            return InternalGetUsuariosDep(id_Usuario, codCli, cpf, true);
        }

        public List<VUSUARIODEP> GetTodosUsuariosDep(int id_Usuario, int codCli, string cpf)
        {
            return InternalGetUsuariosDep(id_Usuario, codCli, cpf, false);
        }

        public List<VUSUARIODEP> Dependentes { get { return _dependentes.Where(d => !d.EXCLUIDO).ToList(); } }

        public bool InserirDependente(int sistema, int id_Usuario, int codCli, string cpf, string nomUsu, string sexo, string datNas, short codpar, DateTime datInc, int numDep, string limDep, string desta)
        {
            var par = new blTabelas(_operadora).ColecaoParentesco();
            var dep = new VUSUARIODEP
            {
                SISTEMA = sistema,
                ID_USUARIO = id_Usuario,
                CODCLI = codCli,
                CPF = cpf,
                NOMUSU = nomUsu,
                SEXO = sexo,
                DATNAS = datNas,
                CODPAR = Convert.ToInt32(codpar),
                DESPAR = par.Where(x => x.CODPAR == codpar).SingleOrDefault().DESPAR,
                NUMDEP = Convert.ToInt16(numDep),
                DATINC = datInc,
                LIMDEP = limDep
            };

            var repetido = _dependentes.Where(x => x.NOMUSU.Trim() == nomUsu.Trim()).SingleOrDefault();
            if (repetido != null)
                return false;

            _dependentes.Add(dep);



            return true;
        }

        public bool AlterarDependente(int sistema, int id_Usuario, int codCli, string cpf, string nomUsu, string sexo, string datNas, short codpar, DateTime datInc, int numDep, string limDep, string COMPOSITEKEY)
        {
            var par = new blTabelas(_operadora).ColecaoParentesco();
            var dep = _dependentes.Where(x => x.CODCLI == codCli && x.CPF == cpf && x.NUMDEP == numDep).SingleOrDefault();

            dep.SISTEMA = sistema;
            dep.ID_USUARIO = id_Usuario;
            dep.CODCLI = codCli;
            dep.CPF = cpf;
            dep.NOMUSU = nomUsu;
            dep.SEXO = sexo;
            dep.DATNAS = datNas;
            dep.CODPAR = Convert.ToInt32(codpar);
            dep.DESPAR = par.Where(x => x.CODPAR == codpar).SingleOrDefault().DESPAR;
            dep.NUMDEP = Convert.ToInt16(numDep);
            dep.DATINC = datInc;
            dep.LIMDEP = limDep;

            return true;
        }

        public bool ExcluirDependente(VUSUARIODEP dependente)
        {
            return _dependentes.Remove(dependente);
        }

        public bool ExcluirDependente(int sistema, int id_Usuario, int codCli, string cpf, DateTime datInc, int numDep)
        {
            var par = new blTabelas(_operadora).ColecaoParentesco();
            var dep = new VUSUARIODEP
            {
                SISTEMA = sistema,
                ID_USUARIO = id_Usuario,
                CODCLI = codCli,
                CPF = cpf,
                NUMDEP = Convert.ToInt16(numDep),
                DATINC = datInc
            };
            var odlDep = _dependentes.Where(x => x.CODCLI == codCli && x.CPF == cpf && x.NUMDEP == numDep).SingleOrDefault();
            if (odlDep != null)
            {
                odlDep.EXCLUIDO = true;
            }
            return true;
        }
    }
}