#pragma warning disable 1591

using System;
using System.Data;
using Telenet.Carga.Abstractions;
using Telenet.Core.Data;

namespace Telenet.Carga
{
    internal class StatusCarga : IStatusCarga, ILoadableObject
    {
        public StatusCarga()
        {
            IniciaInstancia(0, null, EtapaCarga.SemCargaEmExecucao, null, 0);
        }

        public StatusCarga(int erro, string mensagem, EtapaCarga etapaCarga, string idProcesso, int nivel)
        {
            IniciaInstancia(erro, mensagem, etapaCarga, idProcesso, nivel);
        }

        public StatusCarga(OutputValues parameters)
        {
            IniciaInstancia(parameters.Get<int>("@ERRO", 0),
                parameters.Get<string>("@MSG_ERRO", null), 
                (EtapaCarga)Enum.Parse(typeof(EtapaCarga), parameters.Get<int>("@ETAPA_PROC", -1).ToString(), true), 
                parameters.Get<string>("@ID_PROCESSO", null), 
                parameters.Get<int>("@NIVEL", 0));
        }

        private void IniciaInstancia(int erro, string mensagem, EtapaCarga etapaCarga, string idProcesso, int nivel)
        {
            Erro = erro;
            EtapaCarga = etapaCarga;
            IdProcesso = idProcesso;
            Mensagem = mensagem;
            Nivel = nivel;

            if (EtapaCarga == EtapaCarga.SemCargaEmExecucao && !string.IsNullOrEmpty(IdProcesso))
            {
                EtapaCarga = EtapaCarga.ValidacaoLayoutErro;
            }
        }

        public int Erro { get; private set; }
        public EtapaCarga EtapaCarga { get; private set; }
        public string IdProcesso { get; private set; }
        public string Mensagem { get; private set; }
        public int Nivel { get; private set; }

        public void LoadFrom(IDataReader reader)
        {
            var prefixo = string.Empty;

            if (reader.HasField("@ERRO"))
            {
                prefixo = "@";
            }

            IniciaInstancia(reader.GetValue<int>(string.Concat(prefixo, "ERRO")),
                reader.GetValue<string>(string.Concat(prefixo, "MSG_ERRO")), 
                reader.GetValue<EtapaCarga>(string.Concat(prefixo, "ETAPA_PROC")), 
                reader.GetValue<string>(string.Concat(prefixo, "ID_PROCESSO")), 
                reader.GetValue<int>(string.Concat(prefixo, "NIVEL")));
        }
    }
}

#pragma warning restore 1591

