using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIL
{
    public static class StringExtensions
    {
        public static int ToINT32(this string value)
        {
            bool eInteiro = false;
            int numero = 0;

            eInteiro = Int32.TryParse(value, out numero);

            if (eInteiro)
                return numero;
            else
                return 0;
        }

        public static decimal ToDECIMAL(this string value)
        {
            bool eDecimal = false;
            decimal numero = 0;

            eDecimal = Decimal.TryParse(value, out numero);

            if (eDecimal)
                return numero;
            else
                return 0;
        }

        public static DateTime ? ToDATETIME(this string value)
        {
            bool eData = false;
            DateTime data = DateTime.Now;

            eData = DateTime.TryParse(value, out data);

            if (eData)
                return data;
            else
                return null;
        }
    }
}
