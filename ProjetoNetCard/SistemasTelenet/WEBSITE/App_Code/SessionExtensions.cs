using Sil.Criptografia;
using SIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Telenet.Core.DependencyInjection;

/// <summary>
/// Summary description for SessionExtensions
/// </summary>
public static class SessionExtensions
{
    private const string SessionIdKey = "_NC_SessionID_";
    private const string TlnNcAuthentication = "TLN-NC.Session.{0}";

    private static string GetTokenName(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return string.Empty;
        }

        return string.Format(TlnNcAuthentication, id);
    }

    private static bool ExistsIdInCookie(this HttpContext context, string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return false;
        }

        var cookie = context.Request.Cookies[GetTokenName(id)];
        return cookie != null && new Criptografia().Decrypt(cookie.Value) == id;
    }

    public static string CreateSession(this HttpSessionState session)
    {
        if (session == null)
        {
            return null;
        }

        var clientSessionManager = ServiceConfiguration.ServiceProvider.GetService<IClientSessionManager>();
        var id = clientSessionManager.CreateSession();

        session[SessionIdKey] = id;
        HttpContext.Current.Response.Cookies.Add(new HttpCookie(GetTokenName(id), new Criptografia().Encrypt(id)));

        return id;
    }

    public static string GetSessionId(this HttpSessionState session)
    {
        if (session == null)
        {
            return string.Empty;
        }

        return Convert.ToString(session[SessionIdKey]);
    }

    public static void LoadSession(this HttpSessionState session)
    {
        if (session == null)
        {
            return;
        }

        session.Clear();
        var id = HttpContext.Current.Request.QueryString["sid"];

        if (!HttpContext.Current.ExistsIdInCookie(id))
        {
            return;
        }

        var clientSessionManager = ServiceConfiguration.ServiceProvider.GetService<IClientSessionManager>();
        var sessionValues = clientSessionManager.LoadSession(id);

        if (!sessionValues.Any(v => v.Key == SessionIdKey && Convert.ToString(v.Value) == id))
        {
            return;
        }

        foreach (var sessionItem in clientSessionManager.LoadSession(id))
        {
            session[sessionItem.Key] = sessionItem.Value;
        }
    }

    public static void RemoveSession(this HttpSessionState session)
    {
        var id = HttpContext.Current.Request.QueryString["sid"];

        if (session == null || string.IsNullOrEmpty(id))
        {
            return;
        }

        session.Clear();

        var cookie = HttpContext.Current.Response.Cookies[GetTokenName(id)];

        if (cookie != null)
        {
            cookie.Expires = DateTime.Now.AddDays(-1);
        }

        var clientSessionManager = ServiceConfiguration.ServiceProvider.GetService<IClientSessionManager>();
        clientSessionManager.RemoveSession(id);
    }

    public static void SaveSession(this HttpSessionState session)
    {
        if (session == null)
        {
            return;
        }

        var id = Convert.ToString(session[SessionIdKey]);

        if (string.IsNullOrEmpty(id))
        {
            return;
        }

        var sessionValues = new Dictionary<string, object>();

        foreach (var sessionItemKey in session.Keys)
        {
            var itemKey = sessionItemKey as string;
            sessionValues[itemKey] = session[itemKey];
        }

        var clientSessionManager = ServiceConfiguration.ServiceProvider.GetService<IClientSessionManager>();
        clientSessionManager.SaveSession(id, sessionValues);
    }

    public static void RegisterCustomSerializer<TType, TSerializer>(this HttpSessionState session)
        where TSerializer : class, ICustomSerializer, new()
    {
        var clientSessionManager = ServiceConfiguration.ServiceProvider.GetService<IClientSessionManager>();
        var type = typeof(TType);

        if (session == null || clientSessionManager.IdCustomSerializerRegistered(type))
        {
            return;
        }

        clientSessionManager.RegisterCustomSerializer(type, new TSerializer());
    }
}