using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Interface.Areas.Relatorios.Models
{
    public class RelatoriosModel : IAPIRelatorioModel
    {
        public RelatoriosModel()
        {
            listaCampos = new List<APIParametrosRelatorioModel>();
        }
        //public IList<ParametrosRelatorioModel> listaCampos { get; set; }
        public int codRel { get; set; }
        public string nmProc { get; set; }
        public string nmTela { get; set; }
        public string urlRelatorio { get; set; }
        public string pathArquivo { get; set; }
        public string ExecutaViaJob { get; set; }
        public string statusGeracao { get; set; }
        public int TipoRelatorio { get; set; }
        public bool PossuiOutroRelatorioSendoGerado { get; set; }
        public int codigoRelatorioSendoGerado { get; set; }
        public List<APIParametrosRelatorioModel> listaCampos { get; set; }

        internal static RelatoriosModel ToView(IAPIRelatorioModel item)
        {
            return new RelatoriosModel
            {
                codRel = item.codRel,
                nmProc = item.nmProc,
                nmTela = item.nmTela,
                urlRelatorio = item.urlRelatorio,
                pathArquivo = item.pathArquivo,
                ExecutaViaJob = item.ExecutaViaJob,
                statusGeracao = item.statusGeracao,
                TipoRelatorio = item.TipoRelatorio,
                PossuiOutroRelatorioSendoGerado = item.PossuiOutroRelatorioSendoGerado,
                codigoRelatorioSendoGerado = item.codigoRelatorioSendoGerado,
                listaCampos = ToList(item.listaCampos)
            };
        }

        internal static List<APIParametrosRelatorioModel> ToList<T>(ICollection<T> collection) where T : APIParametrosRelatorioModel
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static APIParametrosRelatorioModel ToListView(APIParametrosRelatorioModel item)
        {
            return new APIParametrosRelatorioModel
            {
                ID_PAR = item.ID_PAR,
                ID_REL = item.ID_REL,
                DESPAR = item.DESPAR,
                NOMPAR = item.NOMPAR,
                LABEL = item.LABEL,
                TIPO = item.TIPO,
                TAMANHO = item.TAMANHO,
                _DEFAULT = item._DEFAULT,
                REQUERIDO = item.REQUERIDO,
                ORDEM_TELA = item.ORDEM_TELA,
                ORDEM_PROC = item.ORDEM_PROC,
                NOM_FUNCTION = item.NOM_FUNCTION,
                VALCAMPO = item.VALCAMPO,
                MASCARA = item.MASCARA,
                TEXTOAJUDA = item.TEXTOAJUDA,
            };
        }


    }
    public class ModRelPDFVm
    {
        public bool EmGeracao { get; set; }
        public List<ModRel> Cabecalho { get; set; }
        public List<ModRel> Conteudo { get; set; }
        public string nmRelatorio { get; set; }
        public int TipoRel { get; set; }
        public string caminhoArquivo { get; set; }
    }
    public class ModRel
    {
        public string LINHAIMP { get; set; }
        public int TIP { get; set; }
    }
}