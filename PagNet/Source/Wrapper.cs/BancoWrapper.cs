using Boleto2Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wrapper.cs
{
    public class BancoWrapper
    {
        public BancoWrapper(IBanco banco)
        {
            _banco = banco;
        }

        private IBanco _banco;

        public void FormataCedente()
        {
            _banco.FormataCedente();
        }
    }
}
