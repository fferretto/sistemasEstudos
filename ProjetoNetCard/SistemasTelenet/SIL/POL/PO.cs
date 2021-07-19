/*  Camada Persistencia - Mapeamento Objeto Relacional */
using System;
using System.Collections.Generic;
using TELENET.SIL.BL;

namespace TELENET.SIL.PO
{
    //Junção
    public class VRESUMOCRE
    {
        public int ID_CREDENCIADO { get; set; }
        public int CODCRE { get; set; }
        public string RAZSOC { get; set; }
        public string NOMFAN { get; set; }
        public string NOMEXIBICAO { get; set; }
        public string CNPJ { get; set; }
        public string LOCALIDADE { get; set; }
        public string BAIRRO { get; set; }        
        public string STA { get; set; }
        public string DESTA { get; set; }
    }

    //Junção
    public class VRESUMOCLI : IDadosBasicosCliente
    {
        public int ID_CLIENTE { get; set; }
        public int CODCLI { get; set; }
        public int CODAG { get; set; }
        public string NOMCLI { get; set; }
        public int SISTEMA { get; set; }
        public string TIPOCARTAO { get { return SISTEMA == 0 ? "PÓS PAGO" : SISTEMA == 1 ? "PRÉ PAGO" : string.Empty; } }
        public string PRODUTO { get; set; }
        public string CNPJ { get; set; }
        public string STA { get; set; }
        public string DESTA { get; set; }
        public int MAXPARC { get; set; }
        public int LIMRISCO { get; set; }
        public string NOMLOC { get; set; }
        public string SUBREDE { get; set; }
        public int NUMCARG_VA { get; set; }
        public int MASCVALCARGCART { get; set; }
    }

    //Junção
    public class VRESUMOUSU
    {
        public int ID_USUARIO { get; set; }
        public string CPF { get; set; }
        public string CODCRT { get; set; }
        public string CODCRTMASC { get; set; }
        public string NOMUSU { get; set; }
        public string TIPUSU { get; set; }
        public string TIPO { get; set; }
        public int NUMDEP { get; set; }
        public int QTDDEP { get; set; }
        public int CODCLI { get; set; }
        public string NOMCLI { get; set; }
        public int SISTEMA { get; set; }
        public string TIPOCARTAO { get { return SISTEMA == 0 ? "PÓS PAGO" : SISTEMA == 1 ? "PRÉ PAGO" : string.Empty; } }
        public string PRODUTO { get; set; }        
        public string STA { get; set; }
        public string DESTA { get; set; }
        public string MAT { get; set; }
        public bool BLOQCARTUSU { get; set; }
        public decimal CARGAPAD { get; set; }
        public int MASCVALCARGCART { get; set; }
    }
    //Junção
    public class JustSegViaCard
    {
        public int codJus_Seg_Via_Card { get; set; }
        public string nomJustificativa { get; set; }
        public string CobrarSegundaVia { get; set; }
    }
    //Junção - Calculadora de parcelas de usuário pos
    public class CalcParcela
    {
        public int codCalcParcela { get; set; }
        public string nomCalcParcela { get; set; }
    }
    public class CalcValorParcela
    {
        public string CodCalcParcTotalCompra { get; set; }
        public string CodCalcParcValorParcela { get; set; }
    }

    //Junção
    public class VPRODUTOSCLI
    {
        public int ID_CLIENTE { get; set; }
        public int CODCLI { get; set; }
        public int CODREO { get; set; }
        public int CODEPS { get; set; }
        public string REGIONAL { get; set; }
        public string EPS { get; set; }
        public string NOMCLI { get; set; }
        public int SISTEMA { get; set; }
        public string PRODUTO { get; set; }
        public int CODPROD { get; set; }
        public string CNPJ { get; set; }
        public string SUBREDE { get; set; }
        public string PARCERIA { get; set; }
        public string GRUPOSOCIETARIO { get; set; }
        public DateTime DATULTCARG_VA { get; set; }
        public int NUMCARG_VA { get; set; }
        public decimal SALDOCONTA { get; set; }
        public short PRAPAG_VA { get; set; }
        public decimal TAXSER_VA { get; set; }
        public decimal TAXADM_VA { get; set; }
        public int NSUCARGA { get; set; }        
        public decimal TAXADESTIT { get; set; }
        public decimal TAXADESDEP { get; set; }
        public short NUMPAC { get; set; }
        public char PAGADES { get; set; }
        public char PGTOANTECIPADO { get; set; }
        public char HABCARGASEQ { get; set; }
        public char HABCPFTEMP { get; set; }
        public char HABTROCACPFTEMP { get; set; }
        public int MAXCARTTEMP { get; set; }
        public int QTCARTTEMP { get; set; }
        public int MASCQTCARTTEMP { get; set; }
        public int MASCVALMAXCARGCART { get; set; }
        public char HABCARGACARTTEMP { get; set; }
        public DateTime DATADES { get; set; }
        public short DIASVALSALDO { get; set; }
        public short CARENCIATROCACANC { get; set; }
        public decimal LIMMAXCAR { get; set; }
        public char TIPOTAXSER { get; set; }
        public string PRZVALCART { get; set; }
        public char COBCONS { get; set; }
        public decimal VALCONS { get; set; }
        public decimal CARGPADVA { get; set; }
        public decimal NEGARCARGASALDOACIMA { get; set; }
        public string STA { get; set; }
        public string STACOD
        {
            get
            {
                switch (STA)
                {
                    case "ATIVO"       : return "00";
                    case "BLOQUEADO"   : return "01";
                    case "CANCELADO"   : return "02";
                    case "SUSPENSO"    : return "06";
                    case "INADIMPLENTE": return "08";
                    case "EM RESCISAO" : return "09";
                    default: return "99";
                }
            }
        }
        public DateTime DATINC { get; set; }
        public DateTime DATSTA { get; set; }
        public char CTRATV { get; set; }        
        public string INSEST { get; set; }
        public int NUMCRT { get; set; }
        public int CODFILNUT { get; set; }
        public char COB2AV { get; set; }
        public decimal VAL2AV { get; set; }
        public bool COBINC { get; set; }
        public decimal VALINCTIT { get; set; }
        public decimal VALINCDEP { get; set; }
        public string NOMGRA { get; set; }
        public string NOME { get { return (CODCLI + " - " + NOMCLI); } }
        public short DATFEC { get; set; }
        public short NUMFEC { get; set; }
        public DateTime DATULTFEC { get; set; }        
        public int CODSUBREDE { get; set; }        
        public char EXIREC { get; set; }
        public char EXIBMES { get; set; }
        public decimal VALANUDEP { get; set; }
        public string TIPDES { get; set; }
        public char SUBMED { get; set; }
        public char PAGANU { get; set; }
        public char CONPMO { get; set; }
        public decimal VALTOTCRE { get; set; }
        public decimal TOTCREDUTIL { get; set; }
        public decimal VALGASTO { get; set; }
        public short PERSUB { get; set; }
        public char OUTCRT { get; set; }
        public short MAXPARC { get; set; }
        public int PMOEXCSEG { get; set; }
        public string ORDEMCL { get; set; }
        public char COBRAANU { get; set; }
        public char ANUIPERIODO { get; set; }
        public short NUMANUICOB { get; set; }
        public short LIMRISCO { get; set; }
        public char COBATV { get; set; }
        public decimal VALATV { get; set; }
        public char COBANUIMOV { get; set; }
        public short DADIAMENTO { get; set; }
        public char CRTINCBLQ { get; set; }
        public char SUBTIT { get; set; }
        public int CODMENS { get; set; }
        public char SALDOFUNC { get; set; }
        public char PROXCONPMO { get; set; }
        public char VERTOTCRE { get; set; }
        public char NRENOVPMO { get; set; }
        public char RENOVPGTO { get; set; }
        public char MOVDIACRED { get; set; }
        public DateTime DATCTT { get; set; }
        public decimal VALANUTIT { get; set; }
        public DateTime DATANU { get; set; }
        public short PRAPAG { get; set; }
        public string VANTECIPOU { get; set; }
        public DateTime DATPROXFEC { get; set; }
        public List<CLIENTEVA_OBS> OBSCLIVA { get; set; }
        public char NAOVERIFCPF { get; set; }
        public int TIPFEC { get; set; }
        public char TIPPAG { get; set; }
        public char LIBLIMWEB { get; set; }
        public decimal TXJUROS { get; set; }
        public decimal TXSERVIC { get; set; }
        public char HABFIN { get; set; }
        public string CODFIN { get; set; }
        public string CODLOGO1 { get; set; }
        public string CODLOGO2 { get; set; }
        public string CODMODELO1 { get; set; }
        public string CODMODELO2 { get; set; }
        public DateTime DATRESCISAO { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string EMA { get; set; }
        public string CON { get; set; }
        public bool POSSUIPREMIO { get; set; }        
        public string ENDEDC { get; set; }
        public string ENDCPLEDC { get; set; }
        public string BAIRROEDC { get; set; }
        public string LOCALIDADEEDC { get; set; }
        public string UFEDC { get; set; }
        public string CEPEDC { get; set; }
        public string RESEDC { get; set; }
        public int ATIVADOS { get; set; }
        public int NAO_ATIVADOS { get; set; }
        public int BLOQUEADOS { get; set; }

    }

