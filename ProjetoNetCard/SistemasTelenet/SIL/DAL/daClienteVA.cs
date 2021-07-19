using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SIL;
using SIL.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using TELENET.SIL.PO;


namespace TELENET.SIL.DA
{
    public class daCLIENTEVA
    {
        readonly string BDTELENET = string.Empty;
        readonly string BDAUTORIZADOR = string.Empty;
        readonly OPERADORA FOperador;
        public daCLIENTEVA(OPERADORA Operador)
        {
            FOperador = Operador;

            // Monta String Conecao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDAUTORIZADOR = string.Format(ConstantesSIL.BDAUTORIZADOR, Operador.SERVIDORAUT, Operador.BANCOAUT, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        #region Status

        private void AlteraStatus(Database db, int codigoCliente, string status)
        {
            var msgErro = "Erro na aleração do status do contrato";
            try
            {
                var cmd = db.GetStoredProcCommand("PROC_ALTERA_STATUS_CLIENTE");
                db.AddInParameter(cmd, "SISTEMA", DbType.Int16, 1);
                db.AddInParameter(cmd, "STA", DbType.String, status);
                db.AddInParameter(cmd, "CODCLI", DbType.Int32, codigoCliente);
                db.AddInParameter(cmd, "ID_FUNC", DbType.Int32, FOperador.ID_FUNC);
                DataTable table = new DataTable();
                table.Load(db.ExecuteReader(cmd));

                var msg = Convert.ToString(table.Rows[0][1]);
                if (msg != "SUCESSO")
                {
                    msgErro = msg;
                    throw new Exception(msg);
                }
            }
            catch (Exception)
            {
                throw new Exception(msgErro);
            }
        }

        #endregion

        #region GET Cliente

        public CLIENTEVA GetCliente(int codCli)
        {
            var cliente = new CLIENTEVA();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT * ");
            sql.AppendLine("FROM CLIENTEVA ");
            sql.AppendLine("WHERE CODCLI = @CODCLI ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                cliente.CODCLI = codCli;

                cliente.CODBAI = Convert.ToInt32(idr["CODBAI"]);
                cliente.CODSUBREDE = Convert.ToInt32(idr["CODSUBREDE"]);
                cliente.CODPARCERIA = Convert.ToInt32(idr["CODPARCERIA"]);
                cliente.CODPROD = Convert.ToInt32(idr["CODPROD"]);
                cliente.CODAG = idr["CODAG"] == DBNull.Value ? 0 : Convert.ToInt32(idr["CODAG"]);
                cliente.CODBAIEDC = Convert.ToInt32(idr["CODBAIEDC"]);
                cliente.CODLOCEDC = Convert.ToInt32(idr["CODLOCEDC"]);
                cliente.SIGUF0EDC = idr["SIGUF0EDC"].ToString();
                cliente.CEPEDC = idr["CEPEDC"].ToString();
                cliente.RESEDC = idr["RESEDC"].ToString();
                cliente.TELEDC = idr["TELEDC"].ToString();
                cliente.CODREG = Convert.ToInt32(idr["CODREG"]);
                cliente.CODATI = Convert.ToInt32(idr["CODATI"]);
                cliente.CODUNIDADE = Convert.ToInt32(idr["CODUNIDADE"]);
                cliente.CODSETIND = Convert.ToInt32(idr["CODSETIND"]);
                cliente.CODPORTE = Convert.ToInt32(idr["CODPORTE"]);

                if (idr["DATULTCARG_VA"] != DBNull.Value)
                    cliente.DATULTCARG_VA = Convert.ToDateTime(idr["DATULTCARG_VA"]);
                if (idr["NUMCARG_VA"] != DBNull.Value)
                    cliente.NUMCARG_VA = Convert.ToInt32(idr["NUMCARG_VA"]);
                cliente.SALDOCONTA = Convert.ToDecimal(idr["SALDOCONTA"]);
                cliente.PRAPAG_VA = Convert.ToInt16(idr["PRAPAG_VA"]);
                cliente.DATCTT_VA = Convert.ToDateTime(idr["DATCTT_VA"]);
                cliente.TAXSER_VA = Convert.ToDecimal(idr["TAXSER_VA"]);
                cliente.TAXADM_VA = Convert.ToDecimal(idr["TAXADM_VA"]);
                if (idr["NSUCARGA"] != DBNull.Value)
                    cliente.NSUCARGA = Convert.ToInt32(idr["NSUCARGA"]);
                cliente.CODREO = Convert.ToInt32(idr["CODREO"]);
                cliente.CODEPS = Convert.ToInt32(idr["CODEPS"]);
                cliente.TAXADESTIT = Convert.ToDecimal(idr["TAXADESTIT"]);
                cliente.TAXADESDEP = Convert.ToDecimal(idr["TAXADESDEP"]);
                cliente.NUMPAC = Convert.ToInt16(idr["NUMPAC"]);
                cliente.PGTOANTECIPADO = Convert.ToChar(idr["PGTOANTECIPADO"]);
                cliente.PAGADES = Convert.ToChar(idr["PAGADES"]);
                if (idr["DATADES"] != DBNull.Value)
                    cliente.DATADES = Convert.ToDateTime(idr["DATADES"]);
                cliente.DIASVALSALDO = Convert.ToInt16(idr["DIASVALSALDO"]);
                cliente.LIMMAXCAR = Convert.ToDecimal(idr["LIMMAXCAR"]);
                cliente.CODLOGO1 = Convert.ToString(idr["CODLOGO1"]);
                cliente.CODLOGO2 = Convert.ToString(idr["CODLOGO2"]);
                cliente.CODMODELO1 = Convert.ToString(idr["CODMODELO1"]);
                cliente.CODMODELO2 = Convert.ToString(idr["CODMODELO2"]);
                cliente.TIPOTAXSER = Convert.ToChar(idr["TIPOTAXSER"]);
                cliente.PRZVALCART = idr["PRZVALCART"].ToString();
                cliente.COBCONS = Convert.ToChar(idr["COBCONS"]);
                cliente.VALCONS = Convert.ToDecimal(idr["VALCONS"]);
                cliente.CARGPADVA = Convert.ToDecimal(idr["CARGPADVA"]);
                cliente.CRTINCBLQ = idr["CRTINCBLQ"].ToString();
                cliente.STA = idr["STA"].ToString();
                cliente.DATINC = Convert.ToDateTime(idr["DATINC"]);
                cliente.DATSTA = Convert.ToDateTime(idr["DATSTA"]);
                cliente.CTRATV = Convert.ToChar(idr["CTRATV"]);
                cliente.NOMCLI = idr["NOMCLI"].ToString();
                cliente.CGC = idr["CGC"].ToString();
                cliente.INSEST = idr["INSEST"].ToString();
                cliente.ENDCLI = idr["ENDCLI"].ToString();
                cliente.CEP = idr["CEP"].ToString();
                cliente.NUMCRT = Convert.ToInt32(idr["NUMCRT"]);
                cliente.OBS = idr["OBS"].ToString();
                cliente.TEL = idr["TEL"].ToString();
                cliente.FAX = idr["FAX"].ToString();
                cliente.EMA = idr["EMA"].ToString();
                cliente.CODLOC = Convert.ToInt32(idr["CODLOC"]);
                cliente.SIGUF0 = idr["SIGUF0"].ToString();
                if (idr["CODFILNUT"] != DBNull.Value)
                    cliente.CODFILNUT = Convert.ToInt32(idr["CODFILNUT"]);
                cliente.CON = idr["CON"].ToString();
                cliente.COB2AV = Convert.ToBoolean(idr["COB2AV"]);
                if (idr["VAL2AV"] != DBNull.Value)
                    cliente.VAL2AV = Convert.ToDecimal(idr["VAL2AV"]);
                cliente.COBINC = Convert.ToBoolean(idr["COBINC"]);
                if (idr["VALINCTIT"] != DBNull.Value)
                    cliente.VALINCTIT = Convert.ToDecimal(idr["VALINCTIT"]);
                if (idr["VALINCDEP"] != DBNull.Value)
                    cliente.VALINCDEP = Convert.ToDecimal(idr["VALINCDEP"]);
                cliente.NOMGRA = idr["NOMGRA"].ToString();
                cliente.ENDCPL = idr["ENDCPL"].ToString();
                cliente.ENDEDC = idr["ENDEDC"].ToString();
                cliente.ENDCPLEDC = idr["ENDCPLEDC"].ToString();
                if (idr["DATFEC"] != DBNull.Value)
                    cliente.DATFEC = Convert.ToInt16(idr["DATFEC"]);
                if (idr["NUMFEC"] != DBNull.Value)
                    cliente.NUMFEC = Convert.ToInt16(idr["NUMFEC"]);
                if (idr["DATULTFEC"] != DBNull.Value)
                    cliente.DATULTFEC = Convert.ToDateTime(idr["DATULTFEC"]);
                if (idr["DATRESCISAO"] != DBNull.Value)
                    cliente.DATRESCISAO = Convert.ToDateTime(idr["DATRESCISAO"]);
            }
            idr.Close();

            return cliente;
        }

        #endregion

        //private DbCommand CriaComandoSegmentoEGrupo(Database database, int codigoCliente, bool estaAutorizado)
        //{
        //    //'I' = Inclusão
        //    //'E' = Exclusão
        //    //'A' = Alteração

        //    var command = database.GetStoredProcCommand("PROC_MANU_SEG_GRUPO_AUTORIZ");

        //    database.AddInParameter(command, "SISTEMA", DbType.Int16, 1);
        //    database.AddInParameter(command, "OPERACAO", DbType.String, estaAutorizado ? 'I' : 'E');
        //    database.AddInParameter(command, "CODCLI", DbType.Int32, codigoCliente);
        //    database.AddInParameter(command, "CODSEG", DbType.Int32, null);
        //    database.AddInParameter(command, "CODATI", DbType.Int32, null);
        //    database.AddInParameter(command, "CODGRUPO", DbType.Int32, null);
        //    database.AddInParameter(command, "PERLIMEXC", DbType.Int32, null);
        //    database.AddInParameter(command, "PERLIM", DbType.Int32, null);
        //    database.AddInParameter(command, "LIMRISCO", DbType.Int32, null);
        //    database.AddInParameter(command, "MAXPARC", DbType.Int32, null);
        //    database.AddInParameter(command, "PERSUB", DbType.Int32, null);
        //    database.AddOutParameter(command, "RETORNO", DbType.String, 50);

        //    return command;
        //}

        //private void ExecutarComandoSegmentoEGrupo(Database database, DbCommand command, DbTransaction transaction)
        //{
        //    database.ExecuteNonQuery(command, transaction);
        //    var mensagem = Convert.ToString(command.Parameters["@RETORNO"].Value);
            
        //    if (!mensagem.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        throw new ApplicationException(mensagem);
        //    }
        //}

        #region Segmentos

        //private static Segmento LerSegmento(DataRow[] rows)
        //{
        //    var segmento = new Segmento
        //    {
        //        Codigo = Convert.ToInt32(rows[0]["CODSEG"]),
        //        LimiteRisco = Convert.ToInt16(rows[0]["LIMRISCO"]),
        //        Nome = Convert.ToString(rows[0]["NOMSEG"]),
        //        MaximoParcelas = Convert.ToInt16(rows[0]["MAXPARC"]),
        //        PercentualLimite = Convert.ToInt16(rows[0]["PERLIM"])
        //    };

        //    var checkeds = 0;

        //    foreach (DataRow row in rows)
        //    {
        //        var checkAti = Convert.ToChar(row["CHECKATI"]) == 'S';

        //        if (checkAti)
        //        {
        //            checkeds++;
        //        }

        //        if (row["CODATI"] == DBNull.Value)
        //        {
        //            continue;
        //        }

        //        segmento.RamosAtividade.Add(new RamoAtividade
        //        {
        //            Codigo = row["CODATI"] == DBNull.Value ? 0 : Convert.ToInt32(row["CODATI"]),
        //            Nome = row["NOMATI"] == DBNull.Value ? null : Convert.ToString(row["NOMATI"]),
        //            EstaAutorizado = checkAti
        //        });
        //    }

        //    segmento.EstaAutorizado = checkeds == 0
        //        ? AutorizacaoSegmento.Nao
        //        : checkeds == rows.Length
        //            ? AutorizacaoSegmento.Sim
        //            : AutorizacaoSegmento.Parcialmente;

        //    return segmento;
        //}

        //private void SalvarSegmentosCliente(Database database, DbTransaction transaction, int codigoCliente, IEnumerable<Segmento> segmentos)
        //{
        //    foreach (var segmento in segmentos)
        //    {
        //        foreach (var ramoAtividade in segmento.RamosAtividade)
        //        {
        //            var command = CriaComandoSegmentoEGrupo(database, codigoCliente, ramoAtividade.EstaAutorizado);
        //            command.Parameters["@CODSEG"].Value = segmento.Codigo;
        //            command.Parameters["@CODATI"].Value = ramoAtividade.Codigo;
        //            ExecutarComandoSegmentoEGrupo(database, command, transaction);
        //        }
        //    }
        //}

        //public IEnumerable<Segmento> ObterSegmentosCliente(int codigoSistema, int codigoCliente)
        //{
        //    var segmentos = new List<Segmento>();

        //    using (var conexao = new SqlConnection(BDTELENET))
        //    using (var comando = new SqlCommand("PROC_LISTA_SEG_ATI_CLIENTE", conexao))
        //    {
        //        comando.CommandType = CommandType.StoredProcedure;
        //        comando.Parameters.Add(new SqlParameter("@SISTEMA", codigoSistema));
        //        comando.Parameters.Add(new SqlParameter("@CODCLI", codigoCliente));

        //        conexao.Open();

        //        using (var reader = comando.ExecuteReader())
        //        {
        //            var dataTable = new DataTable();
        //            dataTable.Load(reader);

        //            var codigosSegmentos = dataTable
        //                .Rows
        //                .OfType<DataRow>()
        //                .Where(r => r["CODSEG"] != DBNull.Value)
        //                .Select(r => Convert.ToString(r["CODSEG"])).Distinct();

        //            foreach (var codigoSegmento in codigosSegmentos)
        //            {
        //                segmentos.Add(LerSegmento(dataTable.Select("CODSEG = " + codigoSegmento)));
        //            }
        //        }
        //    }

        //    return segmentos;
        //}

        #endregion

        #region Grupos Autorizados

        //private static GrupoCredenciado LerGrupo(DataRow[] rows)
        //{
        //    var grupo = new GrupoCredenciado
        //    {
        //        Codigo = Convert.ToInt32(rows[0]["CODGRUPO"]),
        //        LimiteRisco = Convert.ToInt16(rows[0]["LIMRISCO"]),
        //        Nome = Convert.ToString(rows[0]["NOMGRUPO"]),
        //        MaximoParcelas = Convert.ToInt16(rows[0]["MAXPARC"]),
        //        PercentualLimite = Convert.ToInt16(rows[0]["PERLIM"]),
        //        EstaAutorizado = Convert.ToChar(rows[0]["CHECKGRUPO"]) == 'S',
        //        PercentualLimiteExclusivo = Convert.ToChar(rows[0]["PERLIMEXC"]) == 'S',
        //        PercentualSubsidio = Convert.ToInt16(rows[0]["PERSUB"])
        //    };

        //    foreach (DataRow row in rows)
        //    {
        //        grupo.Credenciados.Add(new Credenciado { Nome = Convert.ToString(row["NOMFAN"]) });
        //    }

        //    return grupo;
        //}

        //private void SalvarGruposCredendiados(Database database, DbTransaction transaction, int codigoCliente, IEnumerable<GrupoCredenciado> gruposCredenciados)
        //{
        //    foreach (var grupoCredenciado in gruposCredenciados)
        //    {
        //        var command = CriaComandoSegmentoEGrupo(database, codigoCliente, grupoCredenciado.EstaAutorizado);
        //        command.Parameters["@CODGRUPO"].Value = grupoCredenciado.Codigo;
        //        ExecutarComandoSegmentoEGrupo(database, command, transaction);
        //    }
        //}

        //public IEnumerable<GrupoCredenciado> ObterGruposCredenciadosAutorizado(int codigoSistema, int codigoCliente)
        //{
        //    var grupos = new List<GrupoCredenciado>();

        //    using (var conexao = new SqlConnection(BDTELENET))
        //    using (var comando = new SqlCommand("PROC_LISTA_GRUPO_CRED_CLIENTE", conexao))
        //    {
        //        comando.CommandType = CommandType.StoredProcedure;
        //        comando.Parameters.Add(new SqlParameter("@SISTEMA", codigoSistema));
        //        comando.Parameters.Add(new SqlParameter("@CODCLI", codigoCliente));

        //        conexao.Open();

        //        using (var reader = comando.ExecuteReader())
        //        {
        //            var dataTable = new DataTable();
        //            dataTable.Load(reader);

        //            var codigosGrupos = dataTable
        //                .Rows
        //                .OfType<DataRow>()
        //                .Where(r => r["CODGRUPO"] != DBNull.Value)
        //                .Select(r => Convert.ToString(r["CODGRUPO"])).Distinct();

        //            foreach (var codigoGrupo in codigosGrupos)
        //            {
        //                grupos.Add(LerGrupo(dataTable.Select("CODGRUPO = " + codigoGrupo)));
        //            }
        //        }
        //    }

        //    return grupos;
        //}

        #endregion

        #region GET Proximo Codigo

        public int ProximoCodigoLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ISNULL(MIN(CODCLI)+1, 1) AS ProxCod");
            sql.AppendLine("FROM CLIENTEVA");
            sql.AppendLine("WHERE (CODCLI+1) NOT IN (SELECT CODCLI FROM CLIENTEVA)");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        #endregion

        #region GET Proximo Id OperadorMW

        public int ProximoIdOperadorMW()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ISNULL(MIN(ID)+1, 1) ");
            sql.AppendLine("FROM OPERADORMW ");
            sql.AppendLine("WHERE (ID+1) NOT IN (SELECT ID FROM OPERADORMW)");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        #endregion

        #region GET Cliente Ja Cadastrado

        public bool CodigoExistente(int CodCliente)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT CODCLI");
            sql.AppendLine("FROM CLIENTEVA");
            sql.AppendLine("WHERE CODCLI = @CODCLI");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.String, CodCliente);
            var CodCli = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (CodCli != 0);
        }

