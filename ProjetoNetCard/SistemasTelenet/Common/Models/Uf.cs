using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using NetCard.Common.Util;

namespace NetCard.Common.Models
{
    public class Uf
    {
        [Required]
        [Display(Name = "Estado")]
        public string SigUf0 { get; set; }
        public string NomUf0 { get; set; }

        public Uf()
        {
            
        }

        public Uf(string _SigUf0, string _NomUf0)
        {
            SigUf0 = _SigUf0;
            NomUf0 = _NomUf0;
        }

        public List<Uf> ListaUF(ObjConn objConn, DadosAcesso dadosAcesso, string codSeg, string nomEsp)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_CONSULTA_UF";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro> {
                new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo),
                new Parametro("@SEGMENTO ", DbType.String, codSeg),
                new Parametro("@ESPECIALIDADE ", DbType.String, nomEsp)
            };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaUF = new List<Uf>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var uf = new Uf();
                    {
                        uf.SigUf0 = (dr["SIGUF0"]).ToString();
                        uf.NomUf0 = (dr["NOMUF0"]).ToString();
                    }
                    listaUF.Add(uf);
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return listaUF;
        }

        public List<Uf> GetUfUsu()
        {
            var lista = new List<Uf>();
            lista.Add(new Uf("AC", "Acre"));
            lista.Add(new Uf("AL", "Alagoas"));
            lista.Add(new Uf("AM", "Amazonas"));
            lista.Add(new Uf("AP", "Amapá"));
            lista.Add(new Uf("BA", "Bahia"));
            lista.Add(new Uf("CE", "Ceará"));
            lista.Add(new Uf("DF", "Distrito Federal"));
            lista.Add(new Uf("ES", "Espirito Santo"));
            lista.Add(new Uf("GO", "Goias"));
            lista.Add(new Uf("MA", "Maranhão"));
            lista.Add(new Uf("MG", "Minas Gerais"));
            lista.Add(new Uf("MS", "Mato Grosso do Sul"));
            lista.Add(new Uf("MT", "Mato Grosso"));
            lista.Add(new Uf("PA", "Pará"));
            lista.Add(new Uf("PB", "Paraiba"));
            lista.Add(new Uf("PE", "Pernambuco"));
            lista.Add(new Uf("PI", "Piauí"));
            lista.Add(new Uf("PR", "Paraná"));
            lista.Add(new Uf("RJ", "Rio de Janeiro"));
            lista.Add(new Uf("RN", "Rio Grande do Norte"));
            lista.Add(new Uf("RO", "Rondonia"));
            lista.Add(new Uf("RR", "Roraima"));
            lista.Add(new Uf("RS", "Rio Grande do Sul"));
            lista.Add(new Uf("SC", "Santa Catarina"));
            lista.Add(new Uf("SE", "Sergipe"));
            lista.Add(new Uf("SP", "São Paulo"));
            lista.Add(new Uf("TO", "Tocantins"));
            return lista;
        }
    }
}
