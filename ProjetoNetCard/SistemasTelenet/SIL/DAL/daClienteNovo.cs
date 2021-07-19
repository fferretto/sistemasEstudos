using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SIL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using TELENET.SIL.PO;


namespace TELENET.SIL.DA
{
    public class daCLIENTENovo
    {
        readonly string BDTELENET = string.Empty;
        readonly string BDAUTORIZADOR =  string.Empty;
        readonly OPERADORA FOperador;
        public daCLIENTENovo(OPERADORA Operador)
        {
            FOperador = Operador;

            // Monta String Conecao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDAUTORIZADOR = string.Format(ConstantesSIL.BDAUTORIZADOR, Operador.SERVIDORAUT, Operador.BANCOAUT, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        #region GET Cliente

        public CADCLIENTE GetCliente(int idCliente)
        {
            var cliente = new CADCLIENTE();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT");
            sql.AppendLine(" ID_CLIENTE, NOMCLI, SISTEMA, PRODUTO, CODCLI, CNPJ, STA, DESTA ");
            sql.AppendLine(" DATINC, INSEST, ENDCLI, CEP, NOMBAI, NOMLOC, NOMUF0, ");
            sql.AppendLine(" ENDCPL, DESREG, NOMATI, UNIDADE, SETOR, PORTE ");
            sql.AppendLine(" FROM VRESUMOCLI WITH (NOLOCK) ");
            sql.AppendLine(" WHERE ID_CLIENTE = @ID_CLIENTE ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ID_CLIENTE", DbType.Int32, idCliente);
            var idr = db.ExecuteReader(cmd);

            if (idr.Read())
            {
                cliente.ID_CLIENTE = idCliente;
                if (idr["SISTEMA"] != DBNull.Value)
                    cliente.SISTEMA = Convert.ToInt32(idr["SISTEMA"]);
                if (idr["CODCLI"] != DBNull.Value) cliente.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                cliente.PRODUTO = Convert.ToString(idr["PRODUTO"]);
                cliente.BAIRRO = Convert.ToString(idr["NOMBAI"]);
                cliente.REGIAO = Convert.ToString(idr["DESREG"]);
                cliente.RAMOATI = Convert.ToString(idr["NOMATI"]);
                cliente.UNIDADE = Convert.ToString(idr["UNIDADE"]);
                cliente.SETOR = Convert.ToString(idr["SETOR"]);
                cliente.PORTE = Convert.ToString(idr["PORTE"]);
                cliente.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                cliente.CNPJ = Convert.ToString(idr["CNPJ"]);
                cliente.INSEST = Convert.ToString(idr["INSEST"]);
                cliente.ENDCLI = Convert.ToString(idr["ENDCLI"]);
                cliente.CEP = Convert.ToString(idr["CEP"]);
                cliente.LOCALIDADE = Convert.ToString(idr["NOMLOC"]);
                cliente.UF = Convert.ToString(idr["NOMUF0"]);
                cliente.ENDCPL = Convert.ToString(idr["ENDCPL"]);                
            }
            idr.Close();

            return cliente; 
        }

        public List<VPRODUTOSCLI> GetClienteProd(int idCliente)
        {            
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT");
            sql.AppendLine(" ID_CLIENTE, CODCLI, NOMCLI, A.SISTEMA, A.CODPROD, PRODUTO, CNPJ, STA, DATINC, DATCTT, CTRATV, PRAPAG, NUMCRT, DATFEC, NUMFEC, ");
            sql.AppendLine(" DATULTFEC, DATPRCULTFEC, DATPROXFEC, EXIREC, EXIBEMENS, CODFILNUT, TIPDES, SUBMED, PAGANU, CONPMO, COB2AV, VAL2AV, NOMGRA, VALTOTCRE, TOTCREDUTIL, ");
            sql.AppendLine(" OUTCRT, OUTLAY, MAXPARC, PMOEXCSEG, ORDEMCL, COBRAANU, ANUIPERIODO, NUMANUICOB, LIMRISCO, COBATV, CODREO, CODEPS, ");
            sql.AppendLine(" VALATV, COBANUIMOV, DADIAMENTO, CRTINCBLQ, SUBTIT, CODMENS, SALDOFUNC, PROXCONPMO, COBCONS, VALCONS, VERTOTCRE, NAOVERIFCPF, ");
            sql.AppendLine(" NRENOVPMO, RENOVPGTO, LIBLIMWEB, TIPFEC, TIPPAG, TXJUROS, TXSERVIC, HABFIN, CODFIN, SUBREDE, PRODUTO, GRUPOSOCIETARIO, PARCERIA, ");
            sql.AppendLine(" PRZVALCART, CARENCIATROCACANC, CODLOGO1, CODLOGO2, CODMODELO1, CODMODELO2, DATSTA, DATULTCARG_VA, NUMCARG_VA, PRAPAG_VA, DATCTT_VA, TAXSER_VA, ");
            sql.AppendLine(" NSUCARGA, DIASVALSALDO, LIMMAXCAR, TIPOTAXSER, CARGPADVA, NEGARCARGASALDOACIMA, SALDOCONTA, PGTOANTECIPADO, HABCARGASEQ, TAXADM_VA, VALINCTIT, VALINCDEP, COBINC, ");
            sql.AppendLine(" TEL, FAX, EMA, CON, ENDEDC, ENDCPLEDC, NOMBAIEDC, NOMLOCEDC, NOMUF0EDC, CEPEDC, RESEDC, NOMGRA, A.VANTECIPOU, A.ATIVADOS, A.NAO_ATIVADOS, A.BLOQUEADOS ");
            sql.AppendLine(" FROM VPRODUTOSCLI A WITH (NOLOCK)");
            sql.AppendLine("INNER JOIN PRODUTO B WITH (NOLOCK) ON A.CODPROD = B.CODPROD");
            sql.AppendLine(" WHERE ID_CLIENTE = @ID_CLIENTE ");
            sql.AppendLine("UNION ");
            sql.AppendLine("SELECT");
            sql.AppendLine("  @ID_CLIENTE, 0, NULL, ");
            sql.AppendLine(" CASE WHEN A.SISTEMA = 'PJ' THEN  0 ELSE  1 END AS SISTEMA, A.CODPROD, A.ROTULO, ");
            sql.AppendLine(" NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, ");
            sql.AppendLine(" NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, ");
            sql.AppendLine(" NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, ");
            sql.AppendLine(" NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, ");
            sql.AppendLine(" NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, ");
            sql.AppendLine(" NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, ");
            sql.AppendLine(" NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, ");
            sql.AppendLine(" NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, ");
            sql.AppendLine(" NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, ");
            sql.AppendLine(" NULL, NULL, NULL ");
            sql.AppendLine("FROM PRODUTO A WITH (NOLOCK) ");
            sql.AppendLine("ORDER BY CNPJ DESC, CODCLI");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ID_CLIENTE", DbType.Int32, idCliente);

            var idr = db.ExecuteReader(cmd);
            var listProdutosCli = new List<VPRODUTOSCLI>();

            while (idr.Read())
            {
                var clienteProd = new VPRODUTOSCLI();
                clienteProd.ID_CLIENTE = idCliente;
                clienteProd.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                clienteProd.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                clienteProd.SISTEMA = Convert.ToInt16(idr["SISTEMA"]);
                clienteProd.CODPROD = Convert.ToInt16(idr["CODPROD"]);
                clienteProd.PRODUTO = Convert.ToString(idr["PRODUTO"]);
                clienteProd.CNPJ = Convert.ToString(idr["CNPJ"]);
                clienteProd.STA = Convert.ToString(idr["STA"]);
                clienteProd.VANTECIPOU = Convert.ToString(idr["VANTECIPOU"]);
                if (!string.IsNullOrEmpty(clienteProd.STA))
                {
                    if (idr["DATSTA"] != DBNull.Value) clienteProd.DATSTA = Convert.ToDateTime(idr["DATSTA"]);
                    if (Convert.ToDateTime(idr["DATINC"]) != DateTime.MinValue ) clienteProd.DATINC = Convert.ToDateTime(idr["DATINC"]);
                    if (idr["CTRATV"] != DBNull.Value) clienteProd.CTRATV = Convert.ToChar(idr["CTRATV"]);
                    if (clienteProd.SISTEMA == 0 && idr["PRAPAG"] != DBNull.Value) clienteProd.PRAPAG = Convert.ToInt16(idr["PRAPAG"]);
                    if (idr["NUMCRT"] != DBNull.Value) clienteProd.NUMCRT = Convert.ToInt32(idr["NUMCRT"]);
                    if (idr["TIPFEC"] != DBNull.Value) clienteProd.TIPFEC = Convert.ToInt16(idr["TIPFEC"]);
                    if (idr["TIPPAG"] != DBNull.Value) clienteProd.TIPPAG = Convert.ToChar(idr["TIPPAG"]);
                    if (idr["DATFEC"] != DBNull.Value) clienteProd.DATFEC = Convert.ToInt16(idr["DATFEC"]);
                    if (idr["NUMFEC"] != DBNull.Value) clienteProd.NUMFEC = Convert.ToInt16(idr["NUMFEC"]);
                    if (idr["DATULTFEC"] != DBNull.Value && Convert.ToDateTime(idr["DATULTFEC"]) != DateTime.MinValue) clienteProd.DATULTFEC = Convert.ToDateTime(idr["DATULTFEC"]);
                    if (clienteProd.SISTEMA == 0 && Convert.ToDateTime(idr["DATCTT"]) != DateTime.MinValue) clienteProd.DATCTT = Convert.ToDateTime(idr["DATCTT"]);
                    if (clienteProd.SISTEMA == 0)
                    {
                        if (idr["DATPROXFEC"] != DBNull.Value && Convert.ToDateTime(idr["DATPROXFEC"]) != DateTime.MinValue)
                            clienteProd.DATPROXFEC = Convert.ToDateTime(idr["DATPROXFEC"]);
                    }
                    if (clienteProd.SISTEMA == 0) clienteProd.POSSUIPREMIO = PosssuiPremio(idCliente);
                    if (clienteProd.SISTEMA == 0 && idr["EXIREC"] != DBNull.Value) clienteProd.EXIREC = Convert.ToChar(idr["EXIREC"]);
                    if (clienteProd.SISTEMA == 0 && idr["EXIBEMENS"] != DBNull.Value) clienteProd.EXIBMES = Convert.ToChar(idr["EXIBEMENS"]);
                    if (idr["CODFILNUT"] != DBNull.Value) clienteProd.CODFILNUT = Convert.ToInt32(idr["CODFILNUT"]);
                    if (clienteProd.SISTEMA == 0 && idr["TIPDES"] != DBNull.Value) clienteProd.TIPDES = Convert.ToString(idr["TIPDES"]);
                    if (clienteProd.SISTEMA == 0 && idr["SUBMED"] != DBNull.Value) clienteProd.SUBMED = Convert.ToChar(idr["SUBMED"]);
                    if (clienteProd.SISTEMA == 0 && idr["PAGANU"] != DBNull.Value) clienteProd.PAGANU = Convert.ToChar(idr["PAGANU"]);
                    if (clienteProd.SISTEMA == 0 && idr["CONPMO"] != DBNull.Value) clienteProd.CONPMO = Convert.ToChar(idr["CONPMO"]);
                    if (idr["COB2AV"] != DBNull.Value) clienteProd.COB2AV = Convert.ToChar(idr["COB2AV"]);
                    if (idr["VAL2AV"] != DBNull.Value) clienteProd.VAL2AV = Convert.ToDecimal(idr["VAL2AV"]);
                    if (idr["CODREO"] != DBNull.Value) clienteProd.CODREO = Convert.ToInt32(idr["CODREO"]);
                    if (idr["CODEPS"] != DBNull.Value) clienteProd.CODEPS = Convert.ToInt32(idr["CODEPS"]);
                    if (idr["NOMGRA"] != DBNull.Value) clienteProd.NOMGRA = Convert.ToString(idr["NOMGRA"]);
                    if (clienteProd.SISTEMA == 0 && idr["VALTOTCRE"] != DBNull.Value) clienteProd.VALTOTCRE = Convert.ToDecimal(idr["VALTOTCRE"]);
                    if (clienteProd.SISTEMA == 0 && idr["TOTCREDUTIL"] != DBNull.Value) clienteProd.TOTCREDUTIL = Convert.ToDecimal(idr["TOTCREDUTIL"]);
                    if (clienteProd.SISTEMA == 0 && idr["OUTCRT"] != DBNull.Value) clienteProd.OUTCRT = Convert.ToChar(idr["OUTCRT"]);
                    if (clienteProd.SISTEMA == 0 && idr["MAXPARC"] != DBNull.Value) clienteProd.MAXPARC = Convert.ToInt16(idr["MAXPARC"]);
                    if (clienteProd.SISTEMA == 0 && idr["PMOEXCSEG"] != DBNull.Value) clienteProd.PMOEXCSEG = Convert.ToInt32(idr["PMOEXCSEG"]);
                    if (clienteProd.SISTEMA == 0 && idr["ORDEMCL"] != DBNull.Value) clienteProd.ORDEMCL = Convert.ToString(idr["ORDEMCL"]);
                    if (clienteProd.SISTEMA == 0 && idr["COBRAANU"] != DBNull.Value) clienteProd.COBRAANU = Convert.ToChar(idr["COBRAANU"]);
                    if (clienteProd.SISTEMA == 0 && idr["ANUIPERIODO"] != DBNull.Value) clienteProd.ANUIPERIODO = Convert.ToChar(idr["ANUIPERIODO"]);
                    if (clienteProd.SISTEMA == 0 && idr["NUMANUICOB"] != DBNull.Value) clienteProd.NUMANUICOB = Convert.ToInt16(idr["NUMANUICOB"]);
                    if (clienteProd.SISTEMA == 0 && idr["LIMRISCO"] != DBNull.Value) clienteProd.LIMRISCO = Convert.ToInt16(idr["LIMRISCO"]);
                    if (clienteProd.SISTEMA == 0 && idr["COBATV"] != DBNull.Value) clienteProd.COBATV = Convert.ToChar(idr["COBATV"]);
                    if (clienteProd.SISTEMA == 0 && idr["VALATV"] != DBNull.Value) clienteProd.VALATV = Convert.ToDecimal(idr["VALATV"]);
                    if (clienteProd.SISTEMA == 0 && idr["COBANUIMOV"] != DBNull.Value) clienteProd.COBANUIMOV = Convert.ToChar(idr["COBANUIMOV"]);
                    if (clienteProd.SISTEMA == 0 && idr["DADIAMENTO"] != DBNull.Value) clienteProd.DADIAMENTO = Convert.ToInt16(idr["DADIAMENTO"]);
                    if (idr["CRTINCBLQ"] != DBNull.Value) clienteProd.CRTINCBLQ = Convert.ToChar(idr["CRTINCBLQ"]);
                    if (clienteProd.SISTEMA == 0 && idr["SUBTIT"] != DBNull.Value) clienteProd.SUBTIT = Convert.ToChar(idr["SUBTIT"]);
                    if (clienteProd.SISTEMA == 0 && idr["CODMENS"] != DBNull.Value) clienteProd.CODMENS = Convert.ToInt32(idr["CODMENS"]);
                    if (clienteProd.SISTEMA == 0 && idr["SALDOFUNC"] != DBNull.Value) clienteProd.SALDOFUNC = Convert.ToChar(idr["SALDOFUNC"]);
                    if (clienteProd.SISTEMA == 0 && idr["PROXCONPMO"] != DBNull.Value) clienteProd.PROXCONPMO = Convert.ToChar(idr["PROXCONPMO"]);
                    if (idr["COBCONS"] != DBNull.Value) clienteProd.COBCONS = Convert.ToChar(idr["COBCONS"]);
                    if (idr["VALCONS"] != DBNull.Value) clienteProd.VALCONS = Convert.ToDecimal(idr["VALCONS"]);
                    if (clienteProd.SISTEMA == 0 && idr["VERTOTCRE"] != DBNull.Value) clienteProd.VERTOTCRE = Convert.ToChar(idr["VERTOTCRE"]);
                    if (clienteProd.SISTEMA == 0 && idr["NAOVERIFCPF"] != DBNull.Value) clienteProd.NAOVERIFCPF = Convert.ToChar(idr["NAOVERIFCPF"]);
                    if (clienteProd.SISTEMA == 0 && idr["NRENOVPMO"] != DBNull.Value) clienteProd.NRENOVPMO = Convert.ToChar(idr["NRENOVPMO"]);
                    if (clienteProd.SISTEMA == 0 && idr["RENOVPGTO"] != DBNull.Value) clienteProd.RENOVPGTO = Convert.ToChar(idr["RENOVPGTO"]);                    
                    if (clienteProd.SISTEMA == 0 && idr["LIBLIMWEB"] != DBNull.Value) clienteProd.LIBLIMWEB = Convert.ToChar(idr["LIBLIMWEB"]);
                    if (clienteProd.SISTEMA == 0 && idr["TIPFEC"] != DBNull.Value) clienteProd.TIPFEC = Convert.ToInt32(idr["TIPFEC"]);
                    if (clienteProd.SISTEMA == 0 && idr["TXJUROS"] != DBNull.Value) clienteProd.TXJUROS = Convert.ToDecimal(idr["TXJUROS"]);
                    if (clienteProd.SISTEMA == 0 && idr["TXSERVIC"] != DBNull.Value) clienteProd.TXSERVIC = Convert.ToDecimal(idr["TXSERVIC"]);
                    if (clienteProd.SISTEMA == 0 && idr["HABFIN"] != DBNull.Value) clienteProd.HABFIN = Convert.ToChar(idr["HABFIN"]);
                    if (clienteProd.SISTEMA == 0 && idr["CODFIN"] != DBNull.Value) clienteProd.CODFIN = Convert.ToString(idr["CODFIN"]);
                    if (idr["SUBREDE"] != DBNull.Value) clienteProd.SUBREDE = Convert.ToString(idr["SUBREDE"]);
                    if (idr["PRODUTO"] != DBNull.Value) clienteProd.PRODUTO = Convert.ToString(idr["PRODUTO"]);
                    if (idr["GRUPOSOCIETARIO"] != DBNull.Value) clienteProd.GRUPOSOCIETARIO = Convert.ToString(idr["GRUPOSOCIETARIO"]);
                    if (idr["PARCERIA"] != DBNull.Value) clienteProd.PARCERIA = Convert.ToString(idr["PARCERIA"]);
                    if (idr["CARENCIATROCACANC"] != DBNull.Value) clienteProd.CARENCIATROCACANC = Convert.ToInt16(idr["CARENCIATROCACANC"]);
                    if (idr["PRZVALCART"] != DBNull.Value) clienteProd.PRZVALCART = Convert.ToString(idr["PRZVALCART"]);
                    if (idr["CODLOGO1"] != DBNull.Value) clienteProd.CODLOGO1 = Convert.ToString(idr["CODLOGO1"]);
                    if (idr["CODLOGO2"] != DBNull.Value) clienteProd.CODLOGO2 = Convert.ToString(idr["CODLOGO2"]);
                    if (idr["CODMODELO1"] != DBNull.Value) clienteProd.CODMODELO1 = Convert.ToString(idr["CODMODELO1"]);
                    if (idr["CODMODELO2"] != DBNull.Value) clienteProd.CODMODELO2 = Convert.ToString(idr["CODMODELO2"]);
                    if (clienteProd.SISTEMA == 1 && Convert.ToDateTime(idr["DATCTT_VA"]) != DateTime.MinValue) clienteProd.DATCTT= Convert.ToDateTime(idr["DATCTT_VA"]);
                    if (clienteProd.SISTEMA == 1 && idr["DATULTCARG_VA"] != DBNull.Value) clienteProd.DATULTCARG_VA = Convert.ToDateTime(idr["DATULTCARG_VA"]);
                    if (clienteProd.SISTEMA == 1 && idr["NUMCARG_VA"] != DBNull.Value) clienteProd.NUMCARG_VA = Convert.ToInt32(idr["NUMCARG_VA"]);
                    if (clienteProd.SISTEMA == 1 && idr["PRAPAG_VA"] != DBNull.Value) clienteProd.PRAPAG_VA = Convert.ToInt16(idr["PRAPAG_VA"]);
                    if (clienteProd.SISTEMA == 1 && idr["TAXSER_VA"] != DBNull.Value) clienteProd.TAXSER_VA = Convert.ToDecimal(idr["TAXSER_VA"]);
                    if (clienteProd.SISTEMA == 1 && idr["NSUCARGA"] != DBNull.Value) clienteProd.NSUCARGA = Convert.ToInt32(idr["NSUCARGA"]);
                    if (clienteProd.SISTEMA == 1 && idr["DIASVALSALDO"] != DBNull.Value) clienteProd.DIASVALSALDO = Convert.ToInt16(idr["DIASVALSALDO"]);
                    if (clienteProd.SISTEMA == 1 && idr["LIMMAXCAR"] != DBNull.Value) clienteProd.LIMMAXCAR = Convert.ToDecimal(idr["LIMMAXCAR"]);
                    if (clienteProd.SISTEMA == 1 && idr["TIPOTAXSER"] != DBNull.Value) clienteProd.TIPOTAXSER = Convert.ToChar(idr["TIPOTAXSER"]);
                    if (clienteProd.SISTEMA == 1 && idr["CARGPADVA"] != DBNull.Value) clienteProd.CARGPADVA = Convert.ToDecimal(idr["CARGPADVA"]);
                    if (clienteProd.SISTEMA == 1 && idr["NEGARCARGASALDOACIMA"] != DBNull.Value) clienteProd.NEGARCARGASALDOACIMA = Convert.ToDecimal(idr["NEGARCARGASALDOACIMA"]);
                    if (clienteProd.SISTEMA == 1 && idr["SALDOCONTA"] != DBNull.Value) clienteProd.SALDOCONTA = Convert.ToDecimal(idr["SALDOCONTA"]);
                    if (clienteProd.SISTEMA == 1 && idr["PGTOANTECIPADO"] != DBNull.Value) clienteProd.PGTOANTECIPADO = Convert.ToChar(idr["PGTOANTECIPADO"]);
                    if (clienteProd.SISTEMA == 1 && idr["HABCARGASEQ"] != DBNull.Value) clienteProd.HABCARGASEQ = Convert.ToChar(idr["HABCARGASEQ"]);
                    if (clienteProd.SISTEMA == 1 && idr["TAXADM_VA"] != DBNull.Value) clienteProd.TAXADM_VA = Convert.ToDecimal(idr["TAXADM_VA"]);
                    if (clienteProd.SISTEMA == 1 && idr["VALINCTIT"] != DBNull.Value) clienteProd.VALINCTIT = Convert.ToDecimal(idr["VALINCTIT"]);
                    if (clienteProd.SISTEMA == 1 && idr["VALINCDEP"] != DBNull.Value) clienteProd.VALINCDEP = Convert.ToDecimal(idr["VALINCDEP"]);
                    if (clienteProd.SISTEMA == 1 && idr["COBINC"] != DBNull.Value) clienteProd.COBINC = Convert.ToInt16(idr["COBINC"]) != 0;
                    if (idr["TEL"] != DBNull.Value) clienteProd.TEL = Convert.ToString(idr["TEL"]);
                    if (idr["FAX"] != DBNull.Value) clienteProd.FAX = Convert.ToString(idr["FAX"]);
                    if (idr["EMA"] != DBNull.Value) clienteProd.EMA = Convert.ToString(idr["EMA"]);
                    if (idr["CON"] != DBNull.Value) clienteProd.CON = Convert.ToString(idr["CON"]);

                    if (idr["ENDEDC"] != DBNull.Value) clienteProd.ENDEDC = Convert.ToString(idr["ENDEDC"]);
                    if (idr["ENDCPLEDC"] != DBNull.Value) clienteProd.ENDCPLEDC = Convert.ToString(idr["ENDCPLEDC"]);
                    if (idr["NOMBAIEDC"] != DBNull.Value) clienteProd.BAIRROEDC = Convert.ToString(idr["NOMBAIEDC"]);
                    if (idr["NOMLOCEDC"] != DBNull.Value) clienteProd.LOCALIDADEEDC = Convert.ToString(idr["NOMLOCEDC"]);
                    if (idr["NOMUF0EDC"] != DBNull.Value) clienteProd.UFEDC = Convert.ToString(idr["NOMUF0EDC"]);
                    if (idr["CEPEDC"] != DBNull.Value) clienteProd.CEPEDC = Convert.ToString(idr["CEPEDC"]);
                    if (idr["RESEDC"] != DBNull.Value) clienteProd.RESEDC = Convert.ToString(idr["RESEDC"]);

                    if (idr["ATIVADOS"] != DBNull.Value) clienteProd.ATIVADOS = Convert.ToInt32(idr["ATIVADOS"]);
                    if (idr["NAO_ATIVADOS"] != DBNull.Value) clienteProd.NAO_ATIVADOS = Convert.ToInt32(idr["NAO_ATIVADOS"]);
                    if (idr["BLOQUEADOS"] != DBNull.Value) clienteProd.BLOQUEADOS = Convert.ToInt32(idr["BLOQUEADOS"]);
                }
                
                listProdutosCli.Add(clienteProd);
            }
            idr.Close();
            return listProdutosCli;
        }

        public decimal GetValGasto(VPRODUTOSCLI cliente)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_CONSULTA_GASTOCLI", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = cliente.SISTEMA;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = cliente.CODCLI;

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return Convert.ToDecimal(reader["VALGASTO"]);
                }
                return 0m;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public CARTAOTEMPORARIO GetCartaoTemporario(VPRODUTOSCLI cliente, out string retorno)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            retorno = string.Empty;
            var cartaoTemporario = new CARTAOTEMPORARIO();

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("RETORNA_PARAM_CPFTEMP", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = cliente.SISTEMA;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = cliente.CODCLI;
                cmd.Parameters.Add("@RESULTSET", SqlDbType.Char).Value = "S";

                var idr = cmd.ExecuteReader();
                if (idr.Read())
                {
                    if (Convert.ToInt32(idr["CODRETOUT"]) == 0)
                    {
                        cartaoTemporario.HABCPFTEMP = Convert.ToChar(idr["HABCPFTEMP"]);
                        cartaoTemporario.HABTROCACPFTEMP = Convert.ToChar(idr["HABTROCACPFTEMP"]);
                        cartaoTemporario.HABCARGACARTTEMP = Convert.ToChar(idr["HABCARGACARTTEMP"]);
                        cartaoTemporario.MAXCARTTEMP = Convert.ToInt32(idr["MAXCARTTEMP"]);
                        cartaoTemporario.QTCARTTEMP = Convert.ToInt32(idr["QTCARTTEMP"]);
                    }
                    else
                    {
                        retorno = Convert.ToString(idr["@MENSOUT"]);
                    }
                }                
                return cartaoTemporario;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        #endregion

        #region Segmentos

        public List<SEGAUTORIZVA_CLIENTE> SegmentosAutorizados(int IdClienteVA)
        {
            var ColecaoSegAutorizados = new List<SEGAUTORIZVA_CLIENTE>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("  S.CODSEG, S.NOMSEG");
            sql.AppendLine("FROM SEGMENTO S WITH (NOLOCK) ");
            sql.AppendLine("JOIN SEGAUTORIZVA SA WITH (NOLOCK)");
            sql.AppendLine("  ON SA.CODSEG = S.CODSEG");
            sql.AppendLine(" AND SA.CODCLI = @CODCLI");
            sql.AppendLine("ORDER BY S.NOMSEG");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.String, IdClienteVA);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var SegAutorizado = new SEGAUTORIZVA_CLIENTE();
                SegAutorizado.CODCLI = IdClienteVA;
                SegAutorizado.CODSEG = Convert.ToInt32(idr["CODSEG"]);
                SegAutorizado.NOMSEG = Convert.ToString(idr["NOMSEG"]).Trim();

                ColecaoSegAutorizados.Add(SegAutorizado);
            }
            idr.Close();

