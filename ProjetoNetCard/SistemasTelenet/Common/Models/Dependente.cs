using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;

namespace NetCard.Common.Models
{
    public class Dependente
    {
        public string CpfTit { get; set; }
        public string CodCrt { get; set; }
        public int Id { get; set; }
        public int NumDep { get; set; }
        public string NomeDep { get; set; }
        public string CodPar { get; set; }
        public string DesPar { get; set; }
        public string DtNascimento { get; set; }
        public string Sexo { get; set; }
        public DateTime Inclusao { get; set; }
        public string LimDep { get; set; }

        public Cartao ManuDependente(ObjConn objConexao, DadosAcesso dadosAcesso, Dependente dependente, string operacao, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = "MW_LISTAEMANTEN_DEPENDENTE";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo),
                               new Parametro("@CPF", DbType.String, dependente.CpfTit),
                               new Parametro("@OPERACAO", DbType.String, operacao),
                               new Parametro("@NOMUSU", DbType.String, dependente.NomeDep),
                               new Parametro("@CODPAR", DbType.String, dependente.CodPar),
                               new Parametro("@CODCRT", DbType.String, dependente.CodCrt)
                           }; 
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var cartao = new Cartao();

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    retorno = (string)dr["RETORNO"];
                }
                dr.Close();                
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return cartao;
        }
    }
}
