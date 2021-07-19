using NetCard.Bld.Relatorio.Abstraction.Interface.Model;
using NetCard.Bld.Relatorio.Abstraction.Model;
using System;
using System.Collections.Generic;

namespace NetCard.Bld.Relatorio.Web.Setup.Models
{
    public class ParametrosRel : IRelatorioModel
    {
        public List<ParametrosRelatorioVm> listaCampos { get; set; }
        public int codRel { get; set; }
        public string nmProc { get; set; }
        public string nmTela { get; set; }
        public string urlRelatorio { get; set; }
        public string pathArquivo { get; set; }
        public string statusGeracao { get; set; }
        public bool PossuiOutroRelatorioSendoGerado { get; set; }
        public string ExecutaViaJob { get; set; }
        public int TipoRelatorio { get; set; }
        public int codigoRelatorioSendoGerado { get; set; }
        public int idRelatorio { get; set; }
        public int codigoCliente { get; set; }
        public int Sistema { get; set; }
        public string msgRetorno { get; set; }
    }
    public class FiltroPesquisaRelModel : IFiltroRelModel
    {
        public int codigoRelatorio { get; set; }
        public int sistema { get; set; }
        public int codigoCliente { get; set; }
        public int codigoParametro { get; set; }
    }
    public class FiltroDDLRelModel : IFiltroDDLRelModel
    {
        public int codigoParametro { get; set; }
        public string nomeProc { get; set; }
        public string ParametroWhere { get; set; }
    }
}
