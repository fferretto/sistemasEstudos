using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System.Collections.Generic;

namespace PagNet.Api.Service.Interface
{
    public interface IAPIGestaoDescontoFolhaAppClient
    {
        void InformaPGTOAtorizado(IAPIFiltroDescontoFolhaVM listaUsuario);
        void RenovaSaldoUsuario(IAPIFiltroDescontoFolhaVM usuario);

        DadosUsuarioVM BuscaDadosUsuarioByCPF(string CPF);
        DadosUsuarioVM BuscaDadosUsuarioByMatricula(string Matricula);
        IAPIUsuariosArquivoRetornoVm BuscaFechamentosNaoDescontados(IAPIFiltroDescontoFolhaVM filtro);
        IAPIUsuariosArquivoRetornoVm ConsolidaArquivoRetornoCliente(IAPIFiltroDescontoFolhaVM filtro);
        IDictionary<bool, string> ExecutaProcessoDescontoFolha(IAPIUsuariosArquivoRetornoVm vmApi);

        IAPIConfigParamLeituraArquivoVM BuscaConfiguracaoByCliente(IAPIFiltroDescontoFolhaVM filtro);
        IDictionary<bool, string> SalvarParamLeituraArquivo(IAPIConfigParamLeituraArquivoVM model);
        IDictionary<bool, string> DesativarParamLeituraArquivo(int codigoRegra);

        object[][] CarregaListaFaturasAbertas(int codigoCliente);
    }
}
