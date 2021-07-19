using Microsoft.AspNetCore.Http;
using PagNet.Api.Service.Helpers;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using PagNet.Bld.AntecipPGTO.Abstraction.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Api.Service.Service
{
    public class APIGestaoDescontoFolhaAppClient : ApiClientBase, IAPIGestaoDescontoFolhaAppClient
    {
        public APIGestaoDescontoFolhaAppClient(IHttpContextAccessor contextAccessor, ITokensRepository tokensRepository)
            : base("https://localhost:44356/", contextAccessor, tokensRepository)
        //: base("https://www3.tln.com.br/APIsTelenet/APIPagNetCliente/", contextAccessor, tokensRepository)
        { }

        private class Model
        {
            public string Cpf { get; set; }
        }

        public IAPIConfigParamLeituraArquivoVM BuscaConfiguracaoByCliente(IAPIFiltroDescontoFolhaVM filtroDF)
        {
            APIConfigParamLeituraArquivoVM dados = new APIConfigParamLeituraArquivoVM();
            try
            {
                if (filtroDF.codigoCliente != 0)
                {
                    dados = ExecutaPost<APIConfigParamLeituraArquivoVM, IAPIFiltroDescontoFolhaVM>("BuscaConfiguracaoByCliente", filtroDF).Result;

                }

                return dados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IAPIUsuariosArquivoRetornoVm BuscaFechamentosNaoDescontados(IAPIFiltroDescontoFolhaVM filtro)
        {
            try
            {
                var dados = ExecutaPost<APIDFUsuarioNaoDescontadoVM, IAPIFiltroDescontoFolhaVM>("BuscaFechamentosNaoDescontados", filtro).Result;

                return dados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IAPIUsuariosArquivoRetornoVm ConsolidaArquivoRetornoCliente(IAPIFiltroDescontoFolhaVM filtro)
        {
            try
            {
                var dados = ExecutaPost<APIDFUsuarioNaoDescontadoVM, IAPIFiltroDescontoFolhaVM>("ConsolidaArquivoRetornoCliente", filtro).Result;

                return dados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DadosUsuarioVM BuscaDadosUsuarioByCPF(string CPF)
        {
            //return ExecutaPost<DadosUsuarioVM, Model>("", new Model { Cpf = CPF });
            throw new NotImplementedException();
        }

        public DadosUsuarioVM BuscaDadosUsuarioByMatricula(string Matricula)
        {
            throw new NotImplementedException();
        }
        public IDictionary<bool, string> DesativarParamLeituraArquivo(int codigoRegra)
        {
            throw new NotImplementedException();
        }

        public void InformaPGTOAtorizado(IAPIFiltroDescontoFolhaVM listaUsuario)
        {
            throw new NotImplementedException();
        }

        public void RenovaSaldoUsuario(IAPIFiltroDescontoFolhaVM usuario)
        {
            throw new NotImplementedException();
        }

        public IDictionary<bool, string> SalvarParamLeituraArquivo(IAPIConfigParamLeituraArquivoVM model)
        {
            var resultado = new Dictionary<bool, string>();

            var sucesso = ExecutaPost<bool, IAPIConfigParamLeituraArquivoVM>("SalvarParamLeituraArquivo", model).Result;
            string msgRetorno = "";
            if (sucesso)
            {
                msgRetorno = "Configuração salva com sucesso.";
            }
            else
            {
                msgRetorno = "Falha ao salvar a configuração. Favor contactar o suporte.";
            }
            resultado.Add(sucesso, msgRetorno);

            return resultado;
        }

        public IDictionary<bool, string> ExecutaProcessoDescontoFolha(IAPIUsuariosArquivoRetornoVm md)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                ExecutaPostNoResult<IAPIUsuariosArquivoRetornoVm>("ExecutaProcessoDescontoFolha", md);

                resultado.Add(true, "Processo executado com sucesso.");
            }
            catch (Exception ex)
            {
                resultado.Add(false, "Falha ao salvar a configuração. Favor contactar o suporte.");
            }


            return resultado;
        }

        public object[][] CarregaListaFaturasAbertas(int codigoCliente)
        {
            var ddl = ExecutaPost<List<APIRetornoDDLModel>, int>("CarregaListaFaturasAbertas", codigoCliente).Result;

            return ddl.Select(x => new object[] { x.Valor, x.Descricao }).ToArray();
        }
    }
}
