using Microsoft.AspNetCore.Http;
using PagNet.Api.Service.Helpers;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Api.Service.Service
{
    public class APIClienteAPP : ApiClientBase, IAPIClienteAPP
    {
        public APIClienteAPP(IHttpContextAccessor contextAccessor, ITokensRepository tokensRepository)
        //: base("https://localhost:44379/", contextAccessor, tokensRepository)
        : base("CRUDCliente/", contextAccessor, tokensRepository)
        { }

        public async Task<List<APIClienteModel>> ConsultaTodosCliente(int codEmpresa, string TipoCliente)
        {
            try
            {
                APIFiltroClienteModel filtro = new APIFiltroClienteModel();
                filtro.codigoEmpresa = codEmpresa;
                filtro.TipoCliente = TipoCliente;

                var ListaCliente = ExecutaPost<List<APIClienteModel>, APIFiltroClienteModel>("ConsultaTodosCliente", filtro).Result;

                return ListaCliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IDictionary<bool, string>> DesativaCliente(int codCli, string Justificativa)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                APIFiltroClienteModel filtro = new APIFiltroClienteModel();
                filtro.codigoCliente = codCli;
                filtro.JustificativaAcao = Justificativa;

                var sucesso = ExecutaPost<bool, APIFiltroClienteModel>("DesativaCliente", filtro).Result;
                string msgRetorno = "";
                if (sucesso)
                {
                    msgRetorno = "Cliente desativado com sucesso.";
                }
                else
                {
                    msgRetorno = "Falha ao desativar o cliente. Favor contactar o suporte.";
                }
                resultado.Add(sucesso, msgRetorno);
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public APIClienteModel RetornaDadosClienteByCPFCNPJ(string cpfCNPJ)
        {
            try
            {
                APIFiltroClienteModel filtro = new APIFiltroClienteModel();
                filtro.CpfCnpj = cpfCNPJ;

                var DadosCliente = ExecutaPost<APIClienteModel, APIFiltroClienteModel>("RetornaDadosClienteByCPFCNPJ", filtro).Result;

                return DadosCliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public APIClienteModel RetornaDadosClienteByCPFCNPJeCodEmpresa(string cpfCNPJ, int codempresa)
        {
            try
            {
                APIFiltroClienteModel filtro = new APIFiltroClienteModel();
                filtro.CpfCnpj = cpfCNPJ;
                filtro.codigoEmpresa = codempresa;

                var DadosCliente = ExecutaPost<APIClienteModel, APIFiltroClienteModel>("RetornaDadosClienteByCPFCNPJeCodEmpresa", filtro).Result;

                return DadosCliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public APIClienteModel RetornaDadosClienteByID(int codCli)
        {
            try
            {
                APIFiltroClienteModel filtro = new APIFiltroClienteModel();
                filtro.codigoCliente = codCli;

                var DadosCliente = ExecutaPost<APIClienteModel, APIFiltroClienteModel>("RetornaDadosClienteByID", filtro).Result;

                return DadosCliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  APIClienteModel RetornaDadosClienteByIDeCodEmpresa(int codCli, int codempresa)
        {
            try
            {
                APIFiltroClienteModel filtro = new APIFiltroClienteModel();
                filtro.codigoCliente = codCli;
                filtro.codigoEmpresa = codempresa;

                var DadosCliente = ExecutaPost<APIClienteModel, APIFiltroClienteModel>("RetornaDadosClienteByIDeCodEmpresa", filtro).Result;

                return DadosCliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IDictionary<bool, string>> SalvarCliente(IAPIClienteModel cliente)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {

                var sucesso = ExecutaPost<bool, IAPIClienteModel>("SalvarCliente", cliente).Result;
                string msgRetorno = "";
                if (sucesso)
                {
                    msgRetorno = "Cliente salvo com sucesso.";
                }
                else
                {
                    msgRetorno = "Falha ao salvar o cliente. Favor contactar o suporte.";
                }
                resultado.Add(sucesso, msgRetorno);
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
