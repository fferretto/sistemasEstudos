#pragma warning disable 1591

using System;
using System.Data;
using Telenet.Core.Data;

namespace Telenet.Carga
{
    internal class RetornoProcCarga : IRetornoProcCarga, ILoadableObject
    {
        public int Erro { get; private set; }
        public string Mensagem { get; private set; }

        public virtual void LoadFrom(IDataReader reader)
        {
            Erro = Convert.ToInt32(reader["ERRO"]);
            Mensagem = Convert.ToString(reader["MENSAGEM"]);
        }
    }
}

#pragma warning restore 1591
