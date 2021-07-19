using System.Data;

namespace NetCard.Common.Models
{
    public class Parametro
    {
        #region Construtores

        public Parametro()
        {
        }

        public Parametro(string pCampo, DbType pTipo, object pValor)
        {
            Campo = pCampo;
            Tipo = pTipo;
            Valor = pValor;
        }

        #endregion

        public string Campo { get; set; }
        public DbType Tipo { get; set; }
        public object Valor { get; set; }
    }
}
