using System;
using System.IO;
using TELENET.SIL.PO;
using TELENET.SIL.DA;
using System.Collections.Generic;
using System.Text;
using SIL.BLL;


namespace TELENET.SIL.BL
{
    public class blCredenciadoVANovo
    {
        readonly OPERADORA FOperador;

        public blCredenciadoVANovo(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        #region Credenciados

        public List<VRESUMOCRE> ColecaoCredenciados(string Filtro)
        {
            var CredenciadoVADAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoVADAL.ColecaoCredenciados(Filtro);
        }

        public List<VCREDENCIADO> ColecaoCredenciados()
        {
            var CredenciadoVADAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoVADAL.ColecaoCredenciados();
        }

        public List<VCREDENCIADO> GetCredenciadoCNPJPrincipal(int codCRe)
        {
            var CredenciadoVADAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoVADAL.GetCredenciadoCNPJPrincipal(codCRe);
        }

        public int GetCredenciadoCodPosCodAfil(string codPos, string codAfil)
        {
            var CredenciadoVADAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoVADAL.GetCredenciadoCodPosCodAfil(codPos, codAfil);
        }

        public List<VRESUMOCRE> ColecaoCredenciados(IFilter filter)
        {
            var _filtro = string.Empty;
            if (filter != null)
                _filtro = filter.FilterString;
            var lista = new daCredenciadoVANovo(FOperador).ColecaoCredenciados(_filtro);
            if (lista != null && lista.Count > 0)
            {
                var sort = new Sort<VRESUMOCRE>("CODCRE");
                lista.Sort(sort);
            }
            return lista;
        }

        #endregion

        #region Inclusao

        public int Incluir(CADCREDENCIADO cadCredenciado, out string mensagem)
        {            
            try
            {
                var daCredenciado = new daCredenciadoVANovo(FOperador);
                return daCredenciado.Inserir(cadCredenciado, out mensagem);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public bool Incluir(VCREDENCIADO credenciado, out string mensagem)
        {
            try
            {
                var daCredenciado = new daCredenciadoVANovo(FOperador);
                return daCredenciado.Inserir(credenciado, out mensagem);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        #endregion

        #region Alteracao

        public bool Alterar(CADCREDENCIADO cadCredenciado, out string mensagem)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.Alterar(cadCredenciado, out mensagem);
        }

        public bool Alterar(VCREDENCIADO credenciado, out string mensagem)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.Alterar(credenciado, out mensagem);
        }

        #endregion

        #region Exclusao

        public void ValidarExclusao(VCREDENCIADO Credenciado)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            if (!daCredenciado.ValidarExclusao(Credenciado))
            {
                throw new Exception("Exclusao nao pode ser efetuada.\n" +
                    "Para ser excluido o Credenciado devera estar com status igual a Cancelado ha pelo menos 7 dias.\nFavor verificar.");
            }
            var Centralizadora = daCredenciado.CentralizadoraDependente(Credenciado.CODCRE);
            if (Centralizadora != string.Empty)
            {
                throw new Exception(string.Format("Exclusao nao pode ser efetuada.\n" +
                    "O Credenciado e Centralizadora de '{0}'.\nFavor verificar.", Centralizadora));
            }
            var Matriz = daCredenciado.MatrizDependente(Credenciado.CODCRE);
            if (Matriz != string.Empty)
            {
                throw new Exception(string.Format("Exclusao nao pode ser efetuada.\n" +
                    "O Credenciado e Matriz de '{0}'.\nFavor verificar.", Centralizadora));
            }
        }

        public bool Excluir(VCREDENCIADO Credenciado)
        {
            try
            {
                var daCredenciado = new daCredenciadoVANovo(FOperador);
                return daCredenciado.Excluir(Credenciado);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public bool ExcluirCadCredenciado(string cnpj, out string retorno)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.ExcluirCadCredenciado(cnpj, out retorno);
        }

        public bool ExcluirCredenCtrl(int codCre, out string retorno)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.ExcluirCredenCtrl(codCre, out retorno);
        }

        #endregion

        #region Importacao

        public bool ImportarNetcardPj(Int32 Credenciado)
        {
            try
            {
                var daCredenciado = new daCredenciadoVANovo(FOperador);
                return daCredenciado.ImportarNetcardPj(Credenciado);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public bool ImportarTaxasPj(Int32 Credenciado)
        {
            try
            {
                var daCredenciado = new daCredenciadoVANovo(FOperador);
                return daCredenciado.ImportarTaxasPj(Credenciado);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public void InserirTabrafil(int codOpe, string cnpj, string codDestino)
        {
            try
            {
                var daCredenciado = new daCredenciadoVANovo(FOperador);
                daCredenciado.InserirTabrafil(codOpe, cnpj, codDestino);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public string ObtemRede(string codDestino)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.ObtemRede(codDestino);
        }

        public AFILIACAO RetornaDadosCred(string cnpj)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.RetornaDadosCred(cnpj);
        }

        #endregion

        #region Validacoes

        public void ValidarExistenciaCodigo(int Codigo)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            if (daCredenciado.CodigoExistente(Codigo))
                throw new Exception("Credenciado ja cadastrado no Sistema.");
        }
        
        public bool CredenciadoExistenteNetcardpj(int Codigo)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return (daCredenciado.CredenciadoExistenteNetcardPj(Codigo));
        }

        public bool CredenciadoExistenteNetcardpj(string cnpj, out int codcre)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);            
            return (daCredenciado.CredenciadoExistenteNetcardPj(cnpj, out codcre));
        }

        public bool CredenciadoExistentePrincipal(string cnpj, out int codcre)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.CnpjExistentePrincipal(cnpj, out codcre);
        }

        public bool CredenciadoExistenteDadosCred(string cnpj)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return (daCredenciado.CredenciadoExistenteDadosCred(cnpj));
        }

