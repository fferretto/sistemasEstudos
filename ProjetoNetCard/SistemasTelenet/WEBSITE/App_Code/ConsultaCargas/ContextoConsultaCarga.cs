#pragma warning disable 1591

using Telenet.Carga;

/// <summary>
/// Summary description for ContextoConsultaCarga
/// </summary>
public class ContextoConsultaCarga : IContextoConsultaCarga
{
    public ContextoConsultaCarga(IRequestSession requestSession)
    {
        _requestSession = requestSession;
    }

    private IRequestSession _requestSession;

    public int CodigoCliente { get { return _requestSession.Operadora.CODOPE; } }
}

#pragma warning restore 1591
