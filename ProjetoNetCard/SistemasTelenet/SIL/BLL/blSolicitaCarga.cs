using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TELENET.SIL.PO;
using SIL.DAL;
using TELENET.SIL;
using System.Globalization;
using TELENET.SIL.BL;
using System.Data;
using System.IO;
using System.Configuration;

namespace SIL.BLL
{
    public class blSolicitaCarga
    {
        readonly OPERADORA _fOperador;
        public blSolicitaCarga(OPERADORA operador)
        {
            _fOperador = operador;
        }

        public int GetNumUltimaCarga(int codCli)
        {
            return new daSolicitaCarga(_fOperador).GetUltimaCarga(codCli);
        }

        private void CorrigeTabelaCargaCentroCusto(int codCliInt, int numCarga)
        {
            var da = new daSolicitaCarga(_fOperador);
            da.CorrigeTabelaCargaCentroCusto(codCliInt, numCarga);//Corrigo a tabela deletando os registros desnecessarios para centro de custo
        }

        public void TransferenciaSaldo(int tipoTransf, int codCli, string cpfOrig, string cpfDest,
            string crtOrig, string crtDest, decimal valor)
        {
            new daSolicitaCarga(_fOperador).TransferenciaSaldo(tipoTransf, codCli, cpfOrig, cpfDest,
            crtOrig, crtDest, valor, _fOperador.ID_FUNC, _fOperador.BANCOAUT);
        }

        public void TransferenciaUsuario(int codCliOrigem, int CodcliDestino, string cpfOrigem, out string retorno, out string mensagem)
        {
            new daSolicitaCarga(_fOperador).TransferenciaUsuario(codCliOrigem, CodcliDestino, cpfOrigem, _fOperador.ID_FUNC, out retorno, out mensagem);
        }

        #region Via Arquivo

        public LOG VerificaCpfStatusArquivo(List<string> linhas)
        {
            var da = new daSolicitaCarga(_fOperador);
            var linhaHeader = linhas[0];//Primeira Linha
            var codCli = linhaHeader.Substring(134, 5);//Codigo Cliente
            var log = new LOG();
            var i = 2;
            foreach (var usu in from linha in linhas where linha != linhas[0] where linha != linhas[linhas.Count - 1] select new USUARIO_VA { CODCLI = codCli, CPF = linha.Substring(17, 11).Trim() })
            {
                string cargPadVa;
                //Se o usuario existe e o status nao esta ativo
                if (da.VerificaUsuario(usu, out cargPadVa, false) && usu.STA != "00")
                    log.AddLog(new LOG("LINHA " + Convert.ToString(i).PadLeft(5), "CLIENTE " + usu.CODCLI.PadLeft(6, ' ') + " CPF " + usu.CPF,
                                       "CARTAO NAO PODE SER PROCESSADO. STATUS INVALIDO. CARTAO = " + usu.STASTR));
                //Consisto o CPF
                if (!UtilSIL.ValidarCpf(usu.CPF))
                    log.AddLog(new LOG("LINHA " + Convert.ToString(i).PadLeft(5), "CLIENTE " + usu.CODCLI.PadLeft(6, ' ') + " CPF " + usu.CPF,
                                       "CARTAO NAO PODE SER PROCESSADO. CPF INVALIDO."));
                i++;
            }
            return log;
        }

        public LOG VerificaCpf(List<string> linhas, string codCli)
        {
            var da = new daSolicitaCarga(_fOperador);
            var log = new LOG();
            var i = 2;
            foreach (var usu in from linha in linhas select new USUARIO_VA { CODCLI = codCli, CPF = linha.Substring(0, 11).Trim() })
            {
                //Consisto o CPF
                if (!UtilSIL.ValidarCpf(usu.CPF))
                    log.AddLog(new LOG("LINHA " + Convert.ToString(i).PadLeft(5, '0'), "CLIENTE " + usu.CODCLI.PadLeft(6, ' ') + " CPF " + usu.CPF,
                                       "CANCELAMENTO NAO PODE SER PROCESSADO. CPF INVALIDO."));
                else
                    if (!da.VerificaUsuarioPJ(usu))
                        log.AddLog(new LOG("LINHA " + Convert.ToString(i).PadLeft(5, '0'), "CLIENTE " + usu.CODCLI.PadLeft(6, ' ') + " CPF " + usu.CPF,
                                           "CANCELAMENTO NAO PODE SER PROCESSADO. USUARIO NAO CADASTRADO."));
                    else
                        //Se o usuario existe e o status nao esta ativo
                        if (da.VerificaUsuarioPJ(usu) && usu.STA == "02")
                            log.AddLog(new LOG("LINHA " + Convert.ToString(i).PadLeft(5, '0'), "CLIENTE " + usu.CODCLI.PadLeft(6, ' ') + " CPF " + usu.CPF,
                                               "CANCELAMENTO NAO PODE SER PROCESSADO. STATUS DO USUARIO JA ESTA CANCELADO. STATUS = " +
                                               usu.STASTR));
                i++;
            }
            return log;
        }

