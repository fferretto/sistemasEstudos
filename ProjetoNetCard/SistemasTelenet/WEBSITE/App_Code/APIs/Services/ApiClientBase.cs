using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

public class ApiClientBase : IAPIClient
{
    //protected IRequestSession _request;
    public string BaseAddress { get; set; }
    private string _Token = "";

    public ApiClientBase(string baseAddress, string servidor)
    {
        BaseAddress = GetBaseAddress(baseAddress, servidor);
        _Token = "Bearer " + ApiHelper.GetToken();
    }

    //protected virtual IApiResponse<TResult> ExecutaPostNoToken<TResult, TModel>(string relativePath, TModel ModelApi)
    //{

    //    var wb = new System.Net.WebClient();

    //    var content = new StringContent(JsonConvert.SerializeObject(ModelApi),
    //                                    Encoding.UTF8,
    //                                    "application/json");

    //    string url = BaseAddress;

    //    var response = wb.UploadValues(url, "POST", content);

    //    using (var client = new HttpClient())
    //    {
    //        if (client.BaseAddress == null)
    //        {

    //            client.BaseAddress = new Uri(BaseAddress);
    //            client.Timeout = TimeSpan.FromMinutes(5);
    //            //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    //        }

    //        var response = new ApiResponse<TResult>(client.PostAsync(relativePath, content).Result);

    //        if (!response.Success)
    //        {
    //            if (response.isValidationError)
    //                throw new ApplicationException(string.Join(Environment.NewLine, response.Errors));
    //            else
    //                throw new Exception(string.Join(Environment.NewLine, "Ocorreu uma falha durante o processo.favor contactar o suporte técnico!"));
    //        }

    //        return response;
    //    }
    //}
    public virtual IApiResponse<TResult> ExecutaPost<TResult, TModel>(string relativePath, TModel Param)
    {
        var client = new WebClient();
        var Caminho = Path.Combine(BaseAddress, relativePath);

        client.Headers.Add("Authorization", "Bearer " + ApiHelper.GetToken());
        client.Headers[HttpRequestHeader.ContentType] = "application/json";
        client.Encoding = Encoding.UTF8;
        var response = new ApiResponse<TResult>(client.UploadString(Caminho, "POST", JsonConvert.SerializeObject(Param)));

        if (!response.Success)
        {
            if (response.isValidationError)
                throw new ApplicationException(response.Errors);
            else
                throw new Exception("Ocorreu uma falha durante o processo.favor contactar o suporte técnico!");
        }

        return response;

    }
    //public virtual void ExecutaPostNoResult<TModel>(string relativePath, TModel ModelApi)
    //{
    //    using (var client = new HttpClient())
    //    {
    //        if (client.BaseAddress == null)
    //        {
    //            client.BaseAddress = new Uri(BaseAddress);
    //            client.Timeout = TimeSpan.FromMinutes(5);
    //            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_Token}");
    //        }

    //        var content = new StringContent(JsonConvert.SerializeObject(ModelApi),
    //                                        Encoding.UTF8,
    //                                        "application/json");


    //        HttpResponseMessage response = client.PostAsync(relativePath, content).Result;
    //        response.EnsureSuccessStatusCode();


    //    }
    //}
    //public virtual IApiResponse<TResult> ExecutaPostNoParam<TResult>(string relativePath)
    //{
    //    using (var client = new HttpClient())
    //    {
    //        if (client.BaseAddress == null)
    //        {
    //            client.BaseAddress = new Uri(BaseAddress);
    //            client.Timeout = TimeSpan.FromMinutes(5);
    //            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_Token}");
    //        }

    //        var response = new ApiResponse<TResult>(client.PostAsync(relativePath, null).Result);

    //        if (!response.Success)
    //            throw new Exception(string.Join(Environment.NewLine, response.Errors));

    //        return response;
    //    }
    //}

    public virtual string GetBaseAddress(string baseAddress, string servidor)
    {
        //Verifica se está no modo debug, caso positivo não faz nada.
        if (baseAddress.IndexOf("localhost") > 0)
        {
            return baseAddress;
        }

        if (servidor.ToUpper() == "NETUNO")
        {
            baseAddress = @"https://www3.tln.com.br/API/NetCard/" + baseAddress;
        }
        else
        {
            baseAddress = @"https://www1.tln.com.br/API/NetCard/" + baseAddress;
        }

        return baseAddress;
    }
}
