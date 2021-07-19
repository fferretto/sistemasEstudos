#pragma warning disable 1591

using System.Data;
using Telenet.Carga.Abstractions;
using Telenet.Core.Data;

namespace Telenet.Carga
{
    internal class ResumoCarga : IResumoCarga, ILoadableObject
    {
        public int CodigoCliente { get; private set; }

        public string IdCarga { get; private set; }

        public int NumeroCarga { get; private set; }

        public string RegistroLog { get; private set; }

        public char TipoRegistro { get; private set; }

        public void LoadFrom(IDataReader reader)
        {
            var registroLog = reader.GetValue<string>("REGISTRO_LOG");

            CodigoCliente = reader.GetValue<int>("CODCLI");
            IdCarga = reader.GetValue<string>("ID");
            NumeroCarga = reader.GetValue<int>("NUM_CARGA");
            RegistroLog = registroLog == null ? registroLog : registroLog.TrimEnd();
            TipoRegistro = reader.GetValue<char>("TIPO_REG");
        }
    }
}

#pragma warning restore 1591
