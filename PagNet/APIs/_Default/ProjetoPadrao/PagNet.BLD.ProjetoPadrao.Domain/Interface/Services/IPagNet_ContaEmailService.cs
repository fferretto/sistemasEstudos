using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_ContaEmailService : IServiceBase<PAGNET_CONTAEMAIL>
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