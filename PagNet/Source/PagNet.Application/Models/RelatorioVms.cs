using PagNet.Domain.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PagNet.Application.Models
{
    public class RelatorioVms
    {
        public RelatorioVms()
        {
            listaCampos = new List<ParametrosRelatorioVm>();
        }

        public IList<ParametrosRelatorioVm> listaCampos { get; set; }

        public int codRel { get; set; }
        public string nmProc { get; set; }
        public string nmTela { get; set; }
        public string urlRelatorio { get; set; }

        internal static RelatorioVms ToViewRelatorio(PAGNET_RELATORIO item)
        {
            return new RelatorioVms
            {
                codRel = item.ID_REL,
                nmProc = item.NOMPROC,
                nmTela = item.DESCRICAO,
                urlRelatorio = "",
                listaCampos = ParametrosRelatorioVm.ToViewParametros(item.PAGNET_PARAMETRO_REL)
            };
        }


    }
    public class ParametrosRelatorioVm
    {
        public int ID_PAR { get; set; }
        public int ID_REL { get; set; }
        public string DESPAR { get; set; }
        public string NOMPAR { get; set; }
        public string LABEL { get; set; }
        public string TIPO { get; set; }
        public int TAMANHO { get; set; }
        public string _DEFAULT { get; set; }
        public string REQUERIDO { get; set; }
        public int ORDEM_TELA { get; set; }
        public int ORDEM_PROC { get; set; }
        public string NOM_FUNCTION { get; set; }
        public string VALCAMPO { get; set; }
        public string MASCARA { get; set; }
        public string TEXTOAJUDA { get; set; }

        internal static IList<ParametrosRelatorioVm> ToViewParametros<T>(ICollection<T> collection) where T : PAGNET_PARAMETRO_REL
        {
            return collection.Select(x => ToView(x)).ToList();
        }

        internal static ParametrosRelatorioVm ToView(PAGNET_PARAMETRO_REL x)
        {
            return new ParametrosRelatorioVm
            {
                ID_PAR = x.ID_PAR,
                ID_REL = x.ID_REL,
                DESPAR = x.DESPAR,
                NOMPAR = x.NOMPAR.Replace("@", ""),
                LABEL = x.LABEL,
                TIPO = x.TIPO,
                TAMANHO = x.TAMANHO,
                _DEFAULT = x._DEFAULT,
                REQUERIDO = x.REQUERIDO,
                ORDEM_TELA = x.ORDEM_TELA,
                ORDEM_PROC = x.ORDEM_PROC,
                NOM_FUNCTION = x.NOM_FUNCTION,
                MASCARA = x.MASCARA,
                TEXTOAJUDA = x.TEXTOAJUDA
            };
        }
    }
       
    public class ModRelPDFVm
    {
        public List<ModRel> Cabecalho { get; set; }
        public List<ModRel> Conteudo { get; set; }
        public string nmRelatorio { get; set; }
    }
    public class ModRel
    {
        public string LINHAIMP { get; set; }
        public int TIP { get; set; }
    }
    public class ItensGenerico : IEnumerable
    {
        public ItensGenerico(string value, string text, int tamanhoColuna, short sistema = -1)
        {
            Value = value;
            Text = text;
            TamanhoColuna = tamanhoColuna;
            Sistema = sistema; // -1 PJ/VA, 0 Somente PJ, 1 Somente VA
        }
        public string Value { get; set; }
        public string Text { get; set; }
        public int TamanhoColuna { get; set; }
        public short Sistema { get; set; }
        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)GetEnumerator(); }
        public ItensGenerico GetEnumerator() { return null; }
        #endregion
    }

    public class DetalhamentoFaturaReembolsoVm
    {
        public DetalhamentoFaturaReembolsoVm()
        {
            Detalhamento = new List<DetalhamentoValoresCobradosFaturaVm>();
        }

        public IList<DetalhamentoValoresCobradosFaturaVm> Detalhamento { get; set; }


        public string nroDocumento { get; set; }
        public string datEmissao { get; set; }
        public string datVencimento { get; set; }
        public string vlTotal { get; set; }

        //Dados Credor
        public string Credor { get; set; }
        public string CNPJCredor { get; set; }
        public string CEPCredor { get; set; }
        public string EnderecoCredor { get; set; }
        public string nroCredor { get; set; }
        public string ComplementoCredor { get; set; }
        public string BairroCredor { get; set; }
        public string CidadeCredor { get; set; }
        public string EstadoCredor { get; set; }
        public string TelefoneCredor { get; set; }

        //Dados Devedor
        public string Devedor { get; set; }
        public string CNPJDevedor { get; set; }
        public string CEPDevedor { get; set; }
        public string EnderecoDevedor { get; set; }
        public string nroDevedor { get; set; }
        public string ComplementoDevedor { get; set; }
        public string BairroDevedor { get; set; }
        public string CidadeDevedor { get; set; }
        public string EstadoDevedor { get; set; }
        public string TelefoneDevedor { get; set; }

        public string urlRelatorio { get; set; }
    }
    public class DetalhamentoValoresCobradosFaturaVm
    {
        public string Descricao { get; set; }
        public string Valor { get; set; }

    }
}
