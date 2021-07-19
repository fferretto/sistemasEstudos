
using System;

namespace SIL
{
    public interface ICustomSerializer
    {
        string Serialize(Type type, object obj);

        object Deserialize(Type type, string text);
    }
}
