using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_ARQUIVOService : ServiceBase<PAGNET_ARQUIVO>, IPAGNET_ARQUIVOService
    {
        private readonly IPAGNET_ARQUIVORepository _rep;

        public PAGNET_ARQUIVOService(IPAGNET_ARQUIVORepository rep)
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
                int codArquivo = _rep.BuscaProximoID();
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