    //Junção
    public class VCREDENCIADO
    {
        public int ID_CREDENCIADO { get; set; }
        public int CODCRE { get; set; }
        public string RAZSOC { get; set; }
        public string NOMEXIBICAO { get; set; }
        public string CNPJ_CPF { get; set; }
        public string SEGMENTO { get; set; }
        public string RAMO_ATIVIDADE { get; set; }
        public string TIPO { get; set; }
        public int CODCRE_MATRIZ { get; set; }
        public int QUANT_FILIAIS { get; set; }
        public char MENS_ATIV_CARTAO { get; set; }
        public string STATUS { get; set; }
        public string LOCAL_PAGTO { get; set; }
        public string TIPO_REEMBOLSO { get; set; }
        public string FILIAL_REDE { get; set; }
        public string SENHA { get; set; }
        public string CANAL { get; set; }
        public int CODREO { get; set; }
        public int CODEPS { get; set; }
        public string REGIONAL { get; set; }
        public string EPS { get; set; }
        public char FLAG_CAD_PJ { get; set; }
        public int TIPO_FECHAMENTO_PJ { get; set; }
        public string TIPO_FECHAMENTO_PJ_Text { get; set; }
        public int DIA_FECH_PJ { get; set; }
        public string DIA_FECH_PJ_Text { get; set; }
        public int PRAZO_PGTO { get; set; }
        public DateTime DATA_CONTRATO_PJ { get; set; }
        public DateTime DATA_INC_PJ { get; set; }
        public DateTime DATA_CONTRATO_VA { get; set; }
        public DateTime DATA_INC_VA { get; set; }
        public string CONTA_PJ { get; set; }
        public decimal TAXA_SERV { get; set; }
        public DateTime DATA_TAXA_SERV { get; set; }
        public int MAXPARC { get; set; }
        public char FLAG_CAD_VA { get; set; }
        public int TIPO_FECHAMENTO_PP { get; set; }
        public int DIA_FECH_PP { get; set; }
        public string TIPO_FECHAMENTO_PP_Text { get; set; }
        public string DIA_FECH_PP_Text { get; set; }
        public string CONTA_VA { get; set; }
        public decimal TAXA_ADM_PP { get; set; }
        public DateTime DATA_TAXA_ADM { get; set; }
        public string STA { get; set; }
        public string STACOD
        {
            get
            {
                switch (STA)
                {
                    case "ATIVO": return "00";
                    case "BLOQUEADO": return "01";
                    case "CANCELADO": return "02";
                    case "SUSPENSO": return "06";
                    default: return "99";
                }
            }
        }
        public DateTime DATSTA { get; set; }
        public VCREDENCIADO CODMAT { get; set; }
        public VCREDENCIADO CODCEN { get; set; }
        public VCREDENCIADO CODPRI { get; set; }
        public string TRANSHAB { get; set; }
        public int NUMFEC_PJ { get; set; }
        public int NUMFEC_VA { get; set; }
        public DateTime DATULTFEC_PJ { get; set; }
        public DateTime DATULTFEC_VA { get; set; }
        public int PRAZO_REE_VA { get; set; }
        public decimal LIM_REENT_PJ { get; set; }
        public decimal LIM_REENT_VA { get; set; }
    }

    //Junção
    public class CADCLIENTE
    {
        public int ID_CLIENTE { get; set; }
        public int SISTEMA { get; set; }
        public int CODCLI { get; set; }
        public string PRODUTO { get; set; }
        public string NOMCLI { get; set; }
        public string CNPJ { get; set; }
        public string NOVOCNPJ { get; set; }
        public string INSEST { get; set; }
        public string REGIAO { get; set; }
        public string RAMOATI { get; set; }
        public string UNIDADE { get; set; }
        public string SETOR { get; set; }
        public string PORTE { get; set; }
        public string ENDCLI { get; set; }
        public string ENDCPL { get; set; }
        public string BAIRRO { get; set; }
        public string LOCALIDADE { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
    }

    //Junção
    public class CADUSUARIO
    {
        public int ID_USUARIO { get; set; }
        public string CPF { get; set; }
        public string CODCRT { get; set; }
        public string CODCRTMASK { get; set; }
        public bool BLOQCARTUSU { get; set; }
        public string NOMUSU { get; set; }        
        public string MATRICULA { get; set; }
        public string TEL { get; set; }
        public string EMA { get; set; }
        public DateTime DATNAS { get; set; }
        public string CEL { get; set; }
        public string PAI { get; set; }
        public string MAE { get; set; }
        public string RG { get; set; }
        public string SEXO { get; set; }
        public string ORGEXPRG { get; set; }
        public string NATURALIDADE { get; set; }
        public string NACIONALIDADE { get; set; }
        public string NIS { get; set; }
        public string CEP { get; set; }
        public string ENDUSU { get; set; }
        public string ENDNUMUSU { get; set; }
        public string ENDCPL { get; set; }
        public string BAIRRO { get; set; }
        public string LOCALIDADE { get; set; }
        public string UF { get; set; }
        public string ENDUSUCOM { get; set; }
        public string ENDNUMCOM { get; set; }
        public string ENDNUMUSUCOM { get; set; }
        public string ENDCPLCOM { get; set; }
        public string BAIRROCOM { get; set; }
        public string LOCALIDADECOM { get; set; }
        public string UFCOM { get; set; }
        public string CEPCOM { get; set; }
        public string TELCOM { get; set; }
    }

    //Junção
    public class VPRODUTOSUSU
    {
        public int ID_USUARIO { get; set; }
        public int ID_CARTAO { get; set; }
        public int CODCLI { get; set; }
        public string NOMCLI { get; set; }
        public string STACLI { get; set; }
        public string DESTACLI { get; set; }
        public string NOMUSU { get; set; }
        public string CPF { get; set; }
        public int SISTEMA { get; set; }
        public int CODPROD { get; set; }
        public string PRODUTO { get; set; }
        public int TIPOPROD { get; set; }
        public short NUMDEP { get; set; }
        public short CODFIL { get; set; }
        public int CODPAR { get; set; }
        public DateTime DATATV { get; set; }
        public DateTime DATINC { get; set; }
        public string CODSET { get; set; }
        public string MAT { get; set; }
        public DateTime DATGERCRT { get; set; }
        public DateTime DTVAULT { get; set; }
        public string GERCRT { get; set; }
        public string CODCRT { get; set; }
        public string CODCRTANT { get; set; }
        public string STATUS { get; set; }
        public string STA { get; set; }
        public string STACOD
        {
            get
            {
                switch (STA)
                {
                    case "ATIVO": return "00";
                    case "BLOQUEADO": return "01";
                    case "CANCELADO": return "02";
                    case "SUSPENSO": return "06";
                    case "TRANSFERIDO": return "07";
                    default: return "99";
                }
            }
        }
        public DateTime DATSTA { get; set; }
        public bool BLOQCARTUSU { get; set; }
        public DateTime DATBLOQUSU { get; set; }
        public decimal CARGPADVA { get; set; }
        public int MASCVALMAXCARGCART { get; set; }
        public int ULTCARGVA { get; set; }
        public decimal VCARGAUTO { get; set; }
        public decimal SALDOPRE { get; set; }
        public decimal SALDOPOS { get; set; }
        public decimal SALDOCOMPROMETIDO { get; set; }
        public string SENHA { get; set; }
        public DateTime DTSENHA { get; set; }
        public DateTime DTEXPSENHA { get; set; }
        public int QTDEACESSOINV { get; set; }
        public short NUMPAC { get; set; }
        public short NUMULTPAC { get; set; }
        public DateTime DATRENADES { get; set; }
        public DateTime DATADES { get; set; }
        public decimal VALADES { get; set; }
        public string DTVALCART { get; set; }
        public DateTime DTULTRENOV { get; set; }
        public string TRILHA2 { get; set; }
        public string NOMCRT { get; set; }
        public string PRZVALCART { get; set; }
        public decimal PMO { get; set; }
        public decimal BONUS { get; set; }
        public decimal GASTO { get; set; }
        public decimal LIMPAD { get; set; }
        public string ALTLIMITE { get; set; }
        public string IMEDIATO { get; set; }
        public string CTRATV { get; set; }
        public decimal ACRESPAD { get; set; }
        public string AGENCIA { get; set; }
        public string CONTA { get; set; }
        public string BANCO { get; set; }
        public decimal LIMCREDITO { get; set; }
        public int MAXPARCCLI { get; set; }
        public int MAXPARCCRT { get; set; }
        public string HABTRANSDIG { get; set; }
        public int LIMRISCOCLI { get; set; }
        public int LIMRISCOCRT { get; set; }
        public string CODSIND { get; set; }
        public bool COBRASEGVIA { get; set; }
        public decimal VALORSEGVIA { get; set; }
        public int LOTEATUAL { get; set; }
        public List<VUSUARIODEP> DEPENDENTES { get; set; }
    }

    public class SALDO
    {
        public decimal SALDOPRE { get; set; }
        public decimal SALDOPOS { get; set; }
        public decimal VALCOMP { get; set; }
        public decimal GASTOATUAL { get; set; }
    }

    //Junção
    public class VUSUARIODEP 
    {
        public VUSUARIODEP()
        {
            EXCLUIDO = false;
        }

        public int ID_USUARIO { get; set; }
        public int ID_CARTAO { get; set; }
        public int CODCLI { get; set; }
        public string NOMCLI { get; set; }
        public string STACLI { get; set; }
        public string DESTACLI { get; set; }
        public string CPF { get; set; }
        public string NOMUSU { get; set; }
        public int SISTEMA { get; set; }
        public string PRODUTO { get; set; }
        public int TIPOPROD { get; set; }
        public short NUMDEP { get; set; }
        public short CODFIL { get; set; }
        public string SEXO { get; set; }
        public string DATNAS { get; set; }
        public int CODPAR { get; set; }
        public string DESPAR { get; set; }
        public DateTime DATATV { get; set; }
        public DateTime DATINC { get; set; }
        public string CODSET { get; set; }
        public string MAT { get; set; }
        public DateTime DATGERCRT { get; set; }
        public DateTime DTVAULT { get; set; }
        public string GERCRT { get; set; }
        public string CODCRT { get; set; }
        public string CODCRTANT { get; set; }
        public string CODCRTMASK { get { return UtilSIL.MascaraCartao(CODCRT, 17); } }
        public string STATUS { get; set; }
        public string STA { get; set; }
        public string DESTA { get; set; }
        public bool EXCLUIDO { get; set; }
        public string STACOD
        {
            get
            {
                switch (STA)
                {
                    case "ATIVO": return "00";
                    case "BLOQUEADO": return "01";
                    case "CANCELADO": return "02";
                    case "SUSPENSO": return "06";
                    case "TRANSFERIDO": return "07";
                    default: return "99";
                }
            }
        }
        public DateTime DATSTA { get; set; }
        public decimal CARGPADVA { get; set; }
        public int ULTCARGVA { get; set; }
        public decimal VCARGAUTO { get; set; }
        public decimal SALDOPRE { get; set; }
        public decimal SALDOPOS { get; set; }
        public string SENHA { get; set; }
        public DateTime DTSENHA { get; set; }
        public DateTime DTEXPSENHA { get; set; }
        public int QTDEACESSOINV { get; set; }
        public short NUMPAC { get; set; }
        public short NUMULTPAC { get; set; }
        public DateTime DATRENADES { get; set; }
        public DateTime DATADES { get; set; }
        public decimal VALADES { get; set; }
        public string DTVALCART { get; set; }
        public string TRILHA2 { get; set; }
        public string NOMCRT { get; set; }
        public string PRZVALCART { get; set; }
        public decimal PMO { get; set; }
        public decimal LIMPAD { get; set; }
        public string ALTLIMITE { get; set; }
        public string IMEDIATO { get; set; }
        public string CTRATV { get; set; }
        public decimal ACRESPAD { get; set; }
        public string AGENCIA { get; set; }
        public string CONTA { get; set; }
        public string BANCO { get; set; }
        public decimal LIMCREDITO { get; set; }
        public int MAXPARCCLI { get; set; }
        public int MAXPARCCRT { get; set; }
        public string HABTRANSDIG { get; set; }
        public int LIMRISCOCLI { get; set; }
        public int LIMRISCOCRT { get; set; }
        public string CODSIND { get; set; }
        public bool COBRASEGVIA { get; set; }
        public decimal VALORSEGVIA { get; set; }
        public int LOTEATUAL { get; set; }
        public string LIMDEP { get; set; }
    }

    //Junção
    public class CONSULTA
    {
        public int SISTEMA { get; set; }
        public int CODCONS { get; set; }
        public int CODCLI { get; set; }
        public int CODCRE { get; set; }
        public int CODTIPOCONS { get; set; }
        public int CODTIPOCONSINT { get; set; }
        public string DESCRICAO { get; set; }
        public string NOME_CONSULTA { get; set; }
        public DateTime PERIODO_INI { get; set; }
        public DateTime PERIODO_FIM { get; set; }
        public int LOTE { get; set; }
        public int NUM_HOST_INI { get; set; }
        public int NUM_HOST_FIM { get; set; }
        public int NUM_AUT_INI { get; set; }
        public int NUM_AUT_FIM { get; set; }
        public string NOME_USUARIO { get; set; }
        public string MAT_USUARIO { get; set; }
        public string CPF_USUARIO { get; set; }
        public int AGRUPAMENTO { get; set; }
        public string SUBREDE { get; set; }
        public string REDE { get; set; }
        public string TIPO_RESPOSTA { get; set; }
        public string TIPO_TRANSACAO { get; set; }
        public string NUM_CARTAO { get; set; }
        public string LISTA_CRED { get; set; }
        public string INTERVALO_CRED_INI { get; set; }
        public string INTERVALO_CRED_FIM { get; set; }
        public string LISTA_CLI { get; set; }
        public string LISTA_COL { get; set; }
        public string LISTA_COL_FORMATADA
        {
            get
            {
                var format = "";
                switch (LISTA_COL)
                {
                    case UtilSIL.DATTRA:
                        {
                            format = "DATA TRANSACAO";
                            break;
                        }

                    case UtilSIL.NSUHOS:
                        {
                            format = "NºHOST";
                            break;
                        }

                    case UtilSIL.NSUAUT:
                        {
                            format = "NºAUTORIZACAO";
                            break;
                        }

                    case UtilSIL.TIPTRA:
                        {
                            format = "TIPO TRANSACAO";
                            break;
                        }

                    case UtilSIL.DESTIPTRA:
                        {
                            format = "DESCRICAO TRANSACAO";
                            break;
                        }

                    case UtilSIL.VALTRA:
                        {
                            format = "VALOR";
                            break;
                        }

                    case UtilSIL.NOMUSU:
                        {
                            format = "NOME USUARIO";
                            break;
                        }

                    case UtilSIL.CODCRT:
                        {
                            format = "Nº CARTAO";
                            break;
                        }

                    case UtilSIL.CPF:
                        {
                            format = "CPF";
                            break;
                        }

                    case UtilSIL.MAT:
                        {
                            format = "MATRICULA_USU";
                            break;
                        }

                    case UtilSIL.CODCLI:
                        {
                            format = "COD.CLIENTE";
                            break;
                        }

                    case UtilSIL.NOMCLI:
                        {
                            format = "NOME CLIENTE";
                            break;
                        }

                    case UtilSIL.CODCRE:
                        {
                            format = "COD.CRED.";
                            break;
                        }

                    case UtilSIL.RAZSOC:
                        {
                            format = "RAZAO SOCIAL";
                            break;
                        }

                    case UtilSIL.NOMFAN:
                        {
                            format = "NOME FANTASIA";
                            break;
                        }

                    case UtilSIL.CGC:
                        {
                            format = "CNPJ CRED.";
                            break;
                        }

                    case UtilSIL.DATFECCRE:
                        {
                            format = "DT. FECH. CREDENCIADO";
                            break;
                        }

                    case UtilSIL.NUMFECCRE:
                        {
                            format = "Nº FECH. CREDENCIADO";
                            break;
                        }

                    case UtilSIL.CODRTA:
                        {
                            format = "TIPO RESPOSTA";
                            break;
                        }

                    case UtilSIL.NUMCARG_VA:
                        {
                            format = "NºCARGA";
                            break;
                        }

                    case UtilSIL.NUMDEP:
                        {
                            format = "NºSEQ. DEPT";
                            break;
                        }
                }

                return format;
            }
        }
        public string INTERVALO_CLI_INI { get; set; }
        public string INTERVALO_CLI_FIM { get; set; }
        public int NUM_FECH_CRED_INI { get; set; }
        public int NUM_FECH_CRED_FIM { get; set; }
        public DateTime DATA_FECH_CRED_INI { get; set; }
        public DateTime DATA_FECH_CRED_FIM { get; set; }
        public int OPERADOR { get; set; }
        public string HORA_PERIODO_INI { get; set; }
        public string HORA_PERIODO_FIM { get; set; }
        public int CODCEN { get; set; }
    }

    //Junção
    public class TAXACLIENTE
    {
        public int TIPO { get; set; }
        public string DESTIPO { get { return (this.TIPO == 1 ? "USUARIO" : this.TIPO == 3 ? "CLIENTE" : string.Empty ); } }
        public string TRENOVA { get; set; }
        public string TAXADEFAULT { get; set; }
        public string DESTRENOVA { get { return (this.TRENOVA == "A" ? "ANUAL" : this.TRENOVA == "M" ? "MENSAL" : ""); } }
        
    }

    //Junção
    public class TAXAUSUARIO 
    { 
        public int CODTAXA { get; set; }
        public int NUMPAC { get; set; }
        public int NUMULTPAC { get; set; }
        public DateTime DATRENOV { get; set; }
        public DateTime DATTAXA { get; set; }
        public decimal VALTAXA { get; set; }
        public string NOMTAXA { get; set; }
        public string CTRATV { get; set; }
        public string DESCTRATV { get { return (this.CTRATV == "S" ? "Cobra apartir da ativação" : "Cobra apartir da inclusão"); } }
        public string PAGANU { get; set; }
        public string DESPAGANU { get { return (this.CTRATV == "S" ? "Pelo Cliente" : "Pelo Usuário"); } }
        public string INDIVIDUAL { get; set; }
        public int TIPO { get; set; }
        public int NUMDEP  { get; set; }
        public string ABREVCLASSE { get; set; }
        public string TRENOVA { get; set; }
        public string DESTRENOVA { get { return (this.TRENOVA == "A" ? "ANUAL" : this.TRENOVA == "M" ? "MENSAL" : ""); } }
        public decimal VALDEP { get; set; }
        public string ASSOCIADA { get; set; }
        public string DESCASSOCIADA { get { return (this.ASSOCIADA == "S" ? "SIM" : "NÃO"); } }
    }

