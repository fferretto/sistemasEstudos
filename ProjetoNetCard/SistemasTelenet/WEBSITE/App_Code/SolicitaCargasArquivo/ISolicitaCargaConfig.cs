
/// <summary>
/// Define co contrato de implementação de configurações da página de carga via arquivo.
/// </summary>
public interface ISolicitaCargaConfig 
{
    /// <summary>
    /// Obtém acesso a sessão da página.
    /// </summary>
    IRequestSession Session { get; }

    /// <summary>
    /// Obtém acesso a configuração da pasta definida para arquivos de importação.
    /// </summary>
    string PastaArquivosImportacao { get; }
}