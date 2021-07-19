using TELENET.SIL.PO;

/// <summary>
/// Descreve o contrato de implementação de um objeto de acesso a sessão do usuário.
/// </summary>
public interface IRequestSession
{
    /// <summary>
    /// Obtém o objeto que encapsula os dados da operadora logada.
    /// </summary>
    IOperadora Operadora { get; }

    /// <summary>
    /// Obtém o identificador único da sessão.
    /// </summary>
    string SessionID { get; }

    /// <summary>
    /// Obtém o tempo definido para expiração da sessão.
    /// </summary>
    int Timeout { get; }
}