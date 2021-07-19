using SIL.BLL;
using SIL.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TELENET.SIL.DA;
using TELENET.SIL.PO;

namespace TELENET.SIL.BL
{
    public class blCLIENTEVA
    {
        OPERADORA FOperador;
        public blCLIENTEVA(OPERADORA Operador)
        {
            FOperador = Operador;
        }                

        #region Inclusao

        public bool Incluir(CLIENTEVA ClienteVA)
        {
            var retorno = false;
            // Aplicar Regras Negocio :: Valores Default
            ClienteVA.CTRATV = 'S';
            ClienteVA.NUMCRT = 0;
            ClienteVA.CODFILNUT = 0;

            // Validacoes
            ValidarExistenciaCodigo(ClienteVA.CODCLI);
            //ValidarExistenciaCNPJ(ClienteVA.CGC);

            // Digito Verificador
            if (!UtilSIL.ValidarCnpjCpf(ClienteVA.CGC))
                throw new Exception("CNPJ ou CPF invalido. Favor verificar.");

            ValidarFaixaValor("Validade Cartao (Meses)", Convert.ToInt16(ClienteVA.PRZVALCART), 1, 60);
            ValidarFaixaValor("Prazo Pagamento", Convert.ToInt16(ClienteVA.PRAPAG_VA), 1, 60);
            ValidarFaixaValor("No. Parcelas Anu./Mens.", Convert.ToInt16(ClienteVA.NUMPAC), 0, 12);
            AplicarOutrasValidadoes(ClienteVA);

            
            //Persistir
            try
            {
                daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);
                retorno = ClienteDAL.Inserir(ClienteVA);
                var juncaoAtiva = new daCLIENTEVA(FOperador).JuncaoAtiva();
                if (retorno && juncaoAtiva)
                    new daCLIENTENovo(FOperador).AtualizaClientePreJuncao(ClienteVA.CODCLI);

                return retorno;
            }
             catch (Exception err)
            {
                throw new Exception(err.Message);
            }

        }

        #endregion

        //#region Alteracao

        //public bool Alterar(CLIENTEVA ClienteVA, IEnumerable<SegmentoView> segmentos, IEnumerable<GrupoCredenciadoView> gruposCredenciados)
        public bool Alterar(CLIENTEVA ClienteVA, IEnumerable<Segmento> segmentos, IEnumerable<GrupoCredenciado> gruposCredenciados)
        {
            var retorno = false;
            var ClienteVAAux = GetCliente(ClienteVA.CODCLI);
            
            if ((ClienteVAAux != null) && (ClienteVA.STA != ClienteVAAux.STA))
            {
                ClienteVA.DATSTA = DateTime.Now;
            }

            if (!UtilSIL.ValidarCnpjCpf(ClienteVA.NOVOCGC))
            {
                throw new Exception("CNPJ ou CPF invalido. Favor verificar.");
            }

            ValidarFaixaValor("Validade Cartao (Meses)", Convert.ToInt16(ClienteVA.PRZVALCART), 1, 60);
            ValidarFaixaValor("Prazo Pagamento", Convert.ToInt16(ClienteVA.PRAPAG_VA), 1, 60);
            ValidarFaixaValor("No. Parcelas Anu./Mens.", Convert.ToInt16(ClienteVA.NUMPAC), 0, 12);
            AplicarOutrasValidadoes(ClienteVA);
            var CLIENTEDAL = new daCLIENTEVA(FOperador);

            // Somente os segmentos modificados, mas todos os ramos serão atualizados.
            //segmentos = segmentos.Where(s => s.FoiModificado);

            // Somente os grupos modificados.
            //gruposCredenciados = gruposCredenciados.Where(s => s.FoiModificado);
            //retorno = CLIENTEDAL.Alterar(ClienteVA, segmentos.Select(sv => sv.Segmento), gruposCredenciados.Select(g => g.GrupoCredenciado));
            retorno = CLIENTEDAL.Alterar(ClienteVA, segmentos, gruposCredenciados);
            var juncaoAtiva = new daCLIENTEVA(FOperador).JuncaoAtiva();
            if (retorno && juncaoAtiva)
                new daCLIENTENovo(FOperador).AtualizaClientePreJuncao(ClienteVA.CODCLI);

            return retorno;
        }

        //#endregion

        #region Exclusao

