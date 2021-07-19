#pragma warning disable 1591

using NetCardConsulta.Configs.Shared;
using System.Web;
using Telenet.Carga;

namespace NetCardConsulta.Configs.Carga
{
    public class ContextoCarga : IContextoCarga
    {
        public ContextoCarga(IRequestSession requestSession, ISolicitaCargaConfig cliSolicitaCargaArquivoConfig)
        {
            RequestSession = requestSession;
            CliSolicitaCargaArquivoConfig = cliSolicitaCargaArquivoConfig;
        }

        protected IRequestSession RequestSession { get; private set; }
        protected ISolicitaCargaConfig CliSolicitaCargaArquivoConfig { get; private set; }

        private static string ObtemIpMaquina()
        {
            var VisitorsIPAddr = string.Empty;

            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }

            return string.Empty;
        }

        public string CnpjCliente { get { return RequestSession.DadosAcesso.Cnpj; } }

        public int CodigoOperadora { get { return RequestSession.ObjetoConexao.CodOpe; } }

        public int CodigoCliente { get { return RequestSession.DadosAcesso.Codigo; } }

        public int IdOperador { get { return RequestSession.DadosAcesso.Id; } }

        public string Login { get { return string.Concat(RequestSession.DadosAcesso.Login, RequestSession.DadosAcesso.Codigo); } }

        public string NomeOperadora { get { return RequestSession.ObjetoConexao.NomOperadora; } }

        public string PastaArquivosImportacao { get { return CliSolicitaCargaArquivoConfig.PastaArquivosImportacao; } }

        public string SistemaOrigem { get { return "MW"; } }

        public string IpMaquinaSolicitante { get { return ObtemIpMaquina(); } }
    }
}

#pragma warning restore 1591
