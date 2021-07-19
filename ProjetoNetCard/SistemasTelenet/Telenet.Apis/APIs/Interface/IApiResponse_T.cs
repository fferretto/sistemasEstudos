using System.Collections.Generic;

namespace Telenet.Apis.APIs.Interface
{
    public interface IApiResponse<TResult>
    {
        bool Success { get; }
        TResult Result { get; }
        IEnumerable<string> Errors { get; }
        string MoreInfo { get; }
    }
}
