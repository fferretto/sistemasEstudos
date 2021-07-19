#pragma warning disable 1591

using System.Linq;
using Telenet.Carga.Abstractions;

public static class EtapaCargaExtension
{
    public static bool EUmStatus(this EtapaCarga etapaCarga, params EtapaCarga[] etapa)
    {
        return etapa.Any(e => e == etapaCarga);
    }

    public static bool EStatusErro(this EtapaCarga etapaCarga)
    {
        return etapaCarga == EtapaCarga.SolicitacaoCargaErro || etapaCarga == EtapaCarga.ValidacaoLayoutErro || etapaCarga == EtapaCarga.VerificacaoDadosErro;
    }

    public static bool EStatusOK(this EtapaCarga etapaCarga)
    {
        return etapaCarga == EtapaCarga.SolicitacaoCargaOk || etapaCarga == EtapaCarga.ValidacaoLayoutOk || etapaCarga == EtapaCarga.VerificacaoDadosOk;
    }
}

#pragma warning restore 1591
