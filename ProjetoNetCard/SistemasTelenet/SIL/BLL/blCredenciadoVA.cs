using System;
using System.IO;
using TELENET.SIL.PO;
using TELENET.SIL.DA;
using System.Collections.Generic;
using System.Text;
using SIL.BLL;


namespace TELENET.SIL.BL
{
    public class blCredenciadoVA
    {
        readonly OPERADORA FOperador;

        public blCredenciadoVA(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        #region Credenciados

        public List<CREDENCIADO_VA> ColecaoCredenciados(string Filtro)
        {
            var CredenciadoVADAL = new daCredenciadoVA(FOperador);
            return CredenciadoVADAL.ColecaoCredenciados(Filtro);
        }

        public List<CREDENCIADO> ColecaoCredenciados()
        {
            var CredenciadoVADAL = new daCredenciadoVA(FOperador);
            return CredenciadoVADAL.ColecaoCredenciados();
        }

        public List<CREDENCIADO> GetCredenciadoCNPJPrincipal(int codCRe)
        {
            var CredenciadoVADAL = new daCredenciadoVA(FOperador);
            return CredenciadoVADAL.GetCredenciadoCNPJPrincipal(codCRe);
        }

        public List<CREDENCIADO_VA> ColecaoCredenciados(IFilter filter)
        {
            var _filtro = string.Empty;
            if (filter != null)
                _filtro = filter.FilterString;
            var lista = new daCredenciadoVA(FOperador).ColecaoCredenciados(_filtro);
            if (lista != null && lista.Count > 0)
            {
                var sort = new Sort<CREDENCIADO_VA>("CODCRE");
                lista.Sort(sort);
            }
            return lista;
        }

        #endregion

        #region Inclusao

        public void IncluirDadosCred(AFILIACAO afiliacao)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            daCredenciado.IncluirDadosCred(afiliacao);
        }

        public bool Incluir(string codAfil, CREDENCIADO Credenciado)
        {
            var retorno = false;
            Credenciado.FLAG_VA = ConstantesSIL.FlgSim;
            Credenciado.TAXSER = 0;
            Credenciado.DATSTA = DateTime.Now;
            Credenciado.DATTAXSER = DateTime.Today;
            Credenciado.DATTAXADM = DateTime.Today;
            Credenciado.DATINC = DateTime.Now;
            Credenciado.DATGERCRT = DateTime.Now;
            Credenciado.NUMFEC = 0;
            Credenciado.DIAFEC = 1;
            Credenciado.PRAPAG = 0;
            Credenciado.DATULTFEC_VA = DateTime.Today;
            Credenciado.AUTARQSF = ConstantesSIL.FlgNao;
            Credenciado.SENWEB = BlCriptografia.Encrypt(Credenciado.CODCRE.ToString().PadRight(8, '0'));
            if ((Credenciado.TIPEST == 1) && (Credenciado.CODMAT == null))
                throw new Exception("Favor informar Matriz.");
            ValidarExistenciaCodigo(Credenciado.CODCRE);
            if (!UtilSIL.ValidarCnpjCpf(Credenciado.CGC))
                throw new Exception("CNPJ ou CPF invalido. Favor verificar.");

            //Removido a validação de existencia de CNPJ para a inclusão de CNPJ em outro segmento.
            //ValidarExistenciaCnpj(Credenciado.CGC);

            //Validar se segmento do cadastrado é diferente do segmento d Principal
            //ValidarSegmentoPrincipal(Credenciado.CGC);

            AplicarOutrasValidadoes(Credenciado);
            try
            {
                var daCredenciado = new daCredenciadoVA(FOperador);
                retorno = daCredenciado.Inserir(codAfil, Credenciado);
                var juncaoAtiva = new daCLIENTEVA(FOperador).JuncaoAtiva();
                if (retorno && juncaoAtiva)
                    new daCredenciadoVANovo(FOperador).AtualizaCredenciadoPreJuncao(Credenciado.CODCRE);
                return retorno;

            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        #endregion

        #region Alteracao

        public bool Alterar(CREDENCIADO Credenciado)
        {
            var retorno = false;
            var CredenciadoAux = GetCredenciado(Credenciado.CODCRE);
            if ((CredenciadoAux != null) && (Credenciado.STA != CredenciadoAux.STA))
                Credenciado.DATSTA = DateTime.Now;
            if (!UtilSIL.ValidarCnpjCpf(Credenciado.CGC))
                throw new Exception("CNPJ ou CPF invalido. Favor verificar.");
            AplicarOutrasValidadoes(Credenciado);
            var daCredenciado = new daCredenciadoVA(FOperador);
            retorno = daCredenciado.Alterar(Credenciado);
            var juncaoAtiva = new daCLIENTEVA(FOperador).JuncaoAtiva();
            if (retorno && juncaoAtiva)
                new daCredenciadoVANovo(FOperador).AtualizaCredenciadoPreJuncao(Credenciado.CODCRE);
            return retorno;
        }

        #endregion

        #region Exclusao

        public void ValidarExclusao(CREDENCIADO Credenciado)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
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

        public bool Excluir(CREDENCIADO Credenciado)
        {
            try
            {
                var daCredenciado = new daCredenciadoVA(FOperador);
                return daCredenciado.Excluir(Credenciado);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        
        #endregion

        #region Importacao

        public bool ImportarNetcardPj(Int32 Credenciado)
        {
            try
            {
                var daCredenciado = new daCredenciadoVA(FOperador);
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
                var daCredenciado = new daCredenciadoVA(FOperador);
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
                var daCredenciado = new daCredenciadoVA(FOperador);
                daCredenciado.InserirTabrafil(codOpe, cnpj, codDestino);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public string ObtemRede(string codDestino)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.ObtemRede(codDestino);
        }

        public AFILIACAO RetornaDadosCred(string cnpj)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.RetornaDadosCred(cnpj);
        }

        #endregion

        #region Validacoes

        public string ValidarExistenciaCodigo(int Codigo)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            if (daCredenciado.CodigoExistente(Codigo))
                return "Credenciado ja cadastrado no Sistema.";
            return string.Empty;
        }
        
        public bool CredenciadoExistenteNetcardpj(int Codigo)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return (daCredenciado.CredenciadoExistenteNetcardPj(Codigo));
        }

        public bool CredenciadoExistenteNetcardpj(string cnpj, out int codcre)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);            
            return (daCredenciado.CredenciadoExistenteNetcardPj(cnpj, out codcre));
        }

