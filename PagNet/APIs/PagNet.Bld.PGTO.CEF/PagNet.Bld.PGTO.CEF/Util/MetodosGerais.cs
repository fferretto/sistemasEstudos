using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services;
using System;

namespace PagNet.Bld.PGTO.CEF.Util
{
    public class MetodosGerais
    {
        private readonly IPAGNET_ARQUIVOService _arquivo;

        public MetodosGerais(IPAGNET_ARQUIVOService arquivo)
        {
            _arquivo = arquivo;
        }


        public string validaMsgRetorno(string codRet, int codArquivo)
        {
            string status = "LIQUIDADO";

            if (codRet.Length == 0)
            {
                var arquivo = _arquivo.ReturnFile(codArquivo).Result;
                status = arquivo.STATUS.Replace("_", " ");
            }
            else
            {
                for (int i = 0; i < 10; i += 2)
                {
                    if (codRet.Length >= (i + 2))
                    {
                        switch (codRet.Substring(i, 2))
                        {
                            case "00":
                                status = "BAIXADO";
                                break;
                            case "03":
                                status = "AGUARDANDO_ARQUIVO_RETORNO";
                                break;
                            case "BD":
                                status = "AGUARDANDO_ARQUIVO_RETORNO";
                                break;
                            case "BE":
                                status = "AGUARDANDO_ARQUIVO_RETORNO";
                                break;
                            case "BF":
                                status = "AGUARDANDO_ARQUIVO_RETORNO";
                                break;
                            default:
                                status = "RECUSADO";
                                break;
                        }

                    }
                }
            }


            return status;
        }
        public int RetornaNroSeqArquivo()
        {
            try
            {
                int ultimoArquivo = _arquivo.GetMaxNroSequencial();

                ultimoArquivo += 1;

                if (ultimoArquivo < 11)
                {
                    ultimoArquivo = 11;
                }

                return ultimoArquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InseriArquivo(string CaminhoArquivo, string nmArquivo, int nroSeq, decimal vlTotalArquivo, int TotalRegistros, string Status, string TipArqu, string CodBanco)
        {
            try
            {
                PAGNET_ARQUIVO arq = new PAGNET_ARQUIVO();
                arq.CAMINHOARQUIVO = CaminhoArquivo;
                arq.DTARQUIVO = DateTime.Now;
                arq.NMARQUIVO = nmArquivo;
                arq.NROSEQARQUIVO = nroSeq;
                arq.VLTOTAL = vlTotalArquivo;
                arq.QTREGISTRO = TotalRegistros;
                arq.STATUS = Status;
                arq.TIPARQUIVO = TipArqu;
                arq.CODBANCO = CodBanco;


                var CodArquivo = _arquivo.IncluiArquivo(arq);

                return CodArquivo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
