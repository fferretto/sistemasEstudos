using SIL.BLL;
using SIL.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using TELENET.SIL.DA;
using TELENET.SIL.PO;

namespace TELENET.SIL.BL
{
    public class blCLIENTENovo
    {
        OPERADORA FOperador;
        public blCLIENTENovo(OPERADORA Operador)
        {
            FOperador = Operador;
        }

        #region Inclusao

        public int Incluir(CADCLIENTE cadCliente, out string mensagem)
        {
            if (!UtilSIL.ValidarCnpjCpf(cadCliente.CNPJ))
            {
                mensagem = "CNPJ ou CPF invalido. Favor verificar.";
                return 0;
            }
            try
            {
                daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
                return ClienteDAL.Inserir(cadCliente, out mensagem);
            }
            finally { }
        }

        public bool IncluirInfControlePos(VPRODUTOSCLI clienteProd, out string mensagem)
        {
            try
            {
                daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
                return ClienteDAL.InserirInfControlePos(clienteProd, out mensagem);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

        }

        public bool IncluirInfControlePre(VPRODUTOSCLI clienteProd, out string mensagem)
        {
            try
            {
                daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
                return ClienteDAL.InserirInfControlePre(clienteProd, out mensagem);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

        }

        #endregion

        #region Alteracao

        public bool Alterar(CADCLIENTE cadCliente, out string mensagem)
        {
            if (!UtilSIL.ValidarCnpjCpf(cadCliente.NOVOCNPJ))
            {
                mensagem = "CNPJ ou CPF invalido. Favor verificar.";
                return false;
            }
            try
            {
                var CLIENTEDAL = new daCLIENTENovo(FOperador);
                return CLIENTEDAL.Alterar(cadCliente, out mensagem);
            }
            finally { }
        }

        //public void AlterarSegmentosAutorizados(int codigoCliente, int sistema, IEnumerable<SegmentoView> segmentos, out string mensagem)
        public void AlterarSegmentosAutorizados(int codigoCliente, int sistema, IEnumerable<Segmento> segmentos, out string mensagem)
        {
            new daSegmentosEGrupos(FOperador).AlterarSegmentos(codigoCliente, sistema, segmentos, out mensagem);
        }

        //public void AlterarGruposCredenciados(int codigoCliente, int sistema, IEnumerable<GrupoCredenciadoView> gruposCredenciados, out string mensagem)
        public void AlterarGruposCredenciados(int codigoCliente, int sistema, IEnumerable<GrupoCredenciado> gruposCredenciados, out string mensagem)
        {
            new daSegmentosEGrupos(FOperador).AlterarGrupos(codigoCliente, sistema, gruposCredenciados, out mensagem);
        }

        public bool AlterarInfControlePos(VPRODUTOSCLI clienteProd, out string mensagem)
        {
            var CLIENTEDAL = new daCLIENTENovo(FOperador);
            return CLIENTEDAL.AlterarInfControlePos(clienteProd, out mensagem);
        }

        public bool AlterarInfControlePre(VPRODUTOSCLI clienteProd, out string mensagem)
        {
            var CLIENTEDAL = new daCLIENTENovo(FOperador);
            if (clienteProd.STACOD == ConstantesSIL.StatusEmRescisao)
            {
                clienteProd.DATRESCISAO = clienteProd.DATULTCARG_VA.AddDays(clienteProd.DIASVALSALDO);
            }
            return CLIENTEDAL.AlterarInfControlePre(clienteProd, out mensagem);
        }

        public bool AlterarStatus(int codCli, int sistema, out string mensagem)
        {
            var CLIENTEDAL = new daCLIENTENovo(FOperador);
            return CLIENTEDAL.AlterarStatus(codCli, sistema, out mensagem);
        }

        #endregion

        #region Exclusao        

        public bool ExcluirCadCliente(CADCLIENTE cadCliente, out string retorno)
        {
            //Persistir
            try
            {
                daCLIENTENovo CLIENTEDAL = new daCLIENTENovo(FOperador);
                return CLIENTEDAL.ExcluirCadCliente(cadCliente, out retorno);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public bool ExcluirClientePos(int codCli, out string retorno)
        {
            //Persistir
            try
            {
                daCLIENTENovo CLIENTEDAL = new daCLIENTENovo(FOperador);
                return CLIENTEDAL.ExcluirClientePos(codCli, out retorno);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public bool ExcluirClientePre(int codCli, out string retorno)
        {
            //Persistir
            try
            {
                daCLIENTENovo CLIENTEDAL = new daCLIENTENovo(FOperador);
                return CLIENTEDAL.ExcluirClientePre(codCli, out retorno);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        #endregion

        #region Inclusao

        public string ManuSegGrupo(SEG_GRUPO_DISPAUTORIZ seg)
        {
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.ManuSegGrupo(seg);
        }

        #endregion

        #region Selecao

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

        public List<SEG_GRUPO_DISPAUTORIZ> ListaSegGrupo(int sistema, int codCli, string tipo, string classif)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.ListaSegGrupo(sistema, codCli, tipo, classif);
        }

        public List<GRUPOSAUTORIZVA_CLIENTE> GruposAutorizados(int IdCliente)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.GruposAutorizados(IdCliente);
        }

        public List<GRUPOSAUTORIZVA_CLIENTE> GruposDisponiveis(int IdCliente)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.GruposDisponiveis(IdCliente);
        }

        #endregion

        #region Inclusao

        public bool InserirGrupo(int ClienteVA, int Grupo)
        {
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.InserirGrupoAutorizado(ClienteVA, Grupo);
        }

        #endregion

        #region Inclusao Todos

        public bool InserirGrupo(int ClienteVA)
        {
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.InserirGrupoAutorizado(ClienteVA);
        }

        #endregion

        #region Exclusao

        public bool ExcluirSegmento(int ClienteVA)
        {
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.ExcluirSegAutorizado(ClienteVA);
        }

        public bool ExcluirGrupo(int ClienteVA, int Grupo)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.ExcluirGrupoAutorizado(ClienteVA, Grupo);
        }

        #endregion

        #region Exclusao Todos

        public bool ExcluirGrupo(int ClienteVA)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.ExcluirGrupoAutorizado(ClienteVA);
        }

        #endregion

        #region Observacoes

        #region Selecao

        public List<CLIENTE_OBS> Observacoes(int sistema, int CodCli)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.Observacoes(sistema, CodCli);
        }

        #endregion

        #region Inclusao

        public bool InserirObs(int sistema, int codCli, DateTime data, string obs)
        {
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.InserirObs(sistema, codCli, data, obs);
        }

        #endregion

        #region Exclusao

        public bool ExcluirObs(int sistema, int codCli, DateTime data, string COMPOSITEKEY)
        {
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
            var par = COMPOSITEKEY.Split(',');
            data = Convert.ToDateTime(par[1]);
            return ClienteDAL.ExcluirObs(sistema, codCli, data);
        }

        public bool ExcluirObs(int sistema, int codCli, DateTime data)
        {
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.ExcluirObs(sistema, codCli, data);
        }

        #endregion

        #endregion

        #region ObservacoesVa

        #region SelecaoVa

        public List<CLIENTEVA_OBS> ObservacoesVa(Int32 CodCli)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.ObservacoesVa(CodCli);
        }

        #endregion

        #region InclusaoVa

        public bool InserirObsVa(int CODCLI, DateTime DATA, string OBS)
        {
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.InserirObsVa(CODCLI, DATA, OBS);
        }

        #endregion

        #region ExclusaoVa

        public bool ExcluirObsVa(int CODCLI, DateTime DATA, string COMPOSITEKEY)
        {
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
            var par = COMPOSITEKEY.Split(',');
            DATA = Convert.ToDateTime(par[1]);
            return ClienteDAL.ExcluirObsVa(CODCLI, DATA);
        }

        #endregion

        #endregion

        #region Proximo Codigo Livre

        public int ProximoCodigoLivre(int sistema)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.ProximoCodigoLivre(sistema);
        }