        private static string MontaDataProg(string linhaHeader)
        {
            DateTime? dataProg = null;
            var dia = linhaHeader.Substring(17, 2);
            var mes = linhaHeader.Substring(19, 2);
            var ano = linhaHeader.Substring(21, 4);

            if (!string.IsNullOrEmpty(dia.Trim()) && !string.IsNullOrEmpty(mes.Trim()) && !string.IsNullOrEmpty(ano.Trim()))
            {
                int diaInt;
                int.TryParse(dia, out diaInt);
                int mesInt;
                int.TryParse(mes, out mesInt);
                int anoInt;
                int.TryParse(ano, out anoInt);
                dataProg = new DateTime(anoInt, mesInt, diaInt);
            }
            return dataProg != null ? dataProg.Value.ToShortDateString() : string.Empty;
        }

        public LOG VerificaHeaderTrilerArquivo(List<string> linhas, string codCli)
        {
            string layout = string.Empty;

            var log = new LOG();
            if (linhas.Count < 1 || linhas[0].Length < 158)
            {
                log.AddLog(new LOG("ERRO GERAL ", "ERRO GERAL NO ARQUIVO", "ARQUIVO INVALIDO"));
                return log;
            }
            //Primeira Linha
            var linhaHeader = linhas[0];
            //Ultima Linha           
            var linhaTrailer = linhas[linhas.Count - 1];
            var numLinhaTrailer = (linhas.Count - 1).ToString().PadLeft(5, '0');

            layout = linhaHeader.Substring(54, 2);

            if (linhaHeader.Substring(0, 2) != "AA")
                log.AddLog(new LOG("LINHA 00001", "ARQUIVO INVALIDO", "ID DO ARQUIVO INVALIDO NO HEADER. ARQUIVO NAO FOI PROCESSADO"));

            if (linhaHeader.Substring(43, 1) != "6")
                log.AddLog(new LOG("LINHA 00001", "ARQUIVO INVALIDO", "TIPO DO ARQUIVO INVALIDO NO HEADER. ARQUIVO NAO FOI PROCESSADO"));

            if (linhaHeader.Substring(44, 9) != "CARGA    " &&
                linhaHeader.Substring(44, 9) != "CARGAVA  ")
                log.AddLog(new LOG("LINHA 00001", "ARQUIVO INVALIDO", "ID DO TIPO DO ARQUIVO INVALIDO NO HEADER. ARQUIVO NAO FOI PROCESSADO"));

            if (layout != "01" && layout != "02" && layout != "03" && layout != "04" && layout != "05" && layout != "06" && layout != "07")
                log.AddLog(new LOG("LINHA 00001", "ARQUIVO INVALIDO", "LAYOUT DO ARQUIVO INVALIDO NO HEADER. ARQUIVO NAO FOI PROCESSADO"));

            if (linhaHeader.Substring(153, 5) != "00001")
                log.AddLog(new LOG("LINHA 00001", "ARQUIVO INVALIDO", "NUMERO SEQUENCIAL DA LINHA INVALIDO NO HEADER. ARQUIVO NAO FOI PROCESSADO"));

            if (codCli != string.Empty)
            {
                int codCliHeaderInt;
                int.TryParse(linhaHeader.Substring(134, 5), out codCliHeaderInt);
                var codCliHeader = Convert.ToString(codCliHeaderInt).PadLeft(5, '0');
                if (codCli.PadLeft(5, '0') != codCliHeader)
                    log.AddLog(new LOG("LINHA 00001", "ARQUIVO INVALIDO", "CODIGO DO CLIENTE DIFERENTE NO HEADER"));
                int numCargaArq;
                int.TryParse(linhaHeader.Substring(2, 4), out numCargaArq);
                var numCarga = new daSolicitaCarga(_fOperador).GetUltimaCarga(Convert.ToInt32(codCli));
                if (numCargaArq != numCarga)
                    log.AddLog(new LOG("LINHA 00001", "ARQUIVO INVALIDO", "NUMERO SEQUENCIAL DA CARGA INVALIDO - CARGA INFORMADA: " +
                        Convert.ToString(numCargaArq) + " NUMERO SEQUENCIAL ESPERADO: " + Convert.ToString(numCarga)));
            }

            var i = 0;
            foreach (var linha in linhas)
            {
                if (i > 0)
                {
                    if (linha.Substring(153, 5) != Convert.ToString(i + 1).PadLeft(5, '0'))
                    {
                        log.AddLog(new LOG("LINHA " + Convert.ToString(i + 1).PadLeft(5, '0'), "ARQUIVO INVALIDO",
                                           "NUMERO SEQUENCIAL INCORRETO APOS A LINHA " +
                                           Convert.ToString(i).PadLeft(5, '0')));
                        break;
                    }
                }
                i++;
            }

            if (linhaTrailer.Substring(0, 2) != "ZZ")
                log.AddLog(new LOG("LINHA " + numLinhaTrailer, "ARQUIVO INVALIDO", "IDENTIFICADOR INVALIDO NO TRAILER"));

            var totalCargaTrailerStr = linhaTrailer.Substring(24, 14); //Total da carga no trailer
            decimal totalCargaTrailer;
            decimal.TryParse(totalCargaTrailerStr, out totalCargaTrailer); //Total carga em decimal
            var totalGeralPorCartao = 0m;

            linhas.ForEach(l =>
            {
                if (l == linhas[0] || l == linhas[linhas.Count - 1]) return;
                decimal totalPorCartao;
                if (decimal.TryParse(l.Substring(70, 9).Trim(), NumberStyles.Currency, new CultureInfo("pt-BR"), out totalPorCartao))
                    totalGeralPorCartao += totalPorCartao;
            });

            if (totalCargaTrailer / 100 != totalGeralPorCartao / 100)
                log.AddLog(new LOG("LINHA " + numLinhaTrailer, "ARQUIVO INVALIDO", "IDENTIFICACAO DO VALOR TOTAL DA CARGA INVALIDO NO TRAILER."));

            var numRegs = linhas.Count - 2;
            var regsStr = linhaTrailer.Substring(17, 7);
            int regs;
            int.TryParse(regsStr, out regs);
            if (numRegs != regs)
                log.AddLog(new LOG("LINHA " + numLinhaTrailer, "ARQUIVO INVALIDO", "IDENTIFICACAO DA QUANTIDADE DE CARTOES INVALIDA NO TRAILER"));

            //CNPJ
            var cnpj = linhaHeader.Substring(139, 14).Trim();
            if (string.IsNullOrEmpty(cnpj))
                log.AddLog(new LOG("LINHA 00001", "ARQUIVO INVALIDO", "O ARQUIVO NAO POSSUI UM CNPJ DO CLIENTE VALIDO"));

            //DATA PROGRAMACAO
            var dataProg = MontaDataProg(linhaHeader);
            if (dataProg != string.Empty)
            {
                try
                {
                    DateTime? data = Convert.ToDateTime(dataProg);
                    if (data.Value.Ticks <= DateTime.Now.Ticks)
                        log.AddLog(new LOG("LINHA 00001", "ARQUIVO INVALIDO", "ARQUIVO NAO FOI PROCESSADO! DATA DA PROGRAMACAO INVALIDA NO HEADER. DATA INFORMADA NAO PODE SER MENOR OU IGUAL A DATA ATUAL"));
                }
                catch
                {
                    log.AddLog(new LOG("LINHA 00001", "ARQUIVO INVALIDO", "DATA DA PROGRAMACAO COM O FORMATO INVALIDO NO HEADER."));
                }
            }
            return log;
        }

