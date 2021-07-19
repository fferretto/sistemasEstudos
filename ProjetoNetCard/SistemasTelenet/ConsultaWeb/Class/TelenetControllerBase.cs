using NetCardConsulta.Configs.Shared;
using System;
using System.Web.Mvc;
using Telenet.Core.DependencyInjection;

namespace NetCardConsulta.Class
{
    /// <summary>
    /// Define a classe controller base da Telenet.
    /// </summary>
    public abstract class TelenetControllerBase : Controller
    {
        /// <summary>
        /// Cria uma nova instância do controller.
        /// </summary>
        protected TelenetControllerBase()
            : base()
        {
            RequestSession = GetService<IRequestSession>();
        }

        /// <summary>
        /// Obtém os dados básicos da sessão do usuário.
        /// </summary>
        protected IRequestSession RequestSession { get; private set; }

        /// <summary>
        /// Obtém um serviço configurado via DI.
        /// </summary>
        protected TService GetService<TService>()
        {
            return ServiceConfiguration.ServiceProvider.GetService<TService>();
        }

        /// <summary>
        /// Redireciona a chamada ao controller para a ação Index.
        /// </summary>
        protected virtual ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Redireciona a chamada ao controller para a ação de login.
        /// </summary>
        protected virtual ActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Recupera um valor do formulário da requisição.
        /// </summary>
        protected virtual TValue GetFormValue<TValue>(string name)
        {
            return GetFormValue<TValue>(name, default(TValue));
        }

        /// <summary>
        /// Recupera um valor do formulário da requisição, definindo um valor default.
        /// </summary>
        protected virtual TValue GetFormValue<TValue>(string name, TValue defaultValue)
        {
            var value = Request.Form[name];
            return string.IsNullOrWhiteSpace(value) ? defaultValue : (TValue)Convert.ChangeType(value, typeof(TValue));
        }
    }
}