namespace PagNet.Application.Interface
{
    public interface IDiversosApp
    {
        object[][] GetOperadoras();
        object[][] GetSubRede();
        object[][] GetEmpresa(int codEmpresa);
        object[][] GetTiposOcorrenciasBoleto();
        object[][] GetInstrucaoCobranca();
        object[][] StatusTransacao();
        object[][] GetFormasLiquidacao();
        object[][] BuscaSubRedeByID(int id);
        object[][] DDLPlanoContas(int CodEmpresa);
        object[][] DDLPlanoContasRecebimento(int CodEmpresa);
        object[][] DDLPlanoContasPagamento(int CodEmpresa);
        string GetCaminhoArquivoPadrao(int codOpe, int codEmpresa);
        string GetCaminhoArquivoPadrao(int codOpe);
        //string GetnmSubRedeByID(int codSubRede);
        string GetnmEmpresaByID(int codEmpresa);
    }
}
