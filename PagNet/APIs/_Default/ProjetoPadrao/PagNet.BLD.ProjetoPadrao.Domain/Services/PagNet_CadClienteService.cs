using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class PagNet_CadClienteService : ServiceBase<PAGNET_CADCLIENTE>, IPagNet_CadClienteService
    {
        private readonly IPagNet_CadClienteRepository _rep;
        private readonly IPagNet_CadCliente_LogRepository _log;

        public PagNet_CadClienteService(IPagNet_CadClienteRepository rep,
                                        IPagNet_CadCliente_LogRepository log)
            : base(rep)
        {
            _rep = rep;
            _log = log;
        }

        public void AtualizaCliente(PAGNET_CADCLIENTE dados)
        {
            _rep.Update(dados);
        }

        public async Task<List<PAGNET_CADCLIENTE>> BuscaAllCliente()
        {
            var dados = _rep.Get(x => x.ATIVO == "S").ToList();
            return dados;
        }

        public async Task<List<PAGNET_CADCLIENTE>> BuscaAllClienteByCodEmpresa(int codEmpresa, string TipoCliente)
        {
            var dados = _rep.Get(x => x.ATIVO == "S" && 
                                 x.CODEMPRESA == codEmpresa &&
                                 ((TipoCliente == "") || x.TIPOCLIENTE == TipoCliente)).ToList();
            return dados;
        }

        public async Task<PAGNET_CADCLIENTE> BuscaClienteByCNPJ(string CNPJ)
        {
            var dados = _rep.Get(x => x.ATIVO == "S" && x.CPFCNPJ == CNPJ).FirstOrDefault();
            return dados;
        }

        public async Task<PAGNET_CADCLIENTE> BuscaClienteByCNPJeCodEmpresa(string CNPJ, int codempresa)
        {
            var dados = _rep.Get(x => x.ATIVO == "S" && x.CPFCNPJ == CNPJ && x.CODEMPRESA == codempresa).FirstOrDefault();
            return dados;
        }

        public async Task<PAGNET_CADCLIENTE> BuscaClienteByID(int codCli)
        {
            var dados = _rep.Get(x => x.ATIVO == "S" && x.CODCLIENTE == codCli).FirstOrDefault();
            return dados;
        }

        public async Task<PAGNET_CADCLIENTE> BuscaClienteByIDeCodEmpresa(int codCli, int codempresa)
        {
            var dados = _rep.Get(x => x.ATIVO == "S" && x.CODCLIENTE == codCli && x.CODEMPRESA == codempresa).FirstOrDefault();
            return dados;
        }

        public async Task<List<PAGNET_CADCLIENTE_LOG>> ConsultaLog(int codCli)
        {
            var dados = _log.Get(x => x.CODCLIENTE == codCli).ToList();

            return dados;
        }

        public void IncluiCliente(PAGNET_CADCLIENTE dados)
        {
            var id = _rep.GetMaxKey();
            dados.CODCLIENTE = id;

            _rep.Add(dados);
        }

        public void InsertLog(PAGNET_CADCLIENTE Cliente, int codUsuario, string Justificativa)
        {
            int id = _log.GetMaxKey();

            PAGNET_CADCLIENTE_LOG log = new PAGNET_CADCLIENTE_LOG();

            log.CODCLIENTE_LOG = id;
            log.CODCLIENTE = Cliente.CODCLIENTE;
            log.NMCLIENTE = Cliente.NMCLIENTE;
            log.CPFCNPJ = Cliente.CPFCNPJ;
            log.EMAIL = Cliente.EMAIL;
            log.CODEMPRESA = Cliente.CODEMPRESA;
            log.CEP = Cliente.CEP;
            log.LOGRADOURO = Cliente.LOGRADOURO;
            log.NROLOGRADOURO = Cliente.NROLOGRADOURO;
            log.COMPLEMENTO = Cliente.COMPLEMENTO;
            log.BAIRRO = Cliente.BAIRRO;
            log.CIDADE = Cliente.CIDADE;
            log.UF = Cliente.UF;
            log.COBRANCADIFERENCIADA = Cliente.COBRANCADIFERENCIADA;
            log.COBRAJUROS = Cliente.COBRAJUROS;
            log.VLJUROSDIAATRASO = Cliente.VLJUROSDIAATRASO;
            log.PERCJUROS = Cliente.PERCJUROS;
            log.COBRAMULTA = Cliente.COBRAMULTA;
            log.VLMULTADIAATRASO = Cliente.VLMULTADIAATRASO;
            log.PERCMULTA = Cliente.PERCMULTA;
            log.CODPRIMEIRAINSTCOBRA = Cliente.CODPRIMEIRAINSTCOBRA;
            log.CODSEGUNDAINSTCOBRA = Cliente.CODSEGUNDAINSTCOBRA;
            log.TAXAEMISSAOBOLETO = Cliente.TAXAEMISSAOBOLETO;
            log.AGRUPARFATURAMENTOSDIA = Cliente.AGRUPARFATURAMENTOSDIA;
            log.CODFORMAFATURAMENTO = Cliente.CODFORMAFATURAMENTO;
            log.ATIVO = Cliente.ATIVO;
            log.CODUSUARIO = codUsuario;
            log.DATINCLOG = DateTime.Now;
            log.DESCLOG = Justificativa;
            log.TIPOCLIENTE = Cliente.TIPOCLIENTE;


            _log.Add(log);
        }
    }
}
