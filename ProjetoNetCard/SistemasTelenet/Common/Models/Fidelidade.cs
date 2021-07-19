using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System.Data;
using TELENET.SIL;
using System.Data.SqlClient;

namespace NetCard.Common.Models
{
    public class Fidelidade
    {
        [Display(Name = "Data Inicial")]
        public string DtInicial { get; set; }

        [Display(Name = "Data Final")]
        public string DtFinal { get; set; }

        public int OpcaoPesq { get; set; }
        public string Filtro { get; set; }        
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Razao { get; set; }        
        public string Cnpj { get; set; }
        public string Cartao { get; set; }
        public string Nsu { get; set; }
        public string CodCrtMask { get { return Cartao != null ? Utils.MascaraCartao(Cartao, 17) : string.Empty; } }

        [Display(Name = "Data Compra")]
        public string DataPonto { get; set; }
        [Display(Name = "Valor Compra")]
        public decimal ValorCompra { get; set; }
        [Display(Name = "Forma de Pagamento")]
        public string FormaPgto { get; set; }
        [Display(Name = "Quantidade")]
        public int QtdePontos { get; set; }        
        public string Situacao { get; set; }
        public DateTime DataExp { get; set; }
        public string TipoPontuacao { get; set; }
        public string MinimoPontosConversaoBonus { get; set; }
        public string RealizarCadastro { get; set; }



        public List<Fidelidade> ExtratoPontuacaoCredenciado(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "FIDELIDADE_EXT_PONTOS_CRED";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@CODCRE", DbType.Int32, dadosAcesso.Codigo),
                               new Parametro("@DATAINI", DbType.String, Convert.ToDateTime(DtInicial).ToString("yyyyMMdd")),
                               new Parametro("@DATAFIM", DbType.String, Convert.ToDateTime(DtFinal).ToString("yyyyMMdd")),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaResult = new List<Fidelidade>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var pontuacao = new Fidelidade();
                    pontuacao.DataPonto = Convert.ToString(dr["DATA"]);
                    pontuacao.Cpf = Convert.ToString(dr["CPF"]);
                    pontuacao.Nome = Convert.ToString(dr["NOMUSU"]);
                    pontuacao.Cnpj = Convert.ToString(dr["CNPJCRED"]);
                    pontuacao.Razao = Convert.ToString(dr["FANTASIA"]);
                    pontuacao.ValorCompra = Convert.ToDecimal(dr["VALOR"]);
                    pontuacao.QtdePontos = Convert.ToInt32(dr["PONTOS"]);
                    pontuacao.DataExp = Convert.ToDateTime(dr["DATA_EXP_PONTOS"]);
                    pontuacao.TipoPontuacao = Convert.ToString(dr["FORMAPONTO"]);
                    pontuacao.FormaPgto = Convert.ToString(dr["FORMAPGTO"]);
                    pontuacao.Situacao = Convert.ToString(dr["SITUACAO"]);
                    pontuacao.Nsu = dr["NSUPNT"].ToString();
                    listaResult.Add(pontuacao);
                }
                dr.Close();
                retorno = listaResult.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return listaResult;
        }

