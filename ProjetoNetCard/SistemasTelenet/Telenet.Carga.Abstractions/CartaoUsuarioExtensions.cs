
namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Extensões funcionais para números de cartão.
    /// </summary>
    public static class CartaoUsuarioExtensions
    {
        /// <summary>
        /// Mascara o número de um cartão de usuário para exibição pública.
        /// </summary>
        public static string ToNumeroMascarado(this string numero)
        {
            var tamanho = numero.Length;

            if (tamanho <= 6)
            {
                return numero;
            }

            if (tamanho <= 13)
            {
                return string.Concat(numero.Substring(0, 6), new string('*', 11));
            }

            return string.Concat(numero.Substring(0, 6), new string('*', 7), numero.Substring(13));
        }

        /// <summary>
        /// Mascara o número de um cartão de usuário para exibição pública.
        /// </summary>
        public static string ToNumeroMascarado(this ICartaoUsuario cartao)
        {
            if (cartao == null)
            {
                return string.Empty;
            }

            return cartao.Numero.ToNumeroMascarado();
        }
    }
}
