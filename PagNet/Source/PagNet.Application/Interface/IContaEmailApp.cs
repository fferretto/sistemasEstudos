using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface
{
    public interface IContaEmailApp
    {
        bool ValidaContaEmail(ContaEmailVM _email);
        Task<IDictionary<bool, string>> EnviaEmailBoleto(int codEmissaoBoleto, int cod_ope, int codUsuario, int codEmpresa);
        IDictionary<bool, string> EnviaEmailBoletoEmMassa(DadosEnvioEmailMassModel listaBol, int cod_ope, int codUsuario);
        Task<IDictionary<bool, string>> EnviarBoletoOutroEmail(int codEmissaoBoleto, string email, int cod_ope, int codUsuario, int codEmpresa);

        Task<IDictionary<bool, string>> EnviaEmailDetalhamentoFatura(int codEmissaoBoleto, string caminhoFatura, string emailDestino, int codUsuario);
        
        Task<List<ConsultaContaEmailVM>> GetAllContasEmail(int codEmpresa);
        ContaEmailVM GetContaEmailById(int? id, int codEmpresa);

        Task<IDictionary<string, string>> SalvaConta(ContaEmailVM _email);
        Task<IDictionary<string, string>> RemoveConta(int codContaEmail);
    }
}
