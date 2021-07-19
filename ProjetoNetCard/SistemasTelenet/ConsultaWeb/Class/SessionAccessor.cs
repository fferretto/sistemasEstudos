using System.Web;
using Telenet.Core.Web;

namespace NetCardConsulta.Class
{
    public class SessionAccessor : ISessionAccessor
    {
        public object this[int index]
        {
            get { return HttpContext.Current.Session[index]; }
            set { HttpContext.Current.Session[index] = value; }
        }

        public object this[string name]
        {
            get { return HttpContext.Current.Session[name]; }
            set { HttpContext.Current.Session[name] = value; }
        }

        public T GetValue<T>(int index)
        {
            return (T)HttpContext.Current.Session[index];
        }

        public T GetValue<T>(string name)
        {
            return (T)HttpContext.Current.Session[name];
        }

        public void SetValue<T>(int index, T value)
        {
            HttpContext.Current.Session[index] = value;
        }

        public void SetValue<T>(string name, T value)
        {
            HttpContext.Current.Session[name] = value;
        }
    }
}