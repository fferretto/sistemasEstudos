using PagNet.BLD.ContaCorrente.Abstraction.Interface.Model;
using PagNet.BLD.ContaCorrente.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.BLD.ContaCorrente.Abstraction.Interface
{
    public interface IContaCorrenteApp
    {
        List<RetornoDDLVM> GetContaCorrente(int codigoContaCorrente);
        List<RetornoDDLVM> GetContaCorrentePagamento(int codigoContaCorrente);
        List<RetornoDDLVM> GetContaCorrenteBoleto(int codigoContaCorrente);

        List<RetornoDDLVM> GetBanco();
        BancoVM GetBancoByCodContaCorrente(int codContaCorrente);
        string BuscaBancoByID(string codBanco);

        bool ExisteArquivoRemessaBoletoCriado(int codigoContaCorrente);
        ResultadoTransmissaoArquivo GeraArquivoRemessaHomologacao(IDadosHomologarContaCorrenteVm model);
        ResultadoTransmissaoArquivo GeraBoletoPDFHomologacao(IDadosHomologarContaCorrenteVm model);

        List<ConsultaContaCorrenteVM> GetAllContaCorrente(int codigoContaCorrente);
        ContaCorrenteVm GetContaCorrenteById(int id, int codigoEmpresa);

        string Salvar(IContaCorrenteVm val);
        void Desativar(int id);

    }
}
