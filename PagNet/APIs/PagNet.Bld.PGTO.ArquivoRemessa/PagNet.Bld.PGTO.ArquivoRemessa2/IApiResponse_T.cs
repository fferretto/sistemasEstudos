using System.Collections.Generic;

namespace PagNet.Bld.PGTO.ArquivoRemessa2
{
    public interface IApiResponse<TResult>
    {
        bool Success { get; }
        TResult Result { get; }
        IEnumerable<string> Errors { get; }
        string MoreInfo { get; }
    }
}
