using Microsoft.AspNetCore.Http;
using PagNet.Api.Service.Helpers;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Api.Service.Service
{
    public class APIFavorecido : ApiClientBase, IAPIFavorecido
    {
        public APIFavorecido(IHttpContextAccessor contextAccessor, ITokensRepository tokensRepository)
        //: base("https://localhost:44334/", contextAccessor, tokensRepository)
        : base("Favorecido/", contextAccessor, tokensRepository)
        { }

        public List<APIDadosLogModal> ConsultaLog(int codigoFavorecido)
        {
            try
            {
                var dadosRetorno = ExecutaPost<List<APIDadosLogModal>, int>("ConsultaLog", codigoFavorecido).Result;
                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<APIFavorecidoModel> ConsultaTodosFavorecidosCentralizadora(int codigoEmpresa)
        {
            try
            {
                FiltroPesquisaModel filtro = new FiltroPesquisaModel();
                filtro.codigoEmpresa = codigoEmpresa;

                var dadosRetorno = ExecutaPost<List<APIFavorecidoModel>, FiltroPesquisaModel>("ConsultaTodosFavorecidosCentralizadora", filtro).Result;
                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<APIFavorecidoModel> ConsultaTodosFavorecidosFornecedores(int codigoEmpresa)
        {
            try
            {
                FiltroPesquisaModel filtro = new FiltroPesquisaModel();
                filtro.codigoEmpresa = codigoEmpresa;

                var dadosRetorno = ExecutaPost<List<APIFavorecidoModel>, FiltroPesquisaModel>("ConsultaTodosFavorecidosFornecedores", filtro).Result;
                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<APIFavorecidoModel> ConsultaTodosFavorecidosPAG(int codigoEmpresa)
        {
            try
            {
                FiltroPesquisaModel filtro = new FiltroPesquisaModel();
                filtro.codigoEmpresa = codigoEmpresa;

                var dadosRetorno = ExecutaPost<List<APIFavorecidoModel>, FiltroPesquisaModel>("ConsultaTodosFavorecidosPAG", filtro).Result;
                if (dadosRetorno == null)
                {
                    dadosRetorno = new List<APIFavorecidoModel>();
                }
                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IDictionary<bool, string> DesativaFavorecido(int codigoFavorecido)
        {
            var dadosRet = new Dictionary<bool, string>();
            try
            {
                var dadosRetorno = ExecutaPost<RetornoModel, int>("DesativaFavorecido", codigoFavorecido).Result;
                dadosRet.Add(dadosRetorno.Sucesso, dadosRetorno.msgResultado);

            }
            catch (Exception ex)
            {
                dadosRet.Add(false, "Falha ao desativar o favorecido. Favor contactar o suporte.");                
            }
            return dadosRet;
        }
        public IAPIFavorecidoModel RetornaDadosFavorecidoByCodCen(int codigoCentralizadora, int codigoEmpresa)
        {
            try
            {
                FiltroPesquisaModel filtro = new FiltroPesquisaModel();
                filtro.codigoEmpresa = codigoEmpresa;
                filtro.codigoFavorecido = codigoCentralizadora;

                var dadosRetorno = ExecutaPost<APIFavorecidoModel, FiltroPesquisaModel>("RetornaDadosFavorecidoByCodCen", filtro).Result;
                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IAPIFavorecidoModel RetornaDadosFavorecidoByCPFCNPJ(string cpfCNPJ, int codigoEmpresa)
        {
            try
            {
                FiltroPesquisaModel filtro = new FiltroPesquisaModel();
                filtro.codigoEmpresa = codigoEmpresa;
                filtro.CPFCNPJ = cpfCNPJ;

                var dadosRetorno = ExecutaPost<APIFavorecidoModel, FiltroPesquisaModel>("RetornaDadosFavorecidoByCPFCNPJ", filtro).Result;
                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IAPIFavorecidoModel RetornaDadosFavorecidoByID(int codigoFavorecido, int codigoEmpresa)
        {
            try
            {
                FiltroPesquisaModel filtro = new FiltroPesquisaModel();
                filtro.codigoEmpresa = codigoEmpresa;
                filtro.codigoFavorecido = codigoFavorecido;

                var dadosRetorno = ExecutaPost<APIFavorecidoModel, FiltroPesquisaModel>("RetornaDadosFavorecidoByID", filtro).Result;
                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IDictionary<bool, string> SalvarFavorecido(IAPIFavorecidoModel favorecido)
        {
            var dadosRet = new Dictionary<bool, string>();
            try
            {
                var dadosRetorno = ExecutaPost<RetornoModel, IAPIFavorecidoModel>("SalvarFavorecido", favorecido).Result;
                dadosRet.Add(dadosRetorno.Sucesso, dadosRetorno.msgResultado);

            }
            catch (Exception ex)
            {
                dadosRet.Add(false, "Falha ao Salvar o favorecido. Favor contactar o suporte.");
            }
            return dadosRet;
        }
    }
}
