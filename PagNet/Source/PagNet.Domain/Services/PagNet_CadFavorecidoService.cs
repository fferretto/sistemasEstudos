using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Domain.Services
{
    public class PagNet_CadFavorecidoService : ServiceBase<PAGNET_CADFAVORECIDO>, IPagNet_CadFavorecidoService
    {
        private readonly IPagNet_CadFavorecidoRepository _rep;
        private readonly IPagNet_CadFavorecido_LogRepository _log;

        public PagNet_CadFavorecidoService(IPagNet_CadFavorecidoRepository rep,
                                          IPagNet_CadFavorecido_LogRepository log)
            : base(rep)
        {
            _rep = rep;
            _log = log;
        }

        public void AtualizaFavorito(PAGNET_CADFAVORECIDO dados)
        {
            _rep.Update(dados);
        }

        public async Task<List<PAGNET_CADFAVORECIDO>> BuscaAllByCentralizadora()
        {
            var dados = _rep.Get(x => x.ATIVO == "S" && (x.CODCEN != null && x.CODCEN > 0)).ToList();
            return dados;
        }

        public async Task<List<PAGNET_CADFAVORECIDO>> BuscaAllFavorecidosFornecedor(int codEmpresa)
        {
            var dados = _rep.Get(x => x.ATIVO == "S" && 
                                (x.CODCEN == null || x.CODCEN == 0) &&
                                (codEmpresa == 0 || x.CODEMPRESA == codEmpresa || x.CODEMPRESA == null)).ToList();
            return dados;
        }
        
        public async Task<List<PAGNET_CADFAVORECIDO>> BuscaAllFavorecidosPagamento(int codEmpresa)
        {
            return _rep.Get(x =>x.ATIVO == "S" &&
                                (x.CODEMPRESA == null || x.CODEMPRESA == codEmpresa)            
                                ).ToList();
        }

        public async Task<List<PAGNET_CADFAVORECIDO>> BuscaFavorecidosByBanco(string CodBanco)
        {
            return _rep.Get(x => x.BANCO == CodBanco && x.ATIVO == "S").ToList();
        }

        public async Task<PAGNET_CADFAVORECIDO> BuscaFavorecidosByCNPJ(string CNPJ)
        {
            return _rep.Get(x => x.CPFCNPJ == CNPJ && x.ATIVO == "S").FirstOrDefault();
        }

        public async Task<PAGNET_CADFAVORECIDO> BuscaFavorecidosByCodCen(int CodCen)
        {
            return _rep.Get(x => x.CODCEN == CodCen && x.ATIVO == "S").FirstOrDefault();
        }

        public async Task<PAGNET_CADFAVORECIDO> BuscaFavorecidosByID(int CodFavorito)
        {
            return _rep.Get(x => x.CODFAVORECIDO == CodFavorito && x.ATIVO == "S").FirstOrDefault();
        }

        public async Task<List<PAGNET_CADFAVORECIDO_LOG>> ConsultaLog(int codFav)
        {
            var dados = _log.Get(x => x.CODFAVORECIDO == codFav).ToList();

            return dados;
        }

        public void IncluiFavorito(PAGNET_CADFAVORECIDO dados)
        {
            var id = _rep.GetMaxKey();
            dados.CODFAVORECIDO = id;

            _rep.Add(dados);
        }

        public void InsertLog(PAGNET_CADFAVORECIDO favorito, int codUsuario, string Justificativa)
        {
            try
            {
                var id = _log.GetMaxKey();

                PAGNET_CADFAVORECIDO_LOG log = new PAGNET_CADFAVORECIDO_LOG();
                log.CODFAVORECIDO_LOG = id;
                log.CODFAVORECIDO = favorito.CODFAVORECIDO;
                log.NMFAVORECIDO = favorito.NMFAVORECIDO;
                log.CPFCNPJ = favorito.CPFCNPJ;
                log.CODCEN = favorito.CODCEN;
                log.BANCO = favorito.BANCO;
                log.AGENCIA = favorito.AGENCIA;
                log.DVAGENCIA = favorito.DVAGENCIA;
                log.OPE = favorito.OPE;
                log.CONTACORRENTE = favorito.CONTACORRENTE;
                log.DVCONTACORRENTE = favorito.DVCONTACORRENTE;
                log.CEP = favorito.CEP;
                log.LOGRADOURO = favorito.LOGRADOURO;
                log.NROLOGRADOURO = favorito.NROLOGRADOURO;
                log.COMPLEMENTO = favorito.COMPLEMENTO;
                log.BAIRRO = favorito.BAIRRO;
                log.CIDADE = favorito.CIDADE;
                log.UF = favorito.UF;
                log.ATIVO = favorito.ATIVO;
                log.CODUSUARIO = codUsuario;
                log.DATINCLOG = DateTime.Now;
                log.DESCLOG = Justificativa;


                _log.Add(log);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
