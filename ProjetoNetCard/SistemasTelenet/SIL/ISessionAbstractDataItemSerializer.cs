namespace SIL
{
    public interface ISessionAbstractDataItemSerializer
    {
        object Deserializer(string json);

        string Serializer();
    }
}
