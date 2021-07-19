using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;

namespace PagNet.Domain.Services
{
    public class PagNet_ContaCorrenteService : ServiceBase<PAGNET_CONTACORRENTE>, IPagNet_ContaCorrenteService
    {
        private readonly IPagNet_ContaCorrenteRepository _rep;
        private readonly IPagNet_ContaCorrente_SaldoRepository _saldo;

        public PagNet_ContaCorrenteService(IPagNet_ContaCorrenteRepository rep,
                                            IPagNet_ContaCorrente_SaldoRepository saldo)
            : base(rep)
        {
            _rep = rep;
            _saldo = saldo;
        }

        public async Task<bool> AtualizaContaCorrente(PAGNET_CONTACORRENTE conta)
        {
            _rep.Update(conta);
            return true;
        }

        public async Task<bool> Desativa(int id)
        {
            var conta = GetContaCorrenteById(id).Result;

            try
            {
                conta.ATIVO = "N";
                _rep.Update(conta);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool bValidaContaCadastrada(string codBanco, string _agencia, string dgAgencia, string nroContaCorrente, string dgContaCorrente, int CodEmpresa)
        {
            var bExisteConta = false;

            var conta = _rep.Get(x => x.CODBANCO == codBanco &&
                                      x.AGENCIA == _agencia &&
                                      x.DIGITOAGENCIA == dgAgencia &&
                                      x.NROCONTACORRENTE == nroContaCorrente &&
                                      x.DIGITOCC == dgContaCorrente &&
                                      x.CODEMPRESA == CodEmpresa
                                 ).FirstOrDefault();

            if (conta != null)
            {
                bExisteConta = true;
            }

            return bExisteConta;
            
        }
        public bool bValidaContaCadastradaByBanco(string codBanco, int CodEmpresa)
        {
            var bExisteConta = false;

            var conta = _rep.Get(x => x.CODBANCO == codBanco &&
                                      x.CODEMPRESA == CodEmpresa
                                 ).FirstOrDefault();

            if (conta != null)
            {
                bExisteConta = true;
            }

            return bExisteConta;

        }
        public IEnumerable<PAGNET_CONTACORRENTE> GetAllContaCorrente(int CodEmpresa)
        {
            return _rep.Get(x => x.ATIVO == "S" &&
                            (0 == CodEmpresa || x.CODEMPRESA == CodEmpresa)
                 ).OrderBy(x => x.NMCONTACORRENTE);
        }

        public async Task<PAGNET_CONTACORRENTE> GetContaCorrenteById(int id)
        {
            return _rep.Get(x => x.CODCONTACORRENTE == id, "PAGNET_CADEMPRESA").FirstOrDefault();
        }

        public object[][] GetHashContaCorrente(int CodEmpresa)
        {
            return _rep.Get(x => x.ATIVO == "S" &&
                            (0 == CodEmpresa || x.CODEMPRESA == CodEmpresa))
                .Select(x => new object[] { x.CODCONTACORRENTE, "Banco:" + x.CODBANCO + " Agencia:" + x.AGENCIA + " Conta:" + x.NROCONTACORRENTE + "-" + x.DIGITOCC }).ToArray();
        }

        public async Task<bool> IncluiContaCorrente(PAGNET_CONTACORRENTE conta)
        {
            var codContaCorrente = _rep.GetMaxKey();
            conta.CODCONTACORRENTE = codContaCorrente;
            _rep.Add(conta);

            return true;
        }

        public object[][] GetContaCorrentePagamento(int CodEmpresa)
        {
            return _rep.Get(x => x.ATIVO == "S" &&
                            (0 == CodEmpresa || x.CODEMPRESA == CodEmpresa) &&
                            (!string.IsNullOrWhiteSpace(x.CODCONVENIOPAG) || x.CODBANCO =="341")
                            )                            
                .Select(x => new object[] { x.CODCONTACORRENTE, "Banco:" + x.CODBANCO + " Agencia:" + x.AGENCIA + " Conta:" + x.NROCONTACORRENTE + "-" + x.DIGITOCC }).ToArray();
        }
        public object[][] GetContaCorrenteBoleto(int CodEmpresa)
        {
            return _rep.Get(x => x.ATIVO == "S" &&
                            (0 == CodEmpresa || x.CODEMPRESA == CodEmpresa) 
                            )
                .Select(x => new object[] { x.CODCONTACORRENTE, "Banco:" + x.CODBANCO + " Agencia:" + x.AGENCIA + " Conta:" + x.NROCONTACORRENTE + "-" + x.DIGITOCC }).ToArray();
        }

        public decimal RetornaValorTED(int codContaCorrente)
        {
            var DadosConta = _rep.Get(x => x.CODCONTACORRENTE == codContaCorrente).FirstOrDefault();

            var valTED = DadosConta.VALTED ?? 0;

            return valTED;
        }

        public decimal RetornaSaldoAtual(int codContaCorrente)
        {
            var dados = _saldo.Get(x => x.CODCONTACORRENTE == codContaCorrente)
                                .OrderByDescending(y => y.CODSALDO)
                                .FirstOrDefault();

            if (dados == null)
            {
                return 0;
            }

            return dados.SALDO;
        }

        public void InseriNovoSaldo(int codContaCorrente, int codEmpresa, decimal valor)
        {
            PAGNET_CONTACORRENTE_SALDO SALDO = new PAGNET_CONTACORRENTE_SALDO();
            SALDO.CODSALDO = _saldo.GetMaxKey();
            SALDO.CODCONTACORRENTE = codContaCorrente;
            SALDO.CODEMPRESA = codEmpresa;
            SALDO.DATLANCAMENTO = DateTime.Now;
            SALDO.SALDO = valor;

            _saldo.Add(SALDO);
        }
    }
}
