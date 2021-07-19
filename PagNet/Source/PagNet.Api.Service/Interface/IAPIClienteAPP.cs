using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PagNet.Api.Service.Interface
{
    public interface IAPIClienteAPP
    {
        APIClienteModel RetornaDadosClienteByID(int codCli);
        APIClienteModel RetornaDadosClienteByCPFCNPJ(string cpfCNPJ);

        APIClienteModel RetornaDadosClienteByIDeCodEmpresa(int codCli, int codempresa);
        APIClienteModel RetornaDadosClienteByCPFCNPJeCodEmpresa(string cpfCNPJ, int codempresa);
        Task<List<APIClienteModel>> ConsultaTodosCliente(int codEmpresa, string TipoCliente);
        Task<IDictionary<bool, string>> SalvarCliente(IAPIClienteModel cliente);
        Task<IDictionary<bool, string>> DesativaCliente(int codCli, string Justificativa);
    }
}
