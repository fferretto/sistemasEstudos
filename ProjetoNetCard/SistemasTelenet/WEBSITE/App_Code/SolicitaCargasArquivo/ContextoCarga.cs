#pragma warning disable 1591

using System;
using System.Web;
using Telenet.Carga;

public class ContextoCarga : IContextoCarga
{
    public ContextoCarga(IRequestSession requestSession, ISolicitaCargaConfig solicitaCargaArquivoConfig)
	{
        _requestSession = requestSession;
        _solicitaCargaArquivoConfig = solicitaCargaArquivoConfig;
    }

    private IRequestSession _requestSession;
    private ISolicitaCargaConfig _solicitaCargaArquivoConfig;

    private static string ObtemIpMaquina()
    {
        var VisitorsIPAddr = string.Empty;

        if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        {
            return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
        {
            return HttpContext.Current.Request.UserHostAddress;
        }

        return string.Empty;
    }

    public string CnpjCliente { get { return HttpContext.Current.Request.QueryString["cnpj"]; } }

    public int CodigoOperadora { get { return _requestSession.Operadora.CODOPE; } }

    public int CodigoCliente { get { return Convert.ToInt32(HttpContext.Current.Request.QueryString["codCli"]); } }

    public int IdOperador { get { return _requestSession.Operadora.ID_FUNC; } }

    public string Login { get { return _requestSession.Operadora.LOGIN; } }

    public string NomeOperadora { get { return _requestSession.Operadora.NOMEOPERADORA; } }

    public string PastaArquivosImportacao { get { return _solicitaCargaArquivoConfig.PastaArquivosImportacao; } }

    public string SistemaOrigem { get { return "NC"; } }

    public string IpMaquinaSolicitante { get { return ObtemIpMaquina(); } }
}

#pragma warning restore 1591
