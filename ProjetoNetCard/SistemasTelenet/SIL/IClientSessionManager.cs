using System;
using System.Collections.Generic;

namespace SIL
{
    public interface IClientSessionManager
    {
        bool IdCustomSerializerRegistered(Type type);
        void RegisterCustomSerializer(Type type, ICustomSerializer serializer);
        bool SessionExists(string id);
        string CreateSession();
        void SaveSession(string id, IEnumerable<KeyValuePair<string, object>> session);
        IEnumerable<KeyValuePair<string, object>> LoadSession(string id);
        void RemoveSession(string id);
    }
}
