#pragma warning disable 1591

using NetCardConsulta.Configs.Shared;
using System.Configuration;

namespace NetCardConsulta.Configs.Carga
{
    public class SolicitaCargaConfig : ISolicitaCargaConfig
    {
        public SolicitaCargaConfig(IRequestSession requestSession)
        {
            _requestSession = requestSession;
        }

        private IRequestSession _requestSession;

        public string NomeOperadora { get { return _requestSession.ObjetoConexao.NomOperadora; } }
        public string PastaArquivosImportacao { get { return ConfigurationManager.AppSettings["pastaArquivosImportacao"]; } }
    }
}

#pragma warning restore 1591
