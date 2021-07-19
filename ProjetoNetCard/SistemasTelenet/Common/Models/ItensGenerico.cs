using System.Collections;

namespace NetCard.Common.Models
{
    public class ItensGenerico : IEnumerable
    {
        public ItensGenerico(string value, string text, int tamanhoColuna)
        {
            Value = value;
            Text = text;
            TamanhoColuna = tamanhoColuna;
        }
        public string Value { get; set; }
        public string Text { get; set; }
        public int TamanhoColuna { get; set; }
        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)GetEnumerator(); }
        public ItensGenerico GetEnumerator() { return null; }
        #endregion
    }
}