        int _contador = 2;
        int _inclusao;

        public LOG ProcessaCargaArquivo(List<string> linhas, int idOper, int codOpeSis, bool validaCPFTela)
        {
            var linhaHeader = linhas[0]; //Primeira Linha
            var linhaTrailer = linhas[linhas.Count - 1]; //Ultima Linha           
            int codCliInt, numSeqInt;
            decimal totalCargaDecimal;
            var codCli = linhaHeader.Substring(134, 5);
            var numSeq = linhaHeader.Substring(2, 4);
            var totalCarga = linhaTrailer.Substring(24, 14);
            int.TryParse(codCli, out codCliInt);
            int.TryParse(numSeq, out numSeqInt);
            decimal.TryParse(totalCarga, out totalCargaDecimal);
            totalCargaDecimal = totalCargaDecimal / 100;
            var cnpj = linhaHeader.Substring(139, 14).Trim();
            var layout = linhaHeader.Substring(54, 2);
            var log = new LOG();
            var da = new daSolicitaCarga(_fOperador);

            var cargaAndamento = da.ExisteCargaAndamento(codCliInt, numSeqInt);
            bool acessar;
            using (new Mutex(true, codCli + numSeq, out acessar))


                if (layout != "07")
                {
                    #region *** Layout até 06 ***

                    if (!acessar || cargaAndamento)
                    {
                        log.AddLog(new LOG("ERRO GERAL ", "CARGA EM ANDAMENTO",
                                           "Já existe um processo de solicitação para esta mesma carga.Aguarde alguns minutos e tente novamente"));
                    }
                    else
                    {
                        if (log.getArquivoLog().Count == 0)
                        {
                            var dadosVerificaCargaCli = da.VerificaCargaCliente(codCliInt, numSeqInt, totalCargaDecimal,
                                                                                MontaDataProg(linhaHeader), cnpj, idOper,
                                                                                codOpeSis,
                                                                                1);
                            if (dadosVerificaCargaCli[0] != "OK")
                                log.AddLog(new LOG("ERRO GERAL ", "ARQUIVO INVALIDO", dadosVerificaCargaCli[0]));
                            else
                            {
                                USUARIO_VA usu = null;
                                var count = 1;
                                var tentativasCartao = 1;
                                var erroPorCartao = false;

                                #region Loop para carga de cada usuario

                                foreach (var linha in linhas.Where(linha => linha != linhas[0]).Where(linha => linha != linhas[linhas.Count - 1]))
                                {
                                    while (tentativasCartao < 6)
                                    {
                                        try
                                        {
                                            erroPorCartao = false;
                                            count++;
                                            usu = new USUARIO_VA
                                                {
                                                    CODCLI = dadosVerificaCargaCli[5],
                                                    NUMCARGA = numSeqInt,
                                                    NUMPAC = dadosVerificaCargaCli[1],
                                                    VALADES = dadosVerificaCargaCli[2],
                                                    DTVALCART = System.DateTime.Today.AddMonths(Convert.ToInt16(dadosVerificaCargaCli[4])).ToString("yyMM"),
                                                    CPF = linha.Substring(17, 11).Trim(),
                                                    NOMUSU = UtilSIL.RemoverAcentos(linha.Substring(31, 35).ToUpper().Trim()),
                                                    MAT = linha.Substring(7, 10).Trim(),
                                                    CODFIL = linha.Substring(2, 2).Trim(),
                                                    CODSET =
                                                        layout != "06"
                                                            ? linha.Substring(4, 3).Trim()
                                                            : linha.Substring(108, 10).Trim(),
                                                    CARGPADVA =
                                                        linha.Substring(70, 9).Trim() != string.Empty
                                                            ? Convert.ToDecimal(linha.Substring(70, 9)) / 100
                                                            : 0m,
                                                    STA = "00",
                                                    NUMDEP = linha.Substring(66, 2),
                                                    CARGPADVACLIENTE =
                                                        string.IsNullOrEmpty(
                                                            dadosVerificaCargaCli[6].ToString(CultureInfo.InvariantCulture))
                                                            ? 0
                                                            : Convert.ToDecimal(dadosVerificaCargaCli[6]),
                                                    CENTROCUSTO = linha.Substring(88, 20).Trim(),
                                                    TIPO_LAYOUT = layout,
                                                };

                                            //Verificacao necessaria para totalizar os cartoes por cpf devido as listagens com centro de custo
                                            if (usu.TIPO_LAYOUT == "05" || usu.TIPO_LAYOUT == "06")
                                            {
                                                foreach (var t in linhas)
                                                {
                                                    if (t == linhas[0] || t == linhas[linhas.Count - 1])
                                                        continue;
                                                    if (usu.CPF == t.Substring(17, 11).Trim() &&
                                                        usu.NUMDEP == linha.Substring(66, 2))
                                                        usu.TOTALCARGAUSUARIO += t.Substring(70, 9).Trim() !=
                                                                                 string.Empty
                                                                                     ? Convert.ToDecimal(t.Substring(
                                                                                         70, 9)) / 100
                                                                                     : 0m;
                                                }
                                            }
                                            else
                                                usu.TOTALCARGAUSUARIO = usu.CARGPADVA;

                                            if (!string.IsNullOrEmpty(usu.CPF.Trim()))
                                            {
                                                //usu.TRILHA2 = aux.MontaTrilha(1, 1);
                                                //bool b = usu.TRILHA2.Contains("D");
                                                //usu.CODCRT = usu.TRILHA2.Substring(0, b ? usu.TRILHA2.IndexOf('D') : 17);
                                                //Aciona o metodo que inicia a solicitacao de cargas
                                                int inclusao;
                                                _contador = da.SolicitaCargaUsuario(usu, log, ref erroPorCartao, validaCPFTela, _contador, codOpeSis, out inclusao, "06");
                                                _inclusao += inclusao;
                                            }
                                            else
                                            {
                                                erroPorCartao = true;
                                                log.AddLog(new LOG("LINHA " + count.ToString().PadLeft(5, '0'),
                                                                   "CLIENTE " + usu.CODCLI + " CPF " + usu.CPF,
                                                                   "CPF NAO FOI ENCONTRANDO"));
                                            }
                                            break;
                                        }
                                        catch
                                        {
                                            erroPorCartao = true;
                                            if (usu != null)
                                            {                      
                                                count--;
                                                tentativasCartao++;
                                                using (var outfile = new StreamWriter(ConfigurationManager.AppSettings["PastaArquivos"].ToString() + "\\Carga" + usu.CODCLI + "_" + usu.CPF + "_" + usu.NUMCARGA + ".txt"))
                                                {
                                                    outfile.Write(usu.CODCLI + "_" + usu.CPF + "_" + usu.NUMCARGA + "erro: " + tentativasCartao);
                                                }
                                                Thread.Sleep(1000);
                                                if (tentativasCartao >= 6)
                                                {
                                                    log.AddLog(new LOG("OBSERVACAO ", "PROCESSO CONCLUIDO", "ERRO AO PROCESSAR A CARGA. FAVOR ENTRAR EM CONTATO COM SUPORTE."));
                                                    return log;
                                                    //log.AddLog(new LOG("LINHA " + count.ToString().PadLeft(5, '0'),
                                                    //                   "CLIENTE " + usu.CODCLI + " CPF " + usu.CPF,
                                                    //                   "ERRO AO PROCESSAR O CARTAO. FAVOR ENTRAR EM CONTATO COM SUPORTE"));
                                                }
                                            }
                                        }
                                    }
                                }

                                #endregion

                                if (_inclusao > 0)
                                    log.AddLog(_inclusao == 1
                                                   ? new LOG("OBSERVACAO ", "FOI INCLUIDO " + _inclusao + " CARTAO", "")
                                                   : new LOG("OBSERVACAO ", "FORAM INCLUIDOS " + _inclusao + " CARTOES", ""));

                                //Ultimo registro de log para identificar que o processo ocorreu com sucesso total ou possui cartoes nao processados
                                log.AddLog(!erroPorCartao
                                               ? new LOG("OBSERVACAO ", "PROCESSO CONCLUIDO", "PROCESSO CONCLUIDO. ARQUIVO DE LOG DISPONIBILIZADO PARA VERIFICACAO.")
                                               : new LOG("OBSERVACAO ", "PROCESSO CONCLUIDO", "PROCESSO CONCLUIDO. POREM ALGUNS CARTOES NAO FORAM PROCESSADOS. ARQUIVO DE LOG DISPONIBILIZADO PARA VERIFICACAO."));

                                //if (layout == "05" || layout == "06")
                                //Corrijo a tabela de centro de custo para os arquivos com layout 05 (que possuem centro de custo)
                                da.SolicitaCargaUsuario(codCliInt, numSeqInt, DateTime.Now);
                                CorrigeTabelaCargaCentroCusto(codCliInt, numSeqInt);
                                da.BloqueiaSaldoContacliente(codCliInt, numSeqInt);
                            }
                        }
                    }

                    #endregion
                }
                //else
                //{
                //    #region *** Layout 07 ***

                //    LOG erroCadastrarUsuario = new LOG();

                //    if (!acessar || cargaAndamento)
                //    {
                //        log.AddLog(new LOG("ERRO GERAL ", "CARGA EM ANDAMENTO",
                //                           "Já existe um processo de solicitação para esta mesma carga.Aguarde alguns minutos e tente novamente"));
                //    }
                //    else if (!da.VerificaCargaArquivoLayout7(linhas, ref erroCadastrarUsuario))
                //    {
                //        log.AddLog(erroCadastrarUsuario);
                //    }
                //    else
                //    {
                //        if (log.getArquivoLog().Count == 0)
                //        {
                //            var dadosVerificaCargaCli = da.VerificaCargaCliente(codCliInt, numSeqInt, totalCargaDecimal,
                //                                                                MontaDataProg(linhaHeader), cnpj, idOper,
                //                                                                codOpeSis,
                //                                                                1);
                //            if (dadosVerificaCargaCli[0] != "OK")
                //                log.AddLog(new LOG("ERRO GERAL ", "ARQUIVO INVALIDO", dadosVerificaCargaCli[0]));
                //            else
                //            {
                //                USUARIO_VA usu = null;
                //                var count = 1;
                //                var tentativasCartao = 1;
                //                var erroPorCartao = false;
                //                DateTime datavalidade = new DateTime();

                //                #region Loop para carga de cada usuario

                //                foreach (var linha in linhas.Where(linha => linha != linhas[0]).Where(linha => linha != linhas[linhas.Count - 1]))
                //                {
                //                    datavalidade = Convert.ToDateTime(linha.Substring(135, 2) + "/" + linha.Substring(137, 2) + "/" + linha.Substring(139, 4));

                //                    while (tentativasCartao < 6)
                //                    {
                //                        try
                //                        {
                //                            count++;
                //                            usu = new USUARIO_VA
                //                            {
                //                                CODCLI = dadosVerificaCargaCli[5],
                //                                NUMCARGA = numSeqInt,
                //                                NUMPAC = dadosVerificaCargaCli[1],
                //                                VALADES = dadosVerificaCargaCli[2],
                //                                DTVALCART = datavalidade.Year.ToString().Substring(2, 2) + datavalidade.Month.ToString().PadLeft(2, '0'),
                //                                CPF = linha.Substring(17, 11).Trim(),
                //                                NOMUSU = UtilSIL.RemoverAcentos(linha.Substring(31, 35).ToUpper().Trim()),
                //                                MAT = linha.Substring(7, 10).Trim(),
                //                                CODFIL = linha.Substring(143, 4).Trim(),
                //                                CODSET = linha.Substring(101, 23).Trim(),
                //                                CARGPADVA =
                //                                    linha.Substring(70, 9).Trim() != string.Empty
                //                                        ? Convert.ToDecimal(linha.Substring(70, 9)) / 100
                //                                        : 0m,
                //                                STA = "00",
                //                                NUMDEP = linha.Substring(66, 2),
                //                                CARGPADVACLIENTE =
                //                                    string.IsNullOrEmpty(
                //                                        dadosVerificaCargaCli[6].ToString(CultureInfo.InvariantCulture))
                //                                        ? 0
                //                                        : Convert.ToDecimal(dadosVerificaCargaCli[6]),
                //                                CENTROCUSTO = linha.Substring(81, 20).Trim(),
                //                                TIPO_LAYOUT = layout,
                //                                GENERICO = linha.Substring(129, 6).Trim(),
                //                            };

                //                            usu.TOTALCARGAUSUARIO = usu.CARGPADVA;
                //                            if (!string.IsNullOrEmpty(usu.CPF.Trim()))
                //                            {
                //                                int inclusao;
                //                                _contador = da.SolicitaCargaUsuario(usu, log, ref erroPorCartao, validaCPFTela, _contador, idOper, out inclusao, "07");
                //                                _inclusao += inclusao;
                //                            }
                //                            else
                //                            {
                //                                erroPorCartao = true;
                //                                log.AddLog(new LOG("LINHA " + count.ToString().PadLeft(5, '0'),
                //                                                   "CLIENTE " + usu.CODCLI + " CPF " + usu.CPF,
                //                                                   "CPF NAO FOI ENCONTRANDO"));
                //                            }
                //                            break;
                //                        }
                //                        catch
                //                        {
                //                            erroPorCartao = true;
                //                            if (usu != null)
                //                            {
                //                                count--;
                //                                tentativasCartao++;
                //                                using (var outfile = new StreamWriter(ConfigurationManager.AppSettings["PastaArquivos"].ToString() + "\\Carga" + usu.CODCLI + "_" + usu.CPF + "_" + usu.NUMCARGA + ".txt"))
                //                                {
                //                                    outfile.Write(usu.CODCLI + "_" + usu.CPF + "_" + usu.NUMCARGA + "erro: " + tentativasCartao);
                //                                }
                //                                Thread.Sleep(1000);
                //                                if (tentativasCartao >= 6)
                //                                {
                //                                    log.AddLog(new LOG("OBSERVACAO ", "PROCESSO CONCLUIDO", "ERRO AO PROCESSAR A CARGA. FAVOR ENTRAR EM CONTATO COM SUPORTE."));
                //                                    return log;
                //                                    //log.AddLog(new LOG("LINHA " + count.ToString().PadLeft(5, '0'),
                //                                    //                   "CLIENTE " + usu.CODCLI + " CPF " + usu.CPF,
                //                                    //                   "ERRO AO PROCESSAR O CARTAO. FAVOR ENTRAR EM CONTATO COM SUPORTE"));
                //                                }
                //                            }
                //                        }
                //                    }
                //                }

                //                #endregion

                //                if (_inclusao > 0)
                //                {
                //                    log.AddLog(_inclusao == 1
                //                                  ? new LOG("OBSERVACAO ", "FOI INCLUIDO " + _inclusao + " CARTAO", "")
                //                                  : new LOG("OBSERVACAO ", "FORAM INCLUIDOS " + _inclusao + " CARTOES", ""));

                                    
                //                }
                //                //Ultimo registro de log para identificar que o processo ocorreu com sucesso total ou possui cartoes nao processados
                //                log.AddLog(!erroPorCartao
                //                               ? new LOG("OBSERVACAO ", "PROCESSO CONCLUIDO", "PROCESSO CONCLUIDO. ARQUIVO DE LOG DISPONIBILIZADO PARA VERIFICACAO.")
                //                               : new LOG("OBSERVACAO ", "PROCESSO CONCLUIDO", "PROCESSO CONCLUIDO. POREM ALGUNS CARTOES NAO FORAM PROCESSADOS. ARQUIVO DE LOG DISPONIBILIZADO PARA VERIFICACAO."));

                //                //if (layout == "05" || layout == "06")
                //                //Corrijo a tabela de centro de custo para os arquivos com layout 05 (que possuem centro de custo)
                //                da.SolicitaCargaUsuario(codCliInt, numSeqInt, DateTime.Now);
                //                CorrigeTabelaCargaCentroCusto(codCliInt, numSeqInt);
                //                da.BloqueiaSaldoContacliente(codCliInt, numSeqInt);
                //            }
                //        }
                //    }

                //    #endregion
                //}
            return log;
        }

