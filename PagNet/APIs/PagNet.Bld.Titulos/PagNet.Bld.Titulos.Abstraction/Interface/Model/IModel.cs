using PagNet.Bld.Titulos.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.Titulos.Abstraction.Interface.Model
{
    public interface IAPIFiltroBorderoModel
    {
        IList<APIDadosTituloModel> ListaFechamento { get; set; }

        int codUsuario { get; set; }
        int codOpe { get; set; }
        string codEmpresa { get; set; }
        int codigoContaCorrente { get; set; }
        string codigoFormaPGTO { get; set; }
        string codigoBanco { get; set; }
        string descJustificativa { get; set; }
        string qtTitulosSelecionados { get; set; }
        string ValorBordero { get; set; }

    }
    public interface IAPIFiltrotituloModel
    {
        int codigoTitulo { get; set; }
        string dataInicio { get; set; }
        string dataFim { get; set; }
        string status { get; set; }
        string codigoBanco { get; set; }
        int codigoEmpresa { get; set; }
        int codigoFavorecido { get; set; }
        int codigoContaCorrente { get; set; }
    }
    public interface IAPIDadosTituloModel
    {
        string tipoTitulo { get; set; }
        bool Desconto { get; set; }
        int codigoTitulo { get; set; }
        int codigoEmpresa { get; set; }
        int sistema { get; set; }
        string codigoBanco_ORI { get; set; }
        string codgioPGTO_ORI { get; set; }
        string status { get; set; }
        string nomeFavorecido { get; set; }
        string CNPJ { get; set; }
        int codigoBordero { get; set; }
        int codigoFavorecido { get; set; }
        string dataEmissao { get; set; }
        string dataPagamento { get; set; }
        string dataRealPagamento { get; set; }
        string codigoBanco { get; set; }
        string Agencia { get; set; }
        string dvAgencia { get; set; }
        string contaCorrente { get; set; }
        string dvContaCorrente { get; set; }
        string valorLiquido { get; set; }
        string valorConcedido { get; set; }
        string novoValor { get; set; }
        string valorBruto { get; set; }
        string valorTaxa { get; set; }
        string descricaoJustificativa { get; set; }
        string codigoPlanoContas { get; set; }
        string nomePlanoContas { get; set; }
        string linhaDigitavel { get; set; }

        string mensagemRetBanco { get; set; }
        string formaPagamento { get; set; }
        int codigoFormaPagamento { get; set; }
        string quantidadeTransacao { get; set; }
        string descricaoTaxa { get; set; }

    }
    public interface ITaxasCobradasPGTOModel
    {
        int CodigoTaxa { get; set; }
        int CodigoTitulo { get; set; }
        string Descrição { get; set; }
        string Valor { get; set; }
        string DataInclusao { get; set; }
        string nmUsuario { get; set; }
        string ValorTotal { get; set; }
        decimal ValorTotal_aux { get; set; }

    }
    public interface IGridTitulosModel
    {
        IList<APIDadosTituloModel> ListaTitulos { get; set; }
        string codContaCorrente { get; set; }
        string nmContaCorrente { get; set; }
        int FormaPagamento { get; set; }
        int codFormaPagamento { get; set; }
        string dtTransferencia { get; set; }
        string dtInicioGrid { get; set; }
        string dtFimGrid { get; set; }
        bool cartaoPre { get; set; }
        bool cartaoPos { get; set; }
        string CaminhoArquivo { get; set; }
        string codigoEmpresa { get; set; }
        string Justificativa { get; set; }
    }
    public interface IAPIRetornoDDLModel
    {
        string Valor { get; set; }
        string Descricao { get; set; }
        string Title { get; set; }
    }
    public interface IBorderoPagamentoModel
    {

        string codigoBanco { get; set; }
        int codigoBordero { get; set; }
        string CaminhoArquivo { get; set; }
        string codigoEmpresa { get; set; }
        string codigoFormaPGTO { get; set; }
        string status { get; set; }
        string ValorBordero { get; set; }
        DateTime dataBordero { get; set; }
        string codContaCorrente { get; set; }
        string nmContaCorrente { get; set; }

        IList<BorderoPagamentoModel> ListaBordero { get; set; }

    }
    public interface IAPIFiltroTitulosPagamentoModel
    {
        int codFavorecido { get; set; }
        string codBanco { get; set; }
        string CaminhoArquivoDownload { get; set; }
        bool acessoAdmin { get; set; }
        bool cartaoPos { get; set; }
        bool cartaoPre { get; set; }
        string filtroCodBanco { get; set; }
        string FiltroNmBanco { get; set; }
        string filtro { get; set; }
        string nmFavorecido { get; set; }
        string dtInicio { get; set; }
        string dtFim { get; set; }
        string codEmpresa { get; set; }
        string nmEmpresa { get; set; }
        string CodFormaPagamento { get; set; }
        string nmFormaPagamento { get; set; }
        string codContaCorrente { get; set; }
        string nmContaCorrente { get; set; }
    }
}
