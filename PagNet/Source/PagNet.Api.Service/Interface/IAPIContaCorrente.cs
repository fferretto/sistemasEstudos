using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Api.Service.Interface
{
    public interface IAPIContaCorrente
    {
        object[][] GetContaCorrente(int codSubRede);
        object[][] GetContaCorrentePagamento(int codSubRede);
        object[][] GetContaCorrenteBoleto(int codSubRede);

        object[][] GetBanco();
        IAPIBancoModel GetBancoByCodContaCorrente(int codContaCorrente);
        string BuscaBancoByID(string codBanco);

        bool ExisteArquivoRemessaBoletoCriado(int codigoContaCorrente);
        IAPIResultadoTransmissaoArquivoModel GeraArquivoRemessaHomologacao(IAPIDadosHomologarContaCorrenteModel model);
        IAPIResultadoTransmissaoArquivoModel GeraBoletoPDFHomologacao(IAPIDadosHomologarContaCorrenteModel model);

        List<APIConsultaContaCorrenteModel> GetAllContaCorrente(int CodSubRede);
        IAPIContaCorrenteModel GetContaCorrenteById(int? id, int codSubRede);
        IDictionary<string, string> Salvar(IAPIContaCorrenteModel val);
        void Desativar(int id);
    }
}
