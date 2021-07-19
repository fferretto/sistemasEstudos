using PagNet.BLD.DescontoFolha.Abstraction.Interface.Model;
using PagNet.BLD.DescontoFolha.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.BLD.DescontoFolha.Abstraction.Interface
{
    public interface IDescontoFolhaApp
    {
        DadosUsuarioVM BuscaDadosUsuarioByCPF(string CPF);
        DadosUsuarioVM BuscaDadosUsuarioByMatricula(string Matricula);
        UsuariosArquivoRetornoVm BuscaFechamentosNaoDescontados(IFiltroDescontoFolhaVM filtro);
        UsuariosArquivoRetornoVm ConsolidaArquivoRetornoCliente(IFiltroDescontoFolhaVM filtro);
        void ValidaFaturamentoViaArquivo(IFiltroDescontoFolhaVM filtro);
        ConfigParamLeituraArquivoVM BuscaConfiguracaoByCliente(IFiltroDescontoFolhaVM filtro);
        void SalvarParamLeituraArquivo(IConfigParamLeituraArquivoVM model);

        void ExecutaProcessoDescontoFolha(IUsuariosArquivoRetornoVm model);
        List<APIRetornoDDLModel> CarregaListaFaturasAbertas(int codigoCliente);

    }
}