        public bool CredenciadoExistentePrincipal(string cnpj, out int codcre)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.CnpjExistentePrincipal(cnpj, out codcre);
        }

        public bool CredenciadoExistenteDadosCred(string cnpj)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return (daCredenciado.CredenciadoExistenteDadosCred(cnpj));
        }

        //public int ValidarExistenciaCnpj(string cnpj)
        //{
        //    var daCredenciado = new daCredenciadoVA(FOperador);
        //    return (daCredenciado.CnpjExistente(cnpj));

        //}

        public bool ValidarSegmentoPrncipal(string cnpj)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
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
        public List<CREDENCIADO_OBS> Observacoes(Int32 IdCredenciado)
        {
            var CredenciadoDAL = new daCredenciadoVA(FOperador);
            return CredenciadoDAL.Observacoes(IdCredenciado);
        }
        #endregion

        #region Inclusao

        public bool InserirObs(int CODCRE, DateTime DATA, string OBS, Int32 ID)
        {
            var CredenciadoDAL = new daCredenciadoVA(FOperador);
            return CredenciadoDAL.InserirObs(CODCRE, DATA, OBS, ID);
        }

        #endregion

        #region Alteracao
        public bool AlterarObs(Int32 ID, string OBS)
        {
            var CredenciadoDAL = new daCredenciadoVA(FOperador);
            return CredenciadoDAL.AlterarObs(ID, OBS);
        }
        #endregion

        #region Exclusao
        public bool ExcluirObs(Int32 ID)
        {
            var CredenciadoDAL = new daCredenciadoVA(FOperador);
            return CredenciadoDAL.ExcluirObs(ID);
        }
        #endregion

        #endregion

        #region Proximo Codigo Livre
        public int ProximoCodigoLivre()
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.ProximoCodigoLivre();
        }
        #endregion

        #region Taxas Associadas

        public bool ExibeModuloTaxaCre()
        {
            var credenciadoDAL = new daCredenciadoVA(FOperador);
            return credenciadoDAL.ExibeModuloTaxaCre();
        }

        public MODTAXA ConsultaCodTaxaCre(Int32 codCre, Int32 codTaxa)
        {
            var CredenciadoVADAL = new daCredenciadoVA(FOperador);
            return CredenciadoVADAL.ConsultaCodTaxaCre(codCre, codTaxa);
        }

        public List<MODTAXA> ConsultaTaxaCre(Int32 IdCredenciado)
        {
            var CredenciadoVADAL = new daCredenciadoVA(FOperador);
            return CredenciadoVADAL.ConsultaTaxaCre(IdCredenciado);
        }

        public bool SalvarTaxaCre(MODTAXA taxacre)
        {
            var CredenciadoDAL = new daCredenciadoVA(FOperador);
            return CredenciadoDAL.SalvarTaxaCre(taxacre);
        }

        public bool ExcluirTaxaCre(MODTAXA taxacre)
        {
            try
            {
                var daCredenciado = new daCredenciadoVA(FOperador);
                return daCredenciado.ExcluirTaxaCre(taxacre);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public void ValidarExclusaoTaxaCre(MODTAXA taxacre)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            var trasacao = daCredenciado.ValidarExclusaoTaxaCre(taxacre);
            if (trasacao > 0)
            {
                throw new Exception("Exclusao nao pode ser efetuada.\n" +
                                    "Existem transacoes vinculadas a essa taxa.");
            }
        }

        public string ValidarNumPac(Int32 codTaxa)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.ValidarNumPac(codTaxa);
        }

        #endregion

        #region Equipamentos

        public List<CTPOS> Equipamentos(string codAfil, int codOpe)
        {
            var CredenciadoVADAL = new daCredenciadoVA(FOperador);
            return CredenciadoVADAL.ColecaoEquipamentos(codAfil, codOpe);
        }

        public bool InserirEquipamento(int codOpe, string codAfil, CREDENCIADO Credenciado)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.InserirEquipamento(codOpe, codAfil, Credenciado);
        }

        public bool ExcluirEquipamento(string IDEquipamento, CREDENCIADO Credenciado)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.ExcluirEquipamento(IDEquipamento, Credenciado);
        }

        #endregion        

        #region Redes

        public List<REDES> ConsultaRedes(Int32 IdCredenciado)
        {
            var CredenciadoVADAL = new daCredenciadoVA(FOperador);
            return CredenciadoVADAL.ConsultaRedes(IdCredenciado);
        }

        public string SalvarHabRedes(REDES rede, string cnpj, bool excluir)
        {
            var CredenciadoDAL = new daCredenciadoVA(FOperador);
            var retorno  = CredenciadoDAL.SalvarHabRedes(rede, cnpj, excluir);
            CredenciadoDAL.EqualizarHabRedes(rede, cnpj, excluir);
            return retorno;
        }

        #endregion

        #region Especialidades

        #region Selecao

        public List<ESPATIVADA_CREDENCIADO> EspAtivas(int codCre)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.EspAtivas(codCre);
        }

        public List<ESPATIVADA_CREDENCIADO> EspDisponiveis(int codCre)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.EspDisponiveis(codCre);
        }

        #endregion

        #region Inclusao

        public bool InserirEspec(int codCre, int codEsp)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.InserirEspAtiva(codCre, codEsp);
        }

        #endregion

        #region Inclusao Todos

        public bool InserirEspec(int codCre)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.InserirEspAtiva(codCre);
        }

        #endregion

        #region Exclusao

        public bool ExcluirEspAtiva(int codCre, int codEsp)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.ExcluirEspAtiva(codCre, codEsp);
        }

        #endregion

        #region Exclusao Todos

        public bool ExcluirEspAtiva(int codCre)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.ExcluirEspAtiva(codCre);
        }

        #endregion

        #endregion

        #region Modulo Web

        public bool RenovarAcesso(CREDENCIADO Credenciado)
        {
            Credenciado.SENWEB = BlCriptografia.Encrypt(Credenciado.CODCRE.ToString().PadRight(8, '0'));
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.RenovarAcesso(Credenciado);
        }

        public string ValidadeSenha(int codCre)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.ValidadeSenha(codCre);
        }

        #endregion

        #region Codogo Afiliado

        public string CodigoAfiliacao(string cnpj, int codCre)
        {
            var CredenciadoVADAL = new daCredenciadoVA(FOperador);
            return CredenciadoVADAL.CodigoAfiliacao(cnpj, codCre);
        }

        #endregion

        #region Geracao Arquivo Equipamento
        public string GeracaoArqEquipamento(string PathUpload, string PathAbsolutoArquivo, int Credenciado, string CodEquipamento)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            var LinhaDados = daCredenciado.EquipamentoArqParametrizacao(Credenciado, CodEquipamento);

            #region Geracao Arquivo

            var ArquivoGerado = string.Empty;
            var NomeArqSeq = string.Format("{0}.DAT", CodEquipamento);
            var ArquivoExportacao = string.Format(@"{0}\{1}", PathAbsolutoArquivo, NomeArqSeq);
            var ArqDados = new FileStream(ArquivoExportacao, FileMode.Create);
            var DadosArquivo = new StreamWriter(ArqDados);
            if (LinhaDados.Length > 0)
            {
                ArquivoGerado = string.Format(@"{0}/{1}", PathUpload, NomeArqSeq);
                DadosArquivo.Write(LinhaDados);
                DadosArquivo.Close();
            }
            return ArquivoGerado;
            
            #endregion
        }
        #endregion

        #region Get Dados

        public CREDENCIADO GetCredenciado(int codCre)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.GetCredenciado(codCre);
        }

        #endregion
                
        #region Grupo Credenciados

        public List<GRUPO> ColecaoGrupos(string filtro)
        {
            var dal = new daCredenciadoVA(FOperador);
            return dal.ColecaoGrupos(filtro);
        }

        public List<GRUPOCREDENCIADO> ColecaoCredenciadosPorGrupo(int idGrupo)
        {
            var dal = new daCredenciadoVA(FOperador);
            return dal.ColecaoCredenciadosPorGrupo(idGrupo);
        }

        public void ExcluirCredenciadosGrupo(int idGrupo, List<object> listaCredenciados)
        {
            var dal = new daCredenciadoVA(FOperador);
            dal.ExcluirCredenciadosGrupo(idGrupo, listaCredenciados);
        }
      
        public bool SalvarGrupo(string nome)
        {
            var dal = new daCredenciadoVA(FOperador);
            return dal.SalvarGrupo(dal.ProximoCodigoLivreGrupoCrendenciados(), nome);
        }

        public void ExcluirGrupo(int codGrupo)
        {
            var dal = new daCredenciadoVA(FOperador);
            dal.ExcluirGrupo(codGrupo);
        }

        public List<CREDENCIADO_VA> ColecaoCredenciadosForaDoGrupo(string idGrupo, string bairro, string codigoCredenciado, string localidade, 
            string segmento, string uf)
        {
            int codGrupo;
            int.TryParse(idGrupo, out codGrupo);
            int idCredenciado;
            int.TryParse(codigoCredenciado, out idCredenciado);
            var dal = new daCredenciadoVA(FOperador);
            var filter = new StringBuilder();
            filter.AppendLine(" SELECT DISTINCT CC.CODCRE, C.NOMFAN, C.RAZSOC");
            filter.AppendLine(" FROM CADCREDENCIADO C WITH (NOLOCK) ");
            filter.AppendLine("	 INNER JOIN CREDENCIADO_CTRL CC ON C.ID_CREDENCIADO = CC.ID_CREDENCIADO ");
            filter.AppendLine(" INNER JOIN BAIRRO B  ON C.CODBAI = B.CODBAI ");
            filter.AppendLine(" INNER JOIN SEGMENTO S  ON CC.CODSEG = S.CODSEG ");
            filter.AppendLine(" INNER JOIN LOCALIDADE L  ON C.CODLOC = L.CODLOC ");
            filter.AppendLine(" INNER JOIN UF U  ON C.SIGUF0 = U.SIGUF0 ");
            filter.AppendLine(string.Format(" WHERE CC.CODCRE NOT IN (SELECT CODCRE FROM GRUPOCRED WITH (NOLOCK) WHERE CODGRUPO = {0} )", codGrupo));
            if (idCredenciado > 0)
                filter.AppendLine(string.Format(" AND CC.CODCRE = {0} ", idCredenciado));
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

        public void IncluirCredenciadosGrupo(string codGrupo, List<object> listaCredenciados)
        {
            int idGrupo;
            int.TryParse(codGrupo, out idGrupo);
            if (idGrupo < 1) return;
            var dal = new daCredenciadoVA(FOperador);
            dal.IncluirCredenciadosGrupo(idGrupo, listaCredenciados);
        }

        public bool RecuperaGrupobyNome(string nomeGrupo)
        {
            var dal = new daCredenciadoVA(FOperador);
            return dal.RecuperaGrupobyNome(nomeGrupo) != string.Empty;
        }     

        #endregion      
 
        #region 4DatasCred
        public List<_4DATAS> Proc_Ler_4datas_Fechcred(int DIAFEC_VA) 
        {
            var dal = new daCredenciadoVA(FOperador);
            var filter = new StringBuilder();
            return dal.Proc_Ler_4datas_Fechcred(DIAFEC_VA);
        }

        public int Proc_Gera_4dadas_Fechcred(int DIA1, int DIA2, int DIA3, int DIA4) 
        {
            var dal = new daCredenciadoVA(FOperador);
            var filter = new StringBuilder();
            return dal.Proc_Gera_4dadas_Fechcred(DIA1, DIA2, DIA3, DIA4);        
        }
        public bool BuscaParamFechCred4d()
        {
            var dal = new daCredenciadoVA(FOperador);
            var filter = new StringBuilder();
            return dal.BuscaParamFechCred4d();
        }
        #endregion

        #region TransHab

        public TRANSHABCKB GetTransHab(string transHab)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.GetTransHab(transHab);
        }

        public string SetTransHab(TRANSHABCKB transHab)
        {
            var daCredenciado = new daCredenciadoVA(FOperador);
            return daCredenciado.SetTransHab(transHab);
        }

        #endregion
    }
}
