using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_ArquivoRepository : RepositoryBase<PAGNET_ARQUIVO>, IPagNet_ArquivoRepository
    {
        public PagNet_ArquivoRepository(ContextNetCard context)
            : base(context)
            { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_ARQUIVO.Select(p => p.CODARQUIVO).DefaultIfEmpty(0).Max();
        }


        public IEnumerable<PAGNET_ARQUIVO> GetFileByDate(DateTime DataInicio, DateTime DataFinal, int codEmpresa, string TipArquivo)
        {
            try
            {
                string dataA = DataInicio.ToShortDateString();
                string dataB = DataFinal.ToShortDateString() + " 23:59:59";

                DateTime dataIni = Convert.ToDateTime(dataA);
                DateTime dataFim = Convert.ToDateTime(dataB);

                var registros = (from Arq in DbNetCard.PAGNET_ARQUIVO
                                 join Pag in DbNetCard.PAGNET_TITULOS_PAGOS on Arq.CODARQUIVO equals Pag.CODARQUIVO
                                 where Arq.DTARQUIVO >= dataIni &&
                                       Arq.DTARQUIVO <= dataFim &&
                                       Arq.STATUS != "CANCELADO" &&
                                       Arq.STATUS != "RECUSADO" &&
                                       Arq.TIPARQUIVO == TipArquivo &&
                                       (codEmpresa == 0 || Pag.CODEMPRESA == codEmpresa)
                                 group Arq by new
                                 {
                                     Arq.CODARQUIVO,
                                     Arq.NMARQUIVO,
                                     Arq.CODBANCO,
                                     Arq.TIPARQUIVO,
                                     Arq.NROSEQARQUIVO,
                                     Arq.DTARQUIVO,
                                     Arq.CAMINHOARQUIVO,
                                     Arq.VLTOTAL,
                                     Arq.QTREGISTRO,
                                     Arq.STATUS
                                 } into g
                                 select new
                                 {
                                     g.Key.CODARQUIVO,
                                     g.Key.NMARQUIVO,
                                     g.Key.CODBANCO,
                                     g.Key.TIPARQUIVO,
                                     g.Key.NROSEQARQUIVO,
                                     g.Key.DTARQUIVO,
                                     g.Key.CAMINHOARQUIVO,
                                     g.Key.VLTOTAL,
                                     g.Key.QTREGISTRO,
                                     g.Key.STATUS
                                 }).ToList().Distinct();

                List<PAGNET_ARQUIVO> arquivos = new List<PAGNET_ARQUIVO>();

                foreach(var linha in registros)
                {
                    PAGNET_ARQUIVO arq = new PAGNET_ARQUIVO();
                    arq.CODARQUIVO = linha.CODARQUIVO;
                    arq.NMARQUIVO = linha.NMARQUIVO;
                    arq.CODBANCO = linha.CODBANCO;
                    arq.TIPARQUIVO = linha.TIPARQUIVO;
                    arq.NROSEQARQUIVO = linha.NROSEQARQUIVO;
                    arq.DTARQUIVO = linha.DTARQUIVO;
                    arq.CAMINHOARQUIVO = linha.CAMINHOARQUIVO;
                    arq.QTREGISTRO = linha.QTREGISTRO;
                    arq.VLTOTAL = linha.VLTOTAL;
                    arq.STATUS = linha.STATUS;

                    arquivos.Add(arq);

                }


                return arquivos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<PAGNET_ARQUIVO> GetRemessaBoleto(DateTime DataInicio, DateTime DataFinal, int codEmpresa, string TipArquivo)
        {
            try
            {
                string dataA = DataInicio.ToShortDateString();
                string dataB = DataFinal.ToShortDateString() + " 23:59:59";

                DateTime dataIni = Convert.ToDateTime(dataA);
                DateTime dataFim = Convert.ToDateTime(dataB);

                var registros = (from Arq in DbNetCard.PAGNET_ARQUIVO
                                 join bol in DbNetCard.PAGNET_EMISSAOBOLETO on Arq.CODARQUIVO equals bol.CODARQUIVO
                                 where Arq.DTARQUIVO >= dataIni &&
                                       Arq.DTARQUIVO <= dataFim &&
                                       Arq.STATUS != "CANCELADO" &&
                                       Arq.STATUS != "RECUSADO" &&
                                       Arq.TIPARQUIVO == TipArquivo &&
                                       (codEmpresa == 0 || bol.codEmpresa == codEmpresa)
                                 group Arq by new
                                 {
                                     Arq.CODARQUIVO,
                                     Arq.NMARQUIVO,
                                     Arq.CODBANCO,
                                     Arq.TIPARQUIVO,
                                     Arq.NROSEQARQUIVO,
                                     Arq.DTARQUIVO,
                                     Arq.CAMINHOARQUIVO,
                                     Arq.VLTOTAL,
                                     Arq.QTREGISTRO,
                                     Arq.STATUS
                                 } into g
                                 select new
                                 {
                                     g.Key.CODARQUIVO,
                                     g.Key.NMARQUIVO,
                                     g.Key.CODBANCO,
                                     g.Key.TIPARQUIVO,
                                     g.Key.NROSEQARQUIVO,
                                     g.Key.DTARQUIVO,
                                     g.Key.CAMINHOARQUIVO,
                                     g.Key.VLTOTAL,
                                     g.Key.QTREGISTRO,
                                     g.Key.STATUS
                                 }).ToList().Distinct();

                List<PAGNET_ARQUIVO> arquivos = new List<PAGNET_ARQUIVO>();

                foreach (var linha in registros)
                {
                    PAGNET_ARQUIVO arq = new PAGNET_ARQUIVO();
                    arq.CODARQUIVO = linha.CODARQUIVO;
                    arq.NMARQUIVO = linha.NMARQUIVO;
                    arq.CODBANCO = linha.CODBANCO;
                    arq.TIPARQUIVO = linha.TIPARQUIVO;
                    arq.NROSEQARQUIVO = linha.NROSEQARQUIVO;
                    arq.DTARQUIVO = linha.DTARQUIVO;
                    arq.CAMINHOARQUIVO = linha.CAMINHOARQUIVO;
                    arq.QTREGISTRO = linha.QTREGISTRO;
                    arq.VLTOTAL = linha.VLTOTAL;
                    arq.STATUS = linha.STATUS;

                    arquivos.Add(arq);
                }


                return arquivos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