        public void ValidarExclusao(CLIENTEVA ClienteVA)
        {
            daCLIENTEVA CLIENTEDAL = new daCLIENTEVA(FOperador);
            if (!CLIENTEDAL.ValidarExclusao(ClienteVA))
            {
                throw new Exception("Exclusao nao pode ser efetuada.\n" +
                    "Para ser excluido o cliente devera estar com status igual a Cancelado ha pelo menos 7 dias.\nFavor verificar.");
            }
        }

        public bool Excluir(CLIENTEVA ClienteVA)
        {
            //Persistir
            try
            {
                daCLIENTEVA CLIENTEDAL = new daCLIENTEVA(FOperador);
                return CLIENTEDAL.Excluir(ClienteVA);

            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public bool ExcluirSegmento(int ClienteVA)
        {
            daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.ExcluirSegAutorizado(ClienteVA);
        }

        public bool ExcluirGrupo(int ClienteVA)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.ExcluirGrupoAutorizado(ClienteVA);
        }

        #endregion

        //public IEnumerable<SegmentoView> GetSegmentosAutorizados(int codigoSistema, int codigoCliente)
        public IEnumerable<Segmento> GetSegmentosAutorizados(int codigoSistema, int codigoCliente)
        {
            return new daSegmentosEGrupos(FOperador).ObterSegmentos(codigoSistema, codigoCliente);
        }

        //public IEnumerable<GrupoCredenciadoView> GetGruposCredenciados(int codigoSistema, int codigoCliente)
        public IEnumerable<GrupoCredenciado> GetGruposCredenciados(int codigoSistema, int codigoCliente)
        {
            return new daSegmentosEGrupos(FOperador).ObterGrupos(codigoSistema, codigoCliente);
        }

        //#region Selecao

        //public List<GRUPOSAUTORIZVA_CLIENTE> GruposAutorizados(int IdClienteVA)
        //{
        //    var ClienteDAL = new daCLIENTEVA(FOperador);
        //    return ClienteDAL.GruposAutorizados(IdClienteVA);
        //}

        //public List<GRUPOSAUTORIZVA_CLIENTE> GruposDisponiveis(int IdClienteVA)
        //{
        //    var ClienteDAL = new daCLIENTEVA(FOperador);
        //    return ClienteDAL.GruposDisponiveis(IdClienteVA);
        //}

        //#endregion

        //#region Inclusao

        //public bool InserirGrupo(int ClienteVA, int Grupo)
        //{
        //    daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);
        //    return ClienteDAL.InserirGrupoAutorizado(ClienteVA, Grupo);
        //}

        //#endregion

        //#region Inclusao Todos

        //public bool InserirGrupo(int ClienteVA)
        //{
        //    daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);
        //    return ClienteDAL.InserirGrupoAutorizado(ClienteVA);
        //}

        //#endregion

        //#region Exclusao

        //public bool ExcluirGrupo(int ClienteVA, int Grupo)
        //{
        //    var ClienteDAL = new daCLIENTEVA(FOperador);
        //    return ClienteDAL.ExcluirGrupoAutorizado(ClienteVA, Grupo);
        //}

        //#endregion


        #region Observacoes

        #region Selecao

        public List<CLIENTEVA_OBS> Observacoes(Int32 IdClienteVA)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.Observacoes(IdClienteVA);
        }

        #endregion

        #region Inclusao

        public bool InserirObs(int CODCLI, DateTime DATA, string OBS, Int32 ID)
        {
            daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.InserirObs(CODCLI, DATA, OBS, ID);
        }

        #endregion

        #region Alteracao

        public bool AlterarObs(Int32 ID, string OBS)
        {
            daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.AlterarObs(ID, OBS);
        }

        #endregion

        #region Exclusao

        public bool ExcluirObs(Int32 ID)
        {
            daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.ExcluirObs(ID);
        }

        #endregion

        #endregion

        #region Proximo Codigo Livre

        public int ProximoCodigoLivre()
        {
            daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.ProximoCodigoLivre();
        }

        #endregion

        #region Cartoes Seg. Via

