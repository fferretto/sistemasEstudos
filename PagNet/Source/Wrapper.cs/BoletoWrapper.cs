using Boleto2Net;

namespace Wrapper.cs
{
    public class BoletoWrapper
    {
        public BoletoWrapper(BoletoBancario boleto)
        {
            _boleto = boleto;
            _html = boleto.MontaHtml();
        }

        private string _html;
        private BoletoBancario _boleto;

        public string GetHtml()
        {
            return _html;
        }
    }
}
