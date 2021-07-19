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
    public class blUsuarioVANovo
    {
        readonly OPERADORA FOperador;

        public blUsuarioVANovo(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        #region GET

        public CADUSUARIO GetUsuario(int idUsuario)
        {
            var UsuarioDAL = new daUsuarioVANovo(FOperador);
            return UsuarioDAL.GetUsuario(idUsuario);
        }

        public List<VPRODUTOSUSU> GetUsuarioProd(int idUsuario)
        {
            var usuarioDAL = new daUsuarioVANovo(FOperador);
            return usuarioDAL.GetUsuarioProd(idUsuario);
        }

        public List<VUSUARIODEP> GetUsuariosDep(int codCli, string cpf)
        {
            var usuarioDAL = new daUsuarioVANovo(FOperador);
            return usuarioDAL.GetUsuariosDep(codCli, cpf);
        }

        public List<VRESUMOUSU> ColecaoUsuario(string Filtro)
        {
            var UsuarioVADAL = new daUsuarioVANovo(FOperador);
            return UsuarioVADAL.ColecaoUsuario(Filtro);
        }

        public List<JustSegViaCard> ColecaoJustSegViaCard()
        {
            var UsuarioVADAL = new daUsuarioVANovo(FOperador);
            return UsuarioVADAL.ColecaoJustSegViaCard();
        }

        public List<CalcParcela> ColecaoCalcParcela(int maxParcela)
        {
            List<CalcParcela> lista = new List<CalcParcela>();

            for (int i = 1; i <= maxParcela; i++)
            {
                lista.Add(new CalcParcela() { codCalcParcela = i, nomCalcParcela = i.ToString() });
            }

            return lista;
        }
        public CalcValorParcela GetCalcParcela(int numParcela, decimal vlLimiete, int limiteRisco, decimal saldoAtual, decimal vlComprometido)
        {            
            var UsuarioVADAL = new daUsuarioVANovo(FOperador);
            return UsuarioVADAL.GetCalcParcela(numParcela, vlLimiete, limiteRisco, saldoAtual, vlComprometido);
        }
        

        public SALDO GetSaldo(VPRODUTOSUSU usuario)
        {
            var UsuarioVADAL = new daUsuarioVANovo(FOperador);
            return UsuarioVADAL.GetSaldo(usuario);
        }

        public decimal GetSaldoComprometido(VPRODUTOSUSU usuario)
        {
            var UsuarioVADAL = new daUsuarioVANovo(FOperador);
            return UsuarioVADAL.GetSaldoComprometido(usuario);
        }
        

        #endregion

        #region CRUD

        public int Incluir(CADUSUARIO cadUsuario, out string mensagem)
        {
            if (!UtilSIL.ValidarCpf(cadUsuario.CPF))
            {
                mensagem = "CPF inválido. Favor verificar.";
                return 0;
            }
            try
            {
                var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
                return daUsuarioVANovo.Inserir(cadUsuario, out mensagem);
            }
            finally { }
        }

        public bool IncluirCartaoPos(VPRODUTOSUSU usuarioProd, out string mensagem)
        {
            var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
            return daUsuarioVANovo.IncluirCartaoPos(usuarioProd, out mensagem);
        }

        public bool IncluirCartaoPre(VPRODUTOSUSU usuarioProd, out string mensagem)
        {
            var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
            return daUsuarioVANovo.IncluirCartaoPre(usuarioProd, out mensagem);
        }

        public bool Alterar(CADUSUARIO cadUsuario, out string mensagem)
        {
            if (!cadUsuario.CPF.Contains("FC") && !UtilSIL.ValidarCpf(cadUsuario.CPF))
            {
                mensagem = "CPF invalido. Favor verificar.";
                return false;
            }
            try
            {
                var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
                return daUsuarioVANovo.Alterar(cadUsuario, out mensagem);
            }
            finally { }
        }

        public bool AlterarCartaoPos(VPRODUTOSUSU usuarioProd, out string mensagem)
        {
            var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
            return daUsuarioVANovo.AlterarCartaoPos(usuarioProd, out mensagem);
        }

        public bool AlterarCartaoPre(VPRODUTOSUSU usuarioProd, out string mensagem)
        {
            var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
            return daUsuarioVANovo.AlterarCartaoPre(usuarioProd, out mensagem);
        }

        public bool ManuDependente(VUSUARIODEP usuarioDep, out string mensagem)
        {
            var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
            return daUsuarioVANovo.ManuDependente(usuarioDep, out mensagem); 
        }

        #endregion

        #region Exclusao        

        public bool ExcluirCadUsuario(string cpf, out string retorno)
        {
            //Persistir
            try
            {
                var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
                return daUsuarioVANovo.ExcluirCadUsuario(cpf, out retorno);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public bool ExcluirCartao(int sistema, int codCli, string cpf, int numDep, out string retorno)
        {
            //Persistir
            try
            {
                var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
                return daUsuarioVANovo.ExcluirCartao(sistema, codCli, cpf, numDep, out retorno);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        #endregion

        #region Transacoes

        public List<CTTRANSVA> ListaTransacoesNetcardAutorizador(CONSULTA filtros, out string valTot)
        {
            return new daTransacao(FOperador).ListaTransacoesNetcardAutorizador(filtros, out valTot);
        }

        #region Gastos

        public double GastoHoje(VPRODUTOSUSU usuario)
        {
            var daUsuarioVA = new daUsuarioVANovo(FOperador);
            return daUsuarioVA.GastoHoje(usuario);
        }

        public double GastoProcessado(VPRODUTOSUSU usuario, DateTime dataInicio, DateTime dataFim, int lote)
        {
            var daUsuarioVA = new daUsuarioVANovo(FOperador);
            return daUsuarioVA.GastoProcessado(usuario, dataInicio, dataInicio, lote);
        }

        #endregion    

        #endregion

        #region Observações Pós Pago

        #region Selecao Pós Pago

        public List<USUARIO_OBS> ObservacoesPos(Int32 CODCLI, string CPF)
        {
            var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
            return daUsuarioVANovo.ObservacoesPos(CODCLI, CPF);
        }

        #endregion

        #region Inclusao Pós

        public bool InserirObsPos(int CODCLI, string CPF, DateTime DATA, string OBS)
        {
            var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
            return daUsuarioVANovo.InserirObsPos(CODCLI, CPF, DATA, OBS);
        }

        #endregion

        #region Exclusao Pós

        public bool ExcluirObsPos(int CODCLI, string CPF, DateTime DATA)
        {
            var daUsuarioVANovo = new daUsuarioVANovo(FOperador);            
            return daUsuarioVANovo.ExcluirObsPos(CODCLI, CPF, DATA);
        }

        #endregion

        #endregion

        #region Observações Pré Pago

        #region Selecao Pré Pago

        public List<USUARIO_OBS> ObservacoesPre(Int32 CODCLI, string CPF)
        {
            var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
            return daUsuarioVANovo.ObservacoesPre(CODCLI, CPF);
        }

        #endregion

        #region InclusaoVa

        public bool InserirObsPre(int CODCLI, string CPF, DateTime DATA, string OBS)
        {
            var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
            return daUsuarioVANovo.InserirObsPre(CODCLI, CPF, DATA, OBS);
        }

        #endregion

        #region ExclusaoVa

        public bool ExcluirObsPre(int CODCLI, string CPF, DateTime DATA)
        {
            var daUsuarioVANovo = new daUsuarioVANovo(FOperador);
            return daUsuarioVANovo.ExcluirObsPre(CODCLI, CPF, DATA);
        }

        #endregion

        #endregion

        #region GET Taxas e Benefícios

        public List<TAXAUSUARIO> GetTaxaUsuario(VPRODUTOSUSU usuario)
        {
            return new daUsuarioVANovo(FOperador).GetTaxaUsuario(usuario); 
        }

        public List<TAXAUSUARIO> GetTaxasAAssociar(VPRODUTOSUSU usuario)
        {
            return new daUsuarioVANovo(FOperador).GetTaxasAAssociar(usuario); 
        }

        public string AssociarTaxas(VPRODUTOSUSU usuario, int codTaxa)
        {
            return new daUsuarioVANovo(FOperador).AssociarTaxas(usuario, codTaxa);
        }

        public string DesassociarTaxas(VPRODUTOSUSU usuario, int codTaxa)
        {
            return new daUsuarioVANovo(FOperador).DesassociarTaxas(usuario, codTaxa);
        }

        public List<BENEFICIOUSUARIO> GetBeneficiosUsuario(VPRODUTOSUSU usuario)
        {
            return new daUsuarioVANovo(FOperador).GetBeneficiosUsuario(usuario);
        }

        public List<BENEFICIOUSUARIO> GetBeneficiosAAssociar(VPRODUTOSUSU usuario)
        {
            return new daUsuarioVANovo(FOperador).GetBeneficiosAAssociar(usuario);
        }

        public string AssociarBeneficio(VPRODUTOSUSU usuario, int codTaxa)
        {
            return new daUsuarioVANovo(FOperador).AssociarBeneficio(usuario, codTaxa);
        }

        public string DesassociarBeneficio(VPRODUTOSUSU usuario, int codTaxa)
        {
            return new daUsuarioVANovo(FOperador).DesassociarBeneficio(usuario, codTaxa);
        }

        #endregion

        #region Ações Cartões

        public bool Gerar2ViaCartao(VPRODUTOSUSU usu, bool mantemAntigo, bool cobrarSegVia, int CodJustSegViaCard,  out string mensagem)
        {
            return new daUsuarioVANovo(FOperador).Gerar2ViaCartao(usu, mantemAntigo, cobrarSegVia, CodJustSegViaCard, out mensagem);                        
        }
        public bool ReinclusaoCartaoCancelado(VPRODUTOSUSU usu, string CobraTaxa, out string mensagem)
        {
            return new daUsuarioVANovo(FOperador).ReinclusaoCartaoCancelado(usu, CobraTaxa, out mensagem);
        }
        public bool ReativacaoCartaoSuspenso(VPRODUTOSUSU usu, out string mensagem)
        {
            return new daUsuarioVANovo(FOperador).ReativacaoCartaoSuspenso(usu, out mensagem);
        }        
        public bool OperadorTelenetSupervisor(int idPerfil)
        {
            return new daUsuarioVANovo(FOperador).OperadorTelenetSupervisor(idPerfil);                        
        }
        public int DiasCartoesAntigoFuncionando()
        {
            return new daUsuarioVANovo(FOperador).DiasCartoesAntigoFuncionando();
        }

        public bool ExibeCvv()
        {
            return new daUsuarioVANovo(FOperador).ExibeCvv();
        }

        public bool TemCashback()
        {
            return new daUsuarioVANovo(FOperador).TemCashback();
        }

        public bool BloqUsuModulo(int sistema)
        {
            return new daUsuarioVANovo(FOperador).BloqUsuModulo(sistema);
        }

        public bool Cancelar2ViaCartao(VPRODUTOSUSU usu, out string mensagem)
        {
            return new daUsuarioVANovo(FOperador).Cancelar2ViaCartao(usu, out mensagem);
        }

        public bool ResetSenhaCartao(VPRODUTOSUSU usu, out string mensagem)
        {
            return new daUsuarioVANovo(FOperador).ResetSenhaCartao(usu, out mensagem);
        }

        public bool RenovarAcesso(VPRODUTOSUSU usu, out string mensagem)
        {
            return new daUsuarioVANovo(FOperador).RenovarAcesso(usu, out mensagem);
        }

        public bool CancelarCartaoTitularDepend(VPRODUTOSUSU usuario, out string mensagem)
        {
            return new daUsuarioVANovo(FOperador).CancelarCartoes(usuario, out mensagem);
        }
        public bool InsereCartaoBloqueado(int codcli, int Sistema)
        {
            return new daUsuarioVANovo(FOperador).InsereCartaoBloqueado(codcli, Sistema);
        }

        #endregion

        #region Get CEP

        public ENDCEP GetCep(string cep)
        {
            return new daUsuarioVANovo(FOperador).GetCep(cep);
        }

        #endregion
    }
}
