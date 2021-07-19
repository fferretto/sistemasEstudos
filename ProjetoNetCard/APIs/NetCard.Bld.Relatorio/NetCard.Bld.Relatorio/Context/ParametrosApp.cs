using NetCard.Bld.Relatorio.Abstraction.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NetCard.Bld.Relatorio.Context
{
    public class ParametrosInfraDataApp : IParametrosApp
    {
        public ParametrosInfraDataApp(ClaimsPrincipal principal)
        {
            _principal = principal;
        }

        private ClaimsPrincipal _principal;

        private TValue GetValue<TValue>(string name, TValue defaultValue)
        {
            var claim = _principal.Claims.FirstOrDefault(c => c.Type.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (claim == null)
                return defaultValue;

            return (TValue)Convert.ChangeType(claim.Value, typeof(TValue));
        }

        private TValue IsTrue<TValue>(string name, TValue defaultValue)
        {
            var claim = _principal.Claims.FirstOrDefault(c => c.Value == name);

            if (claim == null)
                return defaultValue;

            return (TValue)Convert.ChangeType(true, typeof(TValue));
        }

        private TValue ValidaSN<TValue>(string name, TValue defaultValue)
        {
            var valor = GetValue(name, "N");

            if (valor == "S")
                return (TValue)Convert.ChangeType(true, typeof(TValue));
            else
                return (TValue)Convert.ChangeType(false, typeof(TValue));
        }

        public int cod_ope => GetValue("cod_ope", 0);

        public string id_login => GetValue("id_login", string.Empty);

        public string tp_usu => GetValue("tp_usu", string.Empty);

        public string BdNetCard => GetValue("ds_data", string.Empty);

        public string GetConnectionString()
        {
            var banco = GetValue("ds_data", string.Empty);
            var servidor = GetValue("srv_data", string.Empty);
            string newServer = ReturnIPByServidor(servidor);

            return $"Data Source={newServer};Initial Catalog={banco};Timeout=180;Persist Security Info=True;User ID=TLNUSUMW;Password=TLN22BH22";
        }
        public string GetConnectionStringConcentrador()
        {
            var banco = "";
            var servidor = GetValue("srv_data", string.Empty);
            string newServer = ReturnIPByServidor(servidor);

            if (servidor == "NETUNO") banco = "CONCENTRADOR_TESTE";
            else banco = "CONCENTRADOR";

            return $"Data Source={newServer};Initial Catalog={banco};Timeout=500;Persist Security Info=True;User ID=TLNUSUMW;Password=TLN22BH22";
        }

        private string ReturnIPByServidor(string nmServidor)
        {
            var dadosRetorno = nmServidor;

            if (nmServidor.ToUpper() == "ZEUS")
            {
                dadosRetorno = "192.168.204.14";
            }
            else if (nmServidor.ToUpper() == "SHARON")
            {
                dadosRetorno = "192.168.70.02";
            }
            else if (nmServidor.ToUpper() == "PANDORA")
            {
                dadosRetorno = "192.168.70.91";
            }

            return dadosRetorno;
        }
    }
}
