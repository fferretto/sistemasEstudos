#pragma warning disable 1591

using System.Configuration;

public class SolicitaCargaConfig : ISolicitaCargaConfig
{
    public SolicitaCargaConfig(IRequestSession session)
    {
        Session = session;
    }

    public IRequestSession Session { get; private set; }

    public string PastaArquivosImportacao { get { return ConfigurationManager.AppSettings["pastaArquivosImportacao"]; } }
}

#pragma warning restore 1591