        #endregion

        #region Cartoes Seg. Via

        public List<CARTOES_SEGVIA> CartoesSegVia(DateTime DataInicial, DateTime DataFinal, Int32 CodIncial, Int32 CodFinal)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.CartoesSegundaVia(DataInicial, DataFinal, CodIncial, CodFinal);
        }

        #endregion

        #region Cartoes Bloqueio

        public List<CARTOES_BLOQUEIO> CartoesBloqueio(DateTime DataInicial, DateTime DataFinal, Int32 CodIncial, Int32 CodFinal)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.CartoesBloqueio(DataInicial, DataFinal, CodIncial, CodFinal);
        }

        #endregion

        #region Cartoes Cancelamento

        public List<CARTOES_CANCELAMENTO> CartoesCancelamento(DateTime DataInicial, DateTime DataFinal, Int32 CodIncial, Int32 CodFinal)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.CartoesCancelamento(DataInicial, DataFinal, CodIncial, CodFinal);
        }

        #endregion

        #region Cartoes Inclusao

        public List<CARTOES_INCLUSAO> CartoesInclusao(DateTime DataInicial, DateTime DataFinal, Int32 CodIncial, Int32 CodFinal)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
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
            daCLIENTENovo CLIENTEDAL = new daCLIENTENovo(FOperador);
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

        public string GeracaoArqCartoes(string PathUpload, string PathAbsolutoArquivo, byte Tipo, Int32 Cliente, string Data, int sistema)
        {
            daCLIENTENovo CLIENTEDAL = new daCLIENTENovo(FOperador);
            List<string> LinhasArqCartoes;
            StringBuilder LinhaDados;

            // Rertorna Colecao Cartoes
            LinhasArqCartoes = CLIENTEDAL.ColecaoArqCartoes(Tipo, Cliente, Data, sistema);

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
            return new daCLIENTENovo(FOperador).ConfigJobs();
        }

        public string NomProcEmbosso(int codProd)
        {
            return new daCLIENTENovo(FOperador).NomProcEmbosso(codProd);
        }

        public string GeracaoArqCartoes(byte Tipo, Int32 Cliente, int sistema, string Data, string path, string nomProcEmbosso)
        {
            daCLIENTENovo CLIENTEDAL = new daCLIENTENovo(FOperador);
            string ArquivosGerados;
            ArquivosGerados = string.Empty;
            ArquivosGerados = CLIENTEDAL.ArqCartoesEmbosso(Tipo, Cliente, sistema, Data, path, nomProcEmbosso);
            return ArquivosGerados;
        }

        #endregion

        #region Enviar Carta Boas Vindas

        public string EnviarCarta(int sistema, int codCli)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.EnviarCarta(sistema, codCli);
        }

        #endregion

        #region Operadores WEB

        public string ValidadeSenha(int id)
        {
            var daCliente = new daCLIENTENovo(FOperador);
            return daCliente.ValidadeSenha(id);
        }

        #region Selecao

        public List<ACESSOOPERADORMW> OperadoresWEBPos(int CodCli)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.OperadoresWEBPos(CodCli, string.Empty);
        }

        public List<ACESSOOPERADORMW> OperadoresWEBPre(int CodCli)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.OperadoresWEBPre(CodCli, string.Empty);
        }

        public List<OPERADORMW> OperadoresWEBParceria()
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.OperadoresWEBParceria(string.Empty);
        }

        public List<ACESSOOPERADORMW> OperadoresWEBParceria(string filtro)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.AcessoWEBParceria(filtro);
        }

        public ACESSOOPERADORMW OperadorWEB(int IdCliente, int ID)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.OperadorWEB(IdCliente, ID);
        }

        public ACESSOOPERADORMW[] OperadorWEBParceria(int ID)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            var operadorWebPosPre = new ACESSOOPERADORMW[2];
            var oper = ClienteDAL.OperadorWEBParceria(ID);
            var operPos = oper.Where(o => o.SISTEMA == ConstantesSIL.SistemaPOS).FirstOrDefault();
            var operPre = oper.Where(o => o.SISTEMA == ConstantesSIL.SistemaPRE).FirstOrDefault();
            operadorWebPosPre[0] = operPos;
            operadorWebPosPre[1] = operPre;
            return operadorWebPosPre;
        }

        #endregion

        #region Inclusao

        public bool InserirOperadorWEB(ACESSOOPERADORMW Operador, out string retorno)
        {
            if (Operador.NOME == string.Empty)
            {
                retorno = "Favor informar nome do operador.";
                return false;
            }
            if (Operador.LOGIN == string.Empty)
            {
                retorno = "Favor informar login do operador.";
                return false;
            }
            else
            {
                Operador.NOME = UtilSIL.RemoverAcentos(Operador.NOME).ToUpper();
                Operador.LOGIN = UtilSIL.RemoverAcentos(Operador.LOGIN).ToUpper();
            }

            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
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
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.InserirAcessoOperadorMW(Operador, out retorno);
            //return ClienteDAL.AlterarOperadorWEB(Operador);
        }

        public bool RenovarAcessoOperadorWEB(ACESSOOPERADORMW Operador)
        {
            //Operador.SENHA = BlCriptografia.Encrypt(Operador.LOGIN.PadRight(8, '0'));

            BlCriptografia blCrip = new BlCriptografia(FOperador);
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);

            Operador.SENHA = blCrip.Criptografar(Operador.LOGIN);

            return ClienteDAL.RenovarAcessoOperadorWEB(Operador);
        }

        #endregion

        #region Exclusao

        public bool ExcluirOperadorWEB(ACESSOOPERADORMW Operador)
        {
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.ExcluirOperadorWEB(Operador);
        }

        #endregion

        #endregion

        #region Acesso WEB

        public List<LOGWEB_VA> AcessoWEB(int CodInicial, int CodFinal, DateTime DataInicial, DateTime DataFinal)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.AcessoWEB(CodInicial, CodFinal, DataInicial, DataFinal);
        }

        #endregion

        #region Get

        public CADCLIENTE GetCliente(int idCliente)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.GetCliente(idCliente);
        }

        public List<VPRODUTOSCLI> GetClienteProd(int idCliente)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.GetClienteProd(idCliente);
        }

        public decimal GetValGasto(VPRODUTOSCLI cliente)
        {
            var daCLIENTENovo = new daCLIENTENovo(FOperador);
            return daCLIENTENovo.GetValGasto(cliente);
        }

        public CARTAOTEMPORARIO GetCartaoTemporario(VPRODUTOSCLI cliente, out string retorno)
        {
            var daCLIENTENovo = new daCLIENTENovo(FOperador);
            return daCLIENTENovo.GetCartaoTemporario(cliente, out retorno);
        }

        public List<CLIENTEVA> ListaClientes()
        {
            daCLIENTENovo ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.ListaClientes();
        }

        public List<VRESUMOCLI> ColecaoClientes(IFilter filter)
        {
            var filtro = string.Empty;
            if (filter != null)
                filtro = filter.FilterString;

            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.ColecaoClientes(filtro);
        }

        public List<VRESUMOCLI> ColecaoClientes(IFilter filter, int codAg)
        {
            var filtro = string.Empty;
            filtro = "CODAG = '" + codAg + "' AND ";

            if (filter != null)
                filtro += filter.FilterString;
            else
                filtro = filtro.Replace(" AND", "");

            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.ColecaoClientes(filtro);
        }

        public bool ExibeCobrancaIndividual()
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.ExibeCobrancaIndividual();
        }

        public bool ExibeSensibilizaSaldo()
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.ExibeSensibilizaSaldo();
        }

        public bool ExibeExigeReceita()
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.ExibeExigeReceita();
        }
        public bool ExibeExibeMensagem()
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.ExibeExibeMensagem();
        }

        public bool ExibeSubRede()
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.ExibeSubRede();
        }

        public bool Embosso()
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.Embosso();
        }

        public int GetValorTamanhoNomGraCli()
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.GetValorTamanhoNomGraCli();
        }

        public CONSULTA GetPeriodoLoteCli(int codCli, int numFech)
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.GetPeriodoLoteCli(codCli, numFech);
        }

        public bool ECrediHabita(int tipoProd)
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.ECrediHabita(tipoProd);
        }

        public bool ContaDigitalHabilitada(int tipoProd)
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.ContaDigitalHabilitada(tipoProd);
        }

        public bool NegarCargaSaldo(int tipoProd)
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.NegarCargaSaldo(tipoProd);
        }

        public bool HabilitaEnvioCartas()
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.HabilitaEnvioCartas();
        }

        public bool UsaRamoAtividade(int sistema)
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.UsaRamoAtividade(sistema);
        }

        public bool HabilitaTaxaFaturamentoMinimo(int sistema)
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.HabilitaTaxaFaturamentoMinimo(sistema);
        }
        
        public int ObtemMascaraMaxCartTemp(int sistema)
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.ObtemMascaraMaxCartTemp(sistema);
        }

        public int ObtemMascaraValorMaxCargaCartaoProduto(int tipoProd)
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.ObtemMascaraValorMaxCargaCartaoProduto(tipoProd);
        }

        public bool ExibeTransfSaldoCli()
        {
            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.ExibeTransfSaldoCli();
        }

        public int GetCodSubRede(IFilter filter)
        {
            var filtro = string.Empty;
            if (filter != null)
                filtro = filter.FilterString;

            var clienteDAL = new daCLIENTENovo(FOperador);
            return clienteDAL.GetCodSubRede(filtro);
        }

        public List<VENDEDOR> ListaVendedores()
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.ListaVendedores();
        }

        public string GetStatus(int codCli)
        {
            var ClienteDAL = new daCLIENTENovo(FOperador);
            return ClienteDAL.GetStatus(codCli);
        }

        public decimal ConsultaCargaPadCli(int codCli)
        {
            return new daCLIENTENovo(FOperador).ConsultaCargaPadCli(codCli);
        }

        #endregion

        #region Taxas Associadas Cliente

        public bool ExibeModuloTaxaCli()
        {
            var cliDal = new daCLIENTENovo(FOperador);
            return cliDal.ExibeModuloTaxaCli();
        }

        public bool ExibeTaxaIndividual()
        {
            var cliDal = new daCLIENTENovo(FOperador);
            return cliDal.ExibeTaxaIndividual();
        }

        public List<MODTAXA> ConsultaTaxaCli(Int32 codCli, Int32 Sistema)
        {
            var blCliente = new daCLIENTENovo(FOperador);
            return blCliente.ConsultaTaxaCli(codCli, Sistema);
        }

        public MODTAXA ConsultaCodTaxaCli(Int32 codCli, Int32 codTaxa)
        {
            var dalCliente = new daCLIENTENovo(FOperador);
            return dalCliente.ConsultaCodTaxaCli(codCli, codTaxa);
        }

        public TAXACLIENTE ConsultaTaxaVA(Int32 codCli, Int32 codTaxa)
        {
            var dalCliente = new daCLIENTENovo(FOperador);
            return dalCliente.ConsultaTaxaVA(codTaxa);
        }

        public TAXACLIENTE ConsultaTaxaPJ(Int32 codCli, Int32 codTaxa)
        {
            var dalCliente = new daCLIENTENovo(FOperador);
            return dalCliente.ConsultaTaxaPJ(codTaxa);
        }

        public bool SalvarTaxaCli(MODTAXA taxacli)
        {
            var dalCliente = new daCLIENTENovo(FOperador);
            return dalCliente.SalvarTaxaCli(taxacli);
        }

        public bool ExcluirTaxaCli(MODTAXA taxacli)
        {
            try
            {
                var dalCliente = new daCLIENTENovo(FOperador);
                return dalCliente.ExcluirTaxaCli(taxacli);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        #endregion

        #region Beneficio

        public List<BENEFCLI> GetBeneficiosCliente(string CODCLI)
        {
            daCLIENTENovo da = new daCLIENTENovo(FOperador);

            return da.GetBeneficiosCliente(CODCLI);
        }

        #endregion

        #region Agendamento Cancelamento Cliente

        public List<AGENDCANCCLIENTE> GetAgenCancCliente()
        {
            daCLIENTENovo da = new daCLIENTENovo(FOperador);
            return da.GetAgenCancCliente();
        }

        public bool IncluirAgenCancCliente(AGENDCANCCLIENTE agendamento, out string retorno)
        {
            daCLIENTENovo da = new daCLIENTENovo(FOperador);
            return da.IncluirAgenCancCliente(agendamento, out retorno);
        }
        public bool ExcluirAgenCancCliente(int codAgend, int idFunc, out string retorno)
        {
            daCLIENTENovo da = new daCLIENTENovo(FOperador);
            return da.ExcluirAgenCancCliente(codAgend, idFunc, out retorno);
        }

        #endregion

        public List<RESUMOIMPORTACAO> ImportarUsuarios(string diretorio, string arquivo, string limiteImediato, string validaCpf, int codope, int codcli)
        {
            daCLIENTENovo da = new daCLIENTENovo(FOperador);
            return da.ImportarUsuarios(diretorio, arquivo, limiteImediato, validaCpf, codope, codcli);
        }

        public List<RESUMOIMPORTACAO> ImportarUsuariosTelenet(string diretorio, string arquivo, string limiteImediato, string validaCpf, int codope, int codcli)
        {
            daCLIENTENovo da = new daCLIENTENovo(FOperador);
            return da.ImportarUsuariosTelenet(diretorio, arquivo, limiteImediato, validaCpf, codope, codcli);
        }

        public DataTable ProcessoImportacao(string diretorio, string arquivo, int codope)
        {
            daCLIENTENovo da = new daCLIENTENovo(FOperador);
            return da.ProcessoImportacao(diretorio, arquivo, codope);
        }
    }
}