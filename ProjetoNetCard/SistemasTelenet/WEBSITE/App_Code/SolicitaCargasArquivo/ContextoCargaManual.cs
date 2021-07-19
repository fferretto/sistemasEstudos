using Telenet.Carga;

/// <summary>
/// Descrição resumida de ContextoCargaManual
/// </summary>
public class ContextoCargaManual : ContextoCarga, IContextoCargaManual
{
    public ContextoCargaManual(IRequestSession requestSession, ISolicitaCargaConfig cliSolicitaCargaArquivoConfig)
        : base(requestSession, cliSolicitaCargaArquivoConfig)
    { }
    public int MaximoCartoesPorSolicitacao { get { return 130; } }
}