    //Junção
    public class BENEFICIOUSUARIO
    {
        public int CODBENEF { get; set; }
        public DateTime DATRENOV { get; set; }
        public DateTime DATBENEF { get; set; }
        public decimal VALBENEF { get; set; }
        public string NOMBENEF { get; set; }
        public string COMPULSORIO { get; set; }
        public string DESCOMPULSORIO { get { return (this.COMPULSORIO == "S" ? "Compulsório" : ""); } }
        public string SUBBENEF { get; set; }
        public int PERSUB { get; set; }
        public DateTime DTCARENCIA { get; set; }
        public DateTime DTVIGENCIA { get; set; }
        public DateTime DTASSOC { get; set; }
        public int NUMDEP { get; set; }
        public string TRENOVA { get; set; }
        public string DESTRENOVA { get { return (this.TRENOVA == "A" ? "ANUAL" : this.TRENOVA == "M" ? "MENSAL" : ""); } }
        public decimal VALDEP { get; set; }
        public string ASSOCIADA { get; set; }
        public string DESCASSOCIADA { get { return (this.ADERIDO == "S" ? "SIM" : "NÃO"); } }
        public string ADERIDO { get; set; }
        public string JAASSOC { get; set; }
    }

    //Junção
    public class CONTROLEACESSO
    {
        public int IDCOMP { get; set; }
        public int IDPERFIL { get; set; }
    }

    //Junção
    public class PERMISSAOACESSO
    {
        public Int32 IDCOMP { get; set; }
        public Int32 IDPERFIL { get; set; }
        public string DESCRICAO { get; set; }
        public string TIPOCONTROLE { get; set; }
    }
    
    //Junção
    public class COMPONENTESACESSO
    {
        public int ID { get; set; }
        public string FORM { get; set; }
        public string COMP { get; set; }
        public string FLGFORM { get; set; }
        public string DESCRICAO { get; set; }
        public string ACESSO { get; set; }
    }

    //Junção
    public class ACOESTRANS 
    {
        public bool CONFIRMAR { get; set; }
        public bool ALTERAR { get; set; }
        public bool CANCELAR { get; set; }
        public bool ALTVALOR { get; set; }
        public decimal VALOR { get; set; }
    }

    //Perfil
    public class PERFILACESSO
    {
        public int ID { get; set; }
        public int CODAG { get; set; }
        public string DESCRICAO { get; set; }
        public string DETALHAMENTO { get; set; }
    }

    //Junção
    public class PRODUTONOVO
    {
        public int CODPROD { get; set; }
        public string ROTULO { get; set; }
    }

    //Cartão Temporário
    public class CARTAOTEMPORARIO 
    {
        public char HABCPFTEMP { get; set; }
        public char HABTROCACPFTEMP { get; set; }
        public int MAXCARTTEMP { get; set; }
        public int QTCARTTEMP { get; set; }
        public char HABCARGACARTTEMP { get; set; }
    }

    public class ALERTA
    {
        public int ID_ALERTA { get; set; }
        public string REF { get; set; }

        public string ORIGEM
        {
            get
            {
                switch (REF)
                {
                    case "USU": return "USUÁRIO";
                    case "CLI": return "CLIENTE";
                    case "CRE": return "CREDENCIADO";
                    default: return "";
                }
            }
        }
        public DateTime DATA { get; set; }
        public int NIVEL { get; set; }
        public int TIPO { get; set; }
        public int SISTEMA { get; set; }
        public int CODCLI { get; set; }
        public int CODCRE { get; set; }
        public int ID_USUARIO { get; set; }
        public string CODCRT { get; set; }
        public string CODCRTMASC { get; set; }
        public int NUMDEP { get; set; }
        public int NUMTIT { get; set; }
        public string VISUALIZADO { get; set; }
        public int ID_FUNC { get; set; }
        public string LOGIN { get; set; }
        public string DESTIPO { get; set; }
        public string DESNIVEL { get; set; }
    }

    public class RESUMOIMPORTACAO
    {
        public string RegistroLog { get; set; }

        public char TipoRegistro { get; set; }
    }

    public class AGENDCANCCLIENTE
    {
        public int CODAGENDAMENTO { get; set; }
        public DateTime DATSOLICITACAO { get; set; }
        public DateTime DATACAO { get; set; }
        public string STATUS { get; set; }
        public int CODCLI { get; set; }
        public string STACLI { get; set; }
        public string NOMCLIENTE { get; set; }
        public string DESTACLI
        {
            get
            {
                switch (STATUS)
                {
                    case "00": return "ATIVO";
                    case "01": return "BLOQUEADO";
                    case "02": return "CANCELADO";
                    case "06": return "SUSPENSO";
                    default: return "";
                }
            }
        }
        public string PRODUTO { get; set; }
        public int SISTEMA { get; set; }
        public string TIPOCARTAO { get { return (this.SISTEMA == 0 ? "Pós Pago" : this.SISTEMA == 1 ? "Pré Pago" : ""); } }
        public int IDFUNC { get; set; }
        public string NOMOPERADOR { get; set; }
        public string MSGEXECUCAO { get; set; }
    }

    public class CLIENTEVA
    {
        public int CODCLI { get; set; }
        public int CODBAI { get; set; }
        public int CODSUBREDE { get; set; }
        public int CODAG { get; set; }
        public int CODPARCERIA { get; set; }
        public int CODPROD { get; set; }
        public int CODBAIEDC { get; set; }
        public int CODLOCEDC { get; set; }
        public string SIGUF0EDC { get; set; }
        public string CEPEDC { get; set; }
        public string RESEDC { get; set; }
        public string TELEDC { get; set; }
        public int CODREG { get; set; }
        public int CODREO { get; set; }
        public int CODEPS { get; set; }
        public int CODATI { get; set; }
        public int CODUNIDADE { get; set; }
        public int CODSETIND { get; set; }
        public int CODPORTE { get; set; }
        public DateTime DATULTCARG_VA { get; set; }
        public int NUMCARG_VA { get; set; }
        public decimal SALDOCONTA { get; set; }
        public short PRAPAG_VA { get; set; }
        public DateTime DATCTT_VA { get; set; }
        public decimal TAXSER_VA { get; set; }
        public decimal TAXADM_VA { get; set; }
        public int NSUCARGA { get; set; }
        public int CODVEND { get; set; }
        public decimal TAXADESTIT { get; set; }
        public decimal TAXADESDEP { get; set; }
        public short NUMPAC { get; set; }
        public char PAGADES { get; set; }
        public char PGTOANTECIPADO { get; set; }
        public DateTime DATADES { get; set; }
        public short DIASVALSALDO { get; set; }
        public decimal LIMMAXCAR { get; set; }
        public char TIPOTAXSER { get; set; }
        public string PRZVALCART { get; set; }
        public char COBCONS { get; set; }
        public decimal VALCONS { get; set; }
        public decimal CARGPADVA { get; set; }
        public string STA { get; set; }
        public DateTime DATINC { get; set; }
        public DateTime DATSTA { get; set; }
        public char CTRATV { get; set; }
        public string NOMCLI { get; set; }
        public string CGC { get; set; }
        public string NOVOCGC { get; set; }
        public string INSEST { get; set; }
        public string ENDCLI { get; set; }
        public string CEP { get; set; }
        public int NUMCRT { get; set; }
        public string OBS { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string EMA { get; set; }
        public int CODLOC { get; set; }
        public string SIGUF0 { get; set; }
        public int CODFILNUT { get; set; }
        public string CON { get; set; }
        public bool COB2AV { get; set; }
        public decimal VAL2AV { get; set; }
        public bool COBINC { get; set; }
        public decimal VALINCTIT { get; set; }
        public decimal VALINCDEP { get; set; }
        public string NOMGRA { get; set; }
        public string ENDCPL { get; set; }
        public string ENDEDC { get; set; }
        public string ENDCPLEDC { get; set; }
        public string NOME { get { return (CODCLI + " - " + NOMCLI); } }
        public short DATFEC { get; set; }
        public short NUMFEC { get; set; }
        public DateTime DATULTFEC { get; set; }
        public DateTime DATRESCISAO { get; set; }
        public string CODLOGO1 { get; set; }
        public string CODLOGO2 { get; set; }
        public string CODMODELO1 { get; set; }
        public string CODMODELO2 { get; set; }
        public string CRTINCBLQ { get; set; }

        public List<CLIENTEVA_OBS> OBSCLIVA { get; set; }
    }

    public class TRANSACVA
    {
        public DateTime DATTRA { get; set; }
        public int NSUHOS { get; set; }
        public int NSUAUT { get; set; }
        public int CODCLI { get; set; }
        public int CODCRE { get; set; }
        public string TIPTRA { get; set; }
        public string CODCRT { get; set; }
        public char CODRTA { get; set; }
        public decimal VALTRA { get; set; }
        public string CPF { get; set; }
        public short NUMDEP { get; set; }
        public int NUMCARG_VA { get; set; }
        public DateTime DATFECCRE { get; set; }
        public int NUMFECCRE { get; set; }
        public string DAD { get; set; }
        public char TIPDEB { get; set; }
        public char ATV { get; set; }
        public short CODOPE { get; set; }
        public char CONFERIDA { get; set; }
        public DateTime DTCARGA { get; set; }
    }

