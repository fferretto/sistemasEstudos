using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using NetCard.Common.Util;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace NetCard.Common.Models
{
    public class Adesao
    {
        
        public string Inicio { get; set; }
        public string Fim { get; set; }

        [Display(Name = "Declaro que li o regulamento e aceito participar.")]
        public bool Aceite { get; set; }
        
        [Display(Name = "Data de Adesão")]
        public DateTime DtAdesao { get; set; }
        
        [Display(Name = "Pontos")]
        public int Pontos { get; set; }
        
        [Display(Name = "Disponíveis para Conversão")]
        public int PontosDisp{ get; set; }
        
        [Display(Name = "Data Última Pontuação")]
        public DateTime DtUltPonto { get; set; }

        [Display(Name = "Informe os pontos a converter")]        
        public int PontosConv { get; set; }

        public string Cartao { get; set; }

        public string TextoInfo { get; set; }

        public int MinPontosConversao { get; set; }

        public List<CartaoFidelidade> ListaCartoes { get; set; }

        public Adesao ResumoPontuacaoUsu(ObjConn objConn, string cpf, out string retorno)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand comando = new SqlCommand("FIDELIDADE_RESUMO_PONTOS_USU", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add(new SqlParameter() { ParameterName = "@CPF", Value = cpf, DbType = System.Data.DbType.String });

            
            retorno = string.Empty;

            
            try
            {
                DataTable dt = new DataTable();

                conexao.Open();
                dt.Load(db.ExecuteReader(comando));
                conexao.Close();

                if (dt.Rows.Count > 0)
                {

                    if (Convert.ToInt16(dt.Rows[0]["RETORNO"]) > 0)
                        retorno = Convert.ToString(dt.Rows[0]["MENSAGEM"]);
                    else
                    {
                        DtAdesao = Convert.ToDateTime(dt.Rows[0]["DTADESAO"]);
                        Pontos = Convert.ToInt32(dt.Rows[0]["PONTOS"]);
                        PontosDisp = Convert.ToInt32(dt.Rows[0]["PONTOSDISP"]);
                        DtUltPonto = dt.Rows[0]["DTULTPONTO"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["DTULTPONTO"]) : DateTime.UtcNow;
                        TextoInfo = Convert.ToString(dt.Rows[0]["TEXTO_CONV"]);
                    }
                }
                
            }
            catch
            {
             if(conexao.State != ConnectionState.Closed)
                conexao.Close();
            }
            return this;
        }

        public string AdesaoFidelidade(ObjConn objConn, string cpf, string acao)
        {            
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "FIDELIDADE_ADESAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());            
            db.AddInParameter(cmd, "@CPF", DbType.String, cpf);
            db.AddInParameter(cmd, "@ACAO", DbType.String, acao);
            var retorno = string.Empty;

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

        public string ConvPontos(ObjConn objConn, string cpf, string cartao, int pontos)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "FIDELIDADE_CONV_PONTOS";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            db.AddInParameter(cmd, "@CPF", DbType.String, cpf);
            db.AddInParameter(cmd, "@CARTAO", DbType.String, cartao);
            db.AddInParameter(cmd, "@PONTOS_CONV", DbType.Int32, pontos);
            var retorno = string.Empty;

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
    }
}
