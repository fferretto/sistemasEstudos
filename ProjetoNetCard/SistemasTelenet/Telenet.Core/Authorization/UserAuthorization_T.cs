using System;
using System.Collections.Specialized;
using Telenet.Core.Web;

namespace Telenet.Core.Authorization
{
    public sealed class UserAuthorization<TContext> : AuthorizationBase<TContext>, IUserAuthorization<TContext>
        where TContext : IAuthorizationContext
    {
        public UserAuthorization(TContext context, ISessionAccessor sessionAccessor) 
            : base(context, sessionAccessor)
        { }

        public string CurrentUsername { get; private set; }

        public void Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            var data = new NameValueCollection
            {
                { "username", username },
                { "password", password }
            };

            CurrentUsername = username;
            Authenticate(data);
        }
    }
}
