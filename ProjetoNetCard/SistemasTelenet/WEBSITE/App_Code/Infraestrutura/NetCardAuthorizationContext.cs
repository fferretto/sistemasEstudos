using System;
using System.Configuration;
using Telenet.Core.Authorization;

/// <summary>
/// Summary description for NetCardAuthorizationContext
/// </summary>
public class NetCardAuthorizationContext : AuthorizationContextBase
{
    public NetCardAuthorizationContext(Uri serverAddress)
        : base(ConfigurationManager.AppSettings["NetCardApplicationKey"], serverAddress)
    { }
}