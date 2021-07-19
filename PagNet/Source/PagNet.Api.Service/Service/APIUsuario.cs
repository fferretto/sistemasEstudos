using Microsoft.AspNetCore.Http;
using PagNet.Api.Service.Helpers;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Api.Service.Service
{
    public class APIUsuario : ApiClientBase, IAPIUsuario
    {
        public APIUsuario(IHttpContextAccessor contextAccessor, ITokensRepository tokensRepository)
        //: base("https://localhost:44395/", contextAccessor, tokensRepository)
        : base("Usuario/", contextAccessor, tokensRepository)
        { }

        public RetornoModel Desativar(IFiltroModel filtro)
        {
            try
            {
                var nomeBanco = ExecutaPost<RetornoModel, IFiltroModel>("Desativar", filtro).Result;

                return nomeBanco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UsuarioModel> GetAllUsuarioByCodEmpresa(IFiltroModel filtro)
        {
            try
            {
                var nomeBanco = ExecutaPost<List<UsuarioModel>, IFiltroModel>("GetAllUsuarioByCodEmpresa", filtro).Result;

                return nomeBanco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UsuarioModel> GetAllUsuarioByCodOpe(IFiltroModel filtro)
        {
            try
            {
                var nomeBanco = ExecutaPost<List<UsuarioModel>, IFiltroModel>("GetAllUsuarioByCodOpe", filtro).Result;

                return nomeBanco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UsuarioModel GetUsuario(IFiltroModel filtro)
        {
            try
            {
                var nomeBanco = ExecutaPost<UsuarioModel, IFiltroModel>("GetUsuario", filtro).Result;

                return nomeBanco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RetornoModel SalvaAlteracaoSenha(IUsuarioModel model)
        {
            try
            {
                var nomeBanco = ExecutaPost<RetornoModel, IUsuarioModel>("SalvaAlteracaoSenha", model).Result;

                return nomeBanco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RetornoModel Salvar(IUsuarioModel model)
        {
            try
            {
                var nomeBanco = ExecutaPost<RetornoModel, IUsuarioModel>("Salvar", model).Result;

                return nomeBanco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RetornoValidaUsuario ValidaLoginRecuperarSenha(IFiltroModel filtro)
        {
            try
            {
                filtro.EmailUsuario = Convert.ToString(filtro.EmailUsuario);
                filtro.Login = Convert.ToString(filtro.Login);

                var nomeBanco = ExecutaPostNoToken<RetornoValidaUsuario, IFiltroModel>("ValidaLoginRecuperarSenha", filtro).Result;

                return nomeBanco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
