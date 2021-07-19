using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Web.Routing;
using PagNet.Application.Helpers;
using Microsoft.AspNetCore.Html;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Features;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using PagNet.Application.Interface;
using PagNet.Application.Enum;
using PagNet.Interface.Areas.Relatorios.Models;
using PagNet.Api.Service.Model;

namespace PagNet.Interface.Helpers
{
    public static class HtmlCustom
    {
        public static TAttribute GetModelAttribute<TAttribute>
            (this ViewDataDictionary viewData, bool inherit = false) where TAttribute : Attribute
        {
            if (viewData == null) throw new ArgumentException("ViewData");
            var containerType = viewData.ModelMetadata.ContainerType;
            return
                ((TAttribute[])
                 containerType.GetProperty(viewData.ModelMetadata.PropertyName).
                 GetCustomAttributes(typeof(TAttribute),
                 inherit)).
                    FirstOrDefault();

        }
        public static IHtmlContent RenderScripts(this IHtmlHelper htmlHelper)
        {
            var scripts = htmlHelper.ViewContext.HttpContext.Items["Scripts"] as IList<string>;
            if (scripts != null)
            {
                var builder = new StringBuilder();
                foreach (var script in scripts)
                {
                    builder.AppendLine(script);
                }
                return new HtmlString(builder.ToString());
            }
            return null;
        }

        public static void AddCssClasses(this TagBuilder builder, params string[] cssClasses)
        {
            foreach (var cssClass in cssClasses)
                builder.AddCssClass(cssClass);
        }

        public static TValue GetAttributeValue<TAttribute, TValue>(this MemberInfo member, Func<TAttribute, TValue> getter, TValue defaultValue)
            where TAttribute : Attribute
        {
            var attribute = member.GetCustomAttribute<TAttribute>();
            return attribute.GetValue(getter, defaultValue);
        }

        public static TValue GetValue<TAttribute, TValue>(this TAttribute attribute, Func<TAttribute, TValue> getter, TValue defaultValue)
            where TAttribute : Attribute
        {
            return attribute == null
                ? defaultValue
                : getter(attribute);
        }
        public static IHtmlContent EditBootstrapFor2<TModel, TValue>(this IHtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, int tudoTamanho = 0, int inputTamanho = 12)
        {
            var member = (expression.Body as MemberExpression).Member;
            var ViewFullName = System.IO.Path.GetFileName(self.ViewContext.View.Path);
            var ViewExtension = System.IO.Path.GetExtension(self.ViewContext.View.Path);
            var ViewName = ViewFullName.Replace(ViewExtension, "");
            ViewName = ViewName.Replace(".", "");

            // Tenta definir o valor de maxLength a partir do valor em InputMaskAttribute 
            var inputMask = member.GetCustomAttribute<InputMaskAttribute>();
            var InputAttribute = member.GetCustomAttribute<InputAttrAux>();

            var ID = ViewName + "." + (expression.Body as MemberExpression).Member.Name;

            // Div principal.
            var mainDiv = new TagBuilder("div");

            if (inputMask != null && inputMask.Mask == "99/99/9999")
                mainDiv.AddCssClasses("form-group", "umadata", $"div{ID}", $"col-xs-{tudoTamanho}");
            else if (inputMask != null && inputMask.Mask == "99/9999")
                mainDiv.AddCssClasses("form-group", "ummes", $"col-xs-{tudoTamanho}");
            else
                mainDiv.AddCssClasses("form-group", $"col-xs-{tudoTamanho}", $"{ID}");

            bool valorMonetario = (inputMask != null && inputMask.Mask == "#.##0,00");
            // Adiciona o label do input na div principal.
            mainDiv.InnerHtml.AppendHtml(self.LabelFor(expression));

            // Incluir o valor " *" na div principal se o membro possuir RequiredAttribute.
            if (member.GetCustomAttribute<RequiredAttribute>() != null)
                mainDiv.InnerHtml.Append(" *");



            // Inclui ajuda no div principal se for definido o attributo AjudaAttribute e a propriedade Ajuda não for null ou somente espaços em branco.
            var ajuda = member.GetAttributeValue<AjudaAttribute, string>((a) => a.Ajuda, string.Empty);

            if (!string.IsNullOrWhiteSpace(ajuda))
            {
                var abbr = new TagBuilder("abbr");



                abbr.Attributes.Add("data-toggle", "tooltip");
                abbr.Attributes.Add("data-placement", "top");
                abbr.Attributes.Add("title", ajuda);

                var i = new TagBuilder("i");
                i.AddCssClasses("fa", "fa-question-circle");
                abbr.InnerHtml.AppendHtml(i);


                mainDiv.InnerHtml.AppendHtml(abbr);
            }


            // Define o tamanho máximo do input com o valor default.
            var maxLength = 200;

            // Tenta definir o valor de maxLength a partir do valor em StringLengthAttribute 
            var stringLength = member.GetCustomAttribute<StringLengthAttribute>();


            if (stringLength != null)
            {
                maxLength = stringLength.MaximumLength;
            }
            if (inputMask != null && inputMask.Mask == "99/99/9999")
            {
                maxLength = 10;
            }
            else if (inputMask != null && inputMask.Mask == "99/9999")
            {
                maxLength = 7;
            }
            // Declara a div do input.
            var divInput = new TagBuilder("div");
            divInput.AddCssClasses("input-group", $"col-xs-{inputTamanho}");

            if (InputAttribute != null && InputAttribute.TemInicio)
            {
                divInput.InnerHtml.AppendHtml($"<span class='input-group-addon' id='span{ID}'>{InputAttribute.Inicio}</span>");
            }


            string Tipo = "text";


            if (member.GetCustomAttribute<DataTypeAttribute>() != null)
            {
                Tipo = member.GetAttributeValue<DataTypeAttribute, string>((a) => a.DataType.ToString(), string.Empty);
            }

            // Define os atributos do input.
            var inputAttributes = new Dictionary<string, object>
            {
                { "class", "form-control" },
                { "id", ID },
                { "name", ID },
                { "type", Tipo },
                { "maxlength", maxLength }
            };

            if (inputMask != null)
            {
                if (inputMask.IsReverso)
                {
                    if (valorMonetario)
                    {
                        inputAttributes = new Dictionary<string, object>
                    {
                        { "class", "form-control text-right umValorMonetario" },
                        { "id", ID },
                        { "name", ID },
                        { "type", Tipo },
                        { "data-mask-reverse", "True"},
                        { "data-mask",inputMask.Mask },
                        { "maxlength", maxLength },
                        { "onKeyPress", "return(FormataMoeda(this,'.',',',event))" }
                    };
                    }
                    else
                    {
                        inputAttributes = new Dictionary<string, object>
                    {
                        { "class", "form-control text-right" },
                        { "id", ID },
                        { "name", ID },
                        { "type", Tipo },
                        { "data-mask-reverse", "True"},
                        { "data-mask",inputMask.Mask },
                        { "maxlength", maxLength }
                    };
                    }
                }
                else
                {
                    inputAttributes = new Dictionary<string, object>
                    {
                        { "class", "form-control" },
                        { "id", ID },
                        { "name", ID },
                        { "type", Tipo },
                        { "data-mask", inputMask.Mask },
                        { "maxlength", maxLength }
                    };

                }
            }


            // Adiciona o input na div dele.
            divInput.InnerHtml.AppendHtml(self.TextBoxFor(expression, inputAttributes));

            if (InputAttribute != null && InputAttribute.TemFinal)
            {
                divInput.InnerHtml.AppendHtml($"<span class='input-group-addon' id='span{ID}'>{InputAttribute.Final}</span>");
            }
            // Adiciona a div do input na div principal.
            mainDiv.InnerHtml.AppendHtml(divInput);

            // Adiciona o span na div principal.
            var span = new TagBuilder("span");
            span.AddCssClasses("field-validation-valid", "text-danger");
            span.Attributes.Add("data-valmsg-for", member.Name);
            span.Attributes.Add("data-valmsg-replace", "true");

            mainDiv.InnerHtml.AppendHtml(span);

            return mainDiv;
        }

