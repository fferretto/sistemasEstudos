using NetCard.Common.Models;

namespace NetCardConsulta.Configs.Shared
{
    /// <summary>
    /// Define o contrato de implementação de um objeto de acesso aos dados da sessão básica do aplicativo.
    /// </summary>
    public interface IRequestSession
    {
        /// <summary>
        /// Obtém o objeto de configuração de acesso do usuário.
        /// </summary>
        IDadosAcesso DadosAcesso { get; }

        /// <summary>
        /// Obtém o objeto de configuração de acesso ao banco de dados.
        /// </summary>
        IObjetoConexao ObjetoConexao { get; }

        /// <summary>
        /// Obtém o objeto de permissões configuradas para o usuário logado.
        /// </summary>
        IPermissao Permissao { get; }

        /// <summary>
        /// Obtém o identificador único da sessão.
        /// </summary>
        string SessionID { get; }

        /// <summary>
        /// Obtém o tempo configurado para expiração da sessão.
        /// </summary>
        int Timeout { get; }
    }
}