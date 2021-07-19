#pragma warning disable 1591

using NetCardConsulta.Configs.Shared;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NetCardConsulta.Configs.Carga
{
    internal static class Helper
    {
        const string EsteProcessoFoiIniciadoPorOutroIdLogin = "ESTE_PROCESSO_FOI_INICIADO_POR_OUTRO_ID_LOGIN";
        const string JaExisteProcessoParaEsteIdLogin = "JA_EXISTE_PROCESSO_PARA_ESTE_ID_LOGIN";
        const string MaximoCartoesExcedido = "MAXIMO_CARTOES_CARGA_EXCEDIDO";
        const string ArquivoCargaManualNaoEncontrado = "ARQUIVO_CARGA_MANUAL_NAO_ENCONTRADO";
        const string ProcessoCargaNaoExiste = "PROCESSO_CARGA_NAO_EXISTE";

        public const string ErroInternoSqlServer = "O erro 255 foi informado por um recurso interno. Entre em contato com o suporte";
        public const string ErroInternoSqlServerCargaStatus = "O erro 254 foi informado ao se criar o status inicial da carga. Entre em contato com o suporte";

        public static bool ProcessoIniciadoEmOutraEstacao(this Exception exception)
        {
            return exception.Message == EsteProcessoFoiIniciadoPorOutroIdLogin;
        }

        public static string GetMensagemErroCarga(this Exception exception, IRequestSession requestSession)
        {
            if (exception.Message == "254")
            {
                return ErroInternoSqlServerCargaStatus;
            }

            if (exception.Message == ProcessoCargaNaoExiste)
            {
                return "O processo de carga informado não existe.";
            }

            if (exception.Message == MaximoCartoesExcedido)
            {
                return "Número máximo de cartões para a carga manual foi excedido. Limite máximo é de 30 cartões por carga. Para carga com mais cartões utilize a carga via arquvo.";
            }

            if (exception.Message == ArquivoCargaManualNaoEncontrado)
            {
                return "Não foi possível efetuar a solicitação de carga. Erro na criação da solicitação.";
            }

            if (exception.Message == EsteProcessoFoiIniciadoPorOutroIdLogin)
            {
                return string.Format("Um processo de carga do login {0} no cliente {1} já foi iniciado em outra estação de trabalho.",
                    requestSession.DadosAcesso.Login, requestSession.DadosAcesso.Codigo);
            }

            if (exception.Message == JaExisteProcessoParaEsteIdLogin)
            {
                return string.Format("Já existe um processo de carga do login {0} no cliente {1} já foi existe.",
                    requestSession.DadosAcesso.Login, requestSession.DadosAcesso.Codigo);
            }

            return exception.Message;
        }

        public static HtmlString ExibeErroCarga(this HtmlHelper helper, string mensagem)
        {
            if (!string.IsNullOrWhiteSpace(mensagem))
            {
                var builder = new StringBuilder();
                builder.AppendLine("    <div class=\"table-box\" style=\"width: 100%; color: red;\">");
                builder.AppendLine("        <span>" + mensagem + "</span>");
                builder.AppendLine("    </div>");
                return new HtmlString(builder.ToString());
            }

            return new HtmlString("");
        }
    }
}

#pragma warning restore 1591
