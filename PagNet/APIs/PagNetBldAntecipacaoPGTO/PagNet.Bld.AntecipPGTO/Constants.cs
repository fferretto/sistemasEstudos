using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.AntecipPGTO
{
    public static class Constants
    {
        public static class Escopos
        {
            public const int AntecipacaoPGTO = 1;
        }

        public static class CodigosErro
        {
            public const int IdSistemaInvalido = 1000;
            public const int CodigoTituloNaoInformado = 1001;
            public const int DataAntecipacaoNaoInformada = 1002;
            public const int DataMenorDataAtual = 1003;
            public const int DataSuperiorDataOriginal = 1004;
            public const int CodigoEmpresaNaoInformado = 1005;
            public const int CodigoFavorecidoNaoInformado = 1006;
        }
    }
}
