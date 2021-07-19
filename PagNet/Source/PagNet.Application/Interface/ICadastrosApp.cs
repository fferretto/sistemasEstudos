using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface
{
    public interface ICadastrosApp
    {
        Task<EnderecoVM> RetornaEndereco(string CEP);
        SubRedeVM RetornaDadosSubRede(int CodSubRede);
        CadEmpresaVm RetornaDadosEmpresaByID(int CodEmpresa);
        Task<List<CadEmpresaVm>> ConsultaTodasEmpresas();
        Task<IDictionary<bool, string>> SalvarEmpresa(CadEmpresaVm Emp);

        //CadFavorecidosVm RetornaDadosFavoritoByID(int CodFavorito);
        //CadFavorecidosVm RetornaDadosFavoritoByCodCen(int codCen);
        //CadFavorecidosVm RetornaDadosFavoritoByCPFCNPJ(string cpfCNPJ);
        //Task<List<CadFavorecidosVm>> ConsultaTodosFavorecidosCentralizadora();
        //Task<List<CadFavorecidosVm>> ConsultaTodosFavorecidosFornecedores(int codEmpresa);

        //Task<List<CadFavorecidosVm>> ConsultaTodosFavorecidosPAG(int codEmpresa);
        //Task<IDictionary<bool, string>> SalvarFavorito(CadFavorecidosVm favorito);
        //Task<IDictionary<bool, string>> DesativaFavorito(int codFavorito, int codUsuario);


        //CadClienteVm RetornaDadosClienteByID(int codCli);
        //CadClienteVm RetornaDadosClienteByCPFCNPJ(string cpfCNPJ);

        //CadClienteVm RetornaDadosClienteByIDeCodEmpresa(int codCli, int codempresa);
        //CadClienteVm RetornaDadosClienteByCPFCNPJeCodEmpresa(string cpfCNPJ, int codempresa);
        //Task<List<CadClienteVm>> ConsultaTodosCliente(int codEmpresa, string TipoCliente);
        //Task<IDictionary<bool, string>> SalvarCliente(CadClienteVm cliente);
        //Task<IDictionary<bool, string>> DesativaCliente(int codCli, int codUsuario, string Justificativa);

        int CriaClienteUsuarioByEmpresa(int codigoEmpresa);

    }
}
