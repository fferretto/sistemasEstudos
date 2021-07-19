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
    public class PAGNET_CADCLIENTEService : ServiceBase<PAGNET_CADCLIENTE>, IPAGNET_CADCLIENTEService
    {
        private readonly IPAGNET_CADCLIENTERepository _rep;
        private readonly IPAGNET_CADCLIENTE_LOGRepository _log;

        public PAGNET_CADCLIENTEService(IPAGNET_CADCLIENTERepository rep,
                                        IPAGNET_CADCLIENTE_LOGRepository log)
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
            var dados = _rep.Get(x => x.CPFCNPJ == CNPJ).FirstOrDefault();
            return dados;
        }

        public async Task<PAGNET_CADCLIENTE> BuscaClienteByCNPJeCodEmpresa(string CNPJ, int codempresa)
        {
            var dados = _rep.Get(x => x.ATIVO == "S" && x.CPFCNPJ == CNPJ && x.CODEMPRESA == codempresa).FirstOrDefault();
            return dados;
        }

        public async Task<PAGNET_CADCLIENTE> BuscaClienteByID(int codCli)
        {
            var dados = _rep.Get(x => x.CODCLIENTE == codCli).FirstOrDefault();
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
            var id = _rep.BuscaProximoID();
            dados.CODCLIENTE = id;

            _rep.Add(dados);
        }

        public void InsertLog(PAGNET_CADCLIENTE Cliente, int codUsuario, string Justificativa)
        {
            int id = _log.BuscaProximoID();

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