        public static IHtmlContent EditBootstrapFor<TModel, TValue>(this IHtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, int tudoTamanho = 0, int inputTamanho = 12)
        {
            var member = (expression.Body as MemberExpression).Member;


            // Tenta definir o valor de maxLength a partir do valor em InputMaskAttribute 
            var inputMask = member.GetCustomAttribute<InputMaskAttribute>();
            var InputAttribute = member.GetCustomAttribute<InputAttrAux>();

            // Div principal.
            var mainDiv = new TagBuilder("div");

            if (inputMask != null && inputMask.Mask == "99/99/9999")
                mainDiv.AddCssClasses("form-group", "umadata", $"div{(expression.Body as MemberExpression).Member.Name}", $"col-xs-{tudoTamanho}");
            else if (inputMask != null && inputMask.Mask == "99/9999")
                mainDiv.AddCssClasses("form-group", "ummes", $"col-xs-{tudoTamanho}");
            else
                mainDiv.AddCssClasses("form-group", $"col-xs-{tudoTamanho}", $"{(expression.Body as MemberExpression).Member.Name}");

            bool valorMonetario = (inputMask != null && inputMask.Mask == "#.##0,00");
            // Adiciona o label do input na div principal.
            mainDiv.InnerHtml.AppendHtml(self.LabelFor(expression));

            // Incluir o valor " *" na div principal se o membro possuir RequiredAttribute.
            if (member.GetCustomAttribute<RequiredAttribute>() != null)
                mainDiv.InnerHtml.Append(" *");


            // Inclui ajuda no div principal se for definido o attributo AjudaAttribute e a propriedade Ajuda não for null ou somente espaços em branco.
            var ajuda = member.GetAttributeValue<AjudaAttribute, string>((a) => a.Ajuda, string.Empty);

            if (!string.IsNullOrWhiteSpace(ajuda))
            {
                var abbr = new TagBuilder("abbr");

                abbr.Attributes.Add("data-toggle", "tooltip");
                abbr.Attributes.Add("data-placement", "top");
                abbr.Attributes.Add("title", ajuda);

                var i = new TagBuilder("i");
                i.AddCssClasses("fa", "fa-question-circle");
                abbr.InnerHtml.AppendHtml(i);

                mainDiv.InnerHtml.AppendHtml(abbr);
            }


            // Define o tamanho máximo do input com o valor default.
            var maxLength = 200;

            // Tenta definir o valor de maxLength a partir do valor em StringLengthAttribute 
            var stringLength = member.GetCustomAttribute<StringLengthAttribute>();


            if (stringLength != null)
            {
                maxLength = stringLength.MaximumLength;
            }
            if (inputMask != null && inputMask.Mask == "99/99/9999")
            {
                maxLength = 10;
            }
            else if (inputMask != null && inputMask.Mask == "99/9999")
            {
                maxLength = 7;
            }
            // Declara a div do input.
            var divInput = new TagBuilder("div");
            divInput.AddCssClasses("input-group", $"col-xs-{inputTamanho}");

            if (InputAttribute != null && InputAttribute.TemInicio)
            {
                divInput.InnerHtml.AppendHtml($"<span class='input-group-addon' id='span{(expression.Body as MemberExpression).Member.Name}'>{InputAttribute.Inicio}</span>");
            }

            string Tipo = "text";

            if (InputAttribute != null && InputAttribute.TemTypo)
            {
                Tipo = InputAttribute.Type.ToString();
                if (InputAttribute.Type.ToString() == "number")
                {
                    maxLength = 0;
                }
            }

            if (member.GetCustomAttribute<DataTypeAttribute>() != null)
            {
                Tipo = member.GetAttributeValue<DataTypeAttribute, string>((a) => a.DataType.ToString(), string.Empty);
            }

            // Define os atributos do input.
            var inputAttributes = new Dictionary<string, object>
            {
                { "id", (expression.Body as MemberExpression).Member.Name },
                { "name", (expression.Body as MemberExpression).Member.Name },
                { "type", Tipo }
            };

            if (inputMask != null && inputMask.IsReverso)
            {
                inputAttributes.Add("data-mask-reverse", "True");
                inputAttributes.Add("data-mask", inputMask.Mask);
            }

            if (valorMonetario)
            {
                inputAttributes.Add("class", "form-control text-right umValorMonetario");
                inputAttributes.Add("onKeyPress", "return(FormataMoeda(this,'.',',',event))");
            }
            else
            {
                inputAttributes.Add("class", "form-control");
            }

            if (InputAttribute != null && InputAttribute.TemValorMaximo)
            {
                inputAttributes.Add("max", InputAttribute.ValorMaximo);
            }
            if (InputAttribute != null && InputAttribute.TemValorMinimo)
            {
                inputAttributes.Add("min", InputAttribute.ValorMinimo);
            }
            if (maxLength > 0)
            {
                inputAttributes.Add("maxlength", maxLength);
            }

            // Adiciona o input na div dele.
            divInput.InnerHtml.AppendHtml(self.TextBoxFor(expression, inputAttributes));

            if (InputAttribute != null && InputAttribute.TemFinal)
            {
                divInput.InnerHtml.AppendHtml($"<span class='input-group-addon' id='span{(expression.Body as MemberExpression).Member.Name}'>{InputAttribute.Final}</span>");
            }
            // Adiciona a div do input na div principal.
            mainDiv.InnerHtml.AppendHtml(divInput);

            // Adiciona o span na div principal.
            var span = new TagBuilder("span");
            span.AddCssClasses("field-validation-valid", "text-danger");
            span.Attributes.Add("data-valmsg-for", member.Name);
            span.Attributes.Add("data-valmsg-replace", "true");

            mainDiv.InnerHtml.AppendHtml(span);

            return mainDiv;
        }

        public static IHtmlContent EditBootstrapFor3<TModel, TValue>(this IHtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, int tudoTamanho = 0, int inputTamanho = 12)
        {
            var member = (expression.Body as MemberExpression).Member;


            // Tenta definir o valor de maxLength a partir do valor em InputMaskAttribute 
            var inputMask = member.GetCustomAttribute<InputMaskAttribute>();
            var InputAttribute = member.GetCustomAttribute<InputAttrAux>();

            // Div principal.
            var mainDiv = new TagBuilder("div");

            if (inputMask != null && inputMask.Mask == "99/99/9999")
                mainDiv.AddCssClasses("form-group", "umadata", $"div{(expression.Body as MemberExpression).Member.Name}", $"col-xs-{tudoTamanho}");
            else if (inputMask != null && inputMask.Mask == "99/9999")
                mainDiv.AddCssClasses("form-group", "ummes", $"col-xs-{tudoTamanho}");
            else
                mainDiv.AddCssClasses("form-group", $"col-xs-{tudoTamanho}", $"{(expression.Body as MemberExpression).Member.Name}");

            bool valorMonetario = (inputMask != null && inputMask.Mask == "#.##0,00");
            // Adiciona o label do input na div principal.
            mainDiv.InnerHtml.AppendHtml(self.LabelFor(expression));

            // Incluir o valor " *" na div principal se o membro possuir RequiredAttribute.
            if (member.GetCustomAttribute<RequiredAttribute>() != null)
                mainDiv.InnerHtml.Append(" *");


            // Inclui ajuda no div principal se for definido o attributo AjudaAttribute e a propriedade Ajuda não for null ou somente espaços em branco.
            var ajuda = member.GetAttributeValue<AjudaAttribute, string>((a) => a.Ajuda, string.Empty);

            if (!string.IsNullOrWhiteSpace(ajuda))
            {
                var abbr = new TagBuilder("abbr");



                abbr.Attributes.Add("data-toggle", "tooltip");
                abbr.Attributes.Add("data-placement", "top");
                abbr.Attributes.Add("title", ajuda);

                var i = new TagBuilder("i");
                i.AddCssClasses("fa", "fa-question-circle");
                abbr.InnerHtml.AppendHtml(i);


                mainDiv.InnerHtml.AppendHtml(abbr);
            }


            // Define o tamanho máximo do input com o valor default.
            var maxLength = 200;

            // Tenta definir o valor de maxLength a partir do valor em StringLengthAttribute 
            var stringLength = member.GetCustomAttribute<StringLengthAttribute>();


            if (stringLength != null)
            {
                maxLength = stringLength.MaximumLength;
            }
            if (inputMask != null && inputMask.Mask == "99/99/9999")
            {
                maxLength = 10;
            }
            else if (inputMask != null && inputMask.Mask == "99/9999")
            {
                maxLength = 7;
            }
            // Declara a div do input.
            var divInput = new TagBuilder("div");
            divInput.AddCssClasses("input-group", $"col-xs-{inputTamanho}");

            if (InputAttribute != null && InputAttribute.TemInicio)
            {
                divInput.InnerHtml.AppendHtml($"<span class='input-group-addon' id='span{(expression.Body as MemberExpression).Member.Name}'>{InputAttribute.Inicio}</span>");
            }

            string Tipo = "text";

            if (InputAttribute != null && InputAttribute.TemTypo)
            {
                Tipo = InputAttribute.Type.ToString();
                if (InputAttribute.Type.ToString() == "number")
                {
                    maxLength = 0;
                }
            }

            if (member.GetCustomAttribute<DataTypeAttribute>() != null)
            {
                Tipo = member.GetAttributeValue<DataTypeAttribute, string>((a) => a.DataType.ToString(), string.Empty);
            }

            // Define os atributos do input.
            var inputAttributes = new Dictionary<string, object>
            {
                { "id", (expression.Body as MemberExpression).Member.Name },
                { "name", (expression.Body as MemberExpression).Member.Name },
                { "type", Tipo },
                { "rows", "6" }
            };

            if (inputMask != null && inputMask.IsReverso)
            {
                inputAttributes.Add("data-mask-reverse", "True");
                inputAttributes.Add("data-mask", inputMask.Mask);
            }

            if (valorMonetario)
            {
                inputAttributes.Add("class", "form-control text-right umValorMonetario");
                inputAttributes.Add("onKeyPress", "return(FormataMoeda(this,'.',',',event))");
            }
            else
            {
                inputAttributes.Add("class", "form-control");
            }

            if (InputAttribute != null && InputAttribute.TemValorMaximo)
            {
                inputAttributes.Add("max", InputAttribute.ValorMaximo);
            }
            if (InputAttribute != null && InputAttribute.TemValorMinimo)
            {
                inputAttributes.Add("min", InputAttribute.ValorMinimo);
            }
            if (maxLength > 0)
            {
                inputAttributes.Add("maxlength", maxLength);
            }

            // Adiciona o input na div dele.
            divInput.InnerHtml.AppendHtml(self.TextAreaFor(expression, inputAttributes));

            if (InputAttribute != null && InputAttribute.TemFinal)
            {
                divInput.InnerHtml.AppendHtml($"<span class='input-group-addon' id='span{(expression.Body as MemberExpression).Member.Name}'>{InputAttribute.Final}</span>");
            }
            // Adiciona a div do input na div principal.
            mainDiv.InnerHtml.AppendHtml(divInput);

            // Adiciona o span na div principal.
            var span = new TagBuilder("span");
            span.AddCssClasses("field-validation-valid", "text-danger");
            span.Attributes.Add("data-valmsg-for", member.Name);
            span.Attributes.Add("data-valmsg-replace", "true");

            mainDiv.InnerHtml.AppendHtml(span);

            return mainDiv;
        }

