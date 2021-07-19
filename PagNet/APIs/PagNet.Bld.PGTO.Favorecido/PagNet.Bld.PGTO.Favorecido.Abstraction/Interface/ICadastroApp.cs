using PagNet.Bld.PGTO.Favorecido.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.Favorecido.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.Bld.PGTO.Favorecido.Abstraction.Interface
{
    public interface ICadastroApp
    {
        FavorecidosModel RetornaDadosFavorecidoByID(int codigoFavorecido, int codigoEmpresa);
        FavorecidosModel RetornaDadosFavorecidoByCodCen(int codigoCentralizadora, int codigoEmpresa);
        FavorecidosModel RetornaDadosFavorecidoByCPFCNPJ(string cpfCNPJ, int codigoEmpresa);
        List<FavorecidosModel> ConsultaTodosFavorecidosCentralizadora(int codigoEmpresa);
        List<FavorecidosModel> ConsultaTodosFavorecidosFornecedores(int codigoEmpresa);
        List<DadosLogModal> ConsultaLog(int codigoFavorecido);

        List<FavorecidosModel> ConsultaTodosFavorecidosPAG(int codigoEmpresa);
        RetornoModel SalvarFavorecido(IFavorecidosModel favorecido);
        RetornoModel DesativaFavorecido(int codigoFavorecido);
    }
}
