using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Interface.Services.Procedures;
using PagNet.BLD.DescontoFolha.Abstraction.Interface;
using PagNet.BLD.DescontoFolha.Abstraction.Interface.Model;
using PagNet.BLD.DescontoFolha.Abstraction.Model;
using PagNet.BLD.DescontoFolha.Util;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Telenet.BusinessLogicModel;
using static PagNet.BLD.DescontoFolha.Constants;

namespace PagNet.BLD.DescontoFolha.Application
{
    public class DescontoFolhaApp : Service<IContextoApp>, IDescontoFolhaApp
    {
        private readonly ITAB_GESTAO_DESCONTO_FOLHAService _descontoFolha;
        private readonly INETCARD_USUARIOPOSService _usuarioNetCard;
        private readonly IPAGNET_EMISSAOBOLETOService _emissaoBoleto;
        private readonly IProceduresService _proc;
        private readonly IParametrosApp _user;
        private readonly IPAGNET_ARQUIVO_DESCONTOFOLHAService _arquivoConciliacao;
        private readonly IPAGNET_CADCLIENTEService _cliente;

        public DescontoFolhaApp(IContextoApp contexto,
                                IParametrosApp user,
                                ITAB_GESTAO_DESCONTO_FOLHAService descontoFolha,
                                INETCARD_USUARIOPOSService usuarioNetCard,
                                IPAGNET_EMISSAOBOLETOService emissaoBoleto,
                                IProceduresService proc,
                                IPAGNET_CADCLIENTEService cliente,
                                IPAGNET_ARQUIVO_DESCONTOFOLHAService arquivoConciliacao)
            : base(contexto)
        {
            _user = user;
            _descontoFolha = descontoFolha;
            _proc = proc;
            _usuarioNetCard = usuarioNetCard;
            _emissaoBoleto = emissaoBoleto;
            _arquivoConciliacao = arquivoConciliacao;
            _cliente = cliente;
        }

