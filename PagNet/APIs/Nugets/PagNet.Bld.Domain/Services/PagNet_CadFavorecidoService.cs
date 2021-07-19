using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_CADFAVORECIDOService : ServiceBase<PAGNET_CADFAVORECIDO>, IPAGNET_CADFAVORECIDOService
    {
        private readonly IPAGNET_CADFAVORECIDORepository _rep;
        private readonly IPAGNET_CADFAVORECIDO_LOGRepository _log;
        private readonly IPAGNET_CADFAVORECIDO_CONFIGRepository _config;


        public PAGNET_CADFAVORECIDOService(IPAGNET_CADFAVORECIDORepository rep,
                                           IPAGNET_CADFAVORECIDO_LOGRepository log,
                                           IPAGNET_CADFAVORECIDO_CONFIGRepository config)
            : base(rep)
        {
            _rep = rep;
            _log = log;
            _config = config;
        }

        public void AtualizaFavorito(PAGNET_CADFAVORECIDO dados)
        {
            _rep.Update(dados);
        }
        public void AtualizaFavoritoConfig(PAGNET_CADFAVORECIDO_CONFIG dados)
        {
            _config.Update(dados);
        }
        public void InseriFavoritoConfig(PAGNET_CADFAVORECIDO_CONFIG dados)
        {
            dados.CODFAVORECIDOCONFIG = _config.BuscaProximoID();
            _config.Add(dados);
        }
        public void DeletaFavoritoConfig(int codigoFavorecido, int codigoEmpresa)
        {
            var dados = BuscaConfigFavorecido(codigoFavorecido, codigoEmpresa);
            _config.Remove(dados);
        }
        public PAGNET_CADFAVORECIDO_CONFIG BuscaConfigFavorecido(int codigoFavorecido, int codigoEmpresa)
        {
            var dados = _config.Get(x => x.CODFAVORECIDO == codigoFavorecido && x.CODEMPRESA == codigoEmpresa).FirstOrDefault();
            return dados;
        }

        public List<PAGNET_CADFAVORECIDO> BuscaAllByCentralizadora()
        {
            var dados = _rep.Get(x => x.ATIVO == "S" && (x.CODCEN != null && x.CODCEN > 0)).ToList();
            return dados;
        }
        public List<PAGNET_CADFAVORECIDO> BuscaAllFavorecidosFornecedor(int codEmpresa)
        {
            var dados = _rep.Get(x => x.ATIVO == "S" &&
                                (x.CODCEN == null || x.CODCEN == 0) &&
                                (codEmpresa == 0 || x.CODEMPRESA == codEmpresa || x.CODEMPRESA == null)).ToList();
            return dados;
        }        
        public List<PAGNET_CADFAVORECIDO> BuscaAllFavorecidosPagamento(int codEmpresa)

        {
            return _rep.Get(x => x.ATIVO == "S" &&
                                (x.CODEMPRESA == null || x.CODEMPRESA == codEmpresa)
                                ).ToList();
        }

        public List<PAGNET_CADFAVORECIDO> BuscaFavorecidosByBanco(string CodBanco)
        {
            return _rep.Get(x => x.BANCO == CodBanco && x.ATIVO == "S").ToList();
        }

        public PAGNET_CADFAVORECIDO BuscaFavorecidosByCNPJ(string CNPJ)
        {
            return _rep.Get(x => x.CPFCNPJ == CNPJ && x.ATIVO == "S", "PAGNET_CADFAVORECIDO_CONFIG").FirstOrDefault();
        }

        public PAGNET_CADFAVORECIDO BuscaFavorecidosByCodCen(int CodCen)
        {
            return _rep.Get(x => x.CODCEN == CodCen && x.ATIVO == "S", "PAGNET_CADFAVORECIDO_CONFIG").FirstOrDefault();
        }

        public PAGNET_CADFAVORECIDO BuscaFavorecidosByID(int CodFavorito)
        {
            return _rep.Get(x => x.CODFAVORECIDO == CodFavorito && x.ATIVO == "S", "PAGNET_CADFAVORECIDO_CONFIG").FirstOrDefault();
        }

        public List<PAGNET_CADFAVORECIDO_LOG> ConsultaLog(int codFav)
        {
            var dados = _log.Get(x => x.CODFAVORECIDO == codFav, "PAGNET_USUARIO").ToList();

            return dados;
        }

        public void IncluiFavorito(PAGNET_CADFAVORECIDO dados)
        {
            var id = _rep.BuscaProximoID();
            dados.CODFAVORECIDO = id;

            _rep.Add(dados);
        }

        public void InsertLog(PAGNET_CADFAVORECIDO favorecido, int codUsuario, string Justificativa)
        {
            try
            {
                var id = _log.BuscaProximoID();

                PAGNET_CADFAVORECIDO_LOG log = new PAGNET_CADFAVORECIDO_LOG();
                log.CODFAVORECIDO_LOG = id;
                log.CODFAVORECIDO = favorecido.CODFAVORECIDO;
                log.NMFAVORECIDO = favorecido.NMFAVORECIDO;
                log.CPFCNPJ = favorecido.CPFCNPJ;
                log.CODCEN = favorecido.CODCEN;
                log.BANCO = favorecido.BANCO;
                log.AGENCIA = favorecido.AGENCIA;
                log.DVAGENCIA = favorecido.DVAGENCIA;
                log.OPE = favorecido.OPE;
                log.CONTACORRENTE = favorecido.CONTACORRENTE;
                log.DVCONTACORRENTE = favorecido.DVCONTACORRENTE;
                log.CEP = favorecido.CEP;
                log.LOGRADOURO = favorecido.LOGRADOURO;
                log.NROLOGRADOURO = favorecido.NROLOGRADOURO;
                log.COMPLEMENTO = favorecido.COMPLEMENTO;
                log.BAIRRO = favorecido.BAIRRO;
                log.CIDADE = favorecido.CIDADE;
                log.UF = favorecido.UF;
                log.ATIVO = favorecido.ATIVO;
                log.CODUSUARIO = codUsuario;
                log.DATINCLOG = DateTime.Now;
                log.DESCLOG = Justificativa;
                log.CODEMPRESA = favorecido.CODEMPRESA;
                log.DTINCLUSAO = favorecido.DTINCLUSAO;
                log.DTALTERACAO = favorecido.DTALTERACAO;

                _log.Add(log);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
