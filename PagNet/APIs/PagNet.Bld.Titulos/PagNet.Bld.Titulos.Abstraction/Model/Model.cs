using PagNet.Bld.Titulos.Abstraction.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.Titulos.Abstraction.Model
{
    public class APIRetornoModal
    {
        public bool sucesso { get; set; }
        public string msgRetorno { get; set; }
    }
    public class APIFiltroBorderoModel : IAPIFiltroBorderoModel
    {
        public APIFiltroBorderoModel()
        {
            ListaFechamento = new List<APIDadosTituloModel>();
        }

        public IList<APIDadosTituloModel> ListaFechamento { get; set; }

        public int codUsuario { get; set; }
        public int codOpe { get; set; }
        public string codEmpresa { get; set; }
        public int codigoContaCorrente { get; set; }
        public string codigoFormaPGTO { get; set; }
        public string codigoBanco { get; set; }

        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }
        public string DescJustOutros { get; set; }

        
        public string qtTitulosSelecionados { get; set; }

        public string ValorBordero { get; set; }

    }
    public class APIDadosTituloModel : IAPIDadosTituloModel
    {
        public bool Desconto { get; set; }
        public int codigoTitulo { get; set; }
        public int codigoEmpresa { get; set; }
        public int sistema { get; set; }
        public string codigoBanco_ORI { get; set; }
        public string codgioPGTO_ORI { get; set; }
        public string status { get; set; }
        public string nomeFavorecido { get; set; }
        public string CNPJ { get; set; }
        public int codigoBordero { get; set; }
        public int codigoFavorecido { get; set; }
        public string dataEmissao { get; set; }
        public string dataPagamento { get; set; }
        public string dataRealPagamento { get; set; }
        public string dataInicio { get; set; }
        public string DataFim { get; set; }
        public string codigoBanco { get; set; }
        public string Agencia { get; set; }
        public string dvAgencia { get; set; }
        public string contaCorrente { get; set; }
        public string dvContaCorrente { get; set; }
        public string valorLiquido { get; set; }
        public string valorConcedido { get; set; }
        public string novoValor { get; set; }
        public string valorBruto { get; set; }
        public string valorTaxa { get; set; }
        public string descricaoJustificativa { get; set; }
        public string codigoPlanoContas { get; set; }
        public string nomePlanoContas { get; set; }
        public string linhaDigitavel { get; set; }

        public string mensagemRetBanco { get; set; }
        public string formaPagamento { get; set; }
        public int codigoFormaPagamento { get; set; }
        public string quantidadeTransacao { get; set; }
        public string tipoTitulo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string descricaoTaxa { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class TaxasCobradasPGTOModel : ITaxasCobradasPGTOModel
    {
        public int CodigoTaxa { get; set; }
        public int CodigoTitulo { get; set; }
        public string Descrição { get; set; }
        public string Valor { get; set; }
        public string DataInclusao { get; set; }
        public string nmUsuario { get; set; }
        public string ValorTotal { get; set; }
        public decimal ValorTotal_aux { get; set; }

    }
    public class GridTitulosModel : IGridTitulosModel
    {
        public GridTitulosModel()
        {
            ListaTitulos = new List<APIDadosTituloModel>();
        }

        public IList<APIDadosTituloModel> ListaTitulos { get; set; }

        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }

        public int FormaPagamento { get; set; }
        public int codFormaPagamento { get; set; }

        public string dtTransferencia { get; set; }

        public string dtInicioGrid { get; set; }
        public string dtFimGrid { get; set; }
        public bool cartaoPre { get; set; }
        public bool cartaoPos { get; set; }
        public string CaminhoArquivo { get; set; }
        public string codigoEmpresa { get; set; }
        public string Justificativa { get; set; }
    }

    public class APIRetornoDDLModel : IAPIRetornoDDLModel
    {
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public string Title { get; set; }
    }
    public class LogTituloModel
    {        
        public int CODIGOTITULO { get; set; }
        public int CODTITULO_LOG { get; set; }
        public string STATUS { get; set; }
        public string CODBORDERO { get; set; }
        public string CODFAVORECIDO { get; set; }
        public string DATEMISSAO { get; set; }
        public string DATPGTO { get; set; }
        public string DATREALPGTO { get; set; }
        public string CODBANCO { get; set; }
        public string AGENCIA { get; set; }
        public string CONTACORRENTE { get; set; }
        public string VALLIQ { get; set; }
        public string CODEMPRESA { get; set; }
        public string SISTEMA { get; set; }
        public string CODUSUARIO { get; set; }
        public string NMUSUARIO { get; set; }
        public DateTime DATINCLOG { get; set; }
        public string DESCLOG { get; set; }
        public string VALTAXAS { get; set; }
        public string VALBRUTO { get; set; }
        public string VALTOTAL { get; set; }

    }
    public class BorderoPagamentoModel : IBorderoPagamentoModel
    {
        public BorderoPagamentoModel()
        {
            ListaBordero = new List<BorderoPagamentoModel>();
        }

        public string codigoBanco { get; set; }
        public int codigoBordero { get; set; }
        public string CaminhoArquivo { get; set; }
        public string codigoEmpresa { get; set; }
        public string codigoFormaPGTO { get; set; }
        public string status { get; set; }
        public string ValorBordero { get; set; }
        public DateTime dataBordero { get; set; }
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }


        public IList<BorderoPagamentoModel> ListaBordero { get; set; }

    }
    public class APIFiltroTitulosPagamentoModel : IAPIFiltroTitulosPagamentoModel
    {

        public int codFavorecido { get; set; }
        public string codBanco { get; set; }
        public string CaminhoArquivoDownload { get; set; }
        public bool acessoAdmin { get; set; }
        public bool cartaoPos { get; set; }
        public bool cartaoPre { get; set; }
        public string filtroCodBanco { get; set; }
        public string FiltroNmBanco { get; set; }
        public string filtro { get; set; }
        public string nmFavorecido { get; set; }
        public string dtInicio { get; set; }
        public string dtFim { get; set; }
        public string codEmpresa { get; set; }
        public string nmEmpresa { get; set; }
        public string CodFormaPagamento { get; set; }
        public string nmFormaPagamento { get; set; }
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }
    }


}
