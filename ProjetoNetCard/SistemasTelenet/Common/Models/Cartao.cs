using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NetCard.Common.Models
{
    public class Cartao
    {
        [Display(Name = "Matrícula, nome ou letras iniciais, ou CPF")]
        public string Filtro { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Data Inicial")]
        public string DtInicial { get; set; }
        [Display(Name = "Data Final")]
        public string DtFinal { get; set; }
        public int Id { get; set; }
        public int Id_Usuario { get; set; }
        public string CodCrt { get; set; }
        public string CodCrtMask { get { return CodCrt != null ? Utils.MascaraCartao(CodCrt, 17) : string.Empty; } }
        [Display(Name = "CPF")]
        public string Cpf { get; set; }
        [Display(Name = "Nome")]
        [StringLength(50)]
        public string Nome { get; set; }
        public DateTime DataInc { get; set; }
        public DateTime DataStatus { get; set; }
        [Display(Name = "Número Dependente")]
        public string Td { get; set; }
        [Display(Name = "Nome Dependente")]
        public string NomDep { get; set; }
        public string NumDep { get; set; }
        [Display(Name = "Parentesco")]
        public string Parentesco { get; set; }
        [Display(Name = "Justificativa")]
        public string Justificativa { get; set; }
        public List<Dependente> Dependentes { get; set; }
        public decimal ValCobSegVia { get; set; }
        [StringLength(4)]
        [Display(Name = "Filial")]
        public string Filial { get; set; }
        [Display(Name = "Setor")]
        [StringLength(10)]
        public string Setor { get; set; }
        [StringLength(10)]
        [Display(Name = "Matricula")]
        public string Matricula { get; set; }
        [Display(Name = "Sindicato")]
        public string CodSind { get; set; }
        [Display(Name = "Sindicalizado")]
        public bool Sindicalizado { get; set; }
        public decimal TaxaSind { get; set; }
        [Display(Name = "Celular")]
        public string Cel { get; set; }
        [Display(Name = "Limite")]
        public decimal Limite { get; set; }
        [Display(Name = "Limite Dependente")]
        public string LimDep { get; set; }
        public decimal Premio { get; set; }
        public decimal Bonus { get; set; }
        public int NumMaxParc { get; set; }
        public DateTime DtRenov { get; set; }
        [Display(Name = "Logradouro")]
        public string Logradouro { get; set; }
        [Display(Name = "Bairro")]
        public string Bairro { get; set; }
        [Display(Name = "Complemento")]
        public string Compl { get; set; }
        [Display(Name = "Localidade")]
        public string Localidade { get; set; }
        [Display(Name = "UF")]
        public string Uf { get; set; }
        [Display(Name = "Telefone")]
        public string Tel { get; set; }
        [Display(Name = "RG")]
        public string Rg { get; set; }
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }
        [Display(Name = "Estado Civil")]
        public string Civil { get; set; }
        [Display(Name = "Profissão")]
        public string Profissao { get; set; }
        [Display(Name = "Telefone Comercial")]
        public string TelComercial { get; set; }
        [Display(Name = "e-mail")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail não é válido.")]
        public string Email { get; set; }
        [Display(Name = "Nascimento")]
        public string DataNascimento { get; set; }
        [Display(Name = "Mãe")]
        public string Mae { get; set; }
        [Display(Name = "Pai")]
        public string Pai { get; set; }
        [Display(Name = "Naturalidade")]
        public string Naturalidade { get; set; }
        [Display(Name = "Nacionalidade")]
        public string Nacionalidade { get; set; }
        [Display(Name = "Orgão Expedidor")]
        public string OrgaoExpedidor { get; set; }
        [Display(Name = "Logradouro")]
        public string LogradouroEnderecoResidencial { get; set; }
        [Display(Name = "Nº")]
        public string NumeroEnderecoResidencial { get; set; }
        [Display(Name = "Complemento")]
        public string ComplementoEnderecoResidencial { get; set; }
        [Display(Name = "Bairro")]
        public string BairroEnderecoResidencial { get; set; }
        [Display(Name = "Cidade")]
        public string CidadeEnderecoResidencial { get; set; }
        [Display(Name = "UF")]
        public string UfEnderecoResidencial { get; set; }
        [Display(Name = "CEP")]
        public string CepEnderecoResidencial { get; set; }
        [Display(Name = "Logradouro")]
        public string LogradouroEnderecoComercial { get; set; }
        [Display(Name = "Nº")]
        public string NumeroEnderecoComercial { get; set; }
        [Display(Name = "Complemento")]
        public string ComplementoEnderecoComercial { get; set; }
        [Display(Name = "Bairro")]
        public string BairroEnderecoComercial { get; set; }
        [Display(Name = "Cidade")]
        public string CidadeEnderecoComercial { get; set; }
        [Display(Name = "UF")]
        public string UfEnderecoComercial { get; set; }
        [Display(Name = "CEP")]
        public string CepEnderecoComercial { get; set; }
        public List<Uf> ListaUf { get; set; }
        public bool ExibeMensagem { get; set; }
        public string MensagemAExibir { get; set; }
        public bool TrocaAba { get; set; }
        public decimal Saldo { get; set; }
        public decimal ValorCarga { get; set; }
        public string CentroCusto { get; set; }
        public string CodOpe { get; set; }
        public string GerCRT { get; set; }
        public List<Acao> ListaAcoes { get; set; }

        public bool isAtivo()
        {
            return Status.Equals("00", StringComparison.InvariantCultureIgnoreCase);
        }

        public bool isDependente()
        {
            return Td.Equals("Dependente", StringComparison.InvariantCultureIgnoreCase);
        }

        public bool isBloqueado()
        {
            return Status.Equals("01", StringComparison.InvariantCultureIgnoreCase);
        }

        public bool isCancelado()
        {
            return Status.Equals("02", StringComparison.InvariantCultureIgnoreCase);
        }
        public bool isSuspenso()
        {
            return Status.Equals("06", StringComparison.InvariantCultureIgnoreCase);
        }
        public bool isTransferido()
        {
            return Status.Equals("07", StringComparison.InvariantCultureIgnoreCase);
        }

        public List<Cartao> ListagemCartoes(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_LISTCARTOES";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "STATUS", DbType.String, Status);
            db.AddInParameter(cmd, "NOME", DbType.String, Utils.RetirarCaracteres(".-", Filtro));
            db.AddInParameter(cmd, "FORMATO", DbType.Int32, 2);
            IDataReader dr = null;

            var listaResult = new List<Cartao>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var cartaoUsu = new Cartao();
                    cartaoUsu.CodCrt = (dr["CARTAO"]).ToString().Trim();
                    cartaoUsu.Cpf = (dr["CPF"]).ToString().Trim();
                    cartaoUsu.Nome = (dr["NOME"]).ToString().Trim();
                    cartaoUsu.DataStatus = Convert.ToDateTime(dr["DATA_STATUS"]);
                    cartaoUsu.DataInc = Convert.ToDateTime(dr["DATINC"]);
                    cartaoUsu.Status = (dr["DESC_STATUS"]).ToString().Trim();
                    cartaoUsu.Td = Convert.ToInt16(dr["TD"]) == 0 ? "TITULAR" : "DEPENDENTE";
                    cartaoUsu.NumDep = Convert.ToString(dr["TD"]);
                    if (dadosAcesso.TipProd == 8)
                    {
                        cartaoUsu.CodSind = (dr["SINDICATO"]).ToString().Trim();
                        cartaoUsu.Sindicalizado = Convert.ToString(dr["SINDICALIZADO"]) == "S";
                        cartaoUsu.TaxaSind = Convert.ToDecimal(dr["TAXA"]);
                    }
                    listaResult.Add(cartaoUsu);
                }
                dr.Close();
                retorno = listaResult.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch (Exception)
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return listaResult;
        }

        public List<Cartao> ListarCartoesBloqueados(ObjConn objConn, DadosAcesso dadosAcesso, DateTime dataInicio, DateTime dataFim)
        {
            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand comando = new SqlCommand("MW_BUSCA_CART_BLOQ", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            string dtInicial = dataInicio.Year.ToString() + dataInicio.Month.ToString().PadLeft(2, '0') + dataInicio.Day.ToString().PadLeft(2, '0');
            string dtFinal = dataFim.Year.ToString() + dataFim.Month.ToString().PadLeft(2, '0') + dataFim.Day.ToString().PadLeft(2, '0');

            comando.Parameters.Add(new SqlParameter() { ParameterName = "@SISTEMA", SqlDbType = System.Data.SqlDbType.Int, Value = dadosAcesso.Sistema.cartaoPJVA });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@CODCLI", SqlDbType = System.Data.SqlDbType.Int, Value = dadosAcesso.Codigo });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@DATAINI", SqlDbType = System.Data.SqlDbType.VarChar, Value = dtInicial });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@DATAFIM", SqlDbType = System.Data.SqlDbType.VarChar, Value = dtFinal });

            var listaResult = new List<Cartao>();

            try
            {
                DataTable dt = new DataTable();

                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Cartao cart = new Cartao();
                        cart.CodCrt = item["CODCRT"].ToString();
                        cart.Cpf = item["CPF"].ToString();
                        cart.NumDep = item["NUMDEP"].ToString();
                        cart.Nome = item["NOMUSU"].ToString();
                        cart.Filial = item["CODFIL"].ToString();
                        cart.Matricula = item["MAT"].ToString();
                        cart.DataStatus = Convert.ToDateTime(item["DATSTA"].ToString());
                        cart.DataInc = Convert.ToDateTime(item["DATINC"].ToString());

                        listaResult.Add(cart);
                    }
                }

                return listaResult;
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                return null;
            }

        }

        public List<Cartao> ListagemInclusoes(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_LISTCARTINC";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo),
                               new Parametro("@DATA_INI", DbType.String, Convert.ToDateTime(DtInicial).ToString("yyyyMMdd")),
                               new Parametro("@DATA_FIM", DbType.String, Convert.ToDateTime(DtFinal).ToString("yyyyMMdd")),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaResult = new List<Cartao>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var cartaoUsu = new Cartao();
                    cartaoUsu.CodCrt = (dr["CARTAO"]).ToString();
                    cartaoUsu.Cpf = (dr["CPF"]).ToString();
                    cartaoUsu.Nome = (dr["NOME"]).ToString();
                    cartaoUsu.DataInc = Convert.ToDateTime(dr["DATINC"]);
                    cartaoUsu.Td = Convert.ToInt16(dr["TD"]) == 0 ? "TITULAR" : "DEPENDENTE";
                    cartaoUsu.DataStatus = Convert.ToDateTime(dr["DATA_STATUS"]);
                    cartaoUsu.Matricula = (dr["MATRICULA"]).ToString();
                    cartaoUsu.Status = (dr["STATUS"]).ToString();
                    cartaoUsu.Limite = dadosAcesso.Sistema.cartaoPJVA == 0 ? Convert.ToDecimal(dr["LIMITE"]) : 0;
                    listaResult.Add(cartaoUsu);
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

        public List<Cartao> ListagemCartoesSegundaVia(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_LISTCARTSEGVIA";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo),
                               new Parametro("@DATA_INI", DbType.String, Convert.ToDateTime(DtInicial).ToString("yyyyMMdd")),
                               new Parametro("@DATA_FIM", DbType.String, Convert.ToDateTime(DtFinal).ToString("yyyyMMdd")),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            var listaResult = new List<Cartao>();
            IDataReader dr = null;

            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var cartaoUsu = new Cartao();
                    cartaoUsu.CodCrt = (dr["CARTAO"]).ToString();
                    cartaoUsu.Cpf = (dr["CPF"]).ToString();
                    cartaoUsu.Nome = (dr["NOME"]).ToString();
                    cartaoUsu.DataInc = Convert.ToDateTime(dr["DATA"]);
                    cartaoUsu.Td = Convert.ToInt16(dr["TD"]) == 0 ? "TITULAR" : "DEPENDENTE";
                    cartaoUsu.ValCobSegVia = Convert.ToDecimal(dr["VALOR"]);
                    cartaoUsu.Matricula = (dr["MATRICULA"]).ToString();
                    cartaoUsu.CodOpe = (dr["CODOPE"]).ToString();
                    listaResult.Add(cartaoUsu);
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

        public void DesbloqueiaListaCartoes(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_ALTERA_STATUS_CARTAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            db.AddInParameter(cmd, "@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "@ACAO", DbType.String, "D");
            db.AddInParameter(cmd, "@CODCLI", DbType.String, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "@CPF", DbType.String, Cpf.PadLeft(11, '0'));
            db.AddInParameter(cmd, "@NUMDEP", DbType.String, NumDep);
            db.AddInParameter(cmd, "@IDOPEMW", DbType.Int32, dadosAcesso.Id);
            IDataReader dr = null;
            var listaResult = new List<Cartao>();

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    retorno = dr["RETORNO"].ToString();
                    retorno = retorno != Constantes.ok ? dr["MENSAGEM"].ToString() : retorno;
                }
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }

        }

        public void BloqueiaListaCartoes(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_ALTERA_STATUS_CARTAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            db.AddInParameter(cmd, "@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "@ACAO", DbType.String, "B");
            db.AddInParameter(cmd, "@CODCLI", DbType.String, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "@CPF", DbType.String, Cpf.PadLeft(11, '0'));
            db.AddInParameter(cmd, "@NUMDEP", DbType.String, NumDep);
            db.AddInParameter(cmd, "@IDOPEMW", DbType.Int32, dadosAcesso.Id);
            IDataReader dr = null;
            var listaResult = new List<Cartao>();

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    retorno = dr["RETORNO"].ToString();
                    retorno = retorno != Constantes.ok ? dr["MENSAGEM"].ToString() : retorno;
                }
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
        }

        public Cartao InclusaoCartao(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_INSERE_CARTAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>();
            list.Add(new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA));
            list.Add(new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo));
            list.Add(new Parametro("@CODOPE", DbType.Int32, 0));
            list.Add(new Parametro("@IDOPEWEB", DbType.Int32, dadosAcesso.Id));
            list.Add(new Parametro("@CPF", DbType.String, Utils.RetirarCaracteres("-.", Cpf)));
            list.Add(new Parametro("@NUMDEP", DbType.Int32, 0));
            list.Add(new Parametro("@CODFIL", DbType.Int32, Convert.ToInt32(Filial)));
            list.Add(new Parametro("@NOMUSU", DbType.String, Utils.RemoverAcentos(Nome)));
            list.Add(new Parametro("@DATINC", DbType.String, DateTime.Now.ToString("yyyyMMdd")));
            list.Add(new Parametro("@CODSET", DbType.String, Setor));
            list.Add(new Parametro("@MAT", DbType.String, Matricula));
            list.Add(new Parametro("@AUTORIZADOR", DbType.String, objConn.BancoAutorizador));

            if (!string.IsNullOrEmpty(DataNascimento))
                list.Add(new Parametro("@DATNAS", DbType.String, Convert.ToDateTime(DataNascimento).ToString("yyyyMMdd")));

            list.Add(new Parametro("@CODSIND", DbType.Int32, Convert.ToInt32(CodSind)));
            list.Add(new Parametro("@SINDICALIZADO", DbType.String, Sindicalizado ? "S" : "N"));
            list.Add(new Parametro("@PAI", DbType.String, Pai));
            list.Add(new Parametro("@MAE", DbType.String, Mae));
            list.Add(new Parametro("@RG", DbType.String, Rg));
            list.Add(new Parametro("@ORGEXPRG", DbType.String, OrgaoExpedidor));
            list.Add(new Parametro("@NATURALIDADE", DbType.String, Naturalidade));
            list.Add(new Parametro("@NACIONALIDADE", DbType.String, Nacionalidade));
            list.Add(new Parametro("@ENDUSU", DbType.String, LogradouroEnderecoResidencial));

            if (NumeroEnderecoResidencial != null)
            {
                int aux;
                if (Int32.TryParse(NumeroEnderecoResidencial, out aux))
                    list.Add(new Parametro("@ENDNUMUSU", DbType.Int32, aux));
            }

            list.Add(new Parametro("@ENDCPL", DbType.String, ComplementoEnderecoResidencial));

            if (!string.IsNullOrEmpty(BairroEnderecoResidencial))
                list.Add(new Parametro("@BAIRRO", DbType.String, Utils.RemoverAcentos(BairroEnderecoResidencial)));
            else
                list.Add(new Parametro("@BAIRRO", DbType.String, null));

            if (!string.IsNullOrEmpty(CidadeEnderecoResidencial))
                list.Add(new Parametro("@LOCALIDADE", DbType.String, Utils.RemoverAcentos(CidadeEnderecoResidencial)));
            else
                list.Add(new Parametro("@LOCALIDADE", DbType.String, null));

            list.Add(new Parametro("@UF", DbType.String, UfEnderecoResidencial));
            if (CepEnderecoResidencial != null)
                list.Add(new Parametro("@CEP", DbType.String, Utils.RetirarCaracteres("-. ", CepEnderecoResidencial)));
            list.Add(new Parametro("@ENDUSUCOM", DbType.String, LogradouroEnderecoComercial));

            if (NumeroEnderecoComercial != null)
            {
                int aux;
                if (Int32.TryParse(NumeroEnderecoComercial, out aux))
                    list.Add(new Parametro("@ENDNUMCOM", DbType.Int32, aux));
            }

            list.Add(new Parametro("@ENDCPLCOM", DbType.String, ComplementoEnderecoComercial));


            if (!string.IsNullOrEmpty(BairroEnderecoComercial))
                list.Add(new Parametro("@BAIRROCOM", DbType.String, Utils.RemoverAcentos(BairroEnderecoComercial)));
            else
                list.Add(new Parametro("@BAIRROCOM", DbType.String, null));

            if (!string.IsNullOrEmpty(CidadeEnderecoComercial))
                list.Add(new Parametro("@LOCALIDADECOM", DbType.String, Utils.RemoverAcentos(CidadeEnderecoComercial)));
            else
                list.Add(new Parametro("@LOCALIDADECOM", DbType.String, null));


            list.Add(new Parametro("@UFCOM", DbType.String, UfEnderecoComercial));
            if (CepEnderecoComercial != null)
                list.Add(new Parametro("@CEPCOM", DbType.String, Utils.RetirarCaracteres("-. ", CepEnderecoComercial)));
            list.Add(new Parametro("@TEL", DbType.String, Utils.RetirarCaracteres("-.,() ", Tel)));
            list.Add(new Parametro("@CEL", DbType.String, Utils.RetirarCaracteres("-.,() ", Cel)));
            list.Add(new Parametro("@SEXO", DbType.String, Sexo));
            list.Add(new Parametro("@EMA", DbType.String, Email));

            if (TelComercial != null) list.Add(new Parametro("@TELCOM", DbType.String, Utils.RetirarCaracteres("-.,() ", TelComercial)));
            if (dadosAcesso.Sistema.cartaoPJVA == 0)
            {
                list.Add(new Parametro("@LIMPAD", DbType.Decimal, Limite));
                list.Add(new Parametro("@NUMMAXPARC", DbType.Decimal, NumMaxParc));
            }

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaResult = new List<Cartao>();

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    retorno = dr["RETORNO"].ToString();
                    retorno = retorno != Constantes.ok ? dr["MENSAGEM"].ToString() : retorno;
                }
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return this;
        }

        public Cartao AlterarCartao(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_ALTERA_CARTAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>();
            
            list.Add(new Parametro("@IDOPEWEB", DbType.Int32, dadosAcesso.Id));
            list.Add(new Parametro("@SISTEMA", DbType.Int16, dadosAcesso.Sistema.cartaoPJVA));
            list.Add(new Parametro("@ID_USUARIO", DbType.Int32, Id_Usuario));
            list.Add(new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo));
            list.Add(new Parametro("@NOMUSU", DbType.String, Nome));
            list.Add(new Parametro("@CPF", DbType.String, dadosAcesso.Cpf));
            if (!string.IsNullOrEmpty(DataNascimento))
                list.Add(new Parametro("@DATNAS", DbType.String, Convert.ToDateTime(DataNascimento).ToString("yyyyMMdd")));
            if (Tel != null) list.Add(new Parametro("@TEL", DbType.String, Utils.RetirarCaracteres("-.,() ", Tel)));
            if (Cel != null) list.Add(new Parametro("@CEL", DbType.String, Utils.RetirarCaracteres("-.,() ", Cel)));
            list.Add(new Parametro("@SEXO", DbType.String, Sexo));
            list.Add(new Parametro("@EMA", DbType.String, Email));
            list.Add(new Parametro("@PAI", DbType.String, Pai));
            list.Add(new Parametro("@MAE", DbType.String, Mae));
            list.Add(new Parametro("@RG", DbType.String, Rg));
            list.Add(new Parametro("@ORGEXPRG", DbType.String, OrgaoExpedidor));
            list.Add(new Parametro("@NATURALIDADE", DbType.String, Naturalidade));
            list.Add(new Parametro("@NACIONALIDADE", DbType.String, Nacionalidade));
            list.Add(new Parametro("@ENDUSU", DbType.String, LogradouroEnderecoResidencial));
            list.Add(new Parametro("@ENDNUMUSU", DbType.Int32, NumeroEnderecoResidencial));
            list.Add(new Parametro("@ENDCPL", DbType.String, ComplementoEnderecoResidencial));
            list.Add(new Parametro("@BAIRRO", DbType.String, BairroEnderecoResidencial == null ? "" : Utils.RemoverAcentos(BairroEnderecoResidencial)));
            list.Add(new Parametro("@LOCALIDADE", DbType.String, CidadeEnderecoResidencial == null ? "" : Utils.RemoverAcentos(CidadeEnderecoResidencial)));
            list.Add(new Parametro("@UF", DbType.String, UfEnderecoResidencial));
            if (CepEnderecoResidencial != null) list.Add(new Parametro("@CEP", DbType.String, CepEnderecoResidencial == null ? "" : Utils.RetirarCaracteres(".- ", CepEnderecoResidencial)));
            list.Add(new Parametro("@ENDUSUCOM", DbType.String, LogradouroEnderecoComercial));
            list.Add(new Parametro("@ENDNUMCOM", DbType.Int32, NumeroEnderecoComercial));
            list.Add(new Parametro("@ENDCPLCOM", DbType.String, ComplementoEnderecoComercial));
            list.Add(new Parametro("@BAIRROCOM", DbType.String, BairroEnderecoComercial == null ? "" : Utils.RemoverAcentos(BairroEnderecoComercial)));
            list.Add(new Parametro("@LOCALIDADECOM", DbType.String, CidadeEnderecoComercial == null ? "" : Utils.RemoverAcentos(CidadeEnderecoComercial)));
            list.Add(new Parametro("@UFCOM", DbType.String, UfEnderecoComercial));
            if (CepEnderecoComercial != null) list.Add(new Parametro("@CEPCOM", DbType.String, Utils.RetirarCaracteres(".- ", CepEnderecoComercial)));
            if (TelComercial != null) list.Add(new Parametro("@TELCOM", DbType.String, Utils.RetirarCaracteres("-.,() ", TelComercial)));
            if (Matricula != null) list.Add(new Parametro("@MAT", DbType.String, Utils.RetirarCaracteres("-.,() ", Matricula)));
            if (Setor != null) list.Add(new Parametro("@CODSET", DbType.String, Utils.RetirarCaracteres("-.,() ", Setor)));
            if (Filial != null) list.Add(new Parametro("@CODFIL", DbType.Int16, Utils.RetirarCaracteres("-.,() ", Filial == string.Empty ? "0" : Filial)));

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaResult = new List<Cartao>();

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    retorno = dr["RETORNO"].ToString();
                    retorno = retorno != Constantes.ok ? dr["MENSAGEM"].ToString() : retorno;
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return this;
        }

        public Cartao GetDadosCartao(ObjConn objConn, DadosAcesso dadosAcesso, string codigoCliente, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_GET_DADOSCARTAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>();
            list.Add(new Parametro("@CPF", DbType.String, dadosAcesso.Cpf));
            list.Add(new Parametro("@TIPO_CARTAO", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA));
            list.Add(new Parametro("@TIPO_ACESSO", DbType.Int32, dadosAcesso.Acesso == Constantes.usuario ? 0 : 1));
            list.Add(new Parametro("@CODCLI", DbType.Int32, codigoCliente));

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaResult = new List<Cartao>();

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    Id_Usuario = Convert.ToInt32(dr["ID_USUARIO"]);
                    Nome = dadosAcesso.Acesso == Constantes.usuario ? dadosAcesso.Nome : Convert.ToString(dr["NOME"]);
                    Cpf = dadosAcesso.Cpf;
                    DataNascimento = dr["DATNAS"].ToString();
                    Pai = dr["PAI"].ToString().TrimEnd();
                    Mae = dr["MAE"].ToString().TrimEnd();
                    Rg = dr["RG"].ToString().TrimEnd();
                    Cel = dr["CEL"].ToString();
                    Tel = dr["TEL"].ToString();
                    Sexo = dr["SEXO"].ToString();
                    Email = dr["EMA"].ToString().TrimEnd();
                    OrgaoExpedidor = dr["ORGEXPRG"].ToString().TrimEnd();
                    Naturalidade = dr["NATURALIDADE"].ToString().TrimEnd();
                    Nacionalidade = dr["NACIONALIDADE"].ToString().TrimEnd();
                    LogradouroEnderecoResidencial = dr["ENDUSU"].ToString().TrimEnd();
                    NumeroEnderecoResidencial = dr["ENDNUMUSU"].ToString().TrimEnd();
                    ComplementoEnderecoResidencial = dr["ENDCPL"].ToString().TrimEnd();
                    BairroEnderecoResidencial = dr["BAIRRO"].ToString().TrimEnd();
                    CidadeEnderecoResidencial = dr["LOCALIDADE"].ToString().TrimEnd();
                    UfEnderecoResidencial = dr["UF"].ToString().TrimEnd();
                    CepEnderecoResidencial = dr["CEP"].ToString().TrimEnd();
                    LogradouroEnderecoComercial = dr["ENDUSUCOM"].ToString().TrimEnd();
                    NumeroEnderecoComercial = dr["ENDNUMCOM"].ToString().TrimEnd();
                    ComplementoEnderecoComercial = dr["ENDCPLCOM"].ToString().TrimEnd();
                    BairroEnderecoComercial = dr["BAIRROCOM"].ToString().TrimEnd();
                    CidadeEnderecoComercial = dr["LOCALIDADECOM"].ToString().TrimEnd();
                    UfEnderecoComercial = dr["UFCOM"].ToString().TrimEnd();
                    CepEnderecoComercial = dr["CEPCOM"].ToString().TrimEnd();
                    TelComercial = dr["TELCOM"].ToString();
                    Matricula = dr["MAT"].ToString().TrimEnd();
                    Filial = dr["CODFIL"].ToString().TrimEnd();
                    Setor = dr["CODSET"].ToString().TrimEnd();
                    Status = dr["STA"].ToString();
                    retorno = Constantes.ok;
                }
                else
                {
                    Nome = dadosAcesso.Nome;
                    Cpf = dadosAcesso.Cpf;
                    retorno = Constantes.ok;
                }
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return this;
        }

        public bool ConsDadosAtual(ObjConn objConn, DadosAcesso dadosAcesso)
        {
            var dadosAtualizados = false;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_CONSDADOSCAD";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>();
            list.Add(new Parametro("@CPF", DbType.String, dadosAcesso.Cpf));

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaResult = new List<Cartao>();

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    dadosAtualizados = Convert.ToString(dr["RETORNO"]) == Constantes.ok;
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return dadosAtualizados;
        }

        public Cartao InclusaoDependente(ObjConn objConn, DadosAcesso dadosAcesso, Dependente dependente, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_INSERE_CARTAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>();
            list.Add(new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA));
            list.Add(new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo));
            list.Add(new Parametro("@CODOPE", DbType.Int32, 0));
            list.Add(new Parametro("@IDOPEWEB", DbType.Int32, dadosAcesso.Id));
            list.Add(new Parametro("@CPF", DbType.String, Utils.RetirarCaracteres("-.", dependente.CpfTit)));
            list.Add(new Parametro("@NUMDEP", DbType.Int32, dependente.NumDep));
            list.Add(new Parametro("@LIMDEP", DbType.Decimal, dependente.LimDep));
            list.Add(new Parametro("@NOMUSU", DbType.String, Utils.RemoverAcentos(dependente.NomeDep.ToUpper())));
            list.Add(new Parametro("@CODPAR", DbType.String, dependente.CodPar));
            list.Add(new Parametro("@DATINC", DbType.String, DateTime.Now.ToString("yyyyMMdd")));
            list.Add(new Parametro("@DATNAS", DbType.DateTime, dependente.DtNascimento));
            list.Add(new Parametro("@SEXO", DbType.String, dependente.Sexo));


            list.Add(new Parametro("@CODSET", DbType.String, 0));
            list.Add(new Parametro("@CODFIL", DbType.Int32, 0));
            list.Add(new Parametro("@MAT", DbType.String, 0));
            list.Add(new Parametro("@LIMPAD", DbType.Decimal, 0));


            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaResult = new List<Cartao>();

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    //retorno = dr["RETORNO"].ToString(); não retorna coluna RETORNO
                    retorno = "OK";
                    retorno = retorno != Constantes.ok ? dr["MENSAGEM"].ToString() : retorno;
                }
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return this;
        }

        public Cartao AlteracaoDependente(ObjConn objConn, DadosAcesso dadosAcesso, Dependente dependente, out string retorno)
        {
            retorno = string.Empty;
            string tabela = "";
            tabela = dadosAcesso.Sistema.cartaoPJVA == 0 ? "USUARIO " : "USUARIOVA ";
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            string sql = "UPDATE " + tabela + " SET SEXO = '" + dependente.Sexo + "', DATNAS = '" + Convert.ToDateTime(dependente.DtNascimento).ToString("yyyyMMdd") + "' WHERE CPF = '" + Utils.RetirarCaracteres("-.", dependente.CpfTit) + "' AND NUMDEP = " + dependente.NumDep + " AND CODCLI = " + dadosAcesso.Codigo.ToString();
            var cmd = db.GetSqlStringCommand(sql.ToString());


            IDataReader dr = null;
            var listaResult = new List<Cartao>();

            try
            {
                dr = db.ExecuteReader(cmd);
                retorno = "OK";
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return this;
        }

        public List<Cartao> ListaCartaoTitularDependente(ObjConn objConn, DadosAcesso dadosAcesso, Permissao permissao, out string retorno)
        {
            retorno = string.Empty;
            var cpf = Utils.RetirarCaracteres(".-", Cpf);
            var cid = 0;
            string TipoAcesso = (dadosAcesso.Acesso == "cliente") ? "C" : "U";
            var listaCartao = new List<Cartao>();
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_CONSSALDO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var listPar = new List<Parametro>
                {
                    new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                    new Parametro("@FILTRO", DbType.String, cpf),
                    new Parametro("@CODEMPRESA", DbType.Int32, dadosAcesso.Codigo),
                    new Parametro("@TIPOACESSO", DbType.String, TipoAcesso)
                };
            foreach (var parametro in listPar)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var titularAtivo = true;
                    var saldo = 0m;

                    var cartaoUsu = new Cartao();
                    cartaoUsu.Cpf = cpf;
                    cartaoUsu.CodCrt = (string)dr["CODCRT"];
                    cartaoUsu.Nome = (string)dr["NOME"];
                    cartaoUsu.Status = (string)dr["STATUSU"];
                    cartaoUsu.Td = Convert.ToString(dr["NUMDEPEND"]) == "00" ? "TITULAR" : "DEPENDENTE";
                    cartaoUsu.DataStatus = (DateTime)dr["DATASTATUS"];
                    cartaoUsu.Id = cid;
                    cartaoUsu.NumDep = Convert.ToString(dr["NUMDEPEND"]);
                    cartaoUsu.GerCRT = Convert.ToString(dr["GERCRT"]);
                    saldo = Convert.ToDecimal(dr["SALDO"]);
                    cartaoUsu.Saldo = saldo;
                    var id = Convert.ToString(cartaoUsu.Id);
                    cid++;

                    var list = new List<Acao>();
                    if (cartaoUsu.isAtivo())
                    {
                        if (permissao.FBLOQCART == "S")
                            list.Add(new Acao("Bloquear", "Confirma bloqueio do cartão " + cartaoUsu.CodCrtMask + " ?", cpf, id));
                        if (permissao.FCANCCART == "S")
                        {
                            if(cartaoUsu.GerCRT == "X")
                            {
                                list.Add(new Acao("Cancelar", "O cartão com final " + cartaoUsu.CodCrtMask.Substring(cartaoUsu.CodCrtMask.Length - 4, 4) + 
                                    " ainda não foi impresso para o usuário " + cartaoUsu.Nome + ". " +
                                    "\\nDeseja realmente cancelar? ", cpf, id));
                            }
                            else
                            {
                                list.Add(new Acao("Cancelar", "Confirma o cancelamento do cartão com final " + cartaoUsu.CodCrtMask.Substring(cartaoUsu.CodCrtMask.Length - 4, 4) + 
                                    " do usuário " + cartaoUsu.Nome + "? ", cpf, id));
                            }

                            var habSuspesao = new MenuItem().HabilitaSuspender(objConn);
                            if (dadosAcesso.Sistema.cartaoPJVA == 1 && habSuspesao)
                            {
                                if (cartaoUsu.GerCRT == "X")
                                {
                                    list.Add(new Acao("Suspender", "O cartão com final " + cartaoUsu.CodCrtMask.Substring(cartaoUsu.CodCrtMask.Length - 4, 4) +
                                        " ainda não foi impresso para o usuário " + cartaoUsu.Nome + ". " +
                                        "\\nDeseja realmente suspender? ", cpf, id));
                                }
                                else
                                {
                                    list.Add(new Acao("Suspender", "Confirma a suspensão do cartão com final " + cartaoUsu.CodCrtMask.Substring(cartaoUsu.CodCrtMask.Length - 4, 4) +
                                        " do usuário " + cartaoUsu.Nome + "? ", cpf, id));
                                }
                            }
                        }
                        if (permissao.FSEGVIACART == "S")
                                list.Add(new Acao("Segunda via", "Confirma segunda via do cartão " + cartaoUsu.CodCrtMask + " ?", cpf, id));

                        var eCrediHabita = new MenuItem().HabilitaCrediHabita(objConn, dadosAcesso.TipProd);
                        if (dadosAcesso.Sistema.cartaoPJVA == 0 && !cartaoUsu.isDependente())
                        {
                            if (permissao.FALTLIMITE == "S" && !eCrediHabita)
                            {
                                list.Add(new Acao("Alterar Limite Convênio", null, cpf, id));
                                if (dadosAcesso.HabMaxParc == "S")
                                    list.Add(new Acao("Alterar N° Max de Parcelas", null, cpf, id));
                                if (dadosAcesso.HabPremio == "S" && dadosAcesso.Conpmo == "S")
                                    list.Add(new Acao("Alterar Prêmio", null, cpf, id));
                            }
                            list.Add(new Acao("Compras Em Aberto", null, cpf, id));
                        }
                        if (dadosAcesso.Sistema.cartaoPJVA == 0 && cartaoUsu.isDependente())
                        {
                            if (permissao.FALTLIMITE == "S" && !eCrediHabita)
                            {
                                if (dadosAcesso.HabLimDep == "S")
                                    list.Add(new Acao("Alterar Limite Dependente", null, cpf, id));
                            }
                        }
                        if (!cartaoUsu.isDependente() && dadosAcesso.HabIncDep == "S" && permissao.FINCCART == "S")
                            list.Add(new Acao("Incluir Dependente", null, cpf, id));

                        if (cartaoUsu.isDependente() && dadosAcesso.HabIncDep == "S" && permissao.FINCCART == "S")
                            list.Add(new Acao("Alterar Data Nascimento", null, cpf, id));

                        if (!cartaoUsu.isDependente() && dadosAcesso.HabExcDep == "S" && permissao.FINCCART == "S")
                        list.Add(new Acao("Excluir Dependente", null, cpf, id));                        

                        if (permissao.FINCCART == "S" && !cartaoUsu.isDependente())
                            list.Add(new Acao("Alterar Dados Cadastrais", null, id, cpf));

                        if (dadosAcesso.Status == "00")
                        {
                            if (dadosAcesso.Sistema.cartaoPJVA == 0 && dadosAcesso.HabBeneficios.Equals("S"))
                            {
                                list.Add(new Acao("Gerenciar Beneficios", null, cpf, id));
                            }
                        }
                    }
                    else if (cartaoUsu.isCancelado() || cartaoUsu.isTransferido())
                    {
                        if (permissao.FSEGVIACART == "S")
                            list.Add(new Acao("Reincluir", "A REINCLUSÃO irá emitir UM NOVO CARTÃO COM NOVA NUMERAÇÃO para o usuário " + cartaoUsu.Nome + ". " +
                                                            "\\nConfirma a reinclusão do cartão com o final " + cartaoUsu.CodCrtMask.Substring(cartaoUsu.CodCrtMask.Length - 4, 4) + " ?", cpf, id));
                    }
                    else if (cartaoUsu.isSuspenso())
                    {
                        if (permissao.FSEGVIACART == "S")
                            list.Add(new Acao("Reativar", "A REATIVAÇÃO  irá  alterar o STATUS  do cartão  para ATIVO. " +
                                              "\\nNÃO SERÁ  EMITIDO UM NOVO CARTÃO. " +
                                              "\\nConfirma a REATIVAÇÃO do usuário " + cartaoUsu.Nome + " com cartão com final " + cartaoUsu.CodCrtMask.Substring(cartaoUsu.CodCrtMask.Length - 4,4) + " ?", cpf, id));

                        if (permissao.FSEGVIACART == "S")
                            list.Add(new Acao("Reincluir", "A REINCLUSÃO irá emitir UM NOVO CARTÃO COM NOVA NUMERAÇÃO para o usuário " + cartaoUsu.Nome + ". " +
                                                            "\\nConfirma a reinclusão do cartão com o final " + cartaoUsu.CodCrtMask.Substring(cartaoUsu.CodCrtMask.Length - 4, 4) + " ?", cpf, id));

                    }
                    else if (cartaoUsu.isBloqueado())
                    {
                        if (titularAtivo && permissao.FDESBLOQCART == "S")
                        {
                            list.Add(new Acao("Desbloquear", "Confirma desbloqueio do cartão " + cartaoUsu.CodCrtMask + " ?", cpf, id));
                        }
                        list.Add(new Acao("Cancelar", "Confirma cancelamento do cartão " + cartaoUsu.CodCrtMask + " ?", cpf, id));
                    }
                    if (cartaoUsu.isDependente() && dadosAcesso.HabExcDep == "S" && permissao.FINCCART == "S")
                    {                           
                        list.Add(new Acao("Excluir Dependente", "Confirma a exclusão do dependente " + cartaoUsu.Nome + "?", cpf, id));
                    }

                    if (!cartaoUsu.isDependente())
                        titularAtivo = cartaoUsu.isAtivo();
                    cartaoUsu.ListaAcoes = list;
                    cartaoUsu.Status = Utils.GetStatus(cartaoUsu.Status);
                    listaCartao.Add(cartaoUsu);
                    retorno = listaCartao.Count > 0 ? Constantes.ok : "Nenhum resultado encontrado.";
                }
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return listaCartao;
        }

        public Cartao AcoesCartao(ObjConn objConn, DadosAcesso dadosAcesso, List<Cartao> listaCartao, Acao acao, out string retorno)
        {
            try
            {
                var ds = new DataSet();
                var cid = int.Parse(acao.Tag);
                retorno = string.Empty;

                foreach (var cartao in listaCartao.Where(cartao => cartao.Id == cid))
                {
                    CodCrt = cartao.CodCrt;
                    Nome = cartao.Nome;
                    Cpf = cartao.Cpf;
                    NumDep = cartao.NumDep;
                    break;
                }

                if (acao.Texto.Contains("Bloquear"))
                {
                    this.BloqueiaListaCartoes(objConn, dadosAcesso, out retorno);
                    retorno = !string.IsNullOrEmpty(retorno) ? " Cartão bloqueado com sucesso." : string.Empty;
                }

                if (acao.Texto.Contains("Desbloquear"))
                {
                    this.DesbloqueiaListaCartoes(objConn, dadosAcesso, out retorno);
                    retorno = retorno == "OK" ? "Cartão desbloqueado com sucesso." : retorno;
                }

                if (acao.Texto.Contains("Suspender"))
                {
                    retorno = new Cancelamento().SuspenderCartao(objConn, dadosAcesso, CodCrt);
                }

                if (acao.Texto.Contains("Excluir"))
                {
                    var dep = new Dependente();
                    dep.CodCrt = CodCrt;
                    var crt = dep.ManuDependente(objConn, dadosAcesso, dep, "D", out retorno);
                    retorno = retorno != "OK"
                                  ? retorno
                                  : "Dependente excluído.";

                    crt.ExibeMensagem = true;
                    crt.MensagemAExibir = "Dependente excluido com sucesso.";

                    return crt;
                }

                if (acao.Texto.Contains("Cancelar"))
                {
                    Filtro = "CancCartao";
                    retorno = Constantes.ok;
                }

                if (acao.Texto.Contains("Segunda"))
                {
                    ConsultaCobValSegVia(objConn, dadosAcesso, out string cobraSegundaVia, out decimal valSegVia);

                    if (cobraSegundaVia == "S")
                    {
                        Filtro = "GeraSegVia";
                        retorno = Constantes.ok;
                    }
                    else
                    {
                        ValidaSegundaVia(objConn, dadosAcesso, this, out retorno);
                    }
                }

                if (acao.Texto.Contains("Reincluir"))
                {
                    try
                    {
                        this.ReincluiCartaoCancelado(objConn, dadosAcesso, CodCrt, out retorno);
                        retorno = retorno == "OK" ? "Cartão reincluído com sucesso, necessário configurar novo limite para este cartão!" : retorno;
                    }
                    catch (Exception)
                    {
                        retorno = "Ocorreu um erro na solicitação.";
                    }
                }

                if (acao.Texto.Contains("Reativar"))
                {
                    try
                    {
                        this.ReativarCartaoSuspenso(objConn, dadosAcesso, CodCrt, out retorno);
                        retorno = retorno == "OK" ? "Cartão reativado com sucesso." : retorno;

                    }
                    catch (Exception)
                    {
                        retorno = "Ocorreu um erro na solicitação.";
                    }
                }

                if (dadosAcesso.Sistema.cartaoPJVA == 0)
                {
                    if (acao.Texto.Contains("Alterar Limite Convênio"))
                    {
                        try
                        {
                            Limite = ConsultaLimite(objConn, CodCrt);
                            Filtro = "Limite";
                            retorno = Constantes.ok;

                        }
                        catch (Exception)
                        {
                            retorno = "Falha em alterar o limite! Verifique o cpf informado.";

                            throw;
                        }
                    }
                    if (acao.Texto.Contains("Alterar Limite Dependente"))
                    {
                        try
                        {
                            Limite = ConsultaLimite(objConn, CodCrt);
                            Filtro = "LimDep";
                            retorno = Constantes.ok;

                        }
                        catch (Exception)
                        {
                            retorno = "Falha em alterar o limite do dependente!";

                            throw;
                        }
                    }
                    if (acao.Texto.Contains("Alterar Data Nascimento"))
                    {
                        try
                        {
                            DataNascimento = ConsultaDatNasDep(objConn, CodCrt);
                            Filtro = "DatNasDep";
                            retorno = Constantes.ok;

                        }
                        catch (Exception)
                        {
                            retorno = "Falha em alterar o limite do dependente!";

                            throw;
                        }
                    }
                    if (acao.Texto.Contains("Alterar N° Max de Parcelas"))
                    {
                        try
                        {
                            NumMaxParc = ConsultaMaxParc(objConn, CodCrt);
                            Filtro = "NumMaxParc";
                            retorno = Constantes.ok;
                        }
                        catch (Exception)
                        {
                            retorno = "Ocorreu um erro na solicitação.";

                            throw;
                        }
                    }
                    if (acao.Texto.Contains("Alterar Prêmio"))
                    {
                        try
                        {
                            Premio = ConsultaPremio(objConn, CodCrt);
                            Filtro = "Premio";
                            retorno = Constantes.ok;

                        }
                        catch (Exception)
                        {
                            retorno = "Ocorreu um erro na solicitação.";

                            throw;
                        }
                    }
                }

                if (acao.Texto.Contains("Incluir"))
                {
                    Filtro = "InclusaoDep";
                    retorno = Constantes.ok;
                }

                if (acao.Texto.Contains("Alterar Dados Dependente"))
                {
                    Filtro = "AlterarDep";
                    retorno = Constantes.ok;
                }

                if (acao.Texto.Contains("Excluir"))
                {
                    Filtro = "ExclusaoDep";
                    retorno = Constantes.ok;
                }
            }
            catch (Exception)
            {
                retorno = "Falha ao " + acao + "! Verifique o cpf informado.";
            }
            return this;
        }

        public List<ItensGeneric> ListaJustificativaSegVia(ObjConn objConn)
        {
            SqlConnection conn = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand cmd = new SqlCommand("SELECT CODJUS_SEG_VIA_CARD, NOMJUSTIFICATIVA FROM JUS_SEG_VIA_CARD WITH (NOLOCK) WHERE ATIVO = 'S' ORDER BY NOMJUSTIFICATIVA", conn);

            try
            {
                var justSegVia = new List<ItensGeneric>();
                DataTable dt = new DataTable();

                conn.Open();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var CodJus = row["CODJUS_SEG_VIA_CARD"].ToString();
                        var Descricao = row["NOMJUSTIFICATIVA"].ToString();
                        justSegVia.Add(new ItensGeneric(CodJus, Descricao));
                    }
                }

                return justSegVia;
            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                return new List<ItensGeneric>();
            }
        }

        public List<ItensGeneric> ListaJustificativaCancCartao(ObjConn objConn)
        {
            SqlConnection conn = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand cmd = new SqlCommand("SELECT CODJUS, NOMJUS FROM JUSTIFICATIVA_CANC_CARTAO WITH (NOLOCK) WHERE STA = '00' ORDER BY NOMJUS", conn);

            try
            {
                var justCanc = new List<ItensGeneric>();
                DataTable dt = new DataTable();

                conn.Open();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var CodJus = row["CODJUS"].ToString();
                        var Descricao = row["NOMJUS"].ToString();
                        justCanc.Add(new ItensGeneric(CodJus, Descricao));
                    }
                }

                return justCanc;
            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                return new List<ItensGeneric>();
            }
        }
        public List<ItensGeneric> ListaParentesco(ObjConn objConn)
        {

            SqlConnection conn = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand cmd = new SqlCommand("SELECT CODPAR AS CODIGO, DESPAR AS DESCRICAO FROM PARENTESCO WITH (NOLOCK) ORDER BY DESPAR", conn);

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
                        var CodPar = row["CODIGO"].ToString();
                        var Descricao = row["DESCRICAO"].ToString();
                        parentesco.Add(new ItensGeneric(CodPar, Descricao));
                    }
                }

                return parentesco;
            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                return new List<ItensGeneric>();
            }
        }

        public List<ItensGeneric> ListaSexo()
        {
            var listaSexo = new List<ItensGeneric>();

            listaSexo.Add(new ItensGeneric("M", "Masculino"));
            listaSexo.Add(new ItensGeneric("F", "Feminino"));

            return listaSexo;

        }

        public Cartao BuscarSexoDtNascimentoDep(ObjConn objConn, DadosAcesso dadosAcesso, string cpf, string numDep, out string DtNascimento, out string Sexo)
        {
            string dtnas = "";
            string sex = "";

            string tabela = dadosAcesso.Sistema.cartaoPJVA == 0 ? " USUARIO " : " USUARIOVA ";
            string sql = "SELECT SEXO, DATNAS FROM " + tabela + " WITH (NOLOCK) WHERE CPF = '" + cpf + "' AND NUMDEP  = " + numDep + " AND CODCLI = " + dadosAcesso.Codigo.ToString();

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand comando = new SqlCommand(sql, conexao);

            try
            {
                DataTable dt = new DataTable();

                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                if (dt.Rows.Count == 1)
                {
                    dtnas = dt.Rows[0]["DATNAS"].ToString();
                    sex = dt.Rows[0]["SEXO"].ToString();
                }
            }
            catch 
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();
            }

            DtNascimento = dtnas;
            Sexo = sex;
            return this;
        }

        public Cartao AlteraPremio(ObjConn objConn, DadosAcesso dadosAcesso, string cpf, string premioNovo, string codCrt, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_ALTERA_PREMIO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                                    {
                                        new Parametro("@LOGINWEB", DbType.String, objConn.LoginWeb),
                                        new Parametro("@CODCLI", DbType.Int32, Convert.ToInt32(Convert.ToString(dadosAcesso.Codigo))),
                                        new Parametro("@CPF", DbType.String, cpf),
                                        new Parametro("@PREMIO", DbType.Decimal, Convert.ToDecimal(premioNovo))
                                    };

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read()) retorno = (string)dr["RETORNO"];
                retorno = retorno != Constantes.ok
                              ? retorno
                              : " Cartão: " + codCrt + " - Alterado prêmio.";
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null) dr.Close();
            }
            return this;
        }

        public Cartao ValidaSegundaVia(ObjConn objConn, DadosAcesso dadosAcesso, Cartao model,  out string retorno)
        {
            retorno = string.Empty;
            SqlConnection conn = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand cmd = new SqlCommand("PROC_VERIFICA_SOLICITACAO_2_VIA", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@SISTEMA", DbType = DbType.Int32, Value = dadosAcesso.Sistema.cartaoPJVA });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CODCRT", DbType = DbType.String, Value = model.CodCrt });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CODCLI", DbType = DbType.Int32, Value = dadosAcesso.Codigo });

            DataTable dt = new DataTable();

            try
            {
                conn.Open();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    retorno = "Já houve uma geração de 2ª via para este cartão hoje.";
                }
                else
                {
                    this.GeraSegundaVia(objConn, dadosAcesso, model.CodCrt, Convert.ToInt32(model.Justificativa), out retorno);
                    retorno = retorno == "OK" ? "Segunda via gerada com sucesso." : retorno;
                }

            }
            catch (Exception)
            {
                retorno = "Ocorreu um erro na solicitação.";
            }
            return this;
        }

        public Cartao GeraSegundaVia(ObjConn objConn, DadosAcesso dadosAcesso, string codCrt, int codJustSegViaCard, out string retorno)
        {
            var cobSegVia = string.Empty;
            var valSegVia = 0m;
            ConsultaCobValSegVia(objConn, dadosAcesso, out cobSegVia, out valSegVia);
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_GERA_SEGVIA";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            db.AddInParameter(cmd, "@SISTEMA", DbType.Int16, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "@CODCRT", DbType.String, codCrt);
            db.AddInParameter(cmd, "@MANTEM_ANTIGO", DbType.String, "N");
            db.AddInParameter(cmd, "@COBRA2AV", DbType.String, cobSegVia);
            db.AddInParameter(cmd, "@VAL2AV", DbType.Decimal, valSegVia);
            db.AddInParameter(cmd, "@ID_JUSTIFICATIVA", DbType.Int32, codJustSegViaCard);
            db.AddInParameter(cmd, "@IDOPEMW", DbType.Int32, dadosAcesso.Id);
            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read()) retorno = (string)dr["RETORNO"];
                retorno = retorno != Constantes.ok ? Convert.ToString(dr["MENSAGEM"]) : "Segunda via gerada com sucesso.";
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null) dr.Close();
            }
            return this;
        }

        public Cartao ReincluiCartaoCancelado(ObjConn objConn, DadosAcesso dadosAcesso, string codCrt,  out string retorno)
        {
            var cobSegVia = string.Empty;
            var valSegVia = 0m;
            ConsultaCobValSegVia(objConn, dadosAcesso, out cobSegVia, out valSegVia);
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_REINCLUI_CARTAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            db.AddInParameter(cmd, "@SISTEMA", DbType.Int16, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "@CODCRT", DbType.String, codCrt);
            db.AddInParameter(cmd, "@COBRAR", DbType.String, "S");
            db.AddInParameter(cmd, "@IDOPEMW", DbType.Int32, dadosAcesso.Id);
            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read()) retorno = (string)dr["RETORNO"];

                if (dadosAcesso.Sistema.cartaoPJVA == 0)
                {
                    retorno = retorno != Constantes.ok ? Convert.ToString(dr["MENSAGEM"]) : "Cartão reincluido com sucesso, é necessário configurar novo limite para este cartão!";
                }
                else
                {
                    retorno = retorno != Constantes.ok ? Convert.ToString(dr["MENSAGEM"]) : "Cartão reincluido com sucesso, em breve receberá um novo cartão!";
                }
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null) dr.Close();
            }
            return this;
        }

        public Cartao ReativarCartaoSuspenso(ObjConn objConn, DadosAcesso dadosAcesso, string codCrt, out string retorno)
        {
            var cobSegVia = string.Empty;
            var valSegVia = 0m;
            ConsultaCobValSegVia(objConn, dadosAcesso, out cobSegVia, out valSegVia);
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_REATIVA_CARTAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODCRT", DbType.String, codCrt);
            db.AddInParameter(cmd, "@IDOPEMW", DbType.Int32, dadosAcesso.Id);
            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read()) retorno = (string)dr["RETORNO"];
                retorno = retorno != Constantes.ok ? Convert.ToString(dr["MENSAGEM"]) : "Cartão reativado com sucesso.";
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null) dr.Close();
            }
            return this;
        }

        public Cartao AlteraLimite(ObjConn objConn, DadosAcesso dadosAcesso, string cpf, string limiteNovo, string codCrt, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_ALTERA_LIMITE";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                                    {
                                        new Parametro("@LOGINWEB", DbType.String, objConn.LoginWeb),
                                        new Parametro("@CODCLI", DbType.Int32, Convert.ToInt32(Convert.ToString(dadosAcesso.Codigo))),
                                        new Parametro("@CPF", DbType.String, cpf),
                                        new Parametro("@LIMITE", DbType.Decimal, Convert.ToDecimal(limiteNovo))
                                    };

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read()) retorno = (string)dr["RETORNO"];
                retorno = retorno != Constantes.ok
                              ? retorno
                              : " Cartão: " + codCrt + " - Alterado limite.";
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null) dr.Close();
            }
            return this;
        }

        public Cartao AlteraLimiteDependente(ObjConn objConn, DadosAcesso dadosAcesso, string cpf, string limiteNovo, string codCrt, int numDep, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_ALTERA_LIMITE_DEPENDENTE";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                                    {
                                        new Parametro("@LOGINWEB", DbType.String, objConn.LoginWeb),
                                        new Parametro("@CODCLI", DbType.Int32, Convert.ToInt32(Convert.ToString(dadosAcesso.Codigo))),
                                        new Parametro("@CPF", DbType.String, cpf),
                                        new Parametro("@NUMDEP", DbType.Int32, numDep),
                                        new Parametro("@LIMDEP", DbType.Decimal, Convert.ToDecimal(limiteNovo))
                                    };

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read()) retorno = (string)dr["RETORNO"];
                retorno = retorno != Constantes.ok
                              ? retorno
                              : " Cartão: " + codCrt + " - Alterado limite dependente.";
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null) dr.Close();
            }
            return this;
        }

        public Cartao AlteraDataNascimentoDependente(ObjConn objConn, DadosAcesso dadosAcesso, string cpf, string datNasDep, string codCrt, int numDep, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_ALTERA_DATNAS_DEPENDENTE";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                                    {
                                        new Parametro("@LOGINWEB", DbType.String, objConn.LoginWeb),
                                        new Parametro("@CODCLI", DbType.Int32, Convert.ToInt32(Convert.ToString(dadosAcesso.Codigo))),
                                        new Parametro("@CPF", DbType.String, cpf),
                                        new Parametro("@NUMDEP", DbType.Int32, numDep),
                                        new Parametro("@DATNAS", DbType.DateTime, Convert.ToDateTime(datNasDep))
                                    };

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read()) retorno = (string)dr["RETORNO"];
                retorno = retorno != Constantes.ok
                              ? retorno
                              : " Cartão: " + codCrt + " - Alterado data de nascimento do dependente.";
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null) dr.Close();
            }
            return this;
        }

        public Cartao AlteraParcela(ObjConn objConn, DadosAcesso dadosAcesso, string cpf, string numParcela, string codCrt, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_ALTERA_MAXPARC";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                                    {
                                        new Parametro("@LOGINWEB", DbType.String, objConn.LoginWeb),
                                        new Parametro("@CODCLI", DbType.Int32, Convert.ToInt32(Convert.ToString(dadosAcesso.Codigo))),
                                        new Parametro("@CPF", DbType.String, cpf),
                                        new Parametro("@NUMPARC", DbType.Int32, numParcela)
                                    };

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read()) retorno = (string)dr["RETORNO"];
                retorno = retorno != Constantes.ok
                              ? retorno
                              : " Cartão: " + codCrt + " - Parcela alterada.";
                dr.Close();
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null) dr.Close();
            }
            return this;
        }        

        public Cartao ListagemUsuarioDependente(ObjConn objConn, DadosAcesso dadosAcesso, int _sistema, string _filtro, out string retorno)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_LISTAEMANTEN_DEPENDENTE";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, _sistema),
                               new Parametro("@CODCLI", DbType.Int32, Convert.ToString(dadosAcesso.Codigo)),
                               new Parametro("@CPF", DbType.String, _filtro),
                               new Parametro("@OPERACAO", DbType.String, "C")
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var cartaoUsu = new Cartao();
            var listaDependente = new List<Dependente>();

            try
            {
                dr = db.ExecuteReader(cmd);
                var i = 1;
                while (dr.Read())
                {
                    if (dr["NUMDEP"].ToString() == "0")
                    {
                        cartaoUsu.Cpf = (dr["CPF"]).ToString();
                        cartaoUsu.Td = Convert.ToInt16(dr["NUMDEP"]) == 0 ? "TITULAR" : "DEPENDENTE";
                        cartaoUsu.Filial = Convert.ToString(dr["CODFIL"]);
                        cartaoUsu.Nome = (dr["NOMUSU"]).ToString();
                        cartaoUsu.Setor = Convert.ToString(dr["CODSET"]);
                        cartaoUsu.Matricula = Convert.ToString(dr["MAT"]);
                        cartaoUsu.CodCrt = (dr["CODCRT"]).ToString();
                        cartaoUsu.Id = Convert.ToInt32(dr["ID"]);
                        cartaoUsu.Cel = Convert.ToString(dr["CELULAR"]);
                        cartaoUsu.Limite = _sistema == 0 ? Convert.ToDecimal(dr["LIMITE"]) : 0;
                        cartaoUsu.Premio = _sistema == 0 ? Convert.ToDecimal(dr["PREMIO"]) : 0;
                    }
                    else
                    {
                        var dep = new Dependente();
                        dep.CodCrt = (dr["CODCRT"]).ToString();
                        dep.CpfTit = (dr["CPF"]).ToString();
                        dep.Id = Convert.ToInt32(dr["ID"]);
                        dep.NumDep = i;
                        dep.NomeDep = Convert.ToString(dr["NOMUSU"]);
                        if (Convert.ToDecimal(dr["LIMDEP"]) > 0) dep.LimDep = Convert.ToString(dr["LIMDEP"]);
                        dep.CodPar = Convert.ToString(dr["CODPAR"]);
                        dep.DesPar = Convert.ToString(dr["PARENTESCO"]);
                        dep.Inclusao = Convert.ToDateTime(dr["DATINC"]);
                        listaDependente.Add(dep);
                        i++;
                    }
                }
                cartaoUsu.Dependentes = new List<Dependente>();
                cartaoUsu.Dependentes = listaDependente;
                dr.Close();
                retorno = cartaoUsu.Nome != null ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return cartaoUsu;
        }

        public decimal CalcLimite(ObjConn objConn, DadosAcesso dadosAcesso, string cpf, int numDep)
        {
            var limite = 0m;
            Database db = new SqlDatabase(Utils.GetConnectionStringAutorizador(objConn));
            const string sql = "SELECT dbo.CalcLimite (@CODCLI, @CPF, @NUMDEP)";
            var cmd = db.GetSqlStringCommand(sql.ToString());

            var list = new List<Parametro>
                           {                               
                               new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo),
                               new Parametro("@CPF", DbType.String, Utils.RetirarCaracteres(".-", cpf)),
                               new Parametro("@NUMDEP", DbType.Int16, numDep)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaResult = new List<Cartao>();
            try
            {
                objConn.Acesso = Constantes.autorizador;
                limite = Convert.ToDecimal(db.ExecuteScalar(cmd));
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
                objConn.Acesso = Constantes.netcard;
            }
            objConn.Acesso = Constantes.netcard;
            return limite;
        }

        public decimal CalcPremio(ObjConn objConn, DadosAcesso dadosAcesso, string cpf, int numDep)
        {
            var limite = 0m;
            Database db = new SqlDatabase(Utils.GetConnectionStringAutorizador(objConn));
            const string sql = "SELECT dbo.CalcPremio (@CODCLI, @CPF, @NUMDEP)";
            var cmd = db.GetSqlStringCommand(sql.ToString());

            var list = new List<Parametro>
                           {                               
                               new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo),
                               new Parametro("@CPF", DbType.String, Utils.RetirarCaracteres(".-", cpf)),
                               new Parametro("@NUMDEP", DbType.Int16, numDep)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaResult = new List<Cartao>();
            try
            {
                objConn.Acesso = Constantes.autorizador;
                limite = Convert.ToDecimal(db.ExecuteScalar(cmd));
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
                objConn.Acesso = Constantes.netcard;
            }
            objConn.Acesso = Constantes.netcard;
            return limite;
        }

        public EndCep GetCep(ObjConn objConn, string cep)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringConcentrador(objConn));
            const string sql = "MW_CONSCEP";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            db.AddInParameter(cmd, "@CEP", DbType.String, cep);

            IDataReader dr = null;
            var endereco = new EndCep();
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    endereco.Retorno = Convert.ToString(dr["RETORNO"]);
                    if (endereco.Retorno == Constantes.ok)
                    {
                        endereco.Logra = Convert.ToString(dr["LOGRADOURO"]);
                        endereco.Bairro = Convert.ToString(dr["BAIRRO"]);
                        endereco.Locali = Convert.ToString(dr["LOCALIDADE"]);
                        endereco.Uf = Convert.ToString(dr["UF"]);
                    }
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return endereco;
        }

        public decimal ConsultaLimite(ObjConn objConn, string codCrt)
        {
            decimal limite = 0;

            SqlConnection conn = new SqlConnection(Utils.GetConnectionStringAutorizador(objConn));
            SqlCommand cmd = new SqlCommand("PROC_CONS_LIMITE", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CODCRT", Value = codCrt, DbType = DbType.String});

                DataTable dt = new DataTable();

                conn.Open();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                if (dt.Rows.Count == 1)
                {
                    limite = Convert.ToDecimal(dt.Rows[0]["LIMITE"].ToString());
                }

                return limite;
            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();

                throw;
                
            }
        }

        public string ConsultaDatNasDep(ObjConn objConn, string codCrt)
        {
            SqlConnection conn = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand cmd = new SqlCommand("PROC_CONS_DATNASDEP", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            var datNasDep = string.Empty;

            try
            {
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CODCRT", Value = codCrt, DbType = DbType.String });

                DataTable dt = new DataTable();

                conn.Open();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                if (dt.Rows.Count == 1)
                {
                    datNasDep = Convert.ToString(dt.Rows[0]["DATNAS"].ToString());
                }

                return datNasDep;
            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();

                throw;
            }
        }

        public decimal ConsultaPremio(ObjConn objConn, string codCrt)
        {
            decimal premio = 0;

            SqlConnection conn = new SqlConnection(Utils.GetConnectionStringAutorizador(objConn));
            SqlCommand cmd = new SqlCommand("PROC_CONS_LIMITE", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CODCRT", Value = codCrt, DbType = DbType.String });

                DataTable dt = new DataTable();

                conn.Open();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                if (dt.Rows.Count == 1)
                {
                    premio = Convert.ToDecimal(dt.Rows[0]["PREMIO"].ToString());
                }

                return premio;
            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();

                throw;

            }
        }

        public int ConsultaMaxParc(ObjConn objConn, string codCrt)
        {
            int maxParc = 0;

            SqlConnection conn = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlCommand cmd = new SqlCommand("PROC_CONS_MAXPARC", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CODCRT", Value = codCrt, DbType = DbType.String });

                DataTable dt = new DataTable();

                conn.Open();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                if (dt.Rows.Count == 1)
                {
                    maxParc = Convert.ToInt16(dt.Rows[0]["MAXPARC"].ToString());
                }

                return maxParc;
            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();

                throw;

            }
        }

        public void ConsultaCobValSegVia(ObjConn objConn, DadosAcesso dadosAcesso, out string cobSegVia, out decimal valSegVia)
        {
            cobSegVia = string.Empty;
            valSegVia = 0m;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "SELECT VAL2AV, COB2AV FROM VPRODUTOSCLI WITH (NOLOCK) WHERE SISTEMA = @SISTEMA AND CODCLI = @CODCLI ";
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@SISTEMA", DbType.Int16, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "@CODCLI", DbType.Int32, dadosAcesso.Codigo);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    cobSegVia = Convert.ToString(dr["COB2AV"]);
                    valSegVia = Convert.ToDecimal(dr["VAL2AV"]);
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
                objConn.Acesso = Constantes.netcard;
            }
            objConn.Acesso = Constantes.netcard;
        }
    }
}
