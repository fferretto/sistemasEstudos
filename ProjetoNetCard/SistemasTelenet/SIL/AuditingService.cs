using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using TELENET.SIL.PO;

namespace SIL
{
    /// <summary>
    /// Serviço de log de erros.
    /// </summary>
    public static class AuditingService
    {
        private static string GetUserIP()
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

        private static string GetNextIdSql(string tableName)
        {
            return string.Format(@"(select top 1 isnull(Id, 1) as NextId 
                     from (
                              select top 1 RowNumber as Id 
                              from (
                                       select row_number() over (order by Id) as RowNumber, Id
		                               from {0} with (nolock)
                                   ) FirstGap 
                              where RowNumber <> Id
	                          union
	                          select max(Id) + 1 as Id from {0}
                          ) TableIds)"
                , tableName);

            //return string.Format(@"(select next value for AutoInc.{0})", tableName);
        }

        private static long CreateProcess(long applicationId, SqlConnection connection, string name)
        {
            var processId = connection.ExecuteScalar<long>(0,
                "select Id, ApplicationId, Name from Process where ApplicationId = @1 and Name = @2",
                applicationId, name);

            if (processId == 0)
            {
                processId = connection.ExecuteScalar<long>(0,
                    string.Format("insert into Process (Id, ApplicationId, Name) output Inserted.Id values ({0}, @1, @2)", GetNextIdSql("Process")),
                applicationId, name);
            }

            return processId;
        }

        private static long Start(long applicationId, SqlConnection connection, OPERADORA operadora)
        {
            var processName = HttpContext.Current.Request.CurrentExecutionFilePath;
            var slashPosition = processName.LastIndexOf("/");

            if (slashPosition > -1)
                processName = processName.Substring(slashPosition + 1, processName.Length - slashPosition - 1);

            var processId = CreateProcess(applicationId, connection, processName);

            return connection.ExecuteScalar<long>(0,
                string.Format("insert into ProcessProfiler (Id, ProcessId, ClientId, Started, Status, UserName, IpAddress) output Inserted.Id values ({0}, @1, @2, @3, @4, @5, @6)", GetNextIdSql("ProcessProfiler")),
                processId, operadora.CODOPE, DateTime.Now, Convert.ToInt16(0), operadora.LOGIN, GetUserIP());
        }

        private static void Finish(SqlConnection connection, long processId)
        {
            connection.Execute("update ProcessProfiler set Finished = @1, Status = @2 where Id = @3", DateTime.Now, Convert.ToInt16(2), processId);
        }

        private static IDictionary<string, string> GetHttpRequestEvidenceData(HttpRequest request)
        {
            var requestData = new Dictionary<string, string>();

            requestData.Add("Url", request.Url.ToString());
            requestData.Add("EncodingName", request.ContentEncoding.ToString());
            requestData.Add("HttpMethod", request.HttpMethod);

            requestData.Add("Browser.Type", request.Browser.Type);
            requestData.Add("Browser.UserAgent", request.UserAgent);

            if (!request.Form.HasKeys())
                return requestData;

            var formParams = request.Form;

            for (int i = 0; i < formParams.Count; i++)
            {
                requestData.Add(string.Concat("FormParams.Key_", i.ToString()), formParams.GetKey(i));
                requestData.Add(string.Concat("FormParams.Value_", i.ToString()), formParams[i]);
            }

            return requestData;
        }

        private static IDictionary<string, string> GetExceptionEvidenceData(Exception exception)
        {
            return new Dictionary<string, string>
        {
            { "ExceptionType", exception.GetType().FullName },
            { "Message", exception.Message },
            { "StackTrace", exception.StackTrace }
        };
        }

        private static IDictionary<string, string> GetClientSessionData(OPERADORA operadora)
        {
            return new Dictionary<string, string>
            {
                { "CodigoOperadora", Convert.ToString(operadora.CODOPE) },
                { "LoginUsuario", operadora.LOGIN },
                { "NomeCliente", operadora.NOMCLI },
                { "ServidorBancoAutorizador", operadora.SERVIDORAUT },
                { "ServidorBancoConcentrdor", operadora.SERVIDORCON },
                { "ServidorBancoNeCard", operadora.SERVIDORNC },
                { "BancoConcentrador", operadora.BANCOCON },
                { "BancoAutorizador", operadora.BANCOAUT },
                { "BancoNetCard", operadora.BANCONC },
                { "ServidorIIS", operadora.SERVIDORIIS },
                { "UsuarioAutenticado", operadora.AUTENTICADO ? "S" : "N" },
                { "UsuarioBancoDados", operadora.USUARIOBD },
                { "SenhaBancoDados", operadora.SENHABD },
                { "IdPerfil", Convert.ToString(operadora.IDPERFIL) },
                { "IdFuncionario", Convert.ToString(operadora.ID_FUNC) },
                { "NomeOperadora", operadora.NOMEOPERADORA },
                { "Status", operadora.STA }
            };
        }

