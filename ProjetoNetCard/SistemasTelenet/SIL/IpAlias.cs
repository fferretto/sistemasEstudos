using System;
using System.IO;
using System.Net;
using System.Xml;

namespace SIL
{
    public enum IpType
    {
        None,
        Production,
        Test
    }

    public class AliasData
    {
        public string Alias { get; internal set; }
        public IPAddress Ip { get; internal set; }
        public IpType Type { get; internal set; }
    }

    public class IpAlias
    {
        public IpAlias(string path)
        {
            var filename = Path.Combine(path, "ipalias.config");

            if (File.Exists(filename))
                _ipalias.Load(filename);
        }

        private readonly XmlDocument _ipalias = new XmlDocument();

        public AliasData GetEntry(string ipOrAlias)
        {
            var nodes = _ipalias.SelectNodes(string.Format("alias/add[@ip='{0}']", ipOrAlias));

            if (nodes.Count == 0)
                nodes = _ipalias.SelectNodes(string.Format("alias/add[@alias='{0}']", ipOrAlias));

            if (nodes.Count > 0)
            {
                var node = nodes[0];
                return new AliasData
                {
                    Alias = node.Attributes["alias"].Value,
                    Ip = IPAddress.Parse(node.Attributes["ip"].Value),
                    Type = (IpType)Enum.Parse(typeof(IpType), node.Attributes["type"].Value)
                };
            }

            return new AliasData { Type = IpType.None };
        }
    }
}
