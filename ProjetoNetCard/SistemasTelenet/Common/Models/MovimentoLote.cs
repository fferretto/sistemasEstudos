using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;

namespace NetCard.Common.Models
{
    public class MovimentoLote
    {
        public int Codigo { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public string Download { get; set; }
        public string DatFeclot { get; set; }
        public string SubRede { get; set; }
        public string DataPgto { get; set; }
        public decimal Total { get; set; }
        public decimal ComReceita { get; set; }
        public decimal AnuiTaxas { get; set; }
        public decimal Val2Via { get; set; }
        public decimal Compras { get; set; }
        public decimal Premio { get; set; }
        public decimal Beneficio { get; set; }
        public decimal DescontoFolha { get; set; }
        public string Fechado{ get; set; }

        public MovimentoLote(){}

        public MovimentoLote(int codigo, string dataini, string datafim, string datfeclot, string subRede,
            string dataPgto, decimal total, decimal comReceita, decimal anuiTaxas, decimal val2via, 
            decimal compras, decimal premio, decimal beneficio, decimal descontoFolha, string fechado )
        {
            Codigo = codigo;
            DataInicial = dataini;
            DataFinal = datafim;
            DatFeclot = datfeclot;
            SubRede = subRede;
            DataPgto = dataPgto;
            Total = total;
            ComReceita = comReceita;
            AnuiTaxas = anuiTaxas;
            Val2Via = val2via;
            Compras = compras;
            Premio = premio;
            Beneficio = beneficio;
            DescontoFolha = descontoFolha;
            Fechado = fechado;
        }
        
        public MovimentoLote(int codigo, string dataini, string datafim)
        {
            Codigo = codigo;
            DataInicial = dataini;
            DataFinal = datafim;
        }

        public List<MovimentoLote> ListaFechamento(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            var lista = new List<MovimentoLote>();
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_LOTES_CLIENTES_PJ";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCLI", DbType.Int32, Convert.ToString(dadosAcesso.Codigo)),
                               new Parametro("@SUB_REDE", DbType.Int32, 1),
                               new Parametro("@FORMATO", DbType.Int32, 2),
                               new Parametro("@CPF", DbType.String, dadosAcesso.Cpf)
                           };

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var subRede = dr["SUB_REDE"].ToString();
                    var codigo = Convert.ToInt32(dr["NUM_LOTE_CLIENTE"]);
                    var dataini = Convert.ToDateTime(dr["DT_INI_LOTE"]);
                    var datafim = Convert.ToDateTime(dr["DT_FIM_LOTE"]);
                    var dataPgto = Convert.ToDateTime(dr["DT_PGTO"]);
                    var datfeclot = string.IsNullOrEmpty(dr["DT_FECHTO_LOTE"].ToString()) ? string.Empty : (Convert.ToDateTime(dr["DT_FECHTO_LOTE"])).ToShortDateString();
                    var total = Convert.ToDecimal(dr["TOTAL"]);
                    var fechado = Convert.ToString(dr["FECHADO"]);                    
                    var comReceita = Convert.ToDecimal(dr["COM_RECEITA"]);
                    var anuiTaxas = Convert.ToDecimal(dr["ANUI_TAXAS"]);
                    var val2via = Convert.ToDecimal(dr["VALOR2AVIA"]);
                    var compras = Convert.ToDecimal(dr["COMPRAS"]);
                    var premio = Convert.ToDecimal(dr["PREMIO_UTILIZADO"]);
                    var beneficio = Convert.ToDecimal(dr["BENEFICIO"]);
                    var descontoFolha = Convert.ToDecimal(dr["DESCONTO_FOLHA"]);
                    lista.Add(new MovimentoLote(codigo, dataini.ToShortDateString(), datafim.ToShortDateString(),
                                            datfeclot, subRede, dataPgto.ToShortDateString(), total, comReceita, anuiTaxas, val2via,
                                            compras, premio, beneficio, descontoFolha, fechado));
                }
                retorno = lista.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
                dr.Close();
            }
            catch { retorno = "Ocorreu um erro durante a operação"; }
            return lista;
        }

        public DetalheTotalMovimento MovimentosDetalheLote(ObjConn objConn, DadosAcesso dadosAcesso, string numFech, string cpf, out string retorno)
        {
            var totais = new DetalheTotalMovimento();
            decimal total = 0, subtotal = 0;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_CLI_MOVTO_POSPAGO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@CODCLI", DbType.Int32, Convert.ToString(dadosAcesso.Codigo)),
                               new Parametro("@NUMFECH", DbType.Int32, numFech),
                               new Parametro("@CPF", DbType.String, Utils.RetirarCaracteres(".-", cpf)),
                               new Parametro("@TIPO", DbType.Int32, 1),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);
            
            try
            {
                var ds = Utils.ConvertDataReaderToDataSet(db.ExecuteReader(cmd));
                var drAux = ds.Tables[0].Rows[0];
                var nome = Convert.ToString(drAux["NOMETIT"]);
                var matricula = Convert.ToString(drAux["matricula"]);
                var filial = Convert.ToString(drAux["CODFIL"]);
                var setor = Convert.ToString(drAux["CODSET"]);
                cpf = Convert.ToString(drAux["CPF"]);
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
                        matricula = Convert.ToString(dr["MATRICULA"]);
                        filial = Convert.ToString(dr["CODFIL"]);
                        setor = Convert.ToString(dr["CODSET"]);
                        parcial = new DetalheParcialMovimento(nome, matricula, cpf, filial, setor);
                        subtotal = 0;
                        cpfAux = cpf;
                    }

                    var tipotit = Convert.ToInt32(dr["TITULAR"]);
                    var cartao = Convert.ToString(dr["CARTAO"]);
                    var data = Convert.ToString(dr["DATA"]);
                    var numaut = Convert.ToString(dr["AUTORIZ"]);
                    var valor = Convert.ToDecimal(dr["VALTRA"]);
                    var desc = Convert.ToString(dr["TRANSACAO"]);
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

        public DownFile DownloadArquivo(ObjConn objConn, DadosAcesso dadosAcesso, string id, string pathDestino, string folder, out string retorno)
        {
            var pathRaiz = pathDestino;
            var file = new DownFile();
            var path = ConfigurationManager.AppSettings["arqFechCli"] + folder + @"\CLIENTES\bkp\";
            var pathArqConcilia = ConfigurationManager.AppSettings["arqConcilia"] + folder + @"\CLIENTES\" + dadosAcesso.Codigo.ToString().PadLeft(5, '0');

            pathDestino += "CACL" + Convert.ToString(dadosAcesso.Codigo).PadLeft(5, '0') + "_" + id.PadLeft(3, '0') + ".zip";            
            
            if (!Directory.Exists(pathRaiz))
                Directory.CreateDirectory(pathRaiz);

            var zipOutPut = new ZipOutputStream(File.Create(pathDestino));

            zipOutPut.SetLevel(9);
            zipOutPut.Finish();
            zipOutPut.Close();

            var zip = new ZipFile(pathDestino);
            zip.BeginUpdate();

            try
            {
                var nomeZip = Path.Combine(path, "CA" + Convert.ToString(dadosAcesso.Codigo).PadLeft(5, '0') + "." + id.PadLeft(3, '0'));
                zip.NameTransform = new ZipNameTransform(nomeZip.Substring(0, nomeZip.LastIndexOf("\\", StringComparison.Ordinal)));
                zip.Add(nomeZip);

                nomeZip = Path.Combine(path, "CL" + Convert.ToString(dadosAcesso.Codigo).PadLeft(5, '0') + "_" + id.PadLeft(3, '0') + ".pdf");
                zip.NameTransform = new ZipNameTransform(nomeZip.Substring(0, nomeZip.LastIndexOf("\\", StringComparison.Ordinal)));
                zip.Add(nomeZip);

                nomeZip = Path.Combine(path, "CL" + Convert.ToString(dadosAcesso.Codigo).PadLeft(5, '0') + "_" + id.PadLeft(3, '0') + ".txt");
                zip.NameTransform = new ZipNameTransform(nomeZip.Substring(0, nomeZip.LastIndexOf("\\", StringComparison.Ordinal)));
                zip.Add(nomeZip);

                if (Directory.Exists(pathArqConcilia))
                {                    
                    DirectoryInfo Dir = new DirectoryInfo(pathArqConcilia);                    
                    FileInfo[] Files = Dir.GetFiles("*", SearchOption.AllDirectories);
                    var identCliFech = "_Cliente_" + dadosAcesso.Codigo.ToString().PadLeft(5, '0') + "_" + id;
                    var identCliFech2 = "ARQ" + dadosAcesso.Codigo.ToString().PadLeft(5, '0') + "_" + id.PadLeft(3, '0');

                    foreach (FileInfo File in Files)
                    {
                        if (File.Name.ToUpper().Replace('-','_').Contains(identCliFech.ToUpper()) || File.Name.ToUpper().Replace('-', '_').Contains(identCliFech2.ToUpper()))
                        {
                            nomeZip = File.FullName;
                            zip.NameTransform = new ZipNameTransform(nomeZip.Substring(0, nomeZip.LastIndexOf("\\", StringComparison.Ordinal)));
                            zip.Add(nomeZip);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                //throw new Exception(e.ToString());
                retorno = "Arquivos de fechamento nao encontrados.";
            }

            zip.CommitUpdate();
            zip.Close();

            file.Arquivo = pathDestino;
            file.Nome = "CACL" + Convert.ToString(dadosAcesso.Codigo).PadLeft(5, '0') + "_" + id.PadLeft(3, '0') + ".zip";

            retorno = "OK";
            
            return file;
        }
    }
}