        public List<CARTOES_SEGVIA> CartoesSegVia(DateTime DataInicial, DateTime DataFinal, Int32 CodIncial, Int32 CodFinal)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.CartoesSegundaVia(DataInicial, DataFinal, CodIncial, CodFinal);
        }

        #endregion

        #region Cartoes Bloqueio

        public List<CARTOES_BLOQUEIO> CartoesBloqueio(DateTime DataInicial, DateTime DataFinal, Int32 CodIncial, Int32 CodFinal)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.CartoesBloqueio(DataInicial, DataFinal, CodIncial, CodFinal);
        }

        #endregion

        #region Cartoes Cancelamento

        public List<CARTOES_CANCELAMENTO> CartoesCancelamento(DateTime DataInicial, DateTime DataFinal, Int32 CodIncial, Int32 CodFinal)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.CartoesCancelamento(DataInicial, DataFinal, CodIncial, CodFinal);
        }

        #endregion

        #region Cartoes Inclusao

        public List<CARTOES_INCLUSAO> CartoesInclusao(DateTime DataInicial, DateTime DataFinal, Int32 CodIncial, Int32 CodFinal)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.CartoesInclusao(DataInicial, DataFinal, CodIncial, CodFinal);
        }

        #endregion

        #region Usuarios

        public List<USUARIO_VA> UsuariosVA(short Classificacao, int CodCliInicial, int CodCliFinal,
            DateTime DataInicial, DateTime DataFinal, string ParametroInicial, string ParametroFinal)
        {
            daUsuarioVA UsuarioVADAL = new daUsuarioVA(FOperador);
            return UsuarioVADAL.UsuarioVA(Classificacao, CodCliInicial, CodCliFinal,
               DataInicial, DataFinal, ParametroInicial, ParametroFinal);
        }

        #endregion

        #region Validacoes

        public void ValidarExistenciaCodigo(int CodCliente)
        {
            daCLIENTEVA CLIENTEDAL = new daCLIENTEVA(FOperador);
            if (CLIENTEDAL.CodigoExistente(CodCliente))
                throw new Exception("Codigo ja cadastrado.");
        }

        void ValidarFaixaValor(string Label, int Valor, int Minimo, int Maximo)
        {
            if ((Valor < Minimo) || (Valor > Maximo))
                throw new Exception(string.Format("Favor informar {0} entre {1} e {2}.", Label, Minimo, Maximo));
        }

        void AplicarOutrasValidadoes(CLIENTEVA ClienteVA)
        {
            if ((ClienteVA.COB2AV) && (ClienteVA.VAL2AV == 0))
                throw new Exception("Favor informar Valor 2a. via Cartao.");

            if ((ClienteVA.COBCONS == ConstantesSIL.FlgSim) && (ClienteVA.VALCONS == 0))
                throw new Exception("Favor informar Valor Consulta Saldo URA.");
        }

        #endregion

        #region GeracaoArqCartoes        

        public string GeracaoArqCartoes(string PathUpload, string PathAbsolutoArquivo, byte Tipo, Int32 Cliente, string Data)
        {
            daCLIENTEVA CLIENTEDAL = new daCLIENTEVA(FOperador);
            List<string> LinhasArqCartoes;
            StringBuilder LinhaDados;

            // Rertorna Colecao Cartoes
            LinhasArqCartoes = CLIENTEDAL.ColecaoArqCartoes(Tipo, Cliente, Data);

            #region Geracao Arquivo
            /* Geracao Arquivos */
            // Arq. LST

            string ArquivoExportacao;
            string NomeArqSeq;
            FileStream ArqDados;
            StreamWriter DadosArquivo;
            string ArquivosGerados;

            ArquivosGerados = string.Empty;

            NomeArqSeq = string.Format("LST{0}.TXT", (Cliente.ToString().PadLeft(5, '0')));
            ArquivoExportacao = string.Format(@"{0}\{1}", PathAbsolutoArquivo, NomeArqSeq);
            ArqDados = new FileStream(ArquivoExportacao, FileMode.Create);
            DadosArquivo = new StreamWriter(ArqDados);

            if (LinhasArqCartoes.Count > 0)
            {
                // Lista Arquivos Gerados
                ArquivosGerados = string.Format(@"{0}/{1}", PathUpload, NomeArqSeq);
                LinhaDados = new StringBuilder();

                for (int i = 0; (i < LinhasArqCartoes.Count); i++)
                {
                    LinhaDados.AppendLine(LinhasArqCartoes[i].Split('|')[0]);
                }
                // Persistir Arquivo
                DadosArquivo.Write(LinhaDados.ToString());
                DadosArquivo.Close();

                // Arq. CRT
                NomeArqSeq = string.Format("CRT{0}.TXT", (Cliente.ToString().PadLeft(5, '0')));
                ArquivoExportacao = string.Format(@"{0}\{1}", PathAbsolutoArquivo, NomeArqSeq);

                ArqDados = new FileStream(ArquivoExportacao, FileMode.Create);
                DadosArquivo = new StreamWriter(ArqDados);

                LinhaDados = new StringBuilder();
                for (int i = 0; (i < LinhasArqCartoes.Count); i++)
                {
                    LinhaDados.AppendLine(LinhasArqCartoes[i].Split('|')[1]);
                }
                // Persistir Arquivo
                DadosArquivo.Write(LinhaDados.ToString());
                DadosArquivo.Close();

                // Lista Arquivos Gerados
                ArquivosGerados += string.Format(@"|{0}/{1}", PathUpload, NomeArqSeq);

            }
            return ArquivosGerados;
            #endregion

        }

        public string ConfigJobs()
        {
            return new daCLIENTEVA(FOperador).ConfigJobs();        
        }

        public string NomProcEmbosso(int codProd)
        {
            return new daCLIENTEVA(FOperador).NomProcEmbosso(codProd);
        }

        public string GeracaoArqCartoes(byte Tipo, Int32 Cliente, string Data, string path, string nomProcEmbosso)
        {
            daCLIENTEVA CLIENTEDAL = new daCLIENTEVA(FOperador);
            string ArquivosGerados;
            ArquivosGerados = string.Empty;
            ArquivosGerados = CLIENTEDAL.ArqCartoesEmbosso(Tipo, Cliente, Data, path, nomProcEmbosso);
            return ArquivosGerados;
        }

        #endregion

        #region Operadores WEB

        public string ValidadeSenha(int id)
        {
            var daCliente = new daCLIENTEVA(FOperador);
            return daCliente.ValidadeSenha(id);
        }

        #region Selecao

        public List<ACESSOOPERADORMW> OperadoresWEB(int IdClienteVA)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.OperadoresWEB(IdClienteVA, string.Empty);
        }

        public List<ACESSOOPERADORMW> OperadoresWEBParceria()
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.OperadoresWEBParceria(string.Empty);
        }

        public List<CLIENTEAGRUPAMENTO> OperadoresWEBParceriaAgrupamentoCliente(int idOperador)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.OperadoresWEBParceriaAgrupamentoCliente(idOperador);
        }

        public List<string> OperadoresWEBParceriaClientes(int codParceria)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.OperadoresWEBParceriaClientes(codParceria);
        }

        public List<ACESSOOPERADORMW> OperadoresWEBParceria(string filtro)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.OperadoresWEBParceria(filtro);
        }

        public ACESSOOPERADORMW OperadorWEB(int IdClienteVA, int ID)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.OperadorWEB(IdClienteVA, ID);
        }

        public ACESSOOPERADORMW OperadorWEBParceria(int ID)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.OperadorWEBParceria(ID);
        }

        #endregion

        #region Inclusao

        public bool InserirOperadorWEB(ACESSOOPERADORMW Operador, out string retorno)
        {
            if (Operador.NOME == string.Empty)
                throw new Exception("Favor informar nome do operador.");
            if (Operador.LOGIN == string.Empty)
                throw new Exception("Favor informar login do operador.");
            
            daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);            
            return ClienteDAL.InserirAcessoOperadorMW(Operador, out retorno);
        }

        public string CripSenha(string senha)
        {            
            string novaSenha = string.Empty;
            string senhaNormalizada = senha.PadRight(8, '0');
            novaSenha = BlCriptografia.Encrypt(senhaNormalizada);            
            return novaSenha;
        } 

        #endregion

        #region Alteracao

        public bool AlterarOperadorWEB(ACESSOOPERADORMW Operador, out string retorno)
        {
            daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.InserirAcessoOperadorMW(Operador, out retorno);
            //return ClienteDAL.AlterarOperadorWEB(Operador);
        }

        public bool RenovarAcessoOperadorWEB(ACESSOOPERADORMW Operador)
        {
            //Operador.SENHA = BlCriptografia.Encrypt(Operador.LOGIN.PadRight(8, '0'));

            BlCriptografia blCrip = new BlCriptografia(FOperador);
            daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);

            Operador.SENHA = blCrip.Criptografar(Operador.LOGIN);

            return ClienteDAL.RenovarAcessoOperadorWEB(Operador, Operador.CODCLI);
        }

        #endregion

        #region Exclusao

        public bool ExcluirOperadorWEB(ACESSOOPERADORMW Operador)
        {
            daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.ExcluirOperadorWEB(Operador);
        }

        #endregion

        #endregion

        #region Acesso WEB

        public List<LOGWEB_VA> AcessoWEB(int CodInicial, int CodFinal, DateTime DataInicial, DateTime DataFinal)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.AcessoWEB(CodInicial, CodFinal, DataInicial, DataFinal);
        }

       #endregion

        #region Get

        public CLIENTEVA GetCliente(int codCli)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.GetCliente(codCli);
        }

        public List<CLIENTEVA> ListaClientes()
        {
            daCLIENTEVA ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.ListaClientes();
        }

        public List<CLIENTEVA_PREPAGO> ColecaoClientesVA(IFilter filter)
        {
            var filtro = string.Empty;
            if (filter != null)
                filtro = filter.FilterString;

            var clienteDAL = new daCLIENTEVA(FOperador);
            return clienteDAL.ColecaoClientesVA(filtro);
        }

        public List<CLIENTEVA_PREPAGO> ColecaoClientesVA(IFilter filter, int codAg)
        {
            var filtro = string.Empty;
            filtro = "CODAG = '" + codAg + "' AND ";

            if (filter != null)
                filtro += filter.FilterString;
            else
                filtro = filtro.Replace(" AND", "");

            var clienteDAL = new daCLIENTEVA(FOperador);
            return clienteDAL.ColecaoClientesVA(filtro);
        }

        public bool ExibeSubRede()
        {
            var clienteDAL = new daCLIENTEVA(FOperador);
            return clienteDAL.ExibeSubRede();
        }

        public bool Embosso()
        {
            var clienteDAL = new daCLIENTEVA(FOperador);
            return clienteDAL.Embosso();
        }

        public bool ExibeTransfSaldoCli()
        {
            var clienteDAL = new daCLIENTEVA(FOperador);
            return clienteDAL.ExibeTransfSaldoCli();
        }

        public int GetCodSubRede(IFilter filter)
        {
            var filtro = string.Empty;
            if (filter != null)
                filtro = filter.FilterString;

            var clienteDAL = new daCLIENTEVA(FOperador);
            return clienteDAL.GetCodSubRede(filtro);
        }

        public List<VENDEDOR> ListaVendedores()
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.ListaVendedores();
        }

        public string GetStatus(int codCli)
        {
            var ClienteDAL = new daCLIENTEVA(FOperador);
            return ClienteDAL.GetStatus(codCli);
        }

        public decimal ConsultaCargaPadCli(int codCli)
        {
            return new daCLIENTEVA(FOperador).ConsultaCargaPadCli(codCli);
        }

        #endregion

        #region Taxas Associadas Cliente

        public bool ExibeModuloTaxaCli()
        {
            var cliDal = new daCLIENTEVA(FOperador);
            return cliDal.ExibeModuloTaxaCli();
        }

        public List<MODTAXA> ConsultaTaxaCli(Int32 codCli)
        {
            var blCliente = new daCLIENTEVA(FOperador);
            return blCliente.ConsultaTaxaCli(codCli);
        }

        public MODTAXA ConsultaCodTaxaCli(Int32 codCli, Int32 codTaxa)
        {
            var dalCliente = new daCLIENTEVA(FOperador);
            return dalCliente.ConsultaCodTaxaCli(codCli, codTaxa);
        }

        public string ConsultaTaxa(Int32 codCli, Int32 codTaxa)
        {
            var dalCliente = new daCLIENTEVA(FOperador);
            return dalCliente.ConsultaTaxa(codTaxa);
        }

        public bool SalvarTaxaCli(MODTAXA taxacli)
        {
            var dalCliente = new daCLIENTEVA(FOperador);
            return dalCliente.SalvarTaxaCli(taxacli);
        }

        public bool ExcluirTaxaCli(MODTAXA taxacli)
        {
            try
            {
                var dalCliente = new daCLIENTEVA(FOperador);
                return dalCliente.ExcluirTaxaCli(taxacli);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public void ValidarExclusaoTaxaCli(MODTAXA taxacli)
        {
            var dalCliente = new daCLIENTEVA(FOperador);
            var trasacao = dalCliente.ValidarExclusaoTaxaCli(taxacli);
            if (trasacao > 0)
            {
                throw new Exception("Exclusao nao pode ser efetuada.\n" +
                                    "Existem transacoes vinculadas a essa taxa.");
            }
        }

        #endregion

        #region Beneficio

        public List<BENEFCLI> GetBeneficiosCliente(string CODCLI)
        {
            daCLIENTEVA da = new daCLIENTEVA(FOperador);

            return da.GetBeneficiosCliente(CODCLI);
        }

        #endregion
    }
}