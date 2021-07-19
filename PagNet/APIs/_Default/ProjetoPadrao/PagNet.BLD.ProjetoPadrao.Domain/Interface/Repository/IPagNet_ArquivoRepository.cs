using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;
using System;
using System.Collections.Generic;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_ArquivoRepository : IRepositoryBase<PAGNET_ARQUIVO>
    {
        IEnumerable<PAGNET_ARQUIVO> GetFileByDate(DateTime DataInicio, DateTime DataFinal, int codSubRede, string TipArquivo);
        IEnumerable<PAGNET_ARQUIVO> GetRemessaBoleto(DateTime DataInicio, DateTime DataFinal, int codSubRede, string TipArquivo);
        
        int GetMaxKey();
    }
}
