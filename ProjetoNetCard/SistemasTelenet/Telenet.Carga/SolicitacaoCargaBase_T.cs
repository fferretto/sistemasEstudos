using System;
using Telenet.Core.Data;

namespace Telenet.Carga
{
    /// <summary>
    /// Classe base para construção de regras de carga.
    /// </summary>
    /// <typeparam name="TContexto">O contrato de contexto de carga.</typeparam>
    public abstract class SolicitacaoCargaBase<TContexto> where TContexto : IContextoCarga
    {
        /// <summary>
        /// Cria uma nova instância da classe.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Valor não informado para contexto ou acessoDados.</exception>
        protected SolicitacaoCargaBase(TContexto contexto, IAcessoDados acessoDados)
        {
            if (contexto == null)
                throw new ArgumentNullException("contexto");

            AcessoDados = acessoDados ?? throw new ArgumentNullException("acessoDados");
            Contexto = contexto;
        }

        /// <summary>
        /// Obtém o objeto de acesso a dados para a carga.
        /// </summary>
        protected IAcessoDados AcessoDados { get; private set; }

        /// <summary>
        /// Obtém o objeto de contexto da carga.
        /// </summary>
        protected TContexto Contexto { get; private set; }
    }
}