        public static IHtmlContent CheckBoxBootstrapFor<TModel>(this IHtmlHelper<TModel> self, Expression<Func<TModel, bool>> expression)
        {
            var member = (expression.Body as MemberExpression).Member;

            // Div principal.
            var mainDiv = new TagBuilder("div");
            mainDiv.AddCssClasses("checkbox");


            // Adiciona o label do input na div principal.
            mainDiv.InnerHtml.AppendHtml(self.CheckBoxFor(expression));
            mainDiv.InnerHtml.AppendHtml(self.LabelFor(expression));

            // Incluir o valor " *" na div principal se o membro possuir RequiredAttribute.
            if (member.GetCustomAttribute<RequiredAttribute>() != null)
                mainDiv.InnerHtml.Append(" *");


            // Inclui ajuda no div principal se for definido o attributo AjudaAttribute e a propriedade Ajuda não for null ou somente espaços em branco.
            var ajuda = member.GetAttributeValue<AjudaAttribute, string>((a) => a.Ajuda, string.Empty);
            if (!string.IsNullOrWhiteSpace(ajuda))
            {
                var abbr = new TagBuilder("abbr");
                abbr.Attributes.Add("data-toggle", "tooltip");
                abbr.Attributes.Add("data-placement", "top");
                abbr.Attributes.Add("title", ajuda);

                var i = new TagBuilder("i");
                i.AddCssClasses("fa", "fa-question-circle");
                abbr.InnerHtml.AppendHtml(i);

                mainDiv.InnerHtml.AppendHtml(abbr);
            }


            return mainDiv;

        }
        public static IHtmlContent DDLBootstrapFor<TModel>(this IHtmlHelper<TModel> self, Expression<Func<TModel, String>> expression, String descricao, String url, int Tamanho = 0)
        {
            var member = (expression.Body as MemberExpression).Member;
            var stringLength = member.GetCustomAttribute<StringLengthAttribute>();
            var ajudaAtr = member.GetAttributeValue<AjudaAttribute, string>((a) => a.Ajuda, string.Empty);
            string fieldName = ((MemberExpression)expression.Body).Member.Name;
            var fieldValue = self.ViewData.Model.GetPropertyValue<TModel, string>(fieldName);


            int maxLength = 200;
            if (stringLength != null)
                maxLength = stringLength.MaximumLength;



            int largura = Convert.ToInt32(Math.Sqrt(maxLength * 1.8));
            if (largura > 12) largura = 12;

            // Div principal.
            var mainDiv = new TagBuilder("div");
            if (Tamanho > 0)
            {
                mainDiv.AddCssClasses("form-group", $"col-md-{Tamanho}");
            }
            else
            {
                mainDiv.AddCssClasses("form-group", $"col-md-{largura}");
            }


            // Declara a div do DropDown.
            var divDropDown = new TagBuilder("div");
            //divDropDown.AddCssClasses($"col-xs-{largura}");
            //divDropDown.AddCssClasses("cssDropDownList", $"col-xs-{largura}");


            // Adiciona o label do dropdownlist na div principal.
            divDropDown.InnerHtml.AppendHtml(self.LabelFor(expression));

            // Incluir o valor " *" na div principal se o membro possuir RequiredAttribute.
            if (member.GetCustomAttribute<RequiredAttribute>() != null)
                divDropDown.InnerHtml.Append(" *");

            // Inclui ajuda no div principal se for definido o attributo AjudaAttribute e a propriedade Ajuda não for null ou somente espaços em branco.
            if (!string.IsNullOrWhiteSpace(ajudaAtr))
            {
                var abbr = new TagBuilder("abbr");
                abbr.Attributes.Add("data-toggle", "tooltip");
                abbr.Attributes.Add("data-placement", "top");
                abbr.Attributes.Add("title", ajudaAtr);

                var i = new TagBuilder("i");
                i.AddCssClasses("fa", "fa-question-circle");
                abbr.InnerHtml.AppendHtml(i);

                divDropDown.InnerHtml.AppendHtml(abbr);
            }
            divDropDown.InnerHtml.AppendHtml($"<select class='form-control PagNet-ddl {fieldName}' id='{fieldName}'  name='{fieldName}' PagNet-ddl-url='{url}' PagNet-ddl-pai='nulo' PagNet-ddl-filho='nulo' >");
            divDropDown.InnerHtml.AppendHtml($"<option value='{fieldValue}'> {descricao} </option></select>");

            mainDiv.InnerHtml.AppendHtml(divDropDown);

            // Adiciona o span na div principal.
            var span = new TagBuilder("span");
            span.AddCssClasses("field-validation-valid", "text-danger");
            span.Attributes.Add("data-valmsg-for", member.Name);
            span.Attributes.Add("data-valmsg-replace", "true");

            mainDiv.InnerHtml.AppendHtml(span);

            return mainDiv;

        }
        public static IHtmlContent DDLbtnDinamic<TModel>(this IHtmlHelper<TModel> self, string nomeClasse, String TextoExibicao, String url, int Tamanho = 12)
        {

            // Div principal.
            var mainDiv = new TagBuilder("div");
            mainDiv.AddCssClasses($"col-md-{Tamanho}");


            // Declara a div do DropDown.
            var divDropDown = new TagBuilder("div");
            divDropDown.AddCssClasses("dropdownInView", nomeClasse);
            divDropDown.Attributes.Add("id", nomeClasse);
            divDropDown.Attributes.Add("name", nomeClasse);
            divDropDown.Attributes.Add("PagNet-ddl-url", url);

            divDropDown.InnerHtml.AppendHtml($"<div style = 'cursor: Pointer' class='dropbtn'>{TextoExibicao}</div>");

            mainDiv.InnerHtml.AppendHtml(divDropDown);

            return mainDiv;

        }

        //public static IHtmlContent DDLBootstrapFor<TModel>(this HtmlHelper<TModel> self, Expression<Func<TModel, String>> expression, Expression<Func<TModel, String>> descricao, String url, Expression<Func<TModel, String>> pai, Expression<Func<TModel, String>> filho)
        //{
        //    string pais = null;
        //    if (pai != null)
        //        pais = ((MemberExpression)pai.Body).Member.Name;

        //    return DDLBootstrapFor(self, expression, descricao, url, pais., filho);

