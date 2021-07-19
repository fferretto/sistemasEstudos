using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Text;
using TELENET.SIL.PO;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TELENET.SIL.DA
{
    internal class daCredenciadoVANovo
    {
        private readonly string BDTELENET = string.Empty;
        private readonly string BDCONCENTRADOR = string.Empty;
        private readonly string BDAUTORIZADOR = string.Empty;
        private readonly OPERADORA FOperador;

        public daCredenciadoVANovo(OPERADORA Operador)
        {
            if (Operador == null) return;
            FOperador = Operador;

            // Monta String Conexao
            BDCONCENTRADOR = string.Format(ConstantesSIL.BDCONCENTRADOR, Operador.SERVIDORCON, Operador.BANCOCON, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);            
            BDAUTORIZADOR = string.Format(ConstantesSIL.BDAUTORIZADOR, Operador.SERVIDORAUT, Operador.BANCOAUT, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        #region GET Credenciados

        public VCREDENCIADO GetCredenciadoCodNome(int codCre)
        {
            var credenciado = new VCREDENCIADO();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT CODCRE, RAZSOC ");
            sql.AppendLine("FROM VCREDENCIADO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODCRE = @CODCRE ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                credenciado.CODCRE = codCre;
                credenciado.RAZSOC = idr["RAZSOC"].ToString();
            }
            idr.Close();
            return credenciado;
        }

        public CADCREDENCIADO GetCadCredenciado(int idCredenciado)
        {
            var cadCredenciado = new CADCREDENCIADO();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ID_CREDENCIADO, CNPJ, RAZSOC, NOMFAN, NOMEXIBICAO, INSEST, DESREG, ENDCRE, ");
            sql.AppendLine("ENDCPL, NOMBAI, NOMLOC, NOMUF0, CEP, TEL, FAX, EMA, COA, ENDCOR, ENDCPLCOR, ");
            sql.AppendLine("NOMBAICOR, NOMLOCCOR, NOMUF0COR, CEPCOR, CODCRE ");
            sql.AppendLine("FROM VRESUMOCRE WITH (NOLOCK) ");
            sql.AppendLine("WHERE ID_CREDENCIADO = @ID_CREDENCIADO ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ID_CREDENCIADO", DbType.Int32, idCredenciado);
            var idr = db.ExecuteReader(cmd);
            if (idr.Read())
            {
                cadCredenciado.ID_CREDENCIADO = idCredenciado;
                if (idr["CODCRE"] != DBNull.Value) cadCredenciado.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                if (idr["CNPJ"] != DBNull.Value) cadCredenciado.CNPJ_CPF = Convert.ToString(idr["CNPJ"]);
                if (idr["RAZSOC"] != DBNull.Value) cadCredenciado.RAZAO = Convert.ToString(idr["RAZSOC"]);
                if (idr["NOMFAN"] != DBNull.Value) cadCredenciado.FANTASIA = Convert.ToString(idr["NOMFAN"]);
                if (idr["NOMEXIBICAO"] != DBNull.Value) cadCredenciado.NOMEXIBICAO = Convert.ToString(idr["NOMEXIBICAO"]);
                if (idr["INSEST"] != DBNull.Value) cadCredenciado.INSC_ESTADUAL = Convert.ToString(idr["INSEST"]);
                if (idr["DESREG"] != DBNull.Value) cadCredenciado.REGIAO = Convert.ToString(idr["DESREG"]);
                if (idr["ENDCRE"] != DBNull.Value) cadCredenciado.ENDERECO = Convert.ToString(idr["ENDCRE"]);
                if (idr["ENDCPL"] != DBNull.Value) cadCredenciado.COMP = Convert.ToString(idr["ENDCPL"]);
                if (idr["NOMBAI"] != DBNull.Value) cadCredenciado.BAIRRO = Convert.ToString(idr["NOMBAI"]);
                if (idr["NOMLOC"] != DBNull.Value) cadCredenciado.LOCALIDADE = Convert.ToString(idr["NOMLOC"]);
                if (idr["NOMUF0"] != DBNull.Value) cadCredenciado.UF = Convert.ToString(idr["NOMUF0"]);
                if (idr["CEP"] != DBNull.Value) cadCredenciado.CEP = Convert.ToString(idr["CEP"]);
                if (idr["TEL"] != DBNull.Value) cadCredenciado.TELEFONE = Convert.ToString(idr["TEL"]);
                if (idr["FAX"] != DBNull.Value) cadCredenciado.FAX = Convert.ToString(idr["FAX"]);
                if (idr["EMA"] != DBNull.Value) cadCredenciado.EMAIL = Convert.ToString(idr["EMA"]);
                if (idr["COA"] != DBNull.Value) cadCredenciado.CONTATO = Convert.ToString(idr["COA"]);
                if (idr["ENDCOR"] != DBNull.Value) cadCredenciado.ENDERECO_CORRESP = Convert.ToString(idr["ENDCOR"]);
                if (idr["ENDCPLCOR"] != DBNull.Value) cadCredenciado.COMP_CORRESP = Convert.ToString(idr["ENDCPLCOR"]);
                if (idr["NOMBAICOR"] != DBNull.Value) cadCredenciado.BAIRRO_CORRESP = Convert.ToString(idr["NOMBAICOR"]);
                if (idr["NOMLOCCOR"] != DBNull.Value) cadCredenciado.LOCALIDADE_CORRESP = Convert.ToString(idr["NOMLOCCOR"]);
                if (idr["NOMUF0COR"] != DBNull.Value) cadCredenciado.UF_CORRESP = Convert.ToString(idr["NOMUF0COR"]);
                if (idr["CEPCOR"] != DBNull.Value) cadCredenciado.CEP_CORRESP = Convert.ToString(idr["CEPCOR"]);
            }
            idr.Close();
            return cadCredenciado;
        }

        public List<VCREDENCIADO> GetListCredenciado(int idCredenciado)
        {
            var listCred = new List<VCREDENCIADO>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ");
            sql.AppendLine("CODCRE, CNPJ, ID_CREDENCIADO, CODLOC, CODBAI, SIGUF0, RAZSOC, NOMFAN, NOMEXIBICAO, ");
            sql.AppendLine("TEL, COA, EMA, FAX, ENDCRE, CEP, INSEST, ENDCPL, ENDCOR, ENDCPLCOR, CODBAICOR, CODLOCCOR, ");
            sql.AppendLine("SIGUF0COR, CEPCOR, CODREG, CODATI, CODSEG, STA, DATSTA, DATINC, DATCTT, CTABCO, TAXSER, DATTAXSER, ");
            sql.AppendLine("MSGATVCRT, TIPFEC, DIAFEC, NUMFEC, DATULTFEC, DATPRCULTFEC, CGC, CODFILNUT, SENCRE, PRAZO_PGTO, ");
            sql.AppendLine("LOCPAG, LOCAL_PAGTO, TIPEST, TIPO, CODMAT, QTEFIL, CODCEN, FORPAG, CODCAN, CODREO, CODEPS, TRANSHAB, ");
            sql.AppendLine("FLAG_CAD_PJ, FLAG_CAD_VA, MASCONTA, INTVENDINI, INTVENDFIN, CTRLFUNC, DIAFEC_VA, PRAENT, PRAREE, ");
            sql.AppendLine("TAXADM, DATTAXADM, CTABCO_VA, NUMFEC_VA, DATULTFEC_VA, TIPFEC_VA, DATCTT_VA, LAYADIVA, LAYADIPJ, ");
            sql.AppendLine("NUMPACVA, NUMULTPACVA, VALANUVA, DTANUVA, DTRENANU, AUTARQSF, TIPOCE, NUMSEQRV, NUMARQ_CONCILIA, ");
            sql.AppendLine("MAXPARCPS, DTSENHA, ULTACESSO, QTDEACESSOINV, DTEXPSENHA, SENWEB, CODPRI, NOMATI, NOMSEG, ");
            sql.AppendLine("FILIAL_REDE, SENHA, TIPO_REEMBOLSO, DATINC_VA ");
            sql.AppendLine("FROM VCREDENCIADO WITH (NOLOCK) ");
            sql.AppendLine("WHERE ID_CREDENCIADO = @ID_CREDENCIADO ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ID_CREDENCIADO", DbType.Int32, idCredenciado);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var credenciado = new VCREDENCIADO();
                credenciado.CODMAT = new VCREDENCIADO();
                credenciado.CODCEN = new VCREDENCIADO();
                credenciado.CODPRI = new VCREDENCIADO();

                if (idr["ID_CREDENCIADO"] != DBNull.Value) credenciado.ID_CREDENCIADO = Convert.ToInt32(idr["ID_CREDENCIADO"]);
                if (idr["RAZSOC"] != DBNull.Value) credenciado.RAZSOC = Convert.ToString(idr["RAZSOC"]);
                if (idr["CODCRE"] != DBNull.Value) credenciado.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                if (idr["NOMEXIBICAO"] != DBNull.Value) credenciado.NOMEXIBICAO = Convert.ToString(idr["NOMEXIBICAO"]);
                if (idr["CNPJ"] != DBNull.Value) credenciado.CNPJ_CPF = Convert.ToString(idr["CNPJ"]);
                if (idr["CODMAT"] != DBNull.Value) credenciado.CODMAT = GetCredenciadoCodNome(Convert.ToInt32(idr["CODMAT"]));
                if (idr["CODCEN"] != DBNull.Value) credenciado.CODCEN = GetCredenciadoCodNome(Convert.ToInt32(idr["CODCEN"]));
                if (idr["CODPRI"] != DBNull.Value) credenciado.CODPRI = GetCredenciadoCodNome(Convert.ToInt32(idr["CODPRI"]));
                if (idr["NOMATI"] != DBNull.Value) credenciado.RAMO_ATIVIDADE = Convert.ToString(idr["NOMATI"]);
                if (idr["NOMSEG"] != DBNull.Value) credenciado.SEGMENTO = Convert.ToString(idr["NOMSEG"]);

                if (idr["FILIAL_REDE"] != DBNull.Value) credenciado.FILIAL_REDE = Convert.ToString(idr["FILIAL_REDE"]);
                if (idr["SENHA"] != DBNull.Value) credenciado.SENHA = idr["SENHA"].ToString();
                if (idr["PRAZO_PGTO"] != DBNull.Value) credenciado.PRAZO_PGTO = Convert.ToInt16(idr["PRAZO_PGTO"]);
                if (idr["TIPO"] != DBNull.Value) credenciado.TIPO = Convert.ToString(idr["TIPO"]);
                
                if (idr["TIPO_REEMBOLSO"] != DBNull.Value) credenciado.TIPO_REEMBOLSO = Convert.ToString(idr["TIPO_REEMBOLSO"]);                
                if (idr["CODREO"] != DBNull.Value) credenciado.CODREO = Convert.ToInt16(idr["CODREO"]);
                if (idr["CODEPS"] != DBNull.Value) credenciado.CODEPS = Convert.ToInt16(idr["CODEPS"]);

                if (idr["LOCAL_PAGTO"] != DBNull.Value) credenciado.LOCAL_PAGTO = idr["LOCAL_PAGTO"].ToString();
                if (idr["TRANSHAB"] != DBNull.Value) credenciado.TRANSHAB = idr["TRANSHAB"].ToString();
                if (idr["FLAG_CAD_PJ"] != DBNull.Value) credenciado.FLAG_CAD_PJ = Convert.ToChar(idr["FLAG_CAD_PJ"]);
                if (idr["FLAG_CAD_VA"] != DBNull.Value) credenciado.FLAG_CAD_VA = Convert.ToChar(idr["FLAG_CAD_VA"]);                
                if (idr["DIAFEC_VA"] != DBNull.Value) credenciado.DIA_FECH_PP = Convert.ToInt32(idr["DIAFEC_VA"]);
                if (idr["PRAREE"] != DBNull.Value) credenciado.PRAZO_REE_VA = Convert.ToInt16(idr["PRAREE"]);
                if (idr["TAXADM"] != DBNull.Value) credenciado.TAXA_ADM_PP = Convert.ToDecimal(idr["TAXADM"]);
                if (idr["DATTAXADM"] != DBNull.Value) credenciado.DATA_TAXA_ADM = Convert.ToDateTime(idr["DATTAXADM"]);
                if (idr["CTABCO_VA"] != DBNull.Value) credenciado.CONTA_VA = idr["CTABCO_VA"].ToString();
                if (idr["NUMFEC_VA"] != DBNull.Value) credenciado.NUMFEC_VA = Convert.ToInt32(idr["NUMFEC_VA"]);
                if (idr["DATULTFEC_VA"] != DBNull.Value) credenciado.DATULTFEC_VA = Convert.ToDateTime(idr["DATULTFEC_VA"]);
                if (idr["TIPFEC_VA"] != DBNull.Value) credenciado.TIPO_FECHAMENTO_PP = Convert.ToInt32(idr["TIPFEC_VA"]);
                if (idr["DATCTT_VA"] != DBNull.Value) credenciado.DATA_CONTRATO_VA = Convert.ToDateTime(idr["DATCTT_VA"]);
                if (idr["STA"] != DBNull.Value) credenciado.STA = idr["STA"].ToString();
                if (idr["DATSTA"] != DBNull.Value) credenciado.DATSTA = Convert.ToDateTime(idr["DATSTA"]);
                if (idr["DATINC"] != DBNull.Value) credenciado.DATA_INC_PJ = Convert.ToDateTime(idr["DATINC"]);
                if (idr["DATINC_VA"] != DBNull.Value) credenciado.DATA_INC_VA = Convert.ToDateTime(idr["DATINC_VA"]);
                if (idr["DATCTT"] != DBNull.Value) credenciado.DATA_CONTRATO_PJ = Convert.ToDateTime(idr["DATCTT"]);
                if (idr["CTABCO"] != DBNull.Value) credenciado.CONTA_PJ = idr["CTABCO"].ToString();
                if (idr["TAXSER"] != DBNull.Value) credenciado.TAXA_SERV = Convert.ToDecimal(idr["TAXSER"]);
                if (idr["DATTAXSER"] != DBNull.Value) credenciado.DATA_TAXA_SERV = Convert.ToDateTime(idr["DATTAXSER"]);
                if (idr["MSGATVCRT"] != DBNull.Value) credenciado.MENS_ATIV_CARTAO = Convert.ToChar(idr["MSGATVCRT"]);
                if (idr["TIPFEC"] != DBNull.Value) credenciado.TIPO_FECHAMENTO_PJ = Convert.ToInt32(idr["TIPFEC"]);
                if (idr["DIAFEC"] != DBNull.Value) credenciado.DIA_FECH_PJ = Convert.ToInt32(idr["DIAFEC"]);
                if (idr["NUMFEC"] != DBNull.Value) credenciado.NUMFEC_PJ = Convert.ToInt32(idr["NUMFEC"]);
                if (idr["DATULTFEC"] != DBNull.Value) credenciado.DATULTFEC_PJ = Convert.ToDateTime(idr["DATULTFEC"]);
                if (idr["MAXPARCPS"] != DBNull.Value) credenciado.MAXPARC = Convert.ToInt32(idr["MAXPARCPS"]);

                listCred.Add(credenciado);
            }
            idr.Close();
            return listCred;
        }

        public VCREDENCIADO GetCredenciado(int codCre)
        {
            var credenciado = new VCREDENCIADO();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ");
            sql.AppendLine("CODCRE, CNPJ, ID_CREDENCIADO, CODLOC, CODBAI, SIGUF0, RAZSOC, NOMFAN, NOMEXIBICAO, ");
            sql.AppendLine("TEL, COA, EMA, FAX, ENDCRE, CEP, INSEST, ENDCPL, ENDCOR, ENDCPLCOR, CODBAICOR, CODLOCCOR, ");
            sql.AppendLine("SIGUF0COR, CEPCOR, CODREG, CODATI, CODSEG, STA, DATSTA, DATINC, DATCTT, CTABCO, TAXSER, DATTAXSER, ");
            sql.AppendLine("MSGATVCRT, TIPFEC, DIAFEC, NUMFEC, DATULTFEC, DATPRCULTFEC, CGC, CODFILNUT, SENCRE, PRAZO_PGTO, ");
            sql.AppendLine("LOCPAG, LOCAL_PAGTO, TIPEST, TIPO, CODMAT, QTEFIL, CODCEN, FORPAG, CODCAN, CODREO, CODEPS, TRANSHAB, ");
            sql.AppendLine("FLAG_CAD_PJ, FLAG_CAD_VA, MASCONTA, INTVENDINI, INTVENDFIN, CTRLFUNC, DIAFEC_VA, PRAENT, PRAREE, ");
            sql.AppendLine("TAXADM, DATTAXADM, CTABCO_VA, NUMFEC_VA, DATULTFEC_VA, TIPFEC_VA, DATCTT_VA, LAYADIVA, LAYADIPJ, ");
            sql.AppendLine("NUMPACVA, NUMULTPACVA, VALANUVA, DTANUVA, DTRENANU, AUTARQSF, TIPOCE, NUMSEQRV, NUMARQ_CONCILIA, ");
            sql.AppendLine("MAXPARCPS, DTSENHA, ULTACESSO, QTDEACESSOINV, DTEXPSENHA, SENWEB, CODPRI, NOMATI, NOMSEG, ");
            sql.AppendLine("FILIAL_REDE, SENHA, TIPO_REEMBOLSO, DATINC_VA ");
            sql.AppendLine("FROM VCREDENCIADO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODCRE = @CODCRE ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);
            var idr = db.ExecuteReader(cmd);
            if (idr.Read())
            {
                if (idr["ID_CREDENCIADO"] != DBNull.Value) credenciado.ID_CREDENCIADO = Convert.ToInt32(idr["ID_CREDENCIADO"]);
                if (idr["CODCRE"] != DBNull.Value) credenciado.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                if (idr["RAZSOC"] != DBNull.Value) credenciado.RAZSOC = Convert.ToString(idr["RAZSOC"]);
                if (idr["NOMEXIBICAO"] != DBNull.Value) credenciado.NOMEXIBICAO = Convert.ToString(idr["NOMEXIBICAO"]);
                if (idr["CNPJ"] != DBNull.Value) credenciado.CNPJ_CPF = idr["CNPJ"].ToString();
                if (idr["CODMAT"] != DBNull.Value) credenciado.CODMAT = GetCredenciadoCodNome(Convert.ToInt32(idr["CODMAT"]));
                if (idr["CODCEN"] != DBNull.Value) credenciado.CODCEN = GetCredenciadoCodNome(Convert.ToInt32(idr["CODCEN"]));
                if (idr["CODPRI"] != DBNull.Value) credenciado.CODPRI = GetCredenciadoCodNome(Convert.ToInt32(idr["CODPRI"]));
                if (idr["NOMATI"] != DBNull.Value) credenciado.RAMO_ATIVIDADE = Convert.ToString(idr["NOMATI"]);
                if (idr["NOMSEG"] != DBNull.Value) credenciado.SEGMENTO = Convert.ToString(idr["NOMSEG"]);

                if (idr["FILIAL_REDE"] != DBNull.Value) credenciado.FILIAL_REDE = Convert.ToString(idr["FILIAL_REDE"]);
                if (idr["SENHA"] != DBNull.Value) credenciado.SENHA = idr["SENHA"].ToString();
                if (idr["PRAZO_PGTO"] != DBNull.Value) credenciado.PRAZO_PGTO = Convert.ToInt16(idr["PRAZO_PGTO"]);
                if (idr["TIPO"] != DBNull.Value) credenciado.TIPO = Convert.ToString(idr["TIPO"]);
                if (idr["QTEFIL"] != DBNull.Value) credenciado.QUANT_FILIAIS = Convert.ToInt16(idr["QTEFIL"]);
                if (idr["TIPO_REEMBOLSO"] != DBNull.Value) credenciado.TIPO_REEMBOLSO = Convert.ToString(idr["TIPO_REEMBOLSO"]);
                if (idr["CODREO"] != DBNull.Value) credenciado.CODREO = Convert.ToInt16(idr["CODREO"]);
                if (idr["CODEPS"] != DBNull.Value) credenciado.CODEPS = Convert.ToInt16(idr["CODEPS"]);
                if (idr["TRANSHAB"] != DBNull.Value) credenciado.TRANSHAB = idr["TRANSHAB"].ToString();
                if (idr["FLAG_CAD_PJ"] != DBNull.Value) credenciado.FLAG_CAD_PJ = Convert.ToChar(idr["FLAG_CAD_PJ"]);
                if (idr["FLAG_CAD_VA"] != DBNull.Value) credenciado.FLAG_CAD_VA = Convert.ToChar(idr["FLAG_CAD_VA"]);
                if (idr["DIAFEC_VA"] != DBNull.Value) credenciado.DIA_FECH_PP = Convert.ToInt32(idr["DIAFEC_VA"]);
                if (idr["PRAREE"] != DBNull.Value) credenciado.PRAZO_REE_VA = Convert.ToInt16(idr["PRAREE"]);
                if (idr["TAXADM"] != DBNull.Value) credenciado.TAXA_ADM_PP = Convert.ToDecimal(idr["TAXADM"]);
                if (idr["DATTAXADM"] != DBNull.Value) credenciado.DATA_TAXA_ADM = Convert.ToDateTime(idr["DATTAXADM"]);
                if (idr["CTABCO_VA"] != DBNull.Value) credenciado.CONTA_VA = idr["CTABCO_VA"].ToString();
                if (idr["NUMFEC_VA"] != DBNull.Value) credenciado.NUMFEC_VA = Convert.ToInt32(idr["NUMFEC_VA"]);
                if (idr["DATULTFEC_VA"] != DBNull.Value) credenciado.DATULTFEC_VA = Convert.ToDateTime(idr["DATULTFEC_VA"]);
                if (idr["TIPFEC_VA"] != DBNull.Value) credenciado.TIPO_FECHAMENTO_PP = Convert.ToInt32(idr["TIPFEC_VA"]);
                if (idr["DATCTT_VA"] != DBNull.Value) credenciado.DATA_CONTRATO_VA = Convert.ToDateTime(idr["DATCTT_VA"]);
                if (idr["STA"] != DBNull.Value) credenciado.STA = idr["STA"].ToString();
                if (idr["DATSTA"] != DBNull.Value) credenciado.DATSTA = Convert.ToDateTime(idr["DATSTA"]);
                if (idr["DATINC"] != DBNull.Value) credenciado.DATA_INC_PJ = Convert.ToDateTime(idr["DATINC"]);
                if (idr["DATINC_VA"] != DBNull.Value) credenciado.DATA_INC_VA = Convert.ToDateTime(idr["DATINC_VA"]);
                if (idr["DATCTT"] != DBNull.Value) credenciado.DATA_CONTRATO_PJ = Convert.ToDateTime(idr["DATCTT"]);
                if (idr["CTABCO"] != DBNull.Value) credenciado.CONTA_PJ = idr["CTABCO"].ToString();
                if (idr["TAXSER"] != DBNull.Value) credenciado.TAXA_SERV = Convert.ToDecimal(idr["TAXSER"]);
                if (idr["DATTAXSER"] != DBNull.Value) credenciado.DATA_TAXA_SERV = Convert.ToDateTime(idr["DATTAXSER"]);
                if (idr["MSGATVCRT"] != DBNull.Value) credenciado.MENS_ATIV_CARTAO = Convert.ToChar(idr["MSGATVCRT"]);
                if (idr["TIPFEC"] != DBNull.Value) credenciado.TIPO_FECHAMENTO_PJ = Convert.ToInt32(idr["TIPFEC"]);
                if (idr["DIAFEC"] != DBNull.Value) credenciado.DIA_FECH_PJ = Convert.ToInt32(idr["DIAFEC"]);
                if (idr["NUMFEC"] != DBNull.Value) credenciado.NUMFEC_PJ = Convert.ToInt32(idr["NUMFEC"]);
                if (idr["DATULTFEC"] != DBNull.Value) credenciado.DATULTFEC_PJ = Convert.ToDateTime(idr["DATULTFEC"]);
                if (idr["MAXPARCPS"] != DBNull.Value) credenciado.MAXPARC = Convert.ToInt32(idr["MAXPARCPS"]);
            }
            idr.Close();
            return credenciado;
        }

        public List<VCREDENCIADO> ColecaoCredenciados()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string query = "SELECT CODCRE, RAZSOC FROM CREDENCIADO WITH (NOLOCK) ORDER BY CODCRE";
            var cmd = db.GetSqlStringCommand(query);
            var idr = db.ExecuteReader(cmd);
            var colecaoCred = new List<VCREDENCIADO>();
            while (idr.Read())
            {
                var Credenciado = new VCREDENCIADO();
                Credenciado.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                Credenciado.RAZSOC = Convert.ToString(idr["RAZSOC"]);
                colecaoCred.Add(Credenciado);
            }
            idr.Close();
            return colecaoCred;
        }

        public List<VCREDENCIADO> GetCredenciadoCNPJPrincipal(int codCre)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string query = "SELECT CODCRE, RAZSOC FROM VCREDENCIADO WITH (NOLOCK) WHERE FLAG_VA = 'S' AND CODCRE = @CODCRE AND CODCRE = CODPRI ORDER BY CODCRE";
            var cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@CODCRE", DbType.String, codCre);
            var idr = db.ExecuteReader(cmd);
            var colecaoCred = new List<VCREDENCIADO>();            
            while (idr.Read())
            {
                var credenciado = new VCREDENCIADO();
                credenciado.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                credenciado.RAZSOC = Convert.ToString(idr["RAZSOC"]);
                colecaoCred.Add(credenciado);
            }
            idr.Close();
            return colecaoCred;
        }

        public int GetCredenciadoCodPosCodAfil(string codPos, string codAfil)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_CONSULTA_CODCRE ", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CODPOS", SqlDbType.VarChar).Value = codPos;
                cmd.Parameters.Add("@CODAFIL", SqlDbType.VarChar).Value = codAfil;

                var codCre = 0;
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {                    
                    if (reader["CODCRE"] != DBNull.Value) codCre = Convert.ToInt32(reader["CODCRE"]);
                    
                }
                return codCre;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public List<VRESUMOCRE> ColecaoCredenciados(string Filtro)
        {
            var colecaoCredenciado = new List<VRESUMOCRE>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT");
            sql.AppendLine("ID_CREDENCIADO, CODCRE, CNPJ, RAZSOC, NOMFAN, NOMEXIBICAO, ");
            sql.AppendLine("STA, DESTA, NOMBAI, NOMLOC");
            sql.AppendLine("FROM VRESUMOCRE WITH (NOLOCK) ");
            if (!string.IsNullOrEmpty(Filtro))
                sql.AppendLine(string.Format("WHERE {0} ", Filtro));
            sql.AppendLine("ORDER BY RAZSOC");

            IDataReader idr = null;
            try
            {
                var cmd = db.GetSqlStringCommand(sql.ToString());
                idr = db.ExecuteReader(cmd);
                while (idr.Read())
                {
                    var cadCredenciado = new VRESUMOCRE();
                    cadCredenciado.ID_CREDENCIADO = Convert.ToInt32(idr["ID_CREDENCIADO"]);
                    if (idr["CODCRE"] != DBNull.Value) cadCredenciado.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                    if (idr["RAZSOC"] != DBNull.Value) cadCredenciado.RAZSOC = Convert.ToString(idr["RAZSOC"]);
                    if (idr["CNPJ"] != DBNull.Value) cadCredenciado.CNPJ = UtilSIL.RetirarCaracteres("./-", Convert.ToString(idr["CNPJ"]));
                    if (idr["STA"] != DBNull.Value) cadCredenciado.STA = Convert.ToString(idr["STA"]);
                    if (idr["DESTA"] != DBNull.Value) cadCredenciado.DESTA = Convert.ToString(idr["DESTA"]);
                    if (idr["NOMFAN"] != DBNull.Value) cadCredenciado.NOMFAN = Convert.ToString(idr["NOMFAN"]);
                    if (idr["NOMEXIBICAO"] != DBNull.Value) cadCredenciado.NOMEXIBICAO = Convert.ToString(idr["NOMEXIBICAO"]);
                    if (idr["NOMLOC"] != DBNull.Value) cadCredenciado.LOCALIDADE = Convert.ToString(idr["NOMLOC"]);
                    if (idr["NOMBAI"] != DBNull.Value) cadCredenciado.BAIRRO = Convert.ToString(idr["NOMBAI"]);
                    colecaoCredenciado.Add(cadCredenciado);
                }
                idr.Close();
            }
            catch
            {
                if (idr != null)
                    idr.Close();
            }
            return colecaoCredenciado;
        }

        #endregion

        #region Get Codigo Afiliacao

        public string CodigoAfiliacao(string cnpj, int codCre)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT TB.CODAFIL + TB.REDE AS CODAFIL ");
            sql.AppendLine("  FROM TABRAFIL2 TB WITH (NOLOCK) ");
            sql.AppendLine("      ,OPERADORA OP WITH (NOLOCK) ");
            sql.AppendLine(" WHERE OP.CODOPE = TB.CODOPE ");
            sql.AppendLine("   AND TB.CNPJ = '" + cnpj + "'");
            sql.AppendLine("   AND SUBSTRING(TB.COD_DESTINO, 1, 6) = " + Convert.ToString(codCre).PadLeft(6, '0'));
            sql.AppendLine("   AND OP.BD_NC = (select O.BD_NC from OPERADORA O WITH (NOLOCK) where O.CODOPE = " + FOperador.CODOPE + " )");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var codAfil = Convert.ToString(db.ExecuteScalar(cmd)) != string.Empty
                              ? Convert.ToString(db.ExecuteScalar(cmd))
                              : "";
            return codAfil;
        }

        public string ObtemRede(string codAfil)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT REDE FROM TABRAFIL2 WITH (NOLOCK) WHERE ");
            sql.AppendLine("CODAFIL = '" + codAfil + "'");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var rede = Convert.ToString(db.ExecuteScalar(cmd)) != string.Empty
                           ? Convert.ToString(db.ExecuteScalar(cmd))
                           : "01";
            return rede;
        }

        public AFILIACAO RetornaDadosCred(string cnpj)
        {
            var dadosCred = new AFILIACAO();
            const string sql = "CONSULTA_DADOSCRED";
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                //dadosCred.CODAFIL = Convert.ToString(idr["CODAFIL"]);
                dadosCred.SIGUF0 = Convert.ToString(idr["SIGUF0"]);
                dadosCred.RAZSOC = Convert.ToString(idr["RAZSOC"]);
                dadosCred.NOMFAN = Convert.ToString(idr["NOMFAN"]);
                dadosCred.TEL = Convert.ToString(idr["TEL"]);
                dadosCred.FAX = Convert.ToString(idr["FAX"]);
                dadosCred.EMA = Convert.ToString(idr["EMA"]);
                dadosCred.ENDCRE = Convert.ToString(idr["ENDCRE"]);
                dadosCred.CEP = Convert.ToString(idr["CEP"]);
                dadosCred.INSEST = Convert.ToString(idr["INSEST"]);
                dadosCred.CGC = Convert.ToString(idr["CGC"]);
            }
            idr.Close();
            return dadosCred;
        }

        #endregion

        #region Get Proximo Codigo

        public int ProximoCodigoLivre()
        {
            const string sql = "PROC_PROX_CODCRE";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        public int ProximoCodigoLivreGrupoCrendenciados()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODGRUPO)+1, 1) AS ProxCod");
            sql.AppendLine("FROM GRUPO WITH (NOLOCK) ");
            sql.AppendLine("WHERE (CODGRUPO+1) NOT IN (SELECT CODGRUPO FROM GRUPO WITH (NOLOCK) )");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        #endregion

        #region Get Observacoes

        public List<CREDENCIADO_OBS> Observacoes(Int32 codCre)
        {
            var ColecaoObservacoes = new List<CREDENCIADO_OBS>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE, DATA, OBS, ID");
            sql.AppendLine("FROM OBSCRED WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODCRE = @CODCRE");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.String, codCre);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var Observacao = new CREDENCIADO_OBS();
                Observacao.CODCRE = codCre;
                Observacao.ID = Convert.ToInt32(idr["ID"]);
                Observacao.DATA = Convert.ToDateTime(idr["DATA"]);
                Observacao.OBS = Convert.ToString(idr["OBS"]);
                ColecaoObservacoes.Add(Observacao);
            }
            idr.Close();
            return ColecaoObservacoes;
        }

        #endregion

        #region Get Redes

        public List<REDES> ConsultaRedes(int codCre)
        {
            var redes = new List<REDES>();
            const string sql = "PROC_REDES_HABILITADAS";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODCRE", DbType.String, codCre);
            IDataReader idr = null;
            try
            {
                idr = db.ExecuteReader(cmd);
                while (idr.Read())
                {
                    var rede = new REDES();
                    rede.REDE = Convert.ToString(idr["REDE"]);
                    if (idr["CODCRE"] != DBNull.Value) rede.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                    rede.SELECIONADA = Convert.ToString(idr["SELECIONADA"]);
                    rede.NOME = Convert.ToString(idr["NOME"]);
                    rede.TEMCODAFIL = Convert.ToString(idr["TEMCODAFIL"]);
                    rede.HABREDE = Convert.ToString(idr["HABREDE"]) == "S"? "Sim" : "Não";
                    rede.CODAFILREDE = Convert.ToString(idr["CODAFILREDE"]);
                    if (idr["DATATUALIZ"] != DBNull.Value) rede.DATATUALIZ = Convert.ToDateTime(idr["DATATUALIZ"]);
                    rede.STATUSREDE = Convert.ToString(idr["STATUSREDE"]);
                    if (idr["DATAREDE"] != DBNull.Value) rede.DATAREDE = Convert.ToDateTime(idr["DATAREDE"]);                    
                    redes.Add(rede);
                }
                idr.Close();
            }
            catch 
            {
                if (idr != null)
                    idr.Close(); 
            }
            return redes;
        }

        #endregion

        #region Get Taxas Credenciado

        public List<MODTAXA> ConsultaTaxaCre(int codCre, int sistema)
        {
            var taxaCre = new List<MODTAXA>();
            const string sql = "PROC_LER_TAXACRE";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODCRE", DbType.String, codCre);
            db.AddInParameter(cmd, "@SISTEMA", DbType.String, sistema);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var taxa = new MODTAXA();
                taxa.COD = Convert.ToInt32(idr["CODCRE"]);
                taxa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxa.NOMTAXA = Convert.ToString(idr["NOMTAXA"]);
                taxa.VALOR = Convert.ToDecimal(idr["VALOR"]);
                taxa.NUMPARC = Convert.ToInt32(idr["NUMPAC"]);
                taxa.DTINC = Convert.ToDateTime(idr["DTINC"]);
                taxa.DTINICIO = Convert.ToDateTime(idr["DTINICIO"]);
                taxa.DIASPINICIO = Convert.ToInt32(idr["DIASPINICIO"]);
                taxa.COBSEMCRED = Convert.ToString(idr["COBSEMCRED"]);
                taxa.PRIORIDADE = Convert.ToInt32(idr["PRIORIDADE"]);
                taxa.DESTA = Convert.ToString(idr["DESTA"]);
                taxa.TAXAHAB = Convert.ToString(idr["TAXAHAB"]);
                taxa.VALMINCRED = Convert.ToDecimal(idr["VALMINCRED"]);
                taxa.DIACOB = Convert.ToInt32(idr["DIACOB"]);
                taxa.CENTRALIZADORA = Convert.ToString(idr["CENTRALIZADORA"]) == "S" ? "SIM" : "NÃO";
                taxaCre.Add(taxa);
            }
            idr.Close();
            return taxaCre;
        }

        public List<MODTAXA> ConsultaTaxaCre(int codCre)
        {
            var taxaCre = new List<MODTAXA>();
            const string sql = "PROC_LER_TAXACRE";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODCRE", DbType.String, codCre);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var taxa = new MODTAXA();
                taxa.SISTEMA = Convert.ToInt16(idr["SISTEMA"]);
                taxa.COD = Convert.ToInt32(idr["CODCRE"]);
                taxa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxa.NOMTAXA = Convert.ToString(idr["NOMTAXA"]);
                taxa.VALOR = Convert.ToDecimal(idr["VALOR"]);
                taxa.NUMPARC = Convert.ToInt32(idr["NUMPAC"]);
                taxa.DTINC = Convert.ToDateTime(idr["DTINC"]);
                taxa.DTINICIO = Convert.ToDateTime(idr["DTINICIO"]);
                taxa.DIASPINICIO = Convert.ToInt32(idr["DIASPINICIO"]);
                taxa.COBSEMCRED = Convert.ToString(idr["COBSEMCRED"]);
                taxa.PRIORIDADE = Convert.ToInt32(idr["PRIORIDADE"]);
                taxa.DESTA = Convert.ToString(idr["DESTA"]);
                taxa.TAXAHAB = Convert.ToString(idr["TAXAHAB"]);
                taxa.VALMINCRED = Convert.ToDecimal(idr["VALMINCRED"]);
                if (idr["DIACOB"] != DBNull.Value) taxa.DIACOB = Convert.ToInt32(idr["DIACOB"]);
                taxa.CENTRALIZADORA = Convert.ToString(idr["CENTRALIZADORA"]) == "S" ? "SIM" : "NÃO"; ;
                taxa.TRENOVA = Convert.ToString(idr["TRENOVA"]);
                taxaCre.Add(taxa);
            }
            idr.Close();
            return taxaCre;
        }

        public List<MODTAXA> ConsultaTaxasCre(int sistema, bool eCentralizadora, List<MODTAXA> existentes = null)
        {
            var taxasCre = new List<MODTAXA>();
            const string sql = "SELECT * FROM VTAXA WITH (NOLOCK) WHERE TIPO = 2 AND SISTEMA = @SISTEMA";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@SISTEMA", DbType.String, sistema);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var taxa = new MODTAXA();
                taxa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxa.SISTEMA = Convert.ToInt32(idr["SISTEMA"]);
                taxa.NOMTAXA = Convert.ToString(idr["NOMTAXA"]);
                taxa.CENTRALIZADORA = Convert.ToString(idr["CENTRALIZADORA"]) == "S" ? "SIM" : "NÃO";
                taxa.TRENOVA = Convert.ToString(idr["TRENOVA"]);
                taxa.DESTA = Convert.ToString(idr["STATUS"]) == "00" ? "ATIVO" : Convert.ToString(idr["STATUS"]) == "02" ? "CANCELADO" : "BLOQUEADO";

                if (existentes == null || !existentes.Where(e => e.SISTEMA == sistema && e.CODTAXA == taxa.CODTAXA).Any())
                {
                    if (!eCentralizadora && taxa.CENTRALIZADORA == "SIM")
                        continue;
                    taxasCre.Add(taxa);
                }
            }
            idr.Close();
            return taxasCre;
        }        

        #endregion

        #region Get Equipamentos

        public List<CTPOS> ColecaoEquipamentos(string codAfil, int codOpe)
        {
            var colecaoEquip = new List<CTPOS>();
            const string sql = "LISTA_TERMINAIS";
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODAFIL", DbType.String, codAfil);
            db.AddInParameter(cmd, "@CODOPE", DbType.Int32, codOpe);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var equipamento = new CTPOS();
                equipamento.CODPOS = Convert.ToString(idr["CODPOS"]);
                equipamento.EQUIPID = Convert.ToString(idr["EQUIPID"]);
                equipamento.TIPOEQ = Convert.ToString(idr["TIPOEQ"]);
                equipamento.VERAPL = Convert.ToString(idr["VERAPL"]);
                equipamento.TIPOCON = Convert.ToString(idr["TIPOCON"]);
                equipamento.INICIALIZADO = Convert.ToString(idr["INICIALIZADO"]);
                if (equipamento.INICIALIZADO == "S")
                {
                    if (idr["ULTINIC"] != DBNull.Value)
                        equipamento.ULTINIC = Convert.ToDateTime(idr["ULTINIC"]);
                }
                equipamento.OPERADORA = Convert.ToString(idr["OPERADORA"]);
                colecaoEquip.Add(equipamento);
            }
            idr.Close();
            return colecaoEquip;
        }

        #endregion

        #region TaxaCre

        public bool ExibeModuloTaxaCre()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'TAXACRE'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        #endregion

        #region Exibe novo formato bancário

        public bool ExibeNovoFormatoContaBanco()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'NOVACONTABANC'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        #endregion

        #region Equipamentos Parametrizacao

        public string EquipamentoArqParametrizacao(int Credenciado, string CodEquipamento)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "GERAPESTAB";
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODPS", DbType.Int32, Credenciado);
            db.AddInParameter(cmd, "@CODPOS", DbType.String, CodEquipamento);
            var idr = db.ExecuteReader(cmd);
            var LinhaCartao = string.Empty;
            if (idr.Read())
            {
                LinhaCartao += Convert.ToString(idr["OPERADORA"]);
                LinhaCartao += string.Empty.PadRight(80);
                LinhaCartao += "60";
                LinhaCartao += string.Empty.PadRight(02);
                LinhaCartao += Convert.ToString(idr["RAZSOC"]);
                LinhaCartao += string.Empty.PadRight(26);
                LinhaCartao += Convert.ToString(idr["CGC"]);
                LinhaCartao += string.Empty.PadRight(14);
                LinhaCartao += Convert.ToString(idr["ENDCRE"]);
                LinhaCartao += string.Empty.PadRight(30);
                LinhaCartao += Convert.ToString(idr["CGC"]);
                LinhaCartao += string.Empty.PadRight(26);
                LinhaCartao += Convert.ToString(CodEquipamento);
            }
            idr.Close();
            return LinhaCartao;
        }

        #endregion

        #region CRUD Credenciados

        public int Inserir(CADCREDENCIADO cadCredenciado, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;
            var idCredenciado = 0;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_INSERE_CREDENCIADO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@CNPJ_CPF", SqlDbType.VarChar).Value = cadCredenciado.CNPJ_CPF;
                cmd.Parameters.Add("@RAZAO", SqlDbType.VarChar).Value = cadCredenciado.RAZAO;
                cmd.Parameters.Add("@FANTASIA", SqlDbType.VarChar).Value = cadCredenciado.FANTASIA;
                cmd.Parameters.Add("@INSC_ESTADUAL", SqlDbType.VarChar).Value = cadCredenciado.INSC_ESTADUAL;
                cmd.Parameters.Add("@REGIAO", SqlDbType.VarChar).Value = cadCredenciado.REGIAO;
                cmd.Parameters.Add("@ENDERECO", SqlDbType.VarChar).Value = cadCredenciado.ENDERECO;
                cmd.Parameters.Add("@COMP", SqlDbType.VarChar).Value = cadCredenciado.COMP;
                cmd.Parameters.Add("@BAIRRO", SqlDbType.VarChar).Value = cadCredenciado.BAIRRO;
                cmd.Parameters.Add("@LOCALIDADE", SqlDbType.VarChar).Value = cadCredenciado.LOCALIDADE;
                cmd.Parameters.Add("@UF", SqlDbType.VarChar).Value = cadCredenciado.UF;
                cmd.Parameters.Add("@CEP", SqlDbType.VarChar).Value = cadCredenciado.CEP;
                cmd.Parameters.Add("@TELEFONE", SqlDbType.VarChar).Value = cadCredenciado.TELEFONE;
                cmd.Parameters.Add("@FAX", SqlDbType.VarChar).Value = cadCredenciado.FAX;
                cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = cadCredenciado.EMAIL;
                cmd.Parameters.Add("@CONTATO", SqlDbType.VarChar).Value = cadCredenciado.CONTATO;
                cmd.Parameters.Add("@ENDERECO_CORRESP", SqlDbType.VarChar).Value = cadCredenciado.ENDERECO_CORRESP;
                cmd.Parameters.Add("@COMP_CORRESP", SqlDbType.VarChar).Value = cadCredenciado.COMP_CORRESP;
                cmd.Parameters.Add("@BAIRRO_CORRESP", SqlDbType.VarChar).Value = cadCredenciado.BAIRRO_CORRESP;
                cmd.Parameters.Add("@LOCALIDADE_CORRESP", SqlDbType.VarChar).Value = cadCredenciado.LOCALIDADE_CORRESP;
                cmd.Parameters.Add("@UF_CORRESP", SqlDbType.VarChar).Value = cadCredenciado.UF_CORRESP;
                cmd.Parameters.Add("@CEP_CORRESP", SqlDbType.VarChar).Value = cadCredenciado.CEP_CORRESP;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mensagem = Convert.ToString(reader["RETORNO"]);
                    if (mensagem == "0")
                    {
                        mensagem = "Registro incluído com sucesso.";
                        idCredenciado = Convert.ToInt32(reader["ID_CREDENCIADO"]);
                        return idCredenciado;
                    }
                    else
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return idCredenciado;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool Inserir(VCREDENCIADO credenciado, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_INSERE_CREDEN_CTRL", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@CODIGO", SqlDbType.Int).Value = credenciado.CODCRE;
                cmd.Parameters.Add("@NOMEXIBICAO", SqlDbType.VarChar).Value = credenciado.NOMEXIBICAO;
                cmd.Parameters.Add("@ID_CREDENCIADO", SqlDbType.Int).Value = credenciado.ID_CREDENCIADO;
                cmd.Parameters.Add("@CNPJ_CPF", SqlDbType.VarChar).Value = credenciado.CNPJ_CPF;
                cmd.Parameters.Add("@SEGMENTO", SqlDbType.VarChar).Value = credenciado.SEGMENTO;
                cmd.Parameters.Add("@RAMO_ATIVIDADE", SqlDbType.VarChar).Value = credenciado.RAMO_ATIVIDADE;
                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = credenciado.TIPO;
                cmd.Parameters.Add("@CODCRE_MATRIZ", SqlDbType.Int).Value = credenciado.CODMAT.CODCRE;
                cmd.Parameters.Add("@QUANT_FILIAIS", SqlDbType.Int).Value = credenciado.QUANT_FILIAIS;
                cmd.Parameters.Add("@CODCEN", SqlDbType.Int).Value = credenciado.CODCEN.CODCRE;
                cmd.Parameters.Add("@MENS_ATIV_CARTAO", SqlDbType.Char).Value = credenciado.MENS_ATIV_CARTAO;
                cmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = credenciado.STATUS;
                cmd.Parameters.Add("@LOCAL_PAGTO", SqlDbType.VarChar).Value = credenciado.LOCAL_PAGTO;
                cmd.Parameters.Add("@TIPO_REEMBOLSO", SqlDbType.VarChar).Value = credenciado.TIPO_REEMBOLSO;
                cmd.Parameters.Add("@FILIAL_REDE", SqlDbType.VarChar).Value = credenciado.FILIAL_REDE;
                cmd.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = credenciado.SENHA;
                cmd.Parameters.Add("@CANAL", SqlDbType.VarChar).Value = credenciado.CANAL;
                cmd.Parameters.Add("@REGIONAL", SqlDbType.VarChar).Value = credenciado.REGIONAL;
                cmd.Parameters.Add("@CREDENCIADOR", SqlDbType.VarChar).Value = credenciado.EPS;
                cmd.Parameters.Add("@TRANSHAB", SqlDbType.VarChar).Value = credenciado.TRANSHAB;
                
                //Dados controle PJ
                cmd.Parameters.Add("@FLAG_CAD_PJ", SqlDbType.Char).Value = credenciado.FLAG_CAD_PJ;

                if (credenciado.FLAG_CAD_PJ == 'S')
                {
                    cmd.Parameters.Add("@TIPO_FECHAMENTO_PJ", SqlDbType.VarChar).Value = credenciado.TIPO_FECHAMENTO_PJ_Text;
                    cmd.Parameters.Add("@DIA_FECH_PJ", SqlDbType.VarChar).Value = credenciado.TIPO_FECHAMENTO_PJ_Text == "SEMANAL" ? credenciado.DIA_FECH_PJ_Text : Convert.ToString(credenciado.DIA_FECH_PJ);
                    cmd.Parameters.Add("@PRAZO_PGTO", SqlDbType.Int).Value = credenciado.PRAZO_PGTO;
                    if (credenciado.DATA_CONTRATO_PJ != DateTime.MinValue) cmd.Parameters.Add("@DATA_CONTRATO_PJ", SqlDbType.DateTime).Value = credenciado.DATA_CONTRATO_PJ;
                    cmd.Parameters.Add("@CONTA_PJ", SqlDbType.VarChar).Value = credenciado.CONTA_PJ;
                    cmd.Parameters.Add("@TAXA_SERV", SqlDbType.Decimal).Value = credenciado.TAXA_SERV;
                    if (credenciado.DATA_TAXA_SERV != DateTime.MinValue) cmd.Parameters.Add("@DATA_TAXA_SERV", SqlDbType.DateTime).Value = credenciado.DATA_TAXA_SERV;
                    cmd.Parameters.Add("@MAXPARCPS", SqlDbType.Int).Value = credenciado.MAXPARC;
                }                
                
                //Dados controle VA
                cmd.Parameters.Add("@FLAG_CAD_VA", SqlDbType.Char).Value = credenciado.FLAG_CAD_VA;
                if (credenciado.FLAG_CAD_VA == 'S')
                {
                    cmd.Parameters.Add("@TIPO_FECHAMENTO_PP", SqlDbType.VarChar).Value = credenciado.TIPO_FECHAMENTO_PP_Text;
                    cmd.Parameters.Add("@DIA_FECH_PP", SqlDbType.VarChar).Value = credenciado.TIPO_FECHAMENTO_PP_Text == "SEMANAL" ? credenciado.DIA_FECH_PP_Text : Convert.ToString(credenciado.DIA_FECH_PP);
                    cmd.Parameters.Add("@PRAZO_REEMBOLSO", SqlDbType.Int).Value = credenciado.PRAZO_REE_VA;
                    if (credenciado.DATA_CONTRATO_VA != DateTime.MinValue) cmd.Parameters.Add("@DATA_CONTRATO", SqlDbType.DateTime).Value = credenciado.DATA_CONTRATO_VA;
                    cmd.Parameters.Add("@CONTA", SqlDbType.VarChar).Value = credenciado.CONTA_VA;
                    cmd.Parameters.Add("@TAXA_ADM_PP", SqlDbType.Decimal).Value = credenciado.TAXA_ADM_PP;
                    if (credenciado.DATA_TAXA_ADM != DateTime.MinValue) cmd.Parameters.Add("@DATA_TAXA_ADM", SqlDbType.DateTime).Value = credenciado.DATA_TAXA_ADM;
                }
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
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool Alterar(CADCREDENCIADO cadCredenciado, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ALTERA_CREDENCIADO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();                

                cmd.Parameters.Add("@ID_CREDENCIADO", SqlDbType.Int).Value = cadCredenciado.ID_CREDENCIADO;
                cmd.Parameters.Add("@CNPJ_CPF", SqlDbType.VarChar).Value = cadCredenciado.CNPJ_CPF;
                cmd.Parameters.Add("@RAZAO", SqlDbType.VarChar).Value = cadCredenciado.RAZAO;
                cmd.Parameters.Add("@FANTASIA", SqlDbType.VarChar).Value = cadCredenciado.FANTASIA;
                cmd.Parameters.Add("@INSC_ESTADUAL", SqlDbType.VarChar).Value = cadCredenciado.INSC_ESTADUAL;
                cmd.Parameters.Add("@REGIAO", SqlDbType.VarChar).Value = cadCredenciado.REGIAO;
                cmd.Parameters.Add("@ENDERECO", SqlDbType.VarChar).Value = cadCredenciado.ENDERECO;
                cmd.Parameters.Add("@COMP", SqlDbType.VarChar).Value = cadCredenciado.COMP;
                cmd.Parameters.Add("@BAIRRO", SqlDbType.VarChar).Value = cadCredenciado.BAIRRO;
                cmd.Parameters.Add("@LOCALIDADE", SqlDbType.VarChar).Value = cadCredenciado.LOCALIDADE;
                cmd.Parameters.Add("@UF", SqlDbType.VarChar).Value = cadCredenciado.UF;
                cmd.Parameters.Add("@CEP", SqlDbType.VarChar).Value = cadCredenciado.CEP;
                cmd.Parameters.Add("@TELEFONE", SqlDbType.VarChar).Value = cadCredenciado.TELEFONE;
                cmd.Parameters.Add("@FAX", SqlDbType.VarChar).Value = cadCredenciado.FAX;
                cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = cadCredenciado.EMAIL;
                //cmd.Parameters.Add("@CONTATO", SqlDbType.VarChar).Value = cadCredenciado.CONTATO;
                cmd.Parameters.Add("@ENDERECO_CORRESP", SqlDbType.VarChar).Value = cadCredenciado.ENDERECO_CORRESP;
                cmd.Parameters.Add("@COMP_CORRESP", SqlDbType.VarChar).Value = cadCredenciado.COMP_CORRESP;
                cmd.Parameters.Add("@BAIRRO_CORRESP", SqlDbType.VarChar).Value = cadCredenciado.BAIRRO_CORRESP;
                cmd.Parameters.Add("@LOCALIDADE_CORRESP", SqlDbType.VarChar).Value = cadCredenciado.LOCALIDADE_CORRESP;
                cmd.Parameters.Add("@UF_CORRESP", SqlDbType.VarChar).Value = cadCredenciado.UF_CORRESP;
                cmd.Parameters.Add("@CEP_CORRESP", SqlDbType.VarChar).Value = cadCredenciado.CEP_CORRESP;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mensagem = Convert.ToString(reader["RETORNO"]);
                    if (mensagem == "0")
                    {
                        mensagem = "Registro alterado com sucesso.";
                        return true;
                    }
                    else
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }            
        }

        public bool Alterar(VCREDENCIADO credenciado, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ALTERA_CREDEN_CTRL", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@CODIGO", SqlDbType.Int).Value = credenciado.CODCRE;
                cmd.Parameters.Add("@NOMEXIBICAO", SqlDbType.VarChar).Value = credenciado.NOMEXIBICAO;
                cmd.Parameters.Add("@ID_CREDENCIADO", SqlDbType.Int).Value = credenciado.ID_CREDENCIADO;
                cmd.Parameters.Add("@SEGMENTO", SqlDbType.VarChar).Value = credenciado.SEGMENTO;
                cmd.Parameters.Add("@RAMO_ATIVIDADE", SqlDbType.VarChar).Value = credenciado.RAMO_ATIVIDADE;
                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = credenciado.TIPO;
                cmd.Parameters.Add("@CODCRE_MATRIZ", SqlDbType.Int).Value = credenciado.CODMAT.CODCRE;
                cmd.Parameters.Add("@QUANT_FILIAIS", SqlDbType.Int).Value = credenciado.QUANT_FILIAIS;
                cmd.Parameters.Add("@CODCEN", SqlDbType.Int).Value = credenciado.CODCEN.CODCRE;
                cmd.Parameters.Add("@MENS_ATIV_CARTAO", SqlDbType.Char).Value = credenciado.MENS_ATIV_CARTAO;
                cmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = credenciado.STATUS;
                cmd.Parameters.Add("@DATSTA", SqlDbType.DateTime).Value = credenciado.DATSTA;
                cmd.Parameters.Add("@LOCAL_PAGTO", SqlDbType.VarChar).Value = credenciado.LOCAL_PAGTO;
                cmd.Parameters.Add("@TIPO_REEMBOLSO", SqlDbType.VarChar).Value = credenciado.TIPO_REEMBOLSO;
                cmd.Parameters.Add("@FILIAL_REDE", SqlDbType.VarChar).Value = credenciado.FILIAL_REDE;
                cmd.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = credenciado.SENHA;
                cmd.Parameters.Add("@CANAL", SqlDbType.VarChar).Value = credenciado.CANAL;
                cmd.Parameters.Add("@REGIONAL", SqlDbType.VarChar).Value = credenciado.REGIONAL;
                cmd.Parameters.Add("@CREDENCIADOR", SqlDbType.VarChar).Value = credenciado.EPS;
                cmd.Parameters.Add("@TRANSHAB", SqlDbType.VarChar).Value = credenciado.TRANSHAB;

                //Dados controle PJ
                cmd.Parameters.Add("@FLAG_CAD_PJ", SqlDbType.Char).Value = credenciado.FLAG_CAD_PJ;

                if (credenciado.FLAG_CAD_PJ == 'S')
                {
                    cmd.Parameters.Add("@TIPO_FECHAMENTO_PJ", SqlDbType.VarChar).Value = credenciado.TIPO_FECHAMENTO_PJ_Text;
                    cmd.Parameters.Add("@DIA_FECH_PJ", SqlDbType.VarChar).Value = credenciado.TIPO_FECHAMENTO_PJ_Text == "SEMANAL" ? credenciado.DIA_FECH_PJ_Text : Convert.ToString(credenciado.DIA_FECH_PJ);
                    cmd.Parameters.Add("@PRAZO_PGTO", SqlDbType.Int).Value = credenciado.PRAZO_PGTO;
                    if (credenciado.DATA_CONTRATO_PJ != DateTime.MinValue) cmd.Parameters.Add("@DATA_CONTRATO_PJ", SqlDbType.DateTime).Value = credenciado.DATA_CONTRATO_PJ;
                    cmd.Parameters.Add("@CONTA_PJ", SqlDbType.VarChar).Value = credenciado.CONTA_PJ;
                    cmd.Parameters.Add("@TAXA_SERV", SqlDbType.Decimal).Value = credenciado.TAXA_SERV;
                    if (credenciado.DATA_TAXA_SERV != DateTime.MinValue) cmd.Parameters.Add("@DATA_TAXA_SERV", SqlDbType.DateTime).Value = credenciado.DATA_TAXA_SERV;
                    cmd.Parameters.Add("@MAXPARCPS", SqlDbType.Int).Value = credenciado.MAXPARC;
                }

                //Dados controle VA
                cmd.Parameters.Add("@FLAG_CAD_VA", SqlDbType.Char).Value = credenciado.FLAG_CAD_VA;
                if (credenciado.FLAG_CAD_VA == 'S')
                {
                    cmd.Parameters.Add("@TIPO_FECHAMENTO_PP", SqlDbType.VarChar).Value = credenciado.TIPO_FECHAMENTO_PP_Text;
                    cmd.Parameters.Add("@DIA_FECH_PP", SqlDbType.VarChar).Value = credenciado.TIPO_FECHAMENTO_PP_Text == "SEMANAL" ? credenciado.DIA_FECH_PP_Text : Convert.ToString(credenciado.DIA_FECH_PP);
                    cmd.Parameters.Add("@PRAZO_REEMBOLSO", SqlDbType.Int).Value = credenciado.PRAZO_REE_VA;
                    if (credenciado.DATA_CONTRATO_VA != DateTime.MinValue) cmd.Parameters.Add("@DATA_CONTRATO", SqlDbType.DateTime).Value = credenciado.DATA_CONTRATO_VA;
                    cmd.Parameters.Add("@CONTA", SqlDbType.VarChar).Value = credenciado.CONTA_VA;
                    cmd.Parameters.Add("@TAXA_ADM_PP", SqlDbType.Decimal).Value = credenciado.TAXA_ADM_PP;
                    if (credenciado.DATA_TAXA_ADM != DateTime.MinValue) cmd.Parameters.Add("@DATA_TAXA_ADM", SqlDbType.DateTime).Value = credenciado.DATA_TAXA_ADM;
                }

                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mensagem = Convert.ToString(reader["RETORNO"]);
                    if (mensagem == "0")
                    {
                        mensagem = "Registro alterado com sucesso.";
                        return true;
                    }
                    else
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool AtualizaCredenciadoPreJuncao(int codCre)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ATUALIZA_CREDENCIADO_JUNCAO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CODCRE", SqlDbType.Int).Value = codCre;

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

        public bool ValidarExclusao(VCREDENCIADO Credenciado)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE");
            sql.AppendLine("FROM VCREDENCIADO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODCRE = @CODCRE");
            sql.AppendLine("  AND (STA = @STA)");
            sql.AppendLine("  AND (CONVERT(CHAR(10), DATSTA, 102) <= CONVERT(CHAR(10), GETDATE() - 7, 102))");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado.CODCRE);
            db.AddInParameter(cmd, "STA", DbType.String, ConstantesSIL.StatusCancelado);
            var CodCre = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (CodCre != 0);
        }

        public string CentralizadoraDependente(int IdCredenciado)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT TOP 1 RAZSOC");
            sql.AppendLine("FROM CREDENCIADO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODCEN = @CODCEN");
            sql.AppendLine("  AND CODCRE <> @CODCEN");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCEN", DbType.Int32, IdCredenciado);
            return Convert.ToString(db.ExecuteScalar(cmd));
        }

        public string MatrizDependente(int IdCredenciado)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT TOP 1 RAZSOC");
            sql.AppendLine("FROM CREDENCIADO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODMAT = @CODMAT");
            sql.AppendLine("  AND CODCRE <> @CODMAT");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODMAT", DbType.Int32, IdCredenciado);
            return Convert.ToString(db.ExecuteScalar(cmd));
        }

        public bool Excluir(VCREDENCIADO Credenciado)
        {
            //Database db = new SqlDatabase(BDTELENET);
            //var dbc = db.CreateConnection();
            //DbCommand cmd;
            //if (Credenciado.FLAG_CAD_PJ == ConstantesSIL.FlgSim)
            //{
            //    var sql = string.Format("UPDATE CREDENCIADO SET FLAG_VA = @FLG WHERE CODCRE = @CODCRE");
            //    cmd = db.GetSqlStringCommand(sql);
            //    db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado.CODCRE);
            //    db.AddInParameter(cmd, "FLG", DbType.String, ConstantesSIL.FlgNao);
            //}
            //else
            //{
            //    var sql = string.Format("DELETE CREDENCIADO WHERE CODCRE = @CODCRE");
            //    cmd = db.GetSqlStringCommand(sql);
            //    db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado.CODCRE);
            //}
            //dbc.Open();
            //var dbt = dbc.BeginTransaction();
            //try
            //{
            //    ExcluirObs(db, dbt, Credenciado.CODCRE);
            //    ExcluirDependencias(db, dbt, Credenciado.CODCRE);
            //    db.ExecuteNonQuery(cmd, dbt);

            //    UtilSIL.GravarLog(db, dbt, Credenciado.FLAG_CAD_PJ == ConstantesSIL.FlgSim
            //                          ? "UPDATE CREDENCIADO (Modificou o Flag para PJ)"
            //                          : "DELETE CREDENCIADO", FOperador, cmd);
            //    dbt.Commit();
            //}
            //catch (Exception err)
            //{
            //    dbt.Rollback();
            //    throw new Exception("Erro Camada DAL [Excluir]" + err);
            //}
            //finally
            //{
            //    dbc.Close();
            //}
            //ExcluirAutorizador(Credenciado);
            return true;
        }

        public bool ExcluirCadCredenciado(string cnpj, out string retorno)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            retorno = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_EXCLUI_CREDENCIADO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CNPJ", SqlDbType.VarChar).Value = cnpj;
                cmd.Parameters.Add("@IDFUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retorno = Convert.ToString(reader["RETORNO"]);
                    if (retorno == "0")
                    {
                        retorno = "Cadastro de credenciado excluido com sucesso.";
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

        public bool ExcluirCredenCtrl(int codCre, out string retorno)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            retorno = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_EXCLUI_CREDEN_CTRL", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CODCRE", SqlDbType.Int).Value = codCre;
                cmd.Parameters.Add("@IDFUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retorno = Convert.ToString(reader["RETORNO"]);
                    if (retorno == "0")
                    {
                        retorno = "Credenciado excluido com sucesso.";
                        return true;
                    }
                    else
                    {
                        retorno = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
//            catch(Exception ex) {  }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool ImportarNetcardPj(int Credenciado)
        {
            var sql = new StringBuilder();
            sql.AppendLine("UPDATE CREDENCIADO");
            sql.AppendLine("    SET FLAG_VA = @FLG, ");
            sql.AppendLine("        DIAFEC_VA = DIAFEC, ");
            sql.AppendLine("        CTABCO_VA = CTABCO, ");
            sql.AppendLine("        NUMFEC_VA = NUMFEC, ");
            sql.AppendLine("        DATULTFEC_VA = DATULTFEC, ");
            sql.AppendLine("        TIPFEC_VA = TIPFEC, ");
            sql.AppendLine("        DATCTT_VA = '" + DateTime.Now.ToString("yyyyMMdd") + "', ");
            sql.AppendLine("        PRAREE = PRAPAG, ");
            sql.AppendLine("        TAXADM = TAXSER, ");
            sql.AppendLine("        PRAENT = 5, ");
            sql.AppendLine("        LIMREENT = 600000 ");
            sql.AppendLine("WHERE CODCRE = @CODCRE");
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado);
            db.AddInParameter(cmd, "FLG", DbType.String, ConstantesSIL.FlgSim);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "UPDATE CREDENCIADO (ImportarNETCARDPJ)", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Importacao]" + err);
            }
            finally
            {
                dbc.Close();
            }

            #region AUTORIZADOR
            ImportarAutorizador(Credenciado);
            #endregion

            return true;
        }

        #endregion

        #region Modulo Web

        public bool RenovarAcesso(VCREDENCIADO Credenciado, out string mensagem)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_RENOVA_ACESSO_CREDENCIADO";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            IDataReader idr = null;
            var retorno = string.Empty;

            db.AddInParameter(cmd, "@CODCRE", DbType.Int32, Credenciado.CODCRE);
            db.AddInParameter(cmd, "@ID_FUNC", DbType.Int32, FOperador.ID_FUNC);

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
                    mensagem = "Acesso renovado com sucesso";
                    UtilSIL.GravarLog("UPDATE CREDENCIADO", FOperador, cmd);
                }
                else { mensagem = "Ocorreu um erro ao renovar o acesso."; }
                }
                else
                {
                    mensagem = "Ocorreu um erro ao renovar o acesso.";
                }

                dbt.Commit();
                return true;
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Renovar Acesso]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        public int DiasParaRenovarSenha()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'PRZ_SENHA_INIC'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToInt16(db.ExecuteScalar(cmd));
        }

        public string ValidadeSenha(int CODCRE)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = "SELECT DTEXPSENHA FROM VCREDENCIADO WITH (NOLOCK) WHERE CODCRE = " + CODCRE;
            var cmd = db.GetSqlStringCommand(sql);
            var validade = db.ExecuteScalar(cmd);
            var retorno = "01/01/0001";
            if (validade != DBNull.Value)
                retorno = Convert.ToDateTime(validade).ToShortDateString();
            return retorno;
        }

        #endregion

        #region CRUD Afiliacao

        public void IncluirDadosCred(AFILIACAO dadosCred)
        {            
            const string sql = "INSERE_DADOSCRED";
            Database db = new SqlDatabase(BDCONCENTRADOR);

            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@SIGUF0", DbType.String, dadosCred.SIGUF0);
            db.AddInParameter(cmd, "@RAZSOC", DbType.String, UtilSIL.RemoverAcentos(dadosCred.RAZSOC));
            db.AddInParameter(cmd, "@NOMFAN", DbType.String, UtilSIL.RemoverAcentos(dadosCred.NOMFAN));
            db.AddInParameter(cmd, "@TEL", DbType.String, dadosCred.TEL);
            db.AddInParameter(cmd, "@EMA", DbType.String, dadosCred.EMA);
            db.AddInParameter(cmd, "@FAX", DbType.String, dadosCred.FAX);
            db.AddInParameter(cmd, "@ENDCRE", DbType.String, dadosCred.ENDCRE);
            db.AddInParameter(cmd, "@CEP", DbType.String, dadosCred.CEP);
            db.AddInParameter(cmd, "@INSEST", DbType.String, dadosCred.INSEST);
            db.AddInParameter(cmd, "@CNPJ", DbType.String, dadosCred.CGC);
            var idr = db.ExecuteReader(cmd);
        }

        #endregion

        #region CRUD Observacoes

        public bool InserirObs(int codCre, DateTime data, string obs, Int32 id, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_INSERE_OBSCRED", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@CODCRE", SqlDbType.Int).Value = codCre;
                cmd.Parameters.Add("@DATA", SqlDbType.DateTime).Value = data;
                cmd.Parameters.Add("@OBS", SqlDbType.VarChar).Value = obs;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@RESULTSET", SqlDbType.VarChar).Value = "S";
                
                // Executando o commando e obtendo o resultado
                reader = cmd.ExecuteReader();

                // Exibindo os registros
                if (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        mensagem = "Inclusão de observação realizada com sucesso.";
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

        public bool AlterarObs(Int32 ID, string OBS)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "UPDATE OBSCRED SET OBS = @OBS WHERE ID = @ID";
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "ID", DbType.Int32, ID);
            db.AddInParameter(cmd, "OBS", DbType.String, OBS);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "UPDATE OBSCRED", FOperador, cmd);
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

        public bool ExcluirObs(Int32 id, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_EXCLUI_OBSCRED", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@RESULTSET", SqlDbType.VarChar).Value = "S";

                // Executando o commando e obtendo o resultado
                reader = cmd.ExecuteReader();

                // Exibindo os registros
                if (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        mensagem = "Exclusão de observação realizada com sucesso.";
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

        public bool ExcluirObs(Database db, DbTransaction dbt, int Credenciado)
        {
            const string sql = "DELETE FROM OBSCRED WHERE CODCRE = @CODCRE";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado);
            db.ExecuteNonQuery(cmd, dbt);
            return true;
        }

        public bool ExcluirDependencias(Database db, DbTransaction dbt, int Credenciado)
        {
            var sql = "DELETE FROM FECHCREDVA WHERE CODCRE = @CODCRE";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado);
            db.ExecuteNonQuery(cmd, dbt);
            sql = "DELETE FROM FECHCRED WHERE CODCRE = @CODCRE";
            cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado);
            db.ExecuteNonQuery(cmd, dbt);
            return true;
        }

        #endregion

        #region Credenciado Ja Cadastrado

        public bool CodigoExistente(int codigo)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE");
            sql.AppendLine("FROM CREDENCIADO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODCRE = @CODCRE");
            sql.AppendLine("  AND FLAG_VA = @FLAG_VA");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.String, codigo);
            db.AddInParameter(cmd, "FLAG_VA", DbType.String, ConstantesSIL.FlgSim);
            var CodCre = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (CodCre != 0);
        }

        public bool CredenciadoExistenteNetcardPj(int codigo)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE");
            sql.AppendLine("FROM CREDENCIADO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODCRE = @CODCRE");
            sql.AppendLine("  AND ((FLAG_VA IS NULL) OR (FLAG_VA = @FLAG))");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.String, codigo);
            db.AddInParameter(cmd, "FLAG", DbType.String, ConstantesSIL.FlgNao);
            var CodCre = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (CodCre != 0);
        }

        public bool CredenciadoExistenteNetcardPj(string cnpj, out int codcre)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE");
            sql.AppendLine("FROM CREDENCIADO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CGC = @CGC");
            sql.AppendLine("  AND ((FLAG_VA IS NULL) OR (FLAG_VA = @FLAG))");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CGC", DbType.String, cnpj);
            db.AddInParameter(cmd, "FLAG", DbType.String, ConstantesSIL.FlgNao);
            codcre = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (codcre != 0);
        }

        public bool CredenciadoExistenteDadosCred(string cnpj)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CGC");
            sql.AppendLine("FROM DADOSCRED WITH (NOLOCK) ");
            sql.AppendLine("WHERE CGC = @CNPJ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CNPJ", DbType.String, cnpj);
            var cgc = Convert.ToString(db.ExecuteScalar(cmd));
            return (cgc != string.Empty);
        }

        public bool CnpjExistentePrincipal(string cnpj, out int codcre)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE");
            sql.AppendLine("FROM CREDENCIADO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CGC = @CNPJ");
            sql.AppendLine("  AND FLAG_VA = @FLAG");
            sql.AppendLine("  AND CODCRE = CODPRI");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CNPJ", DbType.String, cnpj);
            db.AddInParameter(cmd, "FLAG", DbType.String, ConstantesSIL.FlgSim);
            codcre = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (codcre != 0);
        }

        public bool ValidarSegmentoPrncipal(string cnpj)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE");
            sql.AppendLine("FROM CREDENCIADO WITH (NOLOCK) ");
            sql.AppendLine("WHERE CGC = @CNPJ");
            sql.AppendLine("  AND FLAG_VA = @FLAG");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CNPJ", DbType.String, cnpj);
            db.AddInParameter(cmd, "FLAG", DbType.String, ConstantesSIL.FlgSim);
            var Codigo = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (Codigo != 0);
        }

        #endregion

        #region CRUD Equipamentos

        private void AtualizarQtdeEquipamento(int qtde, int codCre)
        {
            var db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var cmd = db.GetSqlStringCommand("UPDATE  CREDENCIADO SET QTDPOS = " + qtde + " WHERE CODCRE = " + codCre);
                db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();
            }
            catch
            {
                dbt.Rollback();
                throw new Exception("Erro ao atualizar Qtde Equipamentos");
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool InserirEquipamento(int codOpe, string codAfil, int codcre, string nomExibicao)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            const string sql = "CAD_TERMINAL";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            var codCre = Convert.ToString(codcre).PadLeft(6, '0');
            db.AddInParameter(cmd, "@CODAFIL", DbType.String, codAfil);
            db.AddInParameter(cmd, "@CODOPE", DbType.Int32, codOpe);
            db.AddInParameter(cmd, "@CODPS", DbType.String, codCre);
            db.AddInParameter(cmd, "@NOMEXIBICAO ", DbType.String, nomExibicao);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog("INSERT CTPOS", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Equipamento]" + err);
            }
            finally
            {
                dbc.Close();
            }
            return true;
        }

        public bool InicializarEquipamento(string codEquipamento)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            const string sql = "RESETA_INIC_TERMINAL";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "@CODOPE", DbType.Int32, FOperador.CODOPE);
            db.AddInParameter(cmd, "@TERMINAL", DbType.String, codEquipamento);
            db.AddInParameter(cmd, "@ID_FUNC", DbType.Int32, FOperador.ID_FUNC);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog("RESETA_INIC_TERMINAL", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Equipamento]" + err);
            }
            finally
            {
                dbc.Close();
            }
            return true;
        }

        public bool ExcluirEquipamento(string ID)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var sql = string.Format("DELETE CTPOS WHERE CODPOS = @CODPOS");
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "CODPOS", DbType.String, ID);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                if (LinhaAfetada == 1)
                {
                    UtilSIL.GravarLog("DELETE CTPOS", FOperador, cmd);
                }
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Equipamento]" + err);
            }
            finally
            {
                dbc.Close();
            }
            return true;
        }        

        #endregion

        #region CRUD AUTORIZADOR

        private void InserirAutorizador(CREDENCIADO Credenciado)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                InserirAutorizadorCredenciado(Credenciado, db, dbt);
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

        private void AlterarAutorizador(CREDENCIADO Credenciado)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                if (!ExisteCredenciadoAutorizador(Credenciado, db, dbt))
                    InserirAutorizadorCredenciado(Credenciado, db, dbt);
                else
                    AlterarAutorizadorCredenciado(Credenciado, db, dbt);
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

        private void ImportarAutorizador(int Credenciado)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                ImportarAutorizadorCredenciado(Credenciado, db, dbt);
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

        private void ExcluirAutorizador(CREDENCIADO Credenciado)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                ExcluirAutorizadorCredenciado(Credenciado, db, dbt);
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

        private static bool ExisteCredenciadoAutorizador(CREDENCIADO Credenciado, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODPS FROM CTPREST WITH (NOLOCK) WHERE CODPS = @CODPS";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODPS", DbType.String, Credenciado.CODCRE.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));
            var Codigo = Convert.ToString(db.ExecuteScalar(cmd, dbt));
            return (Codigo != string.Empty);
        }

        private static void InserirAutorizadorCredenciado(CREDENCIADO Credenciado, Database db, DbTransaction dbt)
        {
            const string sql = "INSERT INTO CTPREST " +
                               "(CODPS, NOMEPS, SEGPS, STATPS, DTSTATPS, MSGATIVA, SENHA, UF, FLG_VA)" +
                               "VALUES " +
                               "(@CODPS, @NOMEPS, @SEGPS, @STATPS, @DTSTATPS, @MSGATIVA, @SENHA, @UF, @FLG_VA)";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODPS", DbType.String, Credenciado.CODCRE.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));
            db.AddInParameter(cmd, "NOMEPS", DbType.String, Credenciado.RAZSOC.Length > 30 ? Credenciado.RAZSOC.Substring(0, 30) : Credenciado.RAZSOC);
            db.AddInParameter(cmd, "SEGPS", DbType.String, Credenciado.CODSEG.ToString(CultureInfo.InvariantCulture).PadLeft(5, '0'));
            db.AddInParameter(cmd, "STATPS", DbType.String, Credenciado.STA);
            db.AddInParameter(cmd, "DTSTATPS", DbType.DateTime, Credenciado.DATSTA);
            db.AddInParameter(cmd, "MSGATIVA", DbType.String, Credenciado.MSGATVCRT);
            db.AddInParameter(cmd, "SENHA", DbType.String, Credenciado.SENCRE);
            db.AddInParameter(cmd, "UF", DbType.String, Credenciado.SIGUF0);
            db.AddInParameter(cmd, "FLG_VA", DbType.String, Credenciado.FLAG_VA);
            db.ExecuteNonQuery(cmd, dbt);
        }

        private static void AlterarAutorizadorCredenciado(CREDENCIADO Credenciado, Database db, DbTransaction dbt)
        {
            const string sql = "UPDATE CTPREST SET NOMEPS = @NOMEPS, SEGPS = @SEGPS, STATPS = @STATPS, DTSTATPS = @DTSTATPS, " +
                               "MSGATIVA = @MSGATIVA, SENHA = @SENHA, UF = @UF, FLG_VA = @FLG " +
                               "WHERE CODPS = @CODPS";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODPS", DbType.String, Credenciado.CODCRE.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));
            var NomeCredenciado = Credenciado.RAZSOC;
            if (NomeCredenciado.Length > 30)
                NomeCredenciado = Credenciado.RAZSOC.Substring(0, 30);
            db.AddInParameter(cmd, "NOMEPS", DbType.String, NomeCredenciado);
            db.AddInParameter(cmd, "SEGPS", DbType.String, Credenciado.CODSEG.ToString(CultureInfo.InvariantCulture).PadLeft(5, '0'));
            db.AddInParameter(cmd, "STATPS", DbType.String, Credenciado.STA);
            db.AddInParameter(cmd, "DTSTATPS", DbType.DateTime, Credenciado.DATSTA);
            db.AddInParameter(cmd, "MSGATIVA", DbType.String, Credenciado.MSGATVCRT);
            db.AddInParameter(cmd, "SENHA", DbType.String, Credenciado.SENCRE);
            db.AddInParameter(cmd, "UF", DbType.String, Credenciado.SIGUF0);
            db.AddInParameter(cmd, "FLG", DbType.String, ConstantesSIL.FlgSim);
            db.ExecuteNonQuery(cmd, dbt);
        }

        private static void ImportarAutorizadorCredenciado(int Credenciado, Database db, DbTransaction dbt)
        {
            const string sql = "UPDATE CTPREST SET FLG_VA = @FLG WHERE CODPS = @CODPS";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODPS", DbType.String, Credenciado.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));
            db.AddInParameter(cmd, "FLG", DbType.String, ConstantesSIL.FlgSim);
            db.ExecuteNonQuery(cmd, dbt);
        }

        private static void ExcluirAutorizadorCredenciado(CREDENCIADO Credenciado, Database db, DbTransaction dbt)
        {
            string sql;
            DbCommand cmd;
            if (Credenciado.FLAG_PJ == ConstantesSIL.FlgSim)
            {
                sql = "UPDATE CTPREST SET FLG_VA = @FLAG WHERE CODPS = @CODPS";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODPS", DbType.String, Credenciado.CODCRE.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));
                db.AddInParameter(cmd, "FLAG", DbType.String, ConstantesSIL.FlgNao);
                db.ExecuteNonQuery(cmd, dbt);
            }
            else
            {
                sql = "DELETE CTPREST WHERE CODPS = @CODPS";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODPS", DbType.String, Credenciado.CODCRE.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));
                db.ExecuteNonQuery(cmd, dbt);
            }
        }

        #endregion

        #region CRUD TABRAFIL

        public void InserirTabrafil(int codOpe, string cnpj, string codDestino)
        {            
            const string sql = "INSERE_TABRAFIL";
            Database db = new SqlDatabase(BDCONCENTRADOR);

            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODOPE", DbType.Int16, codOpe);            
            db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
            db.AddInParameter(cmd, "@DESTINO", DbType.String, codDestino);
            db.ExecuteReader(cmd);
        }

        #endregion

        #region CRUD TAXASCRE

        public bool SalvarTaxaCre(MODTAXA taxacre)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_ASSOCIA_TAXACRE";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "@SISTEMA", DbType.Int32, taxacre.SISTEMA);
            db.AddInParameter(cmd, "@CODCRE", DbType.String, taxacre.COD);
            db.AddInParameter(cmd, "@CODTAXA", DbType.String, taxacre.CODTAXA);
            db.AddInParameter(cmd, "@VALOR", DbType.Decimal, taxacre.VALOR);
            db.AddInParameter(cmd, "@NUMPAC", DbType.Int16, taxacre.NUMPARC);
            db.AddInParameter(cmd, "@DTINICIO", DbType.DateTime, taxacre.DTINICIO);
            db.AddInParameter(cmd, "@DIACOB", DbType.Int32, taxacre.DIACOB);
            db.AddInParameter(cmd, "@DIASPINICIO", DbType.Int16, taxacre.DIASPINICIO);
            db.AddInParameter(cmd, "@COBSEMCRED", DbType.String, taxacre.COBSEMCRED);
            db.AddInParameter(cmd, "@PRIORIDADE", DbType.Int16, taxacre.PRIORIDADE);
            db.AddInParameter(cmd, "@TAXAHAB", DbType.String, taxacre.TAXAHAB);
            db.AddInParameter(cmd, "@VALMINCRED", DbType.Decimal, taxacre.VALMINCRED);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {                               
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "PROC_ASSOCIA_TAXACRE", FOperador, cmd);
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

        public bool ImportarTaxasPj(int codCre)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_IMPORTA_TAXAPJ";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "@CODCRE", DbType.Int32, codCre);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "PROC_IMPORTA_TAXAPJ", FOperador, cmd);
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

        public bool ExcluirTaxaCre(MODTAXA taxacre)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            var sql = string.Format("DELETE {0} WHERE CODCRE = @CODCRE AND CODTAXA = @CODTAXA", taxacre.SISTEMA == 0 ? "TAXACRE " : "TAXACREVA ");
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, taxacre.COD);
            db.AddInParameter(cmd, "CODTAXA", DbType.Int32, taxacre.CODTAXA);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "DELETE TAXACREVA", FOperador, cmd);
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
            return true;
        }

        public int ValidarExclusaoTaxaCre(MODTAXA taxacre)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT COUNT(DATTRA) AS NUMTRANS FROM ");
            sql.AppendLine(taxacre.SISTEMA == 0 ? "TRANSACAO WITH (NOLOCK) " : "TRANSACVA WITH (NOLOCK) ");
            sql.AppendLine("WHERE TIPTRA = @TIPTRA ");
            sql.AppendLine("AND CODCRE = @CODCRE");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, taxacre.COD);
            db.AddInParameter(cmd, "TIPTRA", DbType.Int32, taxacre.TIPTRA);
            return (int)(db.ExecuteScalar(cmd));
        }

        public string ValidarNumPac(int sistema, int codTaxa)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT TRENOVA FROM VTAXA WITH (NOLOCK) ");
            sql.AppendLine("WHERE SISTEMA = @SISTEMA AND CODTAXA = @CODTAXA");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, sistema);
            db.AddInParameter(cmd, "CODTAXA", DbType.Int32, codTaxa);
            var idr = db.ExecuteReader(cmd);
            var renovacao = string.Empty;
            while (idr.Read())
            {
                renovacao = Convert.ToString(idr["TRENOVA"]);
            }
            idr.Close();
            return renovacao;
        }

        #endregion

        #region CRUD HABREDES

        public string SalvarHabRedes(REDES rede, string cnpj, bool excluir)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_HABILITA_REDE";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            IDataReader idr = null;
            var retorno = string.Empty;
            var mensagem = string.Empty;

            db.AddInParameter(cmd, "@CODCRE", DbType.String, rede.CODCRE);
            db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
            db.AddInParameter(cmd, "@REDE", DbType.String, rede.REDE);
            db.AddInParameter(cmd, "@HABREDE", DbType.String, excluir ? "E" : rede.HABREDE);
            db.AddInParameter(cmd, "@HABPOS", DbType.String, "S");
            db.AddInParameter(cmd, "@HABTEF", DbType.String, "N");
            db.AddInParameter(cmd, "@CODAFILREDE", DbType.String, rede.CODAFILREDE);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                idr = db.ExecuteReader(cmd, dbt);
                if (idr.Read())
                {
                    retorno = Convert.ToString(idr["RETORNO"]);
                    mensagem = Convert.ToString(idr["MENSAGEM"]);
                    idr.Close();
                    if (retorno == "OK")
                        UtilSIL.GravarLog(db, dbt, "PROC_HABILITA_REDE ", FOperador, cmd);
                }
                if (idr != null) idr.Close();
                dbt.Commit();
                return mensagem;
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

        public string EqualizarHabRedes(REDES rede, string cnpj, bool excluir)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_EQUALIZA_HAB_REDE";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            IDataReader idr = null;
            var retorno = string.Empty;
            var mensagem = string.Empty;

            db.AddInParameter(cmd, "@CODCRE", DbType.String, rede.CODCRE);
            db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
            db.AddInParameter(cmd, "@REDE", DbType.String, rede.REDE);
            db.AddInParameter(cmd, "@HABREDE", DbType.String, excluir ? "E" : rede.HABREDE);
            db.AddInParameter(cmd, "@HABPOS", DbType.String, "S");
            db.AddInParameter(cmd, "@HABTEF", DbType.String, "N");
            db.AddInParameter(cmd, "@CODAFILREDE", DbType.String, rede.CODAFILREDE);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                idr = db.ExecuteReader(cmd, dbt);
                while (idr.Read())
                {
                    mensagem += Convert.ToString(idr["MENSAGEM"]) + ";";
                    retorno = Convert.ToString(idr["RETORNO"]);
                }
                idr.Close();
                if (retorno == "OK")
                {
                    UtilSIL.GravarLog(db, dbt, "PROC_EQUALIZA_HAB_REDE ", FOperador, cmd);
                }
                dbt.Commit();
                return mensagem;
            }
            catch (Exception err)
            {
                if (idr != null)
                    idr.Close();
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        #endregion

        #region CRUD Especialidades Ativas

        public bool InserirEspAtiva(int codCre, int codEsp)
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
                sql = "INSERT INTO ESPECRED (CODCRE, CODESP) " +
                      "SELECT @CODCRE, @CODESP " +
                      " WHERE NOT EXISTS " +
                      "(SELECT CODESP FROM ESPECRED WITH (NOLOCK) WHERE CODCRE = @CODCRE AND CODESP = @CODESP)";

                cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);
                db.AddInParameter(cmd, "CODESP", DbType.Int32, codEsp);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "INSERT ESPECRED", FOperador, cmd);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Especialidades]" + err);
            }
            finally
            {
                dbc.Close();

            }

            // Sucesso
            return true;

        }

        // Inserir Todos Especialidades Disponiveis
        public bool InserirEspAtiva(int codCre)
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
                sql.AppendLine("INSERT INTO ESPECRED (CODCRE, CODESP) ");
                sql.AppendLine("SELECT @CODCRE, E.CODESP");
                sql.AppendLine("FROM ESPECIALIDADE E WITH (NOLOCK) ");
                sql.AppendLine("WHERE NOT EXISTS");
                sql.AppendLine("(SELECT EA.CODESP");
                sql.AppendLine("  FROM ESPECRED EA WITH (NOLOCK) ");
                sql.AppendLine(" WHERE EA.CODESP = E.CODESP");
                sql.AppendLine("   AND EA.CODCRE = @CODCRE)");

                cmd = db.GetSqlStringCommand(sql.ToString());
                db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);
                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "INSERT ESPECRED", FOperador, cmd);
                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Especialidades]" + err);
            }
            finally
            {
                dbc.Close();

            }

            // Sucesso
            return true;

        }

        public bool ExcluirEspAtiva(int codCre, int codEsp)
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
                sql = "DELETE FROM ESPECRED WITH (NOLOCK) WHERE CODCRE = @CODCRE AND CODESP = @CODESP";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);
                db.AddInParameter(cmd, "CODESP", DbType.Int32, codEsp);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "DELETE ESPECRED", FOperador, cmd);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Especialidade]" + err);
            }
            finally
            {
                dbc.Close();
            }

            // Sucesso
            return true;
        }

        // Excluir Todos Segmentos Disponiveis
        public bool ExcluirEspAtiva(int codCre)
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
                sql = "DELETE FROM ESPECRED WITH (NOLOCK) WHERE CODCRE = @CODCRE";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "DELETE ESPECRED", FOperador, cmd);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Colecao Especialidades]" + err);
            }
            finally
            {
                dbc.Close();

            }

            // Sucesso
            return true;
        }

        #endregion

        #region GET Especialidades Ativadas

        public List<ESPATIVADA_CREDENCIADO> EspAtivas(int codCre)
        {
            var ColecaoEspecAtivadas = new List<ESPATIVADA_CREDENCIADO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("  E.CODESP, E.NOMESP");
            sql.AppendLine("FROM ESPECIALIDADE E WITH (NOLOCK) ");
            sql.AppendLine("JOIN ESPECRED EA WITH (NOLOCK) ");
            sql.AppendLine("  ON EA.CODESP = E.CODESP");
            sql.AppendLine(" AND EA.CODCRE = @CODCRE");
            sql.AppendLine("ORDER BY E.NOMESP");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.String, codCre);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var EspecAtivada = new ESPATIVADA_CREDENCIADO();
                EspecAtivada.CODCRE = codCre;
                EspecAtivada.CODESP = Convert.ToInt32(idr["CODESP"]);
                EspecAtivada.NOMESP = Convert.ToString(idr["NOMESP"]).Trim();
                ColecaoEspecAtivadas.Add(EspecAtivada);
            }
            idr.Close();
            return ColecaoEspecAtivadas;
        }

        public List<ESPATIVADA_CREDENCIADO> EspDisponiveis(int codCre)
        {
            var ColecaoEspAtivas = new List<ESPATIVADA_CREDENCIADO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("  E.CODESP, E.NOMESP");
            sql.AppendLine("FROM ESPECIALIDADE E WITH (NOLOCK) ");
            sql.AppendLine("WHERE NOT EXISTS");
            sql.AppendLine("(SELECT EA.CODESP");
            sql.AppendLine("  FROM ESPECRED EA WITH (NOLOCK) ");
            sql.AppendLine(" WHERE EA.CODESP = E.CODESP");
            sql.AppendLine("   AND EA.CODCRE = @CODCRE)");
            sql.AppendLine("ORDER BY E.NOMESP");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.String, codCre);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var espAtivas = new ESPATIVADA_CREDENCIADO();
                espAtivas.CODCRE = codCre;
                espAtivas.CODESP = Convert.ToInt32(idr["CODESP"]);
                espAtivas.NOMESP = Convert.ToString(idr["NOMESP"]).Trim();

                ColecaoEspAtivas.Add(espAtivas);
            }
            idr.Close();

            return ColecaoEspAtivas;
        }

        #endregion

        #region  LOG

        private void GeraLogTrans(DbTransaction dbt, Database db, CREDENCIADO cre, int tiptra)
        {
            var cmd = db.GetStoredProcCommand("PROC_GRAVAR_TRANSACAO");
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, DBNull.Value);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, cre.CODCRE);
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

        #endregion        

        #region Grupos Credenciados

        internal List<GRUPO> ColecaoGrupos(string filtro)
        {
            var colecao = new List<GRUPO>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT CODGRUPO, NOMGRUPO, SISTEMA ");
            sql.AppendLine(" FROM GRUPO WITH (NOLOCK) ");
            sql.AppendLine(" WHERE SISTEMA <> 'PJ' ");
            if (!string.IsNullOrEmpty(filtro))
                sql.AppendLine(string.Format(" AND {0} ", filtro));
            sql.AppendLine(" ORDER BY NOMGRUPO ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var grupo = new GRUPO();
                grupo.CODGRUPO = Convert.ToInt32(idr["CODGRUPO"]);
                grupo.NOMGRUPO = Convert.ToString(idr["NOMGRUPO"]);
                grupo.SISTEMA = Convert.ToString(idr["SISTEMA"]);
                colecao.Add(grupo);
            }
            idr.Close();
            return colecao;
        }

        internal List<GRUPO> ColecaoGrupos(int codCre)
        {
            var colecao = new List<GRUPO>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT G.CODGRUPO, NOMGRUPO, SISTEMA ");
            sql.AppendLine(" FROM GRUPO G WITH (NOLOCK) ");
            sql.AppendLine(" INNER JOIN GRUPOCRED GC WITH (NOLOCK) ON G.CODGRUPO = GC.CODGRUPO ");
            sql.AppendLine(" WHERE GC.CODCRE = @CODCRE ");            
            sql.AppendLine(" ORDER BY G.NOMGRUPO ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);

            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var grupo = new GRUPO();
                grupo.CODGRUPO = Convert.ToInt32(idr["CODGRUPO"]);
                grupo.NOMGRUPO = Convert.ToString(idr["NOMGRUPO"]);
                grupo.SISTEMA = Convert.ToString(idr["SISTEMA"]);
                colecao.Add(grupo);
            }
            idr.Close();
            return colecao;
        }

        internal List<GRUPOCREDENCIADO> ColecaoCredenciadosPorGrupo(int idGrupo)
        {
            var colecao = new List<GRUPOCREDENCIADO>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT G.CODGRUPO, G.CODCRE, C.NOMFAN, C.RAZSOC ");
            sql.AppendLine(" FROM GRUPOCRED G WITH (NOLOCK) INNER JOIN CREDENCIADO C WITH (NOLOCK) ");
            sql.AppendLine(" ON G.CODCRE = C.CODCRE  ");
            sql.AppendLine(string.Format(" WHERE G.CODGRUPO = {0} ", idGrupo));
            sql.AppendLine(" ORDER BY C.NOMFAN ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var grupoCred = new GRUPOCREDENCIADO();
                grupoCred.CODGRUPO = Convert.ToInt32(idr["CODGRUPO"]);
                grupoCred.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                grupoCred.NOMFAN = Convert.ToString(idr["NOMFAN"]);
                grupoCred.RAZSOC = Convert.ToString(idr["RAZSOC"]);

                colecao.Add(grupoCred);
            }
            idr.Close();
            return colecao;
        }

        internal void ExcluirCredenciadosGrupo(int idGrupo, List<object> listaCredenciados)
        {
            try
            {
                ExcluirCredenciadosGrupoNetcard(idGrupo, listaCredenciados);
                ExcluirCredenciadosAutorizador(idGrupo, listaCredenciados);
            }
            catch
            {
                throw new Exception("Erro ao excluir os credenciados do grupo. Repita a operacao");
            }           
        }

        internal void ExcluirCredenciadosAutorizador(int idGrupo, List<object> listaCredenciados)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var lista = listaCredenciados.Aggregate(string.Empty,(current, credenciado) => current + (credenciado + ","));
            lista = lista.Remove(lista.Length - 1, 1);
            sql.AppendLine(" DELETE CTGRUPOCRED ");
            sql.AppendLine(string.Format(" WHERE CODGRUPO = {0} ", idGrupo));
            sql.AppendLine(string.Format(" AND CODPS IN ( {0} )", lista));
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteNonQuery(cmd);
        }

        internal void ExcluirCredenciadosGrupoNetcard(int idGrupo, List<object> listaCredenciados)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            var lista = listaCredenciados.Aggregate(string.Empty, (current, credenciado) => current + (credenciado + ","));
            lista = lista.Remove(lista.Length - 1, 1);
            sql.AppendLine(" DELETE GRUPOCRED ");
            sql.AppendLine(string.Format(" WHERE CODGRUPO = {0} ", idGrupo));
            sql.AppendLine(string.Format(" AND CODCRE IN ( {0} )", lista));
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteNonQuery(cmd);
        }

        internal bool VerificaCodigoExistente(int codGrupo)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT CODGRUPO ");
            sql.AppendLine(" FROM GRUPO WITH (NOLOCK) ");
            sql.AppendLine(string.Format(" WHERE CODGRUPO = {0} ", codGrupo));
            var codigo = 0;
            var cmd = db.GetSqlStringCommand(sql.ToString());
            try
            {
                var idr = db.ExecuteReader(cmd);
                while (idr.Read())
                    codigo = Convert.ToInt32(idr["CODGRUPO"]);
                idr.Close();
            }
            catch
            {
                throw new Exception("Erro ao verificar a existencia do codigo do grupo inserido. Repita a operacao");
            }
            return codigo > 0;
        }

        internal bool SalvarGrupo(int codGrupo, string nome)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" INSERT GRUPO (CODGRUPO, NOMGRUPO, SISTEMA ) ");
            sql.AppendLine(" VALUES ( @CODGRUPO, @NOMGRUPO,  @SISTEMA) ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODGRUPO", DbType.Int32, codGrupo);
            db.AddInParameter(cmd, "NOMGRUPO", DbType.String, nome);
            db.AddInParameter(cmd, "SISTEMA", DbType.String, "VA");
            try
            {
                db.ExecuteNonQuery(cmd);
            }
            catch
            {
                return false;
            }
            return true;
        }

        internal void ExcluirGrupo(int codGrupo)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                ExcluirCredenciadosdoGrupo(db, dbt, codGrupo);
                dbt.Commit();
                ExcluirFinalGrupo(codGrupo);
                ExcluirCredenciadosEGrupoAutorizador(codGrupo);
            }
            catch (Exception ex)
            {
                if (dbt.Connection != null)
                    dbt.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        private static void ExcluirCredenciadosdoGrupo(Database db, DbTransaction dbt, int codGrupo)
        {
            try
            {
                var sql = "DELETE FROM SEGAUTORIZVA WHERE codgrupo = @codgrupo";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "codgrupo", DbType.Int32, codGrupo);
                db.ExecuteNonQuery(cmd, dbt);
                sql = "DELETE FROM grupocred WHERE codgrupo = @codgrupo";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "codgrupo", DbType.Int32, codGrupo);
                db.ExecuteNonQuery(cmd, dbt);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao excluir os credenciados do grupo.");
            }
        }

        private void ExcluirFinalGrupo(int codGrupo)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = "DELETE FROM GRUPO WHERE codgrupo = " + codGrupo;
            var cmd = db.GetSqlStringCommand(sql);
            try
            {
                db.ExecuteNonQuery(cmd);
            }
            catch
            {
                throw new Exception("Somente os credenciados do grupo foram excluidos. Este Grupo nao sera excluido pois o mesmo pode estar sendo usado no sistema PJ.");
            }
        }

        private void ExcluirCredenciadosEGrupoAutorizador(int codGrupo)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var sql = "DELETE FROM CTGRUPOCRED WHERE codgrupo = @codgrupo";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "codgrupo", DbType.Int32, codGrupo);
                db.ExecuteNonQuery(cmd, dbt);
                sql = "DELETE FROM TabSegVA WHERE codgrupo = @codgrupo";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "codgrupo", DbType.Int32, codGrupo);
                db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();
            }
            catch
            {
                dbt.Rollback();
                throw new Exception("Erro ao excluir o grupo no Autorizador.");
            }
            finally
            {
                dbc.Close();
            }
        }

        internal List<CREDENCIADO_VA> ColecaoCredenciadosForaDoGrupo(string sql)
        {
            var colecao = new List<CREDENCIADO_VA>();
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand(sql);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var cred = new CREDENCIADO_VA();
                cred.CODCRE = Convert.ToString(idr["CODCRE"]);
                cred.NOMFAN = Convert.ToString(idr["NOMFAN"]);
                cred.RAZSOC = Convert.ToString(idr["RAZSOC"]);
                colecao.Add(cred);
            }
            idr.Close();
            return colecao;
        }

        internal void IncluirCredenciadosGrupo(int idGrupo, List<object> listaCredenciados, out string mensagem)
        {
            try
            {
                IncluirCredenciadosGrupoNetcard(idGrupo, listaCredenciados, out mensagem);
                //IncluirCredenciadosGrupoAutorizador(idGrupo, listaCredenciados);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar os credenciados ao grupo. Repita a operacao");
            }
        }

        private void IncluirCredenciadosGrupoNetcard(int idGrupo, IEnumerable<object> listaCredenciados, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;
            string retProc = string.Empty;
            string RetCredenciado = string.Empty;

            try
            {

                foreach (var credenciado in listaCredenciados)
                {
                    conn = new SqlConnection(BDTELENET);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("PROC_INCLUI_CRED_GRUPO", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.Clear();

                    cmd.Parameters.Add("@CODGRUPO", SqlDbType.VarChar).Value = idGrupo;
                    cmd.Parameters.Add("@CODCRE", SqlDbType.VarChar).Value = Convert.ToString(credenciado).PadLeft(6, '0');

                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        retProc = Convert.ToString(reader["RETORNO"]);
                        if (retProc != "OK")
                        {
                            RetCredenciado += credenciado + ", ";
                        }
                    }

                    if (reader != null) { reader.Close(); }
                    if (conn != null) { conn.Close(); }
                }

                if (RetCredenciado != "")
                {
                    RetCredenciado = RetCredenciado.Trim();
                    var cred = RetCredenciado.Remove(RetCredenciado.Length - 1);
                    if (!cred.Contains(","))
                    {
                        mensagem = "O Credenciado " + cred + " não pode ser incluído no grupo porque pertence a outro grupo associado para um mesmo cliente.";
                    }
                    else
                    {
                        mensagem = "Os Credenciados " + cred + " não podem ser incluídos no grupo porque pertencem a outro grupo associado para um mesmo cliente.";
                    }
                }
                else
                {
                    mensagem = "Credenciado incluindo com sucesso.";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void IncluirCredenciadosGrupoAutorizador(int idGrupo, IEnumerable<object> listaCredenciados)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDAUTORIZADOR);
            sql.AppendLine(" INSERT INTO CTGRUPOCRED (CODGRUPO, CODPS )");
            foreach (var credenciado in listaCredenciados)
                sql.AppendLine("SELECT " + idGrupo + " , '" + credenciado.ToString().PadLeft(6, '0') + "' UNION ALL ");
            sql.Remove(sql.Length - 10, 10);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteNonQuery(cmd);
        }

        internal string RecuperaGrupobyNome(string nomeGrupo)
        {
            var sql = "SELECT NOMGRUPO FROM GRUPO WITH (NOLOCK) WHERE NOMGRUPO = '" + nomeGrupo + "'";
            Database db = new SqlDatabase(BDTELENET);
            var nomeGrupoRecuperado = string.Empty;
            try
            {
                var cmd = db.GetSqlStringCommand(sql);
                var idr = db.ExecuteReader(cmd);
                while (idr.Read())
                    nomeGrupoRecuperado = Convert.ToString(idr["NOMGRUPO"]);
                idr.Close();
            }
            catch
            {
                return string.Empty;
            }
            return nomeGrupoRecuperado;
        }

        #endregion

        #region 4DatasCred

        public List<_4DATAS> Proc_Ler_4datas_Fechcred(int DIAFEC)
        {
            var colecao4Datas = new List<_4DATAS>();
            const string sql = "PROC_LER_4DATAS_FECHCRED";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@DIAFEC", DbType.Int32, DIAFEC);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var _4Datas = new _4DATAS
                {
                    DIA1 = Convert.ToString(idr["DIA1"]),
                    DIA2 = Convert.ToString(idr["DIA2"]),
                    DIA3 = Convert.ToString(idr["DIA3"]),
                    DIA4 = Convert.ToString(idr["DIA4"])
                };
                colecao4Datas.Add(_4Datas);
            }
            idr.Close();
            return colecao4Datas;
        }

        public int Proc_Gera_4dadas_Fechcred(int DIA1, int DIA2, int DIA3, int DIA4)
        {
           
            const string sql = "PROC_GERA_4DATAS_FECHCRED";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@DIA1", DbType.Int32, DIA1);
            db.AddInParameter(cmd, "@DIA2", DbType.Int32, DIA2);
            db.AddInParameter(cmd, "@DIA3", DbType.Int32, DIA3);
            db.AddInParameter(cmd, "@DIA4", DbType.Int32, DIA4);
         //   var idr = db.ExecuteReader(cmd);
            var DiaFech = Convert.ToInt32(db.ExecuteScalar(cmd));
            return DiaFech;
        }

        ///
        public bool BuscaParamFechCred4dPos() 
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'FECHCRED4D' ";

            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";           

        }

        public bool BuscaParamFechCred4dPre()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'FECHCRED4D' ";

            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";

        }

        #endregion

        #region TransHab

        public TRANSHABCKB GetTransHab(string transHab)
        {
            var transHabCkb = new TRANSHABCKB();
            const string sql = "PROC_TRANSHAB_LER";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@TRANSHAB", DbType.String, transHab);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                transHabCkb.PDV = (Convert.ToString(idr["PDV"]) == "") ? "1" : Convert.ToString(idr["PDV"]); 
                transHabCkb.POSMIC = (Convert.ToString(idr["POSMIC"]) == "") ? "1" : Convert.ToString(idr["POSMIC"]); 
                transHabCkb.URA = (Convert.ToString(idr["URA"]) == "") ? "1" : Convert.ToString(idr["URA"]); 
                transHabCkb.CENTRAL = (Convert.ToString(idr["CENTRAL"]) == "") ? "1": Convert.ToString(idr["CENTRAL"]);

                transHabCkb.COMPRA = (Convert.ToString(idr["COMPRA"]) == "") ? "1" : Convert.ToString(idr["COMPRA"]); 
                transHabCkb.COMPRAMED = (Convert.ToString(idr["COMPRAMED"]) == "") ? "1" : Convert.ToString(idr["COMPRAMED"]); 
                transHabCkb.COMPRAPARC = (Convert.ToString(idr["COMPRAPARC"]) == "") ? "1" : Convert.ToString(idr["COMPRAPARC"]); 
                transHabCkb.COMPRAMEDPARC = (Convert.ToString(idr["COMPRAMEDPARC"]) == "") ? "1" : Convert.ToString(idr["COMPRAMEDPARC"]); 


                transHabCkb.COMPRACRTDIG = Convert.ToString(idr["COMPRACRTDIG"]);
                transHabCkb.COMPRASAQUE = Convert.ToString(idr["COMPRASAQUE"]);
            }
            idr.Close();
            return transHabCkb;
        }

        public string SetTransHab(TRANSHABCKB transHab)
        {
            var transHabVal = string.Empty;
            const string sql = "PROC_TRANSHAB_GRAVAR";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@PDV", DbType.String, transHab.PDV);
            db.AddInParameter(cmd, "@POSMIC", DbType.String, transHab.POSMIC);
            db.AddInParameter(cmd, "@URA", DbType.String, transHab.URA);
            db.AddInParameter(cmd, "@CENTRAL", DbType.String, transHab.CENTRAL);
            db.AddInParameter(cmd, "@COMPRA", DbType.String, transHab.COMPRA);
            db.AddInParameter(cmd, "@COMPRAMED", DbType.String, transHab.COMPRAMED);
            db.AddInParameter(cmd, "@COMPRAPARC", DbType.String, transHab.COMPRAPARC);
            db.AddInParameter(cmd, "@COMPRAMEDPARC", DbType.String, transHab.COMPRAMEDPARC);
            db.AddInParameter(cmd, "@COMPRACRTDIG", DbType.String, transHab.COMPRACRTDIG);
            db.AddInParameter(cmd, "@COMPRASAQUE", DbType.String, transHab.COMPRASAQUE);

            

            var idr = db.ExecuteReader(cmd);
            if (idr.Read())
            {
                transHabVal = Convert.ToString(idr["TRANSHAB"]);
            }
            idr.Close();
            return transHabVal;
        }

        #endregion
    }
}