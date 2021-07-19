#pragma warning disable 1591

using System;
using System.Data;
using Telenet.Carga.Abstractions;
using Telenet.Core.Data;

namespace Telenet.Carga
{
    internal class CargaSolicitada : ICargaSolicitada, ILoadableObject
    {
        public int CodigoCliente { get; private set; }

        public int NumeroCarga { get; private set; }

        public DateTime DataSolicitacao { get; private set; }

        public DateTime DataProgramacao { get; private set; }

        public decimal Valor { get; private set; }

        public int Quantidade { get; private set; }

        public string Situacao { get; private set; }

        public void LoadFrom(IDataReader reader)
        {
            CodigoCliente = reader.GetValue<int>("CLIENTE");
            NumeroCarga = reader.GetValue<int>("NUM_CARGA");
            DataSolicitacao = reader.GetDTValue("DT_SOLICITACAO");            
            DataProgramacao = reader.GetDTValue("DT_PROGRAMACAO");
            Valor = reader.GetValue<decimal>("VALOR");
            Quantidade = reader.GetValue<int>("QUANTIDADE");
            Situacao = reader.GetValue<string>("SITUACAO");
        }
    }
}

#pragma warning restore 1591
