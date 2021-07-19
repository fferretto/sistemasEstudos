using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_CADEMPRESAService : IServiceBase<PAGNET_CADEMPRESA>
    {
        Task<PAGNET_CADEMPRESA> ConsultaEmpresaById(int CodEmpresa);
        Task<PAGNET_CADEMPRESA> ConsultaEmpresaBySubRede(int codSubRede);
        Task<PAGNET_CADEMPRESA> ConsultaEmpresaByCNPJ(string CNPJ);
        Task<List<PAGNET_CADEMPRESA>> GetAllempresas();
        void AtualizaEmpresa(PAGNET_CADEMPRESA emp);
        void InserirEmpresa(PAGNET_CADEMPRESA emp);
        object[][] GetEmpresa();
    }
}
