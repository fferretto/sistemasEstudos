using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using TELENET.SIL.PO;

namespace TELENET.SIL.DA
{
    class daTabelas
    {
        readonly string BDTELENET = string.Empty;
        readonly OPERADORA FOperador;

        public daTabelas(OPERADORA Operador)
        {
            FOperador = Operador;
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        #region STATUS

        public List<STATUS> ListaStatus()
        {
            var ColecaoStatus = new List<STATUS>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("STA, DESTA ");
            sql.AppendLine("FROM STATUS WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY DESTA ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var status = new STATUS();
                status.STA = idr["STA"].ToString();
                status.DESTA = Convert.ToString(idr["DESTA"]).Trim();

                ColecaoStatus.Add(status);
            }
            idr.Close();

            return ColecaoStatus;
        }

        public List<STATUS> ListaStatusUsu()
        {
            var ColecaoStatusUsu = new List<STATUS>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("STA, DESTA ");
            sql.AppendLine("FROM STATUS WITH (NOLOCK) ");
            sql.AppendLine("WHERE USUARIO = 'S'");
            sql.AppendLine("ORDER BY DESTA ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var status = new STATUS();
                status.STA = idr["STA"].ToString();
                status.DESTA = Convert.ToString(idr["DESTA"]).Trim();

                ColecaoStatusUsu.Add(status);
            }
            idr.Close();

            return ColecaoStatusUsu;
        }

        // "which" aqui é "CADUSUPRE" ou "CADUSUPOS"
        // para dizer se queremos pré ou pós.
        public List<STATUS> ListaStatusCadUsu(string which)
        {
            var ColecaoStatusUsu = new List<STATUS>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("STA, DESTA ");
            sql.AppendLine("FROM STATUS WITH (NOLOCK) ");
            sql.AppendLine("WHERE " + which + " = 'S'");
            sql.AppendLine("ORDER BY DESTA ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var status = new STATUS();
                status.STA = idr["STA"].ToString();
                status.DESTA = Convert.ToString(idr["DESTA"]).Trim();

                ColecaoStatusUsu.Add(status);
            }
            idr.Close();

            return ColecaoStatusUsu;
        }

        public List<STATUS> ListaStatusCred()
        {
            var ColecaoStatusCred = new List<STATUS>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("STA, DESTA ");
            sql.AppendLine("FROM STATUS WITH (NOLOCK) ");
            sql.AppendLine("WHERE CREDENCIADO = 'S'");
            sql.AppendLine("ORDER BY DESTA ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var status = new STATUS();
                status.STA = idr["STA"].ToString();
                status.DESTA = Convert.ToString(idr["DESTA"]).Trim();

                ColecaoStatusCred.Add(status);
            }
            idr.Close();

            return ColecaoStatusCred;
        }

        public List<STATUS> ListaStatusOper()
        {
            var ColecaoStatusOper = new List<STATUS>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("STA, DESTA ");
            sql.AppendLine("FROM STATUS WITH (NOLOCK) ");
            sql.AppendLine("WHERE STA IN ('00','01','02') ");
            sql.AppendLine("ORDER BY DESTA ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var status = new STATUS();
                status.STA = idr["STA"].ToString();
                status.DESTA = Convert.ToString(idr["DESTA"]).Trim();

                ColecaoStatusOper.Add(status);
            }
            idr.Close();

            return ColecaoStatusOper;
        }

        public List<STATUS> ListaStatusCli()
        {
            var ColecaoStatusCli = new List<STATUS>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("STA, DESTA ");
            sql.AppendLine("FROM STATUS WITH (NOLOCK) ");
            sql.AppendLine("WHERE CLIENTE = 'S'  ");
            sql.AppendLine("ORDER BY DESTA ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var status = new STATUS();
                status.STA = idr["STA"].ToString();
                status.DESTA = Convert.ToString(idr["DESTA"]).Trim();

                ColecaoStatusCli.Add(status);
            }
            idr.Close();

            return ColecaoStatusCli;
        }

        public STATUS GetStatus(string codSta)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT STA, DESTA");
            sql.AppendLine("FROM STATUS WITH (NOLOCK) ");
            sql.AppendLine("WHERE STA = @STA");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@STA", DbType.String, codSta);
            var idr = db.ExecuteReader(cmd);
            STATUS status = null;
            if (idr.Read())
            {
                status = new STATUS();
                status.DESTA = idr["DESTA"].ToString();
                status.STA = idr["STA"].ToString();
            }
            idr.Close();
            return status;
        }

        public STATUS GetStatusCli(string codSta)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT STA, DESTA");
            sql.AppendLine("FROM STATUS WITH (NOLOCK) ");
            sql.AppendLine("WHERE STA = @STA");
            sql.AppendLine(" AND CLIENTE = 'S'  ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@STA", DbType.String, codSta);
            var idr = db.ExecuteReader(cmd);
            STATUS status = null;
            if (idr.Read())
            {
                status = new STATUS();
                status.DESTA = idr["DESTA"].ToString();
                status.STA = idr["STA"].ToString();
            }
            idr.Close();
            return status;
        }

