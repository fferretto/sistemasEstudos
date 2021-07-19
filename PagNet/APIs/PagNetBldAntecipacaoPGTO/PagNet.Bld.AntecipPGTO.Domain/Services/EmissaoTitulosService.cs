using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Repository;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Services;
using PagNet.Bld.AntecipPGTO.Domain.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagNet.Bld.AntecipPGTO.Domain.Services
{
    public class EmissaoTitulosService : ServiceBase<PAGNET_EMISSAO_TITULOS>, IEmissaoTitulosService
    {
        private static object _syncIncluiTitulo = new object();
        private static object _syncIncluiTituloLog = new object();

        private readonly IPagNet_Emissao_TitulosRepository _rep;
        private readonly IPagNet_Emissao_Titulos_LogRepository _log;


        public EmissaoTitulosService(IPagNet_Emissao_TitulosRepository rep,
                                       IPagNet_Emissao_Titulos_LogRepository log)
            : base(rep)
        {
            _rep = rep;
            _log = log;
        }

        public void AtualizaTitulo(PAGNET_EMISSAO_TITULOS Titulo)
        {
            _rep.Update(Titulo);
        }
        
        public PAGNET_EMISSAO_TITULOS BuscaTituloByID(int CodTitulo)
        {
            return _rep.Get(x => x.CODTITULO == CodTitulo, "PAGNET_CADFAVORECIDO").FirstOrDefault();
        }

        public List<PAGNET_EMISSAO_TITULOS> BustaTitulosAVencer(int codEmpresa, int codigoFavorecido, DateTime APartirDe)
        {
            var dataAtual = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            return _rep.Get(x => x.CODEMPRESA == codEmpresa && 
                                 x.STATUS == "EM_ABERTO" && 
                                 x.CODFAVORECIDO == codigoFavorecido &&
                                 x.DATREALPGTO > APartirDe &&
                                 x.DATREALPGTO >= dataAtual, "PAGNET_CADFAVORECIDO").ToList();
        }

        public void IncluiLog(PAGNET_EMISSAO_TITULOS Titulo, int codUsuario, string Justificativa)
        {
            int id = _log.GetMaxKey();

            PAGNET_EMISSAO_TITULOS_LOG log = new PAGNET_EMISSAO_TITULOS_LOG();
            log.CODTITULO_LOG = id;
            log.CODTITULO = Titulo.CODTITULO;
            log.CODTITULOPAI = Titulo.CODTITULOPAI;
            log.STATUS = Titulo.STATUS;
            log.CODEMPRESA = Titulo.CODEMPRESA;
            log.CODFAVORECIDO = Titulo.CODFAVORECIDO;
            log.DATEMISSAO = Titulo.DATEMISSAO;
            log.DATPGTO = Titulo.DATPGTO;
            log.DATREALPGTO = Titulo.DATREALPGTO;
            log.VALBRUTO = Titulo.VALBRUTO;
            log.VALLIQ = Titulo.VALLIQ;
            log.TIPOTITULO = Titulo.TIPOTITULO;
            log.ORIGEM = Titulo.ORIGEM;
            log.SISTEMA = Titulo.SISTEMA;
            log.LINHADIGITAVEL = Titulo.LINHADIGITAVEL;
            log.CODBORDERO = Titulo.CODBORDERO;
            log.VALTOTAL = Titulo.VALTOTAL;
            log.SEUNUMERO = Titulo.SEUNUMERO;
            log.CODUSUARIO = codUsuario;
            log.DATINCLOG = DateTime.Now;
            log.DESCLOG = Justificativa;
            log.CODCONTACORRENTE = Titulo.CODCONTACORRENTE;

            _log.Add(log);
        }
    }
}