    public class CADUSUVAWEB
    {
        public string CPF { get; set; }
        public int CODCLI { get; set; }
        public string NOME { get; set; }
        public string ENDERECO { get; set; }
        public string BAIRRO { get; set; }
        public string CEP { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string MATRICULA { get; set; }
        public string TEL { get; set; }
        public string EMA { get; set; }
        public DateTime DTNASCIMENTO { get; set; }
    }

    public class OBSCLIVA
    {
        public int CODCLI { get; set; }
        public DateTime DATA { get; set; }
        public string OBS { get; set; }
        public int ID { get; set; }
    }

    public class TIPTRANS
    {
        public int TIPTRA { get; set; }
        public string DESTIPTRA { get; set; }
    }

    public class TAXAUSU
    {
        public int CODCLI { get; set; }
        public string CPF { get; set; }
        public short NUMDEP { get; set; }
        public int CODTAXA { get; set; }
        public short NUMPAC { get; set; }
        public short NUMULTPAC { get; set; }
        public DateTime DATRENOV { get; set; }
        public DateTime DATTAXA { get; set; }
        public decimal VALTAXA { get; set; }
    }

    public class MOTIVO
    {
        public int CODMOT { get; set; }
        public string MOTIVO_ { get; set; }
    }

    public class REGIONAL
    {
        public int CODREO { get; set; }
        public string DESREO { get; set; }
    }

    public class LOGOCARTAO
    {
        public string CODLOGO { get; set; }
        public string DESCRICAO { get; set; }
        public string CODDESCRICAO { get; set; }
    }

    public class MODELOCARTAO
    {
        public string CODMODELO { get; set; }
        public string DESCRICAO { get; set; }
        public string CODDESCRICAO { get; set; }
    }

    public class USUARIOVA
    {
        public int ID { get; set; }
        public CLIENTE CODCLI { get; set; }
        public string CPF { get; set; }
        public string CELULAR { get; set; }
        public short NUMDEP { get; set; }
        public short CODFIL { get; set; }
        public int CODPAR { get; set; }
        public string NOMUSU { get; set; }
        public DateTime DATATV { get; set; }
        public DateTime DATINC { get; set; }
        public string CODSET { get; set; }
        public string MAT { get; set; }
        public DateTime DATGERCRT { get; set; }
        public char GERCRT { get; set; }
        public string CODCRT { get; set; }
        public string STA { get; set; }
        public DateTime DATSTA { get; set; }
        public decimal CARGPADVA { get; set; }
        public int ULTCARGVA { get; set; }
        public decimal VCARGAUTO { get; set; }
        public string SENHA { get; set; }
        public short NUMPAC { get; set; }
        public short NUMULTPAC { get; set; }
        public DateTime DATRENADES { get; set; }
        public DateTime DATADES { get; set; }
        public decimal VALADES { get; set; }
        public string DTVALCART { get; set; }
        public string TRILHA2 { get; set; }
        public string NOMCRT { get; set; }
        public string DATNAS { get; set; }
        public string PAI { get; set; }
        public string MAE { get; set; }
        public string RG { get; set; }
        public string ORGEXPRG { get; set; }
        public string NATURALIDADE { get; set; }
        public string NACIONALIDADE { get; set; }
        public string ENDUSU { get; set; }
        public string ENDNUMUSU { get; set; }
        public string ENDCPL { get; set; }
        public string CODBAI { get; set; }
        public string CODLOC { get; set; }
        public string SIGUF0 { get; set; }
        public string CEP { get; set; }
        public string ENDUSUCOM { get; set; }
        public string ENDNUMCOM { get; set; }
        public string ENDNUMUSUCOM { get; set; }
        public string ENDCPLCOM { get; set; }
        public string CODBAICOM { get; set; }
        public string CODLOCCOM { get; set; }
        public string SIGUF0COM { get; set; }
        public string CEPCOM { get; set; }
    }

    public class COMPACESSOVA
    {
        public int CODFORM { get; set; }
        public int CODITEM { get; set; }
        public string NOMECOMP { get; set; }
        public int CODITEMPAI { get; set; }
        public string DESCRICAO { get; set; }
        public char ReadOnly { get; set; }
        public char Color { get; set; }
        public char Enabled { get; set; }
    }

    public class GRUPOCRED
    {
        public int CODGRUPO { get; set; }
        public int CODCRE { get; set; }
    }

    public class TRANSACAO
    {
        public DateTime DATTRA { get; set; }
        public int NSUHOS { get; set; }
        public int NSUAUT { get; set; }
        public int CODCLI { get; set; }
        public int CODCRE { get; set; }
        public string TIPTRA { get; set; }
        public string CODCRT { get; set; }
        public char CODRTA { get; set; }
        public decimal VALTRA { get; set; }
        public char REC { get; set; }
        public string CPF { get; set; }
        public short NUMDEP { get; set; }
        public DateTime DATFECCLI { get; set; }
        public int NUMFECCLI { get; set; }
        public DateTime DATFECCRE { get; set; }
        public int NUMFECCRE { get; set; }
        public string DAD { get; set; }
        public char TIPDEB { get; set; }
        public char ATV { get; set; }
        public short CODOPE { get; set; }
        public char CONFERIDA { get; set; }
        public string NUMFECHAUT { get; set; }
        public int CODCLA { get; set; }
        public int CODGRUPO { get; set; }
        public string DATNAS { get; set; }
        public string PAI { get; set; }
        public string MAE { get; set; }
        public string RG { get; set; }
        public string ORGEXPRG { get; set; }
        public string NATURALIDADE { get; set; }
        public string NACIONALIDADE { get; set; }
        public string ENDUSU { get; set; }
        public string ENDNUMUSU { get; set; }
        public string ENDCPL { get; set; }
        public string CODBAI { get; set; }
        public string CODLOC { get; set; }
        public string SIGUF0 { get; set; }
        public string CEP { get; set; }
        public string ENDUSUCOM { get; set; }
        public string ENDNUMUSUCOM { get; set; }
        public string ENDCPLCOM { get; set; }
        public string CODBAICOM { get; set; }
        public string CODLOCCOM { get; set; }
        public string SIGUF0COM { get; set; }
        public string CEPCOM { get; set; }
    }

    public class PARAMVA
    {
        public string ID0 { get; set; }
        public string VAL { get; set; }
        public int NUMSEQBP { get; set; }
    }

    public class OPERCLIVAWEB
    {
        public int CODCLI { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
        public string NOME { get; set; }
        public char FINCCART { get; set; }
        public char FBLOQCART { get; set; }
        public char FDESBLOQCART { get; set; }
        public char FCANCCART { get; set; }
        public char FSEGVIACART { get; set; }
        public char FEXTMOV { get; set; }
        public char FCONSREDE { get; set; }
        public char FLISTTRANSAB { get; set; }
        public char FLISTCART { get; set; }
        public char FLISTINCCART { get; set; }
        public char FCARGA { get; set; }
        public int ID { get; set; }
    }

    public class OPERADOR
    {
        public short CODOPE { get; set; }
        public string NOMOPE { get; set; }
        public string LOGOPE { get; set; }
        public string SEN { get; set; }
        public char CLAOPE { get; set; }
        public byte[] MAPACE { get; set; }
    }

    public class REEMBOLSO
    {
        public short FORPAG { get; set; }
        public string DESFORPAG { get; set; }
    }

    public class CTRLACESSO
    {
        public int CODOPE { get; set; }
        public COMPACESSO CODFORM { get; set; }
        public int CODITEM { get; set; }
        public char ACESSO { get; set; }
    }

    public class SUBREDE
    {
        public int CODSUBREDE { get; set; }
        public string NOMSUBREDE { get; set; }
        public string CNPJ { get; set; }
        public string RAZAOSOCIAL { get; set; }
        public string CEP { get; set; }
        public string LOGRADOURO { get; set; }
        public string NROLOGRADOURO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
    }

    public class REDECAPTURA
    {
        public string REDE { get; set; }
        public string NOME { get; set; }
    }

    public class PARCERIA
    {
        public int CODPARCERIA { get; set; }
        public string NOMPARCERIA { get; set; }
    }

    public class GRUPOSOCIETARIO
    {
        public int CODGRUPOSOC { get; set; }
        public string NOMGRUPOSOC { get; set; }
    }

    public class UNIDADE
    {
        public int CODUNIDADE { get; set; }
        public string NOMUNIDADE { get; set; }
    }

    public class SETORIND
    {
        public int CODSETIND { get; set; }
        public string NOMSETOR { get; set; }
    }

    public class PORTE
    {
        public int CODPORTE { get; set; }
        public string NOMPORTE { get; set; }
    }

    public class PRODUTO
    {
        public int CODPROD { get; set; }
        public string ROTULO { get; set; }
        public int SISTEMA { get; set; }
        public int TIPOPROD { get; set; }
        public string DESTIPOPROD { get; set; }
        public string USO { get; set; }
        public string RIBBON { get; set; }
        public string PLASTICO { get; set; }
        public string ARTE_CARTAO { get; set; }
        public string TIPOCARTAO { get { return (this.SISTEMA == 0 ? "Pós Pago" : this.SISTEMA == 1 ? "Pré Pago" : ""); } }
    }

    public class TIPOPRODUTO
    {
        public string SISTEMA { get; set; }
        public int TIPOPROD { get; set; }
        public string DESCRICAO { get; set; }
    }

    public class USO
    {
        public char TIPOUSO { get; set; }
        public string DESCRICAO { get; set; }
    }

