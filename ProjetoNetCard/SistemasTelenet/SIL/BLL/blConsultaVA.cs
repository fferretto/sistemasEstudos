using DevExpress.Web.ASPxGridView;
using SIL.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using TELENET.SIL;
using TELENET.SIL.BL;
using TELENET.SIL.PO;
using Yogesh.ExcelXml;

namespace SIL.BL
{
    public class blConsultaVA
    {
        private readonly OPERADORA FOperador;
        private readonly bool subRede;

        public blConsultaVA(OPERADORA Operador)
            : this(Operador, 1)
        { }

        public blConsultaVA(OPERADORA Operador, short sistema)
        {
            FOperador = Operador;
            subRede = new blCLIENTEVA(FOperador).ExibeSubRede();
            _sistema = sistema;
        }

        private short _sistema;

        public bool Excluir(CONSULTA_VA ConsultaVA)
        {
            try
            {
                var ConsultaDAL = new daCONSULTAVA(FOperador);
                return ConsultaDAL.Excluir(ConsultaVA);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public bool Excluir(int codigoConsulta, int tipoConsulta)
        {
            try
            {
                var ConsultaDAL = new daCONSULTAVA(FOperador);
                return ConsultaDAL.Excluir(codigoConsulta, tipoConsulta);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public bool Incluir(CONSULTA_VA ConsultaVA)
        {
            try
            {
                var ConsultaDAL = new daCONSULTAVA(FOperador);
                return ConsultaDAL.Inserir(ConsultaVA);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public bool Alterar(CONSULTA_VA ConsultaVA)
        {
            try
            {
                var ConsultaDAL = new daCONSULTAVA(FOperador);
                return ConsultaDAL.Alterar(ConsultaVA);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public string ConsultaJustific(string datTra, string nsuHos, string nsuAut, string tipTra)
        {
            var ConsultaVADAL = new daCONSULTAVA(FOperador);
            var retorno = ConsultaVADAL.ConsultaJustific(datTra, nsuHos, nsuAut, tipTra);

            return retorno;
        }
        public string ConsultaJustificSegViaCard(string DAD)
        {
            var ConsultaVADAL = new daCONSULTAVA(FOperador);
            var retorno = ConsultaVADAL.ConsultaJustificSegViaCard(DAD);

            return retorno;
        }

        public List<CONSULTA_VA> ColecaoConsultas(string Filtro)
        {
            var ConsultaVADAL = new daCONSULTAVA(FOperador);
            return ConsultaVADAL.ColecaoConsultas(Filtro);
        }

        public DataTable BuscaConsultaListagem(string query)
        {
            var ConsultaVADAL = new daCONSULTAVA(FOperador);
            return ConsultaVADAL.GeraListagem(query);
        }

        public CREDENCIADO_VA CriaCredConsultaTrans(string razao, string codcre)
        {
            var cred = new CREDENCIADO_VA();
            cred.RAZSOC = razao;
            cred.CODCRE = codcre;
            return cred;
        }

        public CLIENTEVA_PREPAGO CriaCliConsultaTrans(string nomCli, int codcli)
        {
            var cli = new CLIENTEVA_PREPAGO();
            cli.NOMCLI = nomCli;
            cli.CODCLI = codcli;
            return cli;
        }

        public List<TipoResposta> InitColectionTipoResp()
        {
            var itensResposta = new List<TipoResposta>();
            var tipoV = new TipoResposta("Valida", 'V');
            itensResposta.Add(tipoV);
            var tipoP = new TipoResposta("Pendente", 'P');
            itensResposta.Add(tipoP);
            var tipoI = new TipoResposta("Invalida", 'I');
            itensResposta.Add(tipoI);
            var tipoC = new TipoResposta("Cancelada", 'C');
            itensResposta.Add(tipoC); 
            var tipoDes = new TipoResposta("Desfeita", 'X');
            itensResposta.Add(tipoDes);
            return itensResposta;
        }

        public static bool IsNaturalNumber(String strNumber)
        {
            var objNotNaturalPattern = new Regex("[^0-9]");
            var objNaturalPattern = new Regex("0*[1-9][0-9]*");
            return !objNotNaturalPattern.IsMatch(strNumber) &&
                   objNaturalPattern.IsMatch(strNumber);
        }

        public CONSULTA_VA RecuperarConsultaVA(int codigoConsulta)
        {
            return new daCONSULTAVA(FOperador).RecuperaConsultaByCodigo(codigoConsulta);
        }

        public IEnumerable<ItensGenerico> MontarListaTodasColunas()
        {
            IList<ItensGenerico> lista = new List<ItensGenerico>();
            if (subRede) lista.Add(new ItensGenerico(UtilSIL.SUBREDE, "NOME SUB-REDE", 150, 0));
            lista.Add(new ItensGenerico(UtilSIL.DATTRA, "DATA TRANSACAO", 130, 1));
            lista.Add(new ItensGenerico(UtilSIL.NSUHOS, "Nº HOST", 50, 2));
            lista.Add(new ItensGenerico(UtilSIL.NSUAUT, "Nº AUTORIZACAO", 80, 3));

            if (_sistema == 0 || _sistema == -1)
            {
                lista.Add(new ItensGenerico(UtilSIL.TVALOR, "VALOR TOTAL", 50, 4, 0));
                lista.Add(new ItensGenerico(UtilSIL.PARCELA, "PARCELA", 40, 5, 0));
                lista.Add(new ItensGenerico(UtilSIL.TPARCELA, "TOTAL PARCELAS", 40, 6, 0));
            }
            lista.Add(new ItensGenerico(UtilSIL.VALTRA, "VALOR", 50, 7));
            lista.Add(new ItensGenerico(UtilSIL.CODRTA, "RESPOSTA", 30, 8));

            lista.Add(new ItensGenerico(UtilSIL.CODCRE, "COD. CREDENCIADO", 50, 9));
            lista.Add(new ItensGenerico(UtilSIL.RAZSOC, "RAZAO SOCIAL CREDENCIADO", 200, 10));
            lista.Add(new ItensGenerico(UtilSIL.NOMFAN, "NOME FANTASIA CREDENCIADO", 200, 11));
            lista.Add(new ItensGenerico(UtilSIL.CGC, "CNPJ CREDENCIADO", 100, 12));
            lista.Add(new ItensGenerico(UtilSIL.DATFECCRE, "DATA FECH. CREDENCIADO", 130, 13));
            lista.Add(new ItensGenerico(UtilSIL.NUMFECCRE, "Nº FECH. CREDENCIADO", 50, 14));
            lista.Add(new ItensGenerico(UtilSIL.DATPGTOCRE, "DATA PAGTO. CREDENCIADO", 130, 15));
            lista.Add(new ItensGenerico(UtilSIL.CODCLI, "COD.CLIENTE", 50, 16));
            lista.Add(new ItensGenerico(UtilSIL.NOMCLI, "NOME CLIENTE", 200, 17));
            if (_sistema == 1 || _sistema == -1)
            {
                lista.Add(new ItensGenerico(UtilSIL.NUMCARG_VA, "Nº CARGA", 40, 18, 1));
            }
            if (_sistema == 0 || _sistema == -1)
            {
                lista.Add(new ItensGenerico(UtilSIL.DATFECCLI, "DATA FECH. CLIENTE", 130, 19, 0));
                lista.Add(new ItensGenerico(UtilSIL.NUMFECCLI, "Nº FECH. CLIENTE", 50, 20, 0));
                lista.Add(new ItensGenerico(UtilSIL.DATPGTOCLI, "DATA PAGTO. CLIENTE", 130, 21, 0));
            }

            lista.Add(new ItensGenerico(UtilSIL.CPF, "CPF", 100, 22));
            lista.Add(new ItensGenerico(UtilSIL.NOMUSU, "NOME USUARIO", 200, 23));
            lista.Add(new ItensGenerico(UtilSIL.CODCRT, "Nº CARTAO", 130, 24));
            lista.Add(new ItensGenerico(UtilSIL.MAT, "MATRICULA USUARIO", 50, 25));
            lista.Add(new ItensGenerico(UtilSIL.CODFIL, "FILIAL", 50, 26));
            lista.Add(new ItensGenerico(UtilSIL.CODSET, "SETOR", 50, 27));

            lista.Add(new ItensGenerico(UtilSIL.REDE, "REDE", 200, 28));

            lista.Add(new ItensGenerico(UtilSIL.TIPTRA, "TIPO TRANSACAO", 80, 29));
            lista.Add(new ItensGenerico(UtilSIL.ORIGEMOP, "ORIGEM OP", 80, 30));
            lista.Add(new ItensGenerico(UtilSIL.NOMOPERADOR, "NOME OPERADOR", 150, 31));
            lista.Add(new ItensGenerico(UtilSIL.DESTIPTRA, "DESCRICAO TIPO TRANSACAO", 150, 32));

            lista.Add(new ItensGenerico(UtilSIL.NOMSEG, "SEGMENTO", 200, 33));
            lista.Add(new ItensGenerico(UtilSIL.DAD_JUST, "DAD", 50, 34));           

            return lista;
        }

        public IEnumerable<ItensGenerico> ColunasVisualizadas(string colunasNaoVisualizadas)
        {
            if (colunasNaoVisualizadas == "-1" || colunasNaoVisualizadas == string.Empty)
                return MontarListaTodasColunas();
            if (!string.IsNullOrEmpty(colunasNaoVisualizadas))
            {
                var lista = MontarListaTodasColunas();
                var colunas = colunasNaoVisualizadas.Split(';');
                var cols = lista.Where(c => !colunas.Contains(c.Value));
                return cols.ToList();
            }
            return null;
        }

        public IEnumerable<ItensGenerico> ColunasNaoVisualizadas(string colunasNaoVisualizadas)
        {
            if (colunasNaoVisualizadas == "-1")
                return null;
            var lista = MontarListaTodasColunas();
            var colunas = colunasNaoVisualizadas.Split(';');
            var cols = lista.Where(c => colunas.Contains(c.Value));
            return cols.ToList();
        }

        public List<GridViewDataColumn> GeraColunas(IEnumerable<ItensGenerico> colunasVisualizadas)
        {
            var listaColunas = new List<GridViewDataColumn>();
            if (colunasVisualizadas == null || !colunasVisualizadas.Any())
                return null;

            //Estas colunas sao necessarias para processar as transacoes (o tratamento e somente visual mas as colunas sao criadas)
            var DATTRA = new GridViewDataColumn();
            DATTRA.FieldName = "DATTRA";
            DATTRA.Caption = "DATA TRANSACAO";
            DATTRA.VisibleIndex = 1;
            listaColunas.Add(DATTRA);
            
            var NSUHOS = new GridViewDataColumn();
            NSUHOS.FieldName = "NSUHOS";
            NSUHOS.Caption = "NºHOST";
            NSUHOS.VisibleIndex = 2;
            listaColunas.Add(NSUHOS);

            var NSUAUT = new GridViewDataColumn();
            NSUAUT.FieldName = "NSUAUT";
            NSUAUT.Caption = "NºAUTORIZACAO";
            NSUAUT.VisibleIndex = 3;
            listaColunas.Add(NSUAUT);

            var DATFECCRE = new GridViewDataColumn();
            DATFECCRE.FieldName = "DATFECCRE";
            DATFECCRE.Caption = "DATA FECH. CREDENCIADO";
            DATFECCRE.VisibleIndex = 12;
            listaColunas.Add(DATFECCRE);

            var TIPTRA = new GridViewDataColumn();
            TIPTRA.FieldName = "TIPTRA";
            TIPTRA.Caption = "TIPO TRANSACAO";
            TIPTRA.VisibleIndex = 28;
            listaColunas.Add(TIPTRA);

            var CODRTA = new GridViewDataColumn();
            CODRTA.FieldName = "CODRTA";
            CODRTA.Caption = "RESPOSTA";
            CODRTA.VisibleIndex = 8;
            CODRTA.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            CODRTA.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            listaColunas.Add(CODRTA);

            var FLAG_AUT = new GridViewDataColumn();
            FLAG_AUT.FieldName = "FLAG_AUT";
            FLAG_AUT.Caption = "FLAG_AUT";
            FLAG_AUT.VisibleIndex = 40;
            listaColunas.Add(FLAG_AUT);

            var DAD = new GridViewDataColumn();
            DAD.FieldName = "DAD";
            DAD.Caption = "INF. COMPLEMENTARES";
            DAD.VisibleIndex = 41;
            listaColunas.Add(DAD);

            foreach (var item in colunasVisualizadas)
            {
                if (item.Value == "DATTRA" || item.Value == "NSUAUT" || item.Value == "NSUHOS" || item.Value == "TIPTRA" ||
                    item.Value == "DATFECCRE" || item.Value == "CODRTA") continue;
                var column = new GridViewDataColumn();
                column.FieldName = item.Value;
                column.Caption = item.Text;
                column.VisibleIndex = item.OrdemTela;
                if (item.Value == "VALTRA" || item.Value == "TVALOR")
                    column.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                listaColunas.Add(column);
            }
            return listaColunas;
        }

        public string GeraExcel(CONSULTA_VA consulta, List<CTTRANSVA> result, string pathArquivo)
        {
            if (result == null)
                return string.Empty;

            var colunasVisualizadas = !string.IsNullOrEmpty(consulta.LISTA_COL)
                                                          ? ColunasVisualizadas(consulta.LISTA_COL).ToList()
                                                          : MontarListaTodasColunas().ToList();

            var excel = new ExcelXmlWorkbook {Properties = {Author = FOperador.LOGIN}};
            var planilha = excel[0];
            planilha.Name = "Listagem";

            if (colunasVisualizadas.Any())
            {
                for (var i = 0; i < colunasVisualizadas.Count(); i++)
                {
                    //Criei o cabecalho da coluna
                    planilha[i, 0].Value = colunasVisualizadas[i].Text;
                    var style = new XmlStyle
                                    {
                                        Interior = {Color = Color.LightGray},
                                        Font = {Bold = true, Name = "Calibri", Size = 11}
                                    };
                    planilha[i, 0].Style = style;
                    planilha.Columns(i).Width = colunasVisualizadas[i].TamanhoColuna;

                    for (var j = 0; j < result.Count; j++)
                    {
                        planilha[i, j + 1].Value = colunasVisualizadas[i].Text == "VALOR" //nome da coluna == VALOR
                                                       ? Convert.ToDecimal(result[j].GetCollumnValue(colunasVisualizadas[i].Value))
                                                       : result[j].GetCollumnValue(colunasVisualizadas[i].Value);
                    }
                }
            }

            var fileName = "Listagem" + DateTime.Now.Ticks + ".xls";
            var outputFile = pathArquivo + fileName;
            excel.Export(outputFile);
            return fileName;
        }

        public ACOESTRANS ConsultaAcoesTrans(int tipTra, string codrta, string datFecCli, string datFecCre, string flagAut, int sistema, int codCli, int numDep) 
        {
            return new daCONSULTAVA(FOperador).ConsultaAcoesTrans(tipTra, codrta, datFecCli, datFecCre, flagAut, sistema, codCli, numDep);
        }
    }
}
