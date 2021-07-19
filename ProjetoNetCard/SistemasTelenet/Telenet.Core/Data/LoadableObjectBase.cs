#pragma warning disable 1591

using System.Data;

namespace Telenet.Core.Data
{
    public abstract class LoadableObjectBase : ILoadableObject
    {
        public abstract void LoadFrom(IDataReader reader);
    }
}

#pragma warning restore 1591
