#pragma warning disable 1591

using NetCardConsulta.Configs.Shared;
using Telenet.Carga;

namespace NetCardConsulta.Configs.Carga
{
    public class ContextoConsultaCarga : IContextoConsultaCarga
    {
        public ContextoConsultaCarga(IRequestSession requestSession)
        {
            _requestSession = requestSession;
        }

        private IRequestSession _requestSession;

        public int CodigoCliente { get { return _requestSession.DadosAcesso.Codigo; } }
    }
}

#pragma warning restore 1591
