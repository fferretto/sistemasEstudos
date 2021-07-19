using System;
using System.IO;
using TELENET.SIL.PO;
using TELENET.SIL.DA;
using System.Collections;
using System.Collections.Generic;
using SIL.DAL;
using SIL.BLL;

namespace TELENET.SIL.BL
{
    public class blUsuarioVA
    {
        readonly OPERADORA FOperador;

        public blUsuarioVA(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        #region Listas

        public List<USUARIO_VA> UsuariosVA(short Classificacao, int CodCliInicial, int CodCliFinal,
            DateTime DataInicial, DateTime DataFinal, string ParametroInicial, string ParametroFinal)
        {
            daUsuarioVA UsuarioVADAL = new daUsuarioVA(FOperador);
            return UsuarioVADAL.UsuarioVA(Classificacao, CodCliInicial, CodCliFinal,
               DataInicial, DataFinal, ParametroInicial, ParametroFinal);
        }

        public List<USUARIO_VA> ColecaoUsuarioVA(string Filtro)
        {
            daUsuarioVA UsuarioVADAL = new daUsuarioVA(FOperador);
            return UsuarioVADAL.ColecaoUsuarioVA(Filtro);
        }

        public List<USUARIO_VA> ColecaoUsuarioVA(IFilter filter)
        {
            string _filtro = string.Empty;
            if (filter != null)
                _filtro = filter.FilterString;

            daUsuarioVA UsuarioVA = new daUsuarioVA(FOperador);
            return UsuarioVA.ColecaoUsuarioVAFilter(_filtro);
        }

        public List<USUARIO_VA> ColecaoUsuarioVA(int codIni, int codFim)
        {
            daUsuarioVA UsuarioVADAL = new daUsuarioVA(FOperador);
            return UsuarioVADAL.ColecaoUsuarioVA(codIni, codFim);

        }

        //public string MontaTrilha(string dtValCart, int Classe, int NumVia)
        //{
        //    var daUsuarioVa = new daUsuarioVA(FOperador);
        //    var dados = daUsuarioVa.RetornaParamVA();
        //    return daUsuarioVa.MontaTrilha(dtValCart, Classe, NumVia);
        //}

        public bool VerificaSolicitacao2ViaNoDia(string codCrt, int codCli)
        {
            return new daUsuarioVA(FOperador).VerificaSolicitacao2ViaNoDia(codCrt, codCli);

        }

        public string MontaTrilha(int sistema, int codCli, string cpf, int numDep)
        {
            return new daUsuarioVA(FOperador).MontaTrilha(sistema, codCli, cpf, numDep);
        }

        public List<USUARIO_VA> ListaCartoesGeral(Hashtable filtros)
        {
            OPERADORA operador = (OPERADORA)filtros["Operador"];
            return new daUsuarioVA(operador).ListaCartoesGeral(filtros);
        }

        public List<CLIENTEVA_PREPAGO> ListaClientes()
        {
            daUsuarioVA DAL = new daUsuarioVA(FOperador);
            return DAL.ListaClientes();
        }     

        #endregion

        #region Inclusao     

        public void IncluirUsuario(USUARIO_VA usu, bool cpfCancelado)
        {
            ValidaCliente(usu.CODCLI);
            usu.NUMDEP = "0";            
            var daUsuario = new daUsuarioVA(FOperador);            
            if (cpfCancelado) 
                daUsuario.Excluir(usu) ;
            var retorno = daUsuario.InserirUsuario(usu);
            var juncaoAtiva = new daCLIENTEVA(FOperador).JuncaoAtiva();
            if (retorno && juncaoAtiva)
                new daUsuarioVANovo(FOperador).AtualizaUsuarioPreJuncao(usu);
        }

        public string AbreviaNome(string nome)
        {
            return new daUsuarioVA(FOperador).GetNomeAbreviado(nome);
        }

        public ENDCEP GetCep(string cep)
        {
            return new daUsuarioVA(FOperador).GetCep(cep);
        }

        private void ValidaCliente(string codcli)
        {
            int aux = 0;
            Int32.TryParse(codcli, out aux);
            daUsuarioVA daUsuario = new daUsuarioVA(FOperador);
            if (!daUsuario.ValidarCliente(aux))
                throw new Exception("Nao e possivel cadastrar novos cartoes para Clientes com status diferente de Ativo ou Bloqueado!");
        }

        public USUARIO_VA GetDadosCompl(string cpf)
        {
            daUsuarioVA daUsuarioVA = new daUsuarioVA(FOperador);
            return daUsuarioVA.GetDadosCompl(cpf); 
        }

        #endregion

        #region Alteracao      

        public void AtualizarUsuario(USUARIO_VA usu, bool flagSTA)
        {
            usu.NOMCRT = AbreviaNome(usu.NOMUSU);
            var daUsuario = new daUsuarioVA(FOperador);
            daUsuario.Atualizar(usu, flagSTA);
            var juncaoAtiva = new daCLIENTEVA(FOperador).JuncaoAtiva();
            if (juncaoAtiva)
                new daUsuarioVANovo(FOperador).AtualizaUsuarioPreJuncao(usu);
        }

        #endregion

        #region Exclusao

        public void ValidarExclusao(USUARIOVA Usuario)
        {
            daUsuarioVA daUsuario = new daUsuarioVA(FOperador);
            if (!daUsuario.StatusInativo(Usuario))
            {
                throw new Exception("Exclusao nao pode ser efetuada.\n" +
                    "Para ser excluido o usuario devera estar com status diferente de Ativo ha pelo menos 7 dias.\nFavor verificar.");
            }
            else if (daUsuario.ExisteTransacao(Usuario))
            {
                throw new Exception("Exclusao nao pode ser efetuada.\n" +
                    "A base de dados contem transacoes para este usuario .\nFavor verificar.");
            }
        }

        public void ValidarExclusao(USUARIO_VA Usuario)
        {
            daUsuarioVA daUsuario = new daUsuarioVA(FOperador);
            if (!daUsuario.StatusInativo(Usuario))
            {
                throw new Exception("Exclusao nao pode ser efetuada.\n" +
                    "Para ser excluido o usuario devera estar com status diferente de Ativo ha pelo menos 7 dias.\nFavor verificar.");
            }
            else if (daUsuario.ExisteTransacao(Usuario))
            {
                throw new Exception("Exclusao nao pode ser efetuada.\n" +
                    "A base de dados contem transacoes para este usuario .\nFavor verificar.");
            }
        }

        public bool Excluir(USUARIOVA Usuario)
        {
            //Persistir
            try
            {
                daUsuarioVA daUsuario = new daUsuarioVA(FOperador);
                return daUsuario.Excluir(Usuario);

            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public bool Excluir(USUARIO_VA Usuario)
        {
            //Persistir
            try
            {
                daUsuarioVA daUsuario = new daUsuarioVA(FOperador);
                return daUsuario.Excluir(Usuario);

            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        #endregion

        #region Dependentes

        #region Selecao

        public List<USUARIO_VA> SelectDependentes_VA(USUARIO_VA Usuario_VA)
        {
            List<USUARIO_VA> Colecao = new List<USUARIO_VA>();
            if ((Usuario_VA != null) && (Usuario_VA.CODCLI != null) && (Usuario_VA.NUMDEP == "0"))
            {
                daUsuarioVA daUsuarioVA = new daUsuarioVA(FOperador);
                string Filtro = string.Format("U.CODCLI = {0} AND U.CPF = '{1}' AND U.NUMDEP > 0", Usuario_VA.CODCLI, Usuario_VA.CPF);
                Colecao = daUsuarioVA.ColecaoUsuarioVA(Filtro);
            }
            return Colecao;
        }

        #endregion

        #region Inclusao

        public bool InserirDependente(int CODCLI, string CPF, string NOMUSU, short CODPAR, byte NUMDEP, DateTime DATINC, int ID)
        {
            var daUsuarioVA = new daUsuarioVA(FOperador);
            var usuario = daUsuarioVA.GetUsuarioVA(ID);
            usuario.NOMUSU = NOMUSU;
            usuario.CODPAR = Convert.ToString(CODPAR);
            usuario.NUMDEP = Convert.ToString(daUsuarioVA.NumDep(Convert.ToInt32(usuario.CODCLI), usuario.CPF));
            return daUsuarioVA.InserirUsuario(usuario);
        }

        #endregion

        #region Alteracao

        public bool AlterarDependente(int ID, int CODCLI, string CPF, string NOMUSU, short CODPAR, DateTime DATINC, byte NUMDEP)
        {
            var daUsuarioVA = new daUsuarioVA(FOperador);
            return daUsuarioVA.AlterarDependente(ID, NOMUSU, AbreviaNome(NOMUSU), CODPAR);
        }       

        #endregion

        #region Exclusao

        public bool ExcluirDependente(int ID, int CODCLI, string CPF, string NOMUSU, short CODPAR, DateTime DATINC, byte NUMDEP)
        {
            daUsuarioVA daUsuarioVA = new daUsuarioVA(FOperador);
            return daUsuarioVA.ExcluirDependente(ID);
        }

        #endregion

        #endregion

        #region Validacoes

        public bool ValidarExistenciaCPF(string CPF, string codcli)
        {
            int aux;
            Int32.TryParse(codcli, out aux);
            var daUsuario = new daUsuarioVA(FOperador);
            var statusCPF = daUsuario.CPFExistente(CPF, aux);
            var reincCRT = daUsuario.ReincluiCrtParamVa();
            if ((!string.IsNullOrEmpty(statusCPF) && statusCPF == "00") || (!string.IsNullOrEmpty(statusCPF) && !reincCRT))
                throw new Exception("CPF ja cadastrado.");
            return statusCPF == "02";
        }

        public bool PermicaoCartaoMask(int idPerfil)
        {
            var daUsuarioVA = new daUsuarioVA(FOperador);
            return daUsuarioVA.PermicaoCartaoMask(idPerfil);
        }

        public int TamanhoSenha()
        {
            var daUsuarioVA = new daUsuarioVA(FOperador);
            var tamSenha = daUsuarioVA.TamanhoSenha();
            tamSenha = tamSenha < 4 ? 4 : tamSenha;
            tamSenha = tamSenha > 8 ? 8 : tamSenha;
            return tamSenha;
        }
       
        #endregion

        #region Transacoes        

        public List<CTTRANSVA> TransacoesAutorizador(USUARIO_VA Usuario)
        {
            var daUsuarioVA = new daUsuarioVA(FOperador);
            return daUsuarioVA.TransacoesAutorizador(Usuario);
        }

        public List<CTTRANSVA> TransacoesAutorizador(USUARIO_VA Usuario, string dataIni, string dataFim)
        {
            var daUsuarioVA = new daUsuarioVA(FOperador);
            return daUsuarioVA.TransacoesAutorizador(Usuario, dataIni,dataFim);
        }

        public void CancelarTransacoes(TRANSACVA transNetcard)
        {
            var daUsuarioVA = new daUsuarioVA(FOperador);
            daUsuarioVA.CancelarTransacao(transNetcard);
        }

        public void CancelarTransacoes(CTTRANSVA transNetcard, string justific)
        {
            var daUsuarioVA = new daUsuarioVA(FOperador);
            daUsuarioVA.CancelarTransacao(transNetcard, justific);
        }

        public List<CTTRANSVA> ListaTransacoesNetcard(CONSULTA_VA filtros)
        {
            filtros.SISTEMA = (short)ConstantesSIL.SistemaPRE;
            return new daTransacao(FOperador).GeraConsultaTransacao(filtros);
        }

        public List<CTTRANSVA> ListaTransacoesNetcardAutorizador(CONSULTA filtros, out string valTot)
        {
            filtros.SISTEMA = (short)ConstantesSIL.SistemaPRE;
            return new daTransacao(FOperador).ListaTransacoesNetcardAutorizador(filtros, out valTot);
        }

        #endregion

        #region Cartao VA

        public CTCARTVA CartaoVA(string CodCartao)
        {
            daUsuarioVA daUsuarioVA = new daUsuarioVA(FOperador);
            return daUsuarioVA.CartaoVA(CodCartao);
        }

        public CTCARTVA CartaoVA(int codCli, string cpf)
        {
            daUsuarioVA daUsuarioVA = new daUsuarioVA(FOperador);
            return daUsuarioVA.CartaoVA(codCli, cpf);
        }
       
        public string Gerar2ViaCartao(USUARIO_VA usu, bool cobrarSegVia)
        {
            usu.DTVALCART = System.DateTime.Today.AddMonths(Convert.ToInt16(usu.PRZVALCART)).ToString("yyMM");
            //string novo = MontaTrilha(usu.DTVALCART, 1, 2);
            string novo = MontaTrilha(ConstantesSIL.SistemaPRE, Convert.ToInt32(usu.CODCLI), usu.CPF, Convert.ToInt16(usu.NUMDEP));

            int posicaoIgualOuD = novo.LastIndexOf('=');

            if (posicaoIgualOuD < 0)
                posicaoIgualOuD = novo.LastIndexOf('D');


            usu.CODCRTANT = usu.CODCRT;
            usu.TRILHA2 = novo;
            usu.CODCRT = novo.Substring(0, posicaoIgualOuD);            
            usu.GERCRT = "X";
            usu.DATGERCRT = string.Empty;
            new daUsuarioVA(FOperador).Gerar2Via(usu, cobrarSegVia);
            return usu.CODCRT;
        }

        public string Cancelar2ViaCartao(USUARIO_VA usu, out string codRevert)
        {
            return new daUsuarioVA(FOperador).Cancelar2Via(usu.CODCRT, out codRevert);             
        }

        public string CancelarCartaoTitularDepend(string codCrt)
        {
            return new daUsuarioVA(FOperador).CancelarCartoes(codCrt);
        }
       
        public void TrocarSenhaCartaoTitularDepend(USUARIOVA usu)
        {
            usu.SENHA = GeraSenhaCriptografada(usu.SENHA);
            new daUsuarioVA(FOperador).TrocarSenhaCartao(usu);
        }

        public void ResetSenhaCartaoTitularDepend(USUARIO_VA usu)
        {
            usu.SENHA = GeraSenhaCriptografada(usu.CPF.Substring(0, 4));
            new daUsuarioVA(FOperador).ResetSenhaCartao(usu);
        }

        public void RenovarAcesso(USUARIO_VA usu)
        {
            usu.SENHA = GeraSenhaCriptografada(usu.CPF.Substring(0,8));
            new daUsuarioVA(FOperador).RenovarAcesso(usu);
        }

        public string ValidadeSenha(string codcrt, out string dtSenha)
        {            
            return new daUsuarioVA(FOperador).ValidadeSenha(codcrt, out dtSenha);
        }

        public string GeraSenhaCriptografada(string senhaSemCriptrografia)
        {            
            string novaSenha = string.Empty;
            novaSenha = CripSenha(senhaSemCriptrografia);
            return novaSenha;
        }
            
        #endregion

        #region Observacoes

        #region Selecao

        //public List<USUARIO_OBS> Observacoes(USUARIO_VA Usuario_VA)
        //{
        //    var Colecao = new List<USUARIO_OBS>();
        //    if ((Usuario_VA != null) && (Usuario_VA.CODCLI != null) && (Usuario_VA.NUMDEP == "0"))
        //    {
        //        var UsuarioDAL = new daUsuarioVA(FOperador);
        //        Colecao = UsuarioDAL.Observacoes(Convert.ToInt32(Usuario_VA.CODCLI), Usuario_VA.CPF);
        //    }
        //    return Colecao;
        //}

        public List<USUARIO_OBS> Observacoes(int codcli, string cpf)
        {
            return new daUsuarioVA(FOperador).Observacoes(codcli, cpf);
        }

        #endregion

        #region Inclusao

        public bool InserirObs(int CODCLI, string CPF, string OBS, DateTime DATA)
        {
            var UsuarioDAL = new daUsuarioVA(FOperador);
            return UsuarioDAL.InserirObs(CODCLI, CPF, DATA, OBS);
        }

        #endregion

        #region Alteracao

        public bool AlterarObs(Int32 codCli, string cpf, DateTime data, string obs)
        {
            var UsuarioDAL = new daUsuarioVA(FOperador);
            return UsuarioDAL.AlterarObs(codCli, cpf, data, obs);
        }

        #endregion

        #region Exclusao

        public bool ExcluirObs(int CODCLI, string CPF, string OBS, DateTime DATA, int ID)
        {
            return new daUsuarioVA(FOperador).ExcluirObs(ID);
        }

        #endregion

        #endregion

        #region Metodos Auxiliares       

        private string Int642Char(ulong pin)
        {
            int i;
            string StrTemp = string.Empty;
            int val, val2;

            for (i = 0; i < 8; i++)
            {
                val = (int)(pin & 0xFF);
                val2 = (val & 0xF0) >> 4;
                if (val2 < 10)
                    val2 = val2 + '0';
                else
                    val2 = val2 - 10 + 'A';
                StrTemp = StrTemp + (char)val2;

                val2 = (val & 0x0F);
                if (val2 < 10)
                    val2 = val2 + '0';
                else
                    val2 = val2 - 10 + 'A';
                StrTemp = StrTemp + (char)val2;

                pin = pin >> 8;
            }

            return StrTemp;
        }
       
        public string CripSenha(string senha)
        {
            //dllcrip.ENCRIPT_C dllEncrip = new ENCRIPT_C();
            //ulong a;
            string novaSenha = string.Empty;
            string senhaNormalizada = senha.PadRight(8, '0');
            novaSenha = BlCriptografia.Encrypt(senhaNormalizada);
            //a = dllEncrip.CalcDes(Convert.ToInt32(senha));
            //novaSenha = Int642Char(a);
            return novaSenha;
        }      

        #endregion

        #region Gastos

        public double GastoHoje(USUARIOVA Usuario)
        {
            daUsuarioVA daUsuarioVA = new daUsuarioVA(FOperador);
            return daUsuarioVA.GastoHoje(Usuario);
        }

        public double GastoProcessao(USUARIOVA Usuario, DateTime DataInicio, DateTime DataFim)
        {
            daUsuarioVA daUsuarioVA = new daUsuarioVA(FOperador);
            return  daUsuarioVA.GastoProcessado(Usuario, DataInicio, DataFim);
        }

        public double GastoHoje(USUARIO_VA Usuario)
        {
            daUsuarioVA daUsuarioVA = new daUsuarioVA(FOperador);
            return daUsuarioVA.GastoHoje(Usuario);
        }

        public double GastoProcessado(USUARIO_VA Usuario, DateTime DataInicio, DateTime DataFim)
        {
            daUsuarioVA daUsuarioVA = new daUsuarioVA(FOperador);
            return daUsuarioVA.GastoProcessado(Usuario, DataInicio, DataFim);
        }

        #endregion    
                            
        public List<USUARIO_VA> ColecaoUsuarioPaginados(int codCli, int ini, int fim)
        {
            return new daUsuarioVA(FOperador).ColecaoUsuarioPaging(codCli, ini, fim);
        }               
    }
}
