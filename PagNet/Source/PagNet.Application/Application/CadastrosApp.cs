using PagNet.Application.Helpers;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Services;
using PagNet.Domain.Interface.Services.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Application.Application
{
    public class CadastrosApp : ICadastrosApp
    {
        private readonly IPagNet_ContaCorrenteService _conta;
        private readonly IPagNet_BancoService _banco;
        private readonly ISubRedeService _subRede;
        private readonly IPagNet_CadEmpresaService _empresa;
        private readonly IPagNet_CadFavorecidoService _favorito;
        private readonly IPagNet_CadClienteService _cliente;
        private readonly IPagNet_InstrucaoCobrancaService _InstrucaoCobranca;
        private readonly IPagNet_Formas_FaturamentoService _FormaFaturamento;
        private readonly IProceduresService _proc;
        private readonly IPagNet_UsuarioService _usuPagNet;



        public CadastrosApp(IPagNet_ContaCorrenteService conta,
                            IPagNet_BancoService banco,
                            ISubRedeService subRede,
                            IPagNet_CadEmpresaService empresa,
                            IPagNet_CadClienteService cliente,
                            IPagNet_UsuarioService usuPagNet,
                            IPagNet_CadFavorecidoService favorito,
                            IPagNet_InstrucaoCobrancaService InstrucaoCobranca,
                            IPagNet_Formas_FaturamentoService FormaFaturamento,
                            IProceduresService proc)
        {
            _conta = conta;
            _banco = banco;
            _subRede = subRede;
            _empresa = empresa;
            _usuPagNet = usuPagNet;
            _proc = proc;
            _favorito = favorito;
            _cliente = cliente;
            _InstrucaoCobranca = InstrucaoCobranca;
            _FormaFaturamento = FormaFaturamento;
        }

        public async Task<List<CadEmpresaVm>> ConsultaTodasEmpresas()
        {
            try
            {
                List<CadEmpresaVm> ListEmpresa = new List<CadEmpresaVm>();

                var dados = await _empresa.GetAllempresas();

                ListEmpresa = CadEmpresaVm.ToListView(dados).ToList();

                return ListEmpresa;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CadFavorecidosVm>> ConsultaTodosFavorecidosCentralizadora()
        {
            try
            {
                List<CadFavorecidosVm> listaFavorecidos = new List<CadFavorecidosVm>();

                var dados = await _favorito.BuscaAllByCentralizadora();

                listaFavorecidos = CadFavorecidosVm.ToListView(dados).ToList();

                return listaFavorecidos;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CadFavorecidosVm>> ConsultaTodosFavorecidosFornecedores(int codEmpresa)
        {
            try
            {
                List<CadFavorecidosVm> listaFavorecidos = new List<CadFavorecidosVm>();

                var dados = await _favorito.BuscaAllFavorecidosFornecedor(codEmpresa);

                listaFavorecidos = CadFavorecidosVm.ToListView(dados).ToList();

                return listaFavorecidos;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CadFavorecidosVm>> ConsultaTodosFavorecidosPAG(int codEmpresa)
        {
            try
            {
                List<CadFavorecidosVm> listaFavorecidos = new List<CadFavorecidosVm>();

                var dados = await _favorito.BuscaAllFavorecidosPagamento(codEmpresa);

                listaFavorecidos = CadFavorecidosVm.ToListView(dados).ToList();

                return listaFavorecidos;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IDictionary<bool, string>> DesativaFavorito(int codFavorito, int codUsuario)
        {
            var resultado = new Dictionary<bool, string>();

            PAGNET_CADFAVORECIDO CadFavorecidos;
            CadFavorecidos = _favorito.GetById(codFavorito);
            CadFavorecidos.ATIVO = "N";

            try
            {
                _favorito.AtualizaFavorito(CadFavorecidos);
                _favorito.InsertLog(CadFavorecidos, codUsuario, "Favorecido Desativado");
                resultado.Add(true, "Favorecido removido com sucesso");

            }
            catch (ArgumentException ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }
        public CadEmpresaVm RetornaDadosEmpresaByID(int CodEmpresa)
        {
            try
            {
                var dados = _empresa.ConsultaEmpresaById(CodEmpresa).Result;

                var CadEmpresa = CadEmpresaVm.ToView(dados);

                if (!string.IsNullOrWhiteSpace(CadEmpresa.CODSUBREDE))
                {
                    int codsubrede = Convert.ToInt32(CadEmpresa.CODSUBREDE);
                    var dadosSubRede = _subRede.GetById(codsubrede);
                    CadEmpresa.NMSUBREDE = dadosSubRede.NOMSUBREDE;
                }

                return CadEmpresa;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CadFavorecidosVm RetornaDadosFavoritoByID(int CodFavorito)
        {
            try
            {
                CadFavorecidosVm Favorecido = new CadFavorecidosVm();

                var dados = _favorito.BuscaFavorecidosByID(CodFavorito).Result;
                if (dados != null)
                {
                    Favorecido = CadFavorecidosVm.ToView(dados);
                    if (!string.IsNullOrWhiteSpace(Favorecido.CODEMPRESA))
                    {
                        var empresa = _empresa.ConsultaEmpresaById(Convert.ToInt32(Favorecido.CODEMPRESA)).Result;
                        Favorecido.NMEMPRESA = empresa.NMFANTASIA;
                    }
                    if (Favorecido.CONTAPAGAMENTOPADRAO)
                    {
                        var conta = _conta.GetContaCorrenteById(Convert.ToInt32(Favorecido.codContaCorrente)).Result;
                        Favorecido.nmContaCorrente = "Banco:" + conta.CODBANCO + " Agencia:" + conta.AGENCIA + " Conta:" + conta.NROCONTACORRENTE + "-" + conta.DIGITOCC;
                    }
                }

                return Favorecido;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CadFavorecidosVm RetornaDadosFavoritoByCPFCNPJ(string cpfCNPJ)
        {
            try
            {
                CadFavorecidosVm CadEmpresa = new CadFavorecidosVm();
                cpfCNPJ = Geral.RemoveCaracteres(cpfCNPJ);
                var dados = _favorito.BuscaFavorecidosByCNPJ(cpfCNPJ).Result;

                if (dados == null)
                {
                    return CadEmpresa;
                }
                CadEmpresa = CadFavorecidosVm.ToView(dados);

                return CadEmpresa;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SubRedeVM RetornaDadosSubRede(int CodSubRede)
        {
            try
            {
                var dados = _subRede.GetById(CodSubRede);

                var subRede = SubRedeVM.ToView(dados);

                return subRede;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EnderecoVM> RetornaEndereco(string CEP)
        {
            try
            {
                EnderecoVM Endereco = new EnderecoVM();

                CEP = CEP.Replace("-", "");
                var DadosEndereco = await _proc.ConsultaEndereco(CEP);

                if (DadosEndereco.RETORNO == "OK")
                {
                    Endereco = EnderecoVM.ToView(DadosEndereco, CEP);
                }
                return Endereco;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IDictionary<bool, string>> SalvarEmpresa(CadEmpresaVm model)
        {
            bool tudoCerto = false;

            var resultado = new Dictionary<bool, string>();

            PAGNET_CADEMPRESA CadEmpresa;


            if (model.CODEMPRESA == 0)
            {
                model.CNPJ = Geral.RemoveCaracteres(model.CNPJ);
                CadEmpresa = new PAGNET_CADEMPRESA();
                var EmpresaJaCadastrado = await _empresa.ConsultaEmpresaByCNPJ(model.CNPJ);
                if (EmpresaJaCadastrado != null)
                {
                    resultado.Add(false, "Já existe uma empresa cadastrada com este CNPJ.");
                    return resultado;
                }

                CadEmpresa = CadEmpresaVm.ToEntity(model);
            }
            else
            {
                CadEmpresa = _empresa.GetById(model.CODEMPRESA);

                CadEmpresa.RAZAOSOCIAL = model.RAZAOSOCIAL;
                CadEmpresa.NMFANTASIA = model.NMFANTASIA;
                CadEmpresa.CNPJ = Geral.RemoveCaracteres(model.CNPJ);
                CadEmpresa.CEP = Geral.RemoveCaracteres(model.CEP);
                CadEmpresa.LOGRADOURO = model.LOGRADOURO ?? "";
                CadEmpresa.NROLOGRADOURO = model.NROLOGRADOURO ?? "";
                CadEmpresa.COMPLEMENTO = model.COMPLEMENTO ?? "";
                CadEmpresa.BAIRRO = model.BAIRRO ?? "";
                CadEmpresa.CIDADE = model.CIDADE ?? "";
                CadEmpresa.UF = model.UF ?? "";
                CadEmpresa.UTILIZANETCARD = (model.UTILIZANETCARD) ? "S" : "N";

                if (Convert.ToInt32(model.CODSUBREDE) >= 1)
                    CadEmpresa.CODSUBREDE = Convert.ToInt32(model.CODSUBREDE);
            }

            try
            {
                if (model.CODEMPRESA == 0)
                {
                    _empresa.InserirEmpresa(CadEmpresa);
                    resultado.Add(true, "Empresa cadastrada com sucesso");
                }
                else
                {
                    _empresa.AtualizaEmpresa(CadEmpresa);
                    resultado.Add(true, "Empresa atualizada com sucesso");
                }

            }
            catch (ArgumentException ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }
        public async Task<IDictionary<bool, string>> SalvarFavorito(CadFavorecidosVm model)
        {
            bool tudoCerto = false;

            var resultado = new Dictionary<bool, string>();

            PAGNET_CADFAVORECIDO CadFavorecidos;


            if (model.CODFAVORECIDO == 0)
            {
                model.CPFCNPJ = Geral.RemoveCaracteres(model.CPFCNPJ);
                CadFavorecidos = new PAGNET_CADFAVORECIDO();
                if (Convert.ToInt32(model.CODEMPRESA) > 0)
                {
                    CadFavorecidos.CODEMPRESA = Convert.ToInt32(model.CODEMPRESA);
                }

            }
            else
            {
                CadFavorecidos = await _favorito.BuscaFavorecidosByID(model.CODFAVORECIDO);
            }

            model.BANCO = Convert.ToString(Convert.ToInt32(model.BANCO));

            while (model.BANCO.Length < 3)
            {
                model.BANCO = "0" + model.BANCO;
            }

            CadFavorecidos.NMFAVORECIDO = model.NMFAVORECIDO;
            CadFavorecidos.CODCEN = (string.IsNullOrWhiteSpace(model.CODCEN)) ? 0 : Convert.ToInt32(model.CODCEN);
            CadFavorecidos.CPFCNPJ = Geral.RemoveCaracteres(model.CPFCNPJ);
            CadFavorecidos.BANCO = (string.IsNullOrWhiteSpace(model.BANCO)) ? "0" : model.BANCO.Trim();
            CadFavorecidos.AGENCIA = (string.IsNullOrWhiteSpace(model.AGENCIA)) ? "0" : model.AGENCIA.Trim();
            CadFavorecidos.DVAGENCIA = (string.IsNullOrWhiteSpace(model.DVAGENCIA)) ? "0" : model.DVAGENCIA.Trim();
            CadFavorecidos.OPE = (string.IsNullOrWhiteSpace(model.OPERACAO)) ? "0" : model.OPERACAO.Trim();
            CadFavorecidos.CONTACORRENTE = (string.IsNullOrWhiteSpace(model.CONTACORRENTE)) ? "0" : model.CONTACORRENTE.Trim();
            CadFavorecidos.DVCONTACORRENTE = (string.IsNullOrWhiteSpace(model.DVCONTACORRENTE)) ? "0" : model.DVCONTACORRENTE.Trim();
            CadFavorecidos.CEP = Geral.RemoveCaracteres(model.CEP);
            CadFavorecidos.LOGRADOURO = (string.IsNullOrWhiteSpace(model.LOGRADOURO)) ? "" : model.LOGRADOURO.Trim();
            CadFavorecidos.NROLOGRADOURO = (string.IsNullOrWhiteSpace(model.NROLOGRADOURO)) ? "" : model.NROLOGRADOURO.Trim();
            CadFavorecidos.COMPLEMENTO = (string.IsNullOrWhiteSpace(model.COMPLEMENTO)) ? "" : model.COMPLEMENTO.Trim();
            CadFavorecidos.BAIRRO = (string.IsNullOrWhiteSpace(model.BAIRRO)) ? "" : model.BAIRRO.Trim();
            CadFavorecidos.CIDADE = (string.IsNullOrWhiteSpace(model.CIDADE)) ? "" : model.CIDADE.Trim();
            CadFavorecidos.UF = (string.IsNullOrWhiteSpace(model.UF)) ? "" : model.UF.Trim();
           
            CadFavorecidos.ATIVO = "S";

            try
            {
                if (model.CODFAVORECIDO == 0)
                {
                    _favorito.IncluiFavorito(CadFavorecidos);
                    _favorito.InsertLog(CadFavorecidos, model.codUsuario, "Favorecido Atualizado via PAGNET");
                    resultado.Add(true, "Favorito cadastrado com sucesso");
                }
                else
                {
                    _favorito.AtualizaFavorito(CadFavorecidos);
                    _favorito.InsertLog(CadFavorecidos, model.codUsuario, "Favorecido Atualizado via PAGNET");
                    resultado.Add(true, "Favorito atualizado com sucesso");
                }

            }
            catch (ArgumentException ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }
        public CadFavorecidosVm RetornaDadosFavoritoByCodCen(int codCen)
        {
            try
            {
                CadFavorecidosVm Favorito = new CadFavorecidosVm();


                var dados = _favorito.BuscaFavorecidosByCodCen(codCen).Result;

                if (dados != null)
                {
                    Favorito = CadFavorecidosVm.ToView(dados);
                }

                return Favorito;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CadClienteVm RetornaDadosClienteByIDeCodEmpresa(int codCli, int codempresa)
        {
            try
            {
                CadClienteVm cadCliente = new CadClienteVm();

                var dados = _cliente.BuscaClienteByIDeCodEmpresa(codCli, codempresa).Result;

                if (dados != null)
                {
                    cadCliente = CadClienteVm.ToView(dados);

                    var dadosEmpresa = _empresa.ConsultaEmpresaById(dados.CODEMPRESA).Result;

                    cadCliente.NMPRIMEIRAINSTCOBRA = _InstrucaoCobranca.GetInstrucaoCobrancaById(Convert.ToInt32(dados.CODPRIMEIRAINSTCOBRA));
                    cadCliente.NMSEGUNDAINSTCOBRA = _InstrucaoCobranca.GetInstrucaoCobrancaById(Convert.ToInt32(dados.CODSEGUNDAINSTCOBRA));
                    cadCliente.NMEMPRESA = dadosEmpresa.NMFANTASIA;
                }
                return cadCliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CadClienteVm RetornaDadosClienteByCPFCNPJeCodEmpresa(string cpfCNPJ, int codempresa)
        {
            try
            {
                CadClienteVm cadCliente = new CadClienteVm();
                cpfCNPJ = Geral.RemoveCaracteres(cpfCNPJ);
                var dados = _cliente.BuscaClienteByCNPJeCodEmpresa(cpfCNPJ, codempresa).Result;

                if (dados != null)
                {
                    cadCliente = CadClienteVm.ToView(dados);
                }

                return cadCliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CadClienteVm RetornaDadosClienteByID(int codCli)
        {
            try
            {
                var dados = _cliente.BuscaClienteByID(codCli).Result;

                var cadCliente = CadClienteVm.ToView(dados);

                var dadosEmpresa = _empresa.ConsultaEmpresaById(dados.CODEMPRESA).Result;

                cadCliente.NMPRIMEIRAINSTCOBRA = _InstrucaoCobranca.GetInstrucaoCobrancaById(Convert.ToInt32(dados.CODPRIMEIRAINSTCOBRA));
                cadCliente.NMSEGUNDAINSTCOBRA = _InstrucaoCobranca.GetInstrucaoCobrancaById(Convert.ToInt32(dados.CODSEGUNDAINSTCOBRA));
                cadCliente.nomeFormaFaturamento = _FormaFaturamento.GetFormaFaturamentoById(Convert.ToInt32(cadCliente.codigoFormaFaturamento));
                cadCliente.NMEMPRESA = dadosEmpresa.NMFANTASIA;

                return cadCliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CadClienteVm RetornaDadosClienteByCPFCNPJ(string cpfCNPJ)
        {
            try
            {
                CadClienteVm cadCliente = new CadClienteVm();

                var dados = _cliente.BuscaClienteByCNPJ(cpfCNPJ).Result;

                if (dados != null)
                {
                    cadCliente = CadClienteVm.ToView(dados);
                }

                return cadCliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CadClienteVm>> ConsultaTodosCliente(int codEmpresa, string TipoCliente)
        {
            try
            {
                List<CadClienteVm> cliente = new List<CadClienteVm>();
                List<PAGNET_CADCLIENTE> dados = new List<PAGNET_CADCLIENTE>();

                dados = await _cliente.BuscaAllClienteByCodEmpresa(codEmpresa, TipoCliente);


                cliente = CadClienteVm.ToListView(dados).ToList();

                return cliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IDictionary<bool, string>> SalvarCliente(CadClienteVm model)
        {
            bool tudoCerto = false;

            var resultado = new Dictionary<bool, string>();

            PAGNET_CADCLIENTE cliente;


            if (Convert.ToInt32(model.CODPRIMEIRAINSTCOBRA) < 0)
                model.CODPRIMEIRAINSTCOBRA = "0";

            if (Convert.ToInt32(model.CODSEGUNDAINSTCOBRA) < 0)
                model.CODSEGUNDAINSTCOBRA = "0";
            if (string.IsNullOrWhiteSpace(model.CodigoTipoPessoa))
            {
                model.CodigoTipoPessoa = "J";
            }


            if (model.CODCLIENTE == 0)
            {
                cliente = new PAGNET_CADCLIENTE();

            }
            else
            {
                cliente = await _cliente.BuscaClienteByID(model.CODCLIENTE);
            }

            cliente.NMCLIENTE = model.NMCLIENTE;
            cliente.CPFCNPJ = Geral.RemoveCaracteres(model.CPFCNPJ);
            cliente.CODEMPRESA = Convert.ToInt32(model.CODEMPRESA);
            cliente.CEP = Geral.RemoveCaracteres(model.CEP);
            cliente.EMAIL = model.EMAIL;
            cliente.CODFORMAFATURAMENTO = Convert.ToInt32(model.codigoFormaFaturamento);
            cliente.LOGRADOURO = model.LOGRADOURO;
            cliente.NROLOGRADOURO = model.NROLOGRADOURO;
            cliente.COMPLEMENTO = model.COMPLEMENTO;
            cliente.BAIRRO = model.BAIRRO;
            cliente.CIDADE = model.CIDADE;
            cliente.UF = model.UF;
            cliente.COBRANCADIFERENCIADA = (model.COBRANCADIFERENCIADA) ? "S" : "N";
            cliente.AGRUPARFATURAMENTOSDIA = (model.AgruparFaturamentos) ? "S" : "N";
            cliente.COBRAJUROS = (model.COBRAJUROS) ? "S" : "N";
            cliente.VLJUROSDIAATRASO = (model.VLJUROSDIAATRASO != null) ? Convert.ToDecimal(model.VLJUROSDIAATRASO.Replace("R$", "").Replace(".", "")) : 0;
            cliente.PERCJUROS = (model.PERCJUROS != null) ? Convert.ToDecimal(model.PERCJUROS.Replace("R$", "").Replace(".", "")) : 0;
            cliente.COBRAMULTA = (model.COBRAMULTA) ? "S" : "N";
            cliente.VLMULTADIAATRASO = (model.VLMULTADIAATRASO != null) ? Convert.ToDecimal(model.VLMULTADIAATRASO.Replace("R$", "").Replace(".", "")) : 0;
            cliente.PERCMULTA = (model.PERCMULTA != null) ? Convert.ToDecimal(model.PERCMULTA.Replace("R$", "").Replace(".", "")) : 0;
            cliente.CODPRIMEIRAINSTCOBRA = Convert.ToInt32(model.CODPRIMEIRAINSTCOBRA);
            cliente.CODSEGUNDAINSTCOBRA = Convert.ToInt32(model.CODSEGUNDAINSTCOBRA);
            cliente.TAXAEMISSAOBOLETO = (model.TAXAEMISSAOBOLETO != null) ? Convert.ToDecimal(model.TAXAEMISSAOBOLETO.Replace("R$", "").Replace(".", "")) : 0;
            cliente.TIPOCLIENTE = model.CodigoTipoPessoa;

            cliente.ATIVO = "S";


                try
            {
                if (model.CODCLIENTE == 0)
                {
                    _cliente.IncluiCliente(cliente);
                    _cliente.InsertLog(cliente, model.codUsuario, "Cliente incluido via PAGNET");
                    resultado.Add(true, "Cliente cadastrado com sucesso");
                }
                else
                {
                    _cliente.AtualizaCliente(cliente);
                    _cliente.InsertLog(cliente, model.codUsuario, "Cliente Atualizado via PAGNET");
                    resultado.Add(true, "Cliente atualizado com sucesso");
                }

            }
            catch (ArgumentException ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }
        public async Task<IDictionary<bool, string>> DesativaCliente(int codCli, int codUsuario, string Justificativa)
        {
            var resultado = new Dictionary<bool, string>();

            PAGNET_CADCLIENTE cliente;

            cliente = await _cliente.BuscaClienteByID(codCli);
            cliente.ATIVO = "N";

            try
            {
                _cliente.AtualizaCliente(cliente);
                _cliente.InsertLog(cliente, codUsuario, Justificativa);
                resultado.Add(true, "Cliente Desativado com sucesso");

            }
            catch (ArgumentException ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }
        public int CriaClienteUsuarioByEmpresa(int codigoEmpresa)
        {
            int CodigoCliente = 0;
            var DadosUsuario = _usuPagNet.BuscaUsuarioAleatorioByEmpresa(codigoEmpresa);
            var DadosCliente = _cliente.BuscaClienteByCNPJ(Geral.RemoveCaracteres(DadosUsuario.CPF)).Result;
            var DadosEmpresa = _empresa.ConsultaEmpresaById(codigoEmpresa).Result;

            if (DadosCliente != null)
            {
                CodigoCliente = DadosCliente.CODCLIENTE;
            }
            else
            {
                PAGNET_CADCLIENTE cli = new PAGNET_CADCLIENTE();

                cli.NMCLIENTE = DadosUsuario.NMUSUARIO;
                cli.CPFCNPJ = Geral.RemoveCaracteres(DadosUsuario.CPF);
                cli.CODEMPRESA = codigoEmpresa;
                cli.CEP = Geral.RemoveCaracteres(DadosEmpresa.CEP);
                cli.EMAIL = DadosUsuario.EMAIL;
                cli.CODFORMAFATURAMENTO = 1;
                cli.LOGRADOURO = DadosEmpresa.LOGRADOURO;
                cli.NROLOGRADOURO = DadosEmpresa.NROLOGRADOURO;
                cli.COMPLEMENTO = DadosEmpresa.COMPLEMENTO;
                cli.BAIRRO = DadosEmpresa.BAIRRO;
                cli.CIDADE = DadosEmpresa.CIDADE;
                cli.UF = DadosEmpresa.UF;
                cli.COBRANCADIFERENCIADA = "N";
                cli.AGRUPARFATURAMENTOSDIA = "N";
                cli.COBRAJUROS = "N";
                cli.VLJUROSDIAATRASO = 0;
                cli.PERCJUROS = 0;
                cli.COBRAMULTA = "N";
                cli.VLMULTADIAATRASO = 0;
                cli.PERCMULTA = 0;
                cli.CODPRIMEIRAINSTCOBRA = 0;
                cli.CODSEGUNDAINSTCOBRA = 0;
                cli.TAXAEMISSAOBOLETO = 0;
                cli.TIPOCLIENTE = "F";
                cli.ATIVO = "N";

                _cliente.IncluiCliente(cli);
                _cliente.InsertLog(cli, 9999, "Cliente incluído para ser utilizado na homologação de cobrança.");

                CodigoCliente = cli.CODCLIENTE;
            }                       

            return CodigoCliente;

        }
    }
}