    public class EPS
    {
        public int CODREO { get; set; }
        public int CODEPS { get; set; }
        public int CODEPSPAI { get; set; }
        public string NOMEPS { get; set; }
        public string CGCEPS { get; set; }
        public string CPFEPS { get; set; }
        public string ENDEPS { get; set; }
        public string ENDCPLEPS { get; set; }
        public string CEPEPS { get; set; }
        public int CODLOCEPS { get; set; }
        public int CODBAIEPS { get; set; }
        public string SIGUF0EPS { get; set; }
        public char TIPEPS { get; set; }
    }

    public class FECHCRED
    {
        public int CODCRE { get; set; }
        public int NUMFECCRE { get; set; }
        public DateTime DATINI { get; set; }
        public DateTime DATFIN { get; set; }
        public short CODOPE { get; set; }
        public decimal TAXSER { get; set; }
        public decimal VALPAG { get; set; }
        public int QTETRA { get; set; }
        public int QTETRAVAD { get; set; }
        public DateTime DATFECLOT { get; set; }
        public string NUMBOR { get; set; }
        public decimal VALBOR { get; set; }
        public int QTETRABOR { get; set; }
        public short PRAPAG { get; set; }
        public int TIPFEC { get; set; }
        public string CTABCO { get; set; }
        public int CODCEN { get; set; }
        public short FORPAG { get; set; }
        public int CODFEN { get; set; }
        public DateTime DATBOR { get; set; }
        public int QTELANC { get; set; }
        public decimal VALLANC { get; set; }
        public int QTEALANC { get; set; }
        public decimal VALALANC { get; set; }
        public DateTime DATPGTO { get; set; }
    }

    public class FECHCREDVA
    {
        public int CODCRE { get; set; }
        public int NUMFECCRE { get; set; }
        public DateTime DATINI { get; set; }
        public DateTime DATFIN { get; set; }
        public short CODOPE { get; set; }
        public decimal TAXADM { get; set; }
        public decimal VALPAG { get; set; }
        public int QTETRA { get; set; }
        public int QTETRAVAD { get; set; }
        public DateTime DATFECLOT { get; set; }
        public string NUMBOR { get; set; }
        public decimal VALBOR { get; set; }
        public int QTETRABOR { get; set; }
        public short PRAPAG { get; set; }
        public int TIPFEC { get; set; }
        public string CTABCO { get; set; }
        public int CODCEN { get; set; }
        public short FORPAG { get; set; }
        public int CODFEN { get; set; }
        public DateTime DATBOR { get; set; }
        public decimal ANUIDADE { get; set; }
        public DateTime DATPGTO { get; set; }
    }

    public class CADCREDENCIADO {
        public int ID_CREDENCIADO { get; set; }
        public int CODCRE { get; set; }
        public string CNPJ_CPF { get; set; }
        public string NOVO_CNPJ_CPF { get; set; }
        public string RAZAO { get; set; }
        public string FANTASIA { get; set; }
        public string NOMEXIBICAO { get; set; }
        public string INSC_ESTADUAL { get; set; }
        public string REGIAO { get; set; }
        public string ENDERECO { get; set; }
        public string COMP { get; set; }
        public string BAIRRO { get; set; }
        public string LOCALIDADE { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string TELEFONE { get; set; }
        public string FAX { get; set; }
        public string EMAIL { get; set; }
        public string CONTATO { get; set; }
        public string ENDERECO_CORRESP { get; set; }
        public string COMP_CORRESP { get; set; }
        public string BAIRRO_CORRESP { get; set; }
        public string LOCALIDADE_CORRESP { get; set; }
        public string UF_CORRESP { get; set; }
        public string CEP_CORRESP { get; set; }
    }

    public class CREDENCIADO
    {
        public int ID_CREDENCIADO { get; set; }
        public int CODCRE { get; set; }
        public int CODLOC { get; set; }
        public string ENDCRE { get; set; }
        public string OBS { get; set; }
        public string CEP { get; set; }
        public string INSEST { get; set; }
        public string CGC { get; set; }
        public int CODFILNUT { get; set; }
        public string SENCRE { get; set; }
        public string SENWEB { get; set; }
        public short PRAPAG { get; set; }
        public string ENDCPL { get; set; }
        public short TIPEST { get; set; }
        public CREDENCIADO CODMAT { get; set; }
        public short QTEFIL { get; set; }
        public short QTEMAQ { get; set; }
        public short LOCPAG { get; set; }
        public CREDENCIADO CODCEN { get; set; }
        public DateTime DATGERCRT { get; set; }
        public char GERCRT { get; set; }
        public string ENDCOR { get; set; }
        public string ENDCPLCOR { get; set; }
        public int CODBAICOR { get; set; }
        public int CODLOCCOR { get; set; }
        public string SIGUF0COR { get; set; }
        public string CEPCOR { get; set; }
        public short FORPAG { get; set; }
        public int CODCAN { get; set; }
        public int CODREG { get; set; }
        public string NOMREG { get; set; }
        public int CODREO { get; set; }
        public int CODEPS { get; set; }
        public string TRANSHAB { get; set; }
        public char FLAG_PJ { get; set; }
        public string MASCONTA { get; set; }
        public char FLAG_VA { get; set; }
        public short INTVENDINI { get; set; }
        public short INTVENDFIN { get; set; }
        public DateTime CTRLFUNC { get; set; }
        public string PROPRIETARIO { get; set; }
        public string IDENTIDADE { get; set; }
        public short CATEGORIA { get; set; }
        public int DIAFEC_VA { get; set; }
        public short PRAENT { get; set; }
        public short PRAREE { get; set; }
        public short PRAREE_VA { get; set; }
        public decimal TAXADM { get; set; }
        public decimal TAXADM_VA { get; set; }
        public DateTime DATTAXADM { get; set; }
        public DateTime DATTAXADM_VA { get; set; }
        public char SINDIC { get; set; }
        public decimal LIMREENT { get; set; }
        public decimal LIMREENT_VA { get; set; }
        public string RESP { get; set; }
        public string CTABCO_VA { get; set; }
        public int NUMFEC_VA { get; set; }
        public DateTime DATULTFEC_VA { get; set; }
        public int TIPFEC_VA { get; set; }
        public DateTime DATCTT_VA { get; set; }
        public int LAYADIVA { get; set; }
        public int LAYADIPJ { get; set; }
        public short NUMPACVA { get; set; }
        public short NUMULTPACVA { get; set; }
        public decimal VALANUVA { get; set; }
        public DateTime DTANUVA { get; set; }
        public DateTime DTRENANU { get; set; }
        public char AUTARQSF { get; set; }
        public int CODBAI { get; set; }
        public int CODATI { get; set; }
        public int CODATIAUX { get; set; }
        public int CODSEG { get; set; }
        public string SIGUF0 { get; set; }
        public string STA { get; set; }
        public string RAZSOC { get; set; }
        public string NOMFAN { get; set; }
        public string TEL { get; set; }
        public string COA { get; set; }
        public DateTime DATINC { get; set; }
        public DateTime DATINC_VA { get; set; }
        public DateTime DATCTT { get; set; }
        public DateTime DATSTA { get; set; }
        public string CTABCO { get; set; }
        public decimal TAXSER { get; set; }
        public DateTime DATTAXSER { get; set; }
        public char MSGATVCRT { get; set; }
        public short QTDPOS { get; set; }
        public string EMA { get; set; }
        public string FAX { get; set; }
        public int TIPFEC { get; set; }
        public int DIAFEC { get; set; }
        public int NUMFEC { get; set; }
        public DateTime DATULTFEC { get; set; }
        public DateTime DATPRCULTFEC { get; set; }
        public string NOME { get { return (string.Format("{0} - {1}", CODCRE, RAZSOC)); } }
        public CREDENCIADO CODPRI { get; set; }
    }

    public class _4DATAS
    {
        public string DIA1 { get; set; }
        public string DIA2 { get; set; }
        public string DIA3 { get; set; }
        public string DIA4 { get; set; }
    }

    public class MENSAGEM
    {
        public int CODMENS { get; set; }
        public string LINHA1 { get; set; }
        public string LINHA2 { get; set; }
    }

    public class CARGAC
    {
        public int CODCLI { get; set; }
        public int NUMCARG_VA { get; set; }
        public DateTime DTAUTORIZ { get; set; }
        public DateTime DTCARGA { get; set; }
        public decimal VALOR { get; set; }
        public decimal TAXSER { get; set; }
        public decimal VAL2AVIA { get; set; }
        public short PRAPAG { get; set; }
        public short CODOPE { get; set; }
        public DateTime DTPROG { get; set; }
        public decimal TAXADESAO { get; set; }
        public short LIBSUP1 { get; set; }
        public short LIBSUP2 { get; set; }
        public char FINALIZADA { get; set; }
    }

    public class LOGWEB
    {
        public DateTime DATA { get; set; }
        public int CODCLI { get; set; }
        public string LOGIN { get; set; }
        public short CODOPE { get; set; }
        public string OPERACAO { get; set; }
        public string CPF { get; set; }
        public string CODCRT { get; set; }
    }

    public class ATIVIDADE
    {
        public int CODATI { get; set; }
        public string NOMATI { get; set; }
    }

    public class SEG
    {
        public int CODSEG { get; set; }
        public string NOMSEG { get; set; }
    }