        //}
        public static IHtmlContent DDLBootstrapFor<TModel>(this IHtmlHelper<TModel> self, Expression<Func<TModel, String>> expression, String descricao, String url, String pais, Expression<Func<TModel, String>> filho, int Tamanho = 0)
        {
            var member = (expression.Body as MemberExpression).Member;
            var stringLength = member.GetCustomAttribute<StringLengthAttribute>();
            var ajuda = member.GetAttributeValue<AjudaAttribute, string>((a) => a.Ajuda, string.Empty);
            string fieldName = ((MemberExpression)expression.Body).Member.Name;
            var fieldValue = self.ViewData.Model.GetPropertyValue<TModel, string>(fieldName);


            string fieldFilho = "nulo";
            if (filho != null)
                fieldFilho = ((MemberExpression)filho.Body).Member.Name;

            string fieldPai = "nulo";
            if (!String.IsNullOrWhiteSpace(pais))
                fieldPai = pais;

            int maxLength = 200;
            if (stringLength != null)
                maxLength = stringLength.MaximumLength;


            int largura = Convert.ToInt32(Math.Sqrt(maxLength * 1.8));
            if (largura > 12) largura = 12;

            // Div principal.
            var mainDiv = new TagBuilder("div");
            if (Tamanho > 0)
            {
                mainDiv.AddCssClasses("form-group", $"col-md-{Tamanho}");
            }
            else
            {
                mainDiv.AddCssClasses("form-group", $"col-md-{largura}");
            }


            // Declara a div do DropDown.
            var divDropDown = new TagBuilder("div");
            //divDropDown.AddCssClasses($"col-xs-{largura}");
            //divDropDown.AddCssClasses("cssDropDownList", $"col-xs-{largura}");


            // Adiciona o label do dropdownlist na div principal.
            divDropDown.InnerHtml.AppendHtml(self.LabelFor(expression));

            // Incluir o valor " *" na div principal se o membro possuir RequiredAttribute.
            if (member.GetCustomAttribute<RequiredAttribute>() != null)
                divDropDown.InnerHtml.Append(" *");

            // Inclui ajuda no div principal se for definido o attributo AjudaAttribute e a propriedade Ajuda não for null ou somente espaços em branco.
            if (!string.IsNullOrWhiteSpace(ajuda))
            {
                var abbr = new TagBuilder("abbr");
                abbr.Attributes.Add("data-toggle", "tooltip");
                abbr.Attributes.Add("data-placement", "top");
                abbr.Attributes.Add("title", ajuda);

                var i = new TagBuilder("i");
                i.AddCssClasses("fa", "fa-question-circle");
                abbr.InnerHtml.AppendHtml(i);

                divDropDown.InnerHtml.AppendHtml(abbr);
            }
            divDropDown.InnerHtml.AppendHtml($"<select class='form-control PagNet-ddl {fieldName}' id='{fieldName}' name='{fieldName}' PagNet-ddl-url='{url}' PagNet-ddl-pai='{fieldPai}' PagNet-ddl-filho='{fieldFilho}' >");
            divDropDown.InnerHtml.AppendHtml($"<option value='{fieldValue}'> {descricao} </option></select>");

            mainDiv.InnerHtml.AppendHtml(divDropDown);

            // Adiciona o span na div principal.
            var span = new TagBuilder("span");
            span.AddCssClasses("field-validation-valid", "text-danger");
            span.Attributes.Add("data-valmsg-for", member.Name);
            span.Attributes.Add("data-valmsg-replace", "true");

            mainDiv.InnerHtml.AppendHtml(span);

            return mainDiv;
        }
        public static IHtmlContent DinamicParamRel<TModel>(this IHtmlHelper<TModel> self, APIParametrosRelatorioModel model, int cont, string nmParam = "")
        {
            // Div principal.
            var mainDiv = new TagBuilder("div");

            string fieldName = nmParam + $"[{cont}].VALCAMPO";
            mainDiv.InnerHtml.AppendHtml($"<input id= '{nmParam + $"[{cont}].ORDEM_PROC"}' name='{nmParam + $"[{cont}].ORDEM_PROC"}' type='hidden' value='{model.ORDEM_PROC}'>");
            mainDiv.InnerHtml.AppendHtml($"<input id= '{nmParam + $"[{cont}].ID_PAR"}' name='{nmParam + $"[{cont}].ID_PAR"}' type='hidden' value='{model.ID_PAR}'>");

            if (model.LABEL.ToUpper() == "DATEEDIT")
            {
                mainDiv.AddCssClasses("form-group", "umadata", $"col-xs-4");
                if (string.IsNullOrWhiteSpace(model._DEFAULT))
                {
                    model._DEFAULT = DateTime.Now.ToShortDateString();
                }
            }
            else if (model.LABEL.ToUpper() == "MESEDIT")
                mainDiv.AddCssClasses("form-group", "ummes", $"col-xs-3");
            else if (model.NOMPAR == "CODFAVORECIDO")
                mainDiv.AddCssClasses("form-group");
            else
                mainDiv.AddCssClasses("form-group", $"col-xs-12");

            var pagNetUser = self.ViewContext.HttpContext.RequestServices.GetService<IPagNetUser>();
            var isAdmin = pagNetUser.isAdministrator;

            //configura o label
            string label = $"<label for='{fieldName}'>{model.DESPAR}</label>";

            // Inclui ajuda no div principal se for definido o attributo AjudaAttribute e a propriedade Ajuda não for null ou somente espaços em branco.
            var ajuda = model.TEXTOAJUDA;
            var abbr = new TagBuilder("abbr");
            if (!string.IsNullOrWhiteSpace(ajuda))
            {
                abbr.Attributes.Add("data-toggle", "tooltip");
                abbr.Attributes.Add("data-placement", "top");
                abbr.Attributes.Add("title", ajuda);

                var i = new TagBuilder("i");
                i.AddCssClasses("fa", "fa-question-circle");
                abbr.InnerHtml.AppendHtml(i);

            }

            // Define o tamanho máximo do input com o valor default.
            var maxLength = 200;

            // Tenta definir o valor de maxLength a partir do valor em StringLengthAttribute 
            var stringLength = model.TAMANHO;

            if (stringLength > 0)
                maxLength = stringLength;

            int largura = Convert.ToInt32(Math.Sqrt(maxLength * 1.8));
            if (largura > 12) largura = 12;

            if (model.LABEL.ToUpper() == "DATEEDIT") largura = 12;


            if (model.LABEL.ToUpper() == "DROPDOWNLIST")
            {
                // Adiciona o label do input na div principal.
                mainDiv.InnerHtml.AppendHtml(label);

                // Incluir o valor " *" na div principal se o membro possuir RequiredAttribute.
                if (model.REQUERIDO == "S")
                    mainDiv.InnerHtml.Append(" *");

                if (!string.IsNullOrWhiteSpace(ajuda))
                {
                    mainDiv.InnerHtml.AppendHtml(abbr);
                }


                // Declara a div do DropDown.
                var divDropDown = new TagBuilder("div");
                divDropDown.AddCssClasses("cssDropDownListDinamic", $"col-xs-12");


                var codEmpresa = 0;
                var nmEmpresa = "";
                var disabled = "";

                if (model.NOMPAR == "CODEMPRESA")
                {
                    codEmpresa = pagNetUser.cod_empresa;
                    var _diversos = self.ViewContext.HttpContext.RequestServices.GetService<IDiversosApp>();

                    nmEmpresa = _diversos.GetnmEmpresaByID(codEmpresa);

                    if (!isAdmin)
                    {
                        disabled = "disabled";
                    }
                    divDropDown.InnerHtml.AppendHtml($"<select class='{model.NOMPAR} form-control PagNet-ddl {fieldName}' name='{fieldName}' id='{fieldName}' PagNet-ddl-url='{model.NOM_FUNCTION}' PagNet-ddl-pai='nulo' PagNet-ddl-filho='nulo' {disabled}>");
                    divDropDown.InnerHtml.AppendHtml($"<option value='{codEmpresa}'> {nmEmpresa} </option></select>");

                }
                else
                {
                    divDropDown.InnerHtml.AppendHtml($"<select class='{model.NOMPAR} form-control PagNet-ddl {fieldName}' name='{fieldName}' id='{fieldName}' PagNet-ddl-url='{model.NOM_FUNCTION}' PagNet-ddl-pai='nulo' PagNet-ddl-filho='nulo' {disabled}>");
                    divDropDown.InnerHtml.AppendHtml($"<option value='-1'> '' </option></select>");
                }
                                             

                mainDiv.InnerHtml.AppendHtml(divDropDown);
            }
            else if (model.LABEL.ToUpper() == "CHECKBOX")
            {
                // Declara a div do DropDown.
                var divCheckBox = new TagBuilder("div");
                divCheckBox.AddCssClasses("cssCheckBox", $"col-xs-6");
                divCheckBox.InnerHtml.AppendHtml($"<input data-val='true' data-val-required='The Todas as Sub Redes field is required.' id='{fieldName}' name='{fieldName}' type='checkbox' value='true'>");
                divCheckBox.InnerHtml.AppendHtml($"<span>{label}</span>");

                // Incluir o valor " *" na div principal se o membro possuir RequiredAttribute.
                if (model.REQUERIDO == "S")
                    divCheckBox.InnerHtml.Append(" *");

                mainDiv.InnerHtml.AppendHtml(divCheckBox);
            }
            else
            {
                if (model.NOMPAR == "CODFAVORECIDO")
                {
                    mainDiv.InnerHtml.AppendHtml(self.ViewContext.HttpContext.MontaCampoCodFavorecido(label, model.REQUERIDO, ajuda, largura, model.NOMPAR, fieldName, maxLength, model.MASCARA, model._DEFAULT));
                }
                else
                {
                    // Adiciona o label do input na div principal.
                    mainDiv.InnerHtml.AppendHtml(label);
                    // Incluir o valor " *" na div principal se o membro possuir RequiredAttribute.
                    if (model.REQUERIDO == "S")
                        mainDiv.InnerHtml.Append(" *");

                    if (!string.IsNullOrWhiteSpace(ajuda))
                    {
                        mainDiv.InnerHtml.AppendHtml(abbr);
                    }

                    // Declara a div do input.
                    var divInput = new TagBuilder("div");
                    divInput.AddCssClasses("input-group", $"col-xs-{largura}");

                    string Tipo = "text";

                    // Define os atributos do input.
                    var input = $"<input class='{model.NOMPAR} form-control' id='{fieldName}' maxlength='{maxLength}' name='{fieldName}' type='{Tipo}' value='{model._DEFAULT}'>";

                    if (!string.IsNullOrEmpty(model.MASCARA))
                    {
                        input = $"<input class='{model.NOMPAR} form-control' id='{fieldName}' maxlength='{maxLength}' name='{fieldName}' type='{Tipo}' data-mask='{model.MASCARA}' value='{model._DEFAULT}'>";
                    }
                    // Declara a div do input.
                    divInput.InnerHtml.AppendHtml(input);

                    if (model.LABEL.ToUpper() == "DATEEDIT")
                        divInput.InnerHtml.AppendHtml("<span class='input-group-addon' id='spandtFim'><i class='fa fa-calendar' aria-hidden='true'></i></span>");

                    // Adiciona a div do input na div principal.
                    mainDiv.InnerHtml.AppendHtml(divInput);
                }

            }

            // Adiciona o span na div principal.
            var span = new TagBuilder("span");
            span.AddCssClasses("field-validation-valid", "text-danger");
            span.Attributes.Add("data-valmsg-for", fieldName);
            span.Attributes.Add("data-valmsg-replace", "true");

            mainDiv.InnerHtml.AppendHtml(span);

            return mainDiv;
        }
        private static TagBuilder MontaCampoCodFavorecido(this HttpContext context, string label, string REQUERIDO, string ajuda, int largura, string NOMPAR, string fieldName, int maxLength, string MASCARA, string _DEFAULT)
        {

            // Div principal.
            var mainDiv = new TagBuilder("div");

            // -------------------------------MONTA O PRIMEIRO CAMPO, O QUE IRÁ ARMAZENAR O CÓDIGO ----------------------------

            var divCampo1 = new TagBuilder("div");
            divCampo1.AddCssClasses("form-group", $"col-xs-4");

            // Adiciona o label do input na div principal.
            divCampo1.InnerHtml.AppendHtml(label);

            // Incluir o valor " *" na div principal se o membro possuir RequiredAttribute.
            if (REQUERIDO == "S")
                divCampo1.InnerHtml.Append(" *");

            if (!string.IsNullOrWhiteSpace(ajuda))
            {
                var abbr = new TagBuilder("abbr");
                if (!string.IsNullOrWhiteSpace(ajuda))
                {
                    abbr.Attributes.Add("data-toggle", "tooltip");
                    abbr.Attributes.Add("data-placement", "top");
                    abbr.Attributes.Add("title", ajuda);

                    var i = new TagBuilder("i");
                    i.AddCssClasses("fa", "fa-question-circle");
                    abbr.InnerHtml.AppendHtml(i);

                }

                divCampo1.InnerHtml.AppendHtml(abbr);
            }
            else
            {
                // Incluir o valor " *" na div principal se o membro possuir RequiredAttribute.
                if (REQUERIDO == "S")
                    divCampo1.InnerHtml.Append(" *");
            }

            var divInputCodigo = new TagBuilder("div");
            divInputCodigo.AddCssClasses("input-group", $"col-xs-12");

            string Tipo = "text";

            // Define os atributos do input.
            var inputCampo1 = $"<input class='{NOMPAR} form-control' id='{fieldName}' maxlength='20' name='{fieldName}' type='{Tipo}' value='{_DEFAULT}'>";

            // Declara a div do input.
            divInputCodigo.InnerHtml.AppendHtml(inputCampo1);

            divCampo1.InnerHtml.AppendHtml(divInputCodigo);

            // Adiciona a div do input na div principal.
            mainDiv.InnerHtml.AppendHtml(divCampo1);

            // -------------------------------MONTA O SEGUNDO CAMPO, O QUE IRÁ ARMAZENAR O NOME ----------------------------

            var divCampo2 = new TagBuilder("div");
            divCampo2.AddCssClasses("form-group", $"col-xs-6");

            // Adiciona o label do input na div principal.
            divCampo2.InnerHtml.AppendHtml("<label for='nomeFavorecido'>Nome do Favorecido</label>");

            var abbr2 = new TagBuilder("abbr");
            if (!string.IsNullOrWhiteSpace(ajuda))
            {
                abbr2.Attributes.Add("data-toggle", "tooltip");
                abbr2.Attributes.Add("data-placement", "top");
                abbr2.Attributes.Add("title", "Nome do Favorecido que irá receber o pagamento");

                var i = new TagBuilder("i");
                i.AddCssClasses("fa", "fa-question-circle");
                abbr2.InnerHtml.AppendHtml(i);

            }

            divCampo2.InnerHtml.AppendHtml(abbr2);

            var divInputNome = new TagBuilder("div");
            divInputNome.AddCssClasses("input-group", $"col-xs-12");
            var valorCampo = "";
            if (!string.IsNullOrWhiteSpace(_DEFAULT))
            {
                var metodosGerais = context.RequestServices.GetService<MetodosGerais>();
                var valorAux = metodosGerais.RetornaFavorecido(_DEFAULT, 0).FirstOrDefault();
                valorCampo = valorAux.Value;
            }
            // Define os atributos do input.
            var inputCampo2 = $"<input class='NOMEFAVORECIDO form-control' id='NOMEFAVORECIDO' maxlength='200' name='NOMEFAVORECIDO' type='text' value='{valorCampo}'>";

            // Declara a div do input.
            divInputNome.InnerHtml.AppendHtml(inputCampo2);

            divCampo2.InnerHtml.AppendHtml(divInputNome);

            // Adiciona a div do input na div principal.
            mainDiv.InnerHtml.AppendHtml(divCampo2);

            // -------------------------------MONTA O TERCEIRO CAMPO, BOTÃO DE PESQUISA ----------------------------


            var divCampo3 = new TagBuilder("div");
            divCampo3.InnerHtml.AppendHtml("<div class='form-group col-md-2' id='btnLocalizarFavorecido'>");

            // Adiciona o label do input na div principal.
            divCampo3.InnerHtml.AppendHtml("<label></label>");

            var divInputBtn = new TagBuilder("div");
            divInputBtn.AddCssClasses("input-group", $"col-xs-12");

            // Define os atributos do input.
            var inputCampo3 = $"<div class='btn btn-success' onclick='LocalizaFavorecido();'>Localizar</div>";

            // Declara a div do input.
            divInputBtn.InnerHtml.AppendHtml(inputCampo3);

            divCampo3.InnerHtml.AppendHtml(divInputBtn);

            // Adiciona a div do input na div principal.
            mainDiv.InnerHtml.AppendHtml(divCampo3);

            divCampo1.InnerHtml.AppendHtml("</div>");
            return mainDiv;
        }
        public static IHtmlContent ShowError(this IHtmlHelper self)
        {
            if (self.TempData == null || self.TempData.Count == 0)
                return new HtmlString("");

            var stringBuilder = new StringBuilder();
            var arrayMensagem = "";

            stringBuilder.AppendLine("<script>");
            stringBuilder.AppendLine("listAviso.push([");

            foreach (var itemSecreto in self.TempData)
            {
                if (itemSecreto.Key.Length > 5 && itemSecreto.Key.Substring(0, 5) == "Avis.")
                {
                    var no = itemSecreto.Key.Split(".");
                    arrayMensagem += $"['{no[0]}', '{no[1]}', '{Convert.ToString(itemSecreto.Value)}'],";
                }
            }
            self.TempData.Clear();
            stringBuilder.AppendLine(arrayMensagem.Substring(0, arrayMensagem.Length - 1));
            stringBuilder.AppendLine("]);");
            stringBuilder.AppendLine("showError();");
            stringBuilder.AppendLine("</script>");

            return new HtmlString(stringBuilder.ToString());
        }
        private static IEnumerable<SelectListItem> GetCheckboxListWithDefaultValues(object defaultValues, IEnumerable<SelectListItem> selectList)
        {
            var defaultValuesList = defaultValues as IEnumerable;

            if (defaultValuesList == null)
                return selectList;

            IEnumerable<string> values = from object value in defaultValuesList
                                         select Convert.ToString(value, CultureInfo.CurrentCulture);

            var selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
            var newSelectList = new List<SelectListItem>();

            selectList.ToList().ForEach(item =>
            {
                item.Selected = (item.Value != null) ? selectedValues.Contains(item.Value) : selectedValues.Contains(item.Text);
                newSelectList.Add(item);
            });

            return newSelectList;
        }


