using System;

namespace PagNet.Application.Helpers
{
    public class AjudaAttribute : Attribute
    {
        private readonly string _ajuda = string.Empty;

        public AjudaAttribute(string ajuda)
        {
            _ajuda = ajuda;
        }
        public string Ajuda
        {
            get { return _ajuda; }
        }
    }
}