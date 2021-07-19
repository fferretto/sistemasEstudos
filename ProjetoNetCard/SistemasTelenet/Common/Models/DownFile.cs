using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using TELENET.SIL;
using TELENET.SIL.PO;

namespace NetCard.Common.Models
{
    public class DownFile
    {
        public string Arquivo { get; set; }
        public string Nome { get; set; }
        public string Data { get; set; }


        public List<DownFile> ListagemArquivos(string cod_cli, int CodOpe, bool cliente, ObjConn objConn, out string retorno)
        {
            var listaResult = new List<DownFile>();
            try
            {
                string Caminho = CaminhoArquivoDownload(cliente, objConn, CodOpe.ToString());

               DirectoryInfo path = new DirectoryInfo(Caminho + cod_cli);


                int qtArquivos = RetQtdeRegistro(cliente, objConn);

                // Busca automaticamente todos os arquivos em todos os subdiretórios
                FileInfo[] Files = path.GetFiles("*", SearchOption.AllDirectories).OrderByDescending(x => x.CreationTime).Take(qtArquivos).ToArray();                

                foreach (FileInfo File in Files)
                {
                    var arquivo = new DownFile();

                    arquivo.Arquivo = File.FullName;
                    arquivo.Nome = File.Name;
                    arquivo.Data = File.CreationTime.ToString("dd/MM/yyyy hh:mm:ss");

                    listaResult.Add(arquivo);

                }

                retorno = listaResult.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch (Exception)
            {
                retorno = "Ocorreu um erro durante a operação";
            }
            return listaResult;
        }
        public int RetQtdeRegistro(bool cliente, ObjConn objConn)
        {
            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));

            string sql = "";
            int Qtde = 0;
            if (cliente)
            {
                sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'QT_DWN_ARQ_CLI'";
            }
            else
            {
                sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'QT_DWN_ARQ_CRE'";
            }
            SqlCommand comando = new SqlCommand(sql, conexao);

            try
            {
                conexao.Open();

                DataTable dt = new DataTable();

                dt.Load(comando.ExecuteReader());

                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Qtde = Convert.ToInt32(item["VAL"].ToString());
                    }
                }
                return Qtde;
            }
            catch
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                }

                return 30;
            }            
        }
        public string CaminhoArquivoDownload(bool cliente, ObjConn objConn, string CodOpe)
        {

            if (CodOpe.Trim() == "")
                return "";

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));

            string sql = "";
            string Caminho = "";
            if (cliente)
            {
                sql = "SELECT CAMINHO_ARQ_FECH_CLI as VAL FROM concentrador..OPERADORA WITH (NOLOCK) WHERE CODOPE = " + CodOpe;
            }
            else
            {
                sql = "SELECT CAMINHO_ARQ_FECH_CRE as VAL FROM concentrador..OPERADORA WITH (NOLOCK) WHERE CODOPE = " + CodOpe;
            }
            SqlCommand comando = new SqlCommand(sql, conexao);

            try
            {
                conexao.Open();

                DataTable dt = new DataTable();

                dt.Load(comando.ExecuteReader());

                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Caminho = item["VAL"].ToString();
                    }
                }
                return Caminho;
            }
            catch
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                }

                return "";
            }
        }
    }
}
