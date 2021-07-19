using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.BLD.Cliente.Abstraction.Interface;
using PagNet.BLD.Cliente.Abstraction.Interface.Model;
using PagNet.BLD.Cliente.Abstraction.Model;
using PagNet.BLD.Cliente.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using Telenet.BusinessLogicModel;

namespace PagNet.BLD.Cliente.Application
{
    public class ClienteApp : Service<IContextoApp>, IClienteApp
    {

        private readonly IParametrosApp _user;
        private readonly IPAGNET_CADCLIENTEService _cliente;
        private readonly IPAGNET_INSTRUCAOCOBRANCAService _InstrucaoCobranca;
        private readonly IPAGNET_FORMAS_FATURAMENTOService _FormaFaturamento;
        private readonly IPAGNET_CADEMPRESAService _empresa;


        public ClienteApp(IContextoApp contexto,
                          IPAGNET_CADCLIENTEService cliente,
                          IPAGNET_CADEMPRESAService empresa,
                          IPAGNET_INSTRUCAOCOBRANCAService InstrucaoCobranca,
                          IPAGNET_FORMAS_FATURAMENTOService FormaFaturamento,
                          IParametrosApp user)
            : base(contexto)
        {
            _user = user;
            _cliente = cliente;
            _empresa = empresa;
            _InstrucaoCobranca = InstrucaoCobranca;
            _FormaFaturamento = FormaFaturamento;
        }

