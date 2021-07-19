using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for APIResponse
/// </summary>
public class ApiResponse<TResult> : IApiResponse<TResult>
{
    public ApiResponse(string response)
    {
        _response = response;
        var rawApiResult = JsonConvert.DeserializeObject<RawApiResult>(_response);
        Success = rawApiResult.Status == 200;

        Result = Success ? GetValue() : default(TResult);
        isValidationError = ValidationError();

        if (!Success)
            GetErrors();
    }

    private string _response;

    private TResult GetValue()
    {
        var json = _response;
        var rawApiResult = JsonConvert.DeserializeObject<RawApiResult>(json);

        var jArray = rawApiResult.results as Newtonsoft.Json.Linq.JArray;

        if (rawApiResult.results == null || (jArray != null && !jArray.HasValues))
            return default(TResult);

        return JsonConvert.DeserializeObject<ApiResult<TResult>>(json).results;
    }

    private void GetErrors()
    {
        var json = _response;
        var error = JsonConvert.DeserializeObject<ApiError>(json);
        Errors = error.Details.ToString();
        MoreInfo = error.MoreInfo;
    }
    private bool ValidationError()
    {
        var json = _response;
        var error = JsonConvert.DeserializeObject<ApiError>(json);

        return (error.Status == 409);
    }

    public bool Success { get; set; }
    public TResult Result { get; set; }
    public bool isValidationError { get; set; }
    public string Errors { get; private set; }
    public string MoreInfo { get; private set; }
}

internal class ApiError
{
    public int Status { get; set; }
    public IEnumerable<string> Details { get; set; }
    public string MoreInfo { get; set; }
}

internal class RawApiResult
{
    public int Status { get; set; }
    public object results { get; set; }
}

internal class ApiResult<TResult>
{
    public int Status { get; set; }
    public TResult results { get; set; }
}
