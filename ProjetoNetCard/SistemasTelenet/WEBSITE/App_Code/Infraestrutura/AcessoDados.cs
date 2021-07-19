#pragma warning disable 1591

using Telenet.Core.Data;
using Telenet.Core.Data.SqlClient;
using TELENET.SIL;

/// <summary>
/// Summary description for AcessoDados
/// </summary>
public class AcessoDados : IAcessoDados
{
    public AcessoDados(IRequestSession requestSession)
    {
        Concentrador = new SqlDbClient(string.Format(ConstantesSIL.BDCONCENTRADOR,
            requestSession.Operadora.SERVIDORCON,
            requestSession.Operadora.BANCOCON, 
            ConstantesSIL.UsuarioBanco, 
            ConstantesSIL.SenhaBanco));

        NetCard = new SqlDbClient(string.Format(ConstantesSIL.BDTELENET,
            requestSession.Operadora.SERVIDORNC,
            requestSession.Operadora.BANCONC,
            ConstantesSIL.UsuarioBanco,
            ConstantesSIL.SenhaBanco));

        NomeBdNetCard = requestSession.Operadora.BANCONC;
    }

    public string DbUser { get { return TELENET.SIL.ConstantesSIL.UsuarioBanco; } }

    public string NomeBdNetCard { get; private set; }

    public IDbClient Concentrador { get; private set; }

    public IDbClient NetCard { get; private set; }
}

#pragma warning restore 1591
