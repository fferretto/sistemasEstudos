using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.PGTO.Favorecido.Abstraction.Interface;
using PagNet.Bld.PGTO.Favorecido.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.Favorecido.Abstraction.Model;
using PagNet.Bld.PGTO.Favorecido.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telenet.BusinessLogicModel;
using static PagNet.Bld.PGTO.Favorecido.Constants;

namespace PagNet.Bld.PGTO.Favorecido.Application
{
    public class CadastroApp : Service<IContextoApp>, ICadastroApp
    {
        private readonly IParametrosApp _user;
        private readonly IPAGNET_CADFAVORECIDOService _favorecido;
        private readonly IPAGNET_CADEMPRESAService _empresa;
        private readonly IPAGNET_CONTACORRENTEService _conta;

        public CadastroApp(IContextoApp contexto,
                               IParametrosApp user,
                               IPAGNET_CADFAVORECIDOService favorecido,
                               IPAGNET_CADEMPRESAService empresa,
                               IPAGNET_CONTACORRENTEService conta
            )
           : base(contexto)
        {
            _user = user;
            _favorecido = favorecido;
            _empresa = empresa;
            _conta = conta;
        }

        public List<FavorecidosModel> ConsultaTodosFavorecidosCentralizadora(int codigoEmpresa)
        {
            try
            {
                var dados = _favorecido.BuscaAllByCentralizadora();
                var itemsadf = dados.Where(x => x.CODFAVORECIDO == 180188).FirstOrDefault();
                return RetornaListaFavorecidoModel(dados, codigoEmpresa);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FavorecidosModel> ConsultaTodosFavorecidosFornecedores(int codigoEmpresa)
        {

            AssertionValidator
                .AssertNow(codigoEmpresa > 0, CodigosErro.CodigoEmpresaNaoInformado)
                .Validate();

            var dados = _favorecido.BuscaAllFavorecidosFornecedor(codigoEmpresa);
            return RetornaListaFavorecidoModel(dados, codigoEmpresa);
        }
        public List<FavorecidosModel> ConsultaTodosFavorecidosPAG(int codigoEmpresa)
        {
            AssertionValidator
                .AssertNow(codigoEmpresa > 0, CodigosErro.CodigoEmpresaNaoInformado)
                .Validate();

            var dados = _favorecido.BuscaAllFavorecidosPagamento(codigoEmpresa);
            return RetornaListaFavorecidoModel(dados, codigoEmpresa);
        }
        public RetornoModel DesativaFavorecido(int codigoFavorecido)
        {
            AssertionValidator
                .AssertNow(codigoFavorecido > 0, CodigosErro.CodigoFavorecidoNaoInformado)
                .Validate();

            RetornoModel dadosRetorno = new RetornoModel();

            PAGNET_CADFAVORECIDO CadFavorecidos;
            CadFavorecidos = _favorecido.GetById(codigoFavorecido);
            CadFavorecidos.ATIVO = "N";

            try
            {
                _favorecido.AtualizaFavorito(CadFavorecidos);
                _favorecido.InsertLog(CadFavorecidos, _user.cod_usu, "Favorecido Desativado");
                dadosRetorno.Sucesso = true;
                dadosRetorno.msgResultado = "Favorecido removido com sucesso";

            }
            catch (ArgumentException ex)
            {
                dadosRetorno.Sucesso = false;
                dadosRetorno.msgResultado = ex.Message;
            }

            return dadosRetorno;
        }
        public FavorecidosModel RetornaDadosFavorecidoByCodCen(int codigoCentralizadora, int codigoEmpresa)
        {
            AssertionValidator
                .AssertNow(codigoCentralizadora > 0, CodigosErro.CodigoCentralizadoraNaoInformado)
                .Validate();

            FavorecidosModel favorecido = new FavorecidosModel();


            var dados = _favorecido.BuscaFavorecidosByCodCen(codigoCentralizadora);

            if (dados != null)
            {
                favorecido = RetornaFavorecidoModel(dados, codigoEmpresa);
            }

            return favorecido;
        }
        public FavorecidosModel RetornaDadosFavorecidoByCPFCNPJ(string cpfCNPJ, int codigoEmpresa)
        {
            AssertionValidator
                .AssertNow(!string.IsNullOrEmpty(cpfCNPJ), CodigosErro.CpfCnpjNaoInformado)
                .Validate();

            FavorecidosModel dadosRet = new FavorecidosModel();
            cpfCNPJ = Geral.RemoveCaracteres(cpfCNPJ);
            var dados = _favorecido.BuscaFavorecidosByCNPJ(cpfCNPJ);

            if (dados == null)
            {
                return dadosRet;
            }

            dadosRet = RetornaFavorecidoModel(dados, codigoEmpresa);

            return dadosRet;
        }
        public FavorecidosModel RetornaDadosFavorecidoByID(int codigoFavorecido, int codigoEmpresa)
        {
            AssertionValidator
                .AssertNow(codigoFavorecido > 0, CodigosErro.CodigoFavorecidoNaoInformado)
                .Validate();

            FavorecidosModel Favorecido = new FavorecidosModel();

            var dados = _favorecido.BuscaFavorecidosByID(codigoFavorecido);
            if (dados != null)
            {
                Favorecido = RetornaFavorecidoModel(dados, codigoEmpresa);
                if (Favorecido.codigoEmpresa > 0)
                {
                    var empresa = _empresa.ConsultaEmpresaById(Convert.ToInt32(Favorecido.codigoEmpresa)).Result;
                    Favorecido.nomeEmpresa = empresa.NMFANTASIA;
                }
                if (Favorecido.contaPagamentoPadrao)
                {
                    var conta = _conta.GetContaCorrenteById(Convert.ToInt32(Favorecido.codigoContaCorrente)).Result;
                    Favorecido.nmContaCorrente = "Banco:" + conta.CODBANCO + " Agencia:" + conta.AGENCIA + " Conta:" + conta.NROCONTACORRENTE + "-" + conta.DIGITOCC;
                }
            }

            return Favorecido;
        }
        public RetornoModel SalvarFavorecido(IFavorecidosModel model)
        {

            var resultado = new RetornoModel();

            if (model.codigoEmpresa == 0)
                model.codigoEmpresa = _user.cod_empresa;

            PAGNET_CADFAVORECIDO CadFavorecidos = new PAGNET_CADFAVORECIDO();
            PAGNET_CADFAVORECIDO_CONFIG CONFIG_FAV = new PAGNET_CADFAVORECIDO_CONFIG();


            if (model.codigoFavorecido == 0)
            {
                model.CPFCNPJ = Geral.RemoveCaracteres(model.CPFCNPJ);
                CadFavorecidos = new PAGNET_CADFAVORECIDO();
                CONFIG_FAV = new PAGNET_CADFAVORECIDO_CONFIG();
                if (Convert.ToInt32(model.codigoEmpresa) > 0)
                {
                    CadFavorecidos.CODEMPRESA = Convert.ToInt32(model.codigoEmpresa);
                }
                CadFavorecidos.DTINCLUSAO = DateTime.Now;

            }
            else
            {
                CadFavorecidos = _favorecido.BuscaFavorecidosByID(model.codigoFavorecido);
                CONFIG_FAV = _favorecido.BuscaConfigFavorecido(model.codigoFavorecido, model.codigoEmpresa);

                if (CONFIG_FAV == null)
                    CONFIG_FAV = new PAGNET_CADFAVORECIDO_CONFIG();
            }

            model.Banco = Convert.ToString(Convert.ToInt32(model.Banco));

            while (model.Banco.Length < 3)
            {
                model.Banco = "0" + model.Banco;
            }

            CadFavorecidos.NMFAVORECIDO = model.nomeFavorecido;
            CadFavorecidos.CODCEN = (string.IsNullOrWhiteSpace(model.codigoCentralizadora)) ? 0 : Convert.ToInt32(model.codigoCentralizadora);
            CadFavorecidos.CPFCNPJ = Geral.RemoveCaracteres(model.CPFCNPJ);
            CadFavorecidos.BANCO = (string.IsNullOrWhiteSpace(model.Banco)) ? "0" : model.Banco.Trim();
            CadFavorecidos.AGENCIA = (string.IsNullOrWhiteSpace(model.Agencia)) ? "0" : model.Agencia.Trim();
            CadFavorecidos.DVAGENCIA = (string.IsNullOrWhiteSpace(model.DvAgencia)) ? "0" : model.DvAgencia.Trim();
            CadFavorecidos.OPE = (string.IsNullOrWhiteSpace(model.Operacao)) ? "0" : model.Operacao.Trim();
            CadFavorecidos.CONTACORRENTE = (string.IsNullOrWhiteSpace(model.contaCorrente)) ? "0" : model.contaCorrente.Trim();
            CadFavorecidos.DVCONTACORRENTE = (string.IsNullOrWhiteSpace(model.DvContaCorrente)) ? "0" : model.DvContaCorrente.Trim();
            CadFavorecidos.CEP = Geral.RemoveCaracteres(model.CEP);
            CadFavorecidos.LOGRADOURO = (string.IsNullOrWhiteSpace(model.Logradouro)) ? "" : model.Logradouro.Trim();
            CadFavorecidos.NROLOGRADOURO = (string.IsNullOrWhiteSpace(model.nroLogradouro)) ? "" : model.nroLogradouro.Trim();
            CadFavorecidos.COMPLEMENTO = (string.IsNullOrWhiteSpace(model.Complemento)) ? "" : model.Complemento.Trim();
            CadFavorecidos.BAIRRO = (string.IsNullOrWhiteSpace(model.Bairro)) ? "" : model.Bairro.Trim();
            CadFavorecidos.CIDADE = (string.IsNullOrWhiteSpace(model.cidade)) ? "" : model.cidade.Trim();
            CadFavorecidos.UF = (string.IsNullOrWhiteSpace(model.UF)) ? "" : model.UF.Trim();
            CadFavorecidos.ATIVO = "S";
            CadFavorecidos.DTALTERACAO = DateTime.Now;

            CONFIG_FAV.REGRADIFERENCIADA = (model.regraDiferenciada) ? "S" : "N";


            CONFIG_FAV.CODEMPRESA = model.codigoEmpresa;

            if (model.contaPagamentoPadrao)
            {
                CONFIG_FAV.CODCONTACORRENTE = Convert.ToInt32(model.codigoContaCorrente);
            }
            else
            {
                CONFIG_FAV.CODCONTACORRENTE = null;
            }

            CONFIG_FAV.VALTED = 0;
            CONFIG_FAV.VALMINIMOTED = 0;
            CONFIG_FAV.VALMINIMOCC = 0;

            if (model.regraDiferenciada)
            {
                CONFIG_FAV.VALTED = (model.ValorTED != null) ? Convert.ToDecimal(model.ValorTED.Replace("R$", "").Replace(".", "")) : 0;
                CONFIG_FAV.VALMINIMOTED = (model.ValorMinimoTed != null) ? Convert.ToDecimal(model.ValorMinimoTed.Replace("R$", "").Replace(".", "")) : 0;
                CONFIG_FAV.VALMINIMOCC = (model.ValorMinimoCC != null) ? Convert.ToDecimal(model.ValorMinimoCC.Replace("R$", "").Replace(".", "")) : 0;
            }

            try
            {
                if (model.codigoFavorecido == 0)
                {
                    _favorecido.IncluiFavorito(CadFavorecidos);
                    _favorecido.InsertLog(CadFavorecidos, _user.cod_usu, "Favorecido Atualizado via PAGNET");
                    resultado.Sucesso = true;
                    resultado.msgResultado = "Favorecido cadastrado com sucesso";
                }
                else
                {
                    _favorecido.AtualizaFavorito(CadFavorecidos);
                    _favorecido.InsertLog(CadFavorecidos, _user.cod_usu, "Favorecido Atualizado via PAGNET");
                    resultado.Sucesso = true;
                    resultado.msgResultado = "Favorecido atualizado com sucesso";
                }

                CONFIG_FAV.CODFAVORECIDO = CadFavorecidos.CODFAVORECIDO;

                if (CONFIG_FAV.CODFAVORECIDOCONFIG == 0)
                    _favorecido.InseriFavoritoConfig(CONFIG_FAV);
                else
                    _favorecido.AtualizaFavoritoConfig(CONFIG_FAV);

            }
            catch (ArgumentException ex)
            {
                resultado.Sucesso = false;
                resultado.msgResultado = ex.Message;
            }

            return resultado;
        }
        private List<FavorecidosModel> RetornaListaFavorecidoModel(List<PAGNET_CADFAVORECIDO> lista, int codigoEmpresa)
        {
            List<FavorecidosModel> listaRet = new List<FavorecidosModel>();
            FavorecidosModel favorecido = new FavorecidosModel();
            foreach (var item in lista)
            {
                favorecido = RetornaFavorecidoModel(item, codigoEmpresa);
                listaRet.Add(favorecido);
            }

            return listaRet;
        }
        private FavorecidosModel RetornaFavorecidoModel(PAGNET_CADFAVORECIDO item, int codigoEmpresa)
        {
            FavorecidosModel favorecido = new FavorecidosModel();

            favorecido.codigoFavorecido = item.CODFAVORECIDO;
            favorecido.codigoEmpresa = codigoEmpresa;
            favorecido.codigoCentralizadora = Convert.ToString(item.CODCEN);
            var DadosEmpresa = _empresa.ConsultaEmpresaById(codigoEmpresa).Result;
            favorecido.nomeEmpresa = DadosEmpresa.NMFANTASIA;

            favorecido.nomeFavorecido = item.NMFAVORECIDO;
            favorecido.CPFCNPJ = Geral.FormataCPFCnPj(item.CPFCNPJ);
            favorecido.Banco = item.BANCO;
            favorecido.Agencia = item.AGENCIA;
            favorecido.DvAgencia = item.DVAGENCIA;
            favorecido.Operacao = item.OPE;
            favorecido.contaCorrente = item.CONTACORRENTE;
            favorecido.DvContaCorrente = item.DVCONTACORRENTE;
            favorecido.CEP = Geral.FormataCEP(item.CEP);
            favorecido.Logradouro = item.LOGRADOURO;
            favorecido.nroLogradouro = item.NROLOGRADOURO;
            favorecido.Complemento = item.COMPLEMENTO;
            favorecido.Bairro = item.BAIRRO;
            favorecido.cidade = item.CIDADE;
            favorecido.UF = item.UF;

            var config = item.PAGNET_CADFAVORECIDO_CONFIG.Where(x => x.CODEMPRESA == codigoEmpresa).FirstOrDefault();

            if (config != null)
            {

                favorecido.regraDiferenciada = (config.REGRADIFERENCIADA == "S");
                favorecido.ValorTED = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", config.VALTED).Replace("R$", "");
                favorecido.ValorMinimoTed = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", config.VALMINIMOTED).Replace("R$", "");
                favorecido.contaPagamentoPadrao = (config.CODCONTACORRENTE != null);
                favorecido.codigoContaCorrente = Convert.ToString(config.CODCONTACORRENTE);
            }

            return favorecido;
        }
        public List<DadosLogModal> ConsultaLog(int codigoFavorecido)
        {
            AssertionValidator
               .AssertNow(codigoFavorecido > 0, CodigosErro.CodigoFavorecidoNaoInformado)
               .Validate();

            var DadosLog = _favorecido.ConsultaLog(codigoFavorecido);
            
            List<DadosLogModal> dadosRetorno = new List<DadosLogModal>();
            DadosLogModal log = new DadosLogModal();
            foreach (var item in DadosLog)
            {
                log = new DadosLogModal();
                log.dataLog = item.DATINCLOG.ToShortDateString();
                log.descLog = item.DESCLOG;
                log.nomeUsu = item.PAGNET_USUARIO.NMUSUARIO;
                dadosRetorno.Add(log);
            }

            return dadosRetorno;
        }
    }
}
