using System;

namespace TELENET.SIL.PO
{
    public interface IOperadora
    {
        Int16 CODOPE { get; set; }
        string LOGIN { get; set; }
        string NOMCLI { get; set; }
        string SENHA { get; set; }
        string SERVIDORAUT { get; set; }
        string SERVIDORCON { get; set; }
        string SERVIDORNC { get; set; }
        string BANCOCON { get; set; }
        string BANCOAUT { get; set; }
        string BANCONC { get; set; }
        string SERVIDORIIS { get; set; }
        bool AUTENTICADO { get; set; }
        string USUARIOBD { get; }
        string SENHABD { get; }
        Int16 IDPERFIL { get; set; }
        Int16 ID_FUNC { get; set; }
        string NOMEOPERADORA { get; set; }
        string STA { get; set; }
        bool HABAGRUPAMENTO { get; set; }
        int CODAG { get; set; }
        int NUMCONSREGTRANS { get; set; }
    }
}
