#pragma warning disable 1591

using TELENET.SIL.PO;
using Context = System.Web.HttpContext;

public class RequestSession : IRequestSession
{
    public RequestSession()
    {
        SessionID = Context.Current.Session.GetSessionId();
    }


    public IOperadora Operadora { get { return Context.Current.Session["Operador"] as OPERADORA; } }

    public string SessionID { get; private set; }

    public int Timeout { get { return Context.Current.Session.Timeout; } }

}

#pragma warning restore 1591