        public List<Fidelidade> ExtratoPontuacaoUsuario(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "FIDELIDADE_EXT_PONTOS_USU";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@CPF", DbType.String, dadosAcesso.Cpf),
                               new Parametro("@DATAINI", DbType.String, Convert.ToDateTime(DtInicial).ToString("yyyyMMdd")),
                               new Parametro("@DATAFIM", DbType.String, Convert.ToDateTime(DtFinal).ToString("yyyyMMdd")),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaResult = new List<Fidelidade>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var pontuacao = new Fidelidade();
                    //pontuacao.Nome = Convert.ToString(dr["NOMUSU"]);
                    //pontuacao.Cpf = Convert.ToString(dr["CPF"]);
                    pontuacao.DataPonto = Convert.ToString(dr["DATA"]);
                    pontuacao.Cnpj = Convert.ToString(dr["CNPJCRED"]);
                    pontuacao.Razao = Convert.ToString(dr["FANTASIA"]);
                    pontuacao.ValorCompra = Convert.ToDecimal(dr["VALOR"]);
                    pontuacao.QtdePontos = Convert.ToInt32(dr["PONTOS"]);
                    pontuacao.DataExp = Convert.ToDateTime(dr["DATA_EXP_PONTOS"]);
                    pontuacao.TipoPontuacao = Convert.ToString(dr["FORMAPONTO"]);
                    pontuacao.FormaPgto = Convert.ToString(dr["FORMAPGTO"]);
                    //pontuacao.Situacao = Convert.ToString(dr["SITUACAO"]);
                    pontuacao.Nsu = dr["NSUAUT"].ToString();
                    listaResult.Add(pontuacao);
                }
                dr.Close();
                retorno = listaResult.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return listaResult;
        }

        public List<Fidelidade> BuscaUsuarioAutocomplete(ObjConn objConn, string filtro)
        {
            filtro += "%'";
            var campo = "F.CPF";
            var onde = "F.CPF LIKE '" + Utils.RetirarCaracteres(".-", filtro);

            var sql = new StringBuilder();
            sql.AppendLine("SELECT TOP 10 F.CPF, DTADESAO, C.NOMUSU ");
            sql.AppendLine("FROM FIDELIDADE_USUARIO F WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN CONTROLE_USUARIO C WITH (NOLOCK) ON F.CPF = C.CPF ");
            sql.AppendLine("WHERE F.DTCANCEL IS NULL AND F.ADERIU = 'S' AND " + onde);            
            sql.AppendLine("ORDER BY " +campo);

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));            
            var cmd = db.GetSqlStringCommand (sql.ToString());            
            IDataReader dr = null;

            var listaResult = new List<Fidelidade>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var usu = new Fidelidade();                    
                    usu.Cpf = (dr["CPF"]).ToString().Trim();
                    usu.Nome = Convert.ToString(dr["NOMUSU"]);
                    listaResult.Add(usu);
                }
                dr.Close();                
            }
            catch (Exception)
            {                
                if (dr != null)
                    dr.Close();
            }
            return listaResult;
        }

        public string PontuarUsuario(ObjConn objConn)
        {
            var retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "FIDELIDADE_PONTUA";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            db.AddInParameter(cmd, "@CPF", DbType.String, Cpf);
            db.AddInParameter(cmd, "@CODCRE", DbType.String, objConn.CodCre);
            db.AddInParameter(cmd, "@FORMAPONTO", DbType.String, "M");
            db.AddInParameter(cmd, "@FORMAPGTO", DbType.String, FormaPgto);
            db.AddInParameter(cmd, "@VALOR", DbType.Decimal, ValorCompra);
            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    if (Convert.ToInt16(dr["RETORNO"]) > 0)
                        retorno = Convert.ToString(dr["MENSAGEM"]);
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return retorno;
        }

        public string IncluirUsuario(ObjConn objConn)
        {
            var retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "FIDELIDADE_CAD_USU";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            db.AddInParameter(cmd, "@CPF", DbType.String, Utils.RetirarCaracteres(".-", Cpf));
            db.AddInParameter(cmd, "@NOME", DbType.String, Utils.RemoverAcentos(Nome).ToUpper());
            db.AddInParameter(cmd, "@CODCRE", DbType.String, objConn.CodCre);
            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    if (Convert.ToInt16(dr["RETORNO"]) > 0)
                        retorno = Convert.ToString(dr["MENSAGEM"]);
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return retorno;
        }

        public List<CartaoFidelidade> ListaCartoesUsuarioFidelidade(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "FIDELIDADE_CARTOES";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro> { new Parametro("@CPF", DbType.String, dadosAcesso.Cpf) };
            foreach (var parametro in list) db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaResult = new List<CartaoFidelidade>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    if (Convert.ToInt16(dr["RETORNO"]) > 0)
                        retorno = Convert.ToString(dr["MENSAGEM"]);
                    else
                    {
                        var cartao = new CartaoFidelidade();
                        cartao.CodCrt = Convert.ToString(dr["CODCRT"]);
                        cartao.Rotulo = Convert.ToString(dr["ROTULO"]);
                        listaResult.Add(cartao);
                    }
                }
                dr.Close();
                retorno = listaResult.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return listaResult;
        }

        public Fidelidade BuscarParametrosGerais(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            retorno = string.Empty;
            
            const string sql = "SELECT TOP 1 MIN_PONTOS_CONV_BONUS FROM FIDELIDADE_PARAM_GERAL WITH (NOLOCK) ";

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand comando = new SqlCommand(sql, conexao);


            Fidelidade fidelidade = new Fidelidade();

            try
            {
                DataTable dt = new DataTable();

                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    fidelidade.MinimoPontosConversaoBonus = dt.Rows[0]["MIN_PONTOS_CONV_BONUS"].ToString();
                }

                retorno = dt.Rows.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                retorno = "Ocorreu um erro durante a operação";
                
            }

            return fidelidade;
            
        }
    }
}
