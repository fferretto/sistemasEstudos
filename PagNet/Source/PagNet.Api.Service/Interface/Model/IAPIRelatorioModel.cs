using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface.Model
{
    public interface IAPIRelatorioModel
    {
        List<APIParametrosRelatorioModel> listaCampos { get; set; }

        int codRel { get; set; }
        string nmProc { get; set; }
        string nmTela { get; set; }
        string urlRelatorio { get; set; }
        string pathArquivo { get; set; }
        string ExecutaViaJob { get; set; }
        string statusGeracao { get; set; }
        bool PossuiOutroRelatorioSendoGerado { get; set; }
        int TipoRelatorio { get; set; }
        int codigoRelatorioSendoGerado { get; set; }
    }
}
