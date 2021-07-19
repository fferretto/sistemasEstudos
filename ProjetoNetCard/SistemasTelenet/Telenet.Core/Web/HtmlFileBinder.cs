// -----------------------------------------------------------------------------
// Telenet Tecnologia e Serviços em Rede
// Autor: Alexandre Chestter
// Data: 16/09/2019
// -----------------------------------------------------------------------------

#pragma warning disable 1591

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Telenet.Core.Web
{
    public class HtmlFileBinder : IHtmlFileBinder
    {
        public HtmlFileBinder(string name, string filename)
        {
            Template = File.ReadAllText(filename);
            Offsets = LoadOffsets();
            Name = name;
        }

        protected class Offset
        {
            public int Position;
            public int BufferSize;
            public int TagSize;
        }

        protected readonly string Template;
        protected readonly IDictionary<string, Offset> Offsets;

        protected IDictionary<string, Offset> LoadOffsets()
        {
            var tags = new Dictionary<string, Offset>();
            var matches = Regex.Matches(Template, "@{\\w+}", RegexOptions.IgnoreCase);
            var offset = 0;
            var length = 0;

            foreach (Match match in matches)
            {
                length = match.Index - offset;

                tags.Add(
                    match.Value.Substring(2, match.Value.Length - 3),
                    new Offset { Position = offset, BufferSize = length, TagSize = match.Length });

                offset = match.Index + match.Length;
            }

            var lastMatch = matches.Cast<Match>().LastOrDefault();

            // Offset para copia do restante da string após a última tag.
            if (lastMatch != null)
            {
                offset = lastMatch.Index + lastMatch.Length;

                if (Template.Length > length)
                {
                    tags.Add(
                        "",
                        new Offset { Position = offset, BufferSize = Template.Length - offset, TagSize = 0 });
                }
            }

            return tags;
        }

        protected string CreateHtml(IViewBag viewBag)
        {
            var template = Template.ToList();
            var finalLength = template.Count - Offsets.Sum(t => t.Value.TagSize) + viewBag.Sum(k => k.Value.ToString().Length);
            var html = new char[finalLength];
            var targetOffset = 0;

            foreach (var offset in Offsets)
            {
                template.CopyTo(offset.Value.Position, html, targetOffset, offset.Value.BufferSize);
                targetOffset += offset.Value.BufferSize;

                var key = viewBag.Keys.FirstOrDefault(k => k.Equals(offset.Key, System.StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(key))
                {
                    var value = viewBag[key].ToString().ToList();
                    value.CopyTo(html, targetOffset);
                    targetOffset += value.Count;
                }
            }

            return new string(html);
        }

        public string Name { get; private set; }

        public string Bind(IViewBag viewBag)
        {
            return CreateHtml(viewBag);
        }

        public string Bind<TModel>(TModel model) where TModel : class, new()
        {
            if (model is IViewBag) return Bind(model as IViewBag);

            var viewBag = new ViewBag();

            foreach (var propertyInfo in typeof(TModel).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty))
                viewBag.Add(propertyInfo.Name, propertyInfo.GetValue(model, null).ToString());

            return CreateHtml(viewBag);
        }
    }
}

#pragma warning restore 1591
