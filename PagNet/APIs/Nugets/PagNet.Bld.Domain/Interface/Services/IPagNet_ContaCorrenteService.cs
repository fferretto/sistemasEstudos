using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_CONTACORRENTEService : IServiceBase<PAGNET_CONTACORRENTE>
    {

        object[][] GetHashContaCorrente(int codEmpresa);
        object[][] GetContaCorrentePagamento(int codEmpresa);
        object[][] GetContaCorrenteBoleto(int codSubRede);
        Task<PAGNET_CONTACORRENTE> GetContaCorrenteById(int id);
        Task<bool> IncluiContaCorrente(PAGNET_CONTACORRENTE conta);
        Task<bool> AtualizaContaCorrente(PAGNET_CONTACORRENTE conta);
        IEnumerable<PAGNET_CONTACORRENTE> GetAllContaCorrente(int CodEmpresa);
        Task<bool> Desativa(int id);
        bool bValidaContaCadastrada(string codBanco, string _agencia, string dgAgencia, string nroContaCorrente, string dgContaCorrente, int _codSubRede);
        bool bValidaContaCadastradaByBanco(string codBanco, int _codSubRede);
        decimal RetornaValorTED(int codContaCorrente);

        decimal RetornaSaldoAtual(int codContaCorrente);
        void InseriNovoSaldo(int codContaCorrente, int codEmpresa, decimal valor);
    }
}