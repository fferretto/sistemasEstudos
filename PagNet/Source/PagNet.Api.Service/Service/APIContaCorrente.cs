using Microsoft.AspNetCore.Http;
using PagNet.Api.Service.Helpers;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Api.Service.Service
{
    public class APIContaCorrente : ApiClientBase, IAPIContaCorrente
    {
        public APIContaCorrente(IHttpContextAccessor contextAccessor, ITokensRepository tokensRepository)
        //: base("https://localhost:44322/", contextAccessor, tokensRepository)
        : base("ContaCorrente/", contextAccessor, tokensRepository)
        { }
        
        public string BuscaBancoByID(string codBanco)
        {
            try
            {
                var nomeBanco = ExecutaPost<string, string>("BuscaBancoByID", codBanco).Result;

                return nomeBanco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Desativar(int codigoContaCorrente)
        {
            try
            {
                ExecutaPostNoResult<int>("DesativarContaCorrente", codigoContaCorrente);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExisteArquivoRemessaBoletoCriado(int codigoContaCorrente)
        {
            try
            {

                var retornoBoolean = ExecutaPost<bool, int>("ExisteArquivoRemessaBoletoCriado", codigoContaCorrente).Result;
                return retornoBoolean;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IAPIResultadoTransmissaoArquivoModel GeraArquivoRemessaHomologacao(IAPIDadosHomologarContaCorrenteModel model)
        {
            try
            {
                var dadosRetorno = ExecutaPost<APIResultadoTransmissaoArquivoModel, IAPIDadosHomologarContaCorrenteModel>("GeraArquivoRemessaHomologacao", model).Result;
                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IAPIResultadoTransmissaoArquivoModel GeraBoletoPDFHomologacao(IAPIDadosHomologarContaCorrenteModel model)
        {
            try
            {
                var dadosRetorno = ExecutaPost<APIResultadoTransmissaoArquivoModel, IAPIDadosHomologarContaCorrenteModel>("GeraBoletoPDFHomologacao", model).Result;
                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<APIConsultaContaCorrenteModel> GetAllContaCorrente(int codEmpresa)
        {
            try
            {
                var infoContaCorrente = ExecutaPost<List<APIConsultaContaCorrenteModel>, int>("BuscaTodasContaCorrente", codEmpresa).Result;
                return infoContaCorrente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object[][] GetBanco()
        {
            try
            {
                var ddl = ExecutaPostNoParam<List<APIRetornoDDLModel>>("DDLBuscaBanco").Result;

                return FormataDDL(ddl, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IAPIBancoModel GetBancoByCodContaCorrente(int codContaCorrente)
        {
            try
            {
                dynamic filtro = new JsonObject();
                filtro.codigoContaCorrente = codContaCorrente;

                var retorno = ExecutaPost<APIBancoModel, string>("BuscaBancoByContaCorrente", filtro).Result;
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object[][] GetContaCorrente(int codigoEmpresa)
        {
            try
            {
                var ddl = ExecutaPost<List<APIRetornoDDLModel>, int>("DDLBuscaContaCorrente", codigoEmpresa).Result;
                return FormataDDL(ddl,false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object[][] GetContaCorrenteBoleto(int codigoEmpresa)
        {
            try
            {
                var ddl = ExecutaPost<List<APIRetornoDDLModel>, int>("DDLBuscaContaCorrenteBoleto", codigoEmpresa).Result;
                return FormataDDL(ddl, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IAPIContaCorrenteModel GetContaCorrenteById(int? id, int codigoEmpresa)
        {
            try
            {

                APIFiltro _filtro = new APIFiltro();
                _filtro.codigoContaCorrente = (int)id;
                _filtro.codigoEmpresa = codigoEmpresa;

                var infoContaCorrente = ExecutaPost<APIContaCorrenteModel, APIFiltro>("BuscaContaCorrenteByID", _filtro).Result;
                return infoContaCorrente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object[][] GetContaCorrentePagamento(int codigoEmpresa)
        {
            try
            {
                var ddl = ExecutaPost<List<APIRetornoDDLModel>, int>("DDLBuscaContaCorrentePagamento", codigoEmpresa).Result;
                return FormataDDL(ddl, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IDictionary<string, string> Salvar(IAPIContaCorrenteModel model)
        {
            try
            {
                Dictionary<string, string> retorno = new Dictionary<string, string>();

                var msgRetorno = ExecutaPost<string,IAPIContaCorrenteModel>("SalvarContaCorrente", model).Result;

                if(msgRetorno != "")
                {
                    retorno.Add("SUCESSO", msgRetorno);
                }
                else
                {
                    retorno.Add("CONTA", msgRetorno);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private object[][] FormataDDL(List<APIRetornoDDLModel> lista, bool PossuiTitle)
        {
            if(PossuiTitle)
            {
                return lista.Select(x => new object[] { x.Valor, x.Descricao, x.Title }).ToArray();
            }
            else
            {
                return lista.Select(x => new object[] { x.Valor, x.Descricao }).ToArray();
            }
        }
    }
}
