#pragma warning disable 1591

using NetCardConsulta.Configs.Shared;
using Telenet.Carga;

namespace NetCardConsulta.Configs.Carga
{
    public class ContextoCargaManual : ContextoCarga, IContextoCargaManual
    {
        public ContextoCargaManual(IRequestSession requestSession, ISolicitaCargaConfig cliSolicitaCargaArquivoConfig)
            : base(requestSession, cliSolicitaCargaArquivoConfig)
        { }

        public int MaximoCartoesPorSolicitacao { get { return 130; } }
    }
}

#pragma warning restore 1591