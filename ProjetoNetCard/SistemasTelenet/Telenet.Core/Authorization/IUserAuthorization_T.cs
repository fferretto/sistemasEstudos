namespace Telenet.Core.Authorization
{
    public interface IUserAuthorization<TContext> : IAuthorization<TContext>
        where TContext : IAuthorizationContext
    {
        string CurrentUsername { get; }
        void Authenticate(string username, string password);
    }
}
