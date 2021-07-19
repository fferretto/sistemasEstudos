using Newtonsoft.Json;

namespace SIL
{
    public abstract class SessionAbstractDataItemSerializer
    {
        public SessionAbstractDataItemSerializer()
        { }

        public object Instance { get; set; }

        public abstract object Deserializer(string json);

        public abstract string Serializer();
    }
}
