using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;
using System;
using System.Collections.Generic;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_ARQUIVORepository : IRepositoryBase<PAGNET_ARQUIVO>
    {
        IEnumerable<PAGNET_ARQUIVO> GetFileByDate(DateTime DataInicio, DateTime DataFinal, int codSubRede, string TipArquivo);
        IEnumerable<PAGNET_ARQUIVO> GetRemessaBoleto(DateTime DataInicio, DateTime DataFinal, int codSubRede, string TipArquivo);
        
        int BuscaProximoID();
    }
}