        public List<string> ValidaLayoutArquivoCarga(string nomeArquivo, string caminhoArquivo)
        {
            var da = new daSolicitaCarga(_fOperador);

            return da.ValidaLayoutArquivoCarga(nomeArquivo, caminhoArquivo);
        }

        public List<string> ConfirmaCargaArquivo(string nomeTabela, string codigoCliente, string numCarga, string valorCarga, string dataProg, string cnpj, string idOperador, string codigoOperadora)
        {
            var da = new daSolicitaCarga(_fOperador);

            return da.ConfirmaCargaArquivo(nomeTabela, codigoCliente, numCarga, valorCarga, dataProg, cnpj, idOperador, codigoOperadora);
        }

        public List<string> CancelaCargaArquivo(string nomeTabela)
        {
            var da = new daSolicitaCarga(_fOperador);

            return da.CancelaCargaArquivo(nomeTabela);
        }

        #endregion

        #region Via Sistema

        public LOG ProcessaCargaSistema(List<USUARIO_VA> listaCargaGeral, int idOperWeb, int codOpeSis)
        {
            _contador = 1;
            var listaCargaPorCliente = new List<USUARIO_VA>();
            var log = new LOG();

            if (listaCargaGeral.Count > 0)
            {
                var codCliAux = listaCargaGeral[0].CODCLI;
                bool acessar;
                using (new Mutex(true, listaCargaGeral[0].CODCLI + listaCargaGeral[0].NUMCARGA, out acessar))
                    if (!acessar)
                        log.AddLog(new LOG("ERRO GERAL ", "CARGA EM ANDAMENTO",
                                           "Já existe um processo de solicitação para esta mesma carga.Aguarde alguns minutos e tente novamente"));
                    else
                    {

                        foreach (var t in listaCargaGeral)
                        {
                            if (t.CODCLI == codCliAux)
                                listaCargaPorCliente.Add(t);
                            else
                            {
                                log = InitSolicitacaoCargaSistema(listaCargaPorCliente[0].CODCLI,
                                                                  listaCargaPorCliente[0].NUMCARGA,
                                                                  listaCargaPorCliente,
                                                                  log, idOperWeb, codOpeSis, string.Empty);
                                listaCargaPorCliente.Clear();
                                codCliAux = t.CODCLI;
                                listaCargaPorCliente.Add(t);

                            }
                        }
                        if (listaCargaPorCliente.Count > 0)
                        {
                            log = InitSolicitacaoCargaSistema(listaCargaPorCliente[0].CODCLI,
                                                              listaCargaPorCliente[0].NUMCARGA,
                                                              listaCargaPorCliente, log, idOperWeb, codOpeSis,
                                                              string.Empty);
                        }
                    }
            }
            return log;
        }

