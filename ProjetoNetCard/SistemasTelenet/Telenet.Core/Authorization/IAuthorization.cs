namespace Telenet.Core.Authorization
{
    public interface IAuthorization<TContext>
        where TContext : IAuthorizationContext
    {
        string GetToken();
    }
}
