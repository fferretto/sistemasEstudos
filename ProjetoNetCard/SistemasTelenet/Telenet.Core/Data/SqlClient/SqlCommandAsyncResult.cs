#pragma warning disable 1591

using System.Data.SqlClient;

namespace Telenet.Core.Data.SqlClient
{
    internal class SqlCommandAsyncResult : ICommandAsyncResult
    {
        public SqlCommandAsyncResult(string key, SqlCommand command, bool finished)
        {
            Key = key;
            Finished = finished;
            _command = command;
        }

        private readonly SqlCommand _command;

        public OutputValues GetOutputs { get  { return new OutputValues(_command.Parameters); } }

        public string Key { get; private set; }

        public bool Finished { get; private set; }

        public void Dispose()
        {
            Helper.RemoveAsyncResult(Key);
        }
    }
}

#pragma warning restore 1591
