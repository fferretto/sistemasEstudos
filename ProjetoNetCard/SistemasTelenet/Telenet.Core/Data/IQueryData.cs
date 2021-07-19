using System.Collections.Generic;

namespace Telenet.Core.Data
{
    /// <summary>
    /// Define o contrato de implementação de um comando especializado em executar queries no banco de dados.
    /// </summary>
    public interface IQueryData
    {
        /// <summary>
        /// Obtém um enumerado para os objetos que representam o resultset da consulta.
        /// </summary>
        IEnumerable<TRecord> GetData<TRecord>(params object[] values) 
            where TRecord : ILoadableObject, new();

        /// <summary>
        /// Obtém um enumerado para os objetos que representam o resultset da consulta.
        /// </summary>
        IEnumerable<TRecord> GetData<TRecord, TLoader>(params object[] values)
            where TRecord : new()
            where TLoader : IObjectLoader<TRecord>, new();
    }
}
