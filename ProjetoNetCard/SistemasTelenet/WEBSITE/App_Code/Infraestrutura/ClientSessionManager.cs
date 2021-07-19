using Newtonsoft.Json;
using SIL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;

/// <summary>
/// Summary description for ClientSessionManager
/// </summary>
public sealed class ClientSessionManager : IClientSessionManager
{
    public ClientSessionManager(int timeout, string path)
    {
        _cache = HttpRuntime.Cache;
        _path = path;
        _timeout = timeout;

        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }

        // Carrega todas as sessões que estão persistidas.
        foreach (var filename in Directory.GetFiles(_path))
        {
            LoadSessionData(filename);
        }
    }

    private const string SessionFileExtension = ".session";

    private static IDictionary<Type, ICustomSerializer> _serializers = new Dictionary<Type, ICustomSerializer>();
    private static object _sync = new object();

    private readonly Cache _cache;
    private readonly string _path;
    private readonly int _timeout;

    private void AddDataSessionToCache(SessionData sessionData)
    {
        // Remmove o anterior para atualizar a data de expiração.
        _cache.Remove(sessionData.Id);

        // Usamos a data absoluta e não o Sliding porque no caso de uma carga de arquivo (quando a aplicação cai, por exemplo)
        // precisamos garantir que a sessão permaneça com o mesmo prazo de validade que estava antes.
        _cache.Add(sessionData.Id,
           sessionData,
           null,
           sessionData.Expiration,
           Cache.NoSlidingExpiration,
           CacheItemPriority.Default,
           RemoveSession);
    }

    private string GetSessionDataFilename(string id)
    {
        return Path.Combine(_path, string.Concat(id, SessionFileExtension));
    }

    private void DeleteFile(string filename)
    {
        if (File.Exists(filename))
        {
            File.Delete(filename);
        }
    }

    private void DeleteSessionData(string id)
    {
        var filename = GetSessionDataFilename(id);
        DeleteFile(filename);
    }

    private void LoadSessionData(string filename)
    {
        if (!File.Exists(filename))
        {
            return;
        }

        var json = File.ReadAllText(filename);
        var sessionData = JsonConvert.DeserializeObject<SessionData>(json);

        // Sessão expirada. Não vamos carregar e eliminamos o arquivo do diretório de persistência.
        if (sessionData.Expiration < DateTime.Now)
        {
            DeleteFile(filename);
            return;
        }

        AddDataSessionToCache(sessionData);
    }

    private void RemoveSession(string id, object value, CacheItemRemovedReason reason)
    { }

    private void SaveSessionData(SessionData sessionData)
    {
        var filename = GetSessionDataFilename(sessionData.Id);
        var json = JsonConvert.SerializeObject(sessionData, Formatting.None);
        File.WriteAllText(filename, json);
    }

    public IEnumerable<KeyValuePair<string, object>> LoadSession(string id)
    {
        var values = new Dictionary<string, object>();

        if (!SessionExists(id))
        {
            return values;
        }

        try
        {
            var sessionData = _cache[id] as SessionData;

            foreach (var sessionDataItem in sessionData.Values)
            {
                if (!string.IsNullOrEmpty(sessionDataItem.Value))
                {
                    var type = Type.GetType(sessionDataItem.TypeName);

                    if (sessionDataItem.IsCustomSerializer)
                    {
                        var serializer = Activator.CreateInstance(type);

                        var typedSeralizer = serializer as SessionAbstractDataItemSerializer;
                        typedSeralizer.Instance = typedSeralizer.Deserializer(sessionDataItem.Value);

                        values[sessionDataItem.Key] = serializer;
                    }
                    else
                    {
                        if (_serializers.ContainsKey(type))
                        {
                            values[sessionDataItem.Key] = _serializers[type].Deserialize(type, sessionDataItem.Value);
                        }
                        else
                        {
                            values[sessionDataItem.Key] = JsonConvert.DeserializeObject(sessionDataItem.Value, type);
                        }
                    }
                }
                else
                {
                    values[sessionDataItem.Key] = null;
                }
            }

            return values;
        }
        catch (Exception e)
        {
            // Tipo inválido no arquivo de sessão.
            return values;
        }
    }

    public void RemoveSession(string id)
    {
        if (SessionExists(id))
        {
            _cache.Remove(id);
            DeleteSessionData(id);
        }
    }

    public void SaveSession(string id, IEnumerable<KeyValuePair<string, object>> session)
    {
        if (!SessionExists(id))
        {
            return;
        }

        var values = new List<SessionDataItem>();

        foreach (var item in session)
        {
            var sessionItem = new SessionDataItem { Key = item.Key };

            if (item.Value != null)
            {
                sessionItem.TypeName = item.Value.GetType().AssemblyQualifiedName;
                var type = item.Value.GetType();

                if (typeof(SessionAbstractDataItemSerializer).IsAssignableFrom(type))
                {
                    sessionItem.IsCustomSerializer = true;
                    sessionItem.Value = (item.Value as SessionAbstractDataItemSerializer).Serializer();
                }
                else
                {
                    if (_serializers.ContainsKey(type))
                    {
                        sessionItem.Value = _serializers[type].Serialize(type, item.Value);
                    }
                    else
                    {
                        sessionItem.Value = JsonConvert.SerializeObject(item.Value);
                    }
                }
            }

            values.Add(sessionItem);
        }

        var sessionData = _cache[id] as SessionData;
        sessionData.Values = values;
        sessionData.Expiration = DateTime.Now.AddMinutes(_timeout);

        _cache[id] = sessionData;
        SaveSessionData(sessionData);
    }

    public bool SessionExists(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return false;
        }

        var exists = _cache[id] != null;

        if (!exists)
        {
            DeleteSessionData(id);
        }

        return exists;
    }

    public string CreateSession()
    {
        var sessionData = new SessionData
        {
            Expiration = DateTime.Now.AddMinutes(_timeout),
            Id = Guid.NewGuid().ToString(),
            Values = new List<SessionDataItem>()
        };

        AddDataSessionToCache(sessionData);
        return sessionData.Id;
    }

    public void RegisterCustomSerializer(Type type, ICustomSerializer serializer)
    {
        if (!_serializers.ContainsKey(type))
        {
            lock (_sync)
            {
                if (_serializers.ContainsKey(type))
                {
                    return;
                }

                _serializers.Add(type, serializer);
            }
        }
    }

    public bool IdCustomSerializerRegistered(Type type)
    {
        return _serializers.ContainsKey(type);
    }
}