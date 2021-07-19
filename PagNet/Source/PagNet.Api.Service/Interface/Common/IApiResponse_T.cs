using System.Collections.Generic;

namespace PagNet.Api.Service.Interface.Common
{
    public interface IApiResponse<TResult>
    {
        bool Success { get; }
        TResult Result { get; }
        IEnumerable<string> Errors { get; }
        string MoreInfo { get; }
    }
}
