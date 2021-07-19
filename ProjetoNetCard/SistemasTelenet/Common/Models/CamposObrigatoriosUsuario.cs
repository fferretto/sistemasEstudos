using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class CamposObrigatoriosUsuario
    {
        public int Codope { get; set; }

        public bool Filial { get; set; }
        public bool Setor { get; set; }
        public bool Matricula { get; set; }

        public bool Nome { get; set; }
        public bool Cpf { get; set; }
        public bool DataNascimento { get; set; }
        public bool Sexo { get; set; }
        public bool Naturalidade { get; set; }
        public bool Nacionalidade { get; set; }
        public bool Rg { get; set; }
        public bool OrgaoExpedidor { get; set; }
        public bool Mae { get; set; }
        public bool Pai { get; set; }
        public bool CepEnderecoResidencial { get; set; }
        public bool Logradouro { get; set; }
        public bool NumeroEnderecoResidencial { get; set; }
        public bool ComplementoEnderecoResidencial { get; set; }
        public bool BairroEnderecoResidencial { get; set; }
        public bool CidadeEnderecoResidencial { get; set; }
        public bool Uf { get; set; }

        public bool CepEnderecoComercial { get; set; }
        public bool LogradouroEnderecoComercial { get; set; }
        public bool NumeroEnderecoComercial { get; set; }
        public bool ComplementoEnderecoComercial { get; set; }
        public bool BairroEnderecoComercial { get; set; }
        public bool CidadeEnderecoComercial { get; set; }
        public bool UfEnderecoComercial { get; set; }

        public bool Celular { get; set; }
        public bool Telefone { get; set; }
        public bool Email { get; set; }

        public CamposObrigatoriosUsuario BuscarCamposObrigatoriosUsuario(IObjetoConexao objConn, int sistema, string acesso)
        {
            acesso = acesso == "parceria" ? "cliente" : acesso;
            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand comando = new SqlCommand("SELECT * FROM CAMOBGCADUSU WITH (NOLOCK) WHERE ACESSO = '" + acesso + "' AND SISTEMA = " + sistema.ToString(), conexao);

            DataTable dt = new DataTable();

            try
            {
                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                if (dt.Rows.Count == 1)
                {
                    Filial = dt.Rows[0]["FILIAL"].ToString() == "S" ? true : false;
                    Setor = dt.Rows[0]["SETOR"].ToString() == "S" ? true : false;
                    Matricula = dt.Rows[0]["MATRICULA"].ToString() == "S" ? true : false;

                    Nome = dt.Rows[0]["NOME"].ToString() == "S" ? true : false;
                    Cpf = dt.Rows[0]["CPF"].ToString() == "S" ? true : false;
                    DataNascimento = dt.Rows[0]["DTNASC"].ToString() == "S" ? true : false;
                    Sexo = dt.Rows[0]["SEXO"].ToString() == "S" ? true : false;
                    Naturalidade = dt.Rows[0]["NATURALIDADE"].ToString() == "S" ? true : false;
                    Nacionalidade = dt.Rows[0]["NACIONALIDADE"].ToString() == "S" ? true : false;
                    Rg = dt.Rows[0]["RG"].ToString() == "S" ? true : false;
                    OrgaoExpedidor = dt.Rows[0]["ORGEXP"].ToString() == "S" ? true : false;
                    Mae = dt.Rows[0]["MAE"].ToString() == "S" ? true : false;
                    Pai = dt.Rows[0]["PAI"].ToString() == "S" ? true : false;
                    
                    CepEnderecoResidencial = dt.Rows[0]["CEPRES"].ToString() == "S" ? true : false;
                    Logradouro = dt.Rows[0]["LOGRADOURORES"].ToString() == "S" ? true : false;
                    NumeroEnderecoResidencial = dt.Rows[0]["NUMRES"].ToString() == "S" ? true : false;
                    ComplementoEnderecoResidencial = dt.Rows[0]["COMPRES"].ToString() == "S" ? true : false;
                    BairroEnderecoResidencial = dt.Rows[0]["BAIRRORES"].ToString() == "S" ? true : false;
                    CidadeEnderecoResidencial = dt.Rows[0]["CIDADERES"].ToString() == "S" ? true : false;
                    Uf = dt.Rows[0]["UFRES"].ToString() == "S" ? true : false;

                    CepEnderecoComercial = dt.Rows[0]["CEPCOM"].ToString() == "S" ? true : false;
                    LogradouroEnderecoComercial = dt.Rows[0]["LOGRADOUROCOM"].ToString() == "S" ? true : false;
                    NumeroEnderecoComercial = dt.Rows[0]["NUMCOM"].ToString() == "S" ? true : false;
                    ComplementoEnderecoComercial = dt.Rows[0]["COMPCOM"].ToString() == "S" ? true : false;
                    BairroEnderecoComercial = dt.Rows[0]["BAIRROCOM"].ToString() == "S" ? true : false;
                    CidadeEnderecoComercial = dt.Rows[0]["CIDADECOM"].ToString() == "S" ? true : false;
                    UfEnderecoComercial = dt.Rows[0]["UFCOM"].ToString() == "S" ? true : false;

                    Celular = dt.Rows[0]["CELULAR"].ToString() == "S" ? true : false;
                    Telefone = dt.Rows[0]["TELEFONE"].ToString() == "S" ? true : false;
                    Email = dt.Rows[0]["EMAIL"].ToString() == "S" ? true : false;                    

                    return this;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
