#pragma warning disable 1591

using System.Data;
using Telenet.Core.Data;

namespace Telenet.Carga
{
    internal class NovaCarga : RetornoProcCarga, ILoadableObject
    {
        public string Id { get; private set; }

        public override void LoadFrom(IDataReader reader)
        {
            base.LoadFrom(reader);
            Id = reader.GetValue<string>("ID_PROCESSO");
        }
    }
}

#pragma warning restore 1591
