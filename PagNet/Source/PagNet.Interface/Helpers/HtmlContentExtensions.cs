using Microsoft.AspNetCore.Html;
using System;
//using System.Web.Routing;
using System.IO;
using System.Text.Encodings.Web;

namespace PagNet.Interface.Helpers
{
    public static class HtmlContentExtensions
    {
        public static string ToHtmlString(this IHtmlContent htmlContent)
        {
            using (var writer = new StringWriter())
            {
                htmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                return writer.ToString();
            }
        }

        public static IHtmlContent AppentTo(this IHtmlContent content, string text)
        {
            using (var writer = new StringWriter())
            {
                writer.Write(text);
                content.WriteTo(writer, HtmlEncoder.Default);
            }

            return content;
        }

        public static IHtmlContent AppentTo(this IHtmlContent content, IHtmlContent html)
        {
            using (var writer = new StringWriter())
            {
                writer.Write(html.ToString());
                content.WriteTo(writer, HtmlEncoder.Default);
            }

            return content;
        }
        /// <summary>
        ///     Adds a block of script to be rendered out at a later point in the page rendering when
        ///     <see cref="RenderScripts(System.Web.Mvc.HtmlHelper)" /> is called.
        /// </summary>
        /// <param name="htmlHelper">the <see cref="HtmlHelper" /></param>
        /// <param name="scriptTemplate">
        ///     the template for the block of script to render. The template must include the &lt;script
        ///     &gt; tags
        /// </param>
        /// <param name="renderOnAjax">
        ///     if set to <c>true</c> and the request is an AJAX request, the script will be written in the response.
        /// </param>
        /// <remarks>
        ///     A call to <see cref="RenderScripts(HtmlHelper)" /> will render all scripts.
        /// </remarks>
        //public static void AddScriptBlock(this IHtmlHelper htmlHelper, Func<dynamic, HelperResult> scriptTemplate, bool renderOnAjax = false)
        //{
        //    AddToScriptContext(htmlHelper, context => context.AddScriptBlock(scriptTemplate, renderOnAjax));
        //}
        /// <summary>
        ///     Performs an action on the current <see cref="ScriptContext" />
        /// </summary>
        /// <param name="htmlHelper">
        ///     the <see cref="HtmlHelper" />
        /// </param>
        /// <param name="action">the action to perform</param>
        //private static void AddToScriptContext(IHtmlHelper htmlHelper, Action<ScriptContext> action)
        //{
        //    var scriptContext =
        //        htmlHelper.ViewContext.HttpContext.Items[ScriptContext.ScriptContextItem] as ScriptContext;

        //    if (scriptContext == null)
        //    {
        //        throw new InvalidOperationException(
        //            "No ScriptContext in HttpContext.Items. Call Html.BeginScriptContext() to create a ScriptContext.");
        //    }

        //    action(scriptContext);
        //}
        public static string calculaLinha(string barra)
        {
            // Remover caracteres não numéricos.
            string linha = barra.Replace("[^0-9]", "");

            if (linha.Length != 44)
            {
                return null; // 'A linha do Código de Barras está incompleta!'
            }

            string campo1 = linha.Substring(0, 4) + linha.Substring(19, 20) + '.' + linha.Substring(20, 24);
            string campo2 = linha.Substring(24, 29) + '.' + linha.Substring(29, 34);
            string campo3 = linha.Substring(34, 39) + '.' + linha.Substring(39, 44);
            string campo4 = linha.Substring(4, 5); // Digito verificador
            string campo5 = linha.Substring(5, 19); // Vencimento + Valor

            if (modulo11Banco((linha.Substring(0, 4) + linha.Substring(5, 44))) != Convert.ToInt32(campo4))
            {
                return null; //'Digito verificador '+campo4+', o correto é '+modulo11_banco(  linha.substr(0,4)+linha.substr(5,99)  )+'\nO sistema não altera automaticamente o dígito correto na quinta casa!'
            }
            return campo1 + modulo10(campo1)
                    + ' '
                    + campo2 + modulo10(campo2)
                    + ' '
                    + campo3 + modulo10(campo3)
                    + ' '
                    + campo4
                    + ' '
                    + campo5
                    ;
        }

        public static int modulo10(string numero)
        {
            numero = numero.Replace("[^0-9]", "");
            int soma = 0;
            int peso = 2;
            int contador = numero.Length - 1;
            while (contador >= 0)
            {
                int multiplicacao = Convert.ToInt32(numero.Substring(contador, contador + 1)) * peso;
                if (multiplicacao >= 10) { multiplicacao = 1 + (multiplicacao - 10); }
                soma = soma + multiplicacao;
                if (peso == 2)
                {
                    peso = 1;
                }
                else
                {
                    peso = 2;
                }
                contador = contador - 1;
            }
            int digito = 10 - (soma % 10);
            if (digito == 10) digito = 0;

            return digito;
        }

        public static int modulo11Banco(string numero)
        {
            numero = numero.Replace("[^0-9]", "");

            int soma = 0;
            int peso = 2;
            int _base = 9;
            int contador = numero.Length - 1;
            for (int i = contador; i >= 0; i--)
            {
                soma = soma + (Convert.ToInt32(numero.Substring(i, i + 1)) * peso);
                if (peso < _base)
                {
                    peso++;
                }
                else
                {
                    peso = 2;
                }
            }
            int digito = 11 - (soma % 11);
            if (digito > 9) digito = 0;
            /* Utilizar o dígito 1(um) sempre que o resultado do cálculo padrão for igual a 0(zero), 1(um) ou 10(dez). */
            if (digito == 0) digito = 1;
            return digito;
        }
    }
}