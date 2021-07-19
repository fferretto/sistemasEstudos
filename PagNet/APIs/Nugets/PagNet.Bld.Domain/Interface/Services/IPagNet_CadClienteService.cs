using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_CADCLIENTEService : IServiceBase<PAGNET_CADCLIENTE>
    {
        Task<List<PAGNET_CADCLIENTE>> BuscaAllCliente();
        Task<List<PAGNET_CADCLIENTE>> BuscaAllClienteByCodEmpresa(int codEmpresa, string TipoCliente);
        Task<PAGNET_CADCLIENTE> BuscaClienteByCNPJ(string CNPJ);
        Task<PAGNET_CADCLIENTE> BuscaClienteByID(int codCli);
        Task<PAGNET_CADCLIENTE> BuscaClienteByCNPJeCodEmpresa(string CNPJ, int codempresa);
        Task<PAGNET_CADCLIENTE> BuscaClienteByIDeCodEmpresa(int codCli, int codempresa);

        void IncluiCliente(PAGNET_CADCLIENTE dados);
        void AtualizaCliente(PAGNET_CADCLIENTE dados);

        Task<List<PAGNET_CADCLIENTE_LOG>> ConsultaLog(int codCli);
        void InsertLog(PAGNET_CADCLIENTE Cliente, int codUsuario, string Justificativa);
    }
}