        public List<ClienteVm> ConsultaTodosCliente(IFiltroCliente filtro)
        {
            var ListaCliente = _cliente.BuscaAllClienteByCodEmpresa(filtro.codigoEmpresa, filtro.TipoCliente).Result;

            List<ClienteVm> ListaClienteRetono = new List<ClienteVm>();
            ClienteVm cliente = new ClienteVm();
            foreach (var _cad in ListaCliente)
            {
                cliente = new ClienteVm();
                cliente.codigoCliente = _cad.CODCLIENTE;
                cliente.nomeCliente = _cad.NMCLIENTE;
                cliente.cpfCnpj = Helper.FormataCPFCnPj(_cad.CPFCNPJ);
                cliente.email = _cad.EMAIL;
                cliente.codigoEmpresa = _cad.CODEMPRESA.ToString();
                cliente.cep = _cad.CEP;
                cliente.logradouro = _cad.LOGRADOURO;
                cliente.numeroLogradouro = _cad.NROLOGRADOURO;
                cliente.complemento = _cad.COMPLEMENTO;
                cliente.bairro = _cad.BAIRRO;
                cliente.cidade = _cad.CIDADE;
                cliente.UF = _cad.UF;
                cliente.regraPropria = (_cad.COBRANCADIFERENCIADA == "S") ? "SIM" : "NÃO";
                cliente.cobrancaDiferenciada = (_cad.COBRANCADIFERENCIADA == "S");
                cliente.cobraJuros = (_cad.COBRAJUROS == "S");
                cliente.valorJurosDiaAtraso = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLJUROSDIAATRASO ?? 0).Replace("R$", "");
                cliente.percentualJuros = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCJUROS ?? 0).Replace("R$", "");
                cliente.cobraMulta = (_cad.COBRAMULTA == "S");
                cliente.valorMultaDiaAtraso = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLMULTADIAATRASO ?? 0).Replace("R$", "");
                cliente.percentualMulta = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCMULTA ?? 0).Replace("R$", "");
                cliente.codigoPrimeiraIntrucaoCobranca = Convert.ToString(_cad.CODPRIMEIRAINSTCOBRA);
                cliente.codigoSegundaIntrucaoCobranca = Convert.ToString(_cad.CODSEGUNDAINSTCOBRA);
                cliente.taxaEmissaoBoleto = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.TAXAEMISSAOBOLETO ?? 0).Replace("R$", "");
                ListaClienteRetono.Add(cliente);
            }


            return ListaClienteRetono;
        }

        public void DesativaCliente(IFiltroCliente filtro)
        {
            var cliente = _cliente.BuscaClienteByID(filtro.codigoCliente).Result;
            cliente.ATIVO = "N";

            _cliente.AtualizaCliente(cliente);
            _cliente.InsertLog(cliente, _user.cod_usu, filtro.JustificativaAcao);


        }

        public ClienteVm RetornaDadosClienteByCPFCNPJ(IFiltroCliente filtro)
        {
            ClienteVm cliente = new ClienteVm();
            filtro.CpfCnpj = Helper.RemoveCaracteres(filtro.CpfCnpj);
            var _cad = _cliente.BuscaClienteByCNPJ(filtro.CpfCnpj).Result;

            if (_cad != null)
            {
                cliente.codigoCliente = _cad.CODCLIENTE;
                cliente.nomeCliente = _cad.NMCLIENTE;
                cliente.cpfCnpj = Helper.FormataCPFCnPj(_cad.CPFCNPJ);
                cliente.email = _cad.EMAIL;
                cliente.codigoEmpresa = _cad.CODEMPRESA.ToString();
                cliente.cep = _cad.CEP;
                cliente.logradouro = _cad.LOGRADOURO;
                cliente.numeroLogradouro = _cad.NROLOGRADOURO;
                cliente.complemento = _cad.COMPLEMENTO;
                cliente.bairro = _cad.BAIRRO;
                cliente.cidade = _cad.CIDADE;
                cliente.UF = _cad.UF;
                cliente.regraPropria = (_cad.COBRANCADIFERENCIADA == "S") ? "SIM" : "NÃO";
                cliente.cobrancaDiferenciada = (_cad.COBRANCADIFERENCIADA == "S");
                cliente.cobraJuros = (_cad.COBRAJUROS == "S");
                cliente.valorJurosDiaAtraso = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLJUROSDIAATRASO ?? 0).Replace("R$", "");
                cliente.percentualJuros = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCJUROS ?? 0).Replace("R$", "");
                cliente.cobraMulta = (_cad.COBRAMULTA == "S");
                cliente.valorMultaDiaAtraso = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLMULTADIAATRASO ?? 0).Replace("R$", "");
                cliente.percentualMulta = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCMULTA ?? 0).Replace("R$", "");
                cliente.codigoPrimeiraIntrucaoCobranca = Convert.ToString(_cad.CODPRIMEIRAINSTCOBRA);
                cliente.codigoSegundaIntrucaoCobranca = Convert.ToString(_cad.CODSEGUNDAINSTCOBRA);
                cliente.taxaEmissaoBoleto = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.TAXAEMISSAOBOLETO ?? 0).Replace("R$", "");

            }

            return cliente;
        }

        public ClienteVm RetornaDadosClienteByCPFCNPJeCodEmpresa(IFiltroCliente filtro)
        {
            ClienteVm cliente = new ClienteVm();
            filtro.CpfCnpj = Helper.RemoveCaracteres(filtro.CpfCnpj);
            var _cad = _cliente.BuscaClienteByCNPJeCodEmpresa(filtro.CpfCnpj, filtro.codigoEmpresa).Result;

            if (_cad != null)
            {
                cliente.codigoCliente = _cad.CODCLIENTE;
                cliente.nomeCliente = _cad.NMCLIENTE;
                cliente.cpfCnpj = Helper.FormataCPFCnPj(_cad.CPFCNPJ);
                cliente.email = _cad.EMAIL;
                cliente.codigoEmpresa = _cad.CODEMPRESA.ToString();
                cliente.cep = _cad.CEP;
                cliente.logradouro = _cad.LOGRADOURO;
                cliente.numeroLogradouro = _cad.NROLOGRADOURO;
                cliente.complemento = _cad.COMPLEMENTO;
                cliente.bairro = _cad.BAIRRO;
                cliente.cidade = _cad.CIDADE;
                cliente.UF = _cad.UF;
                cliente.regraPropria = (_cad.COBRANCADIFERENCIADA == "S") ? "SIM" : "NÃO";
                cliente.cobrancaDiferenciada = (_cad.COBRANCADIFERENCIADA == "S");
                cliente.cobraJuros = (_cad.COBRAJUROS == "S");
                cliente.valorJurosDiaAtraso = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLJUROSDIAATRASO ?? 0).Replace("R$", "");
                cliente.percentualJuros = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCJUROS ?? 0).Replace("R$", "");
                cliente.cobraMulta = (_cad.COBRAMULTA == "S");
                cliente.valorMultaDiaAtraso = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLMULTADIAATRASO ?? 0).Replace("R$", "");
                cliente.percentualMulta = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCMULTA ?? 0).Replace("R$", "");
                cliente.codigoPrimeiraIntrucaoCobranca = Convert.ToString(_cad.CODPRIMEIRAINSTCOBRA);
                cliente.codigoSegundaIntrucaoCobranca = Convert.ToString(_cad.CODSEGUNDAINSTCOBRA);
                cliente.taxaEmissaoBoleto = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.TAXAEMISSAOBOLETO ?? 0).Replace("R$", "");

            }

            return cliente;
        }

        public ClienteVm RetornaDadosClienteByID(IFiltroCliente filtro)
        {
            ClienteVm cliente = new ClienteVm();
            var _cad = _cliente.BuscaClienteByID(filtro.codigoCliente).Result;

            if (_cad != null)
            {
                cliente.codigoCliente = _cad.CODCLIENTE;
                cliente.nomeCliente = _cad.NMCLIENTE;
                cliente.cpfCnpj = Helper.FormataCPFCnPj(_cad.CPFCNPJ);
                cliente.email = _cad.EMAIL;
                cliente.codigoEmpresa = _cad.CODEMPRESA.ToString();
                cliente.cep = _cad.CEP;
                cliente.logradouro = _cad.LOGRADOURO;
                cliente.numeroLogradouro = _cad.NROLOGRADOURO;
                cliente.complemento = _cad.COMPLEMENTO;
                cliente.bairro = _cad.BAIRRO;
                cliente.cidade = _cad.CIDADE;
                cliente.UF = _cad.UF;
                cliente.regraPropria = (_cad.COBRANCADIFERENCIADA == "S") ? "SIM" : "NÃO";
                cliente.cobrancaDiferenciada = (_cad.COBRANCADIFERENCIADA == "S");
                cliente.cobraJuros = (_cad.COBRAJUROS == "S");
                cliente.valorJurosDiaAtraso = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLJUROSDIAATRASO ?? 0).Replace("R$", "");
                cliente.percentualJuros = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCJUROS ?? 0).Replace("R$", "");
                cliente.cobraMulta = (_cad.COBRAMULTA == "S");
                cliente.valorMultaDiaAtraso = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLMULTADIAATRASO ?? 0).Replace("R$", "");
                cliente.percentualMulta = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCMULTA ?? 0).Replace("R$", "");
                cliente.codigoPrimeiraIntrucaoCobranca = Convert.ToString(_cad.CODPRIMEIRAINSTCOBRA);
                cliente.codigoSegundaIntrucaoCobranca = Convert.ToString(_cad.CODSEGUNDAINSTCOBRA);
                cliente.taxaEmissaoBoleto = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.TAXAEMISSAOBOLETO ?? 0).Replace("R$", "");
                cliente.AgruparFaturamentos = (_cad.AGRUPARFATURAMENTOSDIA == "S");

                if (string.IsNullOrWhiteSpace(cliente.codigoFormaFaturamento)) cliente.codigoFormaFaturamento = "1";

                var dadosEmpresa = _empresa.ConsultaEmpresaById(_cad.CODEMPRESA).Result;

                cliente.nomePrimeiraIntrucaoCobranca = _InstrucaoCobranca.GetInstrucaoCobrancaById(Convert.ToInt32(_cad.CODPRIMEIRAINSTCOBRA));
                cliente.nomeSegundaIntrucaoCobranca = _InstrucaoCobranca.GetInstrucaoCobrancaById(Convert.ToInt32(_cad.CODSEGUNDAINSTCOBRA));
                cliente.nomeFormaFaturamento = _FormaFaturamento.GetFormaFaturamentoById(Convert.ToInt32(cliente.codigoFormaFaturamento));
                cliente.nomeEmpresa = dadosEmpresa.NMFANTASIA;
            }

            return cliente;


        }

        public ClienteVm RetornaDadosClienteByIDeCodEmpresa(IFiltroCliente filtro)
        {
            ClienteVm cliente = new ClienteVm();

            var _cad = _cliente.BuscaClienteByIDeCodEmpresa(filtro.codigoCliente, filtro.codigoEmpresa).Result;

            if (_cad != null)
            {
                cliente.codigoCliente = _cad.CODCLIENTE;
                cliente.nomeCliente = _cad.NMCLIENTE;
                cliente.cpfCnpj = Helper.FormataCPFCnPj(_cad.CPFCNPJ);
                cliente.email = _cad.EMAIL;
                cliente.codigoEmpresa = _cad.CODEMPRESA.ToString();
                cliente.cep = _cad.CEP;
                cliente.logradouro = _cad.LOGRADOURO;
                cliente.numeroLogradouro = _cad.NROLOGRADOURO;
                cliente.complemento = _cad.COMPLEMENTO;
                cliente.bairro = _cad.BAIRRO;
                cliente.cidade = _cad.CIDADE;
                cliente.UF = _cad.UF;
                cliente.regraPropria = (_cad.COBRANCADIFERENCIADA == "S") ? "SIM" : "NÃO";
                cliente.cobrancaDiferenciada = (_cad.COBRANCADIFERENCIADA == "S");
                cliente.cobraJuros = (_cad.COBRAJUROS == "S");
                cliente.valorJurosDiaAtraso = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLJUROSDIAATRASO ?? 0).Replace("R$", "");
                cliente.percentualJuros = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCJUROS ?? 0).Replace("R$", "");
                cliente.cobraMulta = (_cad.COBRAMULTA == "S");
                cliente.valorMultaDiaAtraso = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLMULTADIAATRASO ?? 0).Replace("R$", "");
                cliente.percentualMulta = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCMULTA ?? 0).Replace("R$", "");
                cliente.codigoPrimeiraIntrucaoCobranca = Convert.ToString(_cad.CODPRIMEIRAINSTCOBRA);
                cliente.codigoSegundaIntrucaoCobranca = Convert.ToString(_cad.CODSEGUNDAINSTCOBRA);
                cliente.taxaEmissaoBoleto = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.TAXAEMISSAOBOLETO ?? 0).Replace("R$", "");

                if (string.IsNullOrWhiteSpace(cliente.codigoFormaFaturamento)) cliente.codigoFormaFaturamento = "1";

                var dadosEmpresa = _empresa.ConsultaEmpresaById(_cad.CODEMPRESA).Result;

                cliente.nomePrimeiraIntrucaoCobranca = _InstrucaoCobranca.GetInstrucaoCobrancaById(Convert.ToInt32(_cad.CODPRIMEIRAINSTCOBRA));
                cliente.nomeSegundaIntrucaoCobranca = _InstrucaoCobranca.GetInstrucaoCobrancaById(Convert.ToInt32(_cad.CODSEGUNDAINSTCOBRA));
                cliente.nomeFormaFaturamento = _FormaFaturamento.GetFormaFaturamentoById(Convert.ToInt32(cliente.codigoFormaFaturamento));
                cliente.nomeEmpresa = dadosEmpresa.NMFANTASIA;
            }
            return cliente;
        }

        public void SalvarCliente(IClienteVm model)
        {
            PAGNET_CADCLIENTE cliente;

            if (Convert.ToInt32(model.codigoPrimeiraIntrucaoCobranca) < 0)
                model.codigoPrimeiraIntrucaoCobranca = "0";

            if (Convert.ToInt32(model.codigoSegundaIntrucaoCobranca) < 0)
                model.codigoSegundaIntrucaoCobranca = "0";
            if (string.IsNullOrWhiteSpace(model.CodigoTipoPessoa))
            {
                model.CodigoTipoPessoa = "J";
            }

            if (model.codigoCliente == 0)
            {
                cliente = new PAGNET_CADCLIENTE();

            }
            else
            {
                cliente = _cliente.BuscaClienteByID(model.codigoCliente).Result;
            }

            cliente.NMCLIENTE = model.nomeCliente;
            cliente.CPFCNPJ = Helper.RemoveCaracteres(model.cpfCnpj);
            cliente.CODEMPRESA = Convert.ToInt32(model.codigoEmpresa);
            cliente.CEP = Helper.RemoveCaracteres(model.cep);
            cliente.EMAIL = model.email;
            cliente.CODFORMAFATURAMENTO = Convert.ToInt32(model.codigoFormaFaturamento);
            cliente.LOGRADOURO = model.logradouro;
            cliente.NROLOGRADOURO = model.numeroLogradouro;
            cliente.COMPLEMENTO = model.complemento;
            cliente.BAIRRO = model.bairro;
            cliente.CIDADE = model.cidade;
            cliente.UF = model.UF;
            cliente.COBRANCADIFERENCIADA = (model.cobrancaDiferenciada) ? "S" : "N";
            cliente.AGRUPARFATURAMENTOSDIA = (model.AgruparFaturamentos) ? "S" : "N";
            cliente.COBRAJUROS = (model.cobraJuros) ? "S" : "N";
            cliente.VLJUROSDIAATRASO = (model.valorJurosDiaAtraso != null) ? Convert.ToDecimal(model.valorJurosDiaAtraso.Replace("R$", "").Replace(".", "")) : 0;
            cliente.PERCJUROS = (model.percentualJuros != null) ? Convert.ToDecimal(model.percentualJuros.Replace("R$", "").Replace(".", "")) : 0;
            cliente.COBRAMULTA = (model.cobraMulta) ? "S" : "N";
            cliente.VLMULTADIAATRASO = (model.valorMultaDiaAtraso != null) ? Convert.ToDecimal(model.valorMultaDiaAtraso.Replace("R$", "").Replace(".", "")) : 0;
            cliente.PERCMULTA = (model.percentualMulta != null) ? Convert.ToDecimal(model.percentualMulta.Replace("R$", "").Replace(".", "")) : 0;
            cliente.CODPRIMEIRAINSTCOBRA = Convert.ToInt32(model.codigoPrimeiraIntrucaoCobranca);
            cliente.CODSEGUNDAINSTCOBRA = Convert.ToInt32(model.codigoSegundaIntrucaoCobranca);
            cliente.TAXAEMISSAOBOLETO = (model.taxaEmissaoBoleto != null) ? Convert.ToDecimal(model.taxaEmissaoBoleto.Replace("R$", "").Replace(".", "")) : 0;
            cliente.TIPOCLIENTE = model.CodigoTipoPessoa;

            cliente.ATIVO = "S";

            if (model.codigoCliente == 0)
            {
                _cliente.IncluiCliente(cliente);
                _cliente.InsertLog(cliente, _user.cod_usu, "Cliente incluido via PAGNET");
            }
            else
            {
                _cliente.AtualizaCliente(cliente);
                _cliente.InsertLog(cliente, _user.cod_usu, "Cliente Atualizado via PAGNET");
            }
        }
    }
}