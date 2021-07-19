using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IApiResponse_T
/// </summary>
public interface IApiResponse<TResult>
{
    bool Success { get; }
    TResult Result { get; }
    string Errors { get; }
    string MoreInfo { get; }
}
