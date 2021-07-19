using System;

namespace PagNet.BLD.ContaCorrente
{
    public static class Constants
    {
        public static class Escopos
        {
            public const int ContaCorrente = 1;
        }

        public static class CodigosErro
        {
            public const int CPFInvalido = 1000;
            public const int MatriculaVazia = 1001;
            public const int NenhumUsuarioInformado = 1002;
            public const int ClienteNaoInformado = 1003;
            public const int LoteNaoInformado = 1004;
            public const int ArquivoConciliacaoNaoCoincideLote = 1005;
            public const int ArquivoJaValidado = 1006;
            public const int UsuarioNaoEncontrado = 1007;
            public const int DescontoFolhaNaoConfigurado = 1008;
            public const int ArquivoRetornoInvalido = 1009;
            public const int ArquivoRemessaInvalido = 1010;
        }
    }
}
