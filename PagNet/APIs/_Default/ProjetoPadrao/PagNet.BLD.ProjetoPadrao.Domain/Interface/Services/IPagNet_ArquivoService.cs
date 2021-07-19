using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_ArquivoService : IServiceBase<PAGNET_ARQUIVO>
    {
        int GetMaxNroSequencial();
        int IncluiArquivo(PAGNET_ARQUIVO arq);
        Task<PAGNET_ARQUIVO> ReturnFile(int CodCarquivo);
        Task<PAGNET_ARQUIVO> ReturnFileByName(string nmArquivo);
        Task<PAGNET_ARQUIVO> ReturnFileById(int codArquivo);
        void AtualizaArquivo(PAGNET_ARQUIVO arq);
        IEnumerable<PAGNET_ARQUIVO> GetFileByDate(DateTime DataInicio, DateTime DataFinal, int codSubRede, string TipArquivo);
        
    }
}
