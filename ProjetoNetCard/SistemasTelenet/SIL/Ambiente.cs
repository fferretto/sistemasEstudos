namespace SIL
{
    public class Ambiente
    {
        public Ambiente(string pathConfiguracao,
            string hostBaseConcentrador,
            string nomeBaseConcentrador,
            string hostBseNetCard,
            string nomeBaseNetCard)
        {
            var ipAlias = new IpAlias(pathConfiguracao);
            var dataConcentrador = ipAlias.GetEntry(hostBaseConcentrador.ToLower());
            var dataNetCard = ipAlias.GetEntry(hostBseNetCard.ToLower());

            AmbienteTeste = dataNetCard.Type == IpType.Test;
            if (AmbienteTeste)
                AlertaAmbienteTeste = string.Concat(
                    "SISTEMA ACESSANDO BASE DE TESTES OU HOMOLOGAÇÃO :: ",
                    "SERVIDOR CONCENTRADOR: ", dataConcentrador.Alias.ToUpper().Trim().Substring(0, 3).PadRight(6, '*'),
                    " - BANCO CONCENTRADOR: ", nomeBaseConcentrador.ToUpper().Trim().Substring(0, 6).PadRight(15, '*'),
                    " :: SERVIDOR NC: ", dataNetCard.Alias.ToUpper().Substring(0, 3).PadRight(6, '*'),
                    " - BANCO NC: ", nomeBaseNetCard.ToUpper().Trim().Substring(0, 6).PadRight(15, '*'));

        }

        public bool AmbienteTeste { get; private set; }
        public string AlertaAmbienteTeste { get; private set; }
    }
}
