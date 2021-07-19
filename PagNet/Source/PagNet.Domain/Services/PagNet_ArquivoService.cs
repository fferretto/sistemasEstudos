using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Domain.Services
{
    public class PagNet_ArquivoService : ServiceBase<PAGNET_ARQUIVO>, IPagNet_ArquivoService
    {
        private readonly IPagNet_ArquivoRepository _rep;

        public PagNet_ArquivoService(IPagNet_ArquivoRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public void AtualizaArquivo(PAGNET_ARQUIVO arq)
        {
            try
            {
                _rep.Update(arq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetMaxNroSequencial()
        {
            try
            {
                var ret = _rep.Get().OrderByDescending(x => x.NROSEQARQUIVO).Take(1).FirstOrDefault();

                if (ret == null) return 0;

                return Convert.ToInt32(ret.NROSEQARQUIVO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int IncluiArquivo(PAGNET_ARQUIVO arq)
        {
            try
            {
                int codArquivo = _rep.GetMaxKey();
                arq.CODARQUIVO = codArquivo;

                _rep.Add(arq);

                return arq.CODARQUIVO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PAGNET_ARQUIVO> ReturnFile (int CodCarquivo)
        {
            try
            {
                var arquivo = _rep.Get(x => x.CODARQUIVO == CodCarquivo).FirstOrDefault();


                return arquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<PAGNET_ARQUIVO> GetFileByDate(DateTime DataInicio, DateTime DataFinal, int codEmpresa, string TipArquivo)
        {
            try
            {
                
                if (TipArquivo == "PAG")
                    return _rep.GetFileByDate(DataInicio, DataFinal, codEmpresa, TipArquivo);
                else
                    return _rep.GetRemessaBoleto(DataInicio, DataFinal, codEmpresa, TipArquivo);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PAGNET_ARQUIVO> ReturnFileById(int codArquivo)
        {
            try
            {
                var arquivo = _rep.Get(x => x.CODARQUIVO == codArquivo).FirstOrDefault();

                return arquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PAGNET_ARQUIVO> ReturnFileByName(string nmArquivo)
        {
            try
            {
                var arquivo = _rep.Get(x => x.NMARQUIVO == nmArquivo).FirstOrDefault();


                return arquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