    public class CADUSUWEB
    {
        public string CPF { get; set; }
        public int CODCLI { get; set; }
        public string NOME { get; set; }
        public string ENDERECO { get; set; }
        public string BAIRRO { get; set; }
        public string CEP { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string MATRICULA { get; set; }
        public string TEL { get; set; }
        public string EMA { get; set; }
        public DateTime DTNASCIMENTO { get; set; }
    }

    public class OPERCLIWEB
    {
        public int CODCLI { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
        public string NOME { get; set; }
        public char FINCCART { get; set; }
        public char FBLOQCART { get; set; }
        public char FDESBLOQCART { get; set; }
        public char FCANCCART { get; set; }
        public char FALTLIMITE { get; set; }
        public char FSEGVIACART { get; set; }
        public char FEXTMOV { get; set; }
        public char FCONSREDE { get; set; }
        public char FLISTTRANSAB { get; set; }
        public char FLISTCART { get; set; }
        public char FLISTINCCART { get; set; }
        public char FCARGA { get; set; }
    }

    public class TAXAFECHCLI
    {
        public int CODCLI { get; set; }
        public int NUMFECCLI { get; set; }
        public int CODTAXA { get; set; }
        public char PAGANU { get; set; }
    }

    public class GRUPO
    {
        public int CODGRUPO { get; set; }
        public string NOMGRUPO { get; set; }
        public string SISTEMA { get; set; }
    }

    public class PARAM
    {
        public string ID0 { get; set; }
        public string VAL { get; set; }
    }

    public class USUARIO
    {
        public int CODCLI { get; set; }
        public string CPF { get; set; }
        public string CONTA { get; set; }
        public string BANCO { get; set; }
        public int CODCLA { get; set; }
        public DateTime DATCLA { get; set; }
        public string TRILHA2 { get; set; }
        public int CODBAI { get; set; }
        public string ENDUSU { get; set; }
        public string ENDCPL { get; set; }
        public string CEP { get; set; }
        public int CODLOC { get; set; }
        public string SIGUF0 { get; set; }
        public string TEL { get; set; }
        public string CELULAR { get; set; }
        public string EMA { get; set; }
        public string RG { get; set; }
        public string LOCALTRAB { get; set; }
        public string PAI { get; set; }
        public string MAE { get; set; }
        public char SEXO { get; set; }
        public string CONJUGE { get; set; }
        public char SEXOCONJ { get; set; }
        public decimal RENDA { get; set; }
        public DateTime DATATUALIZ { get; set; }
        public short NUMDEP { get; set; }
        public short CODFIL { get; set; }
        public int CODPAR { get; set; }
        public string NOMUSU { get; set; }
        public short NUMPAC { get; set; }
        public short NUMULTPAC { get; set; }
        public DateTime DATRENANU { get; set; }
        public DateTime DATATV { get; set; }
        public decimal PMO { get; set; }
        public decimal LIMPAD { get; set; }
        public DateTime DATINC { get; set; }
        public string CODSET { get; set; }
        public string MAT { get; set; }
        public DateTime DATANU { get; set; }
        public DateTime DATGERCRT { get; set; }
        public char GERCRT { get; set; }
        public decimal VALANU { get; set; }
        public string CODCRT { get; set; }
        public string STA { get; set; }
        public DateTime DATSTA { get; set; }
        public char CTRATV { get; set; }
        public string GENERICO { get; set; }
        public decimal ACRESPAD { get; set; }
        public string AGENCIA { get; set; }
    }

    public class OBSCLI
    {
        public int CODCLI { get; set; }
        public DateTime DATA { get; set; }
        public string OBS { get; set; }
    }

    public class OBSUSU
    {
        public int CODCLI { get; set; }
        public string CPF { get; set; }
        public DateTime DATA { get; set; }
        public string OBS { get; set; }
    }

    public class SEGAUTORIZVA
    {
        public int CODSEG { get; set; }
        public int CODCLI { get; set; }
    }

    public class CTRLACESSOVA
    {
        public short CODOPE { get; set; }
        public int CODFORM { get; set; }
        public int CODITEM { get; set; }
        public char ACESSO { get; set; }
    }

    public class LOGVAWEB
    {
        public DateTime DATA { get; set; }
        public int CODCLI { get; set; }
        public string LOGIN { get; set; }
        public short CODOPE { get; set; }
        public string OPERACAO { get; set; }
        public string CPF { get; set; }
        public string CODCRT { get; set; }
    }

    public class OBSCRED
    {
        public int CODCRE { get; set; }
        public DateTime DATA { get; set; }
        public string OBS { get; set; }
    }

    public class TAXACLI
    {
        public int CODCLI { get; set; }
        public int CODTAXA { get; set; }
        public decimal VALTIT { get; set; }
        public decimal VALDEP { get; set; }
        public short NUMPAC { get; set; }
        public char CTRATV { get; set; }
        public DateTime DTINICIO { get; set; }
        public char COBCANC { get; set; }
        public char COBUTIL { get; set; }
        public char PAGANU { get; set; }
        public char COBUTILGRUPO { get; set; }
        public char INDIVIDUAL { get; set; }
        public char COBCANCUTIL { get; set; }
    }

    public class REGIAO
    {
        public int CODREG { get; set; }
        public string DESREG { get; set; }
        public int CODREGPAI { get; set; }
    }

    public class CANAL
    {
        public short CODCAN { get; set; }
        public string DESCAN { get; set; }
    }

    public class CLASSE
    {
        public int CODCLA { get; set; }
        public string NOMCLASSE { get; set; }
        public string ABREVCLASSE { get; set; }
    }

    public class TAXA
    {
        public int CODTAXA { get; set; }
        public string TIPTRA { get; set; }
        public string NOMTAXA { get; set; }
        public string ABREVTAXA { get; set; }
        public char TRENOVA { get; set; }
        public string STATUS { get; set; }
        public int CODCLA { get; set; }
        public int SISTEMA { get; set; }
    }

    public class FILNUTRI
    {
        public int CODFILNUT { get; set; }
        public string NOMFILNUT { get; set; }
    }

    public class OPERADORVA
    {
        public short CODOPE { get; set; }
        public string NOMOPE { get; set; }
        public string LOGOPE { get; set; }
        public string SEN { get; set; }
        public char CLAOPE { get; set; }
        public byte[] MAPACE { get; set; }
        public int IDPERFIL { get; set; }
    }

    public class SEGMENTO
    {
        public int CODSEG { get; set; }
        public string NOMSEG { get; set; }
        public int TIPO { get; set; }
    }

    public class PARENTESCO
    {
        public short CODPAR { get; set; }
        public string DESPAR { get; set; }
    }

    public class VENDEDOR
    {
        public int CODVEND { get; set; }
        public string NOMVEND { get; set; }
    }

    public class CLIENTE
    {
        public int CODCLI { get; set; }
        public int CODBAI { get; set; }
        public int CODSUBREDE { get; set; }
        public string SIGUF0 { get; set; }
        public char EXIREC { get; set; }
        public int CODFILNUT { get; set; }
        public string CON { get; set; }
        public decimal VALANUDEP { get; set; }
        public char TIPDES { get; set; }
        public char SUBMED { get; set; }
        public char PAGANU { get; set; }
        public char CONPMO { get; set; }
        public decimal VAL2AV { get; set; }
        public string NOMGRA { get; set; }
        public string ENDCPL { get; set; }
        public double VALTOTCRE { get; set; }
        public string ENDEDC { get; set; }
        public string ENDCPLEDC { get; set; }
        public int CODBAIEDC { get; set; }
        public int CODLOCEDC { get; set; }
        public string SIGUF0EDC { get; set; }
        public string CEPEDC { get; set; }
        public string RESEDC { get; set; }
        public string TELEDC { get; set; }
        public int CODREG { get; set; }
        public int CODATI { get; set; }
        public int CODREO { get; set; }
        public short CODEPS { get; set; }
        public short PERSUB { get; set; }
        public char OUTCRT { get; set; }
        public short OUTLAY { get; set; }
        public short MAXPARC { get; set; }
        public int PMOEXCSEG { get; set; }
        public char ORDEMCL { get; set; }
        public char COB2AV { get; set; }
        public char COBRAANU { get; set; }
        public char ANUIPERIODO { get; set; }
        public short NUMANUICOB { get; set; }
        public short LIMRISCO { get; set; }
        public char COBATV { get; set; }
        public decimal VALATV { get; set; }
        public char COBANUIMOV { get; set; }
        public short DADIAMENTO { get; set; }
        public char CRTINCBLQ { get; set; }
        public int CODMENS { get; set; }
        public char SALDOFUNC { get; set; }
        public char PROXCONPMO { get; set; }
        public char COBCONS { get; set; }
        public decimal VALCONS { get; set; }
        public char VERTOTCRE { get; set; }
        public char NRENOVPMO { get; set; }
        public char RENOVPGTO { get; set; }
        public char MOVDIACRED { get; set; }
        public string STA { get; set; }
        public DateTime DATINC { get; set; }
        public DateTime DATCTT { get; set; }
        public DateTime DATSTA { get; set; }
        public char CTRATV { get; set; }
        public decimal VALANUTIT { get; set; }
        public DateTime DATANU { get; set; }
        public short NUMPAC { get; set; }
        public string NOMCLI { get; set; }
        public string CGC { get; set; }
        public string INSEST { get; set; }
        public string ENDCLI { get; set; }
        public string CEP { get; set; }
        public short PRAPAG { get; set; }
        public int NUMCRT { get; set; }
        public string OBS { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public short DATFEC { get; set; }
        public short NUMFEC { get; set; }
        public DateTime DATULTFEC { get; set; }
        public DateTime DATPRCULTFEC { get; set; }
        public string EMA { get; set; }
        public int CODLOC { get; set; }
        public List<OBSCLI> OBSCLI { get; set; }
    }

