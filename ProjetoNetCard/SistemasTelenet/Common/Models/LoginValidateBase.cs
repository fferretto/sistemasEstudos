using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System;
using System.Data;
using System.Data.SqlClient;
using Telenet.Core.Authorization;
using Telenet.Core.DependencyInjection;

namespace NetCard.Common.Models
{
    public abstract class LoginValidateBase : ILoginValidate
    {
        protected LoginValidateBase(ObjConn objConexao)
        {
            ObjConexao = objConexao;
            Database = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
        }

        protected ObjConn ObjConexao { get; private set; }
        protected SqlDatabase Database { get; private set; }

        protected virtual SqlCommand CreateLoginCommand(Login login)
        {
            var command = Database.GetStoredProcCommand("MW_LOGIN") as SqlCommand;

            Database.AddInParameter(command, "@ACESSO", SqlDbType.NVarChar, login.Acesso);
            Database.AddInParameter(command, "@CODCLI", SqlDbType.Int, login.Codigo);
            Database.AddInParameter(command, "@LOGIN", SqlDbType.NVarChar, login.LogIn);
            Database.AddInParameter(command, "@SENHA", SqlDbType.NVarChar, login.Senha);

            return command;
        }

        protected virtual void OAuthAuthenticate(string username, string password)
        {
            try
            {
                var authorization = ServiceConfiguration
                    .ServiceProvider
                    .GetService<IUserAuthorization<ConsultaWebAuthorizationContext>>();

                authorization.Authenticate(username, password);
            }
            catch (Exception exception)
            {
            }
        }

        protected abstract LoginValidation OnValidate(IDataReader reader, Login login);

        public virtual LoginValidation ValidateLogin(Login login)
        {
            using (var command = CreateLoginCommand(login))
            using (var reader = Database.ExecuteReader(command))
            {
                try
                {
                    return OnValidate(reader, login);
                }
                catch
                {
                    return new LoginValidation(false, "Ocorreu um erro durante o login. Favor entrar em contato com a operadora.", null, null);
                }
            }
        }
    }
}
