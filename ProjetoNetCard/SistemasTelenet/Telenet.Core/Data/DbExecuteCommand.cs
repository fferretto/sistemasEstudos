#pragma warning disable 1591

using System.Data.Common;

namespace Telenet.Core.Data
{
    public abstract class DbExecuteCommand : DbSpecializedCommandBase, IExecuteCommand
    {
        protected DbExecuteCommand(DbCommand commandBase, string parameterPrefixPattern)
            : base(commandBase, parameterPrefixPattern)
        { }

        public int Execute(params object[] values)
        {
            using (new EnsureOpen(CommandBase.Connection))
            {
                BindParameters(values);
                return CommandBase.ExecuteNonQuery();
            }
        }
    }
}

#pragma warning restore 1591
