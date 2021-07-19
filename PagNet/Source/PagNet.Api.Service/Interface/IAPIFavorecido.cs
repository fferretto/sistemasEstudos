using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System.Collections.Generic;

namespace PagNet.Api.Service.Interface
{
    public interface IAPIFavorecido
    {
        IAPIFavorecidoModel RetornaDadosFavorecidoByID(int codigoFavorecido, int codigoEmpresa);
        IAPIFavorecidoModel RetornaDadosFavorecidoByCodCen(int codigoCentralizadora, int codigoEmpresa);
        IAPIFavorecidoModel RetornaDadosFavorecidoByCPFCNPJ(string cpfCNPJ, int codigoEmpresa);
        List<APIFavorecidoModel> ConsultaTodosFavorecidosCentralizadora(int codigoEmpresa);
        List<APIFavorecidoModel> ConsultaTodosFavorecidosFornecedores(int codigoEmpresa);
        List<APIFavorecidoModel> ConsultaTodosFavorecidosPAG(int codigoEmpresa);
        IDictionary<bool, string> SalvarFavorecido(IAPIFavorecidoModel favorecido);
        IDictionary<bool, string> DesativaFavorecido(int codigoFavorecido);
        List<APIDadosLogModal> ConsultaLog(int codigoFavorecido);
    }
}
