using System.Web.SessionState;

namespace SIL
{
    public static class SessionExtensions
    {
        public static TValue GetValue<TValue>(this HttpSessionState self, string key)
        {
            return self.GetValue<TValue>(key, default(TValue));
        }

        public static TValue GetValue<TValue>(this HttpSessionState self, string key, TValue defaultValue)
        {
            var value = self[key];
            return value == null ? defaultValue : (TValue)self[key];
        }
    }
}
