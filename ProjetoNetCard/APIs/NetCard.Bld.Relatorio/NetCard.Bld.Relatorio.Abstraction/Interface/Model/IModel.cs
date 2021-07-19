using NetCard.Bld.Relatorio.Abstraction.Model;
using System.Collections.Generic;

namespace NetCard.Bld.Relatorio.Abstraction.Interface.Model
{
    public interface IRelatorioModel
    {
        List<ParametrosRelatorioVm> listaCampos { get; set; }

        int idRelatorio { get; set; }
        int codigoCliente { get; set; }
        int Sistema { get; set; }
        int codigoRelatorioSendoGerado { get; set; }
        string nmProc { get; set; }
        string nmTela { get; set; }
        int TipoRelatorio { get; set; }
        string urlRelatorio { get; set; }
        string ExecutaViaJob { get; set; }
        string pathArquivo { get; set; }
        string statusGeracao { get; set; }
        bool PossuiOutroRelatorioSendoGerado { get; set; }
        string msgRetorno { get; set; }
    }
    public interface IFiltroRelModel
    {
        int codigoRelatorio { get; set; }
        int sistema { get; set; }
        int codigoCliente { get; set; }
        int codigoParametro { get; set; }
    }
    public interface IFiltroDDLRelModel
    {
        int codigoParametro { get; set; }
        string ParametroWhere { get; set; }
    }
    public interface IAPIRetornoDDLModel
    {
        string Valor { get; set; }
        string Descricao { get; set; }
        string Title { get; set; }
    }
}
