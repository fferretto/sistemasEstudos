using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PagNet.BLD.Relatorio.ModelAuxiliar
{

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
}
