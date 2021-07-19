using System;
using System.Data;
using System.Data.Common;

namespace Telenet.Core.Data
{
    /// <summary>
    /// Objeto que garante a prontidão de uma conexão com o banco para execução de comandos.
    /// </summary>
    public class EnsureOpen : IDisposable
    {
        /// <summary>
        /// Cria uma nova instância do objeto.
        /// </summary>
        public EnsureOpen(DbConnection connection)
        {
            _connection = connection;

            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        private DbConnection _connection;

        /// <summary>
        /// Fecha a conexão ao término.
        /// </summary>
        public void Dispose()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                return;
            }

            _connection.Close();
        }
    }
}
