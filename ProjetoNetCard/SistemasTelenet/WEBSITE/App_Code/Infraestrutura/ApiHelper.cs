using Telenet.Core.Authorization;
using Telenet.Core.DependencyInjection;

/// <summary>
/// Summary description for ApiHelper
/// </summary>
public static class ApiHelper
{
    public static string GetToken()
    {
        var authorizer = ServiceConfiguration
            .ServiceProvider
            .GetService<IUserAuthorization<NetCardAuthorizationContext>>();

        return authorizer.GetToken();
    }
}
