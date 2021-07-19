using PagNet.BLD.Cliente.Abstraction.Interface.Model;
using PagNet.BLD.Cliente.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.BLD.Cliente.Abstraction.Interface
{
    public interface IClienteApp
    {
        ClienteVm RetornaDadosClienteByID(IFiltroCliente filtro);
        ClienteVm RetornaDadosClienteByCPFCNPJ(IFiltroCliente filtro);
        ClienteVm RetornaDadosClienteByIDeCodEmpresa(IFiltroCliente filtro);
        ClienteVm RetornaDadosClienteByCPFCNPJeCodEmpresa(IFiltroCliente filtro);
        List<ClienteVm> ConsultaTodosCliente(IFiltroCliente filtro);
        void SalvarCliente(IClienteVm cliente);
        void DesativaCliente(IFiltroCliente filtro);
    }
}