        public LOG InitSolicitacaoCargaSistema(string codcli, int numCarga, List<USUARIO_VA> listaUsu, LOG log, int idOperWeb, int codOpeSis, string dtProg)
        {
            decimal totalCarga = 0;
            int codCliInt;
            var erroPorCartao = false;
            var da = new daSolicitaCarga(_fOperador);
            int.TryParse(codcli, out codCliInt);
            listaUsu.ForEach(u => totalCarga += Convert.ToDecimal(u.CARGPADVA, new CultureInfo("pt-BR")));

            var dadosVerificaCargaCli = new daSolicitaCarga(_fOperador).VerificaCargaCliente(codCliInt, numCarga,
                                                                                             totalCarga, dtProg,
                                                                                             string.Empty,
                                                                                             idOperWeb,
                                                                                             codOpeSis, 0);
            if (dadosVerificaCargaCli[0] != "OK")
            {
                log.AddLog(new LOG("ERRO GERAL ", "ERRO NO PROCESSO DO CLIENTE: " + codcli, dadosVerificaCargaCli[0]));
                log.AddLog(new LOG("OBSERVACAO ", "DADOS NAO PROCESSADOS", "FAVOR VERIFICAR OS ERROS NO ARQUIVO DE LOG"));
                return log;
            }
            try
            {
                foreach (var usu in listaUsu)
                {
                    usu.NUMCARGA = numCarga;
                    usu.NOMCRT = new blUsuarioVA(_fOperador).AbreviaNome(usu.NOMUSU);
                    usu.NUMPAC = dadosVerificaCargaCli[1]; // NUMPAC
                    usu.VALADES = dadosVerificaCargaCli[2]; // VALADES
                    usu.DTVALCART = dadosVerificaCargaCli[4]; // PRZVALCART   
                    usu.TIPO_LAYOUT = "05";
                    usu.CARGPADVACLIENTE =
                        string.IsNullOrEmpty(
                            dadosVerificaCargaCli[6].ToString(CultureInfo.InvariantCulture))
                            ? 0
                            : Convert.ToDecimal(dadosVerificaCargaCli[6]);
                    usu.TOTALCARGAUSUARIO = usu.CARGPADVA;
                    int inclusao;
                    _contador = da.SolicitaCargaUsuario(usu, log, ref erroPorCartao, true, _contador, idOperWeb, out inclusao);
                }
                da.SolicitaCargaUsuario(codCliInt, numCarga, DateTime.Now);
                CorrigeTabelaCargaCentroCusto(codCliInt, numCarga);
                da.BloqueiaSaldoContacliente(codCliInt, numCarga);
            }
            catch (Exception)
            {
                log.AddLog(new LOG("ERRO GERAL ", "ERRO NO PROCESSO DO CLIENTE: " + codcli, dadosVerificaCargaCli[0]));
            }

            //Ultimo registro de log para identificar que o processo ocorreu com sucesso total ou possui cartoes nao processados
            log.AddLog(!erroPorCartao
                           ? new LOG("OBSERVACAO ", "PROCESSO CONCLUIDO", "PROCESSO CONCLUIDO. ARQUIVO DE LOG DISPONIBILIZADO PARA VERIFICACAO.")
                           : new LOG("OBSERVACAO ", "PROCESSO CONCLUIDO", "PROCESSO CONCLUIDO. POREM ALGUNS CARTOES NAO FORAM PROCESSADOS. ARQUIVO DE LOG DISPONIBILIZADO PARA VERIFICACAO."));


            return log;
        }

