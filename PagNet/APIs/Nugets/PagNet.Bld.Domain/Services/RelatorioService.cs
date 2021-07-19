using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_RELATORIOService : ServiceBase<PAGNET_RELATORIO>, IPAGNET_RELATORIOService
    {
        private readonly IPAGNET_RELATORIORepository _rep;
        private readonly IPAGNET_RELATORIO_PARAM_UTILIZADORepository _paramRel;
        private readonly IPAGNET_RELATORIO_STATUSRepository _statusRel;
        private readonly IPAGNET_RELATORIO_RESULTADORepository _relResult;
        

        public PAGNET_RELATORIOService(IPAGNET_RELATORIORepository rep,
                                       IPAGNET_RELATORIO_STATUSRepository statusRel,
                                       IPAGNET_RELATORIO_PARAM_UTILIZADORepository paramRel,
                                       IPAGNET_RELATORIO_RESULTADORepository relResult
                                       )
            : base(rep)
        {
            _rep = rep;
            _statusRel = statusRel;
            _paramRel = paramRel;
            _relResult = relResult;
        }
        public List<PAGNET_RELATORIO> GetAllRelatorios()
        {
            return _rep.GetAll().ToList();
        }
        public PAGNET_RELATORIO GetRelatorioByID(int codRel)
        {
            var dados = _rep.Get(x => x.ID_REL == codRel, "PAGNET_PARAMETRO_REL").FirstOrDefault();

            return dados;
        }
        public PAGNET_RELATORIO_STATUS BusctaStatusRelatorio(int codigoRelatorio, int codigoUsuario)
        {
            var dados = _statusRel.Get(x => x.ID_REL == codigoRelatorio &&
                                            x.CODUSUARIO == codigoUsuario &&
                                            x.STATUS != "BAIXADO"
                                            , "PAGNET_RELATORIO_PARAM_UTILIZADO,PAGNET_RELATORIO_RESULTADO"
                                      ).FirstOrDefault();

            return dados;
        }

        public List<dynamic> GetDadosRelatorio(string _query, Dictionary<string, object> Parameters)
        {
            var MyList = _rep.ExecQueryDinamica(_query, Parameters).ToList();

            return MyList;
        }    
        public DataTable ListaDadosRelDataTable(string _query, Dictionary<string, object> Parameters)
        {
            var MyList = _rep.ListaDadosRelDataTable(_query, Parameters);

            return MyList;
        }

        public bool PossuiOutroRelatorioEmGeracao(int codigoRelatorio, int codigoUsuario)
        {
            var dados = _statusRel.Get(x => x.ID_REL != codigoRelatorio &&
                                            x.CODUSUARIO == codigoUsuario &&
                                            x.STATUS != "BAIXADO"
                                      ).FirstOrDefault();

            return (dados != null);
        }
        public void IncluiParametroUtilizado(PAGNET_RELATORIO_PARAM_UTILIZADO rel)
        {
            rel.COD_PARAM_UTILIZADO = _paramRel.BuscaProximoID();
            _paramRel.Add(rel);
        }
        public void IncluiStatusRel(PAGNET_RELATORIO_STATUS status)
        {
            _statusRel.Add(status);
        }
        public void AtualizaStatusRel(PAGNET_RELATORIO_STATUS status)
        {
            _statusRel.Update(status);
        }

        public PAGNET_RELATORIO_STATUS BuscaRelatorioPendenteDownload(int codigoUsuario)
        {
            var dados = _statusRel.Get(x => x.CODUSUARIO == codigoUsuario &&
                                            x.STATUS != "BAIXADO"
                                      ).FirstOrDefault();

            return dados;
        }
        public void RemoveRelatorioStatus(string codStatusRel)
        {
            var dados = _statusRel.GetById(codStatusRel);
            if(dados != null)
            {
                _statusRel.Remove(dados);
            }
        }
        public void RemoveParametrosUsadosRel(string codStatusRel)
        {
            var dados = _paramRel.Get(x => x.COD_STATUS_REL == codStatusRel).ToList();
            if (dados.Count > 0)
            {
                _paramRel.RemoveRanger(dados);
            }
        }
        public void RemoveRelatorioResult(string codStatusRel)
        {
            var dados = _relResult.Get(x => x.COD_STATUS_REL == codStatusRel).ToList();
            if (dados.Count > 0)
            {
                _relResult.RemoveRanger(dados);
            }
        }
    }
}
