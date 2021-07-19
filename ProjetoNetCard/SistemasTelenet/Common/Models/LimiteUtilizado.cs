using NetCard.Common.Util;
using System;
using System.Data;
using System.Linq;
using Telenet.Core.Data;
using Telenet.Core.Data.SqlClient;

namespace NetCard.Common.Models
{
    public class LimiteUtilizado
    {
        public LimiteUtilizado(IDadosAcesso dadosAcesso, IObjetoConexao objetoConexao)
        {
            _dadosAcesso = dadosAcesso;
            _objetoConexao = objetoConexao;
            CarregaLimites();
        }

        private IDadosAcesso _dadosAcesso;
        private IObjetoConexao _objetoConexao;

        private class LimiteUtilizadoVO : ILoadableObject
        {
            public decimal ValorTotal { get; private set; }
            public decimal ValorUtilizado { get; private set; }

            public void LoadFrom(IDataReader reader)
            {
                ValorTotal = Convert.ToDecimal(reader["VALORTOTAL"]);
                ValorUtilizado = Convert.ToDecimal(reader["VALORUTILIZADO"]);
            }
        }

        private void CarregaLimites()
        {
            Sucesso = false;
            MensagemErro = "";

            try
            {
                var sqlClient = new SqlDbClient(Utils.GetConnectionStringNerCard(_objetoConexao));
                var command = sqlClient.StoredProcedure(@"CONSULTA_LIMITES_CLIENTE 
@SISTEMA, 
@CODCLI, 
@ERRO INT OUTPUT, 
@MSG_ERRO VARCHAR(255) OUTPUT");
                var result = command.GetData<LimiteUtilizadoVO>(_dadosAcesso.Sistema.cartaoPJVA, _dadosAcesso.Codigo).FirstOrDefault();

                var outputs = command.GetOutputs();

                if (outputs.Get<int>("@ERRO") != 0)
                {
                    MensagemErro = outputs.Get<string>("@MSG_ERRO");
                    return;
                }

                Sucesso = true;
                Sistema = _dadosAcesso.SistemaAcessado;
                ValorTotal = result.ValorTotal;
                ValorUtilizado = result.ValorUtilizado;
            }
            catch
            {
                Sucesso = false;
                MensagemErro = "Erro na leitura dos limites";
            }
        }

        public string MensagemErro { get; private set; }

        public decimal ValorTotal { get; private set; }

        public decimal ValorUtilizado { get; private set; }

        public int Sistema { get; private set; }

        public bool Sucesso { get; private set; }
    }
}