        //public int ValidarExistenciaCnpj(string cnpj)
        //{
        //    var daCredenciado = new daCredenciadoVANovo(FOperador);
        //    return (daCredenciado.CnpjExistente(cnpj));

        //}

        public bool ValidarSegmentoPrncipal(string cnpj)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return (daCredenciado.ValidarSegmentoPrncipal(cnpj));

        }

        static void ValidarFaixaValor(string Label, int Valor, int Minimo, int Maximo)
        {
            if ((Valor < Minimo) || (Valor > Maximo))
                throw new Exception(string.Format("Favor informar {0} entre {1} e {2}.", Label, Minimo, Maximo));
        }

        static void AplicarOutrasValidadoes(CREDENCIADO Credenciado)
        {
            if (Credenciado.CODCRE == 0)
                throw new Exception("Favor informar o codigo.");
            ValidarFaixaValor("Numero Parcelas", Convert.ToInt16(Credenciado.NUMPACVA), 0, 12);
            switch (Credenciado.TIPFEC_VA)
            {
                case 1:
                    ValidarFaixaValor("Dia de Fechamento", Credenciado.DIAFEC_VA, 1, 7);
                    break;
                case 2:
                    ValidarFaixaValor("Dia de Fechamento", Credenciado.DIAFEC_VA, 1, 15);
                    break;
                case 3:
                    ValidarFaixaValor("Dia de Fechamento", Credenciado.DIAFEC_VA, 1, 31);
                    break;
            }
            if (Credenciado.PRAENT > Int16.MaxValue)
                throw new Exception(string.Format("Favor informar 'Prazo de entrega' ate {0}.", Int16.MaxValue));
            if (Credenciado.PRAREE > Int16.MaxValue)
                throw new Exception(string.Format("Favor informar 'Prazo de reembolso' ate {0}.", Int16.MaxValue));
        }

        #endregion

        #region Observacoes

        #region Selecao
        public List<CREDENCIADO_OBS> Observacoes(Int32 codCre)
        {
            var CredenciadoDAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoDAL.Observacoes(codCre);
        }
        #endregion

        #region Inclusao

        public bool InserirObs(int CODCRE, DateTime DATA, string OBS, Int32 ID)
        {
            var CredenciadoDAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoDAL.InserirObs(CODCRE, DATA, OBS, ID, out string mensagem);
        }

        #endregion

        #region Alteracao
        public bool AlterarObs(Int32 ID, string OBS)
        {
            var CredenciadoDAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoDAL.AlterarObs(ID, OBS);
        }
        #endregion

        #region Exclusao
        public bool ExcluirObs(Int32 ID)
        {
            var CredenciadoDAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoDAL.ExcluirObs(ID, out string mensagem);
        }
        #endregion