        #endregion

        #region *** Novo Processo Carga ***

        //public DataTable ValidaLayoutArquivoCarga(string nomeArquivo, string nomeOriginalArquivo, string idsessao, string caminhoArquivo, int numeroLinhas, out string erro)
        //{
        //    var da = new daSolicitaCarga(_fOperador);

        //    return da.ValidaLayoutArquivoCarga(nomeArquivo, nomeOriginalArquivo, idsessao, caminhoArquivo, numeroLinhas, out erro);
        //}

        //public void VerificaProcessamentoCargaBarraProgressoNetCard(string login, string codope, out string nivel, out string estado)
        //{
        //    var da = new daSolicitaCarga(_fOperador);

        //    da.VerificaProcessamentoCargaBarraProgresso(login, codope, out nivel, out estado);

        //}

        public bool VerificaProcessamentoCarga(string paramCodigoOperador, string paramLoginOperador, string paramCodigoCliente)
        {
            var da = new daSolicitaCarga(_fOperador);

            return da.VerificaProcessamentoCarga(paramCodigoOperador, paramLoginOperador, paramCodigoCliente);
        }

        public void VerificaProcessamentoCargaBarraProgresso(string paramCodigoOperador, string paramLoginOperador, string paramCodigoCliente, out string paramNivel, out string paramEstado)
        {
            var da = new daSolicitaCarga(_fOperador);

            da.VerificaProcessamentoCargaBarraProgresso(paramCodigoOperador, paramLoginOperador, paramCodigoCliente, out paramNivel, out paramEstado);
        }

