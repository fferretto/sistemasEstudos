using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NetCard.Common.Models
{
    public class RedeSegmento
    {
        public string NomSeg { get; set; }
        public List<Rede> Redes { get; set; }

        public List<RedeSegmento> GeraRedesSegmento(List<Rede> listaRede )
        {
            var listaRedeSegmento = new List<RedeSegmento>();
            foreach (var x in listaRede)
            {
                var blnRepetido = listaRedeSegmento.Any(segRepetido => segRepetido.NomSeg == x.NomSeg);
                if (blnRepetido) continue;
                var redeSegmento = new RedeSegmento();
                redeSegmento.NomSeg = x.NomSeg;
                redeSegmento.Redes = new List<Rede>();
                foreach (var filtroSegmento in listaRede.Where(filtroSegmento => filtroSegmento.NomSeg == redeSegmento.NomSeg))
                {
                    var rede = filtroSegmento;
                    redeSegmento.Redes.Add(rede);
                }
                listaRedeSegmento.Add(redeSegmento);
            }

            return listaRedeSegmento;
        }

        public List<RedeSegmento> GeraRedes(List<Rede> listaRede) 
        {
            var listaRedeSegmento = new List<RedeSegmento>();
            var redeSegmento = new RedeSegmento();
            redeSegmento.NomSeg = "Todos";
            redeSegmento.Redes = listaRede;
            listaRedeSegmento.Add(redeSegmento);
            return listaRedeSegmento; 
        }

        public string LiberaSegmentoConsultaRede(ObjConn objConexao, DadosAcesso dadosAcesso)
        {
            var retorno = string.Empty;

            if (dadosAcesso == null || objConexao == null)
                return retorno;

            string tabela = dadosAcesso.Sistema.cartaoPJVA == 0 ? "PARAM" : "PARAMVA";
            string sql = @"SELECT VAL FROM " + tabela + " WITH (NOLOCK) WHERE ID0 = 'LIBSEGCONSREDE'";
            string query = string.Format(sql);

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));
            SqlCommand comando = new SqlCommand(query, conexao);

            try
            {
                DataTable dt = new DataTable();
                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    retorno = Convert.ToString(dt.Rows[0]["VAL"]);
                }
                return retorno;
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();
                return retorno;
            }
        }
    }
}
