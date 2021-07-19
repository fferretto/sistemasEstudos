using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class TransacoesPendentes
    {

        [Display(Name = "Nome ou letras iniciais ou CPF")]
        public string Filtro { get; set; }

        [Display(Name = "Data Inicial")]
        public string DtInicial { get; set; }

        [Display(Name = "Data Final")]
        public string DtFinal { get; set; }

        [Display(Name = "Exibe Transações Canceladas")]
        public bool ExibeTransCanc { get; set; }

        public DateTime DatTra { get; set; }
        public int NsuHos { get; set; }
        public int NsuAut { get; set; }
        public int CodCli { get; set; }
        public int CodCre { get; set; }
        public int TipTra { get; set; }
        public string Descricao { get; set; }
        public string CodRta { get; set; }
        public decimal ValTra { get; set; }
        public string Cpf { get; set; }
        public string NomUsu { get; set; }
        public string Matricula { get; set; }
        public string Filial { get; set; }
        public string Setor { get; set; }
        public string SubRede { get; set; }
        public string RazSoc { get; set; }
        public string NomFan { get; set; }
        public string Cnpj { get; set; }
        public string Segmento { get; set; }
        public string Rede { get; set; }
        public bool Checked {get; set;}
        public string Mensagem { get; set; }

        public List<TransacoesPendentes> ListagemTransacoesPendentesPapel(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_CONS_TRANS_PENDPAPEL";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;            
            
            db.AddInParameter(cmd, "@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "@CODCLI", DbType.Int32, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "@FILTRO", DbType.String, Filtro);
            db.AddInParameter(cmd, "@DATAINI", DbType.DateTime, Convert.ToDateTime(DtInicial));
            db.AddInParameter(cmd, "@DATAFIM", DbType.DateTime, Convert.ToDateTime(DtFinal));
            db.AddInParameter(cmd, "@EXIBETRANSCANC", DbType.String, ExibeTransCanc ? "S" : "N");
            

            IDataReader dr = null;
            var listaResult = new List<TransacoesPendentes>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var transPend = new TransacoesPendentes();
                    transPend.DatTra = Convert.ToDateTime(dr["DATTRA"]);
                    transPend.NsuHos = Convert.ToInt32(dr["NSUHOS"]);
                    transPend.NsuAut = Convert.ToInt32(dr["NSUAUT"]);
                    transPend.CodCli = Convert.ToInt32(dr["CODCLI"]);
                    transPend.CodCre = Convert.ToInt32(dr["CODCRE"]);
                    transPend.TipTra = Convert.ToInt32(dr["TIPTRA"]);
                    transPend.Descricao = Convert.ToString(dr["DESCRICAO"]);
                    transPend.CodRta = Convert.ToString(dr["CODRTA"]).ToString();
                    transPend.ValTra = Convert.ToDecimal(dr["VALTRA"]);
                    transPend.Cpf = Convert.ToString(dr["CPF"]);
                    transPend.NomUsu = Convert.ToString(dr["NOMUSU"]);
                    transPend.Matricula = Convert.ToString(dr["MATRICULA"]);
                    transPend.Filial = Convert.ToString(dr["FILIAL"]);
                    transPend.Setor = Convert.ToString(dr["SETOR"]);
                    transPend.SubRede = Convert.ToString(dr["SUBREDE"]);
                    transPend.RazSoc = Convert.ToString(dr["RAZSOC"]);
                    transPend.NomFan = Convert.ToString(dr["NOMFAN"]);
                    transPend.Cnpj = Convert.ToString(dr["CNPJ"]);
                    transPend.Segmento = Convert.ToString(dr["SEGMENTO"]);
                    transPend.Rede = Convert.ToString(dr["REDE"]);
                    listaResult.Add(transPend);
                }
                dr.Close();
                retorno = listaResult.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch (Exception ex)
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return listaResult;
        }

        public bool ConfirmarTransTipoA(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            retorno = string.Empty;
            var sucesso = false;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_CONFIRMA_TRANS_A";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            db.AddInParameter(cmd, "@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "@DATTRA", DbType.DateTime, DatTra);
            db.AddInParameter(cmd, "@NSUHOS", DbType.Int32, NsuHos);
            db.AddInParameter(cmd, "@NSUAUT", DbType.Int32, NsuAut);
            db.AddInParameter(cmd, "@IDOPEMW", DbType.Int32, dadosAcesso.Id);
                         
            IDataReader dr = null;
            var listaResult = new List<Cartao>();

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    retorno = dr["RETORNO"].ToString();
                    if (retorno == Constantes.ok)
                    {
                        sucesso = true;
                    }
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return sucesso;
        }

        public bool InvalidarTransTipoA(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            retorno = string.Empty;
            var sucesso = false;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_CANCELA_TRANS";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            db.AddInParameter(cmd, "@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "@DATTRA", DbType.DateTime, DatTra);
            db.AddInParameter(cmd, "@NSUHOS", DbType.Int32, NsuHos);
            db.AddInParameter(cmd, "@NSUAUT", DbType.Int32, NsuAut);
            db.AddInParameter(cmd, "@JUSTIFIC", DbType.String, "CANCELADA PELO OPERADOR LOGIN: " + objConn.LoginWeb);
            db.AddInParameter(cmd, "@ID_FUNC", DbType.Int32, 0);

            IDataReader dr = null;
            var listaResult = new List<Cartao>();

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    retorno = dr["RETORNO"].ToString();
                    if (retorno == Constantes.ok)
                    {
                        sucesso = true;
                    }
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return sucesso;
        }
    }
}
