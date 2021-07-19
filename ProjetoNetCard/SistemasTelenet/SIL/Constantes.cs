using System.Configuration;

namespace TELENET.SIL
{
    public static class ConstantesSIL
    {   
        // Status 
        public static string StatusAtivo = "00";
        public static string StatusBloqueado = "01";
        public static string StatusCancelado = "02";
        public static string StatusSuspenso = "06";
        public static string StatusSuspenso2 = "04";
        public static string StatusTransferido = "07";
        public static string StatusInadimplente = "08";
        public static string StatusEmRescisao = "09";
       

        // Flags
        public static char FlgSim = 'S';   
        public static char FlgNao = 'N';
        public static string FlgDescrSim = "SIM";
        public static string FlgDescrNao = "NAO";

        // Auxiliares
        public static char BoolToChar(bool valor)
        {
            return valor ? FlgSim : FlgNao;
        }

        public static bool CharToBool(char valor)
        {
            return (valor == FlgSim);
        }


        // Bancos Dados :: Conexoes
        public static string BDAUTORIZADOR =  "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}";
        public static string BDTELENET =      "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}";
        public static string BDCONCENTRADOR = "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}";
        public static string BDTELENETASSINCRONO = "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};Asynchronous Processing=true";

        public static string SERVERAPLICACAO = ConfigurationManager.AppSettings["serverAplicacao"];
        public static string PASTAAPLICACAO = ConfigurationManager.AppSettings["pastaAplicacao"];

        public static string ServidorConcentrador = ConfigurationManager.AppSettings["ServidorConcentrador"];
        public static string BancoConcentrador = ConfigurationManager.AppSettings["bdConcentrador"];

        // Bancos Dados :: Usuario/Senha 
        public static string UsuarioBanco = "netcard";
        public static string SenhaBanco = "netcard2222";

        //Constante Sistema POS
        public static int SistemaPOS = 0;
        public static string PosPago = "PÓS PAGO";

        //Constante Sistema PJ
        public static int SistemaPRE = 1;
        public static string PrePago = "PRÉ PAGO";

    }
}

 