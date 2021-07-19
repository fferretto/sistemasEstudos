using NetCard.Common.Util;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace NetCard.Common.Models
{

    public class AgendamentoBloqDesbloq
    {
        public AgendamentoBloqDesbloq()
        {
            ListaCartoes = new List<AgendamentoManualVM>();
            ConsultaCartoes = new List<AgendamentoManualVM>();
        }

        public List<AgendamentoManualVM> ListaCartoes { get; set; }
        public List<AgendamentoManualVM> ConsultaCartoes { get; set; }

        [Display(Name = "Matrícula, nome ou letras iniciais, ou CPF")]
        public string Filtro { get; set; }

        [Display(Name = "Selecione o Arquivo Desejado")]
        public string Arquivo { get; set; }

        [Display(Name = "Data de Início")]
        public string dtInicio { get; set; }

        [Display(Name = "Data Final")]
        public string dtFim { get; set; }

        [Display(Name = "Data de Bloqueio")]
        public string dtBloqueio { get; set; }

        [Display(Name = "Data de Desbloqueio")]
        public string dtDesbloqueio { get; set; }

        [Display(Name = "Status do Agendamento")]
        public string Status { get; set; }
        
        [Display(Name = "Agendar Bloqueio")]
        public bool ckBloqueio { get; set; }

        [Display(Name = "Agendar Desbloqueio")]
        public bool ckDesbloqueio { get; set; }        

        public string Mensagem { get; set; }
        public string MensagemPesquisa { get; set; }
        
        public int AbaMenu { get; set; }


        public string AgendaBloqueio(string ACAO, DateTime DATACAO, int CODCLI, string CPF, int NUMDEP, int IDOPEMW, int SISTEMA, ObjConn objConn, out int CodRet)
        {
            string msgRetorno = "";

            string procedure = "PROC_INSERI_AGEND_BLOQ_DESBLOQ";

            SqlConnection connection = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand cmd = new SqlCommand(procedure, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@SISTEMA", Value = SISTEMA, DbType = DbType.Int32 });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@ACAO", Value = ACAO, DbType = DbType.String });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DATACAO", Value = DATACAO, DbType = DbType.DateTime });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CODCLI", Value = CODCLI, DbType = DbType.Int32 });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CPF", Value = CPF, DbType = DbType.String });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@NUMDEP", Value = NUMDEP, DbType = DbType.Int16 });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@IDOPEMW", Value = IDOPEMW, DbType = DbType.Int32 });

            DataTable dt = new DataTable();

            try
            {
                string pergunta = "";
                string resposta = "";
                string tentativas = "0";
                CodRet = 0;

                connection.Open();
                dt.Load(cmd.ExecuteReader());
                connection.Close();

                if (dt.Rows.Count > 0)
                {
                    CodRet = Convert.ToInt32(dt.Rows[0]["RETORNO"].ToString());
                    msgRetorno = dt.Rows[0]["MENSAGEM"].ToString();
                }

                return msgRetorno;

            }
            catch (Exception ex)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                throw ex;
            }
        }
        public string CancelamentoAgendamento(int CODCLI, string CPF, string NUMDEP, string ACAO, int IDOPEMW, int SISTEMA, ObjConn objConn)
        {
            SqlConnection conn = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));

            string SQL = "";
            SQL += "UPDATE AGEN_BLOQ_DESBLOQ" + Environment.NewLine;
            SQL += "    SET STATUS = 'CANCELADO'" + Environment.NewLine;
            SQL += "        ,IDOPEMWCANC = " + IDOPEMW + Environment.NewLine;
            SQL += "        ,MSGEXECUCAO = 'Agendamento cancelado via sistema'" + Environment.NewLine;
            SQL += "    where CODCLI = " + CODCLI + Environment.NewLine;
            SQL += "    AND CPF = '" + CPF + "'" + Environment.NewLine;
            SQL += "    AND NUMDEP = " + NUMDEP + Environment.NewLine;
            SQL += "    AND ACAO = '" + ACAO + "'" + Environment.NewLine;
            SQL += "    AND SISTEMA = " + SISTEMA + Environment.NewLine;
            SQL += "    AND STATUS = 'PENDENTE'" + Environment.NewLine;

            SqlCommand cmd = new SqlCommand(SQL, conn);

            try
            {
                conn.Open();
                int linhasAfetadas = cmd.ExecuteNonQuery();
                conn.Close();

                if (linhasAfetadas == 0)
                    return "Não foi possível realizar o cancelamento. favor contactar o suporte.";

                return "Cancelamento Realizado com sucesso!";

            }
            catch (Exception ex)
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }

                return "Não foi possível realizar o cancelamento. favor contactar o suporte.";
            }
        }


    }

    public class AgendamentoManualVM
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Td { get; set; }
        public string NumDep { get; set; }

        public int ID { get; set; }
        public string CodCrt { get; set; }
        public string Status { get; set; }
        public string AgendBloqueio { get; set; }
        public string AgendDesbloqueio { get; set; }
        public bool Checked { get; set; }

        public string Acao { get; set; }
        public string dtAcao { get; set; }
        public string dtSolicitacao { get; set; }
        public string NomOpe { get; set; }

        public string CodCrtMask { get { return CodCrt != null ? Utils.MascaraCartao(CodCrt, 17) : string.Empty; } }

        public List<AgendamentoManualVM> ListaUsuarios(ObjConn objConn, DadosAcesso dadosAcesso, string filtro, out string retorno)
        {

            int count = 0;
            SqlConnection conn = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));

            string SQL = "";
            SQL += "SELECT dbo.MascaraCartao(U.CODCRT,13) AS CARTAO" + Environment.NewLine;
            SQL += "        ,U.CPF" + Environment.NewLine;
            SQL += "        ,UPPER(U.NOMUSU) AS NOME" + Environment.NewLine;
            SQL += "        ,U.NUMDEP AS TD" + Environment.NewLine;
            SQL += "        ,SUBSTRING(S.DESTA,1,15) AS DESC_STATUS" + Environment.NewLine;
            SQL += "        ,CASE WHEN   (SELECT 1 FROM AGEN_BLOQ_DESBLOQ ABD WITH (NOLOCK) " + Environment.NewLine;
            SQL += "                    WHERE ABD.CPF = U.CPF and ABD.CODCLI = U.CODCLI " + Environment.NewLine;
            SQL += "                        AND ABD.NUMDEP = U.NUMDEP AND ABD.STATUS = 'PENDENTE' AND ABD.ACAO = 'B') IS NOT NULL" + Environment.NewLine;
            SQL += "        THEN 'Sim' Else 'Não' END AS AGENDBLOQUEIO" + Environment.NewLine;
            SQL += "        ,CASE WHEN (SELECT 1 FROM AGEN_BLOQ_DESBLOQ ABD WITH (NOLOCK) " + Environment.NewLine;
            SQL += "                    WHERE ABD.CPF = U.CPF and ABD.CODCLI = U.CODCLI " + Environment.NewLine;
            SQL += "                        AND ABD.NUMDEP = U.NUMDEP AND ABD.STATUS = 'PENDENTE' AND ABD.ACAO = 'd') IS NOT NULL" + Environment.NewLine;
            SQL += "        THEN 'Sim' Else 'Não' END AS AGENDDESBLOQUEIO" + Environment.NewLine;

            if (dadosAcesso.Sistema.cartaoPJVA == 0)
                SQL += "    FROM USUARIO U WITH (NOLOCK) " + Environment.NewLine;
            else
                SQL += "    FROM USUARIOVA U WITH (NOLOCK) " + Environment.NewLine;
            
            
            SQL += "        INNER JOIN STATUS S WITH (NOLOCK) ON S.STA = U.STA " + Environment.NewLine;
            SQL += "    where U.CODCLI = " + dadosAcesso.Codigo + Environment.NewLine;
            SQL += "    and U.STA in ('00', '01')" + Environment.NewLine;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                if(Regex.IsMatch(filtro, @"\d"))
                {
                    if (filtro.Length > 10)
                    {
                        SQL += $"    and U.CPF = '{filtro}'" +  Environment.NewLine;
                    }
                    else
                    {
                        SQL += $"    and U.MAT LIKE '{filtro}%'" + Environment.NewLine;
                    }
                }
                else
                {
                    SQL += $"    and U.NOMUSU LIKE '{filtro}%'" + Environment.NewLine;
                }
            }

            SQL += "ORDER BY U.CPF, LTRIM(U.NOMUSU)" + Environment.NewLine;

            SqlCommand cmd = new SqlCommand(SQL, conn);

            var listaResult = new List<AgendamentoManualVM>();
            try
            {
                var parentesco = new List<ItensGeneric>();
                DataTable dt = new DataTable();

                conn.Open();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        count++;
                        var cartaoUsu = new AgendamentoManualVM();
                        cartaoUsu.ID = count;

                        cartaoUsu.Checked = false;

                        cartaoUsu.Cpf = (row["CPF"]).ToString().Trim();
                        cartaoUsu.Nome = (row["NOME"]).ToString().Trim();
                        cartaoUsu.Td = Convert.ToInt16(row["TD"]) == 0 ? "TITULAR" : "DEPENDENTE";
                        cartaoUsu.NumDep = Convert.ToString(row["TD"]);
                        cartaoUsu.Status = (row["DESC_STATUS"]).ToString().Trim();
                        cartaoUsu.CodCrt = (row["CARTAO"]).ToString().Trim();
                        cartaoUsu.AgendBloqueio = (row["AGENDBLOQUEIO"]).ToString().Trim();
                        cartaoUsu.AgendDesbloqueio = (row["AGENDDESBLOQUEIO"]).ToString().Trim();

                        listaResult.Add(cartaoUsu);

                    }
                }

                retorno = listaResult.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch (Exception)
            {
                retorno = "Ocorreu um erro durante a operação";

            }
            return listaResult;

        }
        public List<AgendamentoManualVM> ConsultaAgendamentos(ObjConn objConn, DadosAcesso dadosAcesso, AgendamentoBloqDesbloq model, out string retorno)
        {

            int count = 0;
            model.Status = model.Status ?? "";
            SqlConnection conn = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));

            string SQL = "";
            SQL += " SELECT DBO.MASCARACARTAO(U.CODCRT,13) AS CARTAO" + Environment.NewLine;
            SQL += "       ,ABD.CODAGENDAMENTO" + Environment.NewLine;
            SQL += "       ,U.CPF" + Environment.NewLine;
            SQL += "       ,UPPER(U.NOMUSU) AS NOME" + Environment.NewLine;
            SQL += "       ,U.NUMDEP AS TD" + Environment.NewLine;
            SQL += "       ,ABD.STATUS" + Environment.NewLine;
            SQL += "       ,CASE WHEN ABD.ACAO = 'B' THEN 'Bloqueio' ELSE 'Desbloqueio' END ACAO" + Environment.NewLine;
            SQL += "       ,ABD.DATACAO       " + Environment.NewLine;
            SQL += "       ,ABD.DATSOLICITACAO" + Environment.NewLine;
            SQL += "       ,OP.NOME AS NOMOPE              " + Environment.NewLine;
            SQL += "   FROM AGEN_BLOQ_DESBLOQ  ABD WITH (NOLOCK) " + Environment.NewLine;

            if (dadosAcesso.Sistema.cartaoPJVA == 0)
                SQL += "       ,USUARIO   U WITH (NOLOCK) " + Environment.NewLine;
            else
                SQL += "       ,USUARIOVA U WITH (NOLOCK)" + Environment.NewLine;

            SQL += "       ,OPERADORMW         OP WITH (NOLOCK) " + Environment.NewLine;
            SQL += "   WHERE ABD.CPF = U.CPF " + Environment.NewLine;
            SQL += "     AND ABD.CODCLI = U.CODCLI " + Environment.NewLine;
            SQL += "     AND ABD.NUMDEP = U.NUMDEP" + Environment.NewLine;
            SQL += "     AND (ABD.IDOPEMWCANC IS NULL OR ABD.IDOPEMWCANC > 0) " + Environment.NewLine;
            SQL += "     AND ABD.SISTEMA = " + dadosAcesso.Sistema.cartaoPJVA + Environment.NewLine;
            SQL += "     AND U.CODCLI = " + dadosAcesso.Codigo + Environment.NewLine;
            SQL += "     AND ABD.DATACAO >= '" + Convert.ToDateTime(model.dtInicio).ToString("yyyyMMdd") + "'" + Environment.NewLine;
            SQL += "     AND ABD.DATACAO <= '" + Convert.ToDateTime(model.dtFim).ToString("yyyyMMdd") + "'" + Environment.NewLine;

            if (model.Status != "")
            {
                SQL += "     AND ABD.STATUS = '" + model.Status + "'" + Environment.NewLine;
            }

            if (!string.IsNullOrWhiteSpace(model.Filtro))
            {
                if (Regex.IsMatch(model.Filtro, @"\d"))
                {
                    if (model.Filtro.Length > 10)
                    {
                        SQL += $"    and U.CPF = '{model.Filtro}'" + Environment.NewLine;
                    }
                    else
                    {
                        SQL += $"    and U.MAT LIKE '{model.Filtro}%'" + Environment.NewLine;
                    }
                }
                else
                {
                    SQL += $"    and U.NOMUSU LIKE '{model.Filtro}%'" + Environment.NewLine;
                }
            }

            SQL += "     AND OP.ID = ABD.IDOPEMW" + Environment.NewLine;
            SQL += "ORDER BY U.CPF, LTRIM(U.NOMUSU)" + Environment.NewLine;

            SqlCommand cmd = new SqlCommand(SQL, conn);

            var listaResult = new List<AgendamentoManualVM>();
            try
            {
                DataTable dt = new DataTable();

                conn.Open();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        count++;
                        var cartaoUsu = new AgendamentoManualVM();

                        cartaoUsu.Cpf = (row["CPF"]).ToString().Trim();
                        cartaoUsu.Nome = (row["NOME"]).ToString().Trim();
                        cartaoUsu.Td = Convert.ToInt16(row["TD"]) == 0 ? "TITULAR" : "DEPENDENTE";
                        cartaoUsu.NumDep = Convert.ToString(row["TD"]);
                        cartaoUsu.Status = (row["STATUS"]).ToString().Trim();
                        cartaoUsu.CodCrt = (row["CARTAO"]).ToString().Trim();
                        cartaoUsu.Acao = (row["ACAO"]).ToString().Trim();
                        if (row["DATACAO"] != DBNull.Value && Convert.ToDateTime(row["DATACAO"]) != DateTime.MinValue)
                            cartaoUsu.dtAcao = (Convert.ToDateTime(row["DATACAO"].ToString())).ToShortDateString();
                        if (row["DATSOLICITACAO"] != DBNull.Value && Convert.ToDateTime(row["DATSOLICITACAO"]) != DateTime.MinValue)
                            cartaoUsu.dtSolicitacao = (Convert.ToDateTime(row["DATSOLICITACAO"].ToString().Trim())).ToShortDateString();
                        cartaoUsu.NomOpe = (row["NOMOPE"]).ToString().Trim();

                        listaResult.Add(cartaoUsu);

                    }
                }
                retorno = listaResult.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch (Exception)
            {
                retorno = "Ocorreu um erro durante a operação";

            }
            return listaResult;

        }



    }
}
