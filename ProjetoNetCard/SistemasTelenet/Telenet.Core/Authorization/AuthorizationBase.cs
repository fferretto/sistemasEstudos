using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Telenet.Core.Web;

namespace Telenet.Core.Authorization
{
    public abstract class AuthorizationBase<TContext> : IAuthorization<TContext>
        where TContext : IAuthorizationContext
    {
        public AuthorizationBase(TContext context, ISessionAccessor sessionAccessor)
        {
            SessionAccessor = sessionAccessor;
            CurrentAccessToken = SessionAccessor.GetValue<AccessToken>(ACCESS_TOKEN_SESSION_KEY);
            Context = context;
        }

        protected const string ACCESS_TOKEN_SESSION_KEY =  "__ACCESS_TOKEN__";

        protected readonly TContext Context;
        protected AccessToken CurrentAccessToken;
        protected readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        protected readonly ISessionAccessor SessionAccessor;

        protected virtual bool IsAuthenticationNeed()
        {
            // Nenhuma authenticação foi realizada ainda.
            if (CurrentAccessToken == null || string.IsNullOrEmpty(CurrentAccessToken.Token))
            {
                return true;
            }

            // Verifica se o token está expirado.
            if (DateTime.Now > CurrentAccessToken.ExpiresIn)
            {
                // Se não existir um refresh token, precisa de nova autenticação.
                if (!string.IsNullOrEmpty(CurrentAccessToken.RefreshToken))
                {
                    AuthenticateWithRefreshToken();
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual void Authenticate(NameValueCollection data)
        {
            data["applicationKey"] = Context.ApplicationKey;

            try
            {
                using (WebClient client = new WebClient())
                {
                    var responsebytes = client.UploadValues(Context.ServerAddress.ToString(), "POST", data);
                    var response = Encoding.UTF8.GetString(responsebytes);
                    var accessTokenVo = JsonConvert.DeserializeObject<AccessTokenVo>(response, JsonSerializerSettings);

                    if (accessTokenVo != null)
                    {
                        CurrentAccessToken = new AccessToken
                        {
                            Token = accessTokenVo.Access_Token,
                            ExpiresIn = DateTime.Now.AddSeconds(accessTokenVo.Expires_In - 3000),
                            RefreshToken = accessTokenVo.Refresh_Token ?? CurrentAccessToken?.RefreshToken
                        };

                        SessionAccessor.SetValue(ACCESS_TOKEN_SESSION_KEY, CurrentAccessToken);
                    }
                }
            }
            catch
            { }
        }

        protected virtual void AuthenticateWithRefreshToken()
        {
            var data = new NameValueCollection
            {
                {  "refresh_token", CurrentAccessToken?.RefreshToken }
            };

            Authenticate(data);
        }

        public string GetToken()
        {
            if (IsAuthenticationNeed())
            {
                 Authenticate(new NameValueCollection());
            }

            return CurrentAccessToken?.Token;
        }
    }
}