    public class STATUS
    {
        public string STA { get; set; }
        public string DESTA { get; set; }
    }

    public class COMPACESSO
    {
        public int CODFORM { get; set; }
        public int CODITEM { get; set; }
        public string NOMECOMP { get; set; }
        public int CODITEMPAI { get; set; }
        public string DESCRICAO { get; set; }
        public char ReadOnly { get; set; }
        public char Color { get; set; }
        public char Enabled { get; set; }
    }

    public class FECHCLIENTE
    {
        public int CODCLI { get; set; }
        public int NUMFECCLI { get; set; }
        public DateTime DATINI { get; set; }
        public DateTime DATFIN { get; set; }
        public short CODOPE { get; set; }
        public DateTime DATFECLOT { get; set; }
        public short PRAPAG { get; set; }
        public char PAGANU { get; set; }
        public char CONPMO { get; set; }
        public char SUBMED { get; set; }
        public short PERSUB { get; set; }
        public int TOTTRANS { get; set; }
        public decimal TOTAL { get; set; }
        public decimal COMPCREC { get; set; }
        public decimal ANUIDADE { get; set; }
        public decimal VAL2VIA { get; set; }
        public decimal COMPRAS { get; set; }
        public decimal PREMIO { get; set; }
        public decimal DESCFOLHA { get; set; }
    }

    public class PERFILACESSOVA
    {
        public int ID { get; set; }
        public int CODAG { get; set; }
        public string DESCRICAO { get; set; }
        public string DETALHAMENTO { get; set; }
    }

    public class COMPONENTESACESSOVA
    {
        public int ID { get; set; }
        public string FORM { get; set; }
        public string COMP { get; set; }
        public string FLGFORM { get; set; }
        public string DESCRICAO { get; set; }
    }

    public class CONTROLEACESSOVA
    {
        public int IDCOMP { get; set; }
        public int IDPERFIL { get; set; }
    }

    public class VOPERVAWS
    {
        public int ID_FUNC { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
        public int CODOPE { get; set; }
        public string NOME { get; set; }
        public string CLASSE { get; set; }
        public int IDPERFIL { get; set; }
        public string STA { get; set; }
        public DateTime DTEXPSENHA { get; set; }
    }

    public class AGRUPAMENTO
    {
        public int CODAG { get; set; }
        public string NOMAG { get; set; }    
    }

    public class TAXAVA
    {
        public int CODTAXA { get; set; }
        public string ABREVTAXA { get; set; }
        public string NOMTAXA { get; set; }
        public string TRENOVA { get; set; }
        public string STATUS { get; set; }
        public int TIPO { get; set; }
        public int SISTEMA { get; set; }
        public string CENTRALIZADORA { get; set; }
    }

    public class TAXAPJ
    {
        public int CODTAXA { get; set; }
        public string ABREVTAXA { get; set; }
        public string NOMTAXA { get; set; }
        public string TRENOVA { get; set; }
        public string STATUS { get; set; }
        public int TIPO { get; set; }
        public string TAXADEFAULT { get; set; }
    }

    public class TAXAVAPJ
    {
        public int CODTAXA { get; set; }
        public string ABREVTAXA { get; set; }
        public string NOMTAXA { get; set; }
        public string TRENOVA { get; set; }
        public string STATUS { get; set; }
        public int TIPO { get; set; }
        public string CENTRALIZADORA { get; set; }
        public int SISTEMA { get; set; }
        public string TAXADEFAULT { get; set; }
    }

    public class CONSULTAVA
    {
        public int CODCONS { get; set; }
        public int CODTIPOCONS { get; set; }
        public int CODTIPOCONSINT { get; set; }
        public string DESCRICAO { get; set; }
        public string NOME_CONSULTA { get; set; }
        public DateTime PERIODO_INI { get; set; }
        public DateTime PERIODO_FIM { get; set; }
        public int NUM_HOST_INI { get; set; }
        public int NUM_HOST_FIM { get; set; }
        public int NUM_AUT_INI { get; set; }
        public int NUM_AUT_FIM { get; set; }
        public string NOME_USUARIO { get; set; }
        public string MAT_USUARIO { get; set; }
        public string CPF_USUARIO { get; set; }
        public string TIPO_RESPOSTA { get; set; }
        public string TIPO_TRANSACAO { get; set; }
        public string NUM_CARTAO { get; set; }
        public string LISTA_CRED { get; set; }
        public string INTERVALO_CRED_INI { get; set; }
        public string INTERVALO_CRED_FIM { get; set; }
        public string LISTA_CLI { get; set; }
        public string INTERVALO_CLI_INI { get; set; }
        public string INTERVALO_CLI_FIM { get; set; }
        public int NUM_FECH_CRED_INI { get; set; }
        public int NUM_FECH_CRED_FIM { get; set; }
        public DateTime DATA_FECH_CRED_INI { get; set; }
        public DateTime DATA_FECH_CRED_FIM { get; set; }
        public int OPERADOR { get; set; }
        public string HORA_PERIODO_INI { get; set; }
        public string HORA_PERIODO_FIM { get; set; }
        public string QUERY_LISTAGEM { get; set; }
    }

    public class BENEFICIO
    {
        public int CODBENEF { get; set; }
        public int TIPTRA { get; set; }
        public string NOMBENEF { get; set; }
        public string ABREVBENEF { get; set; }
        public string TRENOVA { get; set; }
        public string STATUS { get; set; }

    }

    public class BENEFCLI
    {
        public int CODCLI { get; set; }
        public int CODBENEF { get; set; }
        public decimal VALTIT { get; set; }
        public decimal VALDEP { get; set; }
        public DateTime DTCARENCIA { get; set; }
        public string COBCANC { get; set; }
        public string SUBBENEF { get; set; }
        public int PERSUB { get; set; }
        public string INDIVIDUAL { get; set; }
        public DateTime DTVINGENCIA { get; set; }
        public string RENOVAUT { get; set; }

    }

    public class BENEFUSU
    {
        public int CODCLI { get; set; }
        public string CPF { get; set; }
        public int NUMDEP { get; set; }
        public int CODBENEF { get; set; }
        public DateTime DATRENOV { get; set; }
        public DateTime DATBENEF { get; set; }
        public decimal VALBENEF { get; set; }
        
    }

    public class BENEFICIO_CLIENTE
    {
        public int CODBENEF { get; set; }
        public int CODCLI { get; set; }
        public string NOMBENEF { get; set; }
        public int TIPTRA { get; set; }
        public string ABREVBENEF { get; set; }
        public string COMPULSORIO { get; set; }
        public string VIGENCIA { get; set; }
        public string RENOVAUT { get; set; }
        public string DTASSOC { get; set; }
        public decimal VALTIT { get; set; }
        public decimal VALDEP { get; set; }
        public string TIPO { get; set; }
        public string VALORTIT { get; set; }
        public string VALORDEP { get; set; }

    }

    public class BENEFICIO_USUARIO
    {
        public int CODBENEF { get; set; }
        public int CODCLI { get; set; }
        public string NOMBENEF { get; set; }
        public string CPF { get; set; }
        public string VIGENCIA { get; set; }
        public string RENOVAUT { get; set; }
        public string DTASSOC { get; set; }
        public string COMPULSORIO { get; set; }
        public decimal VALTIT { get; set; }
    }

    public class BENEFICIO_USUARIO_CLIENTE
    {
        public int CODBENEF { get; set; }
        public int CODCLI { get; set; }
        public string NOMBENEF { get; set; }
        public string CPF { get; set; }
        public string DTASSOC { get; set; }
        public string COMPULSORIO { get; set; }
        public string JAASSOC { get; set; }
        public decimal VALTIT { get; set; }
    }

    public class CARGA_CTRL_TABS
    {
        public string ID { get; set; }
        public string CODCLI { get; set; }
        public string CLIENTE_ORIGEM { get; set; }
        public string DT_CRIACAO { get; set; }
        public string NUM_CARGA { get; set; }
        public string DT_PROG { get; set; }
        public string TOT_REGS { get; set; }
        public string VALOR_CARGA { get; set; }
        public string NOME_TABELA { get; set; }
        public string CNPJ { get; set; }
        public string VALOR_A_CARREGAR { get; set; }
        public string TOT_INVALIDOS { get; set; }
        public string RESULTADO { get; set; }
        public string DT_CARGA { get; set; }
        public string TIPO { get; set; }
        public string LOGINOPE { get; set; }
        public string CODOPE { get; set; }
        public string NOM_ORIGINAL_ARQUIVO{ get; set; }
        public string CAM_COMP_ARQ { get; set; }
        public string ERRO_PROC { get; set; }
        public string STATUS_PROC { get; set; }
        public string PROC_COMP { get; set; }
        public string BAIXOU_LOG { get; set; }
        public string ORIGEM { get; set; }
    }

    public class CARGA_CTRL_TABS_RESUMO
    {
        public string ID { get; set; }
        public string CODCLI { get; set; }
        public string NUM_CARGA { get; set; }
        public string REGISTRO_LOG { get; set; }
        public string TIPO_REG { get; set; }
    }

}
