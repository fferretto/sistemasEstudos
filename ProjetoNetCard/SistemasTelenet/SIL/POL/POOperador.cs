using System;

namespace TELENET.SIL.PO
{
    // OPERADOR
    public class OPERADORA : IOperadora
    {
        public Int16 CODOPE { get; set; }
        public string LOGIN { get; set; }
        public string NOMCLI { get; set; }
        public string SENHA { get; set; }
        public string SERVIDORAUT { get; set; }
        public string SERVIDORCON { get; set; }        
        public string SERVIDORNC { get; set; }
        public string BANCOCON { get; set; }
        public string BANCOAUT { get; set; }
        public string BANCONC { get; set; }
        public string SERVIDORIIS { get; set; }
        public bool AUTENTICADO { get; set; }
        public string USUARIOBD { get { return ConstantesSIL.UsuarioBanco;} }
        public string SENHABD { get { return ConstantesSIL.SenhaBanco;} }
        public Int16 IDPERFIL { get; set; }
        public Int16 ID_FUNC { get; set; }
        public string NOMEOPERADORA { get; set; }
        public bool POSSUI_PAGNET { get; set; }
        public string STA { get; set; }
        public bool HABAGRUPAMENTO { get; set; }
        public int CODAG { get; set; }
        public int NUMCONSREGTRANS { get; set; }
        public string CODCLI { get; set; }
        public string ACESSO { get; set; }
    }

}