        public bool CNPJExistente(string CNPJ)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT CODCLI");
            sql.AppendLine("FROM CLIENTEVA");
            sql.AppendLine("WHERE CGC = @CNPJ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CNPJ", DbType.String, CNPJ);
            var CodCli = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (CodCli != 0);
        }

        #endregion

        #region Grupo de Credenciado pertence a outro grupo associado

        public bool ValidaGrupoCred(int codCli, int codGrupo)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT COUNT(G.CODGRUPO) FROM GRUPOCRED G");
            sql.AppendLine("INNER JOIN SEGAUTORIZVA S ON S.CODGRUPO = G.CODGRUPO");
            sql.AppendLine("WHERE S.CODCLI = @CODCLI");
            sql.AppendLine("AND G.CODGRUPO <> @CODGRUPO");
            sql.AppendLine("AND EXISTS (SELECT G2.CODCRE FROM GRUPOCRED G2 WHERE ");
            sql.AppendLine("G2.CODGRUPO = @CODGRUPO AND G2.CODCRE = G.CODCRE)");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.String, codCli);
            db.AddInParameter(cmd, "CODGRUPO", DbType.String, codGrupo);
            var creJaAssociado = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (creJaAssociado != 0);
        }

        #endregion

        #region GET Relatorio

        // Cartoes 2a. Via
        public List<CARTOES_SEGVIA> CartoesSegundaVia(DateTime DataInicial, DateTime DataFinal, Int32 CodInicial, Int32 CodFinal)
        {
            var ColecaoCartoesSegVia = new List<CARTOES_SEGVIA>();
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_REL_2VIA_CRTVA";
            var cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "DATAINI", DbType.DateTime, DataInicial);
            db.AddInParameter(cmd, "DATAFIM", DbType.DateTime, DataFinal);
            db.AddInParameter(cmd, "CODINI", DbType.Int32, CodInicial);
            db.AddInParameter(cmd, "CODFIM", DbType.Int32, CodFinal);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var CartaoSegVia = new CARTOES_SEGVIA();
                CartaoSegVia.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                CartaoSegVia.CODCRT = Convert.ToString(idr["CODCRT"]);
                CartaoSegVia.CPF = Convert.ToString(idr["CPF"]);
                CartaoSegVia.DATA = Convert.ToDateTime(idr["DATA"]);
                CartaoSegVia.MAT = Convert.ToString(idr["MAT"]);
                CartaoSegVia.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                CartaoSegVia.NOMUSU = Convert.ToString(idr["NOMUSU"]);
                CartaoSegVia.NUMDEP = Convert.ToInt16(idr["NUMDEP"]);
                CartaoSegVia.STA = Convert.ToString(idr["STA"]);
                CartaoSegVia.VALTRA = Convert.ToDecimal(idr["VALOR"]);

                ColecaoCartoesSegVia.Add(CartaoSegVia);
            }
            idr.Close();

            return ColecaoCartoesSegVia;
        }

        // Cartoes Bloqueio
        public List<CARTOES_BLOQUEIO> CartoesBloqueio(DateTime DataInicial, DateTime DataFinal, Int32 CodInicial, Int32 CodFinal)
        {
            var ColecaoCartoesBloqueio = new List<CARTOES_BLOQUEIO>();
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_REL_BLQ_CRTVA";
            var cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "DATAINI", DbType.DateTime, DataInicial);
            db.AddInParameter(cmd, "DATAFIM", DbType.DateTime, DataFinal);
            db.AddInParameter(cmd, "CODINI", DbType.Int32, CodInicial);
            db.AddInParameter(cmd, "CODFIM", DbType.Int32, CodFinal);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var CartaoSegVia = new CARTOES_BLOQUEIO();
                CartaoSegVia.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                CartaoSegVia.CODCRT = Convert.ToString(idr["CODCRT"]);
                CartaoSegVia.CPF = Convert.ToString(idr["CPF"]);
                CartaoSegVia.DATA = Convert.ToDateTime(idr["DATA"]);
                CartaoSegVia.MAT = Convert.ToString(idr["MAT"]);
                CartaoSegVia.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                CartaoSegVia.NOMUSU = Convert.ToString(idr["NOMUSU"]);

                if (idr["NUMDEP"] == DBNull.Value)
                    CartaoSegVia.NUMDEP = null;
                else
                    CartaoSegVia.NUMDEP = Convert.ToInt16(idr["NUMDEP"]);
                CartaoSegVia.STA = idr["STA"] == DBNull.Value ? string.Empty : Convert.ToString(idr["STA"]);
                ColecaoCartoesBloqueio.Add(CartaoSegVia);
            }
            idr.Close();
            return ColecaoCartoesBloqueio;
        }

        // Cartoes Cancelamento
        public List<CARTOES_CANCELAMENTO> CartoesCancelamento(DateTime DataInicial, DateTime DataFinal, Int32 CodInicial, Int32 CodFinal)
        {
            var ColecaoCartoesCancelamento = new List<CARTOES_CANCELAMENTO>();
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_REL_CANC_CRTVA";
            var cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "DATAINI", DbType.DateTime, DataInicial);
            db.AddInParameter(cmd, "DATAFIM", DbType.DateTime, DataFinal);
            db.AddInParameter(cmd, "CODINI", DbType.Int32, CodInicial);
            db.AddInParameter(cmd, "CODFIM", DbType.Int32, CodFinal);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var CartaoCancelamento = new CARTOES_CANCELAMENTO();
                CartaoCancelamento.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                CartaoCancelamento.CODCRT = Convert.ToString(idr["CODCRT"]);
                CartaoCancelamento.CPF = Convert.ToString(idr["CPF"]);
                CartaoCancelamento.DATA = Convert.ToDateTime(idr["DATA"]);
                CartaoCancelamento.MAT = Convert.ToString(idr["MAT"]);
                CartaoCancelamento.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                CartaoCancelamento.NOMUSU = Convert.ToString(idr["NOMUSU"]);
                CartaoCancelamento.NUMDEP = Convert.ToInt16(idr["NUMDEP"]);
                CartaoCancelamento.STA = idr["STA"] == DBNull.Value ? string.Empty : Convert.ToString(idr["STA"]);

                ColecaoCartoesCancelamento.Add(CartaoCancelamento);
            }
            idr.Close();

            return ColecaoCartoesCancelamento;
        }

        // Cartoes Inclusao
        public List<CARTOES_INCLUSAO> CartoesInclusao(DateTime DataInicial, DateTime DataFinal, Int32 CodInicial, Int32 CodFinal)
        {
            var ColecaoCartoesInclusao = new List<CARTOES_INCLUSAO>();

            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_REL_INC_CRTVA";
            var cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "DATAINI", DbType.DateTime, DataInicial);
            db.AddInParameter(cmd, "DATAFIM", DbType.DateTime, DataFinal);
            db.AddInParameter(cmd, "CODINI", DbType.Int32, CodInicial);
            db.AddInParameter(cmd, "CODFIM", DbType.Int32, CodFinal);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var CartaoInclusao = new CARTOES_INCLUSAO();
                CartaoInclusao.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                CartaoInclusao.CODCRT = Convert.ToString(idr["CODCRT"]);
                CartaoInclusao.CPF = Convert.ToString(idr["CPF"]);
                CartaoInclusao.DATA = Convert.ToDateTime(idr["DATA"]);
                CartaoInclusao.MAT = Convert.ToString(idr["MAT"]);

                if (idr["NUMDEP"] != DBNull.Value)
                    CartaoInclusao.NUMDEP = Convert.ToInt16(idr["NUMDEP"]);

                CartaoInclusao.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                CartaoInclusao.NOMUSU = Convert.ToString(idr["NOMUSU"]);
                CartaoInclusao.STA = idr["STA"] == DBNull.Value ? string.Empty : Convert.ToString(idr["STA"]);
                ColecaoCartoesInclusao.Add(CartaoInclusao);
            }
            idr.Close();
            return ColecaoCartoesInclusao;
        }

        // Acesso WEB
        public List<LOGWEB_VA> AcessoWEB(int CodInicial, int CodFinal, DateTime DataInicial, DateTime DataFinal)
        {
            var ColecaoAcessoWEB = new List<LOGWEB_VA>();
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand("PROC_REL_ACESSOWEBVA");
            db.AddInParameter(cmd, "CODINI", DbType.Int32, CodInicial);
            db.AddInParameter(cmd, "CODFIM", DbType.Int32, CodFinal);
            db.AddInParameter(cmd, "DATAINI", DbType.String, DataInicial.ToString("yyyyMMdd"));
            db.AddInParameter(cmd, "DATAFIM", DbType.String, DataFinal.ToString("yyyyMMdd"));
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var LogWebVA = new LOGWEB_VA();
                LogWebVA.CARTAO = Convert.ToString(idr["CARTAO"]);
                LogWebVA.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                LogWebVA.CPF = Convert.ToString(idr["CPF"]);
                LogWebVA.DATA = Convert.ToDateTime(idr["DATA"]);
                LogWebVA.LOGIN = Convert.ToString(idr["LOGIN"]);
                LogWebVA.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                LogWebVA.NOME = Convert.ToString(idr["NOME"]);
                LogWebVA.OPERACAO = Convert.ToString(idr["OPERACAO"]);

                ColecaoAcessoWEB.Add(LogWebVA);
            }
            idr.Close();

            return ColecaoAcessoWEB;
        }

        #endregion

        #region GET Observacoes

        public List<CLIENTEVA_OBS> Observacoes(Int32 Cliente)
        {
            var ColecaoObservacoes = new List<CLIENTEVA_OBS>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("  CODCLI, DATA, OBS, ID");
            sql.AppendLine("FROM OBSCLIVA");
            sql.AppendLine("WHERE CODCLI = @CODCLI");

            DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.String, Cliente);
            IDataReader idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var Observacao = new CLIENTEVA_OBS();
                Observacao.CODCLI = Cliente;
                Observacao.ID = Convert.ToInt32(idr["ID"]);
                Observacao.DATA = Convert.ToDateTime(idr["DATA"]);
                Observacao.OBS = Convert.ToString(idr["OBS"]);

                ColecaoObservacoes.Add(Observacao);
            }
            idr.Close();

            return ColecaoObservacoes;
        }

        #endregion

        #region Junção Ativa

        public bool JuncaoAtiva()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'JUNCAOATIVA'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        #endregion

        #region Sub-rede

        public bool ExibeSubRede()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'SUBREDE'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public int GetCodSubRede(string filtro)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODSUBREDE FROM SUBREDE ");
            if (!string.IsNullOrEmpty(filtro))
                sql.AppendLine(string.Format("WHERE {0} ", filtro));

            var cmd = db.GetSqlStringCommand(sql.ToString());
            return Convert.ToInt32(db.ExecuteScalar(cmd));
        }

        #endregion

        #region TaxaCli

        public bool ExibeModuloTaxaCli()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'TAXACLIVA'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        #endregion

        #region Embosso

        public bool Embosso()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'ARQ_EMB_NOVO'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        #endregion

        public bool ExibeTransfSaldoCli()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'CONTACLI'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public string GetStatus(int codCli)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT STA FROM CLIENTEVA ");
            sql.AppendLine("WHERE CODCLI = " + codCli);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            return Convert.ToString(db.ExecuteScalar(cmd));
        }

        public decimal ConsultaCargaPadCli(int codCli)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CARGPADVA FROM CLIENTEVA ");
            sql.AppendLine("WHERE CODCLI = " + codCli);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            return Convert.ToDecimal(db.ExecuteScalar(cmd));
        }

        #region Colecao Arq. Cartoes

        public string ConfigJobs()
        {
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand("SELECT VAL FROM CONFIG_JOBS WHERE ID0 = 'DIR_ARQ_IIS_EMBOSSO'");
            return Convert.ToString(db.ExecuteScalar(cmd));
        }

        public List<string> ColecaoArqCartoes(byte Tipo, Int32 Cliente, string Data)
        {
            List<string> ColecaoLinhasCartoes = new List<string>();

            Database db = new SqlDatabase(BDTELENET);
            string sql = "SP_GERA_ARQ_EMB_VA";
            DbCommand cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "CODCLIINI", DbType.Int32, Cliente);
            db.AddInParameter(cmd, "CODCLIFIM", DbType.Int32, Cliente);

            if (Data != string.Empty && Convert.ToDateTime(Data).Date != DateTime.MaxValue && Convert.ToDateTime(Data).Date != DateTime.MinValue)
                db.AddInParameter(cmd, "DATA", DbType.String, Convert.ToDateTime(Data).ToString("yyyyMMdd"));
            else
                db.AddInParameter(cmd, "DATA", DbType.String, DBNull.Value);

            db.AddInParameter(cmd, "AUX_DIRETORIO", DbType.String, DBNull.Value);
            db.AddInParameter(cmd, "TIPO", DbType.Int16, 0);
            db.AddInParameter(cmd, "LINI", DbType.Int32, DBNull.Value);
            db.AddInParameter(cmd, "LFIM", DbType.Int32, DBNull.Value);
            IDataReader idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                string LinhaCartao;

                LinhaCartao = String.Format("{0}|{1}", Convert.ToString(idr["LINHALST"]), Convert.ToString(idr["LINHACRT"]));

                ColecaoLinhasCartoes.Add(LinhaCartao);
            }
            idr.Close();

            return ColecaoLinhasCartoes;
        }

        public string NomProcEmbosso(int codProd)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = "SELECT NOM_PROC_EMBOSSO FROM PRODUTO WHERE CODPROD = " + codProd;
            var cmd = db.GetSqlStringCommand(sql);
            var nomProc = db.ExecuteScalar(cmd);
            var retorno = string.Empty;
            if (nomProc != DBNull.Value)
                retorno = Convert.ToString(nomProc);
            return retorno;
        }

        public string ArqCartoesEmbosso(byte Tipo, Int32 Cliente, string Data, string path, string nomProcEmbosso)
        {
            var ArqCartoesEmbosso = "";
            Database db = new SqlDatabase(BDTELENET);
            DbCommand cmd = db.GetStoredProcCommand(nomProcEmbosso);

            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, ConstantesSIL.SistemaPRE);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, Cliente);

            if (Data != string.Empty && Convert.ToDateTime(Data).Date != DateTime.MaxValue && Convert.ToDateTime(Data).Date != DateTime.MinValue)
                db.AddInParameter(cmd, "DATA", DbType.String, Convert.ToDateTime(Data).ToString("yyyyMMdd"));
            else
                db.AddInParameter(cmd, "DATA", DbType.String, DBNull.Value);

            db.AddInParameter(cmd, "AUX_DIRETORIO", DbType.String, path);
            db.AddOutParameter(cmd, "NOMEPADRAO", DbType.String, 128);
            db.AddOutParameter(cmd, "RETORNO", DbType.String, 128);
            db.AddOutParameter(cmd, "MENSAGEM", DbType.String, 128);
            db.AddOutParameter(cmd, "CODRET", DbType.Int16, 128);



            db.ExecuteNonQuery(cmd);

            ArqCartoesEmbosso = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@NOMEPADRAO"));
            var retorno = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@RETORNO"));
            var mensagerm = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@MENSAGEM"));
            var codRet = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@CODRET"));

            return ArqCartoesEmbosso;
        }

        #endregion

        #region Operadores WEB

        public string ValidadeSenha(int id)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = "SELECT DTEXPSENHA FROM OPERADORMW WHERE ID = " + id;
            var cmd = db.GetSqlStringCommand(sql);
            var validade = db.ExecuteScalar(cmd);
            var retorno = string.Empty;
            if (validade != DBNull.Value)
                retorno = Convert.ToDateTime(validade).ToShortDateString();
            return retorno;
        }

        public int DiasParaRenovarSenha()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'PRZ_SENHA_INIC'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToInt16(db.ExecuteScalar(cmd));
        }

        public List<ACESSOOPERADORMW> OperadoresWEB(Int32 Cliente, string Filtro)
        {
            var ColecaoOperadoresWEB = new List<ACESSOOPERADORMW>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ID, CODCLI, NOME, LOGIN, SENHACRIP, SENHA, DTSENHA, CNPJ, FINCCART, FBLOQCART, FDESBLOQCART, FCANCCART, FALTLIMITE, ");
            sql.AppendLine("       FSEGVIACART, FEXTMOV, FCONSREDE, FLISTTRANSAB, FLISTCART, FLISTINCCART, FCARGA, FTRANSFSALDO, FTRANSFSALDOCLI, DTEXPSENHA, QTDEACESSOINV, ");
            sql.AppendLine("       ULTACESSO, TIPOACESSO ");
            sql.AppendLine("FROM VOPERCLIVAWEB WHERE CODCLI = @CODCLI ");
            sql.AppendLine("UNION ");
            sql.AppendLine("SELECT ID, '' AS CODCLI, NOME, LOGIN, SENHACRIP, SENHA, DTSENHA, '' AS CNPJ, FINCCART, FBLOQCART, FDESBLOQCART, FCANCCART, FALTLIMITE, ");
            sql.AppendLine("       FSEGVIACART, FEXTMOV, FCONSREDE, FLISTTRANSAB, FLISTCART, FLISTINCCART, FCARGA, FTRANSFSALDO, FTRANSFSALDOCLI, DTEXPSENHA, QTDEACESSOINV, ");
            sql.AppendLine("       ULTACESSO, TIPOACESSO ");
            sql.AppendLine("FROM VOPERPARVAWEB WHERE CODPARCERIA IN (SELECT CODPARCERIA FROM CLIENTEVA WHERE CODCLI = @CODCLI) ");

            if (Filtro != string.Empty)
                sql.AppendLine(Filtro);

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.String, Cliente);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var Operador = new ACESSOOPERADORMW();
                Operador.CODCLI = Cliente;
                Operador.ID = Convert.ToInt32(idr["ID"]);
                Operador.NOME = Convert.ToString(idr["NOME"]);
                Operador.SENHA = Convert.ToString(idr["SENHA"]);
                Operador.LOGIN = Convert.ToString(idr["LOGIN"]);
                Operador.TIPOACESSO = Convert.ToString(idr["TIPOACESSO"]) == "C" ? "CLIENTE" : "PARCERIA";
                if (idr["DTEXPSENHA"] != DBNull.Value)
                    Operador.DTEXPSENHA = Convert.ToDateTime(idr["DTEXPSENHA"]).ToShortDateString();
                Operador.FBLOQCART = (idr["FBLOQCART"] != DBNull.Value) && (Convert.ToChar(idr["FBLOQCART"]) == ConstantesSIL.FlgSim);
                Operador.FCANCCART = (idr["FCANCCART"] != DBNull.Value) && (Convert.ToChar(idr["FCANCCART"]) == ConstantesSIL.FlgSim);
                Operador.FCONSREDE = (idr["FCONSREDE"] != DBNull.Value) && (Convert.ToChar(idr["FCONSREDE"]) == ConstantesSIL.FlgSim);
                Operador.FEXTMOV = (idr["FEXTMOV"] != DBNull.Value) && (Convert.ToChar(idr["FEXTMOV"]) == ConstantesSIL.FlgSim);
                Operador.FINCCART = (idr["FINCCART"] != DBNull.Value) && (Convert.ToChar(idr["FINCCART"]) == ConstantesSIL.FlgSim);
                Operador.FLISTCART = (idr["FLISTCART"] != DBNull.Value) && (Convert.ToChar(idr["FLISTCART"]) == ConstantesSIL.FlgSim);
                Operador.FLISTINCCART = (idr["FLISTINCCART"] != DBNull.Value) && (Convert.ToChar(idr["FLISTINCCART"]) == ConstantesSIL.FlgSim);
                Operador.FCONSREDE = (idr["FCONSREDE"] != DBNull.Value) && (Convert.ToChar(idr["FCONSREDE"]) == ConstantesSIL.FlgSim);
                Operador.FDESBLOQCART = (idr["FDESBLOQCART"] != DBNull.Value) && (Convert.ToChar(idr["FDESBLOQCART"]) == ConstantesSIL.FlgSim);
                Operador.FEXTMOV = (idr["FEXTMOV"] != DBNull.Value) && (Convert.ToChar(idr["FEXTMOV"]) == ConstantesSIL.FlgSim);
                Operador.FSEGVIACART = (idr["FSEGVIACART"] != DBNull.Value) && (Convert.ToChar(idr["FSEGVIACART"]) == ConstantesSIL.FlgSim);
                Operador.FCARGA = (idr["FCARGA"] != DBNull.Value) && (Convert.ToChar(idr["FCARGA"]) == ConstantesSIL.FlgSim);
                Operador.FTRANSFSALDO = (idr["FTRANSFSALDO"] != DBNull.Value) && (Convert.ToChar(idr["FTRANSFSALDO"]) == ConstantesSIL.FlgSim);
                Operador.FTRANSFSALDOCLI = (idr["FTRANSFSALDOCLI"] != DBNull.Value) && (Convert.ToChar(idr["FTRANSFSALDOCLI"]) == ConstantesSIL.FlgSim);
                Operador.FALTLIMITE = (idr["FALTLIMITE"] != DBNull.Value) && (Convert.ToChar(idr["FALTLIMITE"]) == ConstantesSIL.FlgSim);
                Operador.FLISTTRANSAB = (idr["FLISTTRANSAB"] != DBNull.Value) && (Convert.ToChar(idr["FLISTTRANSAB"]) == ConstantesSIL.FlgSim);

                // Adiciona Item
                ColecaoOperadoresWEB.Add(Operador);
            }
            idr.Close();
            return ColecaoOperadoresWEB;
        }

        public List<ACESSOOPERADORMW> OperadoresWEBParceria(string Filtro)
        {
            var ColecaoOperadoresWEB = new List<ACESSOOPERADORMW>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT DISTINCT(ID), CODPARCERIA, ID, NOME, SENHA, LOGIN, DTEXPSENHA, TIPOACESSO, NOMGRUPO_ACESSO_MULTIPLO AS NOMGRUPO, ACESSOBLOQUEADO, ");
            sql.AppendLine("FINCCART, FBLOQCART, FDESBLOQCART, FCANCCART, FALTLIMITE, FSEGVIACART, FEXTMOV, FCONSREDE, FLISTTRANSAB, FLISTCART, FLISTINCCART, FCARGA, FHABCARGSEQ, FTRANSFSALDO, FTRANSFSALDOCLI ");
            sql.AppendLine("FROM VOPERPARVAWEB ");

            if (Filtro != string.Empty)
                sql.AppendLine(Filtro);

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var Operador = new ACESSOOPERADORMW();
                Operador.TIPOACESSO = Convert.ToString(idr["TIPOACESSO"]);
                if (Operador.TIPOACESSO == "P")
                    Operador.CODPARCERIA = Convert.ToInt32(idr["CODPARCERIA"]);

                Operador.ID = Convert.ToInt32(idr["ID"]);
                Operador.NOME = Convert.ToString(idr["NOME"]);
                Operador.SENHA = Convert.ToString(idr["SENHA"]);
                Operador.LOGIN = Convert.ToString(idr["LOGIN"]);
                Operador.NOMGRUPO = Convert.ToString(idr["NOMGRUPO"]);
                Operador.ACESSOBLOQUEADO = (idr["ACESSOBLOQUEADO"] != DBNull.Value) && (Convert.ToChar(idr["ACESSOBLOQUEADO"]) == ConstantesSIL.FlgSim);

                if (idr["DTEXPSENHA"] != DBNull.Value)
                    Operador.DTEXPSENHA = Convert.ToDateTime(idr["DTEXPSENHA"]).ToShortDateString();
                Operador.FBLOQCART = (idr["FBLOQCART"] != DBNull.Value) && (Convert.ToChar(idr["FBLOQCART"]) == ConstantesSIL.FlgSim);
                Operador.FCANCCART = (idr["FCANCCART"] != DBNull.Value) && (Convert.ToChar(idr["FCANCCART"]) == ConstantesSIL.FlgSim);
                Operador.FCONSREDE = (idr["FCONSREDE"] != DBNull.Value) && (Convert.ToChar(idr["FCONSREDE"]) == ConstantesSIL.FlgSim);
                Operador.FEXTMOV = (idr["FEXTMOV"] != DBNull.Value) && (Convert.ToChar(idr["FEXTMOV"]) == ConstantesSIL.FlgSim);
                Operador.FINCCART = (idr["FINCCART"] != DBNull.Value) && (Convert.ToChar(idr["FINCCART"]) == ConstantesSIL.FlgSim);
                Operador.FLISTCART = (idr["FLISTCART"] != DBNull.Value) && (Convert.ToChar(idr["FLISTCART"]) == ConstantesSIL.FlgSim);
                Operador.FLISTINCCART = (idr["FLISTINCCART"] != DBNull.Value) && (Convert.ToChar(idr["FLISTINCCART"]) == ConstantesSIL.FlgSim);
                Operador.FCONSREDE = (idr["FCONSREDE"] != DBNull.Value) && (Convert.ToChar(idr["FCONSREDE"]) == ConstantesSIL.FlgSim);
                Operador.FDESBLOQCART = (idr["FDESBLOQCART"] != DBNull.Value) && (Convert.ToChar(idr["FDESBLOQCART"]) == ConstantesSIL.FlgSim);
                Operador.FEXTMOV = (idr["FEXTMOV"] != DBNull.Value) && (Convert.ToChar(idr["FEXTMOV"]) == ConstantesSIL.FlgSim);
                Operador.FSEGVIACART = (idr["FSEGVIACART"] != DBNull.Value) && (Convert.ToChar(idr["FSEGVIACART"]) == ConstantesSIL.FlgSim);
                Operador.FCARGA = (idr["FCARGA"] != DBNull.Value) && (Convert.ToChar(idr["FCARGA"]) == ConstantesSIL.FlgSim);
                Operador.FTRANSFSALDO = (idr["FTRANSFSALDO"] != DBNull.Value) && (Convert.ToChar(idr["FTRANSFSALDO"]) == ConstantesSIL.FlgSim);
                Operador.FTRANSFSALDOCLI = (idr["FTRANSFSALDOCLI"] != DBNull.Value) && (Convert.ToChar(idr["FTRANSFSALDOCLI"]) == ConstantesSIL.FlgSim);
                Operador.FALTLIMITE = (idr["FALTLIMITE"] != DBNull.Value) && (Convert.ToChar(idr["FALTLIMITE"]) == ConstantesSIL.FlgSim);
                Operador.FLISTTRANSAB = (idr["FLISTTRANSAB"] != DBNull.Value) && (Convert.ToChar(idr["FLISTTRANSAB"]) == ConstantesSIL.FlgSim);

                // Adiciona Item
                ColecaoOperadoresWEB.Add(Operador);
            }
            idr.Close();
            return ColecaoOperadoresWEB;
        }

        public List<CLIENTEAGRUPAMENTO> OperadoresWEBParceriaAgrupamentoCliente(int id)
        {
            var clientresAgrupamento = new List<CLIENTEAGRUPAMENTO>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT V.CODCLI, V.NOMCLI, V.SISTEMA FROM VPRODUTOSCLI V ");
            sql.AppendLine("INNER JOIN AGRUPAMENTO_CLIENTE_MW A ON(V.CODCLI = A.CODCLI AND V.SISTEMA = A.SISTEMA) ");
            sql.AppendLine("WHERE A.IDOPEMW = @IDOPEMW");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "IDOPEMW", DbType.Int32, id);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var clienteAgrupamento = new CLIENTEAGRUPAMENTO();
                clienteAgrupamento.SISTEMA = Convert.ToInt16(idr["SISTEMA"]);
                clienteAgrupamento.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                clienteAgrupamento.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                clientresAgrupamento.Add(clienteAgrupamento);
            }
            idr.Close();
            return clientresAgrupamento;
        }

        public List<string> OperadoresWEBParceriaClientes(int codParceria)
        {
            var clientresParceria = new List<string>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT V.CODCLI, V.NOMCLI, V.SISTEMA FROM VPRODUTOSCLI V ");
            sql.AppendLine("INNER JOIN PARCERIA P ON(V.PARCERIA = P.NOMPARCERIA) ");
            sql.AppendLine("WHERE P.CODPARCERIA = @CODPARCERIA");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODPARCERIA", DbType.Int32, codParceria);
            var idr = db.ExecuteReader(cmd);

            clientresParceria.Add("TODOS");

            while (idr.Read())
            {
                var sistema = Convert.ToInt16(idr["SISTEMA"]) == 0 ? "PÓS PAGO" : "PRÉ PAGO";
                var codcli = Convert.ToString(idr["CODCLI"]);
                var nomCli = Convert.ToString(idr["NOMCLI"]);
                var retorno = string.Format("{0} - COD. {1} - {2}", sistema, codcli, nomCli);
                clientresParceria.Add(retorno);
            }
            idr.Close();
            return clientresParceria;
        }

        public ACESSOOPERADORMW OperadorWEB(Int32 Cliente, int ID)
        {
            ACESSOOPERADORMW Operador = null;
            var Filtro = " AND ID = " + ID.ToString();
            // Busca Item na Colecao 
            var ColecaoOperadores = OperadoresWEB(Cliente, Filtro);

            if (ColecaoOperadores.Count > 0)
                Operador = ColecaoOperadores[0];
            return Operador;
        }

        public ACESSOOPERADORMW OperadorWEBParceria(int ID)
        {
            ACESSOOPERADORMW Operador = null;
            var Filtro = " WHERE ID = " + ID.ToString();
            // Busca Item na Colecao 
            var ColecaoOperadores = OperadoresWEBParceria(Filtro);

            if (ColecaoOperadores.Count > 0)
                Operador = ColecaoOperadores[0];
            return Operador;
        }

        #endregion

        #region Cartoes Suspensos

        public bool ExistemCartoesSuspensos(int Cliente)
        {
            Database db;
            string sql;
            DbCommand cmd;

            db = new SqlDatabase(BDAUTORIZADOR);
            sql = "SELECT COUNT(CODCARTAO) FROM CTCARTVA WHERE CODEMPRESA = @CODCLI AND STATUSU = @StatusSuspenso";

            cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.String, Cliente.ToString().PadLeft(5, '0'));
            db.AddInParameter(cmd, "StatusSuspenso", DbType.String, ConstantesSIL.StatusSuspenso);
            return (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0);
        }

        #endregion

        #region CRUD SegAutorizados

        public bool ExcluirSegAutorizado(int ClienteVA)
        {
            Database db;
            DbConnection dbc;
            DbTransaction dbt;
            string sql;
            DbCommand cmd;

            db = new SqlDatabase(BDTELENET);
            dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            dbt = dbc.BeginTransaction();

            try
            {
                sql = "DELETE FROM ATIVAUTORIZVA WHERE CODCLI = @CODCLI";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA);
                db.ExecuteNonQuery(cmd, dbt);

                sql = "DELETE FROM SEGAUTORIZVA WHERE CODCLI = @CODCLI";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA);
                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "DELETE SEGAUTORIZVA", FOperador, cmd);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Colecao Segmento]" + err);
            }
            finally
            {
                dbc.Close();

            }


            // AUTORIZADOR
            ExcluirAutorizadorSegAutorizado(ClienteVA);


            // Sucesso
            return true;
        }

        #endregion

        #region CRUD GruposAutorizados

        public bool ExcluirGrupoAutorizado(int ClienteVA)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();

            try
            {
                const string sql = "DELETE FROM SEGAUTORIZVA WHERE CODCLI = @CODCLI AND CODGRUPO IS NOT NULL";
                DbCommand cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "DELETE SEGAUTORIZVA", FOperador, cmd);

                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Colecao Grupo]" + err);
            }
            finally
            {
                dbc.Close();
            }

            // AUTORIZADOR
            ExcluirAutorizadorGrupoAutorizado(ClienteVA);

            // Sucesso
            return true;
        }

        #endregion

        #region CRUD Cliente

        public bool Inserir(CLIENTEVA ClienteVA)
        {
            var sbCamposCliente = new StringBuilder();
            var sbParametrosCliente = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            #region Campos
            // Cliente
            sbCamposCliente.Append("CODCLI, NOMCLI, NOMGRA, CGC, INSEST, CODREG, CODATI, CODUNIDADE, CODSETIND, CODPORTE,");
            // Endereco Comercial
            sbCamposCliente.Append("ENDCLI, ENDCPL, CODBAI, CODLOC, SIGUF0, CEP, TEL, FAX, CON, EMA,");
            // Endereco Entregta Cartoes 
            sbCamposCliente.Append("ENDEDC, ENDCPLEDC, CODBAIEDC, CODLOCEDC, SIGUF0EDC, CEPEDC, RESEDC, ");
            // Fechamento
            sbCamposCliente.Append("DATFEC, ");
            // Cartoes
            sbCamposCliente.Append("COB2AV, VAL2AV, COBINC, VALINCTIT, VALINCDEP, DIASVALSALDO, PRZVALCART, PAGADES, PGTOANTECIPADO, COBCONS, VALCONS, TAXADESTIT, TAXADESDEP, NUMPAC, ");
            // Carga Cartoes
            sbCamposCliente.Append("PRAPAG_VA, TIPOTAXSER, TAXSER_VA, TAXADM_VA, LIMMAXCAR, ");
            // Contrato
            sbCamposCliente.Append("DATCTT_VA, CODSUBREDE,CODPROD, CODREO, CODEPS, STA, DATSTA, CODPARCERIA, DATRESCISAO, CODLOGO1, CODLOGO2, CODMODELO1, CODMODELO2, ");
            // Outros
            sbCamposCliente.Append("CTRATV, NUMCRT, DATINC, CARGPADVA, CRTINCBLQ  ");
            #endregion

            #region Parâmetros
            // Cliente
            sbParametrosCliente.Append("@CODCLI, @NOMCLI, @NOMGRA, @CGC, @INSEST, @CODREG, @CODATI, @CODUNIDADE, @CODSETIND, @CODPORTE,");
            // Endereco Comercial
            sbParametrosCliente.Append("@ENDCLI, @ENDCPL, @CODBAI, @CODLOC, @SIGUF0, @CEP, @TEL, @FAX, @CON, @EMA,");
            // Endereco Entregta Cartoes 
            sbParametrosCliente.Append("@ENDEDC, @ENDCPLEDC, @CODBAIEDC, @CODLOCEDC, @SIGUF0EDC, @CEPEDC, @RESEDC,");
            // Fechamento
            sbParametrosCliente.Append("@DATFEC ,");
            // Cartoes
            sbParametrosCliente.Append("@COB2AV,  @VAL2AV, @COBINC, @VALINCTIT, @VALINCDEP,  @DIASVALSALDO, @PRZVALCART, @PAGADES, @PGTOANTECIPADO, @COBCONS, @VALCONS, @TAXADESTIT, @TAXADESDEP, @NUMPAC, ");
            // Carga Cartoes
            sbParametrosCliente.Append("@PRAPAG_VA, @TIPOTAXSER, @TAXSER_VA, @TAXADM_VA, @LIMMAXCAR,");
            // Contrato
            sbParametrosCliente.Append("@DATCTT_VA, @CODSUBREDE,@CODPROD, @CODREO, @CODEPS, @STA, @DATSTA, @CODPARCERIA, @DATRESCISAO, @CODLOGO1, @CODLOGO2, @CODMODELO1, @CODMODELO2, ");
            // Outros
            sbParametrosCliente.Append("@CTRATV, @NUMCRT, @DATINC, @CARGPADVA, @CRTINCBLQ  ");

            #endregion

            #region NETCARD

            var sql = string.Format("INSERT INTO CLIENTEVA ({0}) VALUES ({1})", sbCamposCliente, sbParametrosCliente);
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();

            #region Add Parameter
            // Cliente
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA.CODCLI);
            db.AddInParameter(cmd, "NOMCLI", DbType.String, UtilSIL.RemoverAcentos(ClienteVA.NOMCLI));
            db.AddInParameter(cmd, "NOMGRA", DbType.String, ClienteVA.NOMGRA);
            db.AddInParameter(cmd, "CGC", DbType.String, ClienteVA.CGC);
            db.AddInParameter(cmd, "INSEST", DbType.String, ClienteVA.INSEST);
            db.AddInParameter(cmd, "CODREG", DbType.Int32, ClienteVA.CODREG);
            db.AddInParameter(cmd, "CODATI", DbType.Int32, ClienteVA.CODATI);
            db.AddInParameter(cmd, "CODUNIDADE", DbType.Int32, ClienteVA.CODUNIDADE);
            db.AddInParameter(cmd, "CODSETIND", DbType.Int32, ClienteVA.CODSETIND);
            db.AddInParameter(cmd, "CODPORTE", DbType.Int32, ClienteVA.CODPORTE);

            // Endereco Comercial
            db.AddInParameter(cmd, "ENDCLI", DbType.String, ClienteVA.ENDCLI);
            db.AddInParameter(cmd, "ENDCPL", DbType.String, ClienteVA.ENDCPL);
            db.AddInParameter(cmd, "CODBAI", DbType.Int32, ClienteVA.CODBAI);
            db.AddInParameter(cmd, "CODLOC", DbType.Int32, ClienteVA.CODLOC);
            db.AddInParameter(cmd, "SIGUF0", DbType.String, ClienteVA.SIGUF0 ?? string.Empty);
            db.AddInParameter(cmd, "CEP", DbType.String, ClienteVA.CEP);
            db.AddInParameter(cmd, "TEL", DbType.String, ClienteVA.TEL);
            db.AddInParameter(cmd, "FAX", DbType.String, ClienteVA.FAX);
            db.AddInParameter(cmd, "CON", DbType.String, ClienteVA.CON);
            db.AddInParameter(cmd, "EMA", DbType.String, ClienteVA.EMA);

            // Endereco Entrega Cartoes 
            db.AddInParameter(cmd, "ENDEDC", DbType.String, ClienteVA.ENDEDC);
            db.AddInParameter(cmd, "ENDCPLEDC", DbType.String, ClienteVA.ENDCPLEDC);
            db.AddInParameter(cmd, "CODBAIEDC", DbType.String, ClienteVA.CODBAIEDC);
            db.AddInParameter(cmd, "CODLOCEDC", DbType.Int32, ClienteVA.CODLOCEDC);
            db.AddInParameter(cmd, "SIGUF0EDC", DbType.String, ClienteVA.SIGUF0EDC ?? string.Empty);
            db.AddInParameter(cmd, "CEPEDC", DbType.String, ClienteVA.CEPEDC);
            db.AddInParameter(cmd, "RESEDC", DbType.String, ClienteVA.RESEDC);

            //Fechamento
            db.AddInParameter(cmd, "DATFEC", DbType.Int16, ClienteVA.DATFEC);

            // Cartoes
            db.AddInParameter(cmd, "COB2AV", DbType.Boolean, ClienteVA.COB2AV);
            db.AddInParameter(cmd, "VAL2AV", DbType.Double, ClienteVA.VAL2AV);
            db.AddInParameter(cmd, "COBINC", DbType.Boolean, ClienteVA.COB2AV);
            db.AddInParameter(cmd, "VALINCTIT", DbType.Double, ClienteVA.VALINCTIT);
            db.AddInParameter(cmd, "VALINCDEP", DbType.Double, ClienteVA.VALINCDEP);
            db.AddInParameter(cmd, "DIASVALSALDO", DbType.Int16, ClienteVA.DIASVALSALDO);
            db.AddInParameter(cmd, "PRZVALCART", DbType.String, ClienteVA.PRZVALCART);
            db.AddInParameter(cmd, "PAGADES", DbType.String, ClienteVA.PAGADES);
            db.AddInParameter(cmd, "PGTOANTECIPADO", DbType.String, ClienteVA.PGTOANTECIPADO);
            db.AddInParameter(cmd, "COBCONS", DbType.String, ClienteVA.COBCONS);
            db.AddInParameter(cmd, "VALCONS", DbType.Decimal, ClienteVA.VALCONS);
            db.AddInParameter(cmd, "TAXADESTIT", DbType.Decimal, ClienteVA.TAXADESTIT);
            db.AddInParameter(cmd, "TAXADESDEP", DbType.Decimal, ClienteVA.TAXADESDEP);
            db.AddInParameter(cmd, "NUMPAC", DbType.Int16, ClienteVA.NUMPAC);

            // Carga Cartoes
            db.AddInParameter(cmd, "PRAPAG_VA", DbType.Int16, ClienteVA.PRAPAG_VA);
            db.AddInParameter(cmd, "TIPOTAXSER", DbType.String, ClienteVA.TIPOTAXSER);
            db.AddInParameter(cmd, "TAXSER_VA", DbType.Decimal, ClienteVA.TAXSER_VA);
            db.AddInParameter(cmd, "TAXADM_VA", DbType.Decimal, ClienteVA.TAXADM_VA);
            db.AddInParameter(cmd, "LIMMAXCAR", DbType.Decimal, ClienteVA.LIMMAXCAR);
            db.AddInParameter(cmd, "CODLOGO1", DbType.String, ClienteVA.CODLOGO1 ?? string.Empty);
            db.AddInParameter(cmd, "CODLOGO2", DbType.String, ClienteVA.CODLOGO2 ?? string.Empty);
            db.AddInParameter(cmd, "CODMODELO1", DbType.String, ClienteVA.CODMODELO1 ?? string.Empty);
            db.AddInParameter(cmd, "CODMODELO2", DbType.String, ClienteVA.CODMODELO2 ?? string.Empty);

            // Contrato
            db.AddInParameter(cmd, "DATCTT_VA", DbType.DateTime, ClienteVA.DATCTT_VA);
            db.AddInParameter(cmd, "CODSUBREDE", DbType.Int32, ClienteVA.CODSUBREDE);
            db.AddInParameter(cmd, "CODPROD", DbType.Int32, ClienteVA.CODPROD);
            db.AddInParameter(cmd, "CODREO", DbType.Int32, ClienteVA.CODREO);
            db.AddInParameter(cmd, "CODEPS", DbType.Int32, ClienteVA.CODEPS);
            db.AddInParameter(cmd, "STA", DbType.String, ClienteVA.STA);
            db.AddInParameter(cmd, "DATSTA", DbType.DateTime, DateTime.Now);
            db.AddInParameter(cmd, "CTRATV", DbType.String, string.Empty);
            db.AddInParameter(cmd, "NUMCRT", DbType.Int32, 0);
            db.AddInParameter(cmd, "DATINC", DbType.DateTime, DateTime.Now);
            db.AddInParameter(cmd, "CARGPADVA", DbType.Decimal, ClienteVA.CARGPADVA);
            db.AddInParameter(cmd, "CRTINCBLQ", DbType.String, ClienteVA.CRTINCBLQ);
            db.AddInParameter(cmd, "CODPARCERIA", DbType.Int32, ClienteVA.CODPARCERIA);
            db.AddInParameter(cmd, "DATRESCISAO", DbType.DateTime, ClienteVA.DATRESCISAO == DateTime.MinValue ? null : (object)ClienteVA.DATRESCISAO);

            #endregion

            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "INSERT CLIENTEVA", FOperador, cmd);
                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir]" + err);
            }
            finally
            {
                dbc.Close();
            }
            #endregion

            #region AUTORIZADOR
            // Sucesso :: Aplicar Regras Autorizador
            InserirAutorizador(ClienteVA);

            return true;

            #endregion
        }

        public bool Alterar(CLIENTEVA ClienteVA, IEnumerable<Segmento> segmentos, IEnumerable<GrupoCredenciado> gruposCredenciados)
        {
            Database db;
            StringBuilder sbCamposCliente;
            DbCommand cmd;
            DbConnection dbc;
            db = new SqlDatabase(BDTELENET);
            sbCamposCliente = new StringBuilder();
            var staAntual = GetStatus(ClienteVA.CODCLI);

            #region Campos

            // Cliente
            sbCamposCliente.Append("NOMCLI = @NOMCLI,");
            sbCamposCliente.Append("NOMGRA = @NOMGRA,");
            sbCamposCliente.Append("CGC = @CGC,");
            sbCamposCliente.Append("INSEST = @INSEST,");
            sbCamposCliente.Append("CODREG = @CODREG,");
            sbCamposCliente.Append("CODATI = @CODATI,");
            sbCamposCliente.Append("CODUNIDADE = @CODUNIDADE,");
            sbCamposCliente.Append("CODSETIND = @CODSETIND,");
            sbCamposCliente.Append("CODPORTE = @CODPORTE,");
            // Endereco Comercial
            sbCamposCliente.Append("ENDCLI = @ENDCLI,");
            sbCamposCliente.Append("ENDCPL = @ENDCPL,");
            sbCamposCliente.Append("CODBAI = @CODBAI,");
            sbCamposCliente.Append("CODLOC = @CODLOC,");
            sbCamposCliente.Append("SIGUF0 = @SIGUF0,");
            sbCamposCliente.Append("CEP = @CEP,");
            sbCamposCliente.Append("TEL = @TEL,");
            sbCamposCliente.Append("FAX = @FAX,");
            sbCamposCliente.Append("CON = @CON,");
            sbCamposCliente.Append("EMA = @EMA,");
            // Endereco Entregta Cartoes 
            sbCamposCliente.Append("ENDEDC = @ENDEDC,");
            sbCamposCliente.Append("ENDCPLEDC = @ENDCPLEDC,");
            sbCamposCliente.Append("CODBAIEDC = @CODBAIEDC,");
            sbCamposCliente.Append("CODLOCEDC = @CODLOCEDC,");
            sbCamposCliente.Append("SIGUF0EDC = @SIGUF0EDC,");
            sbCamposCliente.Append("CEPEDC = @CEPEDC,");
            sbCamposCliente.Append("RESEDC = @RESEDC,");
            // Fechamento
            sbCamposCliente.Append("DATFEC = @DATFEC,");
            // Cartoes
            sbCamposCliente.Append("COB2AV = @COB2AV,");
            sbCamposCliente.Append("VAL2AV = @VAL2AV,");
            sbCamposCliente.Append("COBINC = @COBINC,");
            sbCamposCliente.Append("VALINCTIT = @VALINCTIT,");
            sbCamposCliente.Append("VALINCDEP = @VALINCDEP,");

            sbCamposCliente.Append("DIASVALSALDO = @DIASVALSALDO,");
            sbCamposCliente.Append("PRZVALCART = @PRZVALCART,");
            sbCamposCliente.Append("PGTOANTECIPADO = @PGTOANTECIPADO,");
            sbCamposCliente.Append("PAGADES = @PAGADES,");
            sbCamposCliente.Append("COBCONS = @COBCONS,");
            sbCamposCliente.Append("VALCONS = @VALCONS,");
            sbCamposCliente.Append("TAXADESTIT = @TAXADESTIT,");
            sbCamposCliente.Append("TAXADESDEP = @TAXADESDEP,");
            sbCamposCliente.Append("NUMPAC = @NUMPAC,");
            sbCamposCliente.Append("CARGPADVA = @CARGPADVA, ");
            sbCamposCliente.Append("CRTINCBLQ = @CRTINCBLQ, ");

            // Carga Cartoes
            sbCamposCliente.Append("PRAPAG_VA = @PRAPAG_VA,");
            sbCamposCliente.Append("TIPOTAXSER = @TIPOTAXSER,");
            sbCamposCliente.Append("TAXSER_VA = @TAXSER_VA,");
            sbCamposCliente.Append("TAXADM_VA = @TAXADM_VA,");
            sbCamposCliente.Append("LIMMAXCAR = @LIMMAXCAR,");
            sbCamposCliente.Append("CODLOGO1 = @CODLOGO1,");
            sbCamposCliente.Append("CODLOGO2 = @CODLOGO2,");
            sbCamposCliente.Append("CODMODELO1 = @CODMODELO1,");
            sbCamposCliente.Append("CODMODELO2 = @CODMODELO2,");
            // Contrato
            sbCamposCliente.Append("DATCTT_VA = @DATCTT_VA,");
            sbCamposCliente.Append("CODSUBREDE = @CODSUBREDE,");
            sbCamposCliente.Append("CODAG = @CODAG,");
            sbCamposCliente.Append("CODPROD = @CODPROD,");
            sbCamposCliente.Append("CODPARCERIA = @CODPARCERIA,");
            sbCamposCliente.Append("CODREO = @CODREO,");
            sbCamposCliente.Append("CODEPS = @CODEPS,");
            sbCamposCliente.Append("DATSTA = @DATSTA,");
            sbCamposCliente.Append("DATRESCISAO = @DATRESCISAO");

            #endregion

            string sql = string.Format("UPDATE CLIENTEVA SET {0} WHERE CODCLI = @CODCLI", sbCamposCliente.ToString());
            cmd = db.GetSqlStringCommand(sql);
            dbc = db.CreateConnection();

            #region Parametros

            db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA.CODCLI);
            db.AddInParameter(cmd, "NOMCLI", DbType.String, UtilSIL.RemoverAcentos(ClienteVA.NOMCLI));
            db.AddInParameter(cmd, "NOMGRA", DbType.String, ClienteVA.NOMGRA);
            db.AddInParameter(cmd, "CGC", DbType.String, ClienteVA.NOVOCGC);
            db.AddInParameter(cmd, "INSEST", DbType.String, ClienteVA.INSEST);
            db.AddInParameter(cmd, "CODREG", DbType.Int32, ClienteVA.CODREG);
            db.AddInParameter(cmd, "CODATI", DbType.Int32, ClienteVA.CODATI);
            db.AddInParameter(cmd, "CODUNIDADE", DbType.Int32, ClienteVA.CODUNIDADE);
            db.AddInParameter(cmd, "CODSETIND", DbType.Int32, ClienteVA.CODSETIND);
            db.AddInParameter(cmd, "CODPORTE", DbType.Int32, ClienteVA.CODPORTE);
            db.AddInParameter(cmd, "ENDCLI", DbType.String, ClienteVA.ENDCLI);
            db.AddInParameter(cmd, "ENDCPL", DbType.String, ClienteVA.ENDCPL);
            db.AddInParameter(cmd, "CODBAI", DbType.Int32, ClienteVA.CODBAI);
            db.AddInParameter(cmd, "CODLOC", DbType.Int32, ClienteVA.CODLOC);
            db.AddInParameter(cmd, "SIGUF0", DbType.String, ClienteVA.SIGUF0 ?? string.Empty);
            db.AddInParameter(cmd, "CEP", DbType.String, ClienteVA.CEP);
            db.AddInParameter(cmd, "TEL", DbType.String, ClienteVA.TEL);
            db.AddInParameter(cmd, "FAX", DbType.String, ClienteVA.FAX);
            db.AddInParameter(cmd, "CON", DbType.String, ClienteVA.CON);
            db.AddInParameter(cmd, "EMA", DbType.String, ClienteVA.EMA);

            // Endereco Entrega Cartoes 
            db.AddInParameter(cmd, "ENDEDC", DbType.String, ClienteVA.ENDEDC);
            db.AddInParameter(cmd, "ENDCPLEDC", DbType.String, ClienteVA.ENDCPLEDC);
            db.AddInParameter(cmd, "CODBAIEDC", DbType.String, ClienteVA.CODBAIEDC);
            db.AddInParameter(cmd, "CODLOCEDC", DbType.Int32, ClienteVA.CODLOCEDC);
            db.AddInParameter(cmd, "SIGUF0EDC", DbType.String, ClienteVA.SIGUF0EDC ?? string.Empty);
            db.AddInParameter(cmd, "CEPEDC", DbType.String, ClienteVA.CEPEDC);
            db.AddInParameter(cmd, "RESEDC", DbType.String, ClienteVA.RESEDC);

            // Fechamento
            db.AddInParameter(cmd, "DATFEC", DbType.Int16, ClienteVA.DATFEC);

            // Cartoes
            db.AddInParameter(cmd, "COB2AV", DbType.Boolean, ClienteVA.COB2AV);
            db.AddInParameter(cmd, "VAL2AV", DbType.Double, ClienteVA.VAL2AV);
            db.AddInParameter(cmd, "COBINC", DbType.Boolean, ClienteVA.COBINC);
            db.AddInParameter(cmd, "VALINCTIT", DbType.Double, ClienteVA.VALINCTIT);
            db.AddInParameter(cmd, "VALINCDEP", DbType.Double, ClienteVA.VALINCDEP);
            db.AddInParameter(cmd, "DIASVALSALDO", DbType.Int16, ClienteVA.DIASVALSALDO);
            db.AddInParameter(cmd, "PRZVALCART", DbType.String, ClienteVA.PRZVALCART);
            db.AddInParameter(cmd, "PGTOANTECIPADO", DbType.String, ClienteVA.PGTOANTECIPADO);
            db.AddInParameter(cmd, "PAGADES", DbType.String, ClienteVA.PAGADES);
            db.AddInParameter(cmd, "COBCONS", DbType.String, ClienteVA.COBCONS);
            db.AddInParameter(cmd, "VALCONS", DbType.Decimal, ClienteVA.VALCONS);
            db.AddInParameter(cmd, "TAXADESTIT", DbType.Decimal, ClienteVA.TAXADESTIT);
            db.AddInParameter(cmd, "TAXADESDEP", DbType.Decimal, ClienteVA.TAXADESDEP);
            db.AddInParameter(cmd, "NUMPAC", DbType.Int16, ClienteVA.NUMPAC);
            db.AddInParameter(cmd, "CARGPADVA", DbType.Decimal, ClienteVA.CARGPADVA);
            db.AddInParameter(cmd, "CRTINCBLQ", DbType.String, ClienteVA.CRTINCBLQ);

            // Carga Cartoes
            db.AddInParameter(cmd, "PRAPAG_VA", DbType.Int16, ClienteVA.PRAPAG_VA);
            db.AddInParameter(cmd, "TIPOTAXSER", DbType.String, ClienteVA.TIPOTAXSER);
            db.AddInParameter(cmd, "TAXSER_VA", DbType.Decimal, ClienteVA.TAXSER_VA);
            db.AddInParameter(cmd, "TAXADM_VA", DbType.Decimal, ClienteVA.TAXADM_VA);
            db.AddInParameter(cmd, "LIMMAXCAR", DbType.Decimal, ClienteVA.LIMMAXCAR);
            db.AddInParameter(cmd, "CODLOGO1", DbType.String, ClienteVA.CODLOGO1);
            db.AddInParameter(cmd, "CODLOGO2", DbType.String, ClienteVA.CODLOGO2);
            db.AddInParameter(cmd, "CODMODELO1", DbType.String, ClienteVA.CODMODELO1);
            db.AddInParameter(cmd, "CODMODELO2", DbType.String, ClienteVA.CODMODELO2);

            // Contrato
            db.AddInParameter(cmd, "DATCTT_VA", DbType.DateTime, ClienteVA.DATCTT_VA);
            db.AddInParameter(cmd, "CODSUBREDE", DbType.Int32, ClienteVA.CODSUBREDE);
            db.AddInParameter(cmd, "CODAG", DbType.Int32, ClienteVA.CODAG);
            db.AddInParameter(cmd, "CODPROD", DbType.Int32, ClienteVA.CODPROD);
            db.AddInParameter(cmd, "CODPARCERIA", DbType.Int32, ClienteVA.CODPARCERIA);
            db.AddInParameter(cmd, "CODREO", DbType.Int32, ClienteVA.CODREO);
            db.AddInParameter(cmd, "CODEPS", DbType.Int32, ClienteVA.CODEPS);
            db.AddInParameter(cmd, "DATSTA", DbType.DateTime, ClienteVA.DATSTA);
            db.AddInParameter(cmd, "DATRESCISAO", DbType.DateTime, ClienteVA.DATRESCISAO == DateTime.MinValue ? null : (object)ClienteVA.DATRESCISAO);

            #endregion

            // Controle Transacao
            dbc.Open();
            DbTransaction dbt = dbc.BeginTransaction();

            try
            {
                if (staAntual != ClienteVA.STA)
                    AlteraStatus(db, ClienteVA.CODCLI, ClienteVA.STA);

                db.ExecuteNonQuery(cmd, dbt);
                var dalSegmentosEGrupos = new daSegmentosEGrupos(FOperador);

                dalSegmentosEGrupos.SalvarSegmentos(cmd.Connection, dbt, 1, ClienteVA.CODCLI, segmentos);
                dalSegmentosEGrupos.SalvarGrupos(cmd.Connection, dbt, 1, ClienteVA.CODCLI, gruposCredenciados);

                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "UPDATE CLIENTEVA", FOperador, cmd);

                dbt.Commit();

                // AUTORIZADOR
                AlterarAutorizador(ClienteVA);

                // OPERADORMW
                if (ClienteVA.NOVOCGC != ClienteVA.CGC)
                    AlterarCnpjOperador(ClienteVA);
            }
            catch
            {
                var msg = "Erro alteracao. Alguns valores invalidos foram inseridos na alteracao. Favor corrigir";
                dbt.Rollback();

                try
                {
                    if (staAntual != ClienteVA.STA)
                        AlteraStatus(db, ClienteVA.CODCLI, staAntual);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                } // Tenta voltar o status. Esta alteração é temporária. Será resolvido quando todo o processo de update for para SP.

                throw new Exception(msg);
            }
            finally
            {
                dbc.Close();
            }
            return true;
        }

        public bool ValidarExclusao(CLIENTEVA ClienteVA)
        {
            Database db = new SqlDatabase(BDTELENET);
            StringBuilder sql;
            sql = new StringBuilder();
            sql.AppendLine("SELECT CODCLI");
            sql.AppendLine("FROM CLIENTEVA");
            sql.AppendLine("WHERE CODCLI = @CODCLI");
            sql.AppendLine("  AND (STA = @STA)");
            sql.AppendLine("  AND (CONVERT(CHAR(10), DATSTA, 102) <= CONVERT(CHAR(10), GETDATE() - 7, 102))");
            DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
            int CodCli;
            // Cliente
            cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA.CODCLI);
            db.AddInParameter(cmd, "STA", DbType.String, ConstantesSIL.StatusCancelado);
            CodCli = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (CodCli != 0);
        }

        public bool Excluir(CLIENTEVA ClienteVA)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = string.Format("DELETE CLIENTEVA WHERE CODCLI = @CODCLI");
            DbCommand cmd = db.GetSqlStringCommand(sql);
            DbConnection dbc = db.CreateConnection();
            // Cliente
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA.CODCLI);
            // Controle Transacao
            dbc.Open();
            DbTransaction dbt = dbc.BeginTransaction();

            try
            {
                // Seg Autorizado
                ExcluirSegAutorizado(ClienteVA.CODCLI);
                // Obs 
                ExcluirObs(db, dbt, ClienteVA.CODCLI);
                // Dependencias
                ExcluirDependencias(db, dbt, ClienteVA.CODCLI);
                // Excluir Cliente
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (EXCETO QUANDO GERA TRANSAcaO)
                UtilSIL.GravarLog(db, dbt, "DELETE CLIENTEVA", FOperador, cmd);
                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir]" + err);
            }
            finally
            {
                dbc.Close();
            }
            // AUTORIZADOR
            ExcluirAutorizador(ClienteVA);
            return true;
        }

        #endregion

        #region CRUD Observacoes

        public bool InserirObs(int CODCLI, DateTime DATA, string OBS, Int32 ID)
        {

            Database db;
            StringBuilder sbCamposCliente;
            StringBuilder sbParametrosCliente;
            DbTransaction dbt;

            db = new SqlDatabase(BDTELENET);
            sbCamposCliente = new StringBuilder();
            sbParametrosCliente = new StringBuilder();

            sbCamposCliente.Append("CODCLI, DATA, OBS");

            // Cliente
            sbParametrosCliente.Append("@CODCLI, @DATA, @OBS");

            string sql = string.Format("INSERT INTO OBSCLIVA ({0}) VALUES ({1})", sbCamposCliente.ToString(), sbParametrosCliente.ToString());
            DbCommand cmd = db.GetSqlStringCommand(sql);
            DbConnection dbc = db.CreateConnection();

            db.AddInParameter(cmd, "CODCLI", DbType.Int32, CODCLI);

            db.AddInParameter(cmd, "DATA", DbType.DateTime, DATA.ToString("G"));
            db.AddInParameter(cmd, "OBS", DbType.String, OBS);

            // Controle Transacao
            dbc.Open();
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "INSERT OBSCLIVA", FOperador, cmd);

                dbt.Commit();
                return (LinhaAfetada == 1);

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Obs]" + err);
            }
            finally
            {
                dbc.Close();
            }

        }

        public bool AlterarObs(Int32 ID, string OBS)
        {
            Database db;
            string sql;
            DbCommand cmd;
            DbConnection dbc;
            DbTransaction dbt;

            db = new SqlDatabase(BDTELENET);
            sql = "UPDATE OBSCLIVA SET OBS = @OBS WHERE ID = @ID";
            cmd = db.GetSqlStringCommand(sql);
            dbc = db.CreateConnection();

            // Cliente
            db.AddInParameter(cmd, "ID", DbType.Int32, ID);
            db.AddInParameter(cmd, "OBS", DbType.String, OBS);

            // Controle Transacao
            dbc.Open();
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "UPDATE OBSCLIVA", FOperador, cmd);


                dbt.Commit();
                return (LinhaAfetada == 1);

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Alterar Obs]" + err);
            }
            finally
            {
                dbc.Close();

            }
        }

        public bool ExcluirObs(Int32 ID)
        {
            Database db;
            string sql;
            DbCommand cmd;
            DbConnection dbc;
            DbTransaction dbt;

            db = new SqlDatabase(BDTELENET);

            sql = string.Format("DELETE OBSCLIVA WHERE ID = @ID");
            cmd = db.GetSqlStringCommand(sql);
            dbc = db.CreateConnection();

            // Cliente            
            db.AddInParameter(cmd, "ID", DbType.Int32, ID);

            // Controle Transacao
            dbc.Open();
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "DELETE OBSCLIVA", FOperador, cmd);

                dbt.Commit();
                return (LinhaAfetada == 1);

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Obs]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool ExcluirObs(Database db, DbTransaction dbt, int ClienteVA)
        {
            string sql;
            DbCommand cmd;
            // Deleta primeiro toda a colecao
            sql = "DELETE FROM OBSCLIVA WHERE CODCLI = @CODCLI";
            cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA);

            db.ExecuteNonQuery(cmd, dbt);

            //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
            UtilSIL.GravarLog(db, dbt, "DELETE OBSCLIVA (Exclusao da lista observacoes do cliente excluido)", FOperador, cmd);

            return true;

        }

        public bool ExcluirDependencias(Database db, DbTransaction dbt, int ClienteVA)
        {
            string sql;
            DbCommand cmd;

            sql = "DELETE FROM USUARIOVA WHERE CODCLI = @CODCLI";
            cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA);
            db.ExecuteNonQuery(cmd, dbt);

            //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
            UtilSIL.GravarLog(db, dbt, "DELETE USUARIOVA (Exclusao da lista de usuarios do Cliente excluido )", FOperador, cmd);


            sql = "DELETE FROM TRANSACVA WHERE CODCLI = @CODCLI";
            cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA);
            db.ExecuteNonQuery(cmd, dbt);

            //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
            UtilSIL.GravarLog(db, dbt, "DELETE TRANSACVA (Exclusao da lista de Transacoes do Cliente excluido )", FOperador, cmd);


            sql = "DELETE FROM CARGAC WHERE CODCLI = @CODCLI";
            cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA);
            db.ExecuteNonQuery(cmd, dbt);

            //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
            UtilSIL.GravarLog(db, dbt, "DELETE CARGAC (Exclusao da lista de Cargas do Cliente excluido )", FOperador, cmd);

            sql = "DELETE FROM OPERCLIVAWEB WHERE CODCLI = @CODCLI";
            cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA);
            db.ExecuteNonQuery(cmd, dbt);

            //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
            UtilSIL.GravarLog(db, dbt, "DELETE OPERCLIVAWEB (Exclusao da lista de Operadores do Cliente excluido )", FOperador, cmd);

            return true;

        }

        #endregion

        #region CRUD AUTORIZADOR

        // Principais
        private void InserirAutorizador(CLIENTEVA ClienteVA)
        {
            Database db;
            DbConnection dbc;
            DbTransaction dbt;
            db = db = new SqlDatabase(BDAUTORIZADOR);
            dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            dbt = dbc.BeginTransaction();

            try
            {
                // Cliente
                InserirAutorizadorCliente(ClienteVA, db, dbt);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Autorizador]" + err);
            }
            finally
            {
                dbc.Close();

            }
        }

        private void AlterarAutorizador(CLIENTEVA ClienteVA)
        {
            Database db;
            DbConnection dbc;
            DbTransaction dbt;

            db = db = new SqlDatabase(BDAUTORIZADOR);
            dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            dbt = dbc.BeginTransaction();

            try
            {
                // Se Nao Existir Cliente => Inserir
                if (!ExisteClienteAutorizador(ClienteVA, db, dbt))
                    InserirAutorizadorCliente(ClienteVA, db, dbt);
                else
                    // Alterar
                    AlterarAutorizadorCliente(ClienteVA, db, dbt);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Alterar Autorizador]" + err);
            }
            finally
            {
                dbc.Close();

            }
        }

        private void AlterarCnpjOperador(CLIENTEVA ClienteVA)
        {
            Database db;
            DbConnection dbc;
            DbTransaction dbt;

            db = db = new SqlDatabase(BDTELENET);
            dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            dbt = dbc.BeginTransaction();

            try
            {
                string sql;
                DbCommand cmd;

                sql = "UPDATE O SET O.CNPJ = @CNPJNOVO FROM OPERADORMW O " +
                      "INNER JOIN ACESSOOPERADORMW A ON A.IDOPEMW = O.Id " +
                      "WHERE A.CODCLI = @CODCLIENTE " +
                      "AND O.CNPJ = @CNPJANTIGO";

                cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.CODCLI.ToString().PadLeft(5, '0'));
                db.AddInParameter(cmd, "CNPJNOVO", DbType.String, ClienteVA.NOVOCGC);
                db.AddInParameter(cmd, "CNPJANTIGO", DbType.String, ClienteVA.CGC);

                db.ExecuteNonQuery(cmd, dbt);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Alterar Autorizador]" + err);
            }
            finally
            {
                dbc.Close();

            }
        }

        private void ExcluirAutorizador(CLIENTEVA ClienteVA)
        {
            Database db;
            DbConnection dbc;
            DbTransaction dbt;

            db = db = new SqlDatabase(BDAUTORIZADOR);
            dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            dbt = dbc.BeginTransaction();

            try
            {
                // Cliente
                ExcluirAutorizadorCliente(ClienteVA, db, dbt);

                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Autorizador]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        // Detalhes 
        private bool ExisteClienteAutorizador(CLIENTEVA ClienteVA, Database db, DbTransaction dbt)
        {
            string sql;
            DbCommand cmd;
            string CodCliente;

            sql = "SELECT CODCLIENTE FROM CTCLIVA WHERE CODCLIENTE = @CODCLIENTE";

            cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.CODCLI.ToString().PadLeft(5, '0'));

            CodCliente = Convert.ToString(db.ExecuteScalar(cmd, dbt));

            return (CodCliente != string.Empty);
        }

        private void InserirAutorizadorCliente(CLIENTEVA ClienteVA, Database db, DbTransaction dbt)
        {
            string sql;
            DbCommand cmd;

            sql = "INSERT INTO CTCLIVA " +
                  "(CODCLIENTE, NOMECLI, STATCLI, DTSTATCLI, DIASVALSALDO, COBCONS, VALCONS) VALUES " +
                  "(@CODCLIENTE, @NOMECLI, @STATCLI, @DTSTATCLI, @DIASVALSALDO, @COBCONS, @VALCONS)";
            cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.CODCLI.ToString().PadLeft(5, '0'));
            db.AddInParameter(cmd, "NOMECLI", DbType.String, ClienteVA.NOMCLI.Length > 30 ? ClienteVA.NOMCLI.Substring(0, 30) : ClienteVA.NOMCLI);
            db.AddInParameter(cmd, "STATCLI", DbType.String, ClienteVA.STA);
            db.AddInParameter(cmd, "DTSTATCLI", DbType.DateTime, ClienteVA.DATSTA);
            db.AddInParameter(cmd, "DIASVALSALDO", DbType.Int16, ClienteVA.DIASVALSALDO);
            db.AddInParameter(cmd, "COBCONS", DbType.String, ClienteVA.COBCONS);
            db.AddInParameter(cmd, "VALCONS", DbType.Decimal, ClienteVA.VALCONS);

            db.ExecuteNonQuery(cmd, dbt);

        }

        private void AlterarAutorizadorCliente(CLIENTEVA ClienteVA, Database db, DbTransaction dbt)
        {
            string sql;
            DbCommand cmd;
            string NomeCliente;

            sql = "UPDATE CTCLIVA SET NOMECLI = @NOMECLI, STATCLI = @STATCLI, DTSTATCLI = @DTSTATCLI, " +
                  "DIASVALSALDO = @DIASVALSALDO, COBCONS = @COBCONS, VALCONS = @VALCONS, DATRESCISAO = @DATRESCISAO " +
                  "WHERE CODCLIENTE = @CODCLIENTE";

            cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.CODCLI.ToString().PadLeft(5, '0'));
            NomeCliente = ClienteVA.NOMCLI;
            if (NomeCliente.Length > 30)
                NomeCliente = ClienteVA.NOMCLI.Substring(0, 30);
            db.AddInParameter(cmd, "NOMECLI", DbType.String, NomeCliente);
            db.AddInParameter(cmd, "STATCLI", DbType.String, ClienteVA.STA);
            db.AddInParameter(cmd, "DTSTATCLI", DbType.DateTime, ClienteVA.DATSTA);
            db.AddInParameter(cmd, "DIASVALSALDO", DbType.Int16, ClienteVA.DIASVALSALDO);
            db.AddInParameter(cmd, "COBCONS", DbType.String, ClienteVA.COBCONS);
            db.AddInParameter(cmd, "VALCONS", DbType.Decimal, ClienteVA.VALCONS);
            db.AddInParameter(cmd, "DATRESCISAO", DbType.DateTime, ClienteVA.DATRESCISAO == DateTime.MinValue ? null : (object)ClienteVA.DATRESCISAO);

            db.ExecuteNonQuery(cmd, dbt);
        }

        private void ExcluirAutorizadorCliente(CLIENTEVA ClienteVA, Database db, DbTransaction dbt)
        {
            string sql;
            DbCommand cmd;

            // CTCARTVA 
            sql = "DELETE CTCARTVA WHERE CODEMPRESA = @CODCLIENTE";
            cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.CODCLI.ToString().PadLeft(5, '0'));
            db.ExecuteNonQuery(cmd, dbt);

            // CTCLIVA 
            sql = "DELETE CTCLIVA WHERE CODCLIENTE = @CODCLIENTE";
            cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.CODCLI.ToString().PadLeft(5, '0'));
            db.ExecuteNonQuery(cmd, dbt);
        }

        private void ExcluirAutorizadorSegAutorizado(int ClienteVA)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();

            try
            {
                const string sql = "DELETE FROM TABSEGVA WHERE CODCLIENTE = @CODCLIENTE";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.ToString().PadLeft(5, '0'));
                db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Autorizador :: Coleccao Se. Autorizado]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        private void ExcluirAutorizadorGrupoAutorizado(int ClienteVA)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "DELETE FROM TABSEGVA WHERE CODCLIENTE = @CODCLIENTE AND CODGRUPO IS NOT NULL";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.ToString().PadLeft(5, '0'));
                db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Autorizador :: Coleccao Grupo Autorizado]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool SuspenderCartoesAtivos(int Cliente)
        {
            Database db;
            string sql;
            DbCommand cmd;
            DbConnection dbc;
            DbTransaction dbt;

            db = new SqlDatabase(BDAUTORIZADOR);

            sql = "UPDATE CTCARTVA SET STATUSU = @STATUSU, DTSTATUSU = @DTSTATUSU WHERE CODEMPRESA = @CODCLI";

            cmd = db.GetSqlStringCommand(sql);
            dbc = db.CreateConnection();

            db.AddInParameter(cmd, "CODCLI", DbType.String, Cliente.ToString().PadLeft(5, '0'));
            db.AddInParameter(cmd, "STATUSU", DbType.String, ConstantesSIL.StatusSuspenso);
            db.AddInParameter(cmd, "DTSTATUSU", DbType.DateTime, DateTime.Now);
            // Controle Transacao
            dbc.Open();
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);


                ////LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                //UtilSIL.GravarLog(null, dbt, "UPDATE CTCARTVA (Suspender Cartoes Ativos)", FOperador, cmd);

                dbt.Commit();
                return (LinhaAfetada == 1);

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir CartaoVA] " + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool AtivarCartoesSuspensos(int Cliente)
        {
            Database db;
            string sql;
            DbCommand cmd;
            DbConnection dbc;
            DbTransaction dbt;

            db = new SqlDatabase(BDAUTORIZADOR);

            sql = "UPDATE CTCARTVA SET STATUSU = @STATUSU, DTSTATUSU = @DTSTATUSU WHERE CODEMPRESA = @CODCLI AND STATUSU = @StatusSuspenso";

            cmd = db.GetSqlStringCommand(sql);
            dbc = db.CreateConnection();

            db.AddInParameter(cmd, "CODCLI", DbType.String, Cliente.ToString().PadLeft(5, '0'));
            db.AddInParameter(cmd, "STATUSU", DbType.String, ConstantesSIL.StatusAtivo);
            db.AddInParameter(cmd, "DTSTATUSU", DbType.DateTime, DateTime.Now);
            db.AddInParameter(cmd, "StatusSuspenso", DbType.String, ConstantesSIL.StatusSuspenso);

            // Controle Transacao
            dbc.Open();
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);

                ////LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                //db = new SqlDatabase(BDTELENET);
                //UtilSIL.GravarLog(null, dbt, "UPDATE CTCARTVA (Ativar Cartoes suspensos)", FOperador, cmd);

                dbt.Commit();
                return (LinhaAfetada == 1);

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir CartaoVA] " + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        #endregion

        #region CRUD TAXAS CLIENTE

        public bool SalvarTaxaCli(MODTAXA taxacli)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_ASSOCIA_TAXACLI";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "@SISTEMA", DbType.Int32, ConstantesSIL.SistemaPRE);
            db.AddInParameter(cmd, "@CODCLI", DbType.String, taxacli.COD);
            db.AddInParameter(cmd, "@CODTAXA", DbType.String, taxacli.CODTAXA);
            db.AddInParameter(cmd, "@VALOR", DbType.Decimal, taxacli.VALOR);
            db.AddInParameter(cmd, "@VALORDEP", DbType.Decimal, taxacli.VALORDEP);
            db.AddInParameter(cmd, "@DTINICIO", DbType.DateTime, taxacli.DTINICIO);
            db.AddInParameter(cmd, "@DIASPINICIO", DbType.Int16, taxacli.DIASPINICIO);
            db.AddInParameter(cmd, "@TAXAHAB", DbType.String, taxacli.TAXAHAB);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "PROC_ASSOCIA_TAXACLI", FOperador, cmd);
                dbt.Commit();
                return (LinhaAfetada == 1);
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool ExcluirTaxaCli(MODTAXA taxacli)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_EXCLUI_TAXACLI";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, ConstantesSIL.SistemaPRE);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, taxacli.COD);
            db.AddInParameter(cmd, "CODTAXA", DbType.Int32, taxacli.CODTAXA);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "PROC_EXCLUI_TAXACLI", FOperador, cmd);
                dbt.Commit();
                return (LinhaAfetada == 1);
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        public int ValidarExclusaoTaxaCli(MODTAXA taxacli)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT COUNT(DATTRA) AS NUMTRANS FROM TRANSACVA ");
            sql.AppendLine("WHERE TIPTRA = @TIPTRA ");
            sql.AppendLine("AND CODCLI = @CODCLI");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, taxacli.COD);
            db.AddInParameter(cmd, "TIPTRA", DbType.Int32, taxacli.TIPTRA);
            return (int)(db.ExecuteScalar(cmd));
        }

        #endregion

        #region CRUD Operadores WEB

        public bool InserirAcessoOperadorMW(ACESSOOPERADORMW Acesso, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_INSERE_OPERADORWEB";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            IDataReader idr = null;
            //db.AddInParameter(cmd, "CNPJ", DbType.String, Acesso.CNPJ);
            db.AddInParameter(cmd, "LOGIN", DbType.String, UtilSIL.RemoverAcentos(Acesso.LOGIN));
            db.AddInParameter(cmd, "NOME", DbType.String, UtilSIL.RemoverAcentos(Acesso.NOME));
            if (Acesso.TIPOACESSO == "D") db.AddInParameter(cmd, "NOMGRUPO", DbType.String, UtilSIL.RemoverAcentos(Acesso.NOMGRUPO));
            if (Acesso.TIPOACESSO == "P") db.AddInParameter(cmd, "CODPARCERIA", DbType.Int32, Acesso.CODPARCERIA);
            db.AddInParameter(cmd, "TIPOACESSO", DbType.String, Acesso.TIPOACESSO);
            //db.AddInParameter(cmd, "CODCLI", DbType.Int32, Acesso.CODCLI);
            //db.AddInParameter(cmd, "SISTEMA", DbType.Int16, ConstantesSIL.SistemaPRE);
            db.AddInParameter(cmd, "ACESSOBLOQUEADO", DbType.String, ConstantesSIL.BoolToChar(Acesso.ACESSOBLOQUEADO));
            db.AddInParameter(cmd, "FINCCART", DbType.String, ConstantesSIL.BoolToChar(Acesso.FINCCART));
            db.AddInParameter(cmd, "FBLOQCART", DbType.String, ConstantesSIL.BoolToChar(Acesso.FBLOQCART));
            db.AddInParameter(cmd, "FDESBLOQCART", DbType.String, ConstantesSIL.BoolToChar(Acesso.FDESBLOQCART));
            db.AddInParameter(cmd, "FCANCCART", DbType.String, ConstantesSIL.BoolToChar(Acesso.FCANCCART));
            db.AddInParameter(cmd, "FSEGVIACART", DbType.String, ConstantesSIL.BoolToChar(Acesso.FSEGVIACART));
            db.AddInParameter(cmd, "FEXTMOV", DbType.String, ConstantesSIL.BoolToChar(Acesso.FEXTMOV));
            db.AddInParameter(cmd, "FCONSREDE", DbType.String, ConstantesSIL.BoolToChar(Acesso.FCONSREDE));
            db.AddInParameter(cmd, "FLISTCART", DbType.String, ConstantesSIL.BoolToChar(Acesso.FLISTCART));
            db.AddInParameter(cmd, "FLISTINCCART", DbType.String, ConstantesSIL.BoolToChar(Acesso.FLISTINCCART));
            db.AddInParameter(cmd, "FCARGA", DbType.String, ConstantesSIL.BoolToChar(Acesso.FCARGA));
            db.AddInParameter(cmd, "FTRANSFSALDO", DbType.String, ConstantesSIL.BoolToChar(Acesso.FTRANSFSALDO));
            db.AddInParameter(cmd, "FTRANSFSALDOCLI", DbType.String, ConstantesSIL.BoolToChar(Acesso.FTRANSFSALDOCLI));
            db.AddInParameter(cmd, "FALTLIMITE", DbType.String, ConstantesSIL.BoolToChar(Acesso.FALTLIMITE));
            db.AddInParameter(cmd, "FLISTTRANSAB", DbType.String, ConstantesSIL.BoolToChar(Acesso.FLISTTRANSAB));
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var id = 0;
                idr = db.ExecuteReader(cmd, dbt);
                if (idr.Read())
                {
                    retorno = Convert.ToString(idr["RETORNO"]);
                    if (retorno == "OK") id = Acesso.ID > 0 ? Acesso.ID : Convert.ToInt32(idr["IDOPEMW"]);
                    idr.Close();
                    if (retorno == "OK")
                    {
                        UtilSIL.GravarLog(db, dbt, "PROC_INSERE_OPERADORWEB", FOperador, cmd);

                        if (Acesso.TIPOACESSO == "D")
                        {
                            foreach (var item in Acesso.AGRUPAMENTO.Where(x => x.TIPOATUALIZACAO == "I").ToList())
                            {
                                var sucesso = InsereAgrupamento(db, dbt, id, item.SISTEMA, item.CODCLI);
                                if (!sucesso)
                                {
                                    dbt.Rollback();
                                    retorno = "Erro ao cadastrar o operador.";
                                    return sucesso;
                                }
                            }
                            foreach (var item in Acesso.AGRUPAMENTO.Where(x => x.TIPOATUALIZACAO == "E").ToList())
                            {
                                var sucesso = ExcluirAgrupamento(db, dbt, id, item.SISTEMA, item.CODCLI);
                                if (!sucesso)
                                {
                                    dbt.Rollback();
                                    retorno = "Erro ao cadastrar o operador.";
                                    return sucesso;
                                }
                            }
                        }
                        dbt.Commit();
                        retorno = retorno == "OK" ? "Operador cadastrado com sucesso." : "Erro ao cadastrar o operador.";
                        return true;
                    }
                }
                return false;
            }
            catch (Exception err)
            {
                if (idr != null)
                    idr.Close();
                retorno = "Erro ao cadastrar o operador.";
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool InsereAgrupamento(Database db, DbTransaction dbt, int id, int sistema, int codCli)
        {            
            const string sql = "INSERT INTO AGRUPAMENTO_CLIENTE_MW (IDOPEMW, SISTEMA, CODCLI, ID_FUNC, DATINC) VALUES (@IDOPEMW, @SISTEMA, @CODCLI, @ID_FUNC, @DATINC)";
            var cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "IDOPEMW", DbType.Int32, id);
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, sistema);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            db.AddInParameter(cmd, "ID_FUNC", DbType.Int32, FOperador.ID_FUNC);
            db.AddInParameter(cmd, "DATINC", DbType.DateTime, DateTime.Now);
            return db.ExecuteNonQuery(cmd, dbt) > 0;
        }

        public bool ExcluirAgrupamento(Database db, DbTransaction dbt, int id, int sistema, int codCli)
        {
            const string sql = "DELETE AGRUPAMENTO_CLIENTE_MW WHERE IDOPEMW = @IDOPEMW AND SISTEMA = @SISTEMA AND CODCLI = @CODCLI ";
            var cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "IDOPEMW", DbType.Int32, id);
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, sistema);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            return db.ExecuteNonQuery(cmd, dbt) > 0;
        }

        public bool ExcluirOperadorWEB(ACESSOOPERADORMW Acesso)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "DELETE ACESSOOPERADORMW WHERE IDOPEMW = @IDOPEMW";
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            // Operador 
            db.AddInParameter(cmd, "IDOPEMW", DbType.Int32, Acesso.ID);
            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {   // Linha Afetada                                
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);

                if (LinhaAfetada > 0 && ExisteAgrupamento(Acesso.ID))
                {
                    ExcluirAgrupamento(Acesso.ID);
                }
                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "DELETE ACESSOOPERADORMW", FOperador, cmd);
                dbt.Commit();
                return (LinhaAfetada == 1);
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool ExisteAgrupamento(int id)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT COUNT(IDOPEMW) FROM AGRUPAMENTO_CLIENTE_MW WHERE IDOPEMW = @IDOPEMW";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "IDOPEMW", DbType.Int32, id);
            return Convert.ToInt32(db.ExecuteScalar(cmd)) >= 0;
        }

        public void ExcluirAgrupamento(int id)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "DELETE AGRUPAMENTO_CLIENTE_MW WHERE IDOPEMW = @IDOPEMW";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "IDOPEMW", DbType.Int32, id);
            db.ExecuteScalar(cmd);
        }

        public bool RenovarAcessoOperadorWEB(OPERADORMW Operador, int codCli)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "UPDATE OPERADORMW SET DTEXPSENHA = @DTEXPSENHA, DTSENHA = @DTSENHA, QTDEACESSOINV = 0, " +
                               "SENHA = @SENHA WHERE ID = @ID AND TIPOACESSO = @TIPOACESSO";

            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            var dataRenavacao = DateTime.Now.AddDays(DiasParaRenovarSenha());

            db.AddInParameter(cmd, "DTEXPSENHA", DbType.String, dataRenavacao.ToString("yyyyMMdd"));
            db.AddInParameter(cmd, "DTSENHA", DbType.DateTime, null);
            db.AddInParameter(cmd, "SENHA", DbType.String, Operador.SENHA);
            db.AddInParameter(cmd, "ID", DbType.Int32, Operador.ID);
            db.AddInParameter(cmd, "TIPOACESSO", DbType.String, Operador.TIPOACESSO);

            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "UPDATE OPERADORMW", FOperador, cmd);
                dbt.Commit();
                return (LinhaAfetada == 1);
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        #endregion

        #region Get Listas

        public List<CLIENTEVA_PREPAGO> ColecaoClientesVA(string Filtro)
        {
            var conn = new SqlConnection();
            conn.ConnectionString = BDTELENET;
            var sql = new StringBuilder();
            sql.AppendLine(" SELECT");
            sql.AppendLine(" C.CODCLI, C.NOMCLI, C.CGC, S.DESTA, C.STA, C.PRZVALCART, C.SALDOCONTA, P.ROTULO, P.CODPROD, P.TIPOPROD, T.DESCRICAO ");
            sql.AppendLine(" FROM CLIENTEVA C");
            sql.AppendLine(" INNER JOIN STATUS S ON C.STA = S.STA");
            sql.AppendLine(" INNER JOIN PRODUTO P ON C.CODPROD = P.CODPROD ");
            sql.AppendLine(" INNER JOIN TIPOPRODUTO T ON P.TIPOPROD = T.TIPOPROD ");
            sql.AppendLine(" INNER JOIN LOCALIDADE L ON C.CODLOC = L.CODLOC ");

            if (!string.IsNullOrEmpty(Filtro))
                sql.AppendLine(string.Format("WHERE {0} ", Filtro));
            sql.AppendLine(" ORDER BY CODCLI");
            var cmd = new SqlCommand(sql.ToString(), conn);
            conn.Open();
            var idr = cmd.ExecuteReader();
            var colecaoClientesVA = new List<CLIENTEVA_PREPAGO>();


            while (idr.Read())
            {
                var clienteVA = new CLIENTEVA_PREPAGO();
                clienteVA.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                clienteVA.NOMCLI = idr["NOMCLI"].ToString();
                clienteVA.CGC = idr["CGC"].ToString();
                clienteVA.DESTA = idr["DESTA"].ToString();
                clienteVA.STA = idr["STA"].ToString();
                clienteVA.PRZVALCART = idr["PRZVALCART"].ToString();
                clienteVA.SALDO = idr["SALDOCONTA"].ToString();
                clienteVA.PRODUTO = idr["ROTULO"].ToString();
                clienteVA.CODPROD = Convert.ToInt32(idr["CODPROD"]);
                clienteVA.TIPOPROD = Convert.ToInt32(idr["TIPOPROD"]);
                clienteVA.TIPOPRODDESCRICAO = idr["DESCRICAO"].ToString();
                colecaoClientesVA.Add(clienteVA);
            }
            idr.Close();
            if (conn.State == ConnectionState.Open)
                conn.Close();

            return colecaoClientesVA;
        }

        public List<CLIENTEVA> ListaClientes()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT");
            sql.AppendLine("  CODCLI, NOMCLI");
            sql.AppendLine("  FROM CLIENTEVA");
            sql.AppendLine("  ORDER BY CODCLI");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            var colecaoClientesVA = new List<CLIENTEVA>();

            while (idr.Read())
            {
                var clienteVA = new CLIENTEVA();
                clienteVA.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                clienteVA.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                colecaoClientesVA.Add(clienteVA);
            }
            idr.Close();
            cmd.Connection.Close();
            return colecaoClientesVA;
        }

        public List<VENDEDOR> ListaVendedores()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT");
            sql.AppendLine("  CODVEND, NOMVEND");
            sql.AppendLine("  FROM VENDEDOR");
            sql.AppendLine("  ORDER BY NOMVEND");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            var colecao = new List<VENDEDOR>();

            while (idr.Read())
            {
                var vend = new VENDEDOR();
                vend.CODVEND = Convert.ToInt32(idr["CODVEND"]);
                vend.NOMVEND = Convert.ToString(idr["NOMVEND"]);
                colecao.Add(vend);
            }
            idr.Close();
            return colecao;
        }

        #endregion

        #region LOG

        private void GeraLogTrans(DbTransaction dbt, Database db, CLIENTEVA cli, int tiptra)
        {
            try
            {
                var cmd = db.GetStoredProcCommand("PROC_GRAVAR_TRANSACAO");
                db.AddInParameter(cmd, "CODCLI", DbType.Int32, cli.CODCLI);
                db.AddInParameter(cmd, "CODCRE", DbType.Int32, DBNull.Value);
                db.AddInParameter(cmd, "TIPTRA", DbType.Int32, tiptra);
                db.AddInParameter(cmd, "CODCRT", DbType.String, DBNull.Value);
                db.AddInParameter(cmd, "VALOR", DbType.Decimal, 0m);
                db.AddInParameter(cmd, "CPF", DbType.String, DBNull.Value);
                db.AddInParameter(cmd, "NUMDEP", DbType.Int16, 0);
                db.AddInParameter(cmd, "NUMCARGAVA", DbType.Int16, DBNull.Value);
                db.AddInParameter(cmd, "NUMFECCRE", DbType.Int16, DBNull.Value);
                db.AddInParameter(cmd, "DAD", DbType.Int16, DBNull.Value);
                db.AddInParameter(cmd, "ATV", DbType.String, DBNull.Value);
                db.AddInParameter(cmd, "ID_FUNC", DbType.Int16, FOperador.ID_FUNC);
                db.ExecuteNonQuery(cmd, dbt);
            }
            catch (Exception)
            {
                throw new Exception("Erro na geracao do LOG");
            }
        }

        #endregion

        #region Get Taxas Credenciado

        public List<MODTAXA> ConsultaTaxaCli(int codCli)
        {
            var taxaCli = new List<MODTAXA>();
            const string sql = "PROC_LER_TAXACLI";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODCLI", DbType.String, codCli);
            db.AddInParameter(cmd, "@SISTEMA", DbType.String, ConstantesSIL.SistemaPRE);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var taxa = new MODTAXA();
                taxa.COD = Convert.ToInt32(idr["CODCLI"]);
                taxa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxa.NOMTAXA = Convert.ToString(idr["NOMTAXA"]);
                taxa.VALOR = Convert.ToDecimal(idr["VALOR"]);
                taxa.VALORDEP = idr["VALORDEP"] == DBNull.Value ? 0 : Convert.ToDecimal(idr["VALORDEP"]);
                taxa.DTINC = Convert.ToDateTime(idr["DTINC"]);
                taxa.DTINICIO = Convert.ToDateTime(idr["DTINICIO"]);
                taxa.DIASPINICIO = Convert.ToInt32(idr["DIASPINICIO"]);
                taxa.DESTA = Convert.ToString(idr["DESTA"]);
                taxa.TAXAHAB = Convert.ToString(idr["TAXAHAB"]);
                taxaCli.Add(taxa);
            }
            idr.Close();
            return taxaCli;
        }

        public MODTAXA ConsultaCodTaxaCli(int codCli, int codTaxa)
        {
            var taxa = new MODTAXA();
            var sql = new StringBuilder();
            sql.AppendLine("SELECT C.CODCLI, C.CODTAXA, T.TIPTRA, C.VALOR, C.DTINC, ");
            sql.AppendLine("       C.DTINICIO, C.DIASPINICIO, C.TAXAHAB, T.TIPO, C.VALORDEP ");
            sql.AppendLine("FROM TAXACLIVA C ");
            sql.AppendLine("	INNER JOIN TAXAVA T ON T.CODTAXA = C.CODTAXA AND T.TIPO IN (1, 3) ");
            sql.AppendLine("WHERE C.CODCLI = @CODCLI AND C.CODTAXA = @CODTAXA ");
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODCLI", DbType.Int32, codCli);
            db.AddInParameter(cmd, "@CODTAXA", DbType.Int32, codTaxa);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                taxa.COD = Convert.ToInt32(idr["CODCLI"]);
                taxa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxa.TIPO = Convert.ToInt32(idr["TIPO"]);
                taxa.TIPTRA = Convert.ToInt32(idr["TIPTRA"]);
                taxa.VALOR = Convert.ToDecimal(idr["VALOR"]);
                if (idr["VALORDEP"] != DBNull.Value)
                    taxa.VALORDEP = Convert.ToDecimal(idr["VALORDEP"]);
                taxa.DTINC = Convert.ToDateTime(idr["DTINC"]);
                taxa.DTINICIO = Convert.ToDateTime(idr["DTINICIO"]);
                taxa.DIASPINICIO = Convert.ToInt32(idr["DIASPINICIO"]);
                taxa.TAXAHAB = Convert.ToString(idr["TAXAHAB"]);
            }
            idr.Close();
            return taxa;
        }

        public string ConsultaTaxa(int codTaxa)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT TIPO FROM TAXAVA");
            sql.AppendLine("WHERE CODTAXA = @CODTAXA ");
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODTAXA", DbType.Int32, codTaxa);
            var idr = db.ExecuteReader(cmd);
            var aplicacao = string.Empty;
            if (idr.Read())
            {
                aplicacao = Convert.ToInt32(idr["TIPO"]) == 2 ? "CREDENCIADO" :
                    (Convert.ToInt32(idr["TIPO"]) == 3 ? "CLIENTE" : "USUARIO");
            }
            idr.Close();
            return aplicacao;
        }

        #endregion

        #region Beneficio

        public List<BENEFCLI> GetBeneficiosCliente(string CODCLI)
        {
            try
            {
                var conn = new SqlConnection();
                conn.ConnectionString = BDTELENET;

                var sql = new StringBuilder();

                sql.AppendLine(" SELECT");
                sql.AppendLine(" CODCLI, CODBENEF, VALTIT, VALDEP, DTCARENCIA, COBCANC, SUBBENEF, PERSUB,  INDIVIDUAL, DTVINGENCIA, RENOVAUT ");
                sql.AppendLine(" FROM BENEFCLI C");

                sql.AppendLine(string.Format("WHERE CODCLI = {0}", CODCLI));

                var cmd = new SqlCommand(sql.ToString(), conn);
                conn.Open();

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                if (conn.State == ConnectionState.Open)
                    conn.Close();

                var beneficiosCliente = new List<BENEFCLI>();

                foreach (DataRow row in dt.Rows)
                {
                    beneficiosCliente.Add(new BENEFCLI()
                    {
                        CODCLI = row["CODCLI"].ToString().ToINT32(),
                        CODBENEF = row["CODBENEF"].ToString().ToINT32(),
                        VALTIT = row["VALTIT"].ToString().ToDECIMAL(),
                        VALDEP = row["VALDEP"].ToString().ToDECIMAL(),
                        DTCARENCIA = row["DTCARENCIA"].ToString().ToDATETIME().Value,
                        COBCANC = row["COBCANC"].ToString(),
                        SUBBENEF = row["SUBBENEF"].ToString(),
                        PERSUB = row["PERSUB"].ToString().ToINT32(),
                        INDIVIDUAL = row["INDIVIDUAL"].ToString(),
                        DTVINGENCIA = row["DTVINGENCIA"].ToString().ToDATETIME().Value,
                        RENOVAUT = row["RENOVAUT"].ToString()
                    });
                }

                return beneficiosCliente;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}