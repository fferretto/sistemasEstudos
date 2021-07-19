#pragma warning disable 1591

using System;
using System.Data.SqlClient;

namespace Telenet.Core.Data.SqlClient
{
    internal class AsyncControl
    {
        public AsyncControl(string key, Action<int, OutputValues> callback, SqlCommand command)
        {
            Command = command;
            Callback = callback;
            Key = key;
        }

        public Action<int, OutputValues> Callback { get; private set; }

        public SqlCommand Command { get; private set; }

        public string Key { get; private set; }

        public bool Finished { get; set; }
    }
}

#pragma warning restore 1591
