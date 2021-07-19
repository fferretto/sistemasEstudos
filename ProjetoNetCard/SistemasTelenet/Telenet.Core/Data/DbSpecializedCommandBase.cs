using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;

namespace Telenet.Core.Data
{
    /// <summary>
    /// Define a classe base para a criação de comandos especializados.
    /// </summary>
    public abstract class DbSpecializedCommandBase
    {
        /// <summary>
        /// Cria uma nova instância da classe.
        /// </summary>
        protected DbSpecializedCommandBase(DbCommand commandBase, string parameterPrefixPattern)
        {
            CommandBase = commandBase;
            ParameterPrefixPattern = parameterPrefixPattern;
            CreateParameters();
        }

        private const string OutputTag = "OUTPUT";
        private const string ReturnTag = "RETURN";
        private readonly string ParameterPrefixPattern;

        /// <summary>
        /// Associa os valores com o parâmetros de saída.
        /// </summary>
        protected void BindParameters(params object[] values)
        {
            var i = 0;

            foreach (var parameter in CommandBase
                .Parameters
                .Cast<DbParameter>().Where(p => p.Direction == ParameterDirection.Input || p.Direction == ParameterDirection.InputOutput))
            {
                parameter.Value = values[i++];
            }
        }

        /// <summary>
        /// Obtém o comando base para a criação do comando especializado.
        /// </summary>
        protected DbCommand CommandBase { get; private set; }

        /// <summary>
        /// Cria os parâmetros com base na sentença SQL.
        /// </summary>
        protected virtual void CreateParameters()
        {
            var matches = Regex.Matches(
                CommandBase.CommandText,
                string.Concat(ParameterPrefixPattern, "(\\s+\\w*(\\(\\d+\\)|\\(\\d+,\\d+\\))*\\s+(OUTPUT|RETURN))*"),
                RegexOptions.IgnoreCase);

            if (matches.Count == 0)
            {
                return;
            }

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i].Value;

                if (match.Contains(OutputTag))
                {
                    CreateOutputCommand(match);
                }
                else
                {
                    var parameter = CommandBase.CreateParameter();
                    parameter.ParameterName = matches[i].Value.Trim();
                    CommandBase.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// Cria um comando de saída.
        /// </summary>
        protected virtual void CreateOutputCommand(string matchedName)
        {
            var parameter = CommandBase.CreateParameter();
            parameter.ParameterName = Regex.Match(matchedName, ParameterPrefixPattern).Value.Trim();
            parameter.Direction = ParameterDirection.Output;

            var matchedType = Regex.Match(
                matchedName,
                "\\w+(\\(\\d+\\)|\\(\\d+,\\d+\\))*\\s(OUTPUT|RETURN)",
                RegexOptions.IgnoreCase).Value
                .Replace(OutputTag, string.Empty)
                .Replace(ReturnTag, string.Empty).Trim();

            var matchedSize = Regex.Match(matchedType, "(\\(\\d+\\)|\\(\\d+,\\d+\\))", RegexOptions.IgnoreCase);

            if (matchedSize.Success)
            {
                var type = matchedType.Replace(matchedSize.Value, string.Empty).Trim();
                var indexOf = matchedSize.Value.IndexOf(',');

                if (indexOf == -1)
                {
                    ConfigureParameter(parameter, type, Convert.ToInt32(Regex.Match(matchedSize.Value, "\\d+").Value), 0, 0);
                }
                else
                {
                    ConfigureParameter(parameter, type, 0, 
                        Convert.ToByte(matchedSize.Value.Substring(indexOf + 1, matchedSize.Value.Length - indexOf - 2)), 
                        Convert.ToByte(matchedSize.Value.Substring(1, indexOf - 1)));
                }
            }
            else
            {
                ConfigureParameter(parameter, matchedType, 0, 0, 0);
            }

            CommandBase.Parameters.Add(parameter);
        }

        /// <summary>
        /// Configura um parâmetro de saída.
        /// </summary>
        protected abstract void ConfigureParameter(DbParameter parameter, string type, int size, byte scale, byte precision);

        /// <summary>
        /// Recupera o parâmetro de retorno do comando.
        /// </summary>
        protected virtual TValue GetReturnValue<TValue>()
        {
            var returnValue = CommandBase
                .Parameters
                .Cast<DbParameter>()
                .FirstOrDefault(p => p.Direction == ParameterDirection.ReturnValue);

            if (returnValue == null)
            {
                throw new ArgumentOutOfRangeException("RETURN");
            }

            return returnValue.Value == null || returnValue.Value == DBNull.Value
                ? default(TValue)
                : (TValue)returnValue.Value;
        }
    }
}
