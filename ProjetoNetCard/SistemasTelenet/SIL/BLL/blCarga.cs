using SIL.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using TELENET.SIL.PO;
using System.Threading;

namespace TELENET.SIL.BL
{
    public class blCarga
    {        
        OPERADORA FOperador;
        public blCarga(OPERADORA Operador)
        {
            FOperador = Operador;
        } //

        public CARGAAUTO CargaAutoHabilitada()
        {
            return new daCarga(FOperador).CargaAutoHabilitada();
        }
        
        public void LigaDesligaCargaAuto(bool acao)
        {
            new daCarga(FOperador).LigaDesligaCargaAuto(acao);
        }

        public List<CLIENTE_CARGA> PopulaClientesCarga(bool listaCargasAutomaticas)
        {
            return new daCarga(FOperador).ListaCargaNaoFinalizadas(listaCargasAutomaticas);
        }

        public List<LogCarga> GetLogCargas()
        {
            return new daCarga(FOperador).GetLogCargas();
        }

        public void ConfirmaPgtoCarga(string CODCLI, string NUMCARGA, string IDLIBPGTOOPR)
        {
            new daCarga(FOperador).ConfirmaPgtoCarga(CODCLI, NUMCARGA, IDLIBPGTOOPR);
        }

        public void ConfirmaCancelaCarga(string id_Processo)
        {
            new daCarga(FOperador).ConfirmaCancelaCarga(id_Processo);
        }
        
        public List<CARGA_PENDENTE> PopulaCargasSolicitadas()
        {
            return new daCarga(FOperador).PopulaCargasSolicitadas();
        }

        public List<CARGA_AGUAR_LIB_PGTO> PopulaClientesPendPgto()
        {
            return new daCarga(FOperador).ListaCargaAguardandoPagamento();
        }

        public List<CARGAC_VA> ListaCargaSolicitadas(Hashtable filtros)
        {
            if (filtros == null)
                return null;

            string Selecao = filtros["Selecao"].ToString();
            string Tipo = filtros["TipoRelatorio"].ToString();
            string ParamIni = string.Empty;
            if (!string.IsNullOrEmpty(filtros["ParamIni"].ToString()))
                ParamIni = filtros["ParamIni"].ToString();
            else
                ParamIni = null;
            string ParamFim = string.Empty;
            if (!string.IsNullOrEmpty(filtros["ParamFim"].ToString()))
                ParamFim = filtros["ParamFim"].ToString();
            else
                ParamFim = null;

            List<CARGAC_VA> lista = new daCarga(FOperador).ListaCarga_VA(Selecao, Tipo, ParamIni, ParamFim);

            if (Tipo == "0")//Sintetico
            {
                List<CARGAC_VA> listaAux = new List<CARGAC_VA>();
                int aux = 0, qtdUsu = 0, index = 0;
                decimal valTotCarga = 0m;

                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i].CODCLI != aux)
                    {
                        if (listaAux.Count > 0)
                        {
                            listaAux[index].QTDUSU = qtdUsu;
                            listaAux[index].VCARGAUTO = valTotCarga;
                            index++;
                            qtdUsu = 0;
                            valTotCarga = 0;
                        }

                        qtdUsu++;
                        valTotCarga += lista[i].VCARGAUTO;

                        listaAux.Add(lista[i]);
                        aux = lista[i].CODCLI;
                    }
                    else
                    {
                        qtdUsu++;
                        valTotCarga += lista[i].VCARGAUTO;
                    }
                }

                if (listaAux.Count > 0)
                {
                    listaAux[index].QTDUSU = qtdUsu;
                    listaAux[index].VCARGAUTO = valTotCarga;
                }

                return listaAux;
            }

            return lista;
        }

        public List<CARGA_EFETUADAS> ListaCargaEfetuadas(Hashtable filtros)
        {
            if (filtros == null)
                return null;

            string Tipo = filtros["TipoRelatorio"].ToString();
            
            List<CARGA_EFETUADAS> lista = new daCarga(FOperador).ListaCargaEfetuadas(filtros);
            List<CARGA_EFETUADAS> listaAux = new List<CARGA_EFETUADAS>() ;

            if (Tipo == "1")//Analitico
            {
                int  qtdUsu = 0, auxCodCli = 0;
                double totalPorCliente = 0;

                if (lista.Count > 0)
                {
                    auxCodCli = lista[0].CODCLI;
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (auxCodCli == lista[i].CODCLI)
                        {
                            qtdUsu++;
                            listaAux.Add(lista[i]);
                            totalPorCliente = lista[i].TAXSER;

                        }
                        else
                        {
                            foreach (CARGA_EFETUADAS c in listaAux)
                                c.TAXAADMPORCLIENTE = totalPorCliente / qtdUsu;

                            qtdUsu = 1;
                            auxCodCli = lista[i].CODCLI;
                            listaAux.Clear();
                            listaAux.Add(lista[i]);
                        }
                    }

                    foreach (CARGA_EFETUADAS c in listaAux)
                        c.TAXAADMPORCLIENTE = totalPorCliente / qtdUsu;
                }
            }

            return lista;
        }

        public void CancelaCarga(CLIENTE_CARGA Cliente_Carga)
        {
            new daCarga(FOperador).CancelaCargaCliente(Cliente_Carga);
        }

        public void CancelaProgramacao(CLIENTE_CARGA Cliente_Carga)
        {
            new daCarga(FOperador).CancelaProgramacaoCarga(Cliente_Carga);
        }

        public void GeraProgramacao(CLIENTE_CARGA Cliente_Carga)
        {
            new daCarga(FOperador).GeraProgramacao(Cliente_Carga);
        }

        public LOG EfetuaCarga(List<CLIENTE_CARGA> ColecaoClienteCarga)
        {
            LOG log = new LOG();
            daCarga carga = new daCarga(FOperador);
            foreach (CLIENTE_CARGA Cliente_Carga in ColecaoClienteCarga)
            {
                if (Cliente_Carga.CARREGA)
                {
                    carga.EfetuaCarga(Cliente_Carga.CODCLI, Cliente_Carga.NUMCARGA, log, FOperador.ID_FUNC);                    
                }
            }
            return log;            
        }

        public LOG RetornaLog(List<CLIENTE_CARGA> cargasEfetuadas, LOG log)
        {
            var daCarga = new daCarga(FOperador);
            foreach (var carga in cargasEfetuadas)
            {
                daCarga.RetornaLog(carga.CODCLI, carga.NUMCARGA, log);                
            }            
            return log;
        }

        public void LiberaCargaCliente(int codCLi, int numCarga, DateTime dtAutoriz, int idFun)
        {
            new daCarga(FOperador).LiberaCargaCliente(codCLi, numCarga, dtAutoriz, idFun);
            new daCarga(FOperador).LiberaDataProg(codCLi, numCarga, dtAutoriz);
        }

        public void LiberaCargaVencida(int codCLi, int numCarga, DateTime dtAutoriz, int idFun)
        {
            new daCarga(FOperador).LiberaCargaVencida(codCLi, numCarga, dtAutoriz, idFun);            
        }

        public void BloqueiaCargaCliente(int codCLi, int numCarga, DateTime dtAutoriz)
        {
            new daCarga(FOperador).BloqueiaCargaCliente(codCLi, numCarga, dtAutoriz);
        }

        public bool VerificaValCarga(int codCli, int numCarga)
        {
            return new daCarga(FOperador).VerificaValCarga(codCli, numCarga);
        }

        public string GetPastaArquivosCarga()
        {
            return new daCarga(FOperador).GetPastaArquivosCarga();
        }

    }
}
