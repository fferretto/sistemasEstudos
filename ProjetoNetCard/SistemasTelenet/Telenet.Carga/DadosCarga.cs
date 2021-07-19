#pragma warning disable 1591

using System;
using System.Data;
using Telenet.Carga.Abstractions;
using Telenet.Core.Data;

namespace Telenet.Carga
{
    internal class DadosCarga : IDadosCarga, ILoadableObject
    {
        public DadosCarga()
        {
            EtapaCarga = EtapaCarga.SemCargaEmExecucao;
        }

        public DadosCarga(int erro)
        {
            Erro = erro;
        }

        public bool BaixouLog { get; private set; }

        public string Cnpj { get; private set; }

        public int CodigoCliente { get; private set; }

        public DateTime? DataProgramacao { get; private set; }

        public int Erro { get; private set; }

        public string IdCarga { get; private set; }

        public string IdProcesso { get; private set; }

        public string NomeArquivoCarga { get; private set; }

        public string NomeArquivoOriginal { get; private set; }

        public string NomeTabela { get; private set; }

        public int NumeroCarga { get; private set; }

        public EtapaCarga EtapaCarga { get; private set; }

        public int TotalRegistros { get; private set; }

        public decimal ValorCarga { get; private set; }

        public void LoadFrom(IDataReader reader)
        {
            BaixouLog = reader.GetValue<bool>("BAIXOU_LOG");
            Cnpj = reader.GetValue<string>("CNPJ");
            CodigoCliente = reader.GetValue<int>("CLIENTE_ORIGEM");
            DataProgramacao = reader.GetDTValue("DT_PROG", "yyyyMMdd");
            Erro = reader.GetValue<int>("ERRO");
            IdCarga = reader.GetValue<string>("ID_CARGA");
            IdProcesso = reader.GetValue<string>("ID_PROCESSO");
            NomeArquivoCarga = reader.GetValue<string>("NOM_CARGA_ARQUIVO");
            NomeArquivoOriginal = reader.GetValue<string>("NOM_ORIGINAL_ARQUIVO");
            NomeTabela = reader.GetValue<string>("NOME_TABELA");
            NumeroCarga = reader.GetValue<int>("NUM_CARGA");
            EtapaCarga = reader.GetValue<EtapaCarga>("ETAPA_PROC", EtapaCarga.SemCargaEmExecucao);
            TotalRegistros = reader.GetValue<int>("TOT_REGS");
            ValorCarga = reader.GetValue<decimal>("VALOR_CARGA");
        }
    }
}

#pragma warning restore 1591
