namespace Telenet.Core.Authorization
{
    public interface IAppAuthorization<TContext> : IAuthorization<TContext>
        where TContext : IAuthorizationContext
    { }
}