        #endregion

        #region Proximo Codigo Livre
        public int ProximoCodigoLivre()
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.ProximoCodigoLivre();
        }
        #endregion

        #region Taxas Associadas

        public bool ExibeModuloTaxaCre()
        {
            var credenciadoDAL = new daCredenciadoVANovo(FOperador);
            return credenciadoDAL.ExibeModuloTaxaCre();
        }

        public List<MODTAXA> ConsultaTaxaCre(int codCre, int sistema)
        {
            var CredenciadoVADAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoVADAL.ConsultaTaxaCre(codCre, sistema);
        }

        public List<MODTAXA> ConsultaTaxaCre(int codCre)
        {
            var CredenciadoVADAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoVADAL.ConsultaTaxaCre(codCre);
        }

        // #2690 - No cadastro de taxas para o credenciado,
        // precisamos evitar retornar nessa lista as taxas que já foram
        // adicionadas para o credenciado. Por isso, *se* for passado o parâmetro
        // "existentes" com a lista de taxas já inseridas, essas não serão retornadas novamente.
        public List<MODTAXA> ConsultaTaxasCre(int sistema, bool eCentralizadora, List<MODTAXA> existentes = null)
        {
            var CredenciadoVADAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoVADAL.ConsultaTaxasCre(sistema, eCentralizadora, existentes);
        }

        public bool SalvarTaxaCre(MODTAXA taxacre)
        {
            var CredenciadoDAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoDAL.SalvarTaxaCre(taxacre);
        }

        public bool ExcluirTaxaCre(MODTAXA taxacre)
        {
            try
            {
                var daCredenciado = new daCredenciadoVANovo(FOperador);
                return daCredenciado.ExcluirTaxaCre(taxacre);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public void ValidarExclusaoTaxaCre(MODTAXA taxacre)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            var trasacao = daCredenciado.ValidarExclusaoTaxaCre(taxacre);
            if (trasacao > 0)
            {
                throw new Exception("Exclusao nao pode ser efetuada.\n" +
                                    "Existem transacoes vinculadas a essa taxa.");
            }
        }

        public string ValidarNumPac(int sistema, int codTaxa)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.ValidarNumPac(sistema, codTaxa);
        }

        #endregion

        #region Exibe Novo Formato Conta Bancária
        
        public bool ExibeNovoFormatoContaBanco()
        {
            var credenciadoDAL = new daCredenciadoVANovo(FOperador);
            return credenciadoDAL.ExibeNovoFormatoContaBanco();
        }

        #endregion

        #region Equipamentos

        public List<CTPOS> Equipamentos(string codAfil, int codOpe)
        {
            var CredenciadoVADAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoVADAL.ColecaoEquipamentos(codAfil, codOpe);
        }

        public bool InserirEquipamento(int codOpe, string codAfil, int codcre, string nomExibicao)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.InserirEquipamento(codOpe, codAfil, codcre, nomExibicao);
        }
        public bool InicializarEquipamento(string codEquipamento)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.InicializarEquipamento(codEquipamento);
        }

        public bool ExcluirEquipamento(string IDEquipamento)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.ExcluirEquipamento(IDEquipamento);            
        }

        #endregion        

        #region Redes

        public List<REDES> ConsultaRedes(Int32 IdCredenciado)
        {
            var CredenciadoVADAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoVADAL.ConsultaRedes(IdCredenciado);
        }

        public string SalvarHabRedes(REDES rede, string cnpj, bool excluir)
        {
            var CredenciadoDAL = new daCredenciadoVANovo(FOperador);
            var retorno = CredenciadoDAL.SalvarHabRedes(rede, cnpj, excluir) + ";";
            retorno += CredenciadoDAL.EqualizarHabRedes(rede, cnpj, excluir);
            return retorno;
        }

        #endregion

        #region Especialidades

        #region Selecao

        public List<ESPATIVADA_CREDENCIADO> EspAtivas(int codCre)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.EspAtivas(codCre);
        }

        public List<ESPATIVADA_CREDENCIADO> EspDisponiveis(int codCre)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.EspDisponiveis(codCre);
        }

        #endregion

        #region Inclusao

        public bool InserirEspec(int codCre, int codEsp)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.InserirEspAtiva(codCre, codEsp);
        }

        #endregion

        #region Inclusao Todos

        public bool InserirEspec(int codCre)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.InserirEspAtiva(codCre);
        }

        #endregion

        #region Exclusao

        public bool ExcluirEspAtiva(int codCre, int codEsp)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.ExcluirEspAtiva(codCre, codEsp);
        }

        #endregion

        #region Exclusao Todos

        public bool ExcluirEspAtiva(int codCre)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.ExcluirEspAtiva(codCre);
        }

        #endregion

        #endregion

        #region Modulo Web

        public bool RenovarAcesso(VCREDENCIADO Credenciado, out string mensagem)
        {
            Credenciado.SENHA = BlCriptografia.Encrypt(Credenciado.CODCRE.ToString().PadRight(8, '0'));
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.RenovarAcesso(Credenciado, out mensagem);
        }

        public string ValidadeSenha(int codCre)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.ValidadeSenha(codCre);
        }

        #endregion

        #region Codogo Afiliado

        public string CodigoAfiliacao(string cnpj, int codCre)
        {
            var CredenciadoVADAL = new daCredenciadoVANovo(FOperador);
            return CredenciadoVADAL.CodigoAfiliacao(cnpj, codCre);
        }

        #endregion

        #region Geracao Arquivo Equipamento
        public string GeracaoArqEquipamento(string PathArq, int Credenciado, string CodEquipamento)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            var LinhaDados = daCredenciado.EquipamentoArqParametrizacao(Credenciado, CodEquipamento);

            #region Geracao Arquivo

            var ArquivoGerado = string.Empty;
            var NomeArqSeq = string.Format("{0}.DAT", CodEquipamento);
            var ArquivoExportacao = string.Format(@"{0}\{1}", PathArq, NomeArqSeq);
            var ArqDados = new FileStream(ArquivoExportacao, FileMode.Create);
            var DadosArquivo = new StreamWriter(ArqDados);
            if (LinhaDados.Length > 0)
            {
                ArquivoGerado = string.Format(@"{0}/{1}", PathArq, NomeArqSeq);
                DadosArquivo.Write(LinhaDados);
                DadosArquivo.Close();
            }
            return ArquivoGerado;
            
            #endregion
        }
        #endregion

        #region Get Dados

        public CADCREDENCIADO GetCadCredenciado(int idCredenciado)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.GetCadCredenciado(idCredenciado);
        }

        public List<VCREDENCIADO> GetListCredenciado(int idCredenciado)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.GetListCredenciado(idCredenciado);
        }

        public VCREDENCIADO GetCredenciado(int codCre)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.GetCredenciado(codCre);
        }

        #endregion
                
        #region Grupo Credenciados

        public List<GRUPO> ColecaoGrupos(string filtro)
        {
            var dal = new daCredenciadoVANovo(FOperador);
            return dal.ColecaoGrupos(filtro);
        }

        public List<GRUPO> ColecaoGrupos(int codCred)
        {
            var dal = new daCredenciadoVANovo(FOperador);
            return dal.ColecaoGrupos(codCred);
        }

        public List<GRUPOCREDENCIADO> ColecaoCredenciadosPorGrupo(int idGrupo)
        {
            var dal = new daCredenciadoVANovo(FOperador);
            return dal.ColecaoCredenciadosPorGrupo(idGrupo);
        }

        public void ExcluirCredenciadosGrupo(int idGrupo, List<object> listaCredenciados)
        {
            var dal = new daCredenciadoVANovo(FOperador);
            dal.ExcluirCredenciadosGrupo(idGrupo, listaCredenciados);
        }
      
        public bool SalvarGrupo(string nome)
        {
            var dal = new daCredenciadoVANovo(FOperador);
            return dal.SalvarGrupo(dal.ProximoCodigoLivreGrupoCrendenciados(), nome);
        }

        public void ExcluirGrupo(int codGrupo)
        {
            var dal = new daCredenciadoVANovo(FOperador);
            dal.ExcluirGrupo(codGrupo);
        }

        public List<CREDENCIADO_VA> ColecaoCredenciadosForaDoGrupo(string idGrupo, string bairro, string codigoCredenciado, string localidade, 
            string segmento, string uf)
        {
            int codGrupo;
            int.TryParse(idGrupo, out codGrupo);
            int idCredenciado;
            int.TryParse(codigoCredenciado, out idCredenciado);
            var dal = new daCredenciadoVANovo(FOperador);
            var filter = new StringBuilder();
            filter.AppendLine(" SELECT distinct C.CODCRE, C.NOMFAN, C.RAZSOC ");
            filter.AppendLine(" FROM CREDENCIADO C WITH (NOLOCK) ");
            filter.AppendLine(" INNER JOIN BAIRRO B ");
            filter.AppendLine(" ON C.CODBAI = B.CODBAI ");
            filter.AppendLine(" INNER JOIN SEGMENTO S ");
            filter.AppendLine(" ON C.CODSEG = S.CODSEG ");
            filter.AppendLine(" INNER JOIN LOCALIDADE L ");
            filter.AppendLine(" ON C.CODLOC = L.CODLOC ");
            filter.AppendLine(" INNER JOIN UF U ");
            filter.AppendLine(" ON C.SIGUF0 = U.SIGUF0  ");
            filter.AppendLine(string.Format(" WHERE C.FLAG_VA = 'S' AND C.CODCRE NOT IN (SELECT CODCRE FROM GRUPOCRED WITH (NOLOCK) WHERE CODGRUPO = {0} )", codGrupo));
            if (idCredenciado > 0)
                filter.AppendLine(string.Format(" AND C.CODCRE = {0} ", idCredenciado));
            if (bairro != string.Empty)
                filter.AppendLine(string.Format(" AND B.NOMBAI LIKE '{0}%' ", bairro));
            if (localidade != string.Empty)
                filter.AppendLine(string.Format(" AND L.NOMLOC LIKE '{0}%' ", localidade));
            if (segmento != string.Empty)
                filter.AppendLine(string.Format(" AND S.NOMSEG LIKE '{0}%' ", segmento));
            if (uf != string.Empty)
                filter.AppendLine(string.Format(" AND  U.SIGUF0 = '{0}' ", uf));
            filter.AppendLine(" ORDER BY C.NOMFAN ");
            return dal.ColecaoCredenciadosForaDoGrupo(filter.ToString());
        }

        public void IncluirCredenciadosGrupo(string codGrupo, List<object> listaCredenciados, out string mensagem)
        {
            mensagem = "";
            int idGrupo;
            int.TryParse(codGrupo, out idGrupo);
            if (idGrupo < 1) return;
            var dal = new daCredenciadoVANovo(FOperador);
            dal.IncluirCredenciadosGrupo(idGrupo, listaCredenciados, out mensagem);
        }

        public bool RecuperaGrupobyNome(string nomeGrupo)
        {
            var dal = new daCredenciadoVANovo(FOperador);
            return dal.RecuperaGrupobyNome(nomeGrupo) != string.Empty;
        }     

        #endregion      
 
        #region 4DatasCred
        public List<_4DATAS> Proc_Ler_4datas_Fechcred(int DIAFEC) 
        {
            var dal = new daCredenciadoVANovo(FOperador);
            DIAFEC = DIAFEC.ToString().Length < 8 ? 28221407 : DIAFEC;
            return dal.Proc_Ler_4datas_Fechcred(DIAFEC);
        }

        public int Proc_Gera_4dadas_Fechcred(int DIA1, int DIA2, int DIA3, int DIA4) 
        {
            var dal = new daCredenciadoVANovo(FOperador);
            var filter = new StringBuilder();
            return dal.Proc_Gera_4dadas_Fechcred(DIA1, DIA2, DIA3, DIA4);        
        }

        public bool BuscaParamFechCred4dPos()
        {
            var dal = new daCredenciadoVANovo(FOperador);
            var filter = new StringBuilder();
            return dal.BuscaParamFechCred4dPos();
        }

        public bool BuscaParamFechCred4dPre()
        {
            var dal = new daCredenciadoVANovo(FOperador);
            var filter = new StringBuilder();
            return dal.BuscaParamFechCred4dPre();
        }

        #endregion

        #region TransHab

        public TRANSHABCKB GetTransHab(string transHab)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.GetTransHab(transHab);
        }

        public string SetTransHab(TRANSHABCKB transHab)
        {
            var daCredenciado = new daCredenciadoVANovo(FOperador);
            return daCredenciado.SetTransHab(transHab);
        }

        #endregion
    }
}
