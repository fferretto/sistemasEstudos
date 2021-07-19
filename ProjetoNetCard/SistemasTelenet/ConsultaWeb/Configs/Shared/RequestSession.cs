#pragma warning disable 1591

using NetCard.Common.Models;
using Context = System.Web.HttpContext;

namespace NetCardConsulta.Configs.Shared
{
    public class RequestSession : IRequestSession
    {
        public IDadosAcesso DadosAcesso { get { return Context.Current.Session["DadosAcesso"] as IDadosAcesso; } }

        public IObjetoConexao ObjetoConexao { get { return Context.Current.Session["ObjConexao"] as IObjetoConexao; } }

        public IPermissao Permissao { get { return Context.Current.Session["Permissao"] as IPermissao; } }

        public string SessionID { get { return Context.Current.Session.SessionID; } }

        public int Timeout { get { return Context.Current.Session.Timeout; } }
    }
}

#pragma warning restore 1591
