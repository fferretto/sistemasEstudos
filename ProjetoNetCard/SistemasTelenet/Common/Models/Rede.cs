using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using NetCard.Common.Util;
using System;

namespace NetCard.Common.Models
{
    public class Rede
    {
        public string NomFan { get; set; }
        public string Tel { get; set; }
        public string EndCre { get; set; }
        public string EndCpl { get; set; }
        public string NomBai { get; set; }
        public string NomLoc { get; set; }
        public string SigUf0 { get; set; }
        public string NomSeg { get; set; }
        public string NomAti { get; set; }
        public string CodCre { get; set; }

        public List<Rede> ListagemRedes(ObjConn objConn, int sistema, int codCli, string nomFan, string nomSeg, string nomEsp, string sigUf0, string nomLoc, string codBai, out string retorno)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "REL_LISTA_REDE_CREDENCIADA";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, sistema),
                               new Parametro("@CODCLI", DbType.Int32, codCli),
                               new Parametro("@NOMFAN", DbType.String, nomFan),
                               new Parametro("@SEGMENTO", DbType.String, nomSeg),
                               new Parametro("@ESPECIALIDADE", DbType.String, nomEsp),
                               new Parametro("@UF", DbType.String, sigUf0),
                               new Parametro("@CIDADE", DbType.String, nomLoc),
                               new Parametro("@BAIRRO", DbType.String, codBai),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaResult = new List<Rede>();

            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var consultaRede = new Rede();
                    consultaRede.CodCre = (dr["CODCRE"]).ToString().Trim();
                    consultaRede.NomFan = (dr["NOME_FANTASIA"]).ToString().Trim();
                    consultaRede.EndCre = (dr["ENDERECO"]).ToString().Trim();
                    consultaRede.EndCpl = (dr["COMPLEMENTO"]).ToString().Trim();
                    consultaRede.NomBai = (dr["BAIRRO"]).ToString().Trim();
                    consultaRede.NomLoc = (dr["LOCALIDADE"]).ToString().Trim();
                    consultaRede.SigUf0 = (dr["ESTADO"]).ToString().Trim();
                    consultaRede.Tel = (dr["TEL"]).ToString().Trim();
                    consultaRede.NomSeg = (dr["SEGMENTO"]).ToString().Trim();
                    consultaRede.NomAti = (dr["ATIVIDADE"]).ToString().Trim();
                    listaResult.Add(consultaRede);
                }
                dr.Close();
                retorno = listaResult.Count > 0 ? Constantes.ok : "Nenhum resultado encontrado.";
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return listaResult;
        }

        public List<string> CalcularCredenciado(ObjConn objConn, int codCli, int codCre, string nomSeg, string cpf, decimal valor, int numParcela, out string retorno)
        {
            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand comando = new SqlCommand("CALCULADORA_PARCELAS", conexao);
            comando.CommandType = CommandType.StoredProcedure;
            
            List<string> retornoString = new List<string>();

            comando.Parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "@CODCLI", Value = codCli });
            comando.Parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "@CPF", Value = cpf });
            comando.Parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "@VALORCOMPRA", Value = valor });
            comando.Parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "@NUMPARC", Value = numParcela });
//            comando.Parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "@CODCRE", Value = codCre });
            comando.Parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "@SEGMENTO", Value = nomSeg });

            conexao.Open();

            try
            {
                DataTable dt = new DataTable();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    retorno = dt.Rows[0]["RETORNO"].ToString();

                    if (retorno == Constantes.ok)
                    {
                        retornoString.Add(dt.Rows[0]["LINHA1"].ToString());
                        retornoString.Add(dt.Rows[0]["LINHA2"].ToString());
                    }
                    else
                    {
                        retornoString.Add(dt.Rows[0]["MENSAGEM"].ToString());
                    }
                }
                else
                {
                    retorno = "Ocorreu um erro durante a operação";
                    retornoString.Add("Ocorreu um erro durante a operação");
                }

                return retornoString;
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                retorno = "Ocorreu um erro durante a operação";
                return new List<string>();

            }
        }
    }
}