        public DadosUsuarioVM BuscaDadosUsuarioByCPF(string CPF)
        {

            AssertionValidator
                .AssertNow(!string.IsNullOrWhiteSpace(CPF), CodigosErro.CPFInvalido)
                .Validate();
            try
            {
                DadosUsuarioVM dadosRetorno = new DadosUsuarioVM();
                var DadosUsuarioPos = _usuarioNetCard.BuscaUsuarioPosByCPF(CPF);

                dadosRetorno.cpfUsuario = DadosUsuarioPos.CPF;
                dadosRetorno.nomeUsuario = DadosUsuarioPos.CPF;
                dadosRetorno.codigoUsuario = DadosUsuarioPos.ID_USUARIO;
                dadosRetorno.codigoCliente = DadosUsuarioPos.CODCLI;
                dadosRetorno.CEP = DadosUsuarioPos.CEP;
                dadosRetorno.logradouroUsuario = DadosUsuarioPos.ENDUSU;
                dadosRetorno.numeroLogradouro = DadosUsuarioPos.ENDNUMUSU;
                dadosRetorno.complementoLogradouro = DadosUsuarioPos.ENDNUMCOM;
                dadosRetorno.bairroLogradouro = DadosUsuarioPos.BAIRRO;
                dadosRetorno.cidadeLogradouro = DadosUsuarioPos.LOCALIDADE;
                dadosRetorno.UF = DadosUsuarioPos.UF;

                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DadosUsuarioVM BuscaDadosUsuarioByMatricula(string Matricula)
        {

            AssertionValidator
                .AssertNow(!string.IsNullOrWhiteSpace(Matricula), CodigosErro.MatriculaVazia)
                .Validate();

            DadosUsuarioVM dadosRetorno = new DadosUsuarioVM();
            var DadosUsuarioPos = _usuarioNetCard.BuscaUsuarioPosByMatricula(Matricula);

            dadosRetorno.cpfUsuario = DadosUsuarioPos.CPF;
            dadosRetorno.nomeUsuario = DadosUsuarioPos.CPF;
            dadosRetorno.codigoUsuario = DadosUsuarioPos.ID_USUARIO;
            dadosRetorno.codigoCliente = DadosUsuarioPos.CODCLI;
            dadosRetorno.CEP = DadosUsuarioPos.CEP;
            dadosRetorno.logradouroUsuario = DadosUsuarioPos.ENDUSU;
            dadosRetorno.numeroLogradouro = DadosUsuarioPos.ENDNUMUSU;
            dadosRetorno.complementoLogradouro = DadosUsuarioPos.ENDNUMCOM;
            dadosRetorno.bairroLogradouro = DadosUsuarioPos.BAIRRO;
            dadosRetorno.cidadeLogradouro = DadosUsuarioPos.LOCALIDADE;
            dadosRetorno.UF = DadosUsuarioPos.UF;

            return dadosRetorno;
        }
        public UsuariosArquivoRetornoVm BuscaFechamentosNaoDescontados(IFiltroDescontoFolhaVM filtro)
        {
            UsuariosArquivoRetornoVm DadosRetorno = new UsuariosArquivoRetornoVm();
            try
            {
                //BUSCA INFORMAÇÕES PARA REALIZAR A CONCILIAÇÃO
                var DadosFatura = _emissaoBoleto.BuscaFaturamentoByID((int)filtro.codFatura).Result;

                var DadosDF = _descontoFolha.BuscarDadosDF((int)DadosFatura.CODCLIENTE, Convert.ToInt32(DadosFatura.NROREF_NETCARD));
                var dfAgrupado = (from reg in DadosDF
                                  group reg by new
                                  {
                                      reg.CPF
                                  } into g
                                  select new
                                  {
                                      g.Key.CPF,
                                      VALOR = g.Sum(s => s.VALTRA)
                                  }).ToList();
                ListaUsuariosArquivoRetornoVm ItemUsu = new ListaUsuariosArquivoRetornoVm();
                foreach (var item in dfAgrupado)
                {
                    ItemUsu = new ListaUsuariosArquivoRetornoVm();
                    var usuarioNC = _usuarioNetCard.BuscaUsuarioPosByCPF(item.CPF);

                    ItemUsu.Matricula = usuarioNC.MAT;
                    ItemUsu.CPF = Helper.FormataCPFCnPj(item.CPF);
                    ItemUsu.NomeClienteUsuario = usuarioNC.NOMUSU.Trim();
                    ItemUsu.ValorRem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.VALOR);
                    ItemUsu.ValorRet = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", 0);
                    ItemUsu.valorDecimal = item.VALOR;

                    DadosRetorno.ListaUsuarios.Add(ItemUsu);
                }

                return DadosRetorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UsuariosArquivoRetornoVm ConsolidaArquivoRetornoCliente(IFiltroDescontoFolhaVM filtro)
        {
            try
            {
                //BUSCA INFORMAÇÕES PARA REALIZAR A CONCILIAÇÃO
                var DadosFatura = _emissaoBoleto.BuscaFaturamentoByID((int)filtro.codFatura).Result;
                var dadosConfig = _arquivoConciliacao.BuscaConfiguracaoByCodCliente(Convert.ToInt32(DadosFatura.CODCLIENTE));

                //string extensaoRem = Path.GetExtension(pathArquivoConciliacao);
                string extensaoRet = Path.GetExtension(filtro.CaminhoArquivo);
                extensaoRet = extensaoRet.Replace(".", "");
                //extensaoRem = extensaoRem.Replace(".", "");
                AssertionValidator
                    .AssertNow(dadosConfig != null, CodigosErro.DescontoFolhaNaoConfigurado)
                    .Assert(extensaoRet != dadosConfig.EXTENSAOARQUI_RET, CodigosErro.ArquivoRetornoInvalido)
                    .Validate();


                UsuariosArquivoRetornoVm DadosRetorno = new UsuariosArquivoRetornoVm();

                List<ListaClienteUsuarioVm> ListaRemessa = new List<ListaClienteUsuarioVm>();
                List<ListaClienteUsuarioVm> ListaRetorno = new List<ListaClienteUsuarioVm>();

                DadosRetorno.CodigoCliente = Convert.ToInt32(DadosFatura.CODCLIENTE);
                DadosRetorno.codigoFatura = (int)filtro.codFatura;


                //Carrega os dados dentro do arquivo de retorno do cliente
                switch (dadosConfig.EXTENSAOARQUI_RET.Trim())
                {
                    case "CSV":
                        ListaRetorno = ProcessaArquivoCSV(filtro.CaminhoArquivo, dadosConfig, "RET");
                        break;
                    case "TXT":
                        ListaRetorno = ProcessaArquivoTXT(filtro.CaminhoArquivo, dadosConfig, "RET");
                        break;
                    case "XLSX":
                        ListaRetorno = ProcessaArquivoXLS(filtro.CaminhoArquivo, dadosConfig, "RET");
                        break;
                    case "XLS":
                        ListaRetorno = ProcessaArquivoXLS(filtro.CaminhoArquivo, dadosConfig, "RET");
                        break;
                    default:
                        ListaRetorno = ProcessaArquivoXLS(filtro.CaminhoArquivo, dadosConfig, "RET");
                        break;
                }
                DadosRetorno.quantidadeTotal = ListaRetorno.Count();
                if (ListaRetorno.Where(x => x.msgRetorno != "" && x.msgRetorno != null).Count() > 0)
                {
                    var teste = ListaRetorno.Where(x => x.msgRetorno != "" && x.msgRetorno != null).ToList();
                    DadosRetorno.msgRetorno = "Arquivo inválido. O arquivo não está no mesmo formato que foi configurado no sistema.";
                    return DadosRetorno;
                }


                //carrega o arquivo de conciliação enviado ao cliente
                ListaRemessa = BuscaDadosDescontoFolha((int)DadosFatura.CODCLIENTE, Convert.ToInt32(DadosFatura.NROREF_NETCARD));
                //VALIDA SE O ARQUIVO JÁ FOI PROCESSADO OU SE O ARQUIVO DE RETORNO É REALMENTE DO LOTE INFORMADO
                var DadosValidacao = ValidaArquivoConciliacaoPrefeitura(ListaRetorno, ListaRemessa, (int)filtro.codFatura, dadosConfig);

                if (DadosValidacao.msgRetorno != "")
                {
                    return DadosValidacao;
                }

                switch (dadosConfig.CODFORMAVERIFICACAO)
                {
                    case 1://Utilizando a lista os Usuários que não conseguiram Descontar
                        DadosRetorno.ListaUsuarios.AddRange(ListaUsuariosNaoDescontados1(ListaRetorno, ListaRemessa, (int)filtro.codFatura));
                        break;
                    case 2://Utilizando a lista os Usuários que conseguiram Descontar
                        DadosRetorno.ListaUsuarios.AddRange(ListaUsuariosNaoDescontados2(ListaRetorno, ListaRemessa, (int)filtro.codFatura));
                        break;
                    case 3://Utilizando a lista os Usuários que possui todos os usuários e aqueles que não conseguiram descontar o valor ficará zerado
                        DadosRetorno.ListaUsuarios.AddRange(ListaUsuariosNaoDescontados3(ListaRetorno, ListaRemessa, (int)filtro.codFatura));
                        break;
                }
                DadosRetorno.quantidadeTotalEsperado = ListaRemessa.Count();
                DadosRetorno.quantidadeOK = ListaRemessa.Count() - DadosRetorno.ListaUsuarios.Count();
                DadosRetorno.quantidadeNaoOK = DadosRetorno.ListaUsuarios.Count();

                //Lista os Usuários que conseguiram Descontar
                string listaCPF = "";
                foreach (var item in DadosRetorno.ListaUsuarios)
                {
                    listaCPF += "'" + Helper.RemoveCaracteres(item.CPF) + "',";
                }

                if (!string.IsNullOrWhiteSpace(listaCPF))
                {
                    listaCPF = listaCPF.Substring(0, listaCPF.Length - 1);
                    var RetornoProcessoDF = _proc.ExecutarDescontoFolha((int)DadosFatura.CODCLIENTE, (int)filtro.codFatura, Convert.ToInt32(DadosFatura.NROREF_NETCARD), listaCPF, _user.cod_usu);
                }

                return DadosRetorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<ListaClienteUsuarioVm> ProcessaArquivoCSV(string caminhoArquivo, PAGNET_ARQUIVO_DESCONTOFOLHA dadosConfig, string TipoArquivo)
        {

            //CARREGA INFORMAÇÕES PARA LER O ARQUIVO DE RETORNO DO CLIENTE
            var InfoCPF = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "CPF").FirstOrDefault();
            var InfoMatricula = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "MATRICULA").FirstOrDefault();
            var InfoValor = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "VALOR").FirstOrDefault();

            int LinhaInicioRetorno = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "MATRICULA").Select(t => t.LINHAINICIO - 1).FirstOrDefault();

            List<ListaClienteUsuarioVm> ListaRetorno = new List<ListaClienteUsuarioVm>();
            try
            {

                //LE O ARQUIVO DE RETORNO DO CLIENTE E CARREGA A LISTA DOS USUÁRIOS E VALORES PARA COMPARAÇÃO
                string[] linesRetorno = File.ReadAllLines(caminhoArquivo);

                ListaClienteUsuarioVm itemRet = new ListaClienteUsuarioVm();
                for (int t = LinhaInicioRetorno; t < linesRetorno.Length; t++)
                {
                    itemRet = new ListaClienteUsuarioVm();
                    var item = linesRetorno[t].Split(';');

                    if (InfoCPF != null)
                    {
                        if (string.IsNullOrWhiteSpace(item[(InfoCPF.POSICAOINI - 1)])) continue;

                        itemRet.CPF = item[(InfoCPF.POSICAOINI - 1)].Trim();

                        var dadosUsu = _usuarioNetCard.BuscaUsuarioPosByCPF(itemRet.CPF);
                        //se não encontrar nenhum usuaário com este CPF então o CPF é inválido e devemos recusar o arquivo
                        if (dadosUsu == null)
                        {
                            itemRet.msgRetorno = "Arquivo inválido. O arquivo não está no mesmo formato que foi configurado no sistema.";
                            ListaRetorno.Add(itemRet);
                            break;
                        }

                        itemRet.Matricula = dadosUsu.MAT;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(item[(InfoMatricula.POSICAOINI - 1)])) continue;

                        itemRet.Matricula = item[(InfoMatricula.POSICAOINI - 1)].Trim();
                        var dadosUsu = _usuarioNetCard.BuscaUsuarioPosByMatricula(Helper.FormataInteiro(itemRet.Matricula, 6));
                        //se não encontrar nenhum usuário com esta matricula então o CPF é inválido e devemos recusar o arquivo
                        if (dadosUsu == null)
                        {
                            itemRet.msgRetorno = "Arquivo inválido. O arquivo não está no mesmo formato que foi configurado no sistema.";
                            ListaRetorno.Add(itemRet);
                            break;
                        }
                        itemRet.CPF = dadosUsu.CPF;
                    }

                    itemRet.ValorRet = item[(InfoValor.POSICAOINI - 1)].Trim();
                    var aux1 = Helper.TrataValorMonetario(itemRet.ValorRet);
                    itemRet.valorDecimal = Math.Abs(Helper.TrataDecimal(aux1));
                    itemRet.ValorRet = Helper.TrataValorMonetario(itemRet.valorDecimal);
                    ListaRetorno.Add(itemRet);

                }

            }
            catch (Exception ex)
            {
                ListaClienteUsuarioVm itemRet = new ListaClienteUsuarioVm();
                itemRet.msgRetorno = "Arquivo inválido. O arquivo não está no mesmo formato que foi configurado no sistema.";
                ListaRetorno.Add(itemRet);
            }

            return ListaRetorno;
        }
        private List<ListaClienteUsuarioVm> ProcessaArquivoTXT(string caminhoArquivo, PAGNET_ARQUIVO_DESCONTOFOLHA dadosConfig, string TipoArquivo)
        {
            var InfoCPF = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "CPF").FirstOrDefault();
            var InfoMatricula = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "MATRICULA").FirstOrDefault();
            var InfoValor = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "VALOR").FirstOrDefault();
            int LinhaInicioRetorno = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "MATRICULA").Select(t => t.LINHAINICIO - 1).FirstOrDefault();

            List<ListaClienteUsuarioVm> ListaRetorno = new List<ListaClienteUsuarioVm>();
            try
            {

                //LE O ARQUIVO DE RETORNO DO CLIENTE E CARREGA A LISTA DOS USUÁRIOS E VALORES PARA COMPARAÇÃO
                string[] linesRetorno = File.ReadAllLines(caminhoArquivo);

                ListaClienteUsuarioVm itemRet = new ListaClienteUsuarioVm();
                for (int t = LinhaInicioRetorno; t < linesRetorno.Length; t++)
                {
                    itemRet = new ListaClienteUsuarioVm();

                    if (InfoCPF != null)
                    {
                        if (string.IsNullOrWhiteSpace(linesRetorno[t].Substring(InfoCPF.POSICAOINI, InfoCPF.POSICAOFIM))) continue;

                        itemRet.CPF = linesRetorno[t].Substring(InfoCPF.POSICAOINI, InfoCPF.POSICAOFIM);

                        var dadosUsu = _usuarioNetCard.BuscaUsuarioPosByCPF(itemRet.CPF);
                        //se não encontrar nenhum usuaário com este CPF então o CPF é inválido e devemos recusar o arquivo
                        if (dadosUsu == null)
                        {
                            itemRet.msgRetorno = "Arquivo inválido. O arquivo não está no mesmo formato que foi configurado no sistema.";
                            ListaRetorno.Add(itemRet);
                            break;
                        }
                        itemRet.Matricula = dadosUsu.MAT;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(linesRetorno[t].Substring(InfoMatricula.POSICAOINI, InfoMatricula.POSICAOFIM))) continue;

                        itemRet.Matricula = linesRetorno[t].Substring(InfoMatricula.POSICAOINI, InfoMatricula.POSICAOFIM);
                        var dadosUsu = _usuarioNetCard.BuscaUsuarioPosByMatricula(itemRet.Matricula);
                        //se não encontrar nenhum usuaário com este CPF então o CPF é inválido e devemos recusar o arquivo
                        if (dadosUsu == null)
                        {
                            itemRet.msgRetorno = "Arquivo inválido. O arquivo não está no mesmo formato que foi configurado no sistema.";
                            ListaRetorno.Add(itemRet);
                            break;
                        }
                        itemRet.CPF = dadosUsu.CPF;
                    }

                    itemRet.ValorRet = linesRetorno[t].Substring(InfoValor.POSICAOINI, InfoValor.POSICAOFIM);
                    var aux1 = Helper.TrataValorMonetario(itemRet.ValorRet);
                    itemRet.valorDecimal = Math.Abs(Helper.TrataDecimal(aux1));
                    itemRet.ValorRet = Helper.TrataValorMonetario(itemRet.valorDecimal);
                    ListaRetorno.Add(itemRet);

                }

            }
            catch (Exception ex)
            {
                ListaClienteUsuarioVm itemRet = new ListaClienteUsuarioVm();
                itemRet.msgRetorno = "Arquivo inválido. O arquivo não está no mesmo formato que foi configurado no sistema.";
                ListaRetorno.Add(itemRet);
            }

            return ListaRetorno;
        }
        private List<ListaClienteUsuarioVm> ProcessaArquivoXLS(string caminhoArquivo, PAGNET_ARQUIVO_DESCONTOFOLHA dadosConfig, string TipoArquivo)
        {
            var InfoCPF = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "CPF").FirstOrDefault();
            var InfoMatricula = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "MATRICULA").FirstOrDefault();
            var InfoValor = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "VALOR").FirstOrDefault();
            var listaUsuario = _usuarioNetCard.BuscaUsuarioTodosPosByCliente(dadosConfig.CODCLIENTE);
            int LinhaInicioRetorno = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Select(t => t.LINHAINICIO).FirstOrDefault();

            List<ListaClienteUsuarioVm> ListaRetorno = new List<ListaClienteUsuarioVm>();
            try
            {
                using (SLDocument sl = new SLDocument())
                {
                    using (FileStream fs = new FileStream(caminhoArquivo, FileMode.Open))
                    {
                        SLDocument sheet = new SLDocument(fs);

                        ListaClienteUsuarioVm itemRet = new ListaClienteUsuarioVm();
                        SLWorksheetStatistics stats = sheet.GetWorksheetStatistics();
                        for (int j = LinhaInicioRetorno; j <= stats.EndRowIndex; j++)
                        {
                            itemRet = new ListaClienteUsuarioVm();
                            // Get the first column of the row (SLS is a 1-based index)
                            if (InfoCPF != null)
                            {
                                if (string.IsNullOrWhiteSpace(sheet.GetCellValueAsString(j, InfoCPF.POSICAOINI))) continue;

                                itemRet.CPF = sheet.GetCellValueAsString(j, InfoCPF.POSICAOINI);
                                var dadosUsu = listaUsuario.Where(x => x.CPF == itemRet.CPF).FirstOrDefault();
                                //se não encontrar nenhum usuaário com este CPF então o CPF é inválido e devemos recusar o arquivo
                                if (dadosUsu == null)
                                {
                                    itemRet.msgRetorno = "Arquivo inválido. O arquivo não está no mesmo formato que foi configurado no sistema.";
                                    ListaRetorno.Add(itemRet);
                                    break;
                                }

                                itemRet.Matricula = dadosUsu.MAT;
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(sheet.GetCellValueAsString(j, InfoMatricula.POSICAOINI))) continue;

                                itemRet.Matricula = sheet.GetCellValueAsString(j, InfoMatricula.POSICAOINI);
                                var dadosUsu = listaUsuario.Where(x => x.MAT.Trim() == itemRet.Matricula).FirstOrDefault();
                                //se não encontrar nenhum usuário com esta matricula então o CPF é inválido e devemos recusar o arquivo
                                if (dadosUsu == null)
                                {
                                    itemRet.msgRetorno = "Arquivo inválido. O arquivo não está no mesmo formato que foi configurado no sistema.";
                                    ListaRetorno.Add(itemRet);
                                    break;
                                }
                                itemRet.CPF = dadosUsu.CPF;
                            }
                            itemRet.ValorRet = sheet.GetCellValueAsString(j, (InfoValor.POSICAOINI));
                            var aux1 = Helper.TrataValorMonetario(itemRet.ValorRet);
                            itemRet.valorDecimal = Math.Abs(Helper.TrataDecimal(aux1));
                            itemRet.ValorRet = Helper.TrataValorMonetario(itemRet.valorDecimal);
                            ListaRetorno.Add(itemRet);
                        }
                        fs.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                ListaClienteUsuarioVm itemRet = new ListaClienteUsuarioVm();
                itemRet.msgRetorno = "Arquivo inválido. O arquivo não está no mesmo formato que foi configurado no sistema.";
                ListaRetorno.Add(itemRet);
            }

            return ListaRetorno;
        }
        private List<ListaClienteUsuarioVm> BuscaDadosDescontoFolha(int codigoCliente, int nroLote)
        {
            List<ListaClienteUsuarioVm> listaRetorno = new List<ListaClienteUsuarioVm>();
            var DadosDF = _descontoFolha.BuscarDadosDF(codigoCliente, nroLote);
            var dfAgrupado = (from reg in DadosDF
                              group reg by new
                              {
                                  reg.CPF,
                              } into g
                              select new
                              {
                                  g.Key.CPF,
                                  VALOR = g.Sum(s => s.VALTRA)
                              }).ToList();

            ListaClienteUsuarioVm itemRef;
            foreach (var df in dfAgrupado)
            {
                itemRef = new ListaClienteUsuarioVm();
                itemRef.CPF = df.CPF;
                itemRef.ValorRem = Helper.TrataValorMonetario(Math.Abs(df.VALOR));
                itemRef.valorDecimal = Math.Abs(df.VALOR);
                itemRef.Matricula = "";
                listaRetorno.Add(itemRef);
            }
            
            return listaRetorno;
        }
        private List<ListaUsuariosArquivoRetornoVm> ListaUsuariosNaoDescontados1(List<ListaClienteUsuarioVm> ListaRetorno, List<ListaClienteUsuarioVm> ListaRemessa, int CodigoFatura)
        {
            //Lista os Usuários que não conseguiram Descontar
            List<ListaUsuariosArquivoRetornoVm> ListaUsuarios = new List<ListaUsuariosArquivoRetornoVm>();

            ListaRetorno = ListaRetorno.Where(x => x.valorDecimal > 0).ToList();
            ListaRemessa = ListaRemessa.Where(x => x.valorDecimal > 0).ToList();
            //Lista os Usuários que conseguiram Descontar
            var ListaUsuariosNaoDescontados =
                       (from Rem in ListaRemessa
                        where (from Ret in ListaRetorno
                                select new { Ret.CPF, Ret.valorDecimal })
                               .Contains(new { Rem.CPF, Rem.valorDecimal })
                        select Rem).ToList();


            ListaUsuariosArquivoRetornoVm ItemUsu = new ListaUsuariosArquivoRetornoVm();
            foreach (var item in ListaUsuariosNaoDescontados)
            {
                ItemUsu = new ListaUsuariosArquivoRetornoVm();

                var usuarioNC = _usuarioNetCard.BuscaUsuarioPosByCPF(item.CPF);

                var infoRet = ListaRetorno.Where(x => x.CPF == item.CPF).FirstOrDefault();
                ItemUsu.Matricula = infoRet.Matricula;
                ItemUsu.CPF = Helper.FormataCPFCnPj(item.CPF);
                ItemUsu.NomeClienteUsuario = usuarioNC.NOMUSU.Trim();
                ItemUsu.ValorRem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.valorDecimal);
                ItemUsu.ValorRet = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", infoRet.valorDecimal);
                item.valorDecimal = item.valorDecimal;

                ListaUsuarios.Add(ItemUsu);
            }
            return ListaUsuarios;
        }
        private List<ListaUsuariosArquivoRetornoVm> ListaUsuariosNaoDescontados2(List<ListaClienteUsuarioVm> ListaRetorno, List<ListaClienteUsuarioVm> ListaRemessa, int CodigoFatura)
        {
            //Lista os Usuários que conseguiram Descontar
            List<ListaUsuariosArquivoRetornoVm> ListaUsuarios = new List<ListaUsuariosArquivoRetornoVm>();

            ListaRetorno = ListaRetorno.Where(x => x.valorDecimal > 0).ToList();
            ListaRemessa = ListaRemessa.Where(x => x.valorDecimal > 0).ToList();
            //Lista os Usuários que conseguiram Descontar
            var ListaUsuariosNaoDescontados =
                       (from Rem in ListaRemessa
                        where !(from Ret in ListaRetorno
                               select new { Ret.CPF, Ret.valorDecimal })
                               .Contains(new { Rem.CPF, Rem.valorDecimal })
                        select Rem).ToList();


            ListaUsuariosArquivoRetornoVm ItemUsu = new ListaUsuariosArquivoRetornoVm();
            
            foreach (var item in ListaUsuariosNaoDescontados)
            {
                ItemUsu = new ListaUsuariosArquivoRetornoVm();

                var UsuRet = ListaRetorno.Where(x => x.CPF == item.CPF).FirstOrDefault();
                var usuarioNC = _usuarioNetCard.BuscaUsuarioPosByCPF(item.CPF);
                if (usuarioNC != null)
                {
                    ItemUsu.Matricula = UsuRet.Matricula;
                    ItemUsu.CPF = Helper.FormataCPFCnPj(item.CPF);
                    ItemUsu.NomeClienteUsuario = usuarioNC.NOMUSU.Trim();
                    ItemUsu.ValorRem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.valorDecimal);
                    ItemUsu.ValorRet = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", 0);
                    item.valorDecimal = item.valorDecimal;

                }
                else
                {
                    if(UsuRet != null)
                        ItemUsu.Matricula = UsuRet.Matricula;

                    ItemUsu.CPF = item.CPF;
                    ItemUsu.NomeClienteUsuario = "Usuário não encontrado";
                    ItemUsu.ValorRet = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", 0);
                    ItemUsu.ValorRem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", 0);
                    item.valorDecimal = 0;
                }

                ListaUsuarios.Add(ItemUsu);
            }


            return ListaUsuarios;
        }
        private List<ListaUsuariosArquivoRetornoVm> ListaUsuariosNaoDescontados3(List<ListaClienteUsuarioVm> ListaRetorno, List<ListaClienteUsuarioVm> ListaRemessa, int CodigoFatura)
        {
            ListaRetorno = ListaRetorno.Where(x => x.valorDecimal == 0).ToList();

            List<ListaUsuariosArquivoRetornoVm> ListaUsuarios = new List<ListaUsuariosArquivoRetornoVm>();

            ListaUsuariosArquivoRetornoVm ItemUsu = new ListaUsuariosArquivoRetornoVm();
            foreach (var item in ListaRetorno)
            {
                ItemUsu = new ListaUsuariosArquivoRetornoVm();

                var UsuRet = ListaRetorno.Where(x => x.CPF == item.CPF).FirstOrDefault();
                var usuarioNC = _usuarioNetCard.BuscaUsuarioPosByCPF(item.CPF);
                if (usuarioNC != null)
                {
                    ItemUsu.Matricula = UsuRet.Matricula;
                    ItemUsu.CPF = Helper.FormataCPFCnPj(item.CPF);
                    ItemUsu.NomeClienteUsuario = usuarioNC.NOMUSU.Trim();
                    ItemUsu.ValorRem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.valorDecimal);
                    ItemUsu.ValorRet = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.valorDecimal);
                    ItemUsu.valorDecimal = item.valorDecimal;

                }
                else
                {
                    ItemUsu.Matricula = UsuRet.Matricula;
                    ItemUsu.CPF = "-";
                    ItemUsu.NomeClienteUsuario = "Usuário não encontrado";
                    ItemUsu.ValorRet = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", 0);
                    ItemUsu.ValorRem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", 0);
                    item.valorDecimal = 0;
                }

                ListaUsuarios.Add(ItemUsu);
            }
            return ListaUsuarios;
        }
        private UsuariosArquivoRetornoVm ValidaArquivoConciliacaoPrefeitura(List<ListaClienteUsuarioVm> ListaRetorno, List<ListaClienteUsuarioVm> ListaRemessa, int codFatura, PAGNET_ARQUIVO_DESCONTOFOLHA dadosConfig)
        {
            try
            {
                UsuariosArquivoRetornoVm dadosRetorno = new UsuariosArquivoRetornoVm();
                dadosRetorno.msgRetorno = "";

                ListaRetorno = ListaRetorno.Where(x => x.valorDecimal > 0).ToList();
                ListaRemessa = ListaRemessa.Where(x => x.valorDecimal > 0).ToList();
                //Lista os Usuários que conseguiram Descontar
                var ListaUsuarios =
                           (from Rem in ListaRemessa
                            where !(from Ret in ListaRetorno
                                    select new { Ret.CPF, Ret.valorDecimal })
                                   .Contains(new { Rem.CPF, Rem.valorDecimal })
                            select Rem).ToList();
                ListaUsuariosArquivoRetornoVm itemRet;

                ////Valida se o arquivo já foi processado
                //var ListaLote = _descontoFolha.BuscarDadosLoteDF(dadosConfig.CODCLIENTE, codFatura);
                //ListaLote = ListaLote.Where(x => x.DATRETARQ != null).ToList();
                //if (ListaLote.Count > 0)
                //{
                //    dadosRetorno.msgRetorno = "Arquivo já validado.";
                //    return dadosRetorno;
                //}

                //sistema só deve seguir com a validação se o arquivo ainda não tiver sido processado anteriormente

                foreach (var item in ListaUsuarios)
                {
                    var UsuRet = ListaRetorno.Where(x => x.CPF == item.CPF).FirstOrDefault();
                    var UsuRem = ListaRemessa.Where(x => x.CPF == item.CPF).FirstOrDefault();
                    if (UsuRet != null)
                    {
                        //Realizo a validação de usuários que existe tanto no arquivo de desconto em folha, quanto o arquivo de retorno.
                        //Verifico se existir, o valor tem que bater, caso contrãrio o arquivo não é válido.
                        if (UsuRet.valorDecimal != UsuRem.valorDecimal)
                        {
                            itemRet = new ListaUsuariosArquivoRetornoVm();
                            var usuarioNC = _usuarioNetCard.BuscaUsuarioPosByCPF(item.CPF);
                            if (usuarioNC != null)
                            {
                                var infoRet = ListaRetorno.Where(x => x.CPF == item.CPF).FirstOrDefault();
                                itemRet.Matricula = UsuRet.Matricula;
                                itemRet.CPF = Helper.FormataCPFCnPj(item.CPF);
                                itemRet.NomeClienteUsuario = usuarioNC.NOMUSU.Trim();
                                itemRet.ValorRem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.valorDecimal);
                                itemRet.ValorRet = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", infoRet.valorDecimal);
                                itemRet.valorDecimal = item.valorDecimal;


                            }
                            else
                            {
                                itemRet.Matricula = UsuRet.Matricula;
                                itemRet.CPF = "-";
                                itemRet.NomeClienteUsuario = "Usuário não encontrado";
                                itemRet.ValorRem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", UsuRem.valorDecimal);
                                itemRet.ValorRet = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", UsuRet.valorDecimal);

                            }
                            itemRet.msgRetorno = "Valor de retorno divergente do valor do arquivo enviado.";
                            dadosRetorno.ListaUsuarios.Add(itemRet);
                        }

                    }
                }

                if (dadosRetorno.ListaUsuarios.Count > 0)
                {
                    dadosRetorno.msgRetorno = "Arquivo inválido. Verifique se o arquivo de retorno pertence a fatura selecionada";
                }

                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ConfigParamLeituraArquivoVM BuscaConfiguracaoByCliente(IFiltroDescontoFolhaVM filtro)
        {

            AssertionValidator
                .AssertNow(filtro.codigoCliente != 0, CodigosErro.ClienteNaoInformado)
                .Validate();


            var dadosConfig = _arquivoConciliacao.BuscaConfiguracaoByCodCliente(filtro.codigoCliente);
            var dadosCliente = _cliente.BuscaClienteByID(filtro.codigoCliente).Result;

            ConfigParamLeituraArquivoVM parametros = new ConfigParamLeituraArquivoVM();
            parametros.codigoCliente = dadosCliente.CODCLIENTE;
            parametros.nomeCliente = dadosCliente.NMCLIENTE;

            if (dadosConfig != null)
            {
                var InfoCPF = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "CPF").FirstOrDefault();
                var InfoMatricula = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "MATRICULA").FirstOrDefault();
                var InfoValor = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.CAMPO == "VALOR").FirstOrDefault();

                parametros.IsCPF = (InfoCPF != null);

                parametros.extensaoArquivoRET = dadosConfig.EXTENSAOARQUI_RET;
                parametros.codigoArquivoDescontoFolha = dadosConfig.CODARQUIVO_DESCONTOFOLHA;
                parametros.codigoFormaVerificacao = dadosConfig.CODFORMAVERIFICACAO;

                if (InfoCPF != null)
                {
                    parametros.posicaoInicialCPF = InfoCPF.POSICAOINI;
                    parametros.posicaoFinalCPF = InfoCPF.POSICAOFIM;
                }
                if (InfoMatricula != null)
                {
                    parametros.posicaoInicialMatricula = InfoMatricula.POSICAOINI;
                    parametros.posicaoFinalMatricula = InfoMatricula.POSICAOFIM;
                }
                if (InfoValor != null)
                {
                    parametros.linhaInicial = Convert.ToString(InfoValor.LINHAINICIO);
                    parametros.posicaoInicialValor = InfoValor.POSICAOINI;
                    parametros.posicaoFinalValor = InfoValor.POSICAOFIM;
                }
            }

            return parametros;

        }
        public void SalvarParamLeituraArquivo(IConfigParamLeituraArquivoVM model)
        {
            PAGNET_ARQUIVO_DESCONTOFOLHA arq;
            if (model.codigoArquivoDescontoFolha == 0)
            {
                arq = new PAGNET_ARQUIVO_DESCONTOFOLHA();
                arq.EXTENSAOARQUI_RET = "";
                arq.CODCLIENTE = model.codigoCliente;
                arq.ATIVO = "S";
                arq.CODFORMAVERIFICACAO = 3;
            }
            else
            {
                arq = _arquivoConciliacao.BuscaConfiguracao(model.codigoArquivoDescontoFolha);
            }

                arq.CODFORMAVERIFICACAO = model.codigoFormaVerificacao;
          

            arq.EXTENSAOARQUI_RET = Convert.ToString(model.extensaoArquivoRET);

            if (model.codigoArquivoDescontoFolha == 0)
            {
                _arquivoConciliacao.IncluiArquivoConciliacao(arq);
                model.codigoArquivoDescontoFolha = arq.CODARQUIVO_DESCONTOFOLHA;
            }
            else
            {
                _arquivoConciliacao.AtuializaArquivoConciliacao(arq);
            }

            SalvaParametros(model);
        }
        private void SalvaParametros(IConfigParamLeituraArquivoVM model)
        {
            var ListaParametros = _arquivoConciliacao.BuscaTodosParametrosConfiguracao(model.codigoArquivoDescontoFolha);
            if (ListaParametros.Count > 0)
            {
                foreach (var param in ListaParametros)
                {
                    if (param.CAMPO == "VALOR")
                    {
                        param.POSICAOINI = model.posicaoInicialValor;
                        param.POSICAOINI = model.posicaoInicialValor;
                        param.POSICAOFIM = model.posicaoFinalValor;
                        param.LINHAINICIO = Convert.ToInt32(model.linhaInicial);
                    }
                    else
                    {
                        if (model.IsCPF)
                        {
                            param.CAMPO = "CPF";
                            param.POSICAOINI = model.posicaoInicialCPF;
                            param.POSICAOINI = model.posicaoFinalCPF;
                            param.POSICAOFIM = model.posicaoFinalCPF;
                        }
                        else
                        {
                            param.CAMPO = "MATRICULA";
                            param.POSICAOINI = model.posicaoInicialMatricula;
                            param.POSICAOINI = model.posicaoInicialMatricula;
                            param.POSICAOFIM = model.posicaoFinalMatricula;

                        }
                        param.LINHAINICIO = Convert.ToInt32(model.linhaInicial);
                    }

                    _arquivoConciliacao.AtuializaParamArquivoConciliacao(param);
                }
            }
            else
            {
                //--INCLUIR PARAMETROS DE LOCALIZAÇÃO DO USUÁRIO
                PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA param = new PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA();
                if (model.IsCPF)
                {
                    param.CAMPO = "CPF";
                    param.POSICAOINI = model.posicaoInicialCPF;
                    param.POSICAOINI = model.posicaoFinalCPF;
                    param.POSICAOFIM = model.posicaoFinalCPF;
                }
                else
                {
                    param.CAMPO = "MATRICULA";
                    param.POSICAOINI = model.posicaoInicialMatricula;
                    param.POSICAOINI = model.posicaoInicialMatricula;
                    param.POSICAOFIM = model.posicaoFinalMatricula;

                }
                param.CODARQUIVO_DESCONTOFOLHA = model.codigoArquivoDescontoFolha;
                param.LINHAINICIO = Convert.ToInt32(model.linhaInicial);

                _arquivoConciliacao.IncluiParamArquivoConciliacao(param);

                //----INCLUIR PARAMETROS DE LOCALIZAÇÃO DO VALOR
                param = new PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA();

                param.CAMPO = "VALOR";
                param.POSICAOINI = model.posicaoInicialValor;
                param.POSICAOINI = model.posicaoInicialValor;
                param.POSICAOFIM = model.posicaoFinalValor;
                param.CODARQUIVO_DESCONTOFOLHA = model.codigoArquivoDescontoFolha;
                param.LINHAINICIO = Convert.ToInt32(model.linhaInicial);

                _arquivoConciliacao.IncluiParamArquivoConciliacao(param);
            }
        }
        public void ValidaFaturamentoViaArquivo(IFiltroDescontoFolhaVM filtro)
        {
            try
            {
                int codFatura = (int)filtro.codFatura;
                var DadosFatura = _emissaoBoleto.BuscaFaturamentoByID(codFatura).Result;
                int CODEMISSAOFATURAMENTO = DadosFatura.CODEMISSAOFATURAMENTO;
                var DescontoFolha = _descontoFolha.BuscarTransacaoUsuarioByCPF((int)filtro.numeroLote, filtro.codigoCliente, Helper.RemoveCaracteres(filtro.CPF));
                var valorTotal = DescontoFolha.Select(x => x.VALTRA).Sum();



                var codCliente = (_proc.IncluiClienteUsuarioNC(Helper.RemoveCaracteres(filtro.CPF), 0, _user.cod_empresa, _user.cod_usu)).Result.CODCLIENTE;

                if (codCliente == 0)
                {
                    AssertionValidator
                        .AssertNow(codCliente == 0, CodigosErro.UsuarioNaoEncontrado)
                        .Validate();
                }

                PAGNET_EMISSAOFATURAMENTO fatura = new PAGNET_EMISSAOFATURAMENTO();
                fatura = DadosFatura;
                fatura.CODEMISSAOFATURAMENTOPAI = CODEMISSAOFATURAMENTO;
                fatura.VALORPARCELA = valorTotal;
                fatura.VALOR = valorTotal;
                fatura.PARCELA = 1;
                fatura.STATUS = filtro.status;
                fatura.TOTALPARCELA = 1;
                fatura.CODCLIENTE = codCliente;

                //INCLUI UMA NOVA FATURA
                fatura.CODEMISSAOFATURAMENTO = _emissaoBoleto.BuscaNovoIDEmissaoFaturamento();
                _emissaoBoleto.IncluiFaturamento(fatura);

                //INCLUSÃO DE LOG
                _emissaoBoleto.IncluiLog(fatura, _user.cod_usu, $"Criação de uma nova fatura a partir do arquivo de retorno de validação de faturamento.");

                //Altera o valor da fatura da prefeitura devido ao arquivo de conciliacao bancaria
                if (valorTotal > 0)
                {
                    fatura = new PAGNET_EMISSAOFATURAMENTO();
                    fatura = _emissaoBoleto.BuscaFaturamentoByID(codFatura).Result;

                    fatura.VALORPARCELA = fatura.VALORPARCELA - valorTotal;
                    fatura.STATUS = "EM_ABERTO";

                    _emissaoBoleto.AtualizaFaturamento(fatura);

                    //INCLUSÃO DE LOG
                    _emissaoBoleto.IncluiLog(fatura, _user.cod_usu, $"Alteração do valor a pagar devido o arquivo de retorno que a prefeitura enviou constando quem ela conseguiu ou não descontar.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExecutaProcessoDescontoFolha(IUsuariosArquivoRetornoVm model)
        {
            try
            {
                var DadosFatura = _emissaoBoleto.BuscaFaturamentoByID(model.codigoFatura).Result;

                foreach (var item in model.ListaUsuarios)
                {
                    if (item.Acao == "COBRANCADIRETA")
                    {
                        RegistraCobrancaDireta(model.codigoFatura, item.CPF, DadosFatura, item.valorDecimal);
                    }
                    else if (item.Acao == "PERDOARDIVIDA")
                    {
                        ContabilizarComoPerda(item.CPF, model.CodigoCliente, Convert.ToInt32(DadosFatura.NROREF_NETCARD));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ContabilizarComoPerda(string cpf, int codigoCliente, int lote)
        {
            var listaTransacoes = _descontoFolha.BuscarTransacaoUsuarioByCPF(lote, codigoCliente, cpf);
            for(int i=0; i<= listaTransacoes.Count; i++)
            {
                listaTransacoes[i].DATPGTO = DateTime.Now;
            }
            _descontoFolha.AtualizarLista(listaTransacoes);
        }
        private void RegistraCobrancaDireta(int codigoFatura, string cpf, PAGNET_EMISSAOFATURAMENTO DadosFatura, decimal ValorDevido)
        {
            //Cadastra o usuário como um cliente no sistema PagNet
            var cliente = _proc.IncluiClienteUsuarioNC(cpf, 0, _user.cod_empresa, _user.cod_usu).Result;

            PAGNET_EMISSAOFATURAMENTO faturamento = new PAGNET_EMISSAOFATURAMENTO();
            int codFaturamento = _emissaoBoleto.BuscaNovoIDEmissaoFaturamento();
            faturamento.CODEMISSAOFATURAMENTO = codFaturamento;
            faturamento.CODEMISSAOFATURAMENTOPAI = DadosFatura.CODEMISSAOFATURAMENTO;
            faturamento.ORIGEM = "PAGNET";
            faturamento.TIPOFATURAMENTO = "PAGNET";
            faturamento.DATSOLICITACAO = DateTime.Now;
            faturamento.DATVENCIMENTO = DadosFatura.DATVENCIMENTO;
            faturamento.NROREF_NETCARD = DadosFatura.NROREF_NETCARD;
            faturamento.NRODOCUMENTO = DadosFatura.NRODOCUMENTO;
            faturamento.CODCLIENTE = cliente.CODCLIENTE;
            faturamento.CODBORDERO = null;
            faturamento.CODEMPRESA = _user.cod_empresa;
            faturamento.STATUS = "EM_ABERTO";
            faturamento.VALOR = ValorDevido;
            faturamento.PARCELA = 1;
            faturamento.TOTALPARCELA = 1;
            faturamento.VALORPARCELA = ValorDevido;

            _emissaoBoleto.IncluiFaturamento(faturamento);
            _emissaoBoleto.IncluiLog(faturamento, _user.cod_usu, "Fatura incluida através do arquivo de retorno do desconto em folha.");

        }
        public List<APIRetornoDDLModel> CarregaListaFaturasAbertas(int codigoCliente)
        {
            try
            {
                List<APIRetornoDDLModel> listaRetorno = new List<APIRetornoDDLModel>();
                var lista = _emissaoBoleto.CarregaListaFaturas(codigoCliente).ToList();

                APIRetornoDDLModel itemRet = new APIRetornoDDLModel();
                foreach (var item in lista)
                {
                    itemRet = new APIRetornoDDLModel();

                    itemRet.Valor = item[0].ToString();
                    itemRet.Descricao = item[1].ToString();

                    listaRetorno.Add(itemRet);
                }
                return listaRetorno;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
