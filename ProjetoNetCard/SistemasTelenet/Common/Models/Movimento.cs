using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Linq;
using NetCard.Common.Util;

namespace NetCard.Common.Models
{
    public class Movimento
    {
        [Display(Name = "Listar por Centralizadora")]
        public bool CheckCen { get; set; }

        [Display(Name = "CPF Titular")]
        public string Cpf { get; set; }

        [Display(Name = "Data Inicial")]
        public string DtInicial { get; set; }

        [Display(Name = "Data Final")]
        public string DtFinal { get; set; }

        public string Lote { get; set; }

        public string StaLote { get; set; }

        public DetalheTotalMovimento MovimentosDetalhe(ObjConn objConn, DadosAcesso dadosAcesso, Movimento filtros, out string retorno)
        {
            var totais = new DetalheTotalMovimento();
            decimal total = 0, subtotal = 0;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_MOVTO_USUARIO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCLI", DbType.Int32, Convert.ToString(dadosAcesso.Codigo)),
                               new Parametro("@DATA_INI", DbType.String, Convert.ToDateTime(filtros.DtInicial).ToString("yyyyMMdd")),
                               new Parametro("@DATA_FIM", DbType.String, Convert.ToDateTime(filtros.DtFinal).ToString("yyyyMMdd")),
                               new Parametro("@CPF", DbType.String, string.IsNullOrEmpty(filtros.Cpf) ? null : Utils.RetirarCaracteres(".-", filtros.Cpf)),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);
            
            try
            {
                var ds = Utils.ConvertDataReaderToDataSet(db.ExecuteReader(cmd));
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    retorno = "Nenhuma informação encontrada.";
                    return totais;
                }
                var drAux = ds.Tables[0].Rows[0];
                var nome = Convert.ToString(drAux["NOMETIT"]);
                var matricula = Convert.ToString(drAux["MATRICULA"]);
                var filial = Convert.ToString(drAux["CODFIL"]);
                var setor = Convert.ToString(drAux["CODSET"]);
                var cpf = Convert.ToString(drAux["CPF"]);
                var cpfAux = cpf;

                var parcial = new DetalheParcialMovimento(nome, matricula, cpf, filial, setor);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cpf = (string)dr["CPF"];
                    if (!cpfAux.Equals(cpf))
                    {
                        parcial.SubTotal = subtotal;
                        totais.add(parcial);
                        nome = (string)dr["NOMETIT"];
                        matricula = (string)dr["matricula"];
                        filial = dr["CODFIL"].ToString();
                        setor = dr["CODSET"].ToString();
                        parcial = new DetalheParcialMovimento(nome, matricula, cpf, filial, setor);
                        subtotal = 0;
                        cpfAux = cpf;
                    }

                    var tipotit = Convert.ToInt32(dr["TITULAR"]);
                    var cartao = Convert.ToString(dr["CARTAO"]);
                    var data = Convert.ToString(dr["DATA"]);
                    var numaut = Convert.ToString(dr["AUTORIZ"]);
                    var valor = Convert.ToDecimal(dr["VALTRA"]);
                    var desc = Convert.ToString(dr["TIPO"]);
                    var estab = Convert.ToString(dr["RAZSOC"]);
                    var codest = Convert.ToInt32(dr["CODCRE"]);
                    var trans = Convert.ToInt32(dr["TIPTRA"]);

                    if (dr["TIPTRA"].ToString() != "999010")
                    {
                        total += valor;
                        subtotal += valor;
                    }
                    parcial.add(codest == 0
                                    ? new DetalheMovimento(tipotit, cartao, data, numaut, valor, desc, Convert.ToString(trans),
                                                         " ", estab)
                                    : new DetalheMovimento(tipotit, cartao, data, numaut, valor, desc, Convert.ToString(trans),
                                                         Convert.ToString(codest), estab));
                }