        private static string JsEncode(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            int i;
            int len = s.Length;

            StringBuilder sb = new StringBuilder(len + 4);
            string t;

            for (i = 0; i < len; i += 1)
            {
                char c = s[i];
                switch (c)
                {
                    case '>':
                    case '"':
                    case '\\':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        //sb.Append("\\n");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\r':
                        //sb.Append("\\r");
                        break;
                    default:
                        if (c < ' ')
                        {
                            //t = "000" + Integer.toHexString(c); 
                            string tmp = new string(c, 1);
                            t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                            sb.Append("\\u" + t.Substring(t.Length - 4));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            return sb.ToString();
        }
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var xmlRequest = request.Headers["X-Requested-With"];

            if (xmlRequest != StringValues.Empty && xmlRequest == "XMLHttpRequest")
                return true;

            return false;
        }
        public static IHtmlContent CampoRecursivoPlanoConas<TModel>(this IHtmlHelper<TModel> self, IEnumerable<ListaPlanoContasVm> dados, int Tamanho)
        {
            // Div principal.
            var mainDiv = new TagBuilder("tbody");
            bool PodeRemover = true;
            foreach (var pai in dados)
            {
                PodeRemover = true;

                var divTR = new TagBuilder("tr");
                divTR.Attributes.Add("Style", "background-color:#d6d3d3;cursor:Pointer;");

                var divTD0 = new TagBuilder("td");
                divTD0.Attributes.Add("width", "10%");
                divTD0.InnerHtml.AppendLine(pai.CLASSIFICACAO);
                divTR.InnerHtml.AppendHtml(divTD0);

                var divTD1 = new TagBuilder("td");
                divTD1.Attributes.Add("width", "20%");
                divTD1.InnerHtml.AppendLine(pai.DESCRICAO);
                divTR.InnerHtml.AppendHtml(divTD1);

                var divTD2 = new TagBuilder("td");
                divTD2.Attributes.Add("width", "12%");
                divTD2.Attributes.Add("align", "center");
                divTD2.InnerHtml.AppendLine(pai.TIPO);
                divTR.InnerHtml.AppendHtml(divTD2);

                var divTD3 = new TagBuilder("td");
                divTD3.Attributes.Add("width", "12%");
                divTD3.Attributes.Add("align", "center");
                divTD3.InnerHtml.AppendLine(pai.NATUREZA);
                divTR.InnerHtml.AppendHtml(divTD3);

                var divTD4 = new TagBuilder("td");
                divTD4.Attributes.Add("width", "12%");
                divTD4.Attributes.Add("align", "center");
                divTD4.InnerHtml.AppendLine(pai.DESPESA);
                divTR.InnerHtml.AppendHtml(divTD4);

                var divTD5 = new TagBuilder("td");
                divTD5.Attributes.Add("width", "12%");
                divTD5.Attributes.Add("align", "center");
                divTD5.InnerHtml.AppendLine(pai.UTILIZADOPAGAMENTO);
                divTR.InnerHtml.AppendHtml(divTD5);

                var divTD6 = new TagBuilder("td");
                divTD6.Attributes.Add("width", "12%");
                divTD6.Attributes.Add("align", "center");
                divTD6.InnerHtml.AppendLine(pai.UTILIZADORECEBIMENTO);
                divTR.InnerHtml.AppendHtml(divTD6);

                if (pai.ListaFilho.Count > 0)
                {
                    var Filho = ListaFilhoPlanoContas(pai.ListaFilho, 4, ref PodeRemover);

                    var divTD7 = new TagBuilder("td");
                    divTD7.Attributes.Add("width", "5%");
                    divTD7.Attributes.Add("align", "center");
                    divTD7.InnerHtml.AppendHtmlLine($"<div class='btn btn-warning' onclick='EditarPlanoContas({pai.CODPLANOCONTAS});' style='font-size:11px'>");
                    divTD7.InnerHtml.AppendHtmlLine("<i class='fa fa-edit'></i>");
                    divTD7.InnerHtml.AppendHtmlLine("</div>");
                    divTR.InnerHtml.AppendHtml(divTD7);

                    var divTD8 = new TagBuilder("td");
                    divTD8.Attributes.Add("width", "5%");
                    divTD8.Attributes.Add("align", "center");
                    if (PodeRemover && (pai.UTILIZADORECEBIMENTO != "Sim" && pai.UTILIZADOPAGAMENTO != "Sim"))
                    {
                        divTD8.InnerHtml.AppendHtmlLine($"<div class='btn btn-danger' onclick='DesativaPlanoConta({pai.CODPLANOCONTAS});' style='font-size:11px'>");
                        divTD8.InnerHtml.AppendHtmlLine("<i class='fa fa-trash'></i>");
                        divTD8.InnerHtml.AppendHtmlLine("</div>");
                    }
                    divTR.InnerHtml.AppendHtml(divTD8);

                    var divTD9 = new TagBuilder("td");
                    divTD9.AddCssClass("UTILIZADOPAGAMENTO");
                    divTD9.Attributes.Add("width", "0%");
                    divTD9.Attributes.Add("style", "display:none;");
                    divTD9.InnerHtml.AppendHtmlLine($"<input class='' type='hidden' name='UTILIZADOPAGAMENTO' value='{pai.UTILIZADOPAGAMENTO}' />");
                    divTR.InnerHtml.AppendHtml(divTD9);

                    var divTD10 = new TagBuilder("td");
                    divTD10.AddCssClass("UTILIZADORECEBIMENTO");
                    divTD10.Attributes.Add("width", "0%");
                    divTD10.Attributes.Add("style", "display:none;");
                    divTD10.InnerHtml.AppendHtmlLine($"<input class='' type='hidden' name='UTILIZADORECEBIMENTO' value='{pai.UTILIZADORECEBIMENTO}' />");
                    divTR.InnerHtml.AppendHtml(divTD10);

                    mainDiv.InnerHtml.AppendHtml(divTR);
                    mainDiv.InnerHtml.AppendHtml(Filho);
                }
                else
                {

                    var divTD7 = new TagBuilder("td");
                    divTD7.Attributes.Add("width", "5%");
                    divTD7.Attributes.Add("align", "center");
                    divTD7.InnerHtml.AppendHtmlLine($"<div class='btn btn-warning' onclick='EditarPlanoContas({pai.CODPLANOCONTAS});' style='font-size:11px'>");
                    divTD7.InnerHtml.AppendHtmlLine("<i class='fa fa-edit'></i>");
                    divTD7.InnerHtml.AppendHtmlLine("</div>");
                    divTR.InnerHtml.AppendHtml(divTD7);

                    var divTD8 = new TagBuilder("td");
                    divTD8.Attributes.Add("width", "5%");
                    divTD8.Attributes.Add("align", "center");
                    if (PodeRemover && (pai.UTILIZADORECEBIMENTO != "Sim" && pai.UTILIZADOPAGAMENTO != "Sim"))
                    {
                        divTD8.InnerHtml.AppendHtmlLine($"<div class='btn btn-danger' onclick='DesativaPlanoConta({pai.CODPLANOCONTAS});' style='font-size:11px'>");
                        divTD8.InnerHtml.AppendHtmlLine("<i class='fa fa-trash'></i>");
                        divTD8.InnerHtml.AppendHtmlLine("</div>");
                    }
                    divTR.InnerHtml.AppendHtml(divTD8);

                    var divTD9 = new TagBuilder("td");
                    divTD9.AddCssClass("UTILIZADOPAGAMENTO");
                    divTD9.Attributes.Add("width", "0%");
                    divTD9.Attributes.Add("style", "display:none;");
                    divTD9.InnerHtml.AppendHtmlLine($"<input class='' type='hidden' name='UTILIZADOPAGAMENTO' value='{pai.UTILIZADOPAGAMENTO}' />");
                    divTR.InnerHtml.AppendHtml(divTD9);

                    var divTD10 = new TagBuilder("td");
                    divTD10.AddCssClass("UTILIZADORECEBIMENTO");
                    divTD10.Attributes.Add("width", "0%");
                    divTD10.Attributes.Add("style", "display:none;");
                    divTD10.InnerHtml.AppendHtmlLine($"<input class='' type='hidden' name='UTILIZADORECEBIMENTO' value='{pai.UTILIZADORECEBIMENTO}' />");
                    divTR.InnerHtml.AppendHtml(divTD10);

                    mainDiv.InnerHtml.AppendHtml(divTR);
                }

            }


            return mainDiv;
        }

        public static IHtmlContent ListaFilhoPlanoContas(IList<ListaPlanoContasVm> ListaFilho, int qtEspaco, ref bool PodeRemover)
        {
            // Div principal.
            var mainDiv = new TagBuilder("tr");
            var espaco = "";

            for (int i = 0; i < qtEspaco; i++)
            {
                espaco = espaco + "&nbsp;";
            }
            foreach (var item in ListaFilho)
            {
                var divTR = new TagBuilder("tr");
                divTR.Attributes.Add("Style", "cursor:Pointer;");

                var divTD0 = new TagBuilder("td");
                divTD0.Attributes.Add("width", "10%");
                divTD0.InnerHtml.AppendLine(item.CLASSIFICACAO);
                divTR.InnerHtml.AppendHtml(divTD0);

                var divTD1 = new TagBuilder("td");
                divTD1.Attributes.Add("width", "25%");
                divTD1.InnerHtml.AppendHtmlLine(espaco + item.DESCRICAO);
                divTR.InnerHtml.AppendHtml(divTD1);

                var divTD2 = new TagBuilder("td");
                divTD2.Attributes.Add("width", "12%");
                divTD2.Attributes.Add("align", "center");
                divTD2.InnerHtml.AppendLine(item.TIPO);
                divTR.InnerHtml.AppendHtml(divTD2);

                var divTD3 = new TagBuilder("td");
                divTD3.Attributes.Add("width", "12%");
                divTD3.Attributes.Add("align", "center");
                divTD3.InnerHtml.AppendLine(item.NATUREZA);
                divTR.InnerHtml.AppendHtml(divTD3);

                var divTD4 = new TagBuilder("td");
                divTD4.Attributes.Add("width", "12%");
                divTD4.Attributes.Add("align", "center");
                divTD4.InnerHtml.AppendLine(item.DESPESA);
                divTR.InnerHtml.AppendHtml(divTD4);

                var divTD5 = new TagBuilder("td");
                divTD5.Attributes.Add("width", "12%");
                divTD5.Attributes.Add("align", "center");
                divTD5.InnerHtml.AppendLine(item.UTILIZADOPAGAMENTO);
                divTR.InnerHtml.AppendHtml(divTD5);

                var divTD6 = new TagBuilder("td");
                divTD6.Attributes.Add("width", "12%");
                divTD6.Attributes.Add("align", "center");
                divTD6.InnerHtml.AppendLine(item.UTILIZADORECEBIMENTO);
                divTR.InnerHtml.AppendHtml(divTD6);

                var divTD7 = new TagBuilder("td");
                divTD7.Attributes.Add("width", "5%");
                divTD7.Attributes.Add("align", "center");
                divTD7.InnerHtml.AppendHtmlLine($"<div class='btn btn-warning' onclick='EditarPlanoContas({item.CODPLANOCONTAS});' style='font-size:11px'>");
                divTD7.InnerHtml.AppendHtmlLine("<i class='fa fa-edit'></i>");
                divTD7.InnerHtml.AppendHtmlLine("</div>");
                divTR.InnerHtml.AppendHtml(divTD7);

                var divTD8 = new TagBuilder("td");
                divTD8.Attributes.Add("width", "5%");
                divTD8.Attributes.Add("align", "center");
                if (item.UTILIZADORECEBIMENTO != "Sim" && item.UTILIZADOPAGAMENTO != "Sim")
                {
                    divTD8.InnerHtml.AppendHtmlLine($"<div class='btn btn-danger' onclick='DesativaPlanoConta({item.CODPLANOCONTAS});' style='font-size:11px'>");
                    divTD8.InnerHtml.AppendHtmlLine("<i class='fa fa-trash'></i>");
                    divTD8.InnerHtml.AppendHtmlLine("</div>");
                }
                else
                {
                    PodeRemover = false;
                }
                divTR.InnerHtml.AppendHtml(divTD8);

                var divTD9 = new TagBuilder("td");
                divTD9.AddCssClass("UTILIZADOPAGAMENTO");
                divTD9.Attributes.Add("width", "0%");
                divTD9.Attributes.Add("style", "display:none;");
                divTD9.InnerHtml.AppendHtmlLine($"<input class='' type='hidden' name='UTILIZADOPAGAMENTO' value='{item.UTILIZADOPAGAMENTO}' />");
                divTR.InnerHtml.AppendHtml(divTD9);

                var divTD10 = new TagBuilder("td");
                divTD10.AddCssClass("UTILIZADORECEBIMENTO");
                divTD10.Attributes.Add("width", "0%");
                divTD10.Attributes.Add("style", "display:none;");
                divTD10.InnerHtml.AppendHtmlLine($"<input class='' type='hidden' name='UTILIZADORECEBIMENTO' value='{item.UTILIZADORECEBIMENTO}' />");
                divTR.InnerHtml.AppendHtml(divTD10);

                mainDiv.InnerHtml.AppendHtml(divTR);

                if (item.ListaFilho.Count > 0)
                {
                    var Filho = ListaFilhoPlanoContas(item.ListaFilho, (qtEspaco + 4), ref PodeRemover);
                    mainDiv.InnerHtml.AppendHtml(Filho);
                }

            }
            return mainDiv;
        }
    }
    /// <summary>
    /// Filter to set size limits for request form data
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RequestFormSizeLimitAttribute : Attribute, IAuthorizationFilter, IOrderedFilter
    {
        private readonly FormOptions _formOptions;

        public RequestFormSizeLimitAttribute(int valueCountLimit)
        {
            _formOptions = new FormOptions()
            {
                ValueCountLimit = valueCountLimit
            };
        }

        public int Order { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var features = context.HttpContext.Features;
            var formFeature = features.Get<IFormFeature>();

            if (formFeature == null || formFeature.Form == null)
            {
                // Request form has not been read yet, so set the limits
                features.Set<IFormFeature>(new FormFeature(context.HttpContext.Request, _formOptions));
            }
        }
        public static bool PossuiPermissao(int valorPermissao, TipoPermissao tipoPermissao)
        {
            return ((valorPermissao & Convert.ToInt32(tipoPermissao)) > 0);
        }
    }

}