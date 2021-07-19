using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PagNet.Api.Service.Interface.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Api.Service.Helpers
{
    public abstract class ApiClientBase : IApiClient
    {
        public ApiClientBase(string baseAddress, IHttpContextAccessor contextAccessor, ITokensRepository tokensRepository)
        {
            TokensRepository = tokensRepository;
            ContextAccessor = contextAccessor;
            BaseAddress = GetBaseAddress(baseAddress);
        }

        protected ITokensRepository TokensRepository { get; }
        protected IHttpContextAccessor ContextAccessor { get; }
        protected virtual IApiResponse<TResult> ExecutaPostNoToken<TResult, TModel>(string relativePath, TModel ModelApi)
        {


            using (var client = new HttpClient())
            {
                if (client.BaseAddress == null)
                {

                    client.BaseAddress = new Uri(BaseAddress);
                    client.Timeout = TimeSpan.FromMinutes(5);
                    //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }

                var content = new StringContent(JsonConvert.SerializeObject(ModelApi),
                                                Encoding.UTF8,
                                                "application/json");

                var response = new ApiResponse<TResult>(client.PostAsync(relativePath, content).Result);

                if (!response.Success)
                {
                    if (response.isValidationError)
                        throw new ApplicationException(string.Join(Environment.NewLine, response.Errors));
                    else
                        throw new Exception(string.Join(Environment.NewLine, "Ocorreu uma falha durante o processo.favor contactar o suporte técnico!"));
                }

                return response;
            }
        }
        protected virtual IApiResponse<TResult> ExecutaPost<TResult, TModel>(string relativePath, TModel ModelApi)
        {
            
            using (var client = new HttpClient())
            {
                if (client.BaseAddress == null)
                {
                    var token = (TokensRepository.Get(ContextAccessor.HttpContext.Request.GetSid()) as IAccessToken).Token;

                    client.BaseAddress = new Uri(BaseAddress);
                    client.Timeout = TimeSpan.FromMinutes(5);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }

                var content = new StringContent(JsonConvert.SerializeObject(ModelApi),
                                                Encoding.UTF8,
                                                "application/json");

                var response = new ApiResponse<TResult>(client.PostAsync(relativePath, content).Result);

                if (!response.Success)
                {
                    if(response.isValidationError)
                        throw new ApplicationException(string.Join(Environment.NewLine, response.Errors));
                    else
                        throw new Exception(string.Join(Environment.NewLine, "Ocorreu uma falha durante o processo.favor contactar o suporte técnico!"));
                }

                return response;
            }
        }
        protected virtual void ExecutaPostNoResult<TModel>(string relativePath, TModel ModelApi)
        {
            using (var client = new HttpClient())
            {
                if (client.BaseAddress == null)
                {
                    var token = (TokensRepository.Get(ContextAccessor.HttpContext.Request.GetSid()) as IAccessToken).Token;

                    client.BaseAddress = new Uri(BaseAddress);
                    client.Timeout = TimeSpan.FromMinutes(5);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }

                var content = new StringContent(JsonConvert.SerializeObject(ModelApi),
                                                Encoding.UTF8,
                                                "application/json");


                HttpResponseMessage response = client.PostAsync(relativePath, content).Result;
                response.EnsureSuccessStatusCode();


            }
        }
        protected virtual IApiResponse<TResult> ExecutaPostNoParam<TResult>(string relativePath)
        {
            using (var client = new HttpClient())
            {
                if (client.BaseAddress == null)
                {
                    var token = (TokensRepository.Get(ContextAccessor.HttpContext.Request.GetSid()) as IAccessToken).Token;

                    client.BaseAddress = new Uri(BaseAddress);
                    client.Timeout = TimeSpan.FromMinutes(5);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }

                var response = new ApiResponse<TResult>(client.PostAsync(relativePath, null).Result);

                if (!response.Success)
                    throw new Exception(string.Join(Environment.NewLine, response.Errors));

                return response;
            }
        }
        protected virtual string GetBaseAddress(string baseAddress)
        {
            if(ContextAccessor == null)
            {
                return baseAddress;
            }
            //Verifica se está no modo debug, caso positivo não faz nada.
            if (baseAddress.IndexOf("localhost") > 0)
            {
                return baseAddress;
            }
            var _claims = ContextAccessor.HttpContext.User.Claims;
            var claim = _claims.FirstOrDefault(c => c.Type.Equals("srv_data", StringComparison.InvariantCultureIgnoreCase));
            var servidor = claim.Value;

            if (servidor.ToUpper() == "NETUNO")
            {
                baseAddress = @"https://www3.tln.com.br/API/PagNet/" + baseAddress;
            }
            else
            {
                baseAddress = @"https://www1.tln.com.br/API/PagNet/" + baseAddress;
            }

            return baseAddress;
        }
        public string BaseAddress { get; }
    }
}