        public CARGA_CTRL_TABS BuscarInformacoesCarga(string paramCodigoOperador, string paramLoginOperador, string paramCodigoCliente)
        {
            var da = new daSolicitaCarga(_fOperador);

            return da.BuscarInformacoesCarga(paramCodigoOperador, paramLoginOperador, paramCodigoCliente);
        }

        public List<CARGA_CTRL_TABS_RESUMO> BuscarInformacoesResumoCarga(string paramId)
        {
            var da = new daSolicitaCarga(_fOperador);

            return da.BuscarInformacoesResumoCarga(paramId);
        }

        public bool BuscarInformacaoSolicitacaoCarga(string paramId)
        {
            var da = new daSolicitaCarga(_fOperador);

            return da.BuscarInformacaoSolicitacaoCarga(paramId);
        }

        

        public bool AlterarProcessamentoCarga(string paramSql)
        {
            var da = new daSolicitaCarga(_fOperador);

            return da.AlterarProcessamentoCarga(paramSql);
        }

        public bool DeletarDadosCarga(string paramLoginOpe, string paramCodope, string paramClienteOrigem)
        {
            var da = new daSolicitaCarga(_fOperador);

            return da.DeletarDadosCarga(paramLoginOpe, paramCodope, paramClienteOrigem);
        }

        #endregion
    }
}