        private static void CreateExceptionEvidence(SqlConnection connection, long processId, Exception exception)
        {
            if (exception == null) return;
            CreateEvidence(connection, processId, "Exception", true, GetExceptionEvidenceData(exception));
            CreateExceptionEvidence(connection, processId, exception.InnerException);
        }

        private static void CreateEvidence(SqlConnection connection, long processId, string evidenceType, bool isFailEvidence, IDictionary<string, string> data)
        {
            var evidenceId = connection.ExecuteScalar<long>(0,
                string.Format("insert into ProcessEvidence (Id, ProcessProfilerId, EvidenceType, IsFailEvidence) output Inserted.Id values ({0}, @1, @2, @3)", GetNextIdSql("ProcessEvidence")),
                processId, evidenceType, isFailEvidence ? 'S' : 'N');

            foreach (var evidenceData in data)
            {
                connection.Execute(
                    string.Format("insert into ProcessEvidenceData (Id, ProcessEvidenceId, Name, Value) values ({0}, @1, @2, @3)", GetNextIdSql("ProcessEvidenceData")),
                    evidenceId, evidenceData.Key, evidenceData.Value);
            }
        }

        public static bool CreateAuditingLog(long applicationId, Exception exception)
        {
            bool.TryParse(ConfigurationManager.AppSettings["enableAuditing"], out bool enableAuditing);

            if (!enableAuditing)
                return false;

            var auditingConnStr = ConfigurationManager.AppSettings["auditingConnStr"];

            if (string.IsNullOrEmpty(auditingConnStr))
                return false;

            OPERADORA operadora = null;

            if (HttpContext.Current.Session != null)
                operadora = HttpContext.Current.Session["Operador"] as OPERADORA;

            if (operadora == null)
                operadora = new OPERADORA { CODOPE = 99 }; // Telenet

            using (var connection = new SqlConnection(auditingConnStr))
            {
                var processId = Start(applicationId, connection, operadora);

                CreateEvidence(connection, processId, "ClientSessionData", false, GetClientSessionData(operadora));
                CreateEvidence(connection, processId, "HttpRequest", false, GetHttpRequestEvidenceData(HttpContext.Current.Request));
                CreateExceptionEvidence(connection, processId, exception);

                Finish(connection, processId);
            }

            return true;
        }

        public static bool CreateAuditingConsultLog(long applicationId, Exception exception, DataTable dt)
        {
            bool.TryParse(ConfigurationManager.AppSettings["enableAuditing"], out bool enableAuditing);

            if (!enableAuditing)
                return false;

            var auditingConnStr = ConfigurationManager.AppSettings["auditingConnStr"];

            if (string.IsNullOrEmpty(auditingConnStr))
                return false;

            // Cria a operadora Telenet como default;
            OPERADORA operadora = new OPERADORA { CODOPE = 99 };

            if (dt.Rows.Count > 0)
            {
                operadora.CODOPE = Convert.ToInt16(dt.Rows[0]["CODOPE"].ToString());
                operadora.LOGIN = dt.Rows[0]["LOGIN"].ToString();
                operadora.NOMCLI = dt.Rows[0]["NOMCLI"].ToString();
                operadora.SERVIDORAUT = dt.Rows[0]["SERVIDORAUT"].ToString();
                operadora.SERVIDORCON = dt.Rows[0]["SERVIDORCON"].ToString();
                operadora.SERVIDORNC = dt.Rows[0]["SERVIDORNC"].ToString();
                operadora.BANCOCON = dt.Rows[0]["BANCOCON"].ToString();
                operadora.BANCOAUT = dt.Rows[0]["BANCOAUT"].ToString();
                operadora.BANCONC = dt.Rows[0]["BANCONC"].ToString();
                operadora.STA = dt.Rows[0]["STA"].ToString();
                operadora.ACESSO = dt.Rows[0]["ACESSO"].ToString();
                operadora.ACESSO = dt.Rows[0]["CODCLI"].ToString();
            }

            using (var connection = new SqlConnection(auditingConnStr))
            {
                var processId = Start(applicationId, connection, operadora);

                CreateEvidence(connection, processId, "ClientSessionData", false, GetClientSessionData(operadora));
                CreateEvidence(connection, processId, "HttpRequest", false, GetHttpRequestEvidenceData(HttpContext.Current.Request));
                CreateExceptionEvidence(connection, processId, exception);

                Finish(connection, processId);
            }

            return true;
        }
    }
}