            return ColecaoSegAutorizados;
        }
        
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

        public List<SEG_GRUPO_DISPAUTORIZ> ListaSegGrupo(int sistema, int codCli, string tipo, string classif)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_LISTA_SEG_GRUPO_AUTORIZ", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = sistema;
                cmd.Parameters.Add("@CODCLI", SqlDbType.VarChar).Value = codCli;
                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = tipo;
                cmd.Parameters.Add("@CLASSIFIC", SqlDbType.VarChar).Value = classif;

                var listSeg = new List<SEG_GRUPO_DISPAUTORIZ>();

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var seg = new SEG_GRUPO_DISPAUTORIZ();
                    if (reader["CODSEG"] != DBNull.Value) seg.CODSEG = Convert.ToInt32(reader["CODSEG"]);
                    if (reader["NOMSEG"] != DBNull.Value) seg.NOMSEG = Convert.ToString(reader["NOMSEG"]);
                    if (reader["CODGRUPO"] != DBNull.Value) seg.CODGRUPO = Convert.ToInt32(reader["CODGRUPO"]);
                    if (reader["NOMGRUPO"] != DBNull.Value) seg.NOMGRUPO = Convert.ToString(reader["NOMGRUPO"]);
                    if (sistema == 0 && reader["PERLIM"] != DBNull.Value) seg.PERLIM = Convert.ToInt32(reader["PERLIM"]);
                    if (sistema == 0 && reader["PERLIMEXC"] != DBNull.Value) seg.PERLIMEXC = Convert.ToString(reader["PERLIMEXC"]);
                    if (sistema == 0 && reader["LIMRISCO"] != DBNull.Value) seg.LIMRISCO = Convert.ToInt32(reader["LIMRISCO"]);
                    if (sistema == 0 && reader["MAXPARC"] != DBNull.Value) seg.MAXPARC = Convert.ToInt32(reader["MAXPARC"]);
                    if (sistema == 0 && reader["PERSUB"] != DBNull.Value) seg.PERSUB = Convert.ToInt32(reader["PERSUB"]);

