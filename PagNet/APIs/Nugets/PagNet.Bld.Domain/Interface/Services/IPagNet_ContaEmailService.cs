using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_CONTAEMAILService : IServiceBase<PAGNET_CONTAEMAIL>
    {
        bool ExisteContaPrincipalCadastrada();

        object[][] GetHashEmail(int codSubRede);
        Task<PAGNET_CONTAEMAIL> GetEmailById(int id);
        Task<List<PAGNET_CONTAEMAIL>> GetallEmailAtivos();
        Task<PAGNET_CONTAEMAIL> IncluirEmail(PAGNET_CONTAEMAIL conta);
        void AtualizaEmail(PAGNET_CONTAEMAIL conta);
        void InseriLog(PAGNET_LOGEMAILENVIADO log);
    }
}