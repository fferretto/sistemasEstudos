using Telenet.Core.Web;

namespace Telenet.Core.Authorization
{
    public sealed class AppAuthorization<TContext> : AuthorizationBase<TContext>, IAppAuthorization<TContext>
        where TContext : IAuthorizationContext
    {
        public AppAuthorization(TContext context, ISessionAccessor sessionAccessor) 
            : base(context, sessionAccessor)
        { }
    }
}
