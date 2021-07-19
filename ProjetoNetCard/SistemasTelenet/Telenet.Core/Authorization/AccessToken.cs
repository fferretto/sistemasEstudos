using System;

namespace Telenet.Core.Authorization
{
    public class AccessToken
    {
        /// <summary>
        /// Obtém o prazo de validade do token.
        /// </summary>
        public DateTime ExpiresIn { get; set; }

        /// <summary>
        /// Obtém o token de refresh de acesso.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Obtém o token de acesso.
        /// </summary>
        public string Token { get; set; }
    }
}
