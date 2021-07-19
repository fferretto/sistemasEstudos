using Telenet.Core.Authorization;
using Telenet.Core.DependencyInjection;

namespace NetCard.Common.Util
{
    public static class ApiHelper
    {
        public static string GetToken()
        {
            var authorizer = ServiceConfiguration
                .ServiceProvider
                .GetService<IUserAuthorization<ConsultaWebAuthorizationContext>>();

            return authorizer.GetToken();
        }
    }
}