                parcial.SubTotal = subtotal;
                totais.add(parcial);
                totais.Total = total;
                totais.Compras = totais.Total - totais.Premios - totais.Taxas - totais.Ativacoes - totais.Receitas;
                retorno = totais.Parciais.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch { retorno = "Ocorreu um erro durante a operação"; }
            return totais;
        }

        public DetalheTotalMovimento MovimentosDetalheLote(ObjConn objConn, DadosAcesso dadosAcesso, Movimento filtros, out string retorno)
        {
            var totais = new DetalheTotalMovimento();
            totais.Cpf = "";
            decimal total = 0, subtotal = 0;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_MOVTO_USUARIO_LOTE";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCLI", DbType.Int32, Convert.ToString(dadosAcesso.Codigo)),
                               new Parametro("@NUM_LOTE", DbType.Int32, Convert.ToString(filtros.Lote)),
                               new Parametro("@CPF", DbType.String, string.IsNullOrEmpty(filtros.Cpf) ? null : Utils.RetirarCaracteres(".-", filtros.Cpf)),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            try
            {
                var ds = Utils.ConvertDataReaderToDataSet(db.ExecuteReader(cmd));
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    retorno = "Nenhuma informação encontrada.";
                    return totais;
                }
                var drAux = ds.Tables[0].Rows[0];
                var nome = Convert.ToString(drAux["NOMETIT"]);
                var matricula = Convert.ToString(drAux["MATRICULA"]);
                var filial = Convert.ToString(drAux["CODFIL"]);
                var setor = Convert.ToString(drAux["CODSET"]);
                var cpf = Convert.ToString(drAux["CPF"]);
                var cpfAux = cpf;

                var parcial = new DetalheParcialMovimento(nome, matricula, cpf, filial, setor);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cpf = Convert.ToString(dr["CPF"]);
                    if (!cpfAux.Equals(cpf))
                    {
                        parcial.SubTotal = subtotal;
                        totais.add(parcial);
                        nome = Convert.ToString(dr["NOMETIT"]);
                        matricula = Convert.ToString(dr["matricula"]);
                        filial = dr["CODFIL"].ToString();
                        setor = dr["CODSET"].ToString();
                        parcial = new DetalheParcialMovimento(nome, matricula, cpf, filial, setor);
                        subtotal = 0;
                        cpfAux = cpf;
                    }

                    var tipotit = Convert.ToString(dr["TITULAR"]) == "T" ? 0 : 1;
                    var cartao = Convert.ToString(dr["CARTAO"]);
                    var data = Convert.ToString(dr["DATA"]);
                    var numaut = Convert.ToString(dr["AUTORIZ"]);
                    var valor = Convert.ToDecimal(dr["VALTRA"]);
                    var desc = Convert.ToString(dr["TIPO"]);
                    var estab = Convert.ToString(dr["RAZSOC"]);
                    var codest = Convert.ToInt32(dr["CODCRE"]);
                    var trans = Convert.ToInt32(dr["TIPTRA"]);

                    if (dr["TIPTRA"].ToString() != "999010")
                    {
                        total += valor;
                        subtotal += valor;
                    }
                    parcial.add(codest == 0
                                    ? new DetalheMovimento(tipotit, cartao, data, numaut, valor, desc, Convert.ToString(trans),
                                                         " ", estab)
                                    : new DetalheMovimento(tipotit, cartao, data, numaut, valor, desc, Convert.ToString(trans),
                                                         Convert.ToString(codest), estab));
                    if (dadosAcesso.Acesso == Constantes.usuario)
                    {
                        totais.Limite = Convert.ToDecimal(dr["LIMITE"]);
                        totais.Premios = Convert.ToDecimal(dr["PREMIO"]);
                        totais.Desconto = Convert.ToDecimal(dr["DESCONTO"]);
                        totais.Utilizado = Convert.ToDecimal(dr["UTILIZADO"]);
                    }
                }

                parcial.SubTotal = subtotal;
                totais.add(parcial);
                totais.Total = total;
                totais.Total = total;
                totais.Compras = totais.Total - totais.Premios - totais.Taxas - totais.Ativacoes - totais.Receitas;
                retorno = totais.Parciais.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch { retorno = "Ocorreu um erro durante a operação"; }
            return totais;
        }

        public DetalheTotalLote MovimentoLoteFechado(ObjConn objConn, DadosAcesso dadosAcesso, Movimento filtros, out string retorno)
        {
            var totais = new DetalheTotalLote();
            decimal subTotalValInf = 0m, subTotalValAfe = 0m, subTotalValorLiq = 0m;
            int subTotalQuantInf = 0, subTotalQuantAfe = 0;
            decimal totalValAfe = 0m, totalValorLiq = 0m;
            int totalQuantAfe = 0;
            var pCodCen = (dadosAcesso.Codigo == dadosAcesso.CodCen && filtros.CheckCen);


            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_EXTLOTFECH_2";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCRE", DbType.Int32, objConn.CodCre),
                               new Parametro("@CENTRALIZ", DbType.Int32, pCodCen? 1 : 0),
                               new Parametro("@DATA_INI", DbType.String, Convert.ToDateTime(filtros.DtInicial).ToString("yyyyMMdd")),
                               new Parametro("@DATA_FIM", DbType.String, Convert.ToDateTime(filtros.DtFinal).ToString("yyyyMMdd")),
                               new Parametro("@FORMATO", DbType.Int32, 2)                               
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);           
            
            try
            {
                var ds = Utils.ConvertDataReaderToDataSet(db.ExecuteReader(cmd));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var drAux = ds.Tables[0].Rows[0];
                    var razao = Convert.ToString(drAux["RAZSOC"]);
                    var codcre = Convert.ToInt32(drAux["CODCRE"]);
                    var cnpj = drAux["CNPJ"].ToString();
                    var codAux = codcre;
                    var parcial = new DetalheParcialLote(razao, codcre, cnpj);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        codcre = Convert.ToInt32(dr["CODCRE"]);
                        if (!codAux.Equals(codcre))
                        {
                            parcial.SubTotalQuantAfe = subTotalQuantAfe;
                            parcial.SubTotalQuantInf = subTotalQuantInf;
                            parcial.SubTotalValAfe = subTotalValAfe;
                            parcial.SubTotalValInf = subTotalValInf;
                            parcial.SubTotalValorLiq = subTotalValorLiq;
                            totais.add(parcial);

                            razao = Convert.ToString(dr["RAZSOC"]);
                            codcre = Convert.ToInt32(dr["CODCRE"]);
                            cnpj = dr["CNPJ"].ToString();
                            parcial = new DetalheParcialLote(razao, codcre, cnpj);
                            subTotalValInf = 0;
                            subTotalQuantInf = 0;
                            subTotalValAfe = 0;
                            subTotalQuantAfe = 0;
                            subTotalValorLiq = 0;
                            codAux = codcre;
                        }

                        var codigo = Convert.ToInt32(dr["CODCRE"]);
                        var valorafe = Convert.ToDecimal(dr["VAL_AFE"]);
                        var quantafe = Convert.ToInt32(dr["QTE_AFE"]);
                        var taxa = 0m;
                        if (dr["TAXA"] != DBNull.Value)
                            taxa = Convert.ToDecimal(dr["TAXA"]);
                        var valtaxa = Convert.ToDecimal(dr["VALTAXA"]);
                        var valorliq = Convert.ToDecimal(dr["VAL_LIQ"]);
                        var dinicio = Convert.ToDateTime(dr["INICIO"]);
                        var dfim = Convert.ToDateTime(dr["FIM"]);
                        var nfechamento = Convert.ToInt32(dr["N_FEC"]);
                        var datafech = Convert.ToString(dr["DT_FECH"]);
                        var datapag = string.IsNullOrEmpty(dr["DT_PGTO"].ToString())
                                                ? DateTime.MinValue
                                                : Convert.ToDateTime(dr["DT_PGTO"]);

                        totalValAfe += valorafe;
                        totalQuantAfe += quantafe;
                        totalValorLiq += valorliq;

                        subTotalValAfe += valorafe;
                        subTotalQuantAfe += quantafe;
                        subTotalValorLiq += valorliq;

                        parcial.add(new Lote(codigo, valorafe, quantafe, taxa, valtaxa, valorliq, dinicio, dfim,
                                                    nfechamento, datafech, datapag));
                    }
                    parcial.SubTotalQuantAfe = subTotalQuantAfe;
                    parcial.SubTotalValAfe = subTotalValAfe;
                    parcial.SubTotalValorLiq = subTotalValorLiq;
                    totais.add(parcial);
                    totais.TotalQuantAfe = totalQuantAfe;
                    totais.TotalValAfe = totalValAfe;
                    totais.TotalValorLiq = totalValorLiq;
                }
                retorno = totais.Parciais.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch { retorno = "Ocorreu um erro durante a operação"; }
            return totais;
        }

        public DetalheLote DetalheLoteFechado(ObjConn objConn, DadosAcesso dadosAcesso, int codCre, int numFech, out string retorno)
        {
            retorno = string.Empty;
            var detalheLotesFechados = new DetalheLote();
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_EXTLOTFECH_2";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCRE", DbType.Int32, codCre),
                               new Parametro("@CENTRALIZ", DbType.Int32, 0),
                               new Parametro("@NUMFECH", DbType.Int32, numFech),                               
                               new Parametro("@TIPO", DbType.Int32, 2),
                               new Parametro("@FORMATO", DbType.Int32, 2)                               
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);
            
            IDataReader dr = null;

            try
            {
                dr = db.ExecuteReader(cmd);
                var ListSubRede = new List<LoteSubRede>();
                var ListTaxas = new List<LoteTaxas>();                
                while (dr.Read())
                {                    
                    if (Convert.ToString(dr["TIPO"]) == "COMPRAS")
                    {
                        var detalheLoteFechadoSubRede = new LoteSubRede();
                        detalheLoteFechadoSubRede.CodSubRede = Convert.ToInt32(dr["CODSUBREDE"]);
                        detalheLoteFechadoSubRede.Descricao = Convert.ToString(dr["DESCRICAO"]);
                        detalheLoteFechadoSubRede.Qtde = Convert.ToInt16(dr["QTE"]);
                        detalheLoteFechadoSubRede.Valor = Convert.ToDecimal(dr["VALOR"]);
                        detalheLoteFechadoSubRede.Taxa = Convert.ToDecimal(dr["TAXA"]);
                        detalheLoteFechadoSubRede.ValLiq = Convert.ToDecimal(dr["VAL_LIQ"]);
                        detalheLoteFechadoSubRede.CodCre = Convert.ToInt32(dr["CODCRE"]);
                        detalheLoteFechadoSubRede.NumFech = Convert.ToInt16(dr["N_FEC"]);
                        detalheLotesFechados.TotalQtdeTrans += detalheLoteFechadoSubRede.Qtde;
                        detalheLotesFechados.TotalValor += detalheLoteFechadoSubRede.Valor;
                        detalheLotesFechados.TotalValLiq += detalheLoteFechadoSubRede.ValLiq;                        
                        ListSubRede.Add(detalheLoteFechadoSubRede);
                    }

                    if (Convert.ToString(dr["TIPO"]) == "TAXAS")
                    {
                        var detalheLoteFechadoTaxas = new LoteTaxas();
                        detalheLoteFechadoTaxas.Descricao = Convert.ToString(dr["DESCRICAO"]);
                        detalheLoteFechadoTaxas.Qtde = Convert.ToInt16(dr["QTE"]);
                        detalheLoteFechadoTaxas.Valor = Convert.ToDecimal(dr["VALOR"]);
                        detalheLoteFechadoTaxas.CodCre = Convert.ToInt32(dr["CODCRE"]);
                        detalheLoteFechadoTaxas.NumFech = Convert.ToInt16(dr["N_FEC"]);
                        detalheLotesFechados.TotalValTaxas += detalheLoteFechadoTaxas.Valor;
                        ListTaxas.Add(detalheLoteFechadoTaxas);
                    }
                }                
                detalheLotesFechados.ListSubRede = ListSubRede;
                detalheLotesFechados.ListTaxas = ListTaxas;                
                retorno = detalheLotesFechados.ListSubRede.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
                dr.Close();
            }
            catch { retorno = "Ocorreu um erro durante a operação"; }
            return detalheLotesFechados;
        }

        public List<Transacao> DetalheSubRedeFechado(ObjConn objConn, DadosAcesso dadosAcesso, int codCre, int numFech, int codSubRede, out string retorno)
        {
            retorno = string.Empty;
            var lista = new List<Transacao>();
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_EXTLOTFECH_2";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCRE", DbType.Int32, codCre),
                               new Parametro("@CENTRALIZ", DbType.Int32, 0),
                               new Parametro("@NUMFECH", DbType.Int32, numFech),
                               new Parametro("@CODSUBREDE", DbType.Int32, codSubRede),
                               new Parametro("@TIPO", DbType.Int32, 1),
                               new Parametro("@FORMATO", DbType.Int32, 2)                               
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);
            
            try
            {
                var ds = Utils.ConvertDataReaderToDataSet(db.ExecuteReader(cmd));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lista.AddRange(from DataRow dr in ds.Tables[0].Rows
                                   let codigo = Convert.ToInt32(dr["CODCRE"])
                                   let razsoc = Convert.ToString(dr["RAZSOC"])
                                   let data = Convert.ToDateTime(dr["DATA"])
                                   let nsu = Convert.ToInt32(dr["AUTORIZ"])
                                   let tiptra = Convert.ToInt32(dr["TIPTRA"])
                                   let transacao = Convert.ToString(dr["TRANSACAO"])
                                   let cartao = Convert.ToString(dr["CARTAO"])
                                   let valor = Convert.ToDecimal(dr["VALTRA"])
                                   let valorliq = Convert.ToDecimal(dr["VAL_LIQ"])
                                   let datafech = Convert.ToDateTime(dr["DT_FECH"])
                                   select new Transacao(codSubRede, codigo, razsoc, Convert.ToString(data), nsu, cartao, valor, valorliq,
                                       tiptra, transacao, datafech.ToShortDateString()));
                    
                }
                retorno = lista.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch { retorno = "Ocorreu um erro durante a operação"; }
            return lista;
        }

        public DetalheTotalLote MovimentoLoteAberto(ObjConn objConn, DadosAcesso dadosAcesso, Movimento filtros, out string retorno)
        {
            var totais = new DetalheTotalLote();
            decimal subTotalValInf = 0m, subTotalValAfe = 0m, subTotalValorLiq = 0m;
            int subTotalQuantInf = 0, subTotalQuantAfe = 0;
            decimal totalValAfe = 0m, totalValorLiq = 0m;
            int totalQuantAfe = 0;
            var pCodCen = (dadosAcesso.Codigo == dadosAcesso.CodCen && filtros.CheckCen);

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_EXTLOTAB_2";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCRE", DbType.Int32, objConn.CodCre),
                               new Parametro("@CENTRALIZ", DbType.Int32, pCodCen? 1 : 0),                               
                               new Parametro("@FORMATO", DbType.Int32, 2)                               
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);            

            try
            {
                var ds = Utils.ConvertDataReaderToDataSet(db.ExecuteReader(cmd));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var drAux = ds.Tables[0].Rows[0];
                    var razao = Convert.ToString(drAux["RAZSOC"]);
                    var codcre = Convert.ToInt32(drAux["CODCRE"]);
                    var cnpj = drAux["CNPJ"].ToString();
                    var codAux = codcre;
                    var parcial = new DetalheParcialLote(razao, codcre, cnpj);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        codcre = Convert.ToInt32(dr["CODCRE"]);
                        if (!codAux.Equals(codcre))
                        {
                            parcial.SubTotalQuantAfe = subTotalQuantAfe;
                            parcial.SubTotalQuantInf = subTotalQuantInf;
                            parcial.SubTotalValAfe = subTotalValAfe;
                            parcial.SubTotalValInf = subTotalValInf;
                            parcial.SubTotalValorLiq = subTotalValorLiq;
                            totais.add(parcial);

                            razao = Convert.ToString(dr["RAZSOC"]);
                            codcre = Convert.ToInt32(dr["CODCRE"]);
                            cnpj = dr["CNPJ"].ToString();
                            parcial = new DetalheParcialLote(razao, codcre, cnpj);
                            subTotalValInf = 0;
                            subTotalQuantInf = 0;
                            subTotalValAfe = 0;
                            subTotalQuantAfe = 0;
                            subTotalValorLiq = 0;
                            codAux = codcre;
                        }

                        var codigo = Convert.ToInt32(dr["CODCRE"]);
                        var valorafe = Convert.ToDecimal(dr["VAL_AFE"]);
                        var quantafe = Convert.ToInt32(dr["QTE_AFE"]);
                        var taxa = 0m;
                        if (dr["TAXA"] != DBNull.Value)
                            taxa = Convert.ToDecimal(dr["TAXA"]);
                        var valtaxa = Convert.ToDecimal(dr["VALTAXA"]);
                        var valorliq = Convert.ToDecimal(dr["VAL_LIQ"]);
                        var dinicio = Convert.ToDateTime(dr["INICIO"]);
                        var dfim = Convert.ToDateTime(dr["FIM"]);
                        var nfechamento = Convert.ToInt32(dr["N_FEC"]);
                        var datafech = Convert.ToString(dr["DT_FECH"]);                        

                        totalValAfe += valorafe;
                        totalQuantAfe += quantafe;
                        totalValorLiq += valorliq;

                        subTotalValAfe += valorafe;
                        subTotalQuantAfe += quantafe;
                        subTotalValorLiq += valorliq;

                        parcial.add(new Lote(codigo, valorafe, quantafe, taxa, valtaxa, valorliq, dinicio, dfim,
                                                    nfechamento, datafech));
                    }
                    parcial.SubTotalQuantAfe = subTotalQuantAfe;
                    parcial.SubTotalValAfe = subTotalValAfe;
                    parcial.SubTotalValorLiq = subTotalValorLiq;
                    totais.add(parcial);
                    totais.TotalQuantAfe = totalQuantAfe;
                    totais.TotalValAfe = totalValAfe;
                    totais.TotalValorLiq = totalValorLiq;
                }
                retorno = totais.Parciais.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch { retorno = "Ocorreu um erro durante a operação"; }
            return totais;
        }

        public DetalheLote DetalheLoteAberto(ObjConn objConn, DadosAcesso dadosAcesso, int codCre, int numFech, out string retorno)
        {
            retorno = string.Empty;
            var detalheLotesFechados = new DetalheLote();
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_EXTLOTAB_2";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCRE", DbType.Int32, codCre),
                               new Parametro("@CENTRALIZ", DbType.Int32, 0),
                               new Parametro("@NUMFECH", DbType.Int32, numFech),                               
                               new Parametro("@TIPO", DbType.Int32, 2),
                               new Parametro("@FORMATO", DbType.Int32, 2)                               
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);
            IDataReader dr = null;
            
            try
            {
                dr = db.ExecuteReader(cmd);
                var ListSubRede = new List<LoteSubRede>();
                var ListTaxas = new List<LoteTaxas>();
                while (dr.Read())
                {
                    if (Convert.ToString(dr["TIPO"]) == "COMPRAS")
                    {
                        var detalheLoteFechadoSubRede = new LoteSubRede();
                        detalheLoteFechadoSubRede.CodSubRede = Convert.ToInt32(dr["CODSUBREDE"]);
                        detalheLoteFechadoSubRede.Descricao = Convert.ToString(dr["DESCRICAO"]);
                        detalheLoteFechadoSubRede.Qtde = Convert.ToInt16(dr["QTE"]);
                        detalheLoteFechadoSubRede.Valor = Convert.ToDecimal(dr["VALOR"]);
                        detalheLoteFechadoSubRede.Taxa = Convert.ToDecimal(dr["TAXA"]);
                        detalheLoteFechadoSubRede.ValLiq = Convert.ToDecimal(dr["VAL_LIQ"]);
                        detalheLoteFechadoSubRede.CodCre = Convert.ToInt32(dr["CODCRE"]);
                        detalheLoteFechadoSubRede.NumFech = Convert.ToInt16(dr["N_FEC"]);
                        detalheLotesFechados.TotalQtdeTrans += detalheLoteFechadoSubRede.Qtde;
                        detalheLotesFechados.TotalValor += detalheLoteFechadoSubRede.Valor;
                        detalheLotesFechados.TotalValLiq += detalheLoteFechadoSubRede.ValLiq;
                        ListSubRede.Add(detalheLoteFechadoSubRede);
                    }

                    if (Convert.ToString(dr["TIPO"]) == "TAXAS")
                    {
                        var detalheLoteFechadoTaxas = new LoteTaxas();
                        detalheLoteFechadoTaxas.Descricao = Convert.ToString(dr["DESCRICAO"]);
                        detalheLoteFechadoTaxas.Qtde = Convert.ToInt16(dr["QTE"]);
                        detalheLoteFechadoTaxas.Valor = Convert.ToDecimal(dr["VALOR"]);
                        detalheLoteFechadoTaxas.CodCre = Convert.ToInt32(dr["CODCRE"]);
                        detalheLoteFechadoTaxas.NumFech = Convert.ToInt16(dr["N_FEC"]);
                        detalheLotesFechados.TotalValTaxas += detalheLoteFechadoTaxas.Valor;
                        ListTaxas.Add(detalheLoteFechadoTaxas);
                    }
                }
                detalheLotesFechados.ListSubRede = ListSubRede;
                detalheLotesFechados.ListTaxas = ListTaxas;
                retorno = detalheLotesFechados.ListSubRede.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
                dr.Close();
            }                
            catch { retorno = "Ocorreu um erro durante a operação"; }
            return detalheLotesFechados;
        }

        public List<Transacao> DetalheSubRedeAberto(ObjConn objConn, DadosAcesso dadosAcesso, int codCre, int numFech, int codSubRede, out string retorno)
        {
            retorno = string.Empty;
            var lista = new List<Transacao>();
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_EXTLOTAB_2";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCRE", DbType.Int32, codCre),
                               new Parametro("@CENTRALIZ", DbType.Int32, 0),
                               new Parametro("@NUMFECH", DbType.Int32, numFech),
                               new Parametro("@CODSUBREDE", DbType.Int32, codSubRede),
                               new Parametro("@TIPO", DbType.Int32, 1),
                               new Parametro("@FORMATO", DbType.Int32, 2)                               
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);
            
            try
            {
                var ds = Utils.ConvertDataReaderToDataSet(db.ExecuteReader(cmd));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lista.AddRange(from DataRow dr in ds.Tables[0].Rows
                                   let codigo = Convert.ToInt32(dr["CODCRE"])
                                   let razsoc = Convert.ToString(dr["RAZSOC"])
                                   let data = Convert.ToDateTime(dr["DATA"])
                                   let nsu = Convert.ToInt32(dr["AUTORIZ"])
                                   let tiptra = Convert.ToInt32(dr["TIPTRA"])
                                   let transacao = Convert.ToString(dr["TRANSACAO"])
                                   let cartao = Convert.ToString(dr["CARTAO"])
                                   let valor = Convert.ToDecimal(dr["VALTRA"])
                                   let valorliq = Convert.ToDecimal(dr["VAL_LIQ"])                                   
                                   select new Transacao(codSubRede, codigo, razsoc, Convert.ToString(data), nsu, cartao, valor, valorliq,
                                       tiptra, transacao));

                }
                retorno = lista.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch { retorno = "Ocorreu um erro durante a operação"; }
            return lista;
        }        

        public TotalComprasAberto ListaComprasAberto(ObjConn objConn, DadosAcesso dadosAcesso, string cpf, out string retorno)
        {
            retorno = string.Empty;
            var detalheComprasAberto = new TotalComprasAberto();
            var lista = new List<DetalheMovimento>();
            var totalValor = 0m;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_TRANS_ABERTO";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCLI", DbType.Int32, Convert.ToString(dadosAcesso.Codigo)),
                               new Parametro("@CPF", DbType.String, Utils.RetirarCaracteres(".-",cpf)),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            try
            {
                var ds = Utils.ConvertDataReaderToDataSet(db.ExecuteReader(cmd));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var drAux = ds.Tables[0].Rows[0];
                    detalheComprasAberto.TitNome = Convert.ToString(drAux["USUARIO"]);
                    detalheComprasAberto.Matricula = Convert.ToString(drAux["MATRICULA"]);
                    detalheComprasAberto.Filial = Convert.ToString(drAux["CODFIL"]);
                    detalheComprasAberto.Setor = Convert.ToString(drAux["CODSET"]);
                    detalheComprasAberto.Cpf = Convert.ToString(drAux["CPF"]);
                    
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        var tipotit = Convert.ToInt32(dr["TD"]);
                        var cartao = Convert.ToString(dr["CARTAO"]);
                        var data = Convert.ToDateTime(dr["DATA"]);
                        var lote = Convert.ToInt32(dr["LOTE"]);
                        var numaut = Convert.ToString(dr["AUTORIZ"]);
                        var valor = Convert.ToDecimal(dr["VALOR"]);
                        var desc = Convert.ToString(dr["TIPO"]);
                        var estab = Convert.ToString(dr["RAZSOC"]);
                        var codest = Convert.ToString(dr["CODCRE"]);
                        totalValor += valor;

                        lista.Add(new DetalheMovimento(tipotit, cartao, data.ToShortDateString(), lote, numaut, valor, desc, codest, estab));
                    }
                    detalheComprasAberto.Detalhe = lista;
                    detalheComprasAberto.TotalValor = totalValor;
                }
                retorno = lista.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch { retorno = "Ocorreu um erro durante a operação"; }
            return detalheComprasAberto;
        }
    }
}