        public STATUS GetStatusByName(string desSta)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT STA, DESTA");
            sql.AppendLine("FROM STATUS WITH (NOLOCK) ");
            sql.AppendLine("WHERE DESTA = @DESTA");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@DESTA", DbType.String, desSta);
            var idr = db.ExecuteReader(cmd);
            STATUS status = null;
            if (idr.Read())
            {
                status = new STATUS();
                status.DESTA = idr["DESTA"].ToString();
                status.STA = idr["STA"].ToString();
            }
            idr.Close();
            return status;
        }

        #endregion

        #region TAXA

        public List<TAXAPJ> ListaTaxasCli(int sistema)
        {
            var ColecaoTaxa = new List<TAXAPJ>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODTAXA, NOMTAXA, TIPO ");
            if (sistema == 1)
            {
                sql.AppendLine("FROM TAXAVA WITH (NOLOCK) WHERE TIPO IN (1 ,3) AND TAXADEFAULT <> 'S' ");
            }
            else
            {
                sql.AppendLine("FROM TAXA WITH (NOLOCK) WHERE TIPO IN (1 ,3) AND TAXADEFAULT <> 'S' ");
            }
            sql.AppendLine("ORDER BY NOMTAXA ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var taxa = new TAXAPJ();
                taxa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxa.NOMTAXA = idr["NOMTAXA"].ToString();
                taxa.TIPO = Convert.ToInt32(idr["TIPO"]);
                ColecaoTaxa.Add(taxa);
            }
            idr.Close();

            return ColecaoTaxa;
        }

        public List<TAXAPJ> ListaTaxasCliComTaxaDefault(int sistema)
        {
            var ColecaoTaxa = new List<TAXAPJ>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODTAXA, NOMTAXA, TIPO ");
            if (sistema == 1)
            {
                sql.AppendLine("FROM TAXAVA WITH (NOLOCK) WHERE TIPO IN (1 ,3) ");
            }
            else
            {
                sql.AppendLine("FROM TAXA WITH (NOLOCK) WHERE TIPO IN (1 ,3) ");
            }
            sql.AppendLine("ORDER BY NOMTAXA ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var taxa = new TAXAPJ();
                taxa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxa.NOMTAXA = idr["NOMTAXA"].ToString();
                taxa.TIPO = Convert.ToInt32(idr["TIPO"]);
                ColecaoTaxa.Add(taxa);
            }
            idr.Close();

            return ColecaoTaxa;
        }

        public List<TAXAPJ> ListaTaxasCliPj()
        {
            var ColecaoTaxa = new List<TAXAPJ>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODTAXA, NOMTAXA, TIPO ");
            sql.AppendLine("FROM TAXA WITH (NOLOCK) WHERE TIPO IN (1 ,3)");
            sql.AppendLine("ORDER BY NOMTAXA ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var taxa = new TAXAPJ();
                taxa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxa.NOMTAXA = idr["NOMTAXA"].ToString();
                taxa.TIPO = Convert.ToInt32(idr["TIPO"]);
                ColecaoTaxa.Add(taxa);
            }
            idr.Close();

            return ColecaoTaxa;
        }

        public List<TAXAVA> ListaTaxasCre(int codCre)
        {
            var ColecaoTaxa = new List<TAXAVA>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("T.CODTAXA, T.NOMTAXA ");
            sql.AppendLine("FROM TAXAVA T WITH (NOLOCK) WHERE T.TIPO = 2 ");
            //sql.AppendLine(" AND (T.CENTRALIZADORA = 'N' OR ((SELECT CODCEN FROM CREDENCIADO WHERE CODCRE = @CODCRE) = @CODCRE))");
            sql.AppendLine("ORDER BY NOMTAXA ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODCRE", DbType.Int32, codCre);

            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var taxaVa = new TAXAVA();
                taxaVa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxaVa.NOMTAXA = idr["NOMTAXA"].ToString();
                ColecaoTaxa.Add(taxaVa);
            }
            idr.Close();

            return ColecaoTaxa;
        }

        public TAXAVA GetTaxa(int codTaxa)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODTAXA, NOMTAXA");
            sql.AppendLine("FROM TAXAVA WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODTAXA = @CODTAXA");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODTAXA", DbType.String, codTaxa);
            var idr = db.ExecuteReader(cmd);
            TAXAVA taxaVa = null;
            if (idr.Read())
            {
                taxaVa = new TAXAVA();
                taxaVa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxaVa.NOMTAXA = idr["NOMTAXA"].ToString();
            }
            idr.Close();

            return taxaVa;
        }

        public TAXAVA GetTaxaByName(string nomTaxa)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODTAXA, NOMTAXA");
            sql.AppendLine("FROM TAXAVA WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMTAXA = @NOMTAXA");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMTAXA", DbType.String, nomTaxa);
            var idr = db.ExecuteReader(cmd);
            TAXAVA taxaVa = null;
            if (idr.Read())
            {
                taxaVa = new TAXAVA();
                taxaVa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxaVa.NOMTAXA = idr["NOMTAXA"].ToString();
            }
            idr.Close();
            return taxaVa;
        }

        #endregion

        #region Agrupamento

        public List<AGRUPAMENTO> ListaAgrupamento()
        {
            var ColecaoAg = new List<AGRUPAMENTO>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODAG, NOMAG ");
            sql.AppendLine("FROM AGRUPAMENTO WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMAG ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var agrupamento = new AGRUPAMENTO();
                agrupamento.CODAG = Convert.ToInt32(idr["CODAG"]);
                agrupamento.NOMAG = idr["NOMAG"].ToString();
                ColecaoAg.Add(agrupamento);
            }
            idr.Close();
            return ColecaoAg;
        }

        public AGRUPAMENTO GetAgrupamento(int codAg)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODAG, NOMAG");
            sql.AppendLine("FROM AGRUPAMENTO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODAG = @CODAG");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODAG", DbType.Int32, codAg);
            var idr = db.ExecuteReader(cmd);
            AGRUPAMENTO agrupamento = null;
            if (idr.Read())
            {
                agrupamento = new AGRUPAMENTO();
                agrupamento.CODAG = Convert.ToInt32(idr["CODAG"]);
                agrupamento.NOMAG = idr["NOMAG"].ToString();
            }
            idr.Close();

            return agrupamento;
        }

        public AGRUPAMENTO GetAgrupamentoByName(string nomAg)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODAG, NOMAG");
            sql.AppendLine("FROM AGRUPAMENTO WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMAG = @NOMAG");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMAG", DbType.String, nomAg);
            var idr = db.ExecuteReader(cmd);
            AGRUPAMENTO agrupamento = null;
            if (idr.Read())
            {
                agrupamento = new AGRUPAMENTO();
                agrupamento.CODAG = Convert.ToInt32(idr["CODAG"]);
                agrupamento.NOMAG = idr["NOMAG"].ToString();
            }
            idr.Close();
            return agrupamento;
        }

        #endregion

        #region Parentesco

        public List<PARENTESCO> ListaParentesco()
        {
            var ColecaoParentesco = new List<PARENTESCO>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODPAR, DESPAR ");
            sql.AppendLine("FROM PARENTESCO WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY DESPAR ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var parentesco = new PARENTESCO();
                parentesco.CODPAR = Convert.ToInt16(idr["CODPAR"]);
                parentesco.DESPAR = idr["DESPAR"].ToString();
                ColecaoParentesco.Add(parentesco);
            }
            idr.Close();

            return ColecaoParentesco;
        }

        #endregion

        #region FORMA DE PAGAMENTO

        public List<REEMBOLSO> ListaForPag()
        {
            var ColecaoReembolso = new List<REEMBOLSO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("FORPAG, DESFORPAG ");
            sql.AppendLine("FROM REEMBOLSO WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY DESFORPAG ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var reembolso = new REEMBOLSO();
                reembolso.FORPAG = Convert.ToInt16(idr["FORPAG"]);
                reembolso.DESFORPAG = idr["DESFORPAG"].ToString();

                ColecaoReembolso.Add(reembolso);
            }
            idr.Close();

            return ColecaoReembolso;
        }

        public REEMBOLSO GetForPag(short forPag)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT FORPAG, DESFORPAG");
            sql.AppendLine("FROM REEMBOLSO WITH (NOLOCK) ");
            sql.AppendLine("WHERE FORPAG = @FORPAG");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@FORPAG", DbType.String, forPag);
            var idr = db.ExecuteReader(cmd);
            REEMBOLSO local = null;
            if (idr.Read())
            {
                local = new REEMBOLSO();
                local.FORPAG = Convert.ToInt16(idr["FORPAG"]);
                local.DESFORPAG = idr["DESFORPAG"].ToString();
            }
            idr.Close();
            return local;
        }

        public REEMBOLSO GetForPagByName(string desForPag)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT FORPAG, DESFORPAG");
            sql.AppendLine("FROM REEMBOLSO WITH (NOLOCK) ");
            sql.AppendLine("WHERE DESFORPAG = @DESFORPAG");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMUF0", DbType.String, desForPag);
            var idr = db.ExecuteReader(cmd);
            REEMBOLSO uf = null;
            if (idr.Read())
            {
                uf = new REEMBOLSO();
                uf.FORPAG = Convert.ToInt16(idr["FORPAG"]);
                uf.DESFORPAG = idr["DESFORPAG"].ToString();
            }
            idr.Close();
            return uf;
        }

        #endregion

        #region UF

        public List<UF> ListaUF()
        {
            var ColecaoUF = new List<UF>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("SIGUF0, NOMUF0 ");
            sql.AppendLine("FROM UF WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMUF0 ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var Uf = new UF();
                Uf.SIGUF0 = idr["SIGUF0"].ToString();
                Uf.NOMUF0 = Convert.ToString(idr["NOMUF0"]).Trim();

                ColecaoUF.Add(Uf);
            }
            idr.Close();

            return ColecaoUF;
        }

        public UF GetUF(string codUF)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT SIGUF0, NOMUF0");
            sql.AppendLine("FROM UF WITH (NOLOCK) ");
            sql.AppendLine("WHERE SIGUF0 = @SIGUF0");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@SIGUF0", DbType.String, codUF);
            var idr = db.ExecuteReader(cmd);
            UF local = null;
            if (idr.Read())
            {
                local = new UF();
                local.NOMUF0 = idr["NOMUF0"].ToString();
                local.SIGUF0 = idr["SIGUF0"].ToString();
            }
            idr.Close();
            return local;
        }

        public UF GetUFByName(string nomUF)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT SIGUF0, NOMUF0");
            sql.AppendLine("FROM UF WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMUF0 = @NOMUF0");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMUF0", DbType.String, nomUF);
            var idr = db.ExecuteReader(cmd);
            UF uf = null;
            if (idr.Read())
            {
                uf = new UF();
                uf.NOMUF0 = idr["NOMUF0"].ToString();
                uf.SIGUF0 = idr["SIGUF0"].ToString();
            }
            idr.Close();
            return uf;
        }

        #endregion

        #region REGIAO

        public List<REGIAO> ListarRegiao()
        {
            var ColecaoRegiao = new List<REGIAO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODREG, DESREG ");
            sql.AppendLine("FROM REGIAO WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY DESREG ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var regiao = new REGIAO();
                regiao.CODREG = Convert.ToInt32(idr["CODREG"]);
                regiao.DESREG = Convert.ToString(idr["DESREG"]).Trim();

                ColecaoRegiao.Add(regiao);
            }
            idr.Close();

            return ColecaoRegiao;
        }

        public int ProximoCodigoRegiaoLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ISNULL(MIN(CODREG)+1, 1) AS ProxCod");
            sql.AppendLine("FROM REGIAO WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODREG + 1) NOT IN (SELECT CODREG FROM REGIAO WITH (NOLOCK) )");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteRegiaoCadastrada(REGIAO Regiao, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODREG FROM REGIAO WITH (NOLOCK) WHERE DESREG = @DESREG";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "DESREG", DbType.String, Regiao.DESREG);
            var CodReg = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodReg != 0);
        }

        private static bool ExisteRegiaoAssociada(REGIAO Regiao, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODREG FROM CLIENTEVA WITH (NOLOCK) WHERE CODREG = @CODREG " +
                               "UNION " +
                               "SELECT TOP 1 CODREG FROM CREDENCIADO WITH (NOLOCK) WHERE CODREG = @CODREG";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODREG", DbType.String, Regiao.CODREG);
            var CodReg = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodReg != 0);
        }

        public REGIAO GetRegiao(int codReg)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODREG, DESREG");
            sql.AppendLine("FROM REGIAO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODREG = @CODREG");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODREG", DbType.String, codReg);
            var idr = db.ExecuteReader(cmd);
            REGIAO regiao = null;
            if (idr.Read())
            {
                regiao = new REGIAO();
                regiao.DESREG = idr["DESREG"].ToString();
                regiao.CODREG = Convert.ToInt32(idr["CODREG"]);
            }
            idr.Close();
            return regiao;
        }

        public REGIAO GetRegiaoByName(string nomReg)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODREG, DESREG");
            sql.AppendLine("FROM REGIAO WITH (NOLOCK) ");
            sql.AppendLine("WHERE DESREG = @DESREG");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@DESREG", DbType.String, nomReg);
            var idr = db.ExecuteReader(cmd);
            REGIAO regiao = null;
            if (idr.Read())
            {
                regiao = new REGIAO();
                regiao.DESREG = idr["DESREG"].ToString();
                regiao.CODREG = Convert.ToInt32(idr["CODREG"]);
            }
            idr.Close();
            return regiao;
        }

        public void InserirRegiao(REGIAO Regiao)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se Regiao ja Existe
                if (ExisteRegiaoCadastrada(Regiao, db, dbt))
                    throw new Exception("Regiao jA existe. Favor verificar.");

                const string sql = "INSERT INTO REGIAO (CODREG, DESREG) VALUES (@CODREG, @DESREG)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODREG", DbType.Int32, ProximoCodigoRegiaoLivre());
                db.AddInParameter(cmd, "DESREG", DbType.String, Regiao.DESREG);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT REGIAO", FOperador, cmd);
                dbt.Commit();

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

        public void AlterarRegiao(REGIAO Regiao)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();

            try
            {
                const string sql = "UPDATE REGIAO SET DESREG = @DESREG WHERE CODREG = @CODREG";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODREG", DbType.Int32, Regiao.CODREG);
                db.AddInParameter(cmd, "DESREG", DbType.String, Regiao.DESREG);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE REGIAO", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirRegiao(REGIAO Regiao)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteRegiaoAssociada(Regiao, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE REGIAO WHERE CODREG = @CODREG";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODREG", DbType.Int32, Regiao.CODREG);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE REGIAO", FOperador, cmd);
                dbt.Commit();
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

        #region REGIONAL

        public List<REGIONAL> ListarRegional()
        {
            var ColecaoRegional = new List<REGIONAL>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODREO, DESREO ");
            sql.AppendLine("FROM REGIONAL WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY DESREO ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var regional = new REGIONAL();
                regional.CODREO = Convert.ToInt32(idr["CODREO"]);
                regional.DESREO = Convert.ToString(idr["DESREO"]).Trim();

                ColecaoRegional.Add(regional);
            }
            idr.Close();

            return ColecaoRegional;
        }

        public int ProximoCodigoRegionalLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ISNULL(MIN(CODREO)+1, 1) AS ProxCod");
            sql.AppendLine("FROM REGIONAL WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODREO + 1) NOT IN (SELECT CODREO FROM REGIONAL WITH (NOLOCK) )");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteRegionalCadastrada(REGIONAL Regional, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODREO FROM REGIONAL WITH (NOLOCK) WHERE DESREO = @DESREO";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "DESREO", DbType.String, Regional.DESREO);
            var CodReo = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodReo != 0);
        }

        private static bool ExisteRegionalAssociada(REGIONAL Regional, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODREO FROM CLIENTEVA WITH (NOLOCK) WHERE CODREO = @CODREO " +
                               "UNION " +
                               "SELECT TOP 1 CODREO FROM CREDENCIADO WITH (NOLOCK) WHERE CODREO = @CODREO";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODREO", DbType.String, Regional.CODREO);
            var CodReo = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodReo != 0);
        }

        public REGIONAL GetRegional(int codReo)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODREO, DESREO");
            sql.AppendLine("FROM REGIONAL WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODREO = @CODREO");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODREO", DbType.String, codReo);
            var idr = db.ExecuteReader(cmd);
            REGIONAL regional = null;
            if (idr.Read())
            {
                regional = new REGIONAL();
                regional.DESREO = idr["DESREO"].ToString();
                regional.CODREO = Convert.ToInt32(idr["CODREO"]);
            }
            idr.Close();
            return regional;
        }

        public REGIONAL GetRegionalByName(string nomReo)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODREO, DESREO");
            sql.AppendLine("FROM REGIONAL WITH (NOLOCK) ");
            sql.AppendLine("WHERE DESREO = @DESREO");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@DESREO", DbType.String, nomReo);
            var idr = db.ExecuteReader(cmd);
            REGIONAL regional = null;
            if (idr.Read())
            {
                regional = new REGIONAL();
                regional.DESREO = idr["DESREO"].ToString();
                regional.CODREO = Convert.ToInt32(idr["CODREO"]);
            }
            idr.Close();
            return regional;
        }

        public void InserirRegional(REGIONAL Regional)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se Regiao ja Existe
                if (ExisteRegionalCadastrada(Regional, db, dbt))
                    throw new Exception("Regional já existe. Favor verificar.");

                const string sql = "INSERT INTO REGIONAL (CODREO, DESREO) VALUES (@CODREO, @DESREO)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODREO", DbType.Int32, ProximoCodigoRegionalLivre());
                db.AddInParameter(cmd, "DESREO", DbType.String, Regional.DESREO);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT REGIONAL", FOperador, cmd);
                dbt.Commit();

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

        public void AlterarRegional(REGIONAL Regional)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();

            try
            {
                const string sql = "UPDATE REGIONAL SET DESREO = @DESREO WHERE CODREO = @CODREO";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODREO", DbType.Int32, Regional.CODREO);
                db.AddInParameter(cmd, "DESREO", DbType.String, Regional.DESREO);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE REGIONAL", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirRegional(REGIONAL Regional)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteRegionalAssociada(Regional, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE REGIONAL WHERE CODREO = @CODREO";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODREO", DbType.Int32, Regional.CODREO);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE REGIONAL", FOperador, cmd);
                dbt.Commit();
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

        #region CREDENCIADOR

        public List<EPS> ListarCredenciador(string tipEps, int codReo)
        {
            var ColecaoCredenciador = new List<EPS>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODREO, CODEPS, NOMEPS ");
            sql.AppendLine("FROM EPS WITH (NOLOCK) ");
            sql.AppendLine("WHERE TIPEPS = @TIPEPS AND CODREO = @CODREO ");
            sql.AppendLine("ORDER BY NOMEPS ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@TIPEPS", DbType.String, tipEps);
            db.AddInParameter(cmd, "@CODREO", DbType.Int32, codReo);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var credenciador = new EPS();
                credenciador.CODREO = Convert.ToInt32(idr["CODREO"]);
                credenciador.CODEPS = Convert.ToInt32(idr["CODEPS"]);
                credenciador.NOMEPS = Convert.ToString(idr["NOMEPS"]).Trim();

                ColecaoCredenciador.Add(credenciador);
            }
            idr.Close();

            return ColecaoCredenciador;
        }

        public int ProximoCodigoCredenciadorLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ISNULL(MIN(CODEPS)+1, 1) AS ProxCod");
            sql.AppendLine("FROM EPS WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODEPS + 1) NOT IN (SELECT CODEPS FROM EPS WITH (NOLOCK) )");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteCredenciadorCadastrado(string nomEps, int codReo, string tipEps, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODEPS FROM EPS WITH (NOLOCK) WHERE NOMEPS = @NOMEPS AND CODREO = @CODREO AND TIPEPS = @TIPEPS ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODREO", DbType.Int32, codReo);
            db.AddInParameter(cmd, "TIPEPS", DbType.String, tipEps);
            db.AddInParameter(cmd, "NOMEPS", DbType.String, nomEps);
            var CodEps = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodEps != 0);
        }

        private static bool ExisteCredenciadorAssociada(int CODEPS, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODEPS FROM CLIENTEVA WITH (NOLOCK) WHERE CODEPS = @CODEPS " +
                               "UNION " +
                               "SELECT TOP 1 CODEPS FROM CREDENCIADO WITH (NOLOCK) WHERE CODEPS = @CODEPS";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODEPS", DbType.String, CODEPS);
            var CodReg = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodReg != 0);
        }

        public EPS GetCredenciador(int codReo, int codEps)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODEPS, NOMEPS");
            sql.AppendLine("FROM EPS WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODREO = @CODREO AND CODEPS = @CODEPS");
            sql.AppendLine("AND TIPEPS = 'C'");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODREO", DbType.String, codReo);
            db.AddInParameter(cmd, "@CODEPS", DbType.String, codEps);
            var idr = db.ExecuteReader(cmd);
            EPS credenciador = null;
            if (idr.Read())
            {
                credenciador = new EPS();
                credenciador.NOMEPS = idr["NOMEPS"].ToString();
                credenciador.CODEPS = Convert.ToInt32(idr["CODEPS"]);
            }
            idr.Close();
            return credenciador;
        }

        public EPS GetCredenciadorComTipeps(int codEps)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODEPS, NOMEPS");
            sql.AppendLine("FROM EPS WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODEPS = @CODEPS");
            sql.AppendLine("AND TIPEPS = 'C'");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODEPS", DbType.String, codEps);
            var idr = db.ExecuteReader(cmd);
            EPS credenciador = null;
            if (idr.Read())
            {
                credenciador = new EPS();
                credenciador.NOMEPS = idr["NOMEPS"].ToString();
                credenciador.CODEPS = Convert.ToInt32(idr["CODEPS"]);
            }
            idr.Close();
            return credenciador;
        }

        public EPS GetCredenciadorByName(string nomEps, int codReo)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODEPS, NOMEPS ");
            sql.AppendLine("FROM EPS WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMEPS = @NOMEPS ");
            sql.AppendLine("AND TIPEPS = 'C' ");
            sql.AppendLine("AND CODREO = @CODREO ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMEPS", DbType.String, nomEps);
            db.AddInParameter(cmd, "@CODREO", DbType.String, codReo);
            var idr = db.ExecuteReader(cmd);
            EPS credenciador = null;
            if (idr.Read())
            {
                credenciador = new EPS();
                credenciador.NOMEPS = idr["NOMEPS"].ToString();
                credenciador.CODEPS = Convert.ToInt32(idr["CODEPS"]);
            }
            idr.Close();
            return credenciador;
        }

        public EPS GetVendedorByName(string nomEps, int codReo)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODEPS, NOMEPS ");
            sql.AppendLine("FROM EPS WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMEPS = @NOMEPS ");
            sql.AppendLine("AND TIPEPS = 'V' ");
            sql.AppendLine("AND CODREO = @CODREO ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMEPS", DbType.String, nomEps);
            db.AddInParameter(cmd, "@CODREO", DbType.String, codReo);
            var idr = db.ExecuteReader(cmd);
            EPS credenciador = null;
            if (idr.Read())
            {
                credenciador = new EPS();
                credenciador.NOMEPS = idr["NOMEPS"].ToString();
                credenciador.CODEPS = Convert.ToInt32(idr["CODEPS"]);
            }
            idr.Close();
            return credenciador;
        }

        public void InserirCredenciador(string tipEps, int codReo, string NOMEPS)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se Regiao ja Existe
                if (ExisteCredenciadorCadastrado(NOMEPS, codReo, tipEps, db, dbt))
                    throw new Exception("O nome informado já existe nesta regional. Favor verificar.");

                const string sql = "INSERT INTO EPS (CODREO, CODEPS, NOMEPS, TIPEPS) VALUES (@CODREO, @CODEPS, @NOMEPS, @TIPEPS)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "TIPEPS", DbType.String, tipEps);
                db.AddInParameter(cmd, "CODREO", DbType.Int32, codReo);
                db.AddInParameter(cmd, "CODEPS", DbType.Int32, ProximoCodigoCredenciadorLivre());
                db.AddInParameter(cmd, "NOMEPS", DbType.String, NOMEPS);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT EPS", FOperador, cmd);
                dbt.Commit();

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

        public void AlterarCredenciador(int CODEPS, string NOMEPS)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();

            try
            {
                const string sql = "UPDATE EPS SET NOMEPS = @NOMEPS WHERE CODEPS = @CODEPS";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODEPS", DbType.Int32, CODEPS);
                db.AddInParameter(cmd, "NOMEPS", DbType.String, NOMEPS);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE EPS", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirCredenciador(int CODEPS)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteCredenciadorAssociada(CODEPS, db, dbt))
                    throw new Exception("Não e possível excluir registros que já foram usados em outros cadastros.");
                const string sql = "DELETE EPS WHERE CODEPS = @CODEPS";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODEPS", DbType.Int32, CODEPS);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE EPS", FOperador, cmd);
                dbt.Commit();
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

        #region ATIVIDADE

        public List<ATIVIDADE> ListarAtividade()
        {
            var ColecaoAtividade = new List<ATIVIDADE>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODATI, NOMATI ");
            sql.AppendLine("FROM ATIVIDADE WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMATI ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var atividade = new ATIVIDADE();
                atividade.CODATI = Convert.ToInt32(idr["CODATI"]);
                atividade.NOMATI = Convert.ToString(idr["NOMATI"]).Trim();

                ColecaoAtividade.Add(atividade);
            }
            idr.Close();

            return ColecaoAtividade;
        }

        public int ProximoCodigoAtividadeLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODATI)+1, 1) AS ProxCod");
            sql.AppendLine("FROM Atividade WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODATI + 1) NOT IN (SELECT CODATI FROM Atividade WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteAtividadeCadastrada(ATIVIDADE Atividade, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODATI FROM Atividade WITH (NOLOCK) WHERE NOMATI = @NOMATI";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMATI", DbType.String, Atividade.NOMATI);
            var CodATI = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodATI != 0);
        }

        private static bool ExisteAtividadeAssociada(ATIVIDADE Atividade, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODATI FROM CLIENTEVA WITH (NOLOCK) WHERE CODATI = @CODATI " +
                               "UNION " +
                               "SELECT TOP 1 CODATI FROM CREDENCIADO WITH (NOLOCK) WHERE CODATI = @CODATI";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODATI", DbType.String, Atividade.CODATI);
            var CodATI = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodATI != 0);
        }

        public ATIVIDADE GetAtividade(int codAti)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODATI, NOMATI");
            sql.AppendLine("FROM ATIVIDADE WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODATI = @CODATI");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODATI", DbType.String, codAti);
            var idr = db.ExecuteReader(cmd);
            ATIVIDADE atividade = null;
            if (idr.Read())
            {
                atividade = new ATIVIDADE();
                atividade.NOMATI = idr["NOMATI"].ToString();
                atividade.CODATI = Convert.ToInt32(idr["CODATI"]);
            }
            idr.Close();
            return atividade;
        }

        public ATIVIDADE GetAtividadeByName(string nomAti)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODATI, NOMATI");
            sql.AppendLine("FROM ATIVIDADE WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMATI = @NOMATI");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMATI", DbType.String, nomAti);
            var idr = db.ExecuteReader(cmd);
            ATIVIDADE atividade = null;
            if (idr.Read())
            {
                atividade = new ATIVIDADE();
                atividade.NOMATI = idr["NOMATI"].ToString();
                atividade.CODATI = Convert.ToInt32(idr["CODATI"]);
            }
            idr.Close();
            return atividade;
        }

        public void InserirAtividade(ATIVIDADE Atividade)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se Atividade ja Existe
                if (ExisteAtividadeCadastrada(Atividade, db, dbt))
                    throw new Exception("Atividade ja existe. Favor verificar.");
                const string sql = "INSERT INTO Atividade (CODATI, NOMATI) VALUES (@CODATI, @NOMATI)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODATI", DbType.Int32, ProximoCodigoAtividadeLivre());
                db.AddInParameter(cmd, "NOMATI", DbType.String, Atividade.NOMATI);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT Atividade", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarAtividade(ATIVIDADE Atividade)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE Atividade SET NOMATI = @NOMATI WHERE CODATI = @CODATI";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODATI", DbType.Int32, Atividade.CODATI);
                db.AddInParameter(cmd, "NOMATI", DbType.String, Atividade.NOMATI);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE Atividade", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirAtividade(ATIVIDADE Atividade)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteAtividadeAssociada(Atividade, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE Atividade WHERE CODATI = @CODATI";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODATI", DbType.Int32, Atividade.CODATI);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE Atividade", FOperador, cmd);
                dbt.Commit();
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

        #region SEGMENTO

        public SEGMENTO GetSegmentoByName(string nomSeg)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODSEG, NOMSEG, TIPO");
            sql.AppendLine("FROM SEGMENTO WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMSEG = @NOMSEG");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMSEG", DbType.String, nomSeg);
            var idr = db.ExecuteReader(cmd);

            SEGMENTO seg = null;
            if (idr.Read())
            {
                seg = new SEGMENTO();
                seg.NOMSEG = idr["NOMSEG"].ToString();
                seg.CODSEG = Convert.ToInt32(idr["CODSEG"]);
                seg.TIPO = Convert.ToInt32(idr["TIPO"]);
            }
            idr.Close();
            return seg;
        }

        public List<SEGMENTO> ListarSegmento()
        {
            var ColecaoSegmento = new List<SEGMENTO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODSEG, NOMSEG, TIPO ");
            sql.AppendLine("FROM SEGMENTO WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMSEG ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var segmento = new SEGMENTO();
                segmento.CODSEG = Convert.ToInt32(idr["CODSEG"]);
                segmento.NOMSEG = Convert.ToString(idr["NOMSEG"]).Trim();
                segmento.TIPO = Convert.ToInt32(idr["TIPO"]);
                ColecaoSegmento.Add(segmento);
            }
            idr.Close();

            return ColecaoSegmento;
        }

        public List<SEGMENTO> ListarSegmento(string cnpj)
        {
            var ColecaoSegmento = new List<SEGMENTO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODSEG, NOMSEG, TIPO ");
            sql.AppendLine("FROM SEGMENTO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODSEG NOT IN (SELECT CODSEG FROM CREDENCIADO WITH (NOLOCK) WHERE CGC = @CGC)");
            sql.AppendLine("ORDER BY NOMSEG ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CGC", DbType.String, cnpj);

            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var segmento = new SEGMENTO();
                segmento.CODSEG = Convert.ToInt32(idr["CODSEG"]);
                segmento.NOMSEG = Convert.ToString(idr["NOMSEG"]).Trim();
                segmento.TIPO = Convert.ToInt32(idr["TIPO"]);
                ColecaoSegmento.Add(segmento);
            }
            idr.Close();

            return ColecaoSegmento;
        }

        public int ProximoCodigoSegmentoLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODSEG)+1, 1) AS ProxCod");
            sql.AppendLine("FROM Segmento WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODSEG + 1) NOT IN (SELECT CODSEG FROM Segmento WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteSegmentoCadastrado(SEGMENTO Segmento, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODSEG FROM Segmento WITH (NOLOCK) WHERE NOMSEG = @NOMSEG";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMSEG", DbType.String, Segmento.NOMSEG);
            var CODSEG = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODSEG != 0);
        }

        private static bool ExisteSegmentoAssociado(SEGMENTO Segmento, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODSEG FROM SEGAUTORIZVA WITH (NOLOCK) WHERE CODSEG = @CODSEG " +
                               "UNION " +
                               "SELECT TOP 1 CODSEG FROM SEGAUTORIZ WITH (NOLOCK) WHERE CODSEG = @CODSEG " +
                               "UNION " +
                               "SELECT TOP 1 CODSEG FROM CREDENCIADO WITH (NOLOCK) WHERE CODSEG = @CODSEG";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODSEG", DbType.String, Segmento.CODSEG);
            var CODSEG = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODSEG != 0);
        }

        public SEGMENTO GetSegmento(int codSeg)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODSEG, NOMSEG, TIPO ");
            sql.AppendLine("FROM SEGMENTO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODSEG = @CODSEG");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODSEG", DbType.String, codSeg);
            var idr = db.ExecuteReader(cmd);
            SEGMENTO segmento = null;
            if (idr.Read())
            {
                segmento = new SEGMENTO();
                segmento.NOMSEG = idr["NOMSEG"].ToString();
                segmento.CODSEG = Convert.ToInt32(idr["CODSEG"]);
                segmento.TIPO = Convert.ToInt32(idr["TIPO"]);
            }
            idr.Close();
            return segmento;
        }

        public void InserirSegmento(SEGMENTO segmento)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se Segmento ja Existe
                if (ExisteSegmentoCadastrado(segmento, db, dbt))
                    throw new Exception("Segmento ja existe. Favor verificar.");
                const string sql = "INSERT INTO Segmento (CODSEG, NOMSEG, TIPO) VALUES (@CODSEG, @NOMSEG, @TIPO)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODSEG", DbType.Int32, ProximoCodigoSegmentoLivre());
                db.AddInParameter(cmd, "NOMSEG", DbType.String, segmento.NOMSEG);
                db.AddInParameter(cmd, "TIPO", DbType.String, segmento.TIPO);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT Segmento", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarSegmento(SEGMENTO Segmento)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE Segmento SET NOMSEG = @NOMSEG, TIPO = @TIPO WHERE CODSEG = @CODSEG";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODSEG", DbType.Int32, Segmento.CODSEG);
                db.AddInParameter(cmd, "NOMSEG", DbType.String, Segmento.NOMSEG);
                db.AddInParameter(cmd, "TIPO", DbType.String, Segmento.TIPO);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE Segmento", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirSegmento(SEGMENTO Segmento)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteSegmentoAssociado(Segmento, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE Segmento WHERE CODSEG = @CODSEG";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODSEG", DbType.Int32, Segmento.CODSEG);
                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE Segmento", FOperador, cmd);
                dbt.Commit();
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

        public List<SEG> ListarSeg()
        {
            var ColecaoSegmento = new List<SEG>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODSEG, NOMSEG ");
            sql.AppendLine("FROM SEGMENTO WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMSEG ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var segmento = new SEG();
                segmento.CODSEG = Convert.ToInt32(idr["CODSEG"]);
                segmento.NOMSEG = Convert.ToString(idr["NOMSEG"]).Trim();
                ColecaoSegmento.Add(segmento);
            }
            idr.Close();

            return ColecaoSegmento;
        }

        private static bool ExisteSegCadastrado(SEG Segmento, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODSEG FROM Segmento WITH (NOLOCK) WHERE NOMSEG = @NOMSEG";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMSEG", DbType.String, Segmento.NOMSEG);
            var CODSEG = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODSEG != 0);
        }

        private static bool ExisteSegAssociado(SEG Segmento, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODSEG FROM SEGAUTORIZVA WITH (NOLOCK) WHERE CODSEG = @CODSEG " +
                               "UNION " +
                               "SELECT TOP 1 CODSEG FROM SEGAUTORIZ WITH (NOLOCK) WHERE CODSEG = @CODSEG " +
                               "UNION " +
                               "SELECT TOP 1 CODSEG FROM CREDENCIADO WITH (NOLOCK) WHERE CODSEG = @CODSEG";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODSEG", DbType.String, Segmento.CODSEG);
            var CODSEG = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODSEG != 0);
        }

        public void InserirSeg(SEG segmento)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se Segmento ja Existe
                if (ExisteSegCadastrado(segmento, db, dbt))
                    throw new Exception("Segmento ja existe. Favor verificar.");
                const string sql = "INSERT INTO Segmento (CODSEG, NOMSEG) VALUES (@CODSEG, @NOMSEG)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODSEG", DbType.Int32, ProximoCodigoSegmentoLivre());
                db.AddInParameter(cmd, "NOMSEG", DbType.String, segmento.NOMSEG);
                
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT Segmento", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarSeg(SEG Segmento)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE Segmento SET NOMSEG = @NOMSEG WHERE CODSEG = @CODSEG";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODSEG", DbType.Int32, Segmento.CODSEG);
                db.AddInParameter(cmd, "NOMSEG", DbType.String, Segmento.NOMSEG);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE Segmento", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirSeg(SEG Segmento)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteSegAssociado(Segmento, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE Segmento WHERE CODSEG = @CODSEG";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODSEG", DbType.Int32, Segmento.CODSEG);
                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE Segmento", FOperador, cmd);
                dbt.Commit();
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

        #region BAIRRO

        public List<BAIRRO> ListarBairro()
        {
            var ColecaoBairro = new List<BAIRRO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODBAI, NOMBAI ");
            sql.AppendLine("FROM BAIRRO WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMBAI ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var bairro = new BAIRRO();
                bairro.CODBAI = Convert.ToInt32(idr["CODBAI"]);
                bairro.NOMBAI = Convert.ToString(idr["NOMBAI"]).Trim();

                ColecaoBairro.Add(bairro);
            }
            idr.Close();

            return ColecaoBairro;
        }

        public int ProximoCodigoBairroLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODBAI)+1, 1) AS ProxCod");
            sql.AppendLine("FROM Bairro WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODBAI + 1) NOT IN (SELECT CODBAI FROM Bairro WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteBairroCadastrado(BAIRRO Bairro, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODBAI FROM Bairro WITH (NOLOCK) WHERE NOMBAI = @NOMBAI";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMBAI", DbType.String, Bairro.NOMBAI);
            var CODBAI = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODBAI != 0);
        }

        private static bool ExisteBairroAssociado(BAIRRO Bairro, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODBAI FROM CLIENTEVA WITH (NOLOCK) WHERE (CODBAI = @CODBAI) OR (CODBAIEDC = @CODBAI)" +
                               "UNION " +
                               "SELECT TOP 1 CODBAI FROM CLIENTE WITH (NOLOCK) WHERE (CODBAI = @CODBAI) OR (CODBAIEDC = @CODBAI) " +
                               "UNION " +
                               "SELECT TOP 1 CODBAI FROM CREDENCIADO WITH (NOLOCK) WHERE (CODBAI = @CODBAI) OR (CODBAICOR = @CODBAI) " +
                               "UNION " +
                               "SELECT TOP 1 CODBAI FROM USUARIO WITH (NOLOCK) WHERE (CODBAI = @CODBAI) ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODBAI", DbType.String, Bairro.CODBAI);
            var CodBairro = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodBairro != 0);
        }

        public BAIRRO GetBairro(int codBai)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODBAI, NOMBAI");
            sql.AppendLine("FROM BAIRRO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODBAI = @CODBAI");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODBAI", DbType.String, codBai);
            var idr = db.ExecuteReader(cmd);
            BAIRRO bairro = null;
            if (idr.Read())
            {
                bairro = new BAIRRO();
                bairro.NOMBAI = idr["NOMBAI"].ToString();
                bairro.CODBAI = Convert.ToInt32(idr["CODBAI"]);
            }
            idr.Close();
            return bairro;
        }

        public BAIRRO GetBairroByName(string nomBair)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODBAI, NOMBAI");
            sql.AppendLine("FROM BAIRRO WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMBAI = @NOMBAI");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMBAI", DbType.String, nomBair);
            var idr = db.ExecuteReader(cmd);
            BAIRRO bairro = null;
            if (idr.Read())
            {
                bairro = new BAIRRO();
                bairro.NOMBAI = idr["NOMBAI"].ToString();
                bairro.CODBAI = Convert.ToInt32(idr["CODBAI"]);
            }
            idr.Close();
            return bairro;
        }

        public void InserirBairro(BAIRRO Bairro)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se Bairro ja Existe
                if (ExisteBairroCadastrado(Bairro, db, dbt))
                    throw new Exception("Bairro ja existe. Favor verificar.");
                const string sql = "INSERT INTO Bairro (CODBAI, NOMBAI) VALUES (@CODBAI, @NOMBAI)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODBAI", DbType.Int32, ProximoCodigoBairroLivre());
                db.AddInParameter(cmd, "NOMBAI", DbType.String, Bairro.NOMBAI);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT Bairro", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarBairro(BAIRRO Bairro)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE Bairro SET NOMBAI = @NOMBAI WHERE CODBAI = @CODBAI";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODBAI", DbType.Int32, Bairro.CODBAI);
                db.AddInParameter(cmd, "NOMBAI", DbType.String, Bairro.NOMBAI);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE Bairro", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirBairro(BAIRRO Bairro)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteBairroAssociado(Bairro, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE Bairro WHERE CODBAI = @CODBAI";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODBAI", DbType.Int32, Bairro.CODBAI);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE Bairro", FOperador, cmd);
                dbt.Commit();
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

        #region SUBREDE

        public List<SUBREDE> ListarSubrede()
        {
            var ColecaoSubrede = new List<SUBREDE>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODSUBREDE, NOMSUBREDE, RAZAOSOCIAL, CNPJ, CEP, LOGRADOURO, NROLOGRADOURO, COMPLEMENTO, BAIRRO, UF, CIDADE ");
            sql.AppendLine("FROM SUBREDE WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMSUBREDE ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var subrede = new SUBREDE();
                subrede.CODSUBREDE = Convert.ToInt32(idr["CODSUBREDE"]);
                subrede.NOMSUBREDE = Convert.ToString(idr["NOMSUBREDE"]).Trim();
                subrede.CNPJ = Convert.ToString(idr["CNPJ"]).Trim();
                subrede.RAZAOSOCIAL = Convert.ToString(idr["RAZAOSOCIAL"]).Trim();
                subrede.CEP = Convert.ToString(idr["CEP"]).Trim();
                subrede.LOGRADOURO = Convert.ToString(idr["LOGRADOURO"]).Trim();
                subrede.NROLOGRADOURO = Convert.ToString(idr["NROLOGRADOURO"]).Trim();
                subrede.COMPLEMENTO = Convert.ToString(idr["COMPLEMENTO"]).Trim();
                subrede.BAIRRO = Convert.ToString(idr["BAIRRO"]).Trim();
                subrede.CIDADE = Convert.ToString(idr["CIDADE"]).Trim();
                subrede.UF = Convert.ToString(idr["UF"]).Trim();

                ColecaoSubrede.Add(subrede);
            }
            idr.Close();

            return ColecaoSubrede;
        }

        public List<REDECAPTURA> ListarRedeCaptura()
        {
            var ColecaoRedeCaptura = new List<REDECAPTURA>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("REDE, NOME ");
            sql.AppendLine("FROM TABREDE WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOME ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var redeCaptura = new REDECAPTURA();
                redeCaptura.REDE = Convert.ToString(idr["REDE"]);
                redeCaptura.NOME = Convert.ToString(idr["NOME"]).Trim();

                ColecaoRedeCaptura.Add(redeCaptura);
            }
            idr.Close();

            return ColecaoRedeCaptura;
        }

        public int ProximoCodigoSubRedeLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODSUBREDE)+1, 1) AS ProxCod");
            sql.AppendLine("FROM SubRede WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODSUBREDE + 1) NOT IN (SELECT CODSUBREDE FROM SubRede WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteSubRedeCadastrado(SUBREDE SubRede, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODSUBREDE FROM SubRede WITH (NOLOCK) WHERE NOMSUBREDE = @NOMSUBREDE";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMSUBREDE", DbType.String, SubRede.NOMSUBREDE);
            var CODSUBREDE = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODSUBREDE != 0);
        }

        public SUBREDE GetSubrede(int codSubrede)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT ");
            sql.AppendLine("CODSUBREDE, NOMSUBREDE, RAZAOSOCIAL, CNPJ, CEP, LOGRADOURO, NROLOGRADOURO, COMPLEMENTO, BAIRRO, UF, CIDADE ");
            sql.AppendLine("FROM SUBREDE WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODSUBREDE = @CODSUBREDE");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODSUBREDE", DbType.String, codSubrede);
            var idr = db.ExecuteReader(cmd);
            SUBREDE subrede = null;
            if (idr.Read())
            {
                subrede = new SUBREDE();
                subrede.CODSUBREDE = Convert.ToInt32(idr["CODSUBREDE"]);
                subrede.NOMSUBREDE = Convert.ToString(idr["NOMSUBREDE"]).Trim();
                subrede.CNPJ = Convert.ToString(idr["CNPJ"]).Trim();
                subrede.RAZAOSOCIAL = Convert.ToString(idr["RAZAOSOCIAL"]).Trim();
                subrede.CEP = Convert.ToString(idr["CEP"]).Trim();
                subrede.LOGRADOURO = Convert.ToString(idr["LOGRADOURO"]).Trim();
                subrede.NROLOGRADOURO = Convert.ToString(idr["NROLOGRADOURO"]).Trim();
                subrede.COMPLEMENTO = Convert.ToString(idr["COMPLEMENTO"]).Trim();
                subrede.BAIRRO = Convert.ToString(idr["BAIRRO"]).Trim();
                subrede.CIDADE = Convert.ToString(idr["CIDADE"]).Trim();
                subrede.UF = Convert.ToString(idr["UF"]).Trim();
            }
            idr.Close();
            return subrede;
        }

        public SUBREDE GetSubRedeByName(string nomSubR)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODSUBREDE, NOMSUBREDE");
            sql.AppendLine("FROM SUBREDE WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMSUBREDE = @NOMSUBREDE");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMSUBREDE", DbType.String, nomSubR);
            var idr = db.ExecuteReader(cmd);
            SUBREDE subrede = null;
            if (idr.Read())
            {
                subrede = new SUBREDE();
                subrede.NOMSUBREDE = idr["NOMSUBREDE"].ToString();
                subrede.CODSUBREDE = Convert.ToInt32(idr["CODSUBREDE"]);
            }
            idr.Close();
            return subrede;
        }

        private static bool ExisteSubRedeAssociado(SUBREDE SubRede, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODSUBREDE FROM CLIENTEVA WITH (NOLOCK) WHERE CODSUBREDE = @CODSUBREDE " +
                               "UNION " +
                               "SELECT TOP 1 CODSUBREDE FROM CLIENTE WITH (NOLOCK) WHERE CODSUBREDE = @CODSUBREDE ";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODSUBREDE", DbType.String, SubRede.CODSUBREDE);
            var CodSubRede = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodSubRede != 0);
        }

        public void InserirSubRede(SUBREDE SubRede)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se SubRede ja Existe
                if (ExisteSubRedeCadastrado(SubRede, db, dbt))
                    throw new Exception("Sub-Rede ja existe. Favor verificar.");

                const string sql = "INSERT INTO SUBREDE " +
                                    "   (CODSUBREDE, NOMSUBREDE, CNPJ, CEP, LOGRADOURO, NROLOGRADOURO, COMPLEMENTO, BAIRRO, CIDADE, UF) " + 
                                    "VALUES (" +
                                    "           @CODSUBREDE, @NOMSUBREDE, @CNPJ, @CEP, @LOGRADOURO, @NROLOGRADOURO, @COMPLEMENTO, @BAIRRO, @CIDADE, @UF" + 
                                    "       )";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODSUBREDE", DbType.Int32, ProximoCodigoSubRedeLivre());
                db.AddInParameter(cmd, "NOMSUBREDE", DbType.String, SubRede.NOMSUBREDE);
                db.AddInParameter(cmd, "CNPJ", DbType.String, SubRede.CNPJ);
                db.AddInParameter(cmd, "CEP", DbType.String, SubRede.CEP);
                db.AddInParameter(cmd, "LOGRADOURO", DbType.String, SubRede.LOGRADOURO);
                db.AddInParameter(cmd, "NROLOGRADOURO", DbType.String, SubRede.NROLOGRADOURO);
                db.AddInParameter(cmd, "COMPLEMENTO", DbType.String, SubRede.COMPLEMENTO);
                db.AddInParameter(cmd, "BAIRRO", DbType.String, SubRede.BAIRRO);
                db.AddInParameter(cmd, "CIDADE", DbType.String, SubRede.CIDADE);
                db.AddInParameter(cmd, "UF", DbType.String, SubRede.UF);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT SubRede", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarSubRede(SUBREDE SubRede)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE SUBREDE SET " + 
                                   "    NOMSUBREDE = @NOMSUBREDE, " +
                                   "    CNPJ = @CNPJ, " +
                                   "    CEP = @CEP, " +
                                   "    LOGRADOURO = @LOGRADOURO, " +
                                   "    NROLOGRADOURO = @NROLOGRADOURO, " +
                                   "    COMPLEMENTO = @COMPLEMENTO, " +
                                   "    BAIRRO = @BAIRRO, " +
                                   "    CIDADE = @CIDADE, " +
                                   "    UF = @UF " +
                                   "WHERE CODSUBREDE = @CODSUBREDE ";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODSUBREDE", DbType.Int32, SubRede.CODSUBREDE);
                db.AddInParameter(cmd, "CNPJ", DbType.String, SubRede.CNPJ);
                db.AddInParameter(cmd, "NOMSUBREDE", DbType.String, SubRede.NOMSUBREDE);
                db.AddInParameter(cmd, "CEP", DbType.String, SubRede.CEP);
                db.AddInParameter(cmd, "LOGRADOURO", DbType.String, SubRede.LOGRADOURO);
                db.AddInParameter(cmd, "NROLOGRADOURO", DbType.String, SubRede.NROLOGRADOURO);
                db.AddInParameter(cmd, "COMPLEMENTO", DbType.String, SubRede.COMPLEMENTO);
                db.AddInParameter(cmd, "BAIRRO", DbType.String, SubRede.BAIRRO);
                db.AddInParameter(cmd, "CIDADE", DbType.String, SubRede.CIDADE);
                db.AddInParameter(cmd, "UF", DbType.String, SubRede.UF);
                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE SubRede", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirSubRede(SUBREDE SubRede)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteSubRedeAssociado(SubRede, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE SubRede WHERE CODSUBREDE = @CODSUBREDE";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODSUBREDE", DbType.Int32, SubRede.CODSUBREDE);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE SubRede", FOperador, cmd);
                dbt.Commit();
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

        #region GRUPOSOCIETARIO

        public List<GRUPOSOCIETARIO> ListarGrupoSocietario()
        {
            var ColecaoGrupoSocietario = new List<GRUPOSOCIETARIO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODGRUPOSOC, NOMGRUPOSOC ");
            sql.AppendLine("FROM GRUPOSOCIETARIO WITH (NOLOCK) ");
            //sql.AppendLine("WHERE CODGRUPOSOC > 1");
            sql.AppendLine("ORDER BY NOMGRUPOSOC ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var grupoSocietario = new GRUPOSOCIETARIO();
                grupoSocietario.CODGRUPOSOC = Convert.ToInt32(idr["CODGRUPOSOC"]);
                grupoSocietario.NOMGRUPOSOC = Convert.ToString(idr["NOMGRUPOSOC"]).Trim();

                ColecaoGrupoSocietario.Add(grupoSocietario);
            }
            idr.Close();

            return ColecaoGrupoSocietario;
        }

        public int ProximoCodigoGrupoSocietarioLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODGRUPOSOC)+1, 1) AS ProxCod");
            sql.AppendLine("FROM GRUPOSOCIETARIO WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODGRUPOSOC + 1) NOT IN (SELECT CODGRUPOSOC FROM GRUPOSOCIETARIO WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteGrupoSocietarioCadastrado(GRUPOSOCIETARIO GrupoSocietario, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODGRUPOSOC FROM GRUPOSOCIETARIO WITH (NOLOCK) WHERE NOMGRUPOSOC = @NOMGRUPOSOC";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMGRUPOSOC", DbType.String, GrupoSocietario.NOMGRUPOSOC);
            var CODGRUPOSOC = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODGRUPOSOC != 0);
        }

        public GRUPOSOCIETARIO GetGrupoSocietario(int codGrupoSocietario)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODGRUPOSOC, NOMGRUPOSOC");
            sql.AppendLine("FROM GRUPOSOCIETARIO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODGRUPOSOC = @CODGRUPOSOC");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODGRUPOSOC", DbType.String, codGrupoSocietario);
            var idr = db.ExecuteReader(cmd);
            GRUPOSOCIETARIO grupoSocietario = null;
            if (idr.Read())
            {
                grupoSocietario = new GRUPOSOCIETARIO();
                grupoSocietario.NOMGRUPOSOC = idr["NOMGRUPOSOC"].ToString();
                grupoSocietario.CODGRUPOSOC = Convert.ToInt32(idr["CODGRUPOSOC"]);
            }
            idr.Close();
            return grupoSocietario;
        }

        public GRUPOSOCIETARIO GetGrupoSocietarioByName(string nomGrupoSocietario)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODGRUPOSOC, NOMGRUPOSOC");
            sql.AppendLine("FROM GRUPOSOCIETARIO WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMGRUPOSOC = @NOMGRUPOSOC");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMGRUPOSOC", DbType.String, nomGrupoSocietario);
            var idr = db.ExecuteReader(cmd);
            GRUPOSOCIETARIO grupoSocietario = null;
            if (idr.Read())
            {
                grupoSocietario = new GRUPOSOCIETARIO();
                grupoSocietario.NOMGRUPOSOC = idr["NOMGRUPOSOC"].ToString();
                grupoSocietario.CODGRUPOSOC = Convert.ToInt32(idr["CODGRUPOSOC"]);
            }
            idr.Close();
            return grupoSocietario;
        }

        private static bool ExisteGrupoSocietarioAssociado(GRUPOSOCIETARIO GrupoSocietario, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODGRUPOSOC FROM CLIENTEVA WITH (NOLOCK) WHERE CODGRUPOSOC = @CODGRUPOSOC " +
                               "UNION " +
                               "SELECT TOP 1 CODGRUPOSOC FROM CLIENTE WITH (NOLOCK) WHERE CODGRUPOSOC = @CODGRUPOSOC ";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODGRUPOSOC", DbType.String, GrupoSocietario.CODGRUPOSOC);
            var CodGRUPOSOC = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodGRUPOSOC != 0);
        }

        public void InserirGrupoSocietario(GRUPOSOCIETARIO grupoSocietario)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se SubRede ja Existe
                if (ExisteGrupoSocietarioCadastrado(grupoSocietario, db, dbt))
                    throw new Exception("Grupo Societario ja existe. Favor verificar.");

                const string sql = "INSERT INTO GRUPOSOCIETARIO (CODGRUPOSOC, NOMGRUPOSOC) VALUES (@CODGRUPOSOC, @NOMGRUPOSOC)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODGRUPOSOC", DbType.Int32, ProximoCodigoGrupoSocietarioLivre());
                db.AddInParameter(cmd, "NOMGRUPOSOC", DbType.String, grupoSocietario.NOMGRUPOSOC);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT GRUPOSOCIETARIO", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarGrupoSocietario(GRUPOSOCIETARIO grupoSocietario)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE GRUPOSOCIETARIO SET NOMGRUPOSOC = @NOMGRUPOSOC WHERE CODGRUPOSOC = @CODGRUPOSOC";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODGRUPOSOC", DbType.Int32, grupoSocietario.CODGRUPOSOC);
                db.AddInParameter(cmd, "NOMGRUPOSOC", DbType.String, grupoSocietario.NOMGRUPOSOC);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE GRUPOSOCIETARIO", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirGrupoSocietario(GRUPOSOCIETARIO grupoSocietario)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteGrupoSocietarioAssociado(grupoSocietario, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE GRUPOSOCIETARIO WHERE CODGRUPOSOC = @CODGRUPOSOC";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODGRUPOSOC", DbType.Int32, grupoSocietario.CODGRUPOSOC);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE GRUPOSOCIETARIO", FOperador, cmd);
                dbt.Commit();
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

        #region PARCERIA

        public List<PARCERIA> ListarParceria()
        {
            var ColecaoParceria = new List<PARCERIA>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODPARCERIA, NOMPARCERIA ");
            sql.AppendLine("FROM PARCERIA WITH (NOLOCK) ");
            //sql.AppendLine("WHERE CODPARCERIA > 1");
            sql.AppendLine("ORDER BY NOMPARCERIA ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var parceria = new PARCERIA();
                parceria.CODPARCERIA = Convert.ToInt32(idr["CODPARCERIA"]);
                parceria.NOMPARCERIA = Convert.ToString(idr["NOMPARCERIA"]).Trim();

                ColecaoParceria.Add(parceria);
            }
            idr.Close();

            return ColecaoParceria;
        }

        public int ProximoCodigoParceriaLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODPARCERIA)+1, 1) AS ProxCod");
            sql.AppendLine("FROM Parceria WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODPARCERIA + 1) NOT IN (SELECT CODPARCERIA FROM PARCERIA WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteParceriaCadastrado(PARCERIA Parceria, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODPARCERIA FROM PARCERIA WITH (NOLOCK) WHERE NOMPARCERIA = @NOMPARCERIA";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMPARCERIA", DbType.String, Parceria.NOMPARCERIA);
            var CODPARCERIA = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODPARCERIA != 0);
        }

        public PARCERIA GetParceria(int codParceria)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODPARCERIA, NOMPARCERIA");
            sql.AppendLine("FROM PARCERIA WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODPARCERIA = @CODPARCERIA");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODPARCERIA", DbType.String, codParceria);
            var idr = db.ExecuteReader(cmd);
            PARCERIA parceria = null;
            if (idr.Read())
            {
                parceria = new PARCERIA();
                parceria.NOMPARCERIA = idr["NOMPARCERIA"].ToString();
                parceria.CODPARCERIA = Convert.ToInt32(idr["CODPARCERIA"]);
            }
            idr.Close();
            return parceria;
        }

        public PARCERIA GetParceriaByName(string nomParceria)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODPARCERIA, NOMPARCERIA");
            sql.AppendLine("FROM PARCERIA WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMPARCERIA = @NOMPARCERIA");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMPARCERIA", DbType.String, nomParceria);
            var idr = db.ExecuteReader(cmd);
            PARCERIA parceria = null;
            if (idr.Read())
            {
                parceria = new PARCERIA();
                parceria.NOMPARCERIA = idr["NOMPARCERIA"].ToString();
                parceria.CODPARCERIA = Convert.ToInt32(idr["CODPARCERIA"]);
            }
            idr.Close();
            return parceria;
        }

        private static bool ExisteParceriaAssociado(PARCERIA Parceria, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODPARCERIA FROM CLIENTEVA WITH (NOLOCK) WHERE CODPARCERIA = @CODPARCERIA " +
                               "UNION " +
                               "SELECT TOP 1 CODPARCERIA FROM CLIENTE WITH (NOLOCK) WHERE CODPARCERIA = @CODPARCERIA ";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODPARCERIA", DbType.String, Parceria.CODPARCERIA);
            var CodParceria = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodParceria != 0);
        }

        public void InserirParceria(PARCERIA parceria)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se SubRede ja Existe
                if (ExisteParceriaCadastrado(parceria, db, dbt))
                    throw new Exception("Parceria ja existe. Favor verificar.");

                const string sql = "INSERT INTO PARCERIA (CODPARCERIA, NOMPARCERIA) VALUES (@CODPARCERIA, @NOMPARCERIA)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODPARCERIA", DbType.Int32, ProximoCodigoParceriaLivre());
                db.AddInParameter(cmd, "NOMPARCERIA", DbType.String, parceria.NOMPARCERIA);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT Parceria", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarParceria(PARCERIA parceria)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE PARCERIA SET NOMPARCERIA = @NOMPARCERIA WHERE CODPARCERIA = @CODPARCERIA";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODPARCERIA", DbType.Int32, parceria.CODPARCERIA);
                db.AddInParameter(cmd, "NOMPARCERIA", DbType.String, parceria.NOMPARCERIA);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE Parceria", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirParceria(PARCERIA parceria)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteParceriaAssociado(parceria, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE PARCERIA WHERE CODPARCERIA = @CODPARCERIA";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODPARCERIA", DbType.Int32, parceria.CODPARCERIA);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE Parceria", FOperador, cmd);
                dbt.Commit();
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

        #region UNIDADE

        public List<UNIDADE> ListarUnidade()
        {
            var ColecaoUnidade = new List<UNIDADE>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODUNIDADE, NOMUNIDADE ");
            sql.AppendLine("FROM UNIDADE WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMUNIDADE ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var unidade = new UNIDADE();
                unidade.CODUNIDADE = Convert.ToInt32(idr["CODUNIDADE"]);
                unidade.NOMUNIDADE = Convert.ToString(idr["NOMUNIDADE"]).Trim();

                ColecaoUnidade.Add(unidade);
            }
            idr.Close();

            return ColecaoUnidade;
        }

        public int ProximoCodigoUnidadeLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODUNIDADE)+1, 1) AS ProxCod");
            sql.AppendLine("FROM UNIDADE WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODUNIDADE + 1) NOT IN (SELECT CODUNIDADE FROM UNIDADE WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteUnidadeCadastrado(UNIDADE Unidade, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODUNIDADE FROM UNIDADE WITH (NOLOCK) WHERE NOMUNIDADE = @NOMUNIDADE";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMUNIDADE", DbType.String, Unidade.NOMUNIDADE);
            var CODUNIDADE = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODUNIDADE != 0);
        }

        public UNIDADE GetUnidade(int codUnidade)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODUNIDADE, NOMUNIDADE");
            sql.AppendLine("FROM UNIDADE WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODUNIDADE = @CODUNIDADE");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODUNIDADE", DbType.String, codUnidade);
            var idr = db.ExecuteReader(cmd);
            UNIDADE unidade = null;
            if (idr.Read())
            {
                unidade = new UNIDADE();
                unidade.NOMUNIDADE = idr["NOMUNIDADE"].ToString();
                unidade.CODUNIDADE = Convert.ToInt32(idr["CODUNIDADE"]);
            }
            idr.Close();
            return unidade;
        }

        public UNIDADE GetUnidadeByName(string nomUnidade)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODUNIDADE, NOMUNIDADE");
            sql.AppendLine("FROM UNIDADE WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMUNIDADE = @NOMUNIDADE");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMUNIDADE", DbType.String, nomUnidade);
            var idr = db.ExecuteReader(cmd);
            UNIDADE unidade = null;
            if (idr.Read())
            {
                unidade = new UNIDADE();
                unidade.NOMUNIDADE = idr["NOMUNIDADE"].ToString();
                unidade.CODUNIDADE = Convert.ToInt32(idr["CODUNIDADE"]);
            }
            idr.Close();
            return unidade;
        }

        private static bool ExisteUnidadeAssociado(UNIDADE Unidade, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODUNIDADE FROM CLIENTEVA WITH (NOLOCK) WHERE CODUNIDADE = @CODUNIDADE " +
                               "UNION " +
                               "SELECT TOP 1 CODUNIDADE FROM CLIENTE  WITH (NOLOCK) WHERE CODUNIDADE = @CODUNIDADE ";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODUNIDADE", DbType.String, Unidade.CODUNIDADE);
            var CodUnidade = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodUnidade != 0);
        }

        public void InserirUnidade(UNIDADE unidade)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se SubRede ja Existe
                if (ExisteUnidadeCadastrado(unidade, db, dbt))
                    throw new Exception("Unidade ja existe. Favor verificar.");

                const string sql = "INSERT INTO UNIDADE (CODUNIDADE, NOMUNIDADE) VALUES (@CODUNIDADE, @NOMUNIDADE)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODUNIDADE", DbType.Int32, ProximoCodigoUnidadeLivre());
                db.AddInParameter(cmd, "NOMUNIDADE", DbType.String, unidade.NOMUNIDADE);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT UNIDADE", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarUnidade(UNIDADE unidade)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE UNIDADE SET NOMUNIDADE = @NOMUNIDADE WHERE CODUNIDADE = @CODUNIDADE";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODUNIDADE", DbType.Int32, unidade.CODUNIDADE);
                db.AddInParameter(cmd, "NOMUNIDADE", DbType.String, unidade.NOMUNIDADE);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE Unidade", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirUnidade(UNIDADE unidade)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteUnidadeAssociado(unidade, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE UNIDADE WHERE CODUNIDADE = @CODUNIDADE";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODUNIDADE", DbType.Int32, unidade.CODUNIDADE);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE UNIDADE", FOperador, cmd);
                dbt.Commit();
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

        #region SETORIND

        public List<SETORIND> ListarSetor()
        {
            var ColecaoSetor = new List<SETORIND>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODSETIND, NOMSETOR ");
            sql.AppendLine("FROM SETORIND WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMSETOR ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var setor = new SETORIND();
                setor.CODSETIND = Convert.ToInt32(idr["CODSETIND"]);
                setor.NOMSETOR = Convert.ToString(idr["NOMSETOR"]).Trim();

                ColecaoSetor.Add(setor);
            }
            idr.Close();

            return ColecaoSetor;
        }

        public int ProximoCodigoSetorLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODSETIND)+1, 1) AS ProxCod");
            sql.AppendLine("FROM SETORIND WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODSETIND + 1) NOT IN (SELECT CODSETIND FROM SETORIND WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteSetorCadastrado(SETORIND Setor, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODSETIND FROM SETORIND WITH (NOLOCK) WHERE NOMSETOR = @NOMSETOR";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMSETOR", DbType.String, Setor.NOMSETOR);
            var CODSETIND = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODSETIND != 0);
        }

        public SETORIND GetSetor(int codSetor)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODSETIND, NOMSETOR");
            sql.AppendLine("FROM SETORIND WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODSETIND = @CODSETIND");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODSETIND", DbType.String, codSetor);
            var idr = db.ExecuteReader(cmd);
            SETORIND setor = null;
            if (idr.Read())
            {
                setor = new SETORIND();
                setor.NOMSETOR = idr["NOMSETOR"].ToString();
                setor.CODSETIND = Convert.ToInt32(idr["CODSETIND"]);
            }
            idr.Close();
            return setor;
        }

        public SETORIND GetSetorByName(string nomSetor)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODSETIND, NOMSETOR");
            sql.AppendLine("FROM SETORIND WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMSETOR = @NOMSETOR");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMSETOR", DbType.String, nomSetor);
            var idr = db.ExecuteReader(cmd);
            SETORIND setor = null;
            if (idr.Read())
            {
                setor = new SETORIND();
                setor.NOMSETOR = idr["NOMSETOR"].ToString();
                setor.CODSETIND = Convert.ToInt32(idr["CODSETIND"]);
            }
            idr.Close();
            return setor;
        }

        private static bool ExisteSetorAssociado(SETORIND Setor, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODSETIND FROM CLIENTEVA WITH (NOLOCK) WHERE CODSETIND = @CODSETIND " +
                               "UNION " +
                               "SELECT TOP 1 CODSETIND FROM CLIENTE WITH (NOLOCK) WHERE CODSETIND = @CODSETIND ";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODSETIND", DbType.String, Setor.CODSETIND);
            var CodSetor = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodSetor != 0);
        }

        public void InserirSetor(SETORIND setor)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se SubRede ja Existe
                if (ExisteSetorCadastrado(setor, db, dbt))
                    throw new Exception("Setor ja existe. Favor verificar.");

                const string sql = "INSERT INTO SETORIND (CODSETIND, NOMSETOR) VALUES (@CODSETIND, @NOMSETOR)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODSETIND", DbType.Int32, ProximoCodigoSetorLivre());
                db.AddInParameter(cmd, "NOMSETOR", DbType.String, setor.NOMSETOR);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT SETORIND", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarSetor(SETORIND setor)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE SETORIND SET NOMSETOR = @NOMSETOR WHERE CODSETIND = @CODSETIND";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODSETIND", DbType.Int32, setor.CODSETIND);
                db.AddInParameter(cmd, "NOMSETOR", DbType.String, setor.NOMSETOR);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE SETORIND", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirSetor(SETORIND setor)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteSetorAssociado(setor, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE SETORIND WHERE CODSETIND = @CODSETIND";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODSETIND", DbType.Int32, setor.CODSETIND);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE SETORIND", FOperador, cmd);
                dbt.Commit();
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

        #region PORTE

        public List<PORTE> ListarPorte()
        {
            var ColecaoPorte = new List<PORTE>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODPORTE, NOMPORTE ");
            sql.AppendLine("FROM PORTE WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMPORTE ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var porte = new PORTE();
                porte.CODPORTE = Convert.ToInt32(idr["CODPORTE"]);
                porte.NOMPORTE = Convert.ToString(idr["NOMPORTE"]).Trim();

                ColecaoPorte.Add(porte);
            }
            idr.Close();

            return ColecaoPorte;
        }

        public int ProximoCodigoPorteLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODPORTE)+1, 1) AS ProxCod");
            sql.AppendLine("FROM PORTE WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODPORTE + 1) NOT IN (SELECT CODPORTE FROM PORTE WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExistePorteCadastrado(PORTE Porte, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODPORTE FROM PORTE WITH (NOLOCK) WHERE NOMPORTE = @NOMPORTE";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMPORTE", DbType.String, Porte.NOMPORTE);
            var CODPORTE = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODPORTE != 0);
        }

        public PORTE GetPorte(int codPorte)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODPORTE, NOMPORTE");
            sql.AppendLine("FROM PORTE WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODPORTE = @CODPORTE");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODPORTE", DbType.String, codPorte);
            var idr = db.ExecuteReader(cmd);
            PORTE porte = null;
            if (idr.Read())
            {
                porte = new PORTE();
                porte.NOMPORTE = idr["NOMPORTE"].ToString();
                porte.CODPORTE = Convert.ToInt32(idr["CODPORTE"]);
            }
            idr.Close();
            return porte;
        }

        public PORTE GetPorteByName(string nomPorte)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODPORTE, NOMPORTE");
            sql.AppendLine("FROM PORTE WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMPORTE = @NOMPORTE");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMPORTE", DbType.String, nomPorte);
            var idr = db.ExecuteReader(cmd);
            PORTE porte = null;
            if (idr.Read())
            {
                porte = new PORTE();
                porte.NOMPORTE = idr["NOMPORTE"].ToString();
                porte.CODPORTE = Convert.ToInt32(idr["CODPORTE"]);
            }
            idr.Close();
            return porte;
        }

        private static bool ExistePorteAssociado(PORTE Porte, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODPORTE FROM CLIENTEVA WITH (NOLOCK) WHERE CODPORTE = @CODPORTE " +
                               "UNION " +
                               "SELECT TOP 1 CODPORTE FROM CLIENTE WITH (NOLOCK) WHERE CODPORTE = @CODPORTE ";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODPORTE", DbType.String, Porte.CODPORTE);
            var CodPorte = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodPorte != 0);
        }

        public void InserirPorte(PORTE porte)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se SubRede ja Existe
                if (ExistePorteCadastrado(porte, db, dbt))
                    throw new Exception("Porte ja existe. Favor verificar.");

                const string sql = "INSERT INTO PORTE (CODPORTE, NOMPORTE) VALUES (@CODPORTE, @NOMPORTE)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODPORTE", DbType.Int32, ProximoCodigoPorteLivre());
                db.AddInParameter(cmd, "NOMPORTE", DbType.String, porte.NOMPORTE);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT PORTE", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarPorte(PORTE porte)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE PORTE SET NOMPORTE = @NOMPORTE WHERE CODPORTE = @CODPORTE";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODPORTE", DbType.Int32, porte.CODPORTE);
                db.AddInParameter(cmd, "NOMPORTE", DbType.String, porte.NOMPORTE);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE PORTE", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirPorte(PORTE porte)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExistePorteAssociado(porte, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE PORTE WHERE CODPORTE = @CODPORTE";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODPORTE", DbType.Int32, porte.CODPORTE);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE PORTE", FOperador, cmd);
                dbt.Commit();
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

        #region PRODUTO

        public List<PRODUTO> ListarProduto()
        {
            var ColecaoProduto = new List<PRODUTO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("P.SISTEMA, P.CODPROD, P.ROTULO, P.TIPOPROD, P.USO, T.DESCRICAO AS DESTIPOPROD ");
            sql.AppendLine("FROM PRODUTO P WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN TIPOPRODUTO T WITH (NOLOCK) ON P.TIPOPROD = T.TIPOPROD ");            
            sql.AppendLine("ORDER BY P.ROTULO ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var produto = new PRODUTO();
                produto.SISTEMA = Convert.ToString(idr["SISTEMA"]) == "PJ" ? 0 : 1;
                produto.CODPROD = Convert.ToInt32(idr["CODPROD"]);
                produto.ROTULO = Convert.ToString(idr["ROTULO"]).Trim();
                produto.TIPOPROD = Convert.ToInt32(idr["TIPOPROD"]);
                produto.DESTIPOPROD = Convert.ToString(idr["DESTIPOPROD"]);
                produto.USO = Convert.ToString(idr["USO"]) == "P" ? "Pessoa Física" : "Empresarial";
                ColecaoProduto.Add(produto);
            }
            idr.Close();

            return ColecaoProduto;
        }

        public List<PRODUTONOVO> ListarProdutoNovo()
        {
            var ColecaoProduto = new List<PRODUTONOVO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("P.CODPROD, P.ROTULO, P.TIPOPROD, P.USO, T.DESCRICAO AS DESTIPOPROD ");
            sql.AppendLine("FROM PRODUTO P WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN TIPOPRODUTO T WITH (NOLOCK) ON P.TIPOPROD = T.TIPOPROD ");
            sql.AppendLine("ORDER BY P.ROTULO ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var produto = new PRODUTONOVO();
                produto.CODPROD = Convert.ToInt32(idr["CODPROD"]);
                produto.ROTULO = Convert.ToString(idr["ROTULO"]).Trim();
                ColecaoProduto.Add(produto);
            }
            idr.Close();

            return ColecaoProduto;
        }

        public List<TIPOPRODUTO> ListarTipoProd()
        {
            var ColecaoTipoProduto = new List<TIPOPRODUTO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("TIPOPROD, DESCRICAO, SISTEMA ");
            sql.AppendLine("FROM TIPOPRODUTO WITH (NOLOCK) ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var tipoProduto = new TIPOPRODUTO();
                tipoProduto.SISTEMA = Convert.ToString(idr["SISTEMA"]);
                tipoProduto.TIPOPROD = Convert.ToInt32(idr["TIPOPROD"]);
                tipoProduto.DESCRICAO = Convert.ToString(idr["DESCRICAO"]).Trim();
                ColecaoTipoProduto.Add(tipoProduto);
            }
            idr.Close();

            return ColecaoTipoProduto;
        }

        public int ProximoCodigoProdutolivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODPROD)+1, 1) AS ProxCod");
            sql.AppendLine("FROM PRODUTO WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODPROD + 1) NOT IN (SELECT CODPROD FROM PRODUTO WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteProdutoCadastrado(PRODUTO ROTULO, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODPROD FROM PRODUTO WITH (NOLOCK) WHERE ROTULO = @ROTULO";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "ROTULO", DbType.String, ROTULO.ROTULO);
            var CODPROD = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODPROD!= 0);
        }

        public PRODUTO GetProduto(int CodProd)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT SISTEMA, CODPROD, ROTULO, TIPOPROD, USO, RIBBON, PLASTICO, ARTE_CARTAO ");
            sql.AppendLine("FROM PRODUTO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODPROD = @CODPROD");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODPROD", DbType.String, CodProd);
            var idr = db.ExecuteReader(cmd);
            PRODUTO produto = null;
            if (idr.Read())
            {
                produto = new PRODUTO();
                produto.SISTEMA = Convert.ToString(idr["SISTEMA"]) == "PJ" ? 0 : 1;
                produto.ROTULO = idr["ROTULO"].ToString();
                produto.CODPROD = Convert.ToInt32(idr["CODPROD"]);
                produto.TIPOPROD = Convert.ToInt32(idr["TIPOPROD"]);
                produto.USO = Convert.ToString(idr["USO"]);
                produto.RIBBON = Convert.ToString(idr["RIBBON"]);
                produto.PLASTICO = Convert.ToString(idr["PLASTICO"]);
                produto.ARTE_CARTAO = Convert.ToString(idr["ARTE_CARTAO"]);
            }
            idr.Close();
            return produto;
        }

        public PRODUTO GetProdutoByName(string nomProduto)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODPROD, ROTULO, SISTEMA, TIPOPROD ");
            sql.AppendLine("FROM PRODUTO WITH (NOLOCK) ");
            sql.AppendLine("WHERE ROTULO = @ROTULO");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@ROTULO", DbType.String, nomProduto);
            var idr = db.ExecuteReader(cmd);
            PRODUTO produto = null;
            if (idr.Read())
            {
                produto = new PRODUTO();
                produto.SISTEMA = Convert.ToString(idr["SISTEMA"]) == "PJ" ? 0 : 1;
                produto.ROTULO = idr["ROTULO"].ToString();
                produto.CODPROD = Convert.ToInt32(idr["CODPROD"]);
                produto.TIPOPROD = Convert.ToInt32(idr["TIPOPROD"]);
            }
            idr.Close();
            return produto;
        }

        private static bool ExisteProdutoAssociado(PRODUTO produto, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODPROD FROM CLIENTEVA WITH (NOLOCK) WHERE CODPROD = @CODPROD " +
                               "UNION " +
                               "SELECT TOP 1 CODPROD FROM CLIENTE WITH (NOLOCK) WHERE CODPROD = @CODPROD";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODPROD", DbType.String, produto.CODPROD);
            var CodProd = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodProd != 0);
        }

        public void InserirProduto(PRODUTO Produto)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se SubRede ja Existe
                if (ExisteProdutoCadastrado(Produto, db, dbt))
                    throw new Exception("Produto ja existe. Favor verificar.");

                const string sql = "INSERT INTO PRODUTO (CODPROD, ROTULO, SISTEMA, TIPOPROD, USO, RIBBON, PLASTICO, ARTE_CARTAO ) VALUES (@CODPROD, @ROTULO, @SISTEMA, @TIPOPROD, @USO, @RIBBON, @PLASTICO, @ARTE_CARTAO ) ";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "SISTEMA", DbType.String, Produto.SISTEMA == 0 ? "PJ" : "VA");
                db.AddInParameter(cmd, "CODPROD", DbType.Int32, ProximoCodigoProdutolivre());
                db.AddInParameter(cmd, "ROTULO", DbType.String, Produto.ROTULO);
                db.AddInParameter(cmd, "TIPOPROD", DbType.Int32, Produto.TIPOPROD);
                db.AddInParameter(cmd, "USO", DbType.String, Produto.USO);
                db.AddInParameter(cmd, "RIBBON", DbType.String, Produto.RIBBON);
                db.AddInParameter(cmd, "PLASTICO", DbType.String, Produto.PLASTICO);
                db.AddInParameter(cmd, "ARTE_CARTAO", DbType.String, Produto.ARTE_CARTAO);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT Produto", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarProduto(PRODUTO Produto)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE Produto SET ROTULO = @ROTULO, TIPOPROD = @TIPOPROD, USO = @USO, RIBBON = @RIBBON, PLASTICO = @PLASTICO, ARTE_CARTAO = @ARTE_CARTAO WHERE CODPROD = @CODPROD AND SISTEMA = @SISTEMA ";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "SISTEMA", DbType.String, Produto.SISTEMA == 0 ? "PJ" : "VA");
                db.AddInParameter(cmd, "CODPROD", DbType.Int32, Produto.CODPROD);
                db.AddInParameter(cmd, "ROTULO", DbType.String, Produto.ROTULO);
                db.AddInParameter(cmd, "TIPOPROD", DbType.Int32, Produto.TIPOPROD);
                db.AddInParameter(cmd, "USO", DbType.String, Produto.USO);
                db.AddInParameter(cmd, "RIBBON", DbType.String, Produto.RIBBON);
                db.AddInParameter(cmd, "PLASTICO", DbType.String, Produto.PLASTICO);
                db.AddInParameter(cmd, "ARTE_CARTAO", DbType.String, Produto.ARTE_CARTAO);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE Produto", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirProduto(PRODUTO Produto)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteProdutoAssociado(Produto, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE Produto WHERE CODPROD = @CODPROD";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODPROD", DbType.Int32, Produto.CODPROD);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE Produto", FOperador, cmd);
                dbt.Commit();
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

        #region VENDEDOR

        public List<VENDEDOR> ListarVendedor()
        {
            var ColecaoVendedor = new List<VENDEDOR>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODVEND, NOMVEND ");
            sql.AppendLine("FROM VENDEDOR WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMVEND ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var vendedor = new VENDEDOR();
                vendedor.CODVEND = Convert.ToInt32(idr["CODVEND"]);
                vendedor.NOMVEND = Convert.ToString(idr["NOMVEND"]).Trim();

                ColecaoVendedor.Add(vendedor);
            }
            idr.Close();

            return ColecaoVendedor;
        }

        public int ProximoCodigoVendedorLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODVEND)+1, 1) AS ProxCod");
            sql.AppendLine("FROM Vendedor WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODVEND + 1) NOT IN (SELECT CODVEND FROM Vendedor WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteVendedorCadastrado(VENDEDOR Vendedor, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODVEND FROM Vendedor WITH (NOLOCK) WHERE NOMVEND = @NOMVEND";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMVEND", DbType.String, Vendedor.NOMVEND);
            var CODVEND = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODVEND != 0);
        }

        private static bool ExisteVendedorAssociado(VENDEDOR Vendedor, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODVEND FROM CLIENTEVA WITH (NOLOCK) WHERE (CODVEND = @CODVEND) ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODVEND", DbType.String, Vendedor.CODVEND);
            var CodVendedor = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CodVendedor != 0);
        }

        public EPS GetVendedor(int codReo, int codEps)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODEPS, NOMEPS");
            sql.AppendLine("FROM EPS WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODREO = @CODREO AND CODEPS = @CODEPS AND TIPEPS = 'V'");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODREO", DbType.String, codReo);
            db.AddInParameter(cmd, "@CODEPS", DbType.String, codEps);
            var idr = db.ExecuteReader(cmd);
            EPS local = null;
            if (idr.Read())
            {
                local = new EPS();
                local.NOMEPS = idr["NOMEPS"].ToString();
                local.CODEPS = Convert.ToInt32(idr["CODEPS"]);
            }
            idr.Close();
            return local;
        }

        public EPS GetVendedorByName(string nomVend)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODEPS, NOMEPS");
            sql.AppendLine("FROM EPS WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMEPS = @NOMEPS");
            sql.AppendLine("AND TIPEPS = 'V'");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMEPS", DbType.String, nomVend);
            var idr = db.ExecuteReader(cmd);
            EPS vendedor = null;
            if (idr.Read())
            {
                vendedor = new EPS();
                vendedor.NOMEPS = idr["NOMEPS"].ToString();
                vendedor.CODEPS = Convert.ToInt32(idr["CODEPS"]);
            }
            idr.Close();
            return vendedor;
        }

        public EPS GetVendedorByName(int codReo, string nomVend)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODEPS, NOMEPS");
            sql.AppendLine("FROM EPS WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODREO = @CODREO AND NOMEPS = @NOMEPS ");
            sql.AppendLine("AND TIPEPS = 'V'");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODREO", DbType.Int16, codReo);
            db.AddInParameter(cmd, "@NOMEPS", DbType.String, nomVend);
            var idr = db.ExecuteReader(cmd);
            EPS vendedor = null;
            if (idr.Read())
            {
                vendedor = new EPS();
                vendedor.NOMEPS = idr["NOMEPS"].ToString();
                vendedor.CODEPS = Convert.ToInt32(idr["CODEPS"]);
            }
            idr.Close();
            return vendedor;
        }

        public void InserirVendedor(VENDEDOR Vendedor)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se Vendedor ja Existe
                if (ExisteVendedorCadastrado(Vendedor, db, dbt))
                    throw new Exception("Vendedor ja existe. Favor verificar.");
                const string sql = "INSERT INTO Vendedor (CODVEND, NOMVEND) VALUES (@CODVEND, @NOMVEND)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODVEND", DbType.Int32, ProximoCodigoVendedorLivre());
                db.AddInParameter(cmd, "NOMVEND", DbType.String, Vendedor.NOMVEND);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT Vendedor", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarVendedor(VENDEDOR Vendedor)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE Vendedor SET NOMVEND = @NOMVEND WHERE CODVEND = @CODVEND";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODVEND", DbType.Int32, Vendedor.CODVEND);
                db.AddInParameter(cmd, "NOMVEND", DbType.String, Vendedor.NOMVEND);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE Vendedor", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirVendedor(VENDEDOR Vendedor)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteVendedorAssociado(Vendedor, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE Vendedor WHERE CODVEND = @CODVEND";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODVEND", DbType.Int32, Vendedor.CODVEND);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE Vendedor", FOperador, cmd);
                dbt.Commit();
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

        #region LOCALIDADE

        public List<LOCALIDADE> ListaLocalidade()
        {
            var ColecaoLocalidade = new List<LOCALIDADE>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODLOC, NOMLOC ");
            sql.AppendLine("FROM LOCALIDADE WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMLOC ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var Localidade = new LOCALIDADE();
                Localidade.CODLOC = Convert.ToInt32(idr["CODLOC"]);
                Localidade.NOMLOC = Convert.ToString(idr["NOMLOC"]).Trim();

                ColecaoLocalidade.Add(Localidade);
            }
            idr.Close();

            return ColecaoLocalidade;
        }

        public int ProximoCodigoLocalidadeLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODLOC)+1, 1) AS ProxCod");
            sql.AppendLine("FROM Localidade WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODLOC + 1) NOT IN (SELECT CODLOC FROM Localidade WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteLocalidadeCadastrada(LOCALIDADE Localidade, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODLOC FROM Localidade WITH (NOLOCK) WHERE NOMLOC = @NOMLOC ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMLOC", DbType.String, Localidade.NOMLOC);
            var CODLOC = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODLOC != 0);
        }

        private static bool ExisteLocalidadeAssociado(LOCALIDADE Localidade, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODLOC FROM Cliente WITH (NOLOCK) WHERE (CODLOC = @CODLOC) OR (CODLOCEDC = @CODLOC) " +
                               "UNION " +
                               "SELECT TOP 1 CODLOC FROM ClienteVA WITH (NOLOCK) WHERE (CODLOC = @CODLOC) OR (CODLOCEDC = @CODLOC) " +
                               "UNION " +
                               "SELECT TOP 1 CODLOC FROM Credenciado WITH (NOLOCK) WHERE (CODLOC = @CODLOC) OR (CODLOCCOR = @CODLOC)" +
                               "UNION " +
                               "SELECT TOP 1 CODLOC FROM Usuario WITH (NOLOCK) WHERE CODLOC = @CODLOC ";
                
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODLOC", DbType.String, Localidade.CODLOC);
            var CODLOC = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODLOC != 0);
        }

        public int GetCodLoc(string filtro)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODLOC FROM LOCALIDADE WITH (NOLOCK) ");
            if (!string.IsNullOrEmpty(filtro))
                sql.AppendLine(string.Format("WHERE {0} ", filtro));

            var cmd = db.GetSqlStringCommand(sql.ToString());
            return Convert.ToInt32(db.ExecuteScalar(cmd));
        }

        public LOCALIDADE GetLocalidade(int codLoc)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODLOC, NOMLOC");
            sql.AppendLine("FROM LOCALIDADE WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODLOC = @CODLOC");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODLOC", DbType.String, codLoc);
            var idr = db.ExecuteReader(cmd);
            LOCALIDADE local = null;
            if (idr.Read())
            {
                local = new LOCALIDADE();
                local.NOMLOC = idr["NOMLOC"].ToString();
                local.CODLOC = Convert.ToInt32(idr["CODLOC"]);
            }
            idr.Close();
            return local;
        }

        public LOCALIDADE GetLocalidadeByName(string nomLoc)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODLOC, NOMLOC");
            sql.AppendLine("FROM LOCALIDADE WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOMLOC = @NOMLOC");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@NOMLOC", DbType.String, nomLoc);
            var idr = db.ExecuteReader(cmd);
            LOCALIDADE local = null;
            if (idr.Read())
            {
                local = new LOCALIDADE();
                local.NOMLOC = idr["NOMLOC"].ToString();
                local.CODLOC = Convert.ToInt32(idr["CODLOC"]);
            }
            idr.Close();
            return local;
        }

        public void InserirLocalidade(LOCALIDADE Localidade)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se Localidade ja Existe
                if (ExisteLocalidadeCadastrada(Localidade, db, dbt))
                    throw new Exception("Localidade ja existe. Favor verificar.");
                const string sql = "INSERT INTO Localidade (CODLOC, NOMLOC) VALUES (@CODLOC, @NOMLOC)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODLOC", DbType.Int32, ProximoCodigoLocalidadeLivre());
                db.AddInParameter(cmd, "NOMLOC", DbType.String, Localidade.NOMLOC);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT Localidade", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarLocalidade(LOCALIDADE Localidade)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE Localidade SET NOMLOC = @NOMLOC WHERE CODLOC = @CODLOC";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODLOC", DbType.Int32, Localidade.CODLOC);
                db.AddInParameter(cmd, "NOMLOC", DbType.String, Localidade.NOMLOC);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE Localidade", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirLocalidade(LOCALIDADE Localidade)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteLocalidadeAssociado(Localidade, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE Localidade WHERE CODLOC = @CODLOC";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODLOC", DbType.Int32, Localidade.CODLOC);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE Localidade", FOperador, cmd);
                db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();
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

        #region TIPO REEMBOLSO

        public List<REEMBOLSO> ListaReembolso()
        {
            var ColecaoReembolso = new List<REEMBOLSO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("FORPAG, DESFORPAG ");
            sql.AppendLine("FROM REEMBOLSO WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY DESFORPAG ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var reembolso = new REEMBOLSO();
                reembolso.FORPAG = Convert.ToInt16(idr["FORPAG"]);
                reembolso.DESFORPAG = Convert.ToString(idr["DESFORPAG"]).Trim();

                ColecaoReembolso.Add(reembolso);
            }
            idr.Close();

            return ColecaoReembolso;
        }

        public int ProximoCodigoReembolsoLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(FORPAG)+1, 1) AS ProxCod");
            sql.AppendLine("FROM REEMBOLSO WITH (NOLOCK) ");
            sql.AppendLine("WHERE (FORPAG + 1) NOT IN (SELECT FORPAG FROM REEMBOLSO WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteReembolsoCadastrado(REEMBOLSO Reembolso, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT FORPAG FROM REEMBOLSO WITH (NOLOCK) WHERE DESFORPAG = @DESFORPAG";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "DESFORPAG", DbType.String, Reembolso.DESFORPAG);
            var FORPAG = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (FORPAG != 0);
        }

        private static bool ExisteReembolsoAssociado(REEMBOLSO Reembolso, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 FORPAG FROM CLIENTEVA WITH (NOLOCK) WHERE FORPAG = @FORPAG " +
                               "UNION " +
                               "SELECT TOP 1 FORPAG FROM CREDENCIADO WITH (NOLOCK) WHERE FORPAG = @FORPAG";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "FORPAG", DbType.String, Reembolso.FORPAG);
            var FORPAG = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (FORPAG != 0);
        }

        public void InserirReembolso(REEMBOLSO Reembolso)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se Reembolso ja Existe
                if (ExisteReembolsoCadastrado(Reembolso, db, dbt))
                    throw new Exception("Reembolso ja existe. Favor verificar.");
                const string sql = "INSERT INTO REEMBOLSO (FORPAG, DESFORPAG) VALUES (@FORPAG, @DESFORPAG)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "FORPAG", DbType.Int32, ProximoCodigoReembolsoLivre());
                db.AddInParameter(cmd, "DESFORPAG", DbType.String, Reembolso.DESFORPAG);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT REEMBOLSO", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarReembolso(REEMBOLSO Reembolso)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE REEMBOLSO SET DESFORPAG = @DESFORPAG WHERE FORPAG = @FORPAG";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "FORPAG", DbType.Int32, Reembolso.FORPAG);
                db.AddInParameter(cmd, "DESFORPAG", DbType.String, Reembolso.DESFORPAG);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE REEMBOLSO", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirReembolso(REEMBOLSO Reembolso)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteReembolsoAssociado(Reembolso, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE REEMBOLSO WHERE FORPAG = @FORPAG";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "FORPAG", DbType.Int32, Reembolso.FORPAG);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE REEMBOLSO", FOperador, cmd);
                dbt.Commit();
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

        #region FILIAL REDE

        public List<FILNUTRI> ListaFilial()
        {
            var ColecaoFilial = new List<FILNUTRI>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODFILNUT, NOMFILNUT ");
            sql.AppendLine("FROM FILNUTRI WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY NOMFILNUT ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var filial = new FILNUTRI();
                filial.CODFILNUT = Convert.ToInt32(idr["CODFILNUT"]);
                filial.NOMFILNUT = Convert.ToString(idr["NOMFILNUT"]).Trim();

                ColecaoFilial.Add(filial);
            }
            idr.Close();
            return ColecaoFilial;
        }

        public int ProximoCodigoFilialLivre()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODFILNUT)+1, 1) AS ProxCod");
            sql.AppendLine("FROM FILNUTRI WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODFILNUT + 1) NOT IN (SELECT CODFILNUT FROM FILNUTRI WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        private static bool ExisteFilialCadastrada(FILNUTRI FilNutri, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODFILNUT FROM FILNUTRI WITH (NOLOCK) WHERE NOMFILNUT = @NOMFILNUT";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "NOMFILNUT", DbType.String, FilNutri.NOMFILNUT);
            var CODFILNUT = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODFILNUT != 0);
        }

        private static bool ExisteFilialAssociada(FILNUTRI FilNutri, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODFILNUT FROM CLIENTEVA WITH (NOLOCK) WHERE CODFILNUT = @CODFILNUT " +
                               "UNION " +
                               "SELECT TOP 1 CODFILNUT FROM CREDENCIADO WITH (NOLOCK) WHERE CODFILNUT = @CODFILNUT";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODFILNUT", DbType.String, FilNutri.CODFILNUT);
            var CODFILNUT = Convert.ToInt32(db.ExecuteScalar(cmd, dbt));
            return (CODFILNUT != 0);
        }

        public void InserirFilial(FILNUTRI FilNutri)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se FilNutri ja Existe
                if (ExisteFilialCadastrada(FilNutri, db, dbt))
                    throw new Exception("Filial ja existe. Favor verificar.");
                const string sql = "INSERT INTO FILNUTRI (CODFILNUT, NOMFILNUT) VALUES (@CODFILNUT, @NOMFILNUT)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODFILNUT", DbType.Int32, ProximoCodigoFilialLivre());
                db.AddInParameter(cmd, "NOMFILNUT", DbType.String, FilNutri.NOMFILNUT);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT FILNUTRI", FOperador, cmd);
                dbt.Commit();
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

        public void AlterarFilial(FILNUTRI FilNutri)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE FILNUTRI SET NOMFILNUT = @NOMFILNUT WHERE CODFILNUT = @CODFILNUT";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODFILNUT", DbType.Int32, FilNutri.CODFILNUT);
                db.AddInParameter(cmd, "NOMFILNUT", DbType.String, FilNutri.NOMFILNUT);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE FILNUTRI", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirFilial(FILNUTRI FilNutri)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteFilialAssociada(FilNutri, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");
                const string sql = "DELETE FILNUTRI WHERE CODFILNUT = @CODFILNUT";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODFILNUT", DbType.Int32, FilNutri.CODFILNUT);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE FILNUTRI", FOperador, cmd);
                dbt.Commit();
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

        #region LOGOCARTAO

        public List<LOGOCARTAO> ListarLogoCartao()
        {
            var ColecaoLogoCartao = new List<LOGOCARTAO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODLOGO, DESCRICAO ");
            sql.AppendLine("FROM LOGOCARTAO WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY CODLOGO ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var logoCartao = new LOGOCARTAO();
                logoCartao.CODLOGO = Convert.ToString(idr["CODLOGO"]).Trim();
                logoCartao.DESCRICAO = Convert.ToString(idr["DESCRICAO"]).Trim();
                logoCartao.CODDESCRICAO = logoCartao.CODLOGO + " - " + logoCartao.DESCRICAO;
                ColecaoLogoCartao.Add(logoCartao);
            }
            idr.Close();

            return ColecaoLogoCartao;
        }

        private static bool ExisteLogoCartaoCadastrado(LOGOCARTAO LogoCartao, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODLOGO FROM LOGOCARTAO WITH (NOLOCK) WHERE CODLOGO = @CODLOGO";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODLOGO", DbType.String, LogoCartao.CODLOGO);
            var CodLogo = Convert.ToString(db.ExecuteScalar(cmd, dbt));
            return !string.IsNullOrEmpty(CodLogo);
        }

        private static bool ExisteLogoCartaoAssociado(LOGOCARTAO LogoCartao, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODLOGO1 FROM CLIENTEVA WITH (NOLOCK) WHERE CODLOGO1 = @CODLOGO " +
                                "UNION " +
                                "SELECT TOP 1 CODLOGO2 FROM CLIENTEVA WITH (NOLOCK) WHERE CODLOGO2 = @CODLOGO";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODLOGO", DbType.String, LogoCartao.CODLOGO);
            var CodLogo = Convert.ToString(db.ExecuteScalar(cmd, dbt));
            return !string.IsNullOrEmpty(CodLogo);
        }

        public LOGOCARTAO GetLogoCartao(string codLogo)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODLOGO ");
            sql.AppendLine("FROM LOGOCARTAO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODLOGO = @CODLOGO");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODLOGO", DbType.String, codLogo);
            var idr = db.ExecuteReader(cmd);
            LOGOCARTAO logoCartao = null;
            if (idr.Read())
            {
                logoCartao = new LOGOCARTAO();
                logoCartao.CODLOGO = Convert.ToString(idr["CODLOGO"]).Trim();
            }
            idr.Close();
            return logoCartao;
        }

        public void InserirLogoCartao(LOGOCARTAO LogoCartao)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle de transação
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se logo já existe
                if (ExisteLogoCartaoCadastrado(LogoCartao, db, dbt))
                    throw new Exception("Logo já existe. Favor verificar.");

                const string sql = "INSERT INTO LOGOCARTAO (CODLOGO, DESCRICAO) VALUES (@CODLOGO, @DESCRICAO)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODLOGO", DbType.String, LogoCartao.CODLOGO);
                db.AddInParameter(cmd, "DESCRICAO", DbType.String, LogoCartao.DESCRICAO);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICAÇÂO NOS DADOS (O cmd é pra listar o valor dos parâmetros, caso existam.)
                UtilSIL.GravarLog(db, dbt, "INSERT LOGOCARTAO", FOperador, cmd);
                dbt.Commit();

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

        public void AlterarLogoCartao(LOGOCARTAO LogoCartao)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle de transação
            var dbt = dbc.BeginTransaction();

            try
            {
                const string sql = "UPDATE LOGOCARTAO SET CODLOGO = @CODLOGO WHERE CODLOGO = @CODLOGO";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODLOGO", DbType.String, LogoCartao.CODLOGO);
                db.ExecuteNonQuery(cmd, dbt);
                
                //LOG GERAL PARA QUALQUER MODIFICAÇÂO NOS DADOS (O cmd é pra listar o valor dos parâmetros, caso existam.)
                UtilSIL.GravarLog(db, dbt, "UPDATE LOGOCARTAO", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirLogoCartao(LOGOCARTAO LogoCartao)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle de transação
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteLogoCartaoAssociado(LogoCartao, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");

                const string sql = "DELETE LOGOCARTAO WHERE CODLOGO = @CODLOGO";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODLOGO", DbType.String, LogoCartao.CODLOGO);
                db.ExecuteNonQuery(cmd, dbt);
                
                //LOG GERAL PARA QUALQUER MODIFICAÇÂO NOS DADOS (O cmd é pra listar o valor dos parâmetros, caso existam.)
                UtilSIL.GravarLog(db, dbt, "DELETE LOGOCARTAO", FOperador, cmd);
                dbt.Commit();
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

        #region MODELOCARTAO

        public List<MODELOCARTAO> ListarModeloCartao()
        {
            var ColecaoModeloCartao = new List<MODELOCARTAO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("CODMODELO, DESCRICAO ");
            sql.AppendLine("FROM MODELOCARTAO WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY CODMODELO ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var modeloCartao = new MODELOCARTAO();
                modeloCartao.CODMODELO = Convert.ToString(idr["CODMODELO"]).Trim();
                modeloCartao.DESCRICAO = Convert.ToString(idr["DESCRICAO"]).Trim();
                modeloCartao.CODDESCRICAO = modeloCartao.CODMODELO + " - " + modeloCartao.DESCRICAO;
                ColecaoModeloCartao.Add(modeloCartao);
            }
            idr.Close();

            return ColecaoModeloCartao;
        }

        private static bool ExisteLogoCartaoCadastrado(MODELOCARTAO ModeloCartao, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODMODELO FROM MODELOCARTAO WITH (NOLOCK) WHERE CODMODELO = @CODMODELO";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODMODELO", DbType.String, ModeloCartao.CODMODELO);
            var CodModelo = Convert.ToString(db.ExecuteScalar(cmd, dbt));
            return !string.IsNullOrEmpty(CodModelo);
        }

        private static bool ExisteLogoCartaoAssociado(MODELOCARTAO ModeloCartao, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT TOP 1 CODMODELO1 FROM CLIENTEVA WITH (NOLOCK) WHERE CODMODELO1 = @CODMODELO " +
                                "UNION " +
                                "SELECT TOP 1 CODMODELO2 FROM CLIENTEVA WITH (NOLOCK) WHERE CODMODELO2 = @CODMODELO";

            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODMODELO", DbType.String, ModeloCartao.CODMODELO);
            var CodModelo = Convert.ToString(db.ExecuteScalar(cmd, dbt));
            return !string.IsNullOrEmpty(CodModelo);
        }

        public MODELOCARTAO GetModeloCartao(string codModelo)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT CODMODELO ");
            sql.AppendLine("FROM MODELOCARTAO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODMODELO = @CODMODELO");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODMODELO", DbType.String, codModelo);
            var idr = db.ExecuteReader(cmd);
            MODELOCARTAO modeloCartao = null;
            if (idr.Read())
            {
                modeloCartao = new MODELOCARTAO();
                modeloCartao.CODMODELO = Convert.ToString(idr["CODMODELO"]).Trim();
            }
            idr.Close();
            return modeloCartao;
        }

        public void InserirModeloCartao(MODELOCARTAO ModeloCartao)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle de transação
            var dbt = dbc.BeginTransaction();
            try
            {
                // Verifica se logo já existe
                if (ExisteLogoCartaoCadastrado(ModeloCartao, db, dbt))
                    throw new Exception("Modelo já existe. Favor verificar.");

                const string sql = "INSERT INTO MODELOCARTAO (CODMODELO, DESCRICAO) VALUES (@CODMODELO, @DESCRICAO)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODMODELO", DbType.String, ModeloCartao.CODMODELO);
                db.AddInParameter(cmd, "DESCRICAO", DbType.String, ModeloCartao.DESCRICAO);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICAÇÂO NOS DADOS (O cmd é pra listar o valor dos parâmetros, caso existam.)
                UtilSIL.GravarLog(db, dbt, "INSERT MODELOCARTAO", FOperador, cmd);
                dbt.Commit();

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

        public void AlterarModeloCartao(MODELOCARTAO ModeloCartao)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle de transação
            var dbt = dbc.BeginTransaction();

            try
            {
                const string sql = "UPDATE MODELOCARTAO SET CODMODELO = @CODMODELO WHERE CODMODELO = @CODMODELO";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODMODELO", DbType.String, ModeloCartao.CODMODELO);
                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICAÇÂO NOS DADOS (O cmd é pra listar o valor dos parâmetros, caso existam.)
                UtilSIL.GravarLog(db, dbt, "UPDATE MODELOCARTAO", FOperador, cmd);
                dbt.Commit();
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

        public void ExcluirModeloCartao(MODELOCARTAO ModeloCartao)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle de transação
            var dbt = dbc.BeginTransaction();
            try
            {
                if (ExisteLogoCartaoAssociado(ModeloCartao, db, dbt))
                    throw new Exception("Nao e possivel excluir registros que ja foram usados em outros cadastros.");

                const string sql = "DELETE MODELOCARTAO WHERE CODMODELO = @CODMODELO";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODMODELO", DbType.String, ModeloCartao.CODMODELO);
                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICAÇÂO NOS DADOS (O cmd é pra listar o valor dos parâmetros, caso existam.)
                UtilSIL.GravarLog(db, dbt, "DELETE MODELOCARTAO", FOperador, cmd);
                dbt.Commit();
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

        #region BENEFICIO

        public List<BENEFICIO> ListarBeneficio()
        {
            var ColecaoBeneficio = new List<BENEFICIO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("select * from BENEFICIO WITH (NOLOCK) ");
            

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var beneficio = new BENEFICIO();
                beneficio.CODBENEF = Convert.ToInt32(idr["CODBENEF"]);
                beneficio.ABREVBENEF = idr["ABREVBENEF"].ToString();
                beneficio.NOMBENEF = idr["NOMBENEF"].ToString();
                beneficio.STATUS = idr["STATUS"].ToString();
                beneficio.TIPTRA = Convert.ToInt32(idr["TIPTRA"]);
                beneficio.TRENOVA = idr["TRENOVA"].ToString();

                ColecaoBeneficio.Add(beneficio);
            }
            idr.Close();

            return ColecaoBeneficio;
        }

        public List<BENEFICIO> ListarBeneficioNaoAssosiado(int codcli)
        {
            var ColecaoBeneficio = new List<BENEFICIO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("select * from BENEFICIO WITH (NOLOCK) WHERE CODBENEF NOT IN (SELECT CODBENEF FROM BENEFCLI WITH (NOLOCK) WHERE CODCLI = " + codcli.ToString() + ")");


            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var beneficio = new BENEFICIO();
                beneficio.CODBENEF = Convert.ToInt32(idr["CODBENEF"]);
                beneficio.ABREVBENEF = idr["ABREVBENEF"].ToString();
                beneficio.NOMBENEF = idr["NOMBENEF"].ToString();
                beneficio.STATUS = idr["STATUS"].ToString();
                beneficio.TIPTRA = Convert.ToInt32(idr["TIPTRA"]);
                beneficio.TRENOVA = idr["TRENOVA"].ToString();

                ColecaoBeneficio.Add(beneficio);
            }
            idr.Close();

            return ColecaoBeneficio;
        }

        public List<BENEFICIO> ListarBeneficioAssosiado(int codcli)
        {
            var ColecaoBeneficio = new List<BENEFICIO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("select * from BENEFICIO WITH (NOLOCK) WHERE CODBENEF IN (SELECT CODBENEF FROM BENEFCLI WITH (NOLOCK) WHERE CODCLI = " + codcli.ToString() + ")");


            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var beneficio = new BENEFICIO();
                beneficio.CODBENEF = Convert.ToInt32(idr["CODBENEF"]);
                beneficio.ABREVBENEF = idr["ABREVBENEF"].ToString();
                beneficio.NOMBENEF = idr["NOMBENEF"].ToString();
                beneficio.STATUS = idr["STATUS"].ToString();
                beneficio.TIPTRA = Convert.ToInt32(idr["TIPTRA"]);
                beneficio.TRENOVA = idr["TRENOVA"].ToString();

                ColecaoBeneficio.Add(beneficio);
            }
            idr.Close();

            return ColecaoBeneficio;
        }

        public List<BENEFICIO_CLIENTE> ColecaoBeneficioAssociadoDoCliente(int codCli)
        {
            var ColecaoBeneficio = new List<BENEFICIO_CLIENTE>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = @"SELECT A.CODBENEF, 
                        B.NOMBENEF,
                        CASE WHEN A.COMPULSORIO = 'S' THEN 'COMPULSORIO' ELSE 'POR ADESAO' END TIPO,
                        A.VALTIT, 
                        A.VALDEP, 
                        A.DTASSOC
                        FROM BENEFCLI A WITH (NOLOCK) INNER JOIN BENEFICIO B WITH (NOLOCK) ON A.CODBENEF = B.CODBENEF
                        WHERE A.CODCLI = " + codCli;


            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var beneficio = new BENEFICIO_CLIENTE();
                beneficio.CODBENEF = Convert.ToInt32(idr["CODBENEF"]);
                beneficio.NOMBENEF = idr["NOMBENEF"].ToString();
                beneficio.TIPO = idr["TIPO"].ToString();
                beneficio.VALTIT = Convert.ToDecimal(idr["VALTIT"]);
                beneficio.VALDEP = Convert.ToDecimal(idr["VALDEP"]);
                beneficio.VALORTIT = Convert.ToDecimal(idr["VALTIT"]).ToString();
                beneficio.VALORDEP = Convert.ToDecimal(idr["VALDEP"]).ToString();

                beneficio.DTASSOC = idr["DTASSOC"].ToString();

                ColecaoBeneficio.Add(beneficio);
            }
            idr.Close();

            return ColecaoBeneficio;
        }

        #endregion

        #region ALERTAS

        public bool ExibeAlertas()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'HABALERTA'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public IEnumerable<ALERTA> ObterAlertas()
        {
            var listaAlerta = new List<ALERTA>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT A.ID_ALERTA, A.DATA, A.NIVEL, A.TIPO, A.CODCLI, A.CODCRE, A.ID_USUARIO, U.CODCRTMASC, U.CODCRT, ");
            sql.AppendLine("A.NUMDEP, A.NUMTIT, A.VISUALIZADO, A.ID_FUNC, AT.DESCRICAO AS DESTIPO, REF, AN.DESCRICAO AS DESNIVEL, O.LOGIN ");
            sql.AppendLine("FROM ALERTA A WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN ALERTATIPO AT WITH (NOLOCK) ON A.TIPO = AT.TIPO ");
            sql.AppendLine("INNER JOIN ALERTANIVEL AN WITH (NOLOCK) ON A.NIVEL = AN.NIVEL ");
            sql.AppendLine("LEFT JOIN OPERVAWS O WITH (NOLOCK) ON A.ID_FUNC = O.ID_FUNC ");
            sql.AppendLine("LEFT JOIN VRESUMOUSU U WITH (NOLOCK) ON A.ID_USUARIO = U.ID_USUARIO AND A.CODCLI = U.CODCLI AND A.CODCLI = U.CODCLI AND A.NUMDEP = U.NUMDEP ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var alerta = new ALERTA();
                alerta.ID_ALERTA = Convert.ToInt32(idr["ID_ALERTA"]);
                alerta.REF = Convert.ToString(idr["REF"]);
                alerta.DATA  = Convert.ToDateTime(idr["DATA"]);
                alerta.NIVEL  = Convert.ToInt32(idr["NIVEL"]);
                alerta.TIPO  = Convert.ToInt32(idr["TIPO"]);
                if (idr["CODCLI"] != DBNull.Value) alerta.CODCLI  = Convert.ToInt32(idr["CODCLI"]);
                if (idr["CODCRE"] != DBNull.Value) alerta.CODCRE  = Convert.ToInt32(idr["CODCRE"]);
                if (idr["ID_USUARIO"] != DBNull.Value) alerta.ID_USUARIO  = Convert.ToInt32(idr["ID_USUARIO"]);
                alerta.CODCRT = Convert.ToString(idr["CODCRT"]);
                alerta.CODCRTMASC = Convert.ToString(idr["CODCRTMASC"]);
                if (idr["NUMDEP"] != DBNull.Value) alerta.NUMDEP  = Convert.ToInt32(idr["NUMDEP"]);
                if (idr["NUMTIT"] != DBNull.Value) alerta.NUMTIT  = Convert.ToInt32(idr["NUMTIT"]);
                alerta.VISUALIZADO  = Convert.ToString(idr["VISUALIZADO"]);
                if (idr["ID_FUNC"] != DBNull.Value) alerta.ID_FUNC  = Convert.ToInt32(idr["ID_FUNC"]);
                alerta.LOGIN = Convert.ToString(idr["LOGIN"]);
                alerta.DESTIPO  = Convert.ToString(idr["DESTIPO"]);
                alerta.DESNIVEL  = Convert.ToString(idr["DESNIVEL"]);        

                listaAlerta.Add(alerta);
            }
            idr.Close();

            return listaAlerta;
        }

        public int CountAlertas()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT COUNT(*) FROM ALERTA WITH (NOLOCK) WHERE ID_FUNC IS NULL ");
            
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        public void MarcarAlertaLido(int idAlerta)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE ALERTA SET ID_FUNC = @ID_FUNC WHERE ID_ALERTA = @ID_ALERTA";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "ID_ALERTA", DbType.Int32, idAlerta);
                db.AddInParameter(cmd, "ID_FUNC", DbType.Int32, FOperador.ID_FUNC);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "UPDATE ALERTA", FOperador, cmd);
                dbt.Commit();
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
    }
}