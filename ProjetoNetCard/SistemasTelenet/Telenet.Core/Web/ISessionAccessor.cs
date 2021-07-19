namespace Telenet.Core.Web
{
    public interface ISessionAccessor
    {
        object this[int index] { get; set; }
        object this[string name] { get; set; }

        T GetValue<T>(int index);
        T GetValue<T>(string name);

        void SetValue<T>(int index, T value);
        void SetValue<T>(string name, T value);
    }
}