                    listSeg.Add(seg);
                }
                return listSeg;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        #endregion

        #region GET Grupos Autorizados

        public List<GRUPOSAUTORIZVA_CLIENTE> GruposAutorizados(int IdClienteVA)
        {
            var ColecaoGruposAutorizados = new List<GRUPOSAUTORIZVA_CLIENTE>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT");
            sql.AppendLine("  G.CODGRUPO, G.NOMGRUPO");
            sql.AppendLine("FROM GRUPO G WITH (NOLOCK) ");
            sql.AppendLine("JOIN SEGAUTORIZVA SA WITH (NOLOCK) ");
            sql.AppendLine("  ON SA.CODGRUPO = G.CODGRUPO");
            sql.AppendLine(" AND SA.CODCLI = @CODCLI");
            sql.AppendLine("ORDER BY G.NOMGRUPO");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.String, IdClienteVA);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var GrupoAutorizado = new GRUPOSAUTORIZVA_CLIENTE();
                GrupoAutorizado.CODCLI = IdClienteVA;
                GrupoAutorizado.CODGRUPO = Convert.ToInt32(idr["CODGRUPO"]);
                GrupoAutorizado.NOMGRUPO = Convert.ToString(idr["NOMGRUPO"]).Trim();
                ColecaoGruposAutorizados.Add(GrupoAutorizado);
            }
            idr.Close();
            return ColecaoGruposAutorizados;
        }

        public List<GRUPOSAUTORIZVA_CLIENTE> GruposDisponiveis(int IdClienteVA)
        {
            var ColecaoGruposAutorizados = new List<GRUPOSAUTORIZVA_CLIENTE>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT");
            sql.AppendLine("  G.CODGRUPO, G.NOMGRUPO");
            sql.AppendLine("FROM GRUPO G WITH (NOLOCK) ");
            sql.AppendLine("WHERE SISTEMA IN ('VA', '*') AND NOT EXISTS");
            sql.AppendLine("(SELECT SA.CODGRUPO");
            sql.AppendLine("  FROM SEGAUTORIZVA SA WITH (NOLOCK) ");
            sql.AppendLine(" WHERE SA.CODGRUPO = G.CODGRUPO");
            sql.AppendLine("   AND SA.CODCLI = @CODCLI)");
            sql.AppendLine("ORDER BY G.NOMGRUPO");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.String, IdClienteVA);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var GrupoAutorizado = new GRUPOSAUTORIZVA_CLIENTE();
                GrupoAutorizado.CODCLI = IdClienteVA;
                GrupoAutorizado.CODGRUPO = Convert.ToInt32(idr["CODGRUPO"]);
                GrupoAutorizado.NOMGRUPO = Convert.ToString(idr["NOMGRUPO"]).Trim();
                ColecaoGruposAutorizados.Add(GrupoAutorizado);
            }
            idr.Close();
            return ColecaoGruposAutorizados;
        }

        #endregion

        #region GET Proximo Codigo

        public int ProximoCodigoLivre(int sistema)
        {
            const string sql = "PROC_PROX_CODCLI";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, sistema);
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
            sql.AppendLine("FROM OPERADORMW WITH (NOLOCK) ");
            sql.AppendLine("WHERE (ID+1) NOT IN (SELECT ID FROM OPERADORMW WITH (NOLOCK))");

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
            sql.AppendLine("FROM CLIENTEVA WITH (NOLOCK) ");
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
            sql.AppendLine("FROM CLIENTEVA WITH (NOLOCK) ");
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

            sql.AppendLine("SELECT COUNT(G.CODGRUPO) FROM GRUPOCRED G WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN SEGAUTORIZVA S WITH (NOLOCK) ON S.CODGRUPO = G.CODGRUPO");
            sql.AppendLine("WHERE S.CODCLI = @CODCLI");
            sql.AppendLine("AND G.CODGRUPO <> @CODGRUPO");
            sql.AppendLine("AND EXISTS (SELECT G2.CODCRE FROM GRUPOCRED G2 WITH (NOLOCK) WHERE ");
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

        public List<CLIENTE_OBS> Observacoes(int sistema, int codCli)
        {
            var ColecaoObservacoes = new List<CLIENTE_OBS>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("CODCLI, DATA, OBS");
            sql.AppendLine(string.Format("FROM {0} WITH (NOLOCK) ", sistema == 0 ? "OBSCLI" : "OBSCLIVA"));
            sql.AppendLine("WHERE CODCLI = @CODCLI");

            DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.String, codCli);
            IDataReader idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var Observacao = new CLIENTE_OBS();
                Observacao.CODCLI = codCli;
                Observacao.DATA = Convert.ToDateTime(idr["DATA"]);
                Observacao.OBS = Convert.ToString(idr["OBS"]);

                ColecaoObservacoes.Add(Observacao);
            }
            idr.Close();

            return ColecaoObservacoes;
        }

        public List<CLIENTEVA_OBS> ObservacoesVa(Int32 Cliente)
        {
            var ColecaoObservacoes = new List<CLIENTEVA_OBS>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("  CODCLI, DATA, OBS, ID");
            sql.AppendLine("FROM OBSCLIVA WITH (NOLOCK) ");
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

        #region Sub-rede

        public bool ExibeSubRede()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'SUBREDE'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public int GetCodSubRede(string filtro)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CODSUBREDE FROM SUBREDE WITH (NOLOCK) ");
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
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'TAXACLIVA'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool ExibeTaxaIndividual()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'TAXAIND'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        #endregion

        #region Embosso

        public bool Embosso()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'ARQ_EMB_NOVO'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        #endregion

        #region Taxa Individual
        public bool ExibeCobrancaIndividual()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'TAXAIND'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool ExibeSensibilizaSaldo()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'TXSENSSALDO'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool ExibeExigeReceita()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'HABEXIGERECEITA'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }
        public bool ExibeExibeMensagem()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'HABEXIBEMENS'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        #endregion

        public bool ExibeTransfSaldoCli()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'CONTACLI'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public int GetValorTamanhoNomGraCli()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'TAM_NOM_GRA_CLI'";
            var cmd = db.GetSqlStringCommand(sql);
            int.TryParse(Convert.ToString(db.ExecuteScalar(cmd)), out int tamanho);
            tamanho = tamanho >= 27 ? 27 : tamanho <= 23 ? 23 : tamanho;
            return tamanho;
        }

        public CONSULTA GetPeriodoLoteCli(int codCli, int numFech)
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendLine(" SELECT DATINI, DATFIN FROM FECHCLIENTE WITH (NOLOCK) WHERE CODCLI = @CODCLI AND NUMFECCLI = @NUMFECCLI");                

                Database db = new SqlDatabase(BDTELENET);
                var cmd = db.GetSqlStringCommand(sql.ToString());
                db.AddInParameter(cmd, "CODCLI", DbType.String, codCli);
                db.AddInParameter(cmd, "NUMFECCLI", DbType.String, numFech);

                var idr = db.ExecuteReader(cmd);
                var periodo = new CONSULTA();
                if (idr.Read())
                {
                    periodo.PERIODO_INI = Convert.ToDateTime(idr["DATINI"]);
                    periodo.PERIODO_FIM = Convert.ToDateTime(idr["DATFIN"]);
                }
                else
                {
                    periodo.PERIODO_INI = DateTime.Now.AddDays(-30);
                    periodo.PERIODO_FIM = DateTime.Now;
                }
                idr.Close();
                return periodo;
            }
            catch (Exception) { throw; }
        }

        public bool ECrediHabita(int tipoProd)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT PENDPAPEL FROM TIPOPRODUTO WITH (NOLOCK) WHERE TIPOPROD = @TIPOPROD ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "TIPOPROD", DbType.Int32, tipoProd);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool ContaDigitalHabilitada(int tipoProd)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT CONTADIG FROM TIPOPRODUTO WITH (NOLOCK) WHERE TIPOPROD = @TIPOPROD ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "TIPOPROD", DbType.Int32, tipoProd);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool NegarCargaSaldo(int tipoProd)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT NEGARCARGASALDO FROM TIPOPRODUTO WITH (NOLOCK) WHERE TIPOPROD = @TIPOPROD ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "TIPOPROD", DbType.Int32, tipoProd);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool HabilitaEnvioCartas()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'HABEMAILBVINDAS'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool UsaRamoAtividade(int sistema)
        {
            Database db = new SqlDatabase(BDTELENET);
            var tabela = sistema == 0 ? "PARAM" : "PARAMVA";
            string sql = "SELECT VAL FROM " + tabela + " WITH (NOLOCK) WHERE ID0 = 'USARAMOATIV'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public int ObtemMascaraMaxCartTemp(int sistema)
        {
            Database db = new SqlDatabase(BDTELENET);
            var tabela = sistema == 0 ? "PARAM" : "PARAMVA";
            string sql = "SELECT VAL FROM " + tabela + " WITH (NOLOCK) WHERE ID0 = 'MASC_QTCARTTEMP'";
            var cmd = db.GetSqlStringCommand(sql);
            int.TryParse(Convert.ToString(db.ExecuteScalar(cmd)), out int tamanho);
            tamanho = tamanho <= 0 ? 3 : tamanho;
            return tamanho;
        }

        public int ObtemMascaraValorMaxCargaCartaoProduto(int tipoProd)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VALORMAXCARGACART FROM TIPOPRODUTO WITH (NOLOCK) WHERE TIPOPROD = @TIPOPROD ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "TIPOPROD", DbType.Int32, tipoProd);
            int.TryParse(Convert.ToString(db.ExecuteScalar(cmd)), out int tamanho);
            tamanho = tamanho <= 0 ? 8 : tamanho;
            return tamanho;
        }


        public bool HabilitaTaxaFaturamentoMinimo(int sistema)
        {
            var tabela = sistema == 0 ? "PARAM" : "PARAMVA";
            Database db = new SqlDatabase(BDTELENET);
            var sql = string.Format("SELECT VAL FROM {0} WITH (NOLOCK) WHERE ID0 = 'TXFATMIN'", tabela);
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public string GetStatus(int codCli)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT STA FROM CLIENTEVA WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODCLI = " + codCli);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            return Convert.ToString(db.ExecuteScalar(cmd));
        }

        public decimal ConsultaCargaPadCli(int codCli)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT CARGPADVA FROM CLIENTEVA WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODCLI = " + codCli);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            return Convert.ToDecimal(db.ExecuteScalar(cmd));
        }

        #region Colecao Arq. Cartoes

        public string ConfigJobs()
        {
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand("SELECT VAL FROM CONFIG_JOBS WITH (NOLOCK) WHERE ID0 = 'DIR_ARQ_IIS_EMBOSSO'");
            return Convert.ToString(db.ExecuteScalar(cmd));
        }

        public List<string> ColecaoArqCartoes(byte Tipo, Int32 Cliente, string Data, int sistema)
        {
            List<string> ColecaoLinhasCartoes = new List<string>();            
            
            Database db = new SqlDatabase(BDTELENET);
            string sql = sistema == 0 ? "SP_GERA_ARQ_EMB" : "SP_GERA_ARQ_EMB_VA";
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
            string sql = "SELECT NOM_PROC_EMBOSSO FROM PRODUTO WITH (NOLOCK) WHERE CODPROD = " + codProd;
            var cmd = db.GetSqlStringCommand(sql);
            var nomProc = db.ExecuteScalar(cmd);
            var retorno = string.Empty;
            if (nomProc != DBNull.Value)
                retorno = Convert.ToString(nomProc);
            return retorno;
        }

        public string ArqCartoesEmbosso(byte Tipo, Int32 Cliente, int sistema, string Data, string path, string nomProcEmbosso)
        {
            var ArqCartoesEmbosso = "";
            Database db = new SqlDatabase(BDTELENET);
            DbCommand cmd = db.GetStoredProcCommand(nomProcEmbosso);

            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, sistema);
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
            string sql = "SELECT DTEXPSENHA FROM OPERADORMW WITH (NOLOCK) WHERE ID = " + id;
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
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'PRZ_SENHA_INIC'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToInt16(db.ExecuteScalar(cmd));            
        }

        public List<ACESSOOPERADORMW> OperadoresWEBPos(Int32 Cliente, string Filtro)
        {
            var ColecaoOperadoresWEB = new List<ACESSOOPERADORMW>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ID, CODCLI, NOME, LOGIN, SISTEMA, SENHACRIP, SENHA, DTSENHA, CNPJ, FINCCART, FBLOQCART, FDESBLOQCART, FCANCCART, FALTLIMITE, ");
            sql.AppendLine("       FSEGVIACART, FEXTMOV, FCONSREDE, FLISTTRANSAB, FLISTCART, FLISTINCCART, FCARGA, FTRANSFSALDO, FHABCARGSEQ, FTRANSFSALDOCLI, FCARGAACIMALIMITE, DTEXPSENHA, QTDEACESSOINV, ");
            sql.AppendLine("       ULTACESSO, TIPOACESSO, ACESSOBLOQUEADO ");
            sql.AppendLine("FROM VOPERCLIWEB WITH (NOLOCK) WHERE CODCLI = @CODCLI ");
            sql.AppendLine("UNION ");
            sql.AppendLine("SELECT ID, '' AS CODCLI, NOME, LOGIN, SISTEMA, SENHACRIP, SENHA, DTSENHA, '' AS CNPJ, FINCCART, FBLOQCART, FDESBLOQCART, FCANCCART, FALTLIMITE, ");
            sql.AppendLine("       FSEGVIACART, FEXTMOV, FCONSREDE, FLISTTRANSAB, FLISTCART, FLISTINCCART, FCARGA, FTRANSFSALDO, FHABCARGSEQ, FTRANSFSALDOCLI, FCARGAACIMALIMITE, DTEXPSENHA, QTDEACESSOINV, ");
            sql.AppendLine("       ULTACESSO, TIPOACESSO, ACESSOBLOQUEADO ");
            sql.AppendLine("FROM VOPERPARWEB WITH (NOLOCK) WHERE CODPARCERIA IN (SELECT CODPARCERIA FROM CLIENTE WITH (NOLOCK) WHERE CODCLI = @CODCLI) ");
            
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
                Operador.SISTEMA = Convert.ToInt32(idr["SISTEMA"]);
                Operador.TIPOACESSO = Convert.ToString(idr["TIPOACESSO"]) == "C" ? "CLIENTE" : "PARCERIA";
                Operador.ACESSOBLOQUEADO = (idr["ACESSOBLOQUEADO"] != DBNull.Value) && (Convert.ToChar(idr["ACESSOBLOQUEADO"]) == ConstantesSIL.FlgSim);
                if (idr["DTEXPSENHA"] != DBNull.Value)
                    Operador.DTEXPSENHA =  Convert.ToDateTime(idr["DTEXPSENHA"]).ToShortDateString();
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
                Operador.FALTLIMITE = (idr["FALTLIMITE"] != DBNull.Value) && (Convert.ToChar(idr["FALTLIMITE"]) == ConstantesSIL.FlgSim);
                Operador.FLISTTRANSAB = (idr["FLISTTRANSAB"] != DBNull.Value) && (Convert.ToChar(idr["FLISTTRANSAB"]) == ConstantesSIL.FlgSim);

                Operador.FTRANSFSALDOCLI = (idr["FTRANSFSALDOCLI"] != DBNull.Value) && (Convert.ToChar(idr["FTRANSFSALDOCLI"]) == ConstantesSIL.FlgSim);
                Operador.FCARGA = (idr["FCARGA"] != DBNull.Value) && (Convert.ToChar(idr["FCARGA"]) == ConstantesSIL.FlgSim);
                Operador.FTRANSFSALDO = (idr["FTRANSFSALDO"] != DBNull.Value) && (Convert.ToChar(idr["FTRANSFSALDO"]) == ConstantesSIL.FlgSim);
                Operador.FCARGAACIMALIMITE = (idr["FCARGAACIMALIMITE"] != DBNull.Value) && (Convert.ToChar(idr["FCARGAACIMALIMITE"]) == ConstantesSIL.FlgSim);

                // Adiciona Item
                ColecaoOperadoresWEB.Add(Operador);
            }
            idr.Close();
            return ColecaoOperadoresWEB;
        }

        public List<ACESSOOPERADORMW> OperadoresWEBPre(Int32 Cliente, string Filtro)
        {
            var ColecaoOperadoresWEB = new List<ACESSOOPERADORMW>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ID, CODCLI, NOME, LOGIN, SISTEMA, SENHACRIP, SENHA, DTSENHA, CNPJ, FINCCART, FBLOQCART, FDESBLOQCART, FCANCCART, FALTLIMITE, ");
            sql.AppendLine("       FSEGVIACART, FEXTMOV, FCONSREDE, FLISTTRANSAB, FLISTCART, FLISTINCCART, FCARGA, FTRANSFSALDO, FHABCARGSEQ, FTRANSFSALDOCLI, FCARGAACIMALIMITE, DTEXPSENHA, QTDEACESSOINV, ");
            sql.AppendLine("       ULTACESSO, TIPOACESSO, ACESSOBLOQUEADO ");
            sql.AppendLine("FROM VOPERCLIVAWEB WITH (NOLOCK) WHERE CODCLI = @CODCLI ");
            sql.AppendLine("UNION ");
            sql.AppendLine("SELECT ID, '' AS CODCLI, NOME, LOGIN, SISTEMA, SENHACRIP, SENHA, DTSENHA, '' AS CNPJ, FINCCART, FBLOQCART, FDESBLOQCART, FCANCCART, FALTLIMITE, ");
            sql.AppendLine("       FSEGVIACART, FEXTMOV, FCONSREDE, FLISTTRANSAB, FLISTCART, FLISTINCCART, FCARGA, FTRANSFSALDO, FHABCARGSEQ, FTRANSFSALDOCLI, FCARGAACIMALIMITE, DTEXPSENHA, QTDEACESSOINV, ");
            sql.AppendLine("       ULTACESSO, TIPOACESSO, ACESSOBLOQUEADO ");
            sql.AppendLine("FROM VOPERPARVAWEB WITH (NOLOCK) WHERE CODPARCERIA IN (SELECT CODPARCERIA FROM CLIENTEVA WITH (NOLOCK) WHERE CODCLI = @CODCLI) ");

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
                Operador.SISTEMA = Convert.ToInt32(idr["SISTEMA"]);
                Operador.TIPOACESSO = Convert.ToString(idr["TIPOACESSO"]) == "C" ? "CLIENTE" : "PARCERIA";
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
                Operador.FCARGAACIMALIMITE = (idr["FCARGAACIMALIMITE"] != DBNull.Value) && (Convert.ToChar(idr["FCARGAACIMALIMITE"]) == ConstantesSIL.FlgSim);

                Operador.FALTLIMITE = (idr["FALTLIMITE"] != DBNull.Value) && (Convert.ToChar(idr["FALTLIMITE"]) == ConstantesSIL.FlgSim);
                Operador.FLISTTRANSAB = (idr["FLISTTRANSAB"] != DBNull.Value) && (Convert.ToChar(idr["FLISTTRANSAB"]) == ConstantesSIL.FlgSim);

                // Adiciona Item
                ColecaoOperadoresWEB.Add(Operador);
            }
            idr.Close();
            return ColecaoOperadoresWEB;
        }

        public List<ACESSOOPERADORMW> AcessoWEBParceria(string Filtro)
        {
            var ColecaoOperadoresWEB = new List<ACESSOOPERADORMW>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT O.Id, LOGIN, SISTEMA, NOME, DTSENHA, DTEXPSENHA, O.CODPARCERIA, SISTEMA, ACESSOBLOQUEADO, ");
            sql.AppendLine("FINCCART ,FBLOQCART ,FDESBLOQCART, FCANCCART, FALTLIMITE, FSEGVIACART, ");
            sql.AppendLine("FEXTMOV, FCONSREDE, FLISTTRANSAB, FLISTCART, FLISTINCCART, FCARGA, FTRANSFSALDO, FHABCARGSEQ, FTRANSFSALDOCLI, FCARGAACIMALIMITE ");
            sql.AppendLine("FROM OPERADORMW O WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN ACESSOOPERADORMW A WITH (NOLOCK) ON O.Id = A.IDOPEMW ");
            sql.AppendLine("WHERE O.CODPARCERIA IS NOT NULL AND TIPOACESSO = 'P' ");

            if (Filtro != string.Empty)
                sql.AppendLine(Filtro);

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var Operador = new ACESSOOPERADORMW();
                Operador.ID = Convert.ToInt32(idr["ID"]); 
                Operador.CODPARCERIA = Convert.ToInt32(idr["CODPARCERIA"]);
                Operador.NOME = Convert.ToString(idr["NOME"]);
                Operador.LOGIN = Convert.ToString(idr["LOGIN"]);
                Operador.SISTEMA = Convert.ToInt32(idr["SISTEMA"]);
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
                
                Operador.FALTLIMITE = (idr["FALTLIMITE"] != DBNull.Value) && (Convert.ToChar(idr["FALTLIMITE"]) == ConstantesSIL.FlgSim);
                Operador.FLISTTRANSAB = (idr["FLISTTRANSAB"] != DBNull.Value) && (Convert.ToChar(idr["FLISTTRANSAB"]) == ConstantesSIL.FlgSim);

                Operador.FTRANSFSALDOCLI = (idr["FTRANSFSALDOCLI"] != DBNull.Value) && (Convert.ToChar(idr["FTRANSFSALDOCLI"]) == ConstantesSIL.FlgSim);
                Operador.FCARGAACIMALIMITE = (idr["FCARGAACIMALIMITE"] != DBNull.Value) && (Convert.ToChar(idr["FCARGAACIMALIMITE"]) == ConstantesSIL.FlgSim);

                // Adiciona Item
                ColecaoOperadoresWEB.Add(Operador);
            }
            idr.Close();
            return ColecaoOperadoresWEB;
        }

        public List<OPERADORMW> OperadoresWEBParceria(string Filtro)
        {
            var ColecaoOperadoresWEB = new List<OPERADORMW>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT DISTINCT ID, LOGIN, NOME, DTSENHA, DTEXPSENHA, O.CODPARCERIA ");
            sql.AppendLine("FROM OPERADORMW O WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN ACESSOOPERADORMW A WITH (NOLOCK) ON O.ID = A.IDOPEMW ");
            sql.AppendLine("WHERE O.CODPARCERIA IS NOT NULL AND TIPOACESSO = 'P' ");

            if (Filtro != string.Empty)
                sql.AppendLine(Filtro);

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var Operador = new OPERADORMW();
                Operador.ID = Convert.ToInt32(idr["ID"]);
                Operador.CODPARCERIA = Convert.ToInt32(idr["CODPARCERIA"]);
                Operador.NOME = Convert.ToString(idr["NOME"]);
                Operador.LOGIN = Convert.ToString(idr["LOGIN"]);
                if (idr["DTEXPSENHA"] != DBNull.Value)
                    Operador.DTEXPSENHA = Convert.ToDateTime(idr["DTEXPSENHA"]).ToShortDateString();

                // Adiciona Item
                ColecaoOperadoresWEB.Add(Operador);
            }
            idr.Close();
            return ColecaoOperadoresWEB;
        }

        public ACESSOOPERADORMW OperadorWEB(Int32 Cliente, int ID)
        {
            ACESSOOPERADORMW Operador = null;
            var Filtro = " AND ID = " + ID.ToString();
            // Busca Item na Colecao 
            var ColecaoOperadores = OperadoresWEBPre(Cliente, Filtro);

            if (ColecaoOperadores.Count > 0)
                Operador = ColecaoOperadores[0];
            return Operador;
        }

        public List<ACESSOOPERADORMW> OperadorWEBParceria(int ID)
        {            
            var Filtro = " AND ID = " + ID.ToString();            
            var ColecaoOperadores = AcessoWEBParceria(Filtro);
            return ColecaoOperadores;
        }

        #endregion

        #region Cartoes Suspensos 

        public bool ExistemCartoesSuspensos(int Cliente)
        {
            Database db;
            string sql;
            DbCommand cmd;

            db = new SqlDatabase(BDAUTORIZADOR);
            sql = "SELECT COUNT(CODCARTAO) FROM CTCARTVA WITH (NOLOCK) WHERE CODEMPRESA = @CODCLI AND STATUSU = @StatusSuspenso";

            cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.String, Cliente.ToString().PadLeft(5, '0'));
            db.AddInParameter(cmd, "StatusSuspenso", DbType.String, ConstantesSIL.StatusSuspenso);
            return (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0);            
        }

        #endregion 

        #region CRUD SegAutorizados

        public string ManuSegGrupo(SEG_GRUPO_DISPAUTORIZ seg_grupo)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var retorno = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_MANU_SEG_GRUPO_AUTORIZ", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = seg_grupo.SISTEMA;
                cmd.Parameters.Add("@OPERACAO", SqlDbType.VarChar).Value = seg_grupo.OPERACAO;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = seg_grupo.CODCLI;
                cmd.Parameters.Add("@CODSEG", SqlDbType.Int).Value = seg_grupo.CODSEG;
                cmd.Parameters.Add("@CODGRUPO", SqlDbType.Int).Value = seg_grupo.CODGRUPO;
                cmd.Parameters.Add("@PERLIMEXC", SqlDbType.VarChar).Value = seg_grupo.PERLIMEXC;
                cmd.Parameters.Add("@PERLIM", SqlDbType.Int).Value = seg_grupo.PERLIM;
                cmd.Parameters.Add("@LIMRISCO", SqlDbType.Int).Value = seg_grupo.LIMRISCO;
                cmd.Parameters.Add("@MAXPARC", SqlDbType.Int).Value = seg_grupo.MAXPARC;
                cmd.Parameters.Add("@PERSUB", SqlDbType.Int).Value = seg_grupo.PERSUB;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retorno = Convert.ToString(reader["RETORNO"]);
                    if (retorno != "OK")
                    {
                        return retorno;                        
                    }
                }
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }
       
        #endregion

        #region CRUD GruposAutorizados

        public bool InserirGrupoAutorizado(int ClienteVA, int Grupo)
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

            if (ValidaGrupoCred(ClienteVA, Grupo))
                throw new Exception("O Grupo nao pode ser selecionado porque contem credenciado que pertence a outro grupo ja associado para este cliente.");
            try
            {
                sql = "INSERT INTO SEGAUTORIZVA (CODCLI, CODGRUPO) " +
                      "SELECT @CODCLI, @CODGRUPO " +
                      " WHERE NOT EXISTS " +
                      "(SELECT CODGRUPO FROM SEGAUTORIZVA WITH (NOLOCK) WHERE CODCLI = @CODCLI AND CODGRUPO = @CODGRUPO)";

                cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA);
                db.AddInParameter(cmd, "CODGRUPO", DbType.Int32, Grupo);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "INSERT SEGAUTORIZVA", FOperador, cmd);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Grupo]" + err);
            }
            finally
            {
                dbc.Close();

            }

            // AUTORIZADOR
            InserirAutorizadorGrupoAutorizado(ClienteVA, Grupo);

            // Sucesso
            return true;

        }

        // Inserir Todos Grupos Disponiveis
        public bool InserirGrupoAutorizado(int ClienteVA)
        {

            Database db;
            DbConnection dbc;
            DbTransaction dbt;
            StringBuilder sql;
            DbCommand cmd;

            db = new SqlDatabase(BDTELENET);
            dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            dbt = dbc.BeginTransaction();
            sql = new StringBuilder();
            try
            {
                sql.AppendLine("INSERT INTO SEGAUTORIZVA (CODCLI, CODGRUPO) ");
                sql.AppendLine("SELECT @CODCLI, G.CODGRUPO");
                sql.AppendLine("FROM GRUPO G WITH (NOLOCK) ");
                sql.AppendLine("WHERE NOT EXISTS");
                sql.AppendLine("(SELECT SA.CODGRUPO");
                sql.AppendLine("  FROM SEGAUTORIZVA SA WITH (NOLOCK)");
                sql.AppendLine(" WHERE SA.CODGRUPO = G.CODGRUPO");
                sql.AppendLine("   AND SA.CODCLI = @CODCLI)");

                cmd = db.GetSqlStringCommand(sql.ToString());

                db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "INSERT SEGAUTORIZVA", FOperador, cmd);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Grupos]" + err);
            }
            finally
            {
                dbc.Close();

            }

            // AUTORIZADOR
            InserirAutorizadorGrupoAutorizado(ClienteVA);

            // Sucesso
            return true;

        }

        public bool ExcluirGrupoAutorizado(int ClienteVA, int Grupo)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();

            try
            {
                const string sql = "DELETE FROM SEGAUTORIZVA WITH (NOLOCK) WHERE CODCLI = @CODCLI AND CODGRUPO = @CODGRUPO";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA);
                db.AddInParameter(cmd, "CODGRUPO", DbType.Int32, Grupo);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "DELETE SEGAUTORIZVA", FOperador, cmd);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Grupo]" + err);
            }
            finally
            {
                dbc.Close();

            }

            // AUTORIZADOR
            ExcluirAutorizadorGrupoAutorizado(ClienteVA, Grupo);

            // Sucesso
            return true;
        }

        // Excluir Todos Grupos Autorizados
        public bool ExcluirGrupoAutorizado(int ClienteVA)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();

            try
            {
                const string sql = "DELETE FROM SEGAUTORIZVA WITH (NOLOCK) WHERE CODCLI = @CODCLI AND CODGRUPO IS NOT NULL";
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

        public int Inserir(CADCLIENTE cadCliente, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;
            var idCliente = 0;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_INSERE_CLIENTE", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@NOMCLI", SqlDbType.VarChar).Value = cadCliente.NOMCLI;
                cmd.Parameters.Add("@CNPJ", SqlDbType.VarChar).Value = cadCliente.NOVOCNPJ ?? cadCliente.CNPJ;
                cmd.Parameters.Add("@INSEST", SqlDbType.VarChar).Value = cadCliente.INSEST;
                cmd.Parameters.Add("@REGIAO", SqlDbType.VarChar).Value = cadCliente.REGIAO;
                cmd.Parameters.Add("@RAMOATI", SqlDbType.VarChar).Value = cadCliente.RAMOATI;
                cmd.Parameters.Add("@UNIDADE", SqlDbType.VarChar).Value = cadCliente.UNIDADE;
                cmd.Parameters.Add("@SETOR", SqlDbType.VarChar).Value = cadCliente.SETOR;
                cmd.Parameters.Add("@PORTE", SqlDbType.VarChar).Value = cadCliente.PORTE;

                // Endereco Comercial
                cmd.Parameters.Add("@ENDCLI", SqlDbType.VarChar).Value = cadCliente.ENDCLI;
                cmd.Parameters.Add("@ENDCPL", SqlDbType.VarChar).Value = cadCliente.ENDCPL;
                cmd.Parameters.Add("@BAIRRO", SqlDbType.VarChar).Value = cadCliente.BAIRRO;
                cmd.Parameters.Add("@LOCALIDADE", SqlDbType.VarChar).Value = cadCliente.LOCALIDADE;
                cmd.Parameters.Add("@UF", SqlDbType.VarChar).Value = cadCliente.UF ?? string.Empty;
                cmd.Parameters.Add("@CEP", SqlDbType.VarChar).Value = cadCliente.CEP;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mensagem = Convert.ToString(reader["RETORNO"]);
                    if (mensagem == "0")
                    {
                        mensagem = "Registro incluído com sucesso.";
                        idCliente = Convert.ToInt32(reader["ID_CLIENTE"]);
                        return idCliente;
                    }
                    else
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return idCliente;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool InserirInfControlePos(VPRODUTOSCLI clienteProd, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_INSERE_CLIENTE_POS", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.Int).Value = clienteProd.ID_CLIENTE;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = clienteProd.CODCLI;
                cmd.Parameters.Add("@NOMGRA", SqlDbType.VarChar).Value = clienteProd.NOMGRA;
                cmd.Parameters.Add("@REGIONAL", SqlDbType.VarChar).Value = clienteProd.REGIONAL;
                cmd.Parameters.Add("@EPS", SqlDbType.VarChar).Value = clienteProd.EPS;                  
                cmd.Parameters.Add("@PRODUTO", SqlDbType.VarChar).Value = clienteProd.PRODUTO;
                cmd.Parameters.Add("@MAXPARC", SqlDbType.Int).Value = clienteProd.MAXPARC;
                cmd.Parameters.Add("@HABCPFTEMP", SqlDbType.VarChar).Value = clienteProd.HABCPFTEMP;
                cmd.Parameters.Add("@MAXCARTTEMP", SqlDbType.VarChar).Value = clienteProd.MAXCARTTEMP;
                cmd.Parameters.Add("@HABTROCACPFTEMP", SqlDbType.VarChar).Value = clienteProd.HABTROCACPFTEMP;
                cmd.Parameters.Add("@OUTCRT", SqlDbType.VarChar).Value = clienteProd.OUTCRT;
                cmd.Parameters.Add("@EXIREC", SqlDbType.VarChar).Value = clienteProd.EXIREC;
                cmd.Parameters.Add("@EXIBEMENS", SqlDbType.VarChar).Value = clienteProd.EXIBMES;
                cmd.Parameters.Add("@CRTINCBLQ", SqlDbType.VarChar).Value = clienteProd.CRTINCBLQ;
                cmd.Parameters.Add("@SUBTIT", SqlDbType.VarChar).Value = clienteProd.SUBTIT;
                cmd.Parameters.Add("@LIMRISCO", SqlDbType.Int).Value = clienteProd.LIMRISCO;
                cmd.Parameters.Add("@CARENCIATROCACANC", SqlDbType.Int).Value = clienteProd.CARENCIATROCACANC;
                cmd.Parameters.Add("@PRZVALCART", SqlDbType.VarChar).Value = clienteProd.PRZVALCART;
                cmd.Parameters.Add("@CONPMO", SqlDbType.VarChar).Value = clienteProd.CONPMO;
                cmd.Parameters.Add("@PROXCONPMO", SqlDbType.VarChar).Value = clienteProd.PROXCONPMO;
                cmd.Parameters.Add("@NRENOVPMO", SqlDbType.VarChar).Value = clienteProd.NRENOVPMO;
                cmd.Parameters.Add("@COB2AV", SqlDbType.VarChar).Value = clienteProd.COB2AV;
                cmd.Parameters.Add("@VAL2AV", SqlDbType.Decimal).Value = clienteProd.VAL2AV;
                cmd.Parameters.Add("@COBATV", SqlDbType.VarChar).Value = clienteProd.COBATV;
                cmd.Parameters.Add("@VALATV", SqlDbType.Decimal).Value = clienteProd.VALATV;
                cmd.Parameters.Add("@COBCONS", SqlDbType.VarChar).Value = clienteProd.COBCONS;
                cmd.Parameters.Add("@VALCONS", SqlDbType.Decimal).Value = clienteProd.VALCONS;
                cmd.Parameters.Add("@TIPDES", SqlDbType.VarChar).Value = clienteProd.TIPDES;
                cmd.Parameters.Add("@TIPFEC", SqlDbType.Int).Value = clienteProd.TIPFEC;
                cmd.Parameters.Add("@TIPPAG", SqlDbType.VarChar).Value = clienteProd.TIPPAG;
                cmd.Parameters.Add("@DATFEC", SqlDbType.Int).Value = clienteProd.DATFEC;
                cmd.Parameters.Add("@PRAPAG", SqlDbType.Int).Value = clienteProd.PRAPAG;
                cmd.Parameters.Add("@ORDEMCL", SqlDbType.VarChar).Value = clienteProd.ORDEMCL;
                
                if (clienteProd.DATCTT != DateTime.MinValue) cmd.Parameters.Add("@DATCTT", SqlDbType.DateTime).Value = clienteProd.DATCTT;
                cmd.Parameters.Add("@SUBREDE", SqlDbType.VarChar).Value = clienteProd.SUBREDE;
                cmd.Parameters.Add("@GRUPOSOCIETARIO", SqlDbType.VarChar).Value = clienteProd.GRUPOSOCIETARIO;
                cmd.Parameters.Add("@PARCERIA", SqlDbType.VarChar).Value = clienteProd.PARCERIA;
                cmd.Parameters.Add("@STA", SqlDbType.VarChar).Value = clienteProd.STACOD;
                cmd.Parameters.Add("@VALTOTCRE", SqlDbType.Decimal).Value = clienteProd.VALTOTCRE;
                
                cmd.Parameters.Add("@CODLOGO1", SqlDbType.VarChar).Value = clienteProd.CODLOGO1;
                cmd.Parameters.Add("@CODLOGO2", SqlDbType.VarChar).Value = clienteProd.CODLOGO2;
                cmd.Parameters.Add("@CODMODELO1", SqlDbType.VarChar).Value = clienteProd.CODMODELO1;
                cmd.Parameters.Add("@CODMODELO2", SqlDbType.VarChar).Value = clienteProd.CODMODELO2;

                cmd.Parameters.Add("@TEL", SqlDbType.VarChar).Value = clienteProd.TEL;
                cmd.Parameters.Add("@FAX", SqlDbType.VarChar).Value = clienteProd.FAX;
                cmd.Parameters.Add("@CON", SqlDbType.VarChar).Value = clienteProd.CON;
                cmd.Parameters.Add("@EMA", SqlDbType.VarChar).Value = clienteProd.EMA;

                // Endereco Entrega Cartoes 
                cmd.Parameters.Add("@ENDEDC", SqlDbType.VarChar).Value = clienteProd.ENDEDC;
                cmd.Parameters.Add("@ENDCPLEDC", SqlDbType.VarChar).Value = clienteProd.ENDCPLEDC;
                cmd.Parameters.Add("@BAIRROEDC", SqlDbType.VarChar).Value = clienteProd.BAIRROEDC;
                cmd.Parameters.Add("@LOCALIDADEEDC", SqlDbType.VarChar).Value = clienteProd.LOCALIDADEEDC;
                cmd.Parameters.Add("@UFEDC", SqlDbType.VarChar).Value = clienteProd.UFEDC ?? string.Empty;
                cmd.Parameters.Add("@CEPEDC", SqlDbType.VarChar).Value = clienteProd.CEPEDC;
                cmd.Parameters.Add("@RESEDC", SqlDbType.VarChar).Value = clienteProd.RESEDC;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mensagem = Convert.ToString(reader["RETORNO"]);
                    if (mensagem == "0")
                    {
                        mensagem = "Registro incluído com sucesso.";
                        return true;
                    }
                    else 
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }                
                if (conn != null) { conn.Close(); }
            }            
        }

        public bool InserirInfControlePre(VPRODUTOSCLI clienteProd, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_INSERE_CLIENTE_PRE", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.Int).Value = clienteProd.ID_CLIENTE;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = clienteProd.CODCLI;
                cmd.Parameters.Add("@NOMGRA", SqlDbType.VarChar).Value = clienteProd.NOMGRA;
                cmd.Parameters.Add("@REGIONAL", SqlDbType.VarChar).Value = clienteProd.REGIONAL;
                cmd.Parameters.Add("@EPS", SqlDbType.VarChar).Value = clienteProd.EPS;                 
                cmd.Parameters.Add("@PRODUTO", SqlDbType.VarChar).Value = clienteProd.PRODUTO;
                cmd.Parameters.Add("@COB2AV", SqlDbType.Bit).Value = clienteProd.COB2AV == 'N' ? 0 : 1;
                cmd.Parameters.Add("@VAL2AV", SqlDbType.Decimal).Value = clienteProd.VAL2AV;
                cmd.Parameters.Add("@COBINC", SqlDbType.Bit).Value = clienteProd.COBINC ? 1 : 0;
                cmd.Parameters.Add("@VALINCTIT", SqlDbType.Decimal).Value = clienteProd.VALINCTIT;
                cmd.Parameters.Add("@VALINCDEP", SqlDbType.Decimal).Value = clienteProd.VALINCDEP;
                cmd.Parameters.Add("@DIASVALSALDO", SqlDbType.Int).Value = clienteProd.DIASVALSALDO;
                cmd.Parameters.Add("@CARENCIATROCACANC", SqlDbType.Int).Value = clienteProd.CARENCIATROCACANC;
                cmd.Parameters.Add("@PRZVALCART", SqlDbType.VarChar).Value = clienteProd.PRZVALCART;
                cmd.Parameters.Add("@COBCONS", SqlDbType.VarChar).Value = clienteProd.COBCONS;
                cmd.Parameters.Add("@VALCONS", SqlDbType.Decimal).Value = clienteProd.VALCONS;
                cmd.Parameters.Add("@CARGPADVA", SqlDbType.Decimal).Value = clienteProd.CARGPADVA;
                cmd.Parameters.Add("@NEGARCARGASALDOACIMA", SqlDbType.Decimal).Value = clienteProd.NEGARCARGASALDOACIMA;
                cmd.Parameters.Add("@PRAPAG_VA", SqlDbType.Int).Value = clienteProd.PRAPAG_VA;
                cmd.Parameters.Add("@HABCPFTEMP", SqlDbType.VarChar).Value = clienteProd.HABCPFTEMP;
                cmd.Parameters.Add("@MAXCARTTEMP", SqlDbType.VarChar).Value = clienteProd.MAXCARTTEMP;
                cmd.Parameters.Add("@HABTROCACPFTEMP", SqlDbType.VarChar).Value = clienteProd.HABTROCACPFTEMP;
                cmd.Parameters.Add("@TIPOTAXSER", SqlDbType.VarChar).Value = clienteProd.TIPOTAXSER;
                cmd.Parameters.Add("@TAXSER_VA", SqlDbType.Decimal).Value = clienteProd.TAXSER_VA;
                cmd.Parameters.Add("@TAXADM_VA", SqlDbType.Decimal).Value = clienteProd.TAXADM_VA;
                cmd.Parameters.Add("@LIMMAXCAR", SqlDbType.Decimal).Value = clienteProd.LIMMAXCAR;
                cmd.Parameters.Add("@PGTOANTECIPADO", SqlDbType.VarChar).Value = clienteProd.PGTOANTECIPADO;
                cmd.Parameters.Add("@HABCARGASEQ", SqlDbType.VarChar).Value = clienteProd.HABCARGASEQ;
                cmd.Parameters.Add("@HABCARGACARTTEMP", SqlDbType.VarChar).Value = clienteProd.HABCARGACARTTEMP;
                if (clienteProd.DATCTT != DateTime.MinValue) cmd.Parameters.Add("@DATCTT_VA", SqlDbType.DateTime).Value = clienteProd.DATCTT;
                cmd.Parameters.Add("@SUBREDE", SqlDbType.VarChar).Value = clienteProd.SUBREDE;
                cmd.Parameters.Add("@GRUPOSOCIETARIO", SqlDbType.VarChar).Value = clienteProd.GRUPOSOCIETARIO;
                cmd.Parameters.Add("@PARCERIA", SqlDbType.VarChar).Value = clienteProd.PARCERIA;
                cmd.Parameters.Add("@STA", SqlDbType.VarChar).Value = clienteProd.STACOD;
                cmd.Parameters.Add("@CODLOGO1", SqlDbType.VarChar).Value = clienteProd.CODLOGO1;
                cmd.Parameters.Add("@CODLOGO2", SqlDbType.VarChar).Value = clienteProd.CODLOGO2;
                cmd.Parameters.Add("@CODMODELO1", SqlDbType.VarChar).Value = clienteProd.CODMODELO1;
                cmd.Parameters.Add("@CODMODELO2", SqlDbType.VarChar).Value = clienteProd.CODMODELO2;
                cmd.Parameters.Add("@CRTINCBLQ", SqlDbType.VarChar).Value = clienteProd.CRTINCBLQ;

                cmd.Parameters.Add("@TEL", SqlDbType.VarChar).Value = clienteProd.TEL;
                cmd.Parameters.Add("@FAX", SqlDbType.VarChar).Value = clienteProd.FAX;
                cmd.Parameters.Add("@EMA", SqlDbType.VarChar).Value = clienteProd.EMA;
                cmd.Parameters.Add("@CON", SqlDbType.VarChar).Value = clienteProd.CON;
                
                // Endereco Entrega Cartoes 
                cmd.Parameters.Add("@ENDEDC", SqlDbType.VarChar).Value = clienteProd.ENDEDC;
                cmd.Parameters.Add("@ENDCPLEDC", SqlDbType.VarChar).Value = clienteProd.ENDCPLEDC;
                cmd.Parameters.Add("@BAIRROEDC", SqlDbType.VarChar).Value = clienteProd.BAIRROEDC;
                cmd.Parameters.Add("@LOCALIDADEEDC", SqlDbType.VarChar).Value = clienteProd.LOCALIDADEEDC;
                cmd.Parameters.Add("@UFEDC", SqlDbType.VarChar).Value = clienteProd.UFEDC ?? string.Empty;
                cmd.Parameters.Add("@CEPEDC", SqlDbType.VarChar).Value = clienteProd.CEPEDC;
                cmd.Parameters.Add("@RESEDC", SqlDbType.VarChar).Value = clienteProd.RESEDC;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mensagem = Convert.ToString(reader["RETORNO"]);
                    if (mensagem == "0")
                    {
                        mensagem = "Registro incluído com sucesso.";
                        return true;
                    }
                    else
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro alteracao. Alguns valores invalidos foram inseridos na alteracao. Favor corrigir");
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }  
        }

        public bool Alterar(CADCLIENTE cadCliente, out string retorno)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            retorno = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ALTERA_CLIENTE", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.Int).Value = cadCliente.ID_CLIENTE;
                cmd.Parameters.Add("@NOMCLI", SqlDbType.VarChar).Value = cadCliente.NOMCLI;
                cmd.Parameters.Add("@CNPJ", SqlDbType.VarChar).Value = cadCliente.NOVOCNPJ ?? cadCliente.CNPJ;
                cmd.Parameters.Add("@INSEST", SqlDbType.VarChar).Value = cadCliente.INSEST;
                cmd.Parameters.Add("@REGIAO", SqlDbType.VarChar).Value = cadCliente.REGIAO;
                cmd.Parameters.Add("@RAMOATI", SqlDbType.VarChar).Value = cadCliente.RAMOATI;
                cmd.Parameters.Add("@UNIDADE", SqlDbType.VarChar).Value = cadCliente.UNIDADE;
                cmd.Parameters.Add("@SETOR", SqlDbType.VarChar).Value = cadCliente.SETOR;
                cmd.Parameters.Add("@PORTE", SqlDbType.VarChar).Value = cadCliente.PORTE;

                // Endereco Comercial
                cmd.Parameters.Add("@ENDCLI", SqlDbType.VarChar).Value = cadCliente.ENDCLI;
                cmd.Parameters.Add("@ENDCPL", SqlDbType.VarChar).Value = cadCliente.ENDCPL;
                cmd.Parameters.Add("@BAIRRO", SqlDbType.VarChar).Value = cadCliente.BAIRRO;
                cmd.Parameters.Add("@LOCALIDADE", SqlDbType.VarChar).Value = cadCliente.LOCALIDADE;
                cmd.Parameters.Add("@UF", SqlDbType.VarChar).Value = cadCliente.UF ?? string.Empty;
                cmd.Parameters.Add("@CEP", SqlDbType.VarChar).Value = cadCliente.CEP;

                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retorno = Convert.ToString(reader["RETORNO"]);
                    if (retorno == "0")
                    {
                        retorno = "Registro alterado com sucesso.";
                        return true;
                    }
                    else
                    {
                        retorno = Convert.ToString(reader["MENSAGEM"]);
                    }

                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool AtualizaClientePreJuncao(int codCli)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ATUAILIZA_CLIENTE_PRE_JUNCAO ", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();                
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var mensagem = Convert.ToString(reader["RETORNO"]);
                    if (mensagem == "0")
                    {                        
                        return true;
                    }
                    else
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool AlterarInfControlePos(VPRODUTOSCLI clienteProd, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ALTERA_CLIENTE_POS", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = clienteProd.CODCLI;
                cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.Int).Value = clienteProd.ID_CLIENTE;
                cmd.Parameters.Add("@NOMGRA", SqlDbType.VarChar).Value = clienteProd.NOMGRA;
                cmd.Parameters.Add("@PRODUTO", SqlDbType.VarChar).Value = clienteProd.PRODUTO;
                cmd.Parameters.Add("@REGIONAL", SqlDbType.VarChar).Value = clienteProd.REGIONAL;
                cmd.Parameters.Add("@EPS", SqlDbType.VarChar).Value = clienteProd.EPS;
                cmd.Parameters.Add("@HABCPFTEMP", SqlDbType.VarChar).Value = clienteProd.HABCPFTEMP;
                cmd.Parameters.Add("@HABTROCACPFTEMP", SqlDbType.VarChar).Value = clienteProd.HABTROCACPFTEMP;
                cmd.Parameters.Add("@MAXCARTTEMP", SqlDbType.VarChar).Value = clienteProd.MAXCARTTEMP;
                cmd.Parameters.Add("@MAXPARC", SqlDbType.Int).Value = clienteProd.MAXPARC;
                cmd.Parameters.Add("@OUTCRT", SqlDbType.VarChar).Value = clienteProd.OUTCRT;
                cmd.Parameters.Add("@EXIREC", SqlDbType.VarChar).Value = clienteProd.EXIREC;
                cmd.Parameters.Add("@EXIBEMENS", SqlDbType.VarChar).Value = clienteProd.EXIBMES;
                cmd.Parameters.Add("@CRTINCBLQ", SqlDbType.VarChar).Value = clienteProd.CRTINCBLQ;
                cmd.Parameters.Add("@SUBTIT", SqlDbType.VarChar).Value = clienteProd.SUBTIT;
                cmd.Parameters.Add("@LIMRISCO", SqlDbType.Int).Value = clienteProd.LIMRISCO;
                cmd.Parameters.Add("@CARENCIATROCACANC", SqlDbType.Int).Value = clienteProd.CARENCIATROCACANC;
                cmd.Parameters.Add("@PRZVALCART", SqlDbType.VarChar).Value = clienteProd.PRZVALCART;
                cmd.Parameters.Add("@CONPMO", SqlDbType.VarChar).Value = clienteProd.CONPMO;
                cmd.Parameters.Add("@PROXCONPMO", SqlDbType.VarChar).Value = clienteProd.PROXCONPMO;
                cmd.Parameters.Add("@NRENOVPMO", SqlDbType.VarChar).Value = clienteProd.NRENOVPMO;
                cmd.Parameters.Add("@COB2AV", SqlDbType.VarChar).Value = clienteProd.COB2AV;
                cmd.Parameters.Add("@VAL2AV", SqlDbType.Decimal).Value = clienteProd.VAL2AV;
                cmd.Parameters.Add("@COBATV", SqlDbType.VarChar).Value = clienteProd.COBATV;
                cmd.Parameters.Add("@VALATV", SqlDbType.Decimal).Value = clienteProd.VALATV;
                cmd.Parameters.Add("@COBCONS", SqlDbType.VarChar).Value = clienteProd.COBCONS;
                cmd.Parameters.Add("@VALCONS", SqlDbType.Decimal).Value = clienteProd.VALCONS;
                cmd.Parameters.Add("@TIPDES", SqlDbType.Char).Value = clienteProd.TIPDES;
                cmd.Parameters.Add("@TIPFEC", SqlDbType.Int).Value = clienteProd.TIPFEC;
                cmd.Parameters.Add("@TIPPAG", SqlDbType.VarChar).Value = clienteProd.TIPPAG;
                cmd.Parameters.Add("@DATFEC", SqlDbType.Int).Value = clienteProd.DATFEC;
                cmd.Parameters.Add("@PRAPAG", SqlDbType.Int).Value = clienteProd.PRAPAG;
                cmd.Parameters.Add("@ORDEMCL", SqlDbType.VarChar).Value = clienteProd.ORDEMCL;

                if (clienteProd.DATCTT != DateTime.MinValue) cmd.Parameters.Add("@DATCTT", SqlDbType.DateTime).Value = clienteProd.DATCTT;
                cmd.Parameters.Add("@SUBREDE", SqlDbType.VarChar).Value = clienteProd.SUBREDE;
                cmd.Parameters.Add("@GRUPOSOCIETARIO", SqlDbType.VarChar).Value = clienteProd.GRUPOSOCIETARIO;
                cmd.Parameters.Add("@PARCERIA", SqlDbType.VarChar).Value = clienteProd.PARCERIA;
                cmd.Parameters.Add("@VALTOTCRE", SqlDbType.Decimal).Value = clienteProd.VALTOTCRE;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;
                cmd.Parameters.Add("@STA", SqlDbType.VarChar).Value = clienteProd.STACOD;
                cmd.Parameters.Add("@DATSTA", SqlDbType.DateTime).Value = clienteProd.DATSTA;

                cmd.Parameters.Add("@CODLOGO1", SqlDbType.VarChar).Value = clienteProd.CODLOGO1;
                cmd.Parameters.Add("@CODLOGO2", SqlDbType.VarChar).Value = clienteProd.CODLOGO2;
                cmd.Parameters.Add("@CODMODELO1", SqlDbType.VarChar).Value = clienteProd.CODMODELO1;
                cmd.Parameters.Add("@CODMODELO2", SqlDbType.VarChar).Value = clienteProd.CODMODELO2;

                cmd.Parameters.Add("@TEL", SqlDbType.VarChar).Value = clienteProd.TEL;
                cmd.Parameters.Add("@FAX", SqlDbType.VarChar).Value = clienteProd.FAX;
                cmd.Parameters.Add("@EMA", SqlDbType.VarChar).Value = clienteProd.EMA;
                cmd.Parameters.Add("@CON", SqlDbType.VarChar).Value = clienteProd.CON;

                // Endereco Entrega Cartoes 
                cmd.Parameters.Add("@ENDEDC", SqlDbType.VarChar).Value = clienteProd.ENDEDC;
                cmd.Parameters.Add("@ENDCPLEDC", SqlDbType.VarChar).Value = clienteProd.ENDCPLEDC;
                cmd.Parameters.Add("@BAIRROEDC", SqlDbType.VarChar).Value = clienteProd.BAIRROEDC;
                cmd.Parameters.Add("@LOCALIDADEEDC", SqlDbType.VarChar).Value = clienteProd.LOCALIDADEEDC;
                cmd.Parameters.Add("@UFEDC", SqlDbType.VarChar).Value = clienteProd.UFEDC ?? string.Empty;
                cmd.Parameters.Add("@CEPEDC", SqlDbType.VarChar).Value = clienteProd.CEPEDC;
                cmd.Parameters.Add("@RESEDC", SqlDbType.VarChar).Value = clienteProd.RESEDC;
                
                // Executando o commando e obtendo o resultado
                reader = cmd.ExecuteReader();

                // Exibindo os registros
                while (reader.Read())
                {
                    mensagem = Convert.ToString(reader["RETORNO"]);
                    if (mensagem == "0")
                    {
                        //// OPERADORMW
                        //if (cadCliente.NOVOCNPJ != cadCliente.CNPJ)
                        //    AlterarCnpjOperador(cadCliente);
                        mensagem = "Registro alterado com sucesso.";
                        return true;
                    }
                    else 
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }                
                if (conn != null) { conn.Close(); }
            }
        }

        public bool AlterarInfControlePre(VPRODUTOSCLI clienteProd, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ALTERA_CLIENTE_PRE", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.Int).Value = clienteProd.ID_CLIENTE;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = clienteProd.CODCLI;
                cmd.Parameters.Add("@NOMGRA", SqlDbType.VarChar).Value = clienteProd.NOMGRA;
                cmd.Parameters.Add("@REGIONAL", SqlDbType.VarChar).Value = clienteProd.REGIONAL;
                cmd.Parameters.Add("@EPS", SqlDbType.VarChar).Value = clienteProd.EPS;               
                cmd.Parameters.Add("@PRODUTO", SqlDbType.VarChar).Value = clienteProd.PRODUTO;
                cmd.Parameters.Add("@HABCPFTEMP", SqlDbType.VarChar).Value = clienteProd.HABCPFTEMP;
                cmd.Parameters.Add("@HABTROCACPFTEMP", SqlDbType.VarChar).Value = clienteProd.HABTROCACPFTEMP;
                cmd.Parameters.Add("@MAXCARTTEMP", SqlDbType.VarChar).Value = clienteProd.MAXCARTTEMP;
                cmd.Parameters.Add("@COB2AV", SqlDbType.Bit).Value = clienteProd.COB2AV == 'N' ? 0 : 1;
                cmd.Parameters.Add("@VAL2AV", SqlDbType.Decimal).Value = clienteProd.VAL2AV;
                cmd.Parameters.Add("@COBINC", SqlDbType.Bit).Value = clienteProd.COBINC ? 1 : 0;
                cmd.Parameters.Add("@VALINCTIT", SqlDbType.Decimal).Value = clienteProd.VALINCTIT;
                cmd.Parameters.Add("@VALINCDEP", SqlDbType.Decimal).Value = clienteProd.VALINCDEP;
                cmd.Parameters.Add("@DIASVALSALDO", SqlDbType.Int).Value = clienteProd.DIASVALSALDO;
                cmd.Parameters.Add("@CARENCIATROCACANC", SqlDbType.Int).Value = clienteProd.CARENCIATROCACANC;
                cmd.Parameters.Add("@PRZVALCART", SqlDbType.VarChar).Value = clienteProd.PRZVALCART;
                cmd.Parameters.Add("@COBCONS", SqlDbType.VarChar).Value = clienteProd.COBCONS;
                cmd.Parameters.Add("@VALCONS", SqlDbType.Decimal).Value = clienteProd.VALCONS;
                cmd.Parameters.Add("@CARGPADVA", SqlDbType.Decimal).Value = clienteProd.CARGPADVA;
                cmd.Parameters.Add("@NEGARCARGASALDOACIMA", SqlDbType.Decimal).Value = clienteProd.NEGARCARGASALDOACIMA;
                cmd.Parameters.Add("@PRAPAG_VA", SqlDbType.Int).Value = clienteProd.PRAPAG_VA;
                cmd.Parameters.Add("@TIPOTAXSER", SqlDbType.VarChar).Value = clienteProd.TIPOTAXSER;
                cmd.Parameters.Add("@TAXSER_VA", SqlDbType.Decimal).Value = clienteProd.TAXSER_VA;
                cmd.Parameters.Add("@TAXADM_VA", SqlDbType.Decimal).Value = clienteProd.TAXADM_VA;
                cmd.Parameters.Add("@LIMMAXCAR", SqlDbType.Decimal).Value = clienteProd.LIMMAXCAR;
                cmd.Parameters.Add("@PGTOANTECIPADO", SqlDbType.VarChar).Value = clienteProd.PGTOANTECIPADO;
                cmd.Parameters.Add("@HABCARGASEQ", SqlDbType.VarChar).Value = clienteProd.HABCARGASEQ;
                cmd.Parameters.Add("@HABCARGACARTTEMP", SqlDbType.VarChar).Value = clienteProd.HABCARGACARTTEMP;
                if (clienteProd.DATCTT != DateTime.MinValue) cmd.Parameters.Add("@DATCTT_VA", SqlDbType.DateTime).Value = clienteProd.DATCTT;
                cmd.Parameters.Add("@SUBREDE", SqlDbType.VarChar).Value = clienteProd.SUBREDE;
                cmd.Parameters.Add("@GRUPOSOCIETARIO", SqlDbType.VarChar).Value = clienteProd.GRUPOSOCIETARIO;
                cmd.Parameters.Add("@PARCERIA", SqlDbType.VarChar).Value = clienteProd.PARCERIA;
                cmd.Parameters.Add("@DATRESCISAO", SqlDbType.DateTime).Value = clienteProd.DATRESCISAO == DateTime.MinValue ? null : (object)clienteProd.DATRESCISAO;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;
                cmd.Parameters.Add("@STA", SqlDbType.VarChar).Value = clienteProd.STACOD;
                cmd.Parameters.Add("@DATSTA", SqlDbType.DateTime).Value = clienteProd.DATSTA;

                cmd.Parameters.Add("@CODLOGO1", SqlDbType.VarChar).Value = clienteProd.CODLOGO1;
                cmd.Parameters.Add("@CODLOGO2", SqlDbType.VarChar).Value = clienteProd.CODLOGO2;
                cmd.Parameters.Add("@CODMODELO1", SqlDbType.VarChar).Value = clienteProd.CODMODELO1;
                cmd.Parameters.Add("@CODMODELO2", SqlDbType.VarChar).Value = clienteProd.CODMODELO2;
                cmd.Parameters.Add("@CRTINCBLQ", SqlDbType.VarChar).Value = clienteProd.CRTINCBLQ;

                cmd.Parameters.Add("@TEL", SqlDbType.VarChar).Value = clienteProd.TEL;
                cmd.Parameters.Add("@FAX", SqlDbType.VarChar).Value = clienteProd.FAX;
                cmd.Parameters.Add("@EMA", SqlDbType.VarChar).Value = clienteProd.EMA;
                cmd.Parameters.Add("@CON", SqlDbType.VarChar).Value = clienteProd.CON;

                // Endereco Entrega Cartoes 
                cmd.Parameters.Add("@ENDEDC", SqlDbType.VarChar).Value = clienteProd.ENDEDC;
                cmd.Parameters.Add("@ENDCPLEDC", SqlDbType.VarChar).Value = clienteProd.ENDCPLEDC;
                cmd.Parameters.Add("@BAIRROEDC", SqlDbType.VarChar).Value = clienteProd.BAIRROEDC;
                cmd.Parameters.Add("@LOCALIDADEEDC", SqlDbType.VarChar).Value = clienteProd.LOCALIDADEEDC;
                cmd.Parameters.Add("@UFEDC", SqlDbType.VarChar).Value = clienteProd.UFEDC ?? string.Empty;
                cmd.Parameters.Add("@CEPEDC", SqlDbType.VarChar).Value = clienteProd.CEPEDC;
                cmd.Parameters.Add("@RESEDC", SqlDbType.VarChar).Value = clienteProd.RESEDC;

                // Executando o commando e obtendo o resultado
                reader = cmd.ExecuteReader();

                // Exibindo os registros
                while (reader.Read())
                {
                    mensagem = Convert.ToString(reader["RETORNO"]);
                    if (mensagem == "0")
                    {
                        //// OPERADORMW
                        //if (cadCliente.NOVOCNPJ != cadCliente.CNPJ)
                        //    AlterarCnpjOperador(cadCliente);
                        mensagem = "Registro alterado com sucesso.";
                        return true;
                    }
                    else 
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }                
                if (conn != null) { conn.Close(); }
            }
        }

        public bool AlterarStatus(int codCli, int sistema, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ALTERA_STATUS_CLIENTE", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = sistema;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;
                cmd.Parameters.Add("@STA", SqlDbType.VarChar).Value = ConstantesSIL.StatusBloqueado;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.VarChar).Value = FOperador.ID_FUNC;

                // Executando o commando e obtendo o resultado
                reader = cmd.ExecuteReader();

                // Exibindo os registros
                if (reader.Read())
                {
                    mensagem = Convert.ToString(reader["RETORNO"]);
                    if (mensagem == "0")
                    {                        
                        mensagem = "Cliente bloqueado com sucesso.";
                        return true;
                    }
                    else
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool ExcluirCadCliente(CADCLIENTE cadCliente, out string retorno)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            retorno = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_EXCLUI_CLIENTE", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                //cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.Int).Value = cadCliente.ID_CLIENTE;
                cmd.Parameters.Add("@CNPJ", SqlDbType.VarChar).Value = cadCliente.NOVOCNPJ ?? cadCliente.CNPJ;
                cmd.Parameters.Add("@IDFUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retorno = Convert.ToString(reader["RETORNO"]);
                    if (retorno == "0")
                    {
                        retorno = "Cadastro de cliente excluido com sucesso.";
                        return true;
                    }
                    else
                    {
                        retorno = Convert.ToString(reader["MENSAGEM"]);
                    }

                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool ExcluirClientePos(int codCli, out string retorno)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            retorno = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_EXCLUI_CLIENTE_POS", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();                
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;
                cmd.Parameters.Add("@IDFUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retorno = Convert.ToString(reader["RETORNO"]);
                    if (retorno == "0")
                    {
                        retorno = "Cliente excluido com sucesso.";
                        return true;
                    }
                    else
                    {
                        retorno = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool ExcluirClientePre(int codCli, out string retorno)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            retorno = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_EXCLUI_CLIENTE_PRE", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();                
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;
                cmd.Parameters.Add("@IDFUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retorno = Convert.ToString(reader["RETORNO"]);
                    if (retorno == "0")
                    {
                        retorno = "Cliente excluido com sucesso.";
                        return true;
                    }
                    else
                    {
                        retorno = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        #endregion

        #region CRUD Observacoes

        public bool InserirObs(int sistema, int codCli, DateTime data, string obs)
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
            
            string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", sistema == 0 ? "OBSCLI" : "OBSCLIVA", sbCamposCliente.ToString(), sbParametrosCliente.ToString());
            DbCommand cmd = db.GetSqlStringCommand(sql);
            DbConnection dbc = db.CreateConnection();
                        
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);            
            db.AddInParameter(cmd, "DATA", DbType.DateTime, data.ToString("G"));
            db.AddInParameter(cmd, "OBS", DbType.String, obs);

            // Controle Transacao
            dbc.Open();
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "INSERT OBSCLI", FOperador, cmd);

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

        public bool ExcluirObs(int sistema, int codCli, DateTime data)
        {
            Database db;
            string sql;
            DbCommand cmd;
            DbConnection dbc;
            DbTransaction dbt;

            db = new SqlDatabase(BDTELENET);

            sql = string.Format("DELETE {0} WHERE CODCLI = @CODCLI AND DATA = @DATA", sistema == 0 ? "OBSCLI" : "OBSCLIVA");
            cmd = db.GetSqlStringCommand(sql);
            dbc = db.CreateConnection();

            // Cliente            
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            db.AddInParameter(cmd, "DATA", DbType.DateTime, data.ToString("G"));

            // Controle Transacao
            dbc.Open();
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "DELETE OBSCLI", FOperador, cmd);

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
            sql = "DELETE FROM OBSCLI WHERE CODCLI = @CODCLI";
            cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, ClienteVA);
            db.ExecuteNonQuery(cmd, dbt);

            //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
            UtilSIL.GravarLog(db, dbt, "DELETE OBSCLI (Exclusao da lista observacoes do cliente excluido)" , FOperador, cmd);
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

        #region CRUD ObservacoesVa

        public bool InserirObsVa(int CODCLI, DateTime DATA, string OBS)
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

        public bool ExcluirObsVa(int CODCLI, DateTime DATA)
        {
            Database db;
            string sql;
            DbCommand cmd;
            DbConnection dbc;
            DbTransaction dbt;

            db = new SqlDatabase(BDTELENET);

            sql = string.Format("DELETE OBSCLIVA WHERE CODCLI = @CODCLI AND DATA = @DATA");
            cmd = db.GetSqlStringCommand(sql);
            dbc = db.CreateConnection();

            // Cliente            
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, CODCLI);
            db.AddInParameter(cmd, "DATA", DbType.DateTime, DATA.ToString("G"));

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

        public bool ExcluirObsVa(Database db, DbTransaction dbt, int ClienteVA)
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

            sql = "SELECT CODCLIENTE FROM CTCLIVA WITH (NOLOCK) WHERE CODCLIENTE = @CODCLIENTE";
              
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
            db.AddInParameter(cmd, "NOMECLI", DbType.String,ClienteVA.NOMCLI.Length > 30 ? ClienteVA.NOMCLI.Substring(0, 30) : ClienteVA.NOMCLI);
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
                  "DIASVALSALDO = @DIASVALSALDO, COBCONS = @COBCONS, VALCONS = @VALCONS " +
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

        private void InserirAutorizadorSegAutorizado(int ClienteVA, int Segmento)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();

            try
            {
                const string sql = "INSERT INTO TABSEGVA (CODCLIENTE, CODSEG) " +
                                   "SELECT @CODCLIENTE, @CODSEG " +
                                   " WHERE NOT EXISTS " +
                                   "(SELECT CODSEG FROM TABSEGVA WITH (NOLOCK) WHERE CODCLIENTE = @CODCLIENTE AND CODSEG = @CODSEG)";
 
                var cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.ToString().PadLeft(5, '0'));
                db.AddInParameter(cmd, "CODSEG", DbType.String, Segmento.ToString().PadLeft(5, '0'));

                db.ExecuteNonQuery(cmd, dbt);

                
                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Autorizador Seg. Autorizado]" + err);
            }
            finally
            {
                dbc.Close();

            }
        }

        private void InserirAutorizadorSegAutorizado(int ClienteVA)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);

            // Colecao Seg. Autorizados
            var SegAutorizados = SegmentosAutorizados(ClienteVA);

            foreach (var SegCliente in SegAutorizados)
            {
                #region Inserir Segmento
                var dbc = db.CreateConnection();
                dbc.Open();
                // Controle Transacao
                var dbt = dbc.BeginTransaction();

                try
                {
                    const string sql = "INSERT INTO TABSEGVA (CODCLIENTE, CODSEG) " +
                                       "SELECT @CODCLIENTE, @CODSEG " +
                                       " WHERE NOT EXISTS " +
                                       "(SELECT CODSEG FROM TABSEGVA WITH (NOLOCK) WHERE CODCLIENTE = @CODCLIENTE AND CODSEG = @CODSEG)";

                    var cmd = db.GetSqlStringCommand(sql);

                    db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.ToString().PadLeft(5, '0'));
                    db.AddInParameter(cmd, "CODSEG", DbType.String, SegCliente.CODSEG.ToString().PadLeft(5, '0'));

                    db.ExecuteNonQuery(cmd, dbt);


                    dbt.Commit();

                }
                catch (Exception err)
                {
                    dbt.Rollback();
                    throw new Exception("Erro Camada DAL [Inserir Autorizador Seg. Autorizado]" + err);
                }
                finally
                {
                    dbc.Close();

                }

                #endregion
            }
        }

        private void ExcluirAutorizadorSegAutorizado(int ClienteVA, int Segmento)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();

            try
            {
                const string sql = "DELETE FROM TABSEGVA WHERE CODCLIENTE = @CODCLIENTE AND CODSEG = @CODSEG";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.ToString().PadLeft(5, '0'));
                db.AddInParameter(cmd, "CODSEG", DbType.String, Segmento.ToString().PadLeft(5, '0'));
                               
                db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Autorizador :: Seg Autorizado]" + err);
            }
            finally
            {
                dbc.Close();
            }
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

        private void InserirAutorizadorGrupoAutorizado(int ClienteVA, int Grupo)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();

            try
            {
                const string sql = "INSERT INTO TABSEGVA (CODCLIENTE, CODGRUPO) " +
                                   "SELECT @CODCLIENTE, @CODGRUPO " +
                                   " WHERE NOT EXISTS " +
                                   "(SELECT CODGRUPO FROM TABSEGVA WITH (NOLOCK) WHERE CODCLIENTE = @CODCLIENTE AND CODGRUPO = @CODGRUPO)";

                var cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.ToString().PadLeft(5, '0'));
                db.AddInParameter(cmd, "CODGRUPO", DbType.String, Grupo.ToString().PadLeft(5, '0'));

                db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Autorizador Grupo Autorizado]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        private void InserirAutorizadorGrupoAutorizado(int ClienteVA)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            // Colecao Grupos Autorizados
            var GruposAutoriz = GruposAutorizados(ClienteVA);

            foreach (var GruposCliente in GruposAutoriz)
            {
                #region Inserir Grupo
                var dbc = db.CreateConnection();
                dbc.Open();
                // Controle Transacao
                var dbt = dbc.BeginTransaction();

                try
                {
                    const string sql = "INSERT INTO TABSEGVA (CODCLIENTE, CODGRUPO) " +
                                       "SELECT @CODCLIENTE, @CODGRUPO " +
                                       " WHERE NOT EXISTS " +
                                       "(SELECT CODGRUPO FROM TABSEGVA WITH (NOLOCK) WHERE CODCLIENTE = @CODCLIENTE AND CODGRUPO = @CODGRUPO)";

                    var cmd = db.GetSqlStringCommand(sql);

                    db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.ToString().PadLeft(5, '0'));
                    db.AddInParameter(cmd, "CODGRUPO", DbType.String, GruposCliente.CODGRUPO.ToString().PadLeft(5, '0'));

                    db.ExecuteNonQuery(cmd, dbt);


                    dbt.Commit();

                }
                catch (Exception err)
                {
                    dbt.Rollback();
                    throw new Exception("Erro Camada DAL [Inserir Autorizador Grupo Autorizado]" + err);
                }
                finally
                {
                    dbc.Close();

                }

                #endregion
            }
        }

        private void ExcluirAutorizadorGrupoAutorizado(int ClienteVA, int Grupo)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "DELETE FROM TABSEGVA WHERE CODCLIENTE = @CODCLIENTE AND CODGRUPO = @CODGRUPO";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCLIENTE", DbType.String, ClienteVA.ToString().PadLeft(5, '0'));
                db.AddInParameter(cmd, "CODGRUPO", DbType.String, Grupo.ToString().PadLeft(5, '0'));
                db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Autorizador :: Grupo Autorizado]" + err);
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
            const string sql = "PROC_ASSOCIA_TAXACLI_PJ";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "@SISTEMA", DbType.Int32, taxacli.SISTEMA);
            db.AddInParameter(cmd, "@CODCLI", DbType.String, taxacli.COD);
            db.AddInParameter(cmd, "@CODTAXA", DbType.String, taxacli.CODTAXA);
            db.AddInParameter(cmd, "@VALOR", DbType.Decimal, taxacli.VALOR);
            db.AddInParameter(cmd, "@VALORDEP", DbType.Decimal, taxacli.VALORDEP);
            db.AddInParameter(cmd, "@COBFATABAIXO", DbType.String, taxacli.COBFATABAIXO);
            db.AddInParameter(cmd, "@VALCOBFATABAIXO", DbType.Decimal, taxacli.VALCOBFATABAIXO);
            db.AddInParameter(cmd, "@NUMPARC", DbType.Int16, taxacli.NUMPARC);
            db.AddInParameter(cmd, "@DTINICIO", DbType.DateTime, taxacli.DTINICIO);
            db.AddInParameter(cmd, "@MESESPINICIO", DbType.Int16, taxacli.DIASPINICIO);
            db.AddInParameter(cmd, "@PGTOTAXA", DbType.Int16, taxacli.PGTOTAXA);
            db.AddInParameter(cmd, "@COBRAATV", DbType.String, taxacli.COBRAATV);
            db.AddInParameter(cmd, "@INDIVIDUAL", DbType.String, taxacli.INDIVIDUAL);
            db.AddInParameter(cmd, "@SENSISALDO", DbType.String, taxacli.SENSISALDO);
            db.AddInParameter(cmd, "@COBCANC", DbType.String, taxacli.COBCANC);
            db.AddInParameter(cmd, "@COBCANCUTIL", DbType.String, taxacli.COBCANCUTIL);
            db.AddInParameter(cmd, "@COBUTIL", DbType.String, taxacli.COBUTIL);
            db.AddInParameter(cmd, "@COBUTILGRUPO", DbType.String, taxacli.COBUTILGRUPO);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "PROC_ASSOCIA_TAXACLI_PJ", FOperador, cmd);
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
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, taxacli.SISTEMA);
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
            db.AddInParameter(cmd, "CNPJ", DbType.String, Acesso.CNPJ);
            db.AddInParameter(cmd, "LOGIN", DbType.String, Acesso.LOGIN);
            db.AddInParameter(cmd, "NOME", DbType.String, Acesso.NOME);
            db.AddInParameter(cmd, "CODPARCERIA", DbType.Int32, Acesso.CODPARCERIA);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, Acesso.CODCLI);
            db.AddInParameter(cmd, "SISTEMA", DbType.Int16, Acesso.SISTEMA);
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
            db.AddInParameter(cmd, "FALTLIMITE", DbType.String, ConstantesSIL.BoolToChar(Acesso.FALTLIMITE));
            db.AddInParameter(cmd, "FLISTTRANSAB", DbType.String, ConstantesSIL.BoolToChar(Acesso.FLISTTRANSAB));
            db.AddInParameter(cmd, "FTRANSFSALDOCLI", DbType.String, ConstantesSIL.BoolToChar(Acesso.FTRANSFSALDOCLI));
            db.AddInParameter(cmd, "FCARGAACIMALIMITE", DbType.String, ConstantesSIL.BoolToChar(Acesso.FCARGAACIMALIMITE));

            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                idr = db.ExecuteReader(cmd, dbt);
                if (idr.Read())
                {
                    retorno = Convert.ToString(idr["RETORNO"]);
                    idr.Close();
                    if (retorno == "OK")
                    {                        
                        UtilSIL.GravarLog(db, dbt, "PROC_INSERE_OPERADORWEB", FOperador, cmd);
                        dbt.Commit();
                    }
                    if (retorno == "OK")
                    {
                        retorno = "Operador cadastrado com sucesso.";
                        return true;                        
                    }
                }
                retorno = "Erro ao cadastrar o operador.";
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

        public bool ExcluirOperadorWEB(ACESSOOPERADORMW Acesso)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "DELETE ACESSOOPERADORMW WHERE IDOPEMW = @IDOPEMW AND SISTEMA = @SISTEMA";
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            // Operador 
            db.AddInParameter(cmd, "IDOPEMW", DbType.Int32, Acesso.ID);
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, Acesso.SISTEMA);
            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {   // Linha Afetada                                
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
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

        public bool RenovarAcessoOperadorWEB(OPERADORMW Operador)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = @"UPDATE OPERADORMW SET DTEXPSENHA = @DTEXPSENHA, DTSENHA = @DTSENHA, QTDEACESSOINV = 0, SENHA = @SENHA WHERE ID = @ID";

            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            var dataRenavacao = DateTime.Now.AddDays(DiasParaRenovarSenha());

            db.AddInParameter(cmd, "DTEXPSENHA", DbType.String, dataRenavacao.ToString("yyyyMMdd"));
            db.AddInParameter(cmd, "DTSENHA", DbType.DateTime, null);
            db.AddInParameter(cmd, "SENHA", DbType.String, Operador.SENHA);
            db.AddInParameter(cmd, "ID", DbType.String, Operador.ID);

            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICAcaO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "UPDATE OPERCLIVAWEB", FOperador, cmd);
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

        #region Enviar Carta Boas Vindas

        public string EnviarCarta(int sistema, int codCli)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var msgErro = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("ENVIA_EMAIL_BOAS_VINDAS_POR_CLIENTE", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = sistema;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;
                cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = FOperador.CODOPE;

                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@ERRO", DbType = DbType.Int32, Direction = ParameterDirection.Output });
                //cmd.Parameters.Add(new SqlParameter() { ParameterName = "@MSG_ERRO", DbType = DbType.String, Direction = ParameterDirection.Output });

                cmd.ExecuteReader();
                conn.Close();

                var erro = Convert.ToInt32(cmd.Parameters["@ERRO"].Value);
                //msgErro = cmd.Parameters["@MSG_ERRO"].Value.ToString();

                return erro == 0 ? "Email de boas vindas enviado com sucesso." : "Ocorreu um erro ao enviar o email, favor entrar em contato com o suporte.";
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        #endregion

        #region Get Listas      

        public List<VRESUMOCLI> ColecaoClientes(string Filtro)
        {
            var conn = new SqlConnection();
            conn.ConnectionString = BDTELENET;
            var sql = new StringBuilder();
            sql.AppendLine(" SELECT");
            sql.AppendLine(" ID_CLIENTE, NOMCLI, SISTEMA, PRODUTO, CODCLI, CNPJ, STA, DESTA, CODAG, MAXPARC, LIMRISCO, NOMLOC, SUBREDE, NUMCARG_VA ");
            sql.AppendLine(" FROM VRESUMOCLI WITH (NOLOCK) ");
            
            if (!string.IsNullOrEmpty(Filtro))
                sql.AppendLine(string.Format("WHERE {0} ", Filtro));
            sql.AppendLine(" ORDER BY CNPJ, CODCLI");
            var cmd = new SqlCommand(sql.ToString(), conn);
            conn.Open();
            var idr = cmd.ExecuteReader();
            var colecaoClientesVA = new List<VRESUMOCLI>();


            while (idr.Read())
            {
                var cadCliente = new VRESUMOCLI();
                cadCliente.ID_CLIENTE = Convert.ToInt32(idr["ID_CLIENTE"]);
                if (idr["CODCLI"] != DBNull.Value) cadCliente.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                if (idr["NOMCLI"] != DBNull.Value) cadCliente.NOMCLI = idr["NOMCLI"].ToString();
                cadCliente.CNPJ = idr["CNPJ"].ToString();
                if (idr["STA"] != DBNull.Value) cadCliente.STA = idr["STA"].ToString();
                if (idr["DESTA"] != DBNull.Value) cadCliente.DESTA = idr["DESTA"].ToString();
                if (idr["SISTEMA"] == DBNull.Value) cadCliente.SISTEMA = 2;
                else cadCliente.SISTEMA = Convert.ToInt32(idr["SISTEMA"]);
                if (idr["PRODUTO"] != DBNull.Value) cadCliente.PRODUTO = idr["PRODUTO"].ToString();
                if (idr["CODAG"] != DBNull.Value) cadCliente.CODAG = Convert.ToInt32(idr["CODAG"]);
                if (idr["MAXPARC"] != DBNull.Value) cadCliente.MAXPARC= Convert.ToInt32(idr["MAXPARC"]);
                if (idr["LIMRISCO"] != DBNull.Value) cadCliente.LIMRISCO = Convert.ToInt32(idr["LIMRISCO"]);
                if (idr["NOMLOC"] != DBNull.Value) cadCliente.NOMLOC = idr["NOMLOC"].ToString();
                if (idr["SUBREDE"] != DBNull.Value) cadCliente.SUBREDE = idr["SUBREDE"].ToString();
                if (idr["NUMCARG_VA"] != DBNull.Value) cadCliente.NUMCARG_VA = Convert.ToInt32(idr["NUMCARG_VA"]);
                colecaoClientesVA.Add(cadCliente);
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
            sql.AppendLine("  FROM CLIENTEVA WITH (NOLOCK) ");
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
            sql.AppendLine("  FROM VENDEDOR WITH (NOLOCK) ");
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

        public List<MODTAXA> ConsultaTaxaCli(int codCli, int Sistema)
        {
            var taxaCli = new List<MODTAXA>();
            const string sql = "PROC_LER_TAXACLIPJ";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODCLI", DbType.String, codCli);
            db.AddInParameter(cmd, "@SISTEMA", DbType.String, Sistema);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var taxa = new MODTAXA();
                taxa.COD = Convert.ToInt32(idr["CODCLI"]);
                taxa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxa.NOMTAXA = Convert.ToString(idr["NOMTAXA"]);
                taxa.TIPO = Convert.ToInt32(idr["TIPO"]);
                taxa.VALOR = Convert.ToDecimal(idr["VALTIT"]);
                taxa.VALORDEP = idr["VALDEP"] == DBNull.Value ? 0 : Convert.ToDecimal(idr["VALDEP"]);
                taxa.COBFATABAIXO = Convert.ToString(idr["COBFATABAIXO"]);
                taxa.VALCOBFATABAIXO = idr["VALCOBFATABAIXO"] == DBNull.Value ? 0 : Convert.ToDecimal(idr["VALCOBFATABAIXO"]);
                taxa.DTINICIO = Convert.ToDateTime(idr["DTINICIO"]);
                taxa.DIASPINICIO = Convert.ToInt32(idr["MESESPINICIO"]);
                taxa.TIPTRA = Convert.ToInt32(idr["TIPTRA"]);
                taxa.NUMPARC = Convert.ToInt32(idr["NUMPAC"]);
                taxa.PGTOTAXA = idr["PGTOTAXA"].ToString() == "N" ? (short)0 : (short)1;
                taxa.COBRAATV = Convert.ToString(idr["COBRAATV"]);
                taxa.INDIVIDUAL = Convert.ToString(idr["INDIVIDUAL"]);
                taxa.SENSISALDO = Convert.ToString(idr["SENSISALDO"]);
                taxa.COBCANC = Convert.ToString(idr["COBCANC"]);
                taxa.COBCANCUTIL = Convert.ToString(idr["COBCANCUTIL"]);
                taxa.COBUTIL = Convert.ToString(idr["COBUTIL"]);
                taxa.COBUTILGRUPO = Convert.ToString(idr["COBUTILGRUPO"]);
                taxa.TAXADEFAULT = Convert.ToString(idr["TAXADEFAULT"]);
                taxa.DESTA = Convert.ToString(idr["DESTA"]);
                taxaCli.Add(taxa);
            }
            idr.Close();
            return taxaCli;
        }

        public MODTAXA ConsultaCodTaxaCli(int codCli, int codTaxa)
        {
            var taxa = new MODTAXA();
            const string sql = "PROC_LER_TAXACLIPJ";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@SISTEMA", DbType.String, ConstantesSIL.SistemaPOS);
            db.AddInParameter(cmd, "@CODCLI", DbType.String, codCli);
            db.AddInParameter(cmd, "@CODTAXA", DbType.String, codTaxa);
            var idr = db.ExecuteReader(cmd);
            if (idr.Read())
            {                
                taxa.COD = Convert.ToInt32(idr["CODCLI"]);
                taxa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxa.NOMTAXA = Convert.ToString(idr["NOMTAXA"]);
                taxa.TIPO = Convert.ToInt32(idr["TIPO"]);
                taxa.VALOR = Convert.ToDecimal(idr["VALTIT"]);
                taxa.VALORDEP = idr["VALDEP"] == DBNull.Value ? 0 : Convert.ToDecimal(idr["VALDEP"]);
                taxa.DTINICIO = Convert.ToDateTime(idr["DTINICIO"]);
                taxa.DIASPINICIO = Convert.ToInt32(idr["MESESPINICIO"]);
                taxa.NUMPARC = Convert.ToInt32(idr["NUMPAC"]);
                taxa.PGTOTAXA = idr["PGTOTAXA"].ToString() == "N" ? (short)0 : (short)1;
                taxa.COBRAATV = Convert.ToString(idr["COBRAATV"]);
                taxa.INDIVIDUAL = Convert.ToString(idr["INDIVIDUAL"]);
                taxa.COBCANC = Convert.ToString(idr["COBCANC"]);
                taxa.COBCANCUTIL = Convert.ToString(idr["COBCANCUTIL"]);
                taxa.COBUTIL = Convert.ToString(idr["COBUTIL"]);
                taxa.COBUTILGRUPO = Convert.ToString(idr["COBUTILGRUPO"]);
            }
            idr.Close();
            return taxa;
        }

        public TAXACLIENTE ConsultaTaxaVA(int codTaxa)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT TIPO, TRENOVA, TAXADEFAULT FROM TAXAVA WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODTAXA = @CODTAXA ");
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODTAXA", DbType.Int32, codTaxa);
            var idr = db.ExecuteReader(cmd);
            var taxaCli = new TAXACLIENTE();
            if (idr.Read())
            {
                taxaCli.TIPO = Convert.ToInt32(idr["TIPO"]);
                taxaCli.TRENOVA = Convert.ToString(idr["TRENOVA"]);
                taxaCli.TAXADEFAULT = Convert.ToString(idr["TAXADEFAULT"]);


            }
            idr.Close();
            return taxaCli;
        }

        public TAXACLIENTE ConsultaTaxaPJ(int codTaxa)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT TIPO, TRENOVA, TAXADEFAULT FROM TAXA WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODTAXA = @CODTAXA ");
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODTAXA", DbType.Int32, codTaxa);
            var idr = db.ExecuteReader(cmd);
            var taxaCli = new TAXACLIENTE();
            if (idr.Read())
            {
                taxaCli.TIPO = Convert.ToInt32(idr["TIPO"]);
                taxaCli.TRENOVA = Convert.ToString(idr["TRENOVA"]);
                taxaCli.TAXADEFAULT = Convert.ToString(idr["TAXADEFAULT"]);


            }
            idr.Close();
            return taxaCli;
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
                sql.AppendLine(" FROM BENEFCLI C WITH (NOLOCK) ");

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

        #region Agendamento Cancelamento Cliente

        public List<AGENDCANCCLIENTE> GetAgenCancCliente()
        {
            try
            {               
                var sql = new StringBuilder();
                sql.AppendLine(" SELECT");
                sql.AppendLine(" A.CODAGENDAMENTO, A.DATSOLICITACAO, A.DATACAO, A.STATUS, A.CODCLI, C.NOMCLI, A.SISTEMA, A.IDFUNC, A.MSGEXECUCAO, O.NOME, C.STA ");
                sql.AppendLine(" FROM AGEN_CANC_CLIENTE A WITH (NOLOCK) ");
                sql.AppendLine(" INNER JOIN VRESUMOCLI C WITH (NOLOCK) ON A.CODCLI = C.CODCLI AND A.SISTEMA = C.SISTEMA ");
                sql.AppendLine(" INNER JOIN OPERVAWS O  WITH (NOLOCK) ON A.IDFUNC = O.ID_FUNC ");
                sql.AppendLine(" WHERE A.STATUS = 'PENDENTE' ");

                Database db = new SqlDatabase(BDTELENET);
                var cmd = db.GetSqlStringCommand(sql.ToString());                
                var idr = db.ExecuteReader(cmd);
                var listAgendamento = new List<AGENDCANCCLIENTE>();
                while (idr.Read())
                {
                    var agendamento = new AGENDCANCCLIENTE
                    {
                        CODAGENDAMENTO = Convert.ToInt32(idr["CODAGENDAMENTO"]),
                        SISTEMA = Convert.ToInt16(idr["SISTEMA"]),
                        CODCLI = Convert.ToInt32(idr["CODCLI"]),
                        STACLI = Convert.ToString(idr["STA"]),
                        NOMCLIENTE = Convert.ToString(idr["NOMCLI"]),
                        DATSOLICITACAO = Convert.ToDateTime(idr["DATSOLICITACAO"]),
                        DATACAO = Convert.ToDateTime(idr["DATACAO"]),
                        STATUS = Convert.ToString(idr["STATUS"]),
                        IDFUNC = Convert.ToInt32(idr["IDFUNC"]),
                        NOMOPERADOR = Convert.ToString(idr["NOME"]),
                        MSGEXECUCAO = Convert.ToString(idr["STATUS"])                        
                    };
                    listAgendamento.Add(agendamento);
                }
                idr.Close();
                return listAgendamento;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IncluirAgenCancCliente(AGENDCANCCLIENTE agendamento, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_INSERE_AGEND_CANC_CLIENTE";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            IDataReader idr = null;
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, agendamento.SISTEMA);
            db.AddInParameter(cmd, "DATACAO", DbType.DateTime, agendamento.DATACAO);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, agendamento.CODCLI);
            db.AddInParameter(cmd, "IDFUNC", DbType.Int32, agendamento.IDFUNC);

            dbc.Open();
            try
            {
                idr = db.ExecuteReader(cmd);
                if (idr.Read())
                {
                    retorno = Convert.ToString(idr["RETORNO"]);
                    if (retorno == "0")
                    {
                        retorno = Convert.ToString(idr["MENSAGEM"]);
                        idr.Close();
                        return true;
                    }
                    idr.Close();
                    retorno = Convert.ToString(idr["MENSAGEM"]);
                }
                return false;
            }
            catch (Exception err)
            {
                if (idr != null) idr.Close();
                retorno = "Erro ao cadastrar o operador.";
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool ExcluirAgenCancCliente(int codAgendamento, int idFunc, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_EXCLUIR_AGEND_CANC_CLIENTE";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            IDataReader idr = null;            
            db.AddInParameter(cmd, "CODAGENDAMENTO", DbType.Int32, codAgendamento);
            db.AddInParameter(cmd, "ID_FUNC", DbType.Int32, idFunc);

            dbc.Open();            
            try
            {
                idr = db.ExecuteReader(cmd);
                if (idr.Read())
                {
                    retorno = Convert.ToString(idr["RETORNO"]);
                    if (retorno == "0")
                    {
                        retorno = Convert.ToString(idr["MENSAGEM"]);
                        idr.Close();
                        return true;
                    }
                    idr.Close();
                    retorno = Convert.ToString(idr["MENSAGEM"]);
                }                
                return false;
            }
            catch (Exception err)
            {
                if (idr != null) idr.Close();
                retorno = "Erro ao cadastrar o operador.";                
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        #endregion

        #region Importação de planilhas
        public List<RESUMOIMPORTACAO> ImportarUsuariosTelenet(string diretorio, string arquivo, string limiteImediato, string validaCpf, int codope, int codcli)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var retorno = new List<RESUMOIMPORTACAO>();

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("IMPORTA_IDENTIFICA_ARQ_PERFIL_TELENET", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.CommandTimeout = 600;
                
                cmd.Parameters.Add("@DIRETORIO", SqlDbType.VarChar).Value = diretorio;
                cmd.Parameters.Add("@ARQUIVO", SqlDbType.VarChar).Value = arquivo;
                cmd.Parameters.Add("@LIMITE_IMEDIATO", SqlDbType.VarChar).Value = limiteImediato;
                cmd.Parameters.Add("@VALIDA_CPF", SqlDbType.VarChar).Value = validaCpf;
                cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = codope;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codcli;
                cmd.Parameters.Add("@IDOPEWEB", SqlDbType.Int).Value = 0;


                // Executando o commando e obtendo o resultado
                reader = cmd.ExecuteReader();

                // Exibindo os registros
                while (reader.Read())
                {
                    var resumo = new RESUMOIMPORTACAO();
                    resumo.RegistroLog = Convert.ToString(reader["REGISTRO_LOG"]).Replace("<", "&lt;").Replace(">", "&gt;");
                    resumo.TipoRegistro = Convert.ToChar(reader["TIPO_REG"]);
                    retorno.Add(resumo);
                }
                return retorno;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public List<RESUMOIMPORTACAO> ImportarUsuarios(string diretorio, string arquivo, string limiteImediato, string validaCpf, int codope, int codcli)
        {

            SqlConnection conn = null;
            SqlDataReader reader = null;
            var retorno = new List<RESUMOIMPORTACAO>();

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("IMPORTA_IDENTIFICA_ARQ", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.CommandTimeout = 600;

                cmd.Parameters.Add("@DIRETORIO", SqlDbType.VarChar).Value = diretorio;
                cmd.Parameters.Add("@ARQUIVO", SqlDbType.VarChar).Value = arquivo;
                cmd.Parameters.Add("@LIMITE_IMEDIATO", SqlDbType.VarChar).Value = limiteImediato;
                cmd.Parameters.Add("@VALIDA_CPF", SqlDbType.VarChar).Value = validaCpf;
                cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = codope;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codcli;
                cmd.Parameters.Add("@IDOPEWEB", SqlDbType.Int).Value = 0;

                // Executando o commando e obtendo o resultado
                reader = cmd.ExecuteReader();

                // Exibindo os registros
                while (reader.Read())
                {
                    var resumo = new RESUMOIMPORTACAO();
                    resumo.RegistroLog = Convert.ToString(reader["REGISTRO_LOG"]).Replace("<", "&lt;").Replace(">", "&gt;");
                    resumo.TipoRegistro = Convert.ToChar(reader["TIPO_REG"]);
                    retorno.Add(resumo);
                }
                return retorno;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public DataTable ProcessoImportacao(string diretorio, string arquivo, int codope)
        {

            SqlConnection conexao = new SqlConnection(BDTELENET);
            SqlCommand comando = new SqlCommand("TAREFAS_IDENTIFICA_ARQUIVO", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            try
            {
                comando.Parameters.Add(new SqlParameter() { ParameterName = "@DIRETORIO", DbType = DbType.String, Value = diretorio });
                comando.Parameters.Add(new SqlParameter() { ParameterName = "@ARQUIVO", DbType = DbType.String, Value = arquivo });
                comando.Parameters.Add(new SqlParameter() { ParameterName = "@CODOPE", DbType = DbType.Int32, Value = codope });
                comando.Parameters.Add(new SqlParameter() { ParameterName = "@IDOPEWEB", DbType = DbType.Int32, Value = 0 });
                //comando.Parameters.Add(new SqlParameter() { ParameterName = "@ERRO", DbType = DbType.Int32, Direction = ParameterDirection.Output });
                //comando.Parameters.Add(new SqlParameter() { ParameterName = "@MSG_ERRO", DbType = DbType.String, Direction = ParameterDirection.Output });

                DataTable dt = new DataTable();

                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                //var erro = comando.Parameters["@ERRO"].Value.ToString();
                //var msgErro = comando.Parameters["@MSG_ERRO"].Value.ToString();

                return dt;
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();
                throw;
            }
        }

        #endregion

        private bool PosssuiPremio(int idCliente)
        {
            try
            {
                bool Retorno = false;

                Database db = new SqlDatabase(BDTELENET);
                var sql = new StringBuilder();
                
                sql.AppendLine(" SELECT top 1 ");
                sql.AppendLine("        ISNULL(U.PMO,0) AS PMO ");
                sql.AppendLine("   FROM USUARIO U WITH (NOLOCK) ");
                sql.AppendLine("       ,CLIENTE_POS CP WITH (NOLOCK) ");
                sql.AppendLine("  WHERE U.CODCLI = CP.CODCLI ");
                sql.AppendLine("    AND CP.ID_CLIENTE = @ID_CLIENTE ");
                sql.AppendLine("    AND U.PMO > 0 ");

                var cmd = db.GetSqlStringCommand(sql.ToString());
                db.AddInParameter(cmd, "ID_CLIENTE", DbType.Int32, idCliente);

                var idr = db.ExecuteReader(cmd);
                var listProdutosCli = new List<VPRODUTOSCLI>();

                while (idr.Read())
                {
                    if (Convert.ToDecimal(idr["PMO"]) > 0)
                        Retorno = true;

                }
                return Retorno;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}