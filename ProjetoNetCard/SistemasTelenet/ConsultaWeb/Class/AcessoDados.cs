#pragma warning disable 1591

using NetCard.Common.Util;
using NetCardConsulta.Configs.Shared;
using System;
using Telenet.Core.Data;
using Telenet.Core.Data.SqlClient;

namespace NetCardConsulta.Class
{
    public class AcessoDados : IAcessoDados
    {
        public AcessoDados(IRequestSession requestSession)
        {
            if (requestSession != null)
            {
                if (requestSession.ObjetoConexao != null)
                {
                    NomeBdNetCard = requestSession.ObjetoConexao.BancoNetcard;

                    _concentrador = new Lazy<IDbClient>(() => new SqlDbClient(Utils.GetConnectionStringConcentrador(requestSession.ObjetoConexao)));
                    _netCard = new Lazy<IDbClient>(() => new SqlDbClient(Utils.GetConnectionStringNerCard(requestSession.ObjetoConexao)));
                }
            }
        }

        private Lazy<IDbClient> _concentrador;
        private Lazy<IDbClient> _netCard;

        public string DbUser { get { return Utils.UsuarioBanco; } }

        public IDbClient Concentrador { get { return _concentrador.Value; } }

        public IDbClient NetCard { get { return _netCard.Value; } }

        public string NomeBdNetCard { get; private set; }
    }
}

#pragma warning restore 1591
