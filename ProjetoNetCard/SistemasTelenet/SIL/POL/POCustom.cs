using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TELENET.SIL.PO
{
    public class ENDCEP
    {
        public string RETORNO { get; set; }
        public string LOGRA { get; set; }
        public string BAIRRO { get; set; }
        public string LOCALI { get; set; }
        public string UF { get; set; }
    }

    public class CLIENTEVA_OBS
    {
        public int CODCLI { get; set; }
        public DateTime DATA { get; set; }
        public string OBS { get; set; }
        public int ID { get; set; }
    }

    public class CLIENTE_OBS
    {
        public int CODCLI { get; set; }
        public DateTime DATA { get; set; }
        public string OBS { get; set; }
    }

    public class ESPATIVADA_CREDENCIADO
    {
        public int CODCRE { get; set; }
        public int CODESP { get; set; }
        public string NOMESP { get; set; }
    }

    public class CREDENCIADO_OBS
    {
        public int CODCRE { get; set; }
        public DateTime DATA { get; set; }
        public string OBS { get; set; }
        public int ID { get; set; }
    }

    public class USUARIO_OBS
    {
        public int CODCLI { get; set; }
        public string CPF { get; set; }
        public DateTime DATA { get; set; }
        public string OBS { get; set; }
        public int ID { get; set; }
    }

    public class SEG_GRUPO_DISPAUTORIZ
    {
        public int SISTEMA { get; set; }
        public string OPERACAO { get; set; }
        public int CODCLI { get; set; }
        public int CODSEG { get; set; }
        public string NOMSEG { get; set; }
        public int CODGRUPO { get; set; }
        public string NOMGRUPO { get; set; }
        public int PERLIM { get; set; }
        public string PERLIMEXC { get; set; }
        public int LIMRISCO { get; set; }
        public int MAXPARC { get; set; }
        public int PERSUB { get; set; }
    }

    public class GRUP_DISPAUTORIZ
    {
        public int SISTEMA { get; set; }
        public string OPERACAO { get; set; }
        public int CODCLI { get; set; }
        public int CODGRUPO { get; set; }
        public string NOMGRUPO { get; set; }
        public int PERLIM { get; set; }
        public string PERLIMEXC { get; set; }
        public int LIMRISCO { get; set; }
        public int MAXPARC { get; set; }
        public int PERSUB { get; set; }
    }

    public class GRUPO_DISPAUTORIZ
    {
        public int CODCLI { get; set; }
        public int CODSEG { get; set; }
        public string NOMSEG { get; set; }
        public int PERLIM { get; set; }
        public string PERLIMEXC { get; set; }
        public int LIMRISCO { get; set; }
        public int MAXPARC { get; set; }
        public int PERSUB { get; set; }
    }

    #region /* Segmentos antigo - apagar após témino da junção */

    public enum AutorizacaoSegmento
    {
        Nao = 0,
        Parcialmente = -1,
        Sim = 1
    }

    #endregion /* Segmentos antigo - apagar após témino da junção */

    public interface ItemAutorizacao
    {
        int Codigo { get; set; }
        bool EstaAutorizado { get; set; }
        short LimiteRisco { get; set; }
        short MaximoParcelas { get; set; }
        string Nome { get; set; }
        short PercentualLimite { get; set; }
        bool PercentualLimiteExclusivo { get; set; }
        short PercentualSubsidio { get; set; }
        short PercentualSubsidioDependente { get; set; }
    }

    public class Segmento : ItemAutorizacao
    {
        public Segmento()
        {
            RamosAtividade = new List<RamoAtividade>();
        }

        private bool _estaAutorizado { get; set; }

        public int Codigo { get; set; }

        public bool EstaAutorizado
        {
            get { return _estaAutorizado; }

            set
            {
                _estaAutorizado = value;

                if (_estaAutorizado)
                    RamosAtividade.ToList().ForEach(r => r.EstaAutorizado = true);
                else 
                    RamosAtividade.ToList().ForEach(r => r.EstaAutorizado = false);
            }
        }

        public short LimiteRisco { get; set; }
        public short MaximoParcelas { get; set; }
        public string Nome { get; set; }
        public short PercentualLimite { get; set; }
        public bool PercentualLimiteExclusivo { get; set; }
        public short PercentualSubsidio { get; set; }
        public short PercentualSubsidioDependente { get; set; }
        public IList<RamoAtividade> RamosAtividade { get; set; }
    }

    public class RamoAtividade
    {
        public RamoAtividade() { }

        public int Codigo { get; set; }
        public bool EstaAutorizado { get; set; }
        public string Nome { get; set; }
    }

    #region /* Segmentos antigo - apagar após témino da junção */

    public class RamoAtividadeView
    {
        public RamoAtividadeView(RamoAtividade ramoAtividade)
        {
            _estadoAutorizacaoOriginal = ramoAtividade.EstaAutorizado ? true : false;
            RamoAtividade = ramoAtividade;
        }

        private bool _estadoAutorizacaoOriginal;

        public int Codigo { get { return RamoAtividade.Codigo; } }
        public bool EstaAutorizado { get { return RamoAtividade.EstaAutorizado; } set { RamoAtividade.EstaAutorizado = value; } }
        public bool FoiModificado { get { return RamoAtividade.EstaAutorizado != _estadoAutorizacaoOriginal; } }
        public string Nome { get { return RamoAtividade.Nome; } }
        public RamoAtividade RamoAtividade { get; private set; }
    }

    public class SegmentoView
    {
        public SegmentoView(Segmento segmento)
        {
            Segmento = segmento;

            _estadoAutorizacaoOriginal = segmento.EstaAutorizado ? AutorizacaoSegmento.Sim : AutorizacaoSegmento.Nao;
            _limiteRisco = segmento.LimiteRisco;
            _percentualLimiteExclusivo = segmento.PercentualLimiteExclusivo;
            _percentualLimite = segmento.PercentualLimite;
            _maximoParcelas = segmento.MaximoParcelas;
            _percentualSubsidio = segmento.PercentualSubsidio;

            RamosAtividade = Segmento.RamosAtividade.ToList().Select(ra => new RamoAtividadeView(ra)).ToArray();
        }

        private AutorizacaoSegmento _estadoAutorizacaoOriginal;
        private short _limiteRisco;
        private bool _percentualLimiteExclusivo;
        private short _percentualLimite;
        private short _maximoParcelas;
        private short _percentualSubsidio;

        public int Codigo { get { return Segmento.Codigo; } }
        public string Nome { get { return Segmento.Nome; } }
        public string EstaAutorizado
        {
            get
            {
                return Segmento.EstaAutorizado //== AutorizacaoSegmento.Nao
                    ? "U"
                    : Segmento.EstaAutorizado //== AutorizacaoSegmento.Sim
                        ? "C"
                        : "N";
            }

            set
            {
                switch (value)
                {
                    case "U":
                        Segmento.EstaAutorizado = false; //= AutorizacaoSegmento.Nao;
                        break;
                    case "C":
                    case "N":
                    default:
                        Segmento.EstaAutorizado = true; //= AutorizacaoSegmento.Sim;
                        break;
                }
            }
        }

        public bool FoiModificado
        {
            get
            {
                return RamosAtividade.Any(r => r.FoiModificado)
                    || Segmento.EstaAutorizado != (_estadoAutorizacaoOriginal == AutorizacaoSegmento.Sim ? true : false)
                    || Segmento.LimiteRisco != _limiteRisco
                    || Segmento.MaximoParcelas != _maximoParcelas
                    || Segmento.PercentualLimite != _percentualLimite
                    || Segmento.PercentualSubsidio != _percentualSubsidio
                    || Segmento.PercentualLimiteExclusivo != _percentualLimiteExclusivo;
            }
        }

        public short LimiteRisco { get { return Segmento.LimiteRisco; } set { Segmento.LimiteRisco = value; } }
        public short MaximoParcelas { get { return Segmento.MaximoParcelas; } set { Segmento.MaximoParcelas = value; } }
        public short PercentualLimite { get { return Segmento.PercentualLimite; } set { Segmento.PercentualLimite = value; } }
        public short PercentualSubsidio { get { return Segmento.PercentualSubsidio; } set { Segmento.PercentualSubsidio = value; } }
        public string PercentualLimiteExclusivo { get { return Segmento.PercentualLimiteExclusivo ? "Sim" : "Não"; } set { Segmento.PercentualLimiteExclusivo = value == "Sim"; } }
        public IEnumerable<RamoAtividadeView> RamosAtividade { get; set; }

        public Segmento Segmento { get; private set; }
    }

    #endregion /* Segmentos antigo - apagar após témino da junção */

    public class GrupoCredenciado : ItemAutorizacao
    {
        public GrupoCredenciado()
        {
            Credenciados = new List<Credenciado>();
        }

        public int Codigo { get; set; }
        public bool EstaAutorizado { get; set; }
        public short LimiteRisco { get; set; }
        public string Nome { get; set; }
        public short MaximoParcelas { get; set; }
        public short PercentualLimite { get; set; }
        public bool PercentualLimiteExclusivo { get; set; }
        public short PercentualSubsidio { get; set; }
        public short PercentualSubsidioDependente { get; set; }
        public IList<Credenciado> Credenciados { get; }
    }

    public class Credenciado
    {
        public string Nome { get; set; }
    }

    #region /* Segmentos antigo - apagar após témino da junção */

    public class GrupoCredenciadoView
    {
        public GrupoCredenciadoView(GrupoCredenciado grupoCredenciado)
        {
            _estadoAutorizacaoOriginal = grupoCredenciado.EstaAutorizado ? true : false;
            _limiteRisco = grupoCredenciado.LimiteRisco;
            _maximoParcelas = grupoCredenciado.MaximoParcelas;
            _percentualLimite = grupoCredenciado.PercentualLimite;
            _percentualLimiteExclusivo = grupoCredenciado.PercentualLimiteExclusivo;
            _percentualSubsidio = grupoCredenciado.PercentualSubsidio;

            GrupoCredenciado = grupoCredenciado;
        }

        private bool _estadoAutorizacaoOriginal;
        private short _limiteRisco;
        private short _maximoParcelas;
        private short _percentualLimite;
        private bool _percentualLimiteExclusivo;
        private short _percentualSubsidio;


        public int Codigo { get { return GrupoCredenciado.Codigo; } }
        public string Nome { get { return GrupoCredenciado.Nome; } }
        public bool EstaAutorizado { get { return GrupoCredenciado.EstaAutorizado; } set { GrupoCredenciado.EstaAutorizado = value; } }

        public bool FoiModificado
        {
            get
            {
                return GrupoCredenciado.EstaAutorizado != _estadoAutorizacaoOriginal
                    || GrupoCredenciado.LimiteRisco != _limiteRisco
                    || GrupoCredenciado.MaximoParcelas != _maximoParcelas
                    || GrupoCredenciado.PercentualLimite != _percentualLimite
                    || GrupoCredenciado.PercentualLimiteExclusivo != _percentualLimiteExclusivo
                    || GrupoCredenciado.PercentualSubsidio != _percentualSubsidio;
            }
        }

        public short LimiteRisco { get { return GrupoCredenciado.LimiteRisco; } set { GrupoCredenciado.LimiteRisco = value; } }
        public short MaximoParcelas { get { return GrupoCredenciado.MaximoParcelas; } set { GrupoCredenciado.MaximoParcelas = value; } }
        public short PercentualLimite { get { return GrupoCredenciado.PercentualLimite; } set { GrupoCredenciado.PercentualLimite = value; } }
        public string PercentualLimiteExclusivo { get { return GrupoCredenciado.PercentualLimiteExclusivo ? "Sim" : "Nao"; } set { GrupoCredenciado.PercentualLimiteExclusivo = value == "Sim"; } }
        public short PercentualSubsidio { get { return GrupoCredenciado.PercentualSubsidio; } set { GrupoCredenciado.PercentualSubsidio = value; } }
        public IList<Credenciado> Credenciados { get { return GrupoCredenciado.Credenciados; } }

        public GrupoCredenciado GrupoCredenciado { get; private set; }
    }

    #endregion /* Segmentos antigo - apagar após témino da junção */

    public class SEGAUTORIZVA_CLIENTE
    {
        public int CODCLI { get; set; }
        public int CODSEG { get; set; }
        public string NOMSEG { get; set; }
    }

    public class GRUPOSAUTORIZVA_CLIENTE
    {
        public int CODCLI { get; set; }
        public int CODGRUPO { get; set; }
        public string NOMGRUPO { get; set; }
    }

    public interface IDadosBasicosCliente
    {
        int CODCLI { get; set; }
        string NOMCLI { get; set; }
        int SISTEMA { get; set; }
        string TIPOCARTAO { get; }
        string PRODUTO { get; set; }
        string CNPJ { get; set; }
        string STA { get; set; }
        string DESTA { get; set; }
    }

    public class CLIENTEVA_PREPAGO : IDadosBasicosCliente
    {
        public string PRZVALCART { get; set; }
        public string DESTA { get; set; }
        public string BAIRRO { get; set; }
        public int CODCLI { get; set; }
        public int CODPROD { get; set; }
        public int TIPOPROD { get; set; }
        public string TIPOPRODDESCRICAO { get; set; }
        public string PRODUTO { get; set; }
        public string SUBREDE { get; set; }
        public DateTime? DATINC { get; set; }
        public DateTime? DATSTA { get; set; }
        public string DTFECH { get; set; }
        public string NOMCLI { get; set; }
        public string CNPJ { get; set; }
        public string CGC { get; set; }
        public string INSEST { get; set; }
        public string ENDCLI { get; set; }
        public string CEP { get; set; }
        public short PRAPAG_VA { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string EMA { get; set; }
        public string CONTATO { get; set; }
        public string STATUS { get; set; }
        public string STA { get; set; }
        public string LOCALIDADE { get; set; }
        public string NOMUF { get; set; }
        public string NOMGRA { get; set; }
        public string ENDCPL { get; set; }
        public string ENDEDC { get; set; }
        public string ENDCPLEDC { get; set; }
        public string CEPEDC { get; set; }
        public string CUIDADOSEDC { get; set; }
        public string TELEDC { get; set; }
        public string NOMUFEDC { get; set; }
        public string LOCALIDADEEDC { get; set; }
        public string BAIRROEDC { get; set; }
        public string REGIAO { get; set; }
        public string REGIONAL { get; set; }
        public int CODATI { get; set; }
        public string ATIVIDADE { get; set; }
        public int NMAXPAR { get; set; }
        public double LIMMAXCAR { get; set; }
        public double LIMRISCO { get; set; }
        public string NOMVEND { get; set; }
        public int USUCANC { get; set; }
        public int USUBLOC { get; set; }
        public int USUATIV { get; set; }
        public int USUNATIV { get; set; }
        public decimal TOTALCARGA { get; set; }
        public string SALDO { get; set; }
        public string DATA_INCLUSAO { get; set; }
        public int SISTEMA { get { return 1; } set { } }
        public string TIPOCARTAO { get { return "PRÉ PAGO"; } }
    }

    public class CARTOES_SEGVIA
    {
        public int CODCLI { get; set; }
        public string NOMCLI { get; set; }
        public DateTime? DATA { get; set; }
        public short? NUMDEP { get; set; }
        public string CODCRT { get; set; }
        public string CPF { get; set; }
        public string NOMUSU { get; set; }
        public string STA { get; set; }
        public string MAT { get; set; }
        public Decimal VALTRA { get; set; }
    }

    public class CARTOES_BLOQUEIO
    {
        public int CODCLI { get; set; }
        public string NOMCLI { get; set; }
        public DateTime? DATA { get; set; }
        public short? NUMDEP { get; set; }
        public string CODCRT { get; set; }
        public string CPF { get; set; }
        public string NOMUSU { get; set; }
        public string STA { get; set; }
        public string MAT { get; set; }
    }

    public class CARTOES_CANCELAMENTO
    {
        public int CODCLI { get; set; }
        public string NOMCLI { get; set; }
        public DateTime? DATA { get; set; }
        public short? NUMDEP { get; set; }
        public string CODCRT { get; set; }
        public string CPF { get; set; }
        public string NOMUSU { get; set; }
        public string STA { get; set; }
        public string MAT { get; set; }
    }

    public class CARTOES_INCLUSAO
    {
        public int CODCLI { get; set; }
        public string NOMCLI { get; set; }
        public DateTime? DATA { get; set; }
        public short? NUMDEP { get; set; }
        public string CODCRT { get; set; }
        public string CPF { get; set; }
        public string NOMUSU { get; set; }
        public string STA { get; set; }
        public string MAT { get; set; }
    }

    public class CREDENCIADO_VA
    {
        public string CODBAI { get; set; }
        public string BAIRRO { get; set; }
        public string CODATI { get; set; }
        public string ATIV { get; set; }
        public string STATUS { get; set; }
        public string CODSTA { get; set; }
        public string CODSEG { get; set; }
        public string SEGMENTO { get; set; }
        public string LOCALIDADE { get; set; }
        public string NOMUF { get; set; }
        public string CODCRE { get; set; }
        public string SENCRE { get; set; }
        public string RAZSOC { get; set; }
        public string NOMFAN { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string EMA { get; set; }
        public string CONTATO { get; set; }
        public DateTime DATINC { get; set; }
        public DateTime DATCTT { get; set; }
        public DateTime DATSTA { get; set; }
        public string CTABCO { get; set; }
        public string MASCONTA { get; set; }
        public string TAXADM { get; set; }
        public DateTime DATTAXADM { get; set; }
        public string ENDEDC { get; set; }
        public string QTDEQUIP { get; set; }
        public string TIPFEC { get; set; }
        public string DIAFEC { get; set; }
        public string TIPFEC_VA { get; set; }
        public string DIAFEC_VA { get; set; }
        public string NUMFEC { get; set; }
        public DateTime DATULTFEC { get; set; }
        public string ENDCRE { get; set; }
        public string CEP { get; set; }
        public string INSEST { get; set; }
        public string MATRIZ { get; set; }
        public string QTEFIL { get; set; }
        public string QTEMAQ { get; set; }
        public string ENDCPL { get; set; }
        public string PRAPAG { get; set; }
        public string PRAREE { get; set; }
        public string LOCPAG { get; set; }
        public string CODCEN { get; set; }
        public string FORMAPAG { get; set; }
        public string LINHA1_PQ { get; set; }
        public string LINHA2_PQ { get; set; }
        public DateTime DATGERCRT { get; set; }
        public string GERCRT { get; set; }
        public string ENDCOR { get; set; }
        public string ENDCPLCOR { get; set; }
        public string NOMUFCOR { get; set; }
        public string LOCALIDADECOR { get; set; }
        public string BAIRROCOR { get; set; }
        public string CEPCOR { get; set; }
        public string CNPJ { get; set; }
        public string FLAG_PJ { get; set; }
        public string TAXSER { get; set; }
        public string FLAG_VA { get; set; }
        public string NOME { get { return (string.Format("{0} - {1}", CODCRE, RAZSOC)); } }
        public int NUMTRAOK0;
    }

    public class CREDENFECHAR_VA
    {
        public int CODCRE { get; set; }
        public string NOMFAN { get; set; }
        public decimal VALAFE { get; set; }
        public int QTEAFE { get; set; }
        public DateTime DTINIFECH { get; set; }
        public DateTime DTFIMFECH { get; set; }
        public string NUMFECH { get; set; }
        public string LINHAIMP { get; set; }
        public string TIP { get; set; }
    }

    public class USUARIO_VA
    {
        public CLIENTEVA_PREPAGO CLIENTEVA { get; set; }
        public bool COBRASEGVIA { get; set; }
        public decimal VALORSEGVIA { get; set; }
        public int CODCLIINT { get { if (CODCLI != string.Empty) return Convert.ToInt32(CODCLI); return 0; } }
        public string CODCLI { get; set; }
        public string PRZVALCART { get; set; }
        public string NOMCLI { get; set; }
        public string CODFILNUT { get; set; }
        public string CODSET { get; set; }
        public string MAT { get; set; }
        public string CPF { get; set; }
        public string NOMUSU { get; set; }
        public string NUMDEP { get; set; }
        public string CODPAR { get; set; }
        public DateTime DATINC { get; set; }
        public int ULTCARGVA { get; set; }
        public DateTime DATSTA { get; set; }
        public string DATSTASTR { get; set; }
        public string DATATV { get; set; }
        public string STA { get; set; }
        public string GERCRT { get; set; }
        public string DTVALCART { get; set; }
        public string GERARNOVOCARTAO { get { if (GERCRT == "X") return "SIM"; return "NAO"; } }
        public string CELULARMASK { get { return !string.IsNullOrEmpty(CEL) ? string.Format("{0:(##)#####-####}", Int64.Parse(UtilSIL.RetirarCaracteres("()- ", CEL))) : string.Empty; } }
        public string TELMASK { get { return !string.IsNullOrEmpty(TEL) ? string.Format("{0:(##)####-####}", Int64.Parse(UtilSIL.RetirarCaracteres("()- ", TEL))) : string.Empty; } }
        public string TELCOMMASK { get { return !string.IsNullOrEmpty(TELCOM) ? string.Format("{0:(##)####-####}", Int64.Parse(UtilSIL.RetirarCaracteres("()- ", TELCOM))) : string.Empty; } }
        public string STASTR
        {
            get
            {
                switch (STA)
                {
                    case "00":
                        return "ATIVO";
                    case "01":
                        return "BLOQUEADO";
                    case "02":
                        return "CANCELADO";
                    case "06":
                        return "SUSPENSO";
                    default:
                        return "STATUS N ENCONTRADO";
                }
            }
        }
        public string STASTRCLI
        {
            get
            {
                switch (STACLI)
                {
                    case "00":
                        return "ATIVO";
                    case "01":
                        return "BLOQUEADO";
                    case "02":
                        return "CANCELADO";
                    case "06":
                        return "SUSPENSO";
                    case "08":
                        return "INADIMPLENTE";
                    case "09":
                        return "EM RESCISAO";
                    default:
                        return "STATUS N ENCONTRADO";
                }
            }
        }
        public string STACLI { get; set; }
        public string DESTA { get; set; }
        public string ROTULO { get; set; }
        public string CODCRT { get; set; }
        public string CODCRTMASK { get { return UtilSIL.MascaraCartao(CODCRT, 17); } }
        public string CODCRTANT { get; set; }
        public string VALADES { get; set; }
        public int ID { get; set; }
        public string NUMPAC { get; set; }
        public string DATADES { get; set; }
        public string NUMULTPAC { get; set; }
        public string TITULAR_DEPENDENTE
        {
            get
            {
                if (!string.IsNullOrEmpty(CODPAR) && NUMDEP != "0")
                    return "DEPENDENTE";
                return "TITULAR";
            }
            set { if (value == null) throw new ArgumentNullException("value"); }
        }
        public decimal SALDO { get; set; }
        public string SENHA { get; set; }
        public string SENUSU { get; set; }
        public string TRILHA2 { get; set; }
        public decimal CARGPADVA { get; set; }
        public decimal CARGPADVACLIENTE { get; set; }
        public string ULTMOV { get; set; }
        public string VALIDADE { get; set; }
        public string CODFIL { get; set; }
        public string DATGERCRT { get; set; }
        public int NUMCARGA { get; set; }
        public string NOMCRT { get; set; }
        public string CENTROCUSTO { get; set; }
        public decimal TOTALCARGAUSUARIO { get; set; }
        public string TIPO_LAYOUT { get; set; }
        public string GENERICO { get; set; }
        public DateTime DATNAS { get; set; }
        public string PAI { get; set; }
        public string MAE { get; set; }
        public string CEL { get; set; }
        public string TEL { get; set; }
        public string SEXO { get; set; }
        public string EMA { get; set; }
        public string RG { get; set; }
        public string ORGEXPRG { get; set; }
        public string NATURALIDADE { get; set; }
        public string NACIONALIDADE { get; set; }
        public string ENDUSU { get; set; }
        public string ENDNUMUSU { get; set; }
        public string ENDCPL { get; set; }
        public string BAIRRO { get; set; }
        public string LOCALIDADE { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string ENDUSUCOM { get; set; }
        public string ENDNUMCOM { get; set; }
        public string ENDNUMUSUCOM { get; set; }
        public string ENDCPLCOM { get; set; }
        public string BAIRROCOM { get; set; }
        public string LOCALIDADECOM { get; set; }
        public string UFCOM { get; set; }
        public string CEPCOM { get; set; }
        public string TELCOM { get; set; }
        public bool TEMCAD { get; set; }
        public string DTEXPSENHA { get; set; }
    }

    public class OPERADORMW
    {
        public string CNPJ { get; set; }
        public string TIPOACESSO { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
        public string NOME { get; set; }
        public string NOMGRUPO { get; set; }
        public string DTEXPSENHA { get; set; }
        public int ID { get; set; }
        public int CODPARCERIA { get; set; }
        public List<CLIENTEAGRUPAMENTO> AGRUPAMENTO { get; set; }
    }

    public class ACESSOOPERADORMW : OPERADORMW
    {
        public int SISTEMA { get; set; }
        public int CODCLI { get; set; }
        public bool ACESSOBLOQUEADO { get; set; }
        public bool FINCCART { get; set; }
        public bool FBLOQCART { get; set; }
        public bool FDESBLOQCART { get; set; }
        public bool FCANCCART { get; set; }
        public bool FALTLIMITE { get; set; }
        public bool FSEGVIACART { get; set; }
        public bool FEXTMOV { get; set; }
        public bool FCONSREDE { get; set; }
        public bool FLISTTRANSAB { get; set; }
        public bool FLISTCART { get; set; }
        public bool FLISTINCCART { get; set; }
        public bool FCARGA { get; set; }
        public bool FTRANSFSALDO { get; set; }
        public bool FTRANSFSALDOCLI { get; set; }
        public bool FCARGAACIMALIMITE { get; set; }
    }

    public class CLIENTEAGRUPAMENTO
    {
        public int SISTEMA { get; set; }
        public string TIPOCARTAO {
            get
            {
                switch (SISTEMA)
                {
                    case 0: return "PÓS PAGO";
                    case 1: return "PRÉ PAGO";
                    default: return "";
                }
            }
        }
        public int CODCLI { get; set; }
        public string NOMCLI { get; set; }
        public string PRODUTO { get; set; }
        public string STA { get; set; }
        public string TIPOATUALIZACAO { get; set; }
    }

    public class LOGWEB_VA
    {
        public int CODCLI { get; set; }
        public DateTime DATA { get; set; }
        public string NOMCLI { get; set; }
        public string LOGIN { get; set; }
        public string NOME { get; set; }
        public string OPERACAO { get; set; }
        public string CPF { get; set; }
        public string CARTAO { get; set; }
    }

    public class CTPOS
    {
        public string CODPOS { get; set; }
        public string TIPOEQ { get; set; }
        public string EQUIPID { get; set; }
        public string VERAPL { get; set; }
        public string CODPS { get; set; }
        public string NOMEPS { get; set; }
        public string TIPOCON { get; set; }
        public DateTime ULTINIC { get; set; }
        public string INICIALIZADO { get; set; }
        public string OPERADORA { get; set; }
        public int QUANTPOS { get; set; }
        public int QUANTMIC { get; set; }
        public int QUANTPDV { get; set; }
        public int QUANTGERAL { get; set; }
    }

    public class CTTRANSVA
    {
        public string SUBREDE { get; set; }
        public string DATTRA { get; set; }
        public string NSUHOS { get; set; }
        public string NSUAUT { get; set; }
        public string TIPTRA { get; set; }
        public string CODRTA { get; set; }
        public string CODPS { get; set; }
        public string NOMEPS { get; set; }
        public string NOMECODPS { get { return (string.Format("{0} - {1}", CODPS, NOMEPS)); } }
        public string CODCRT { get; set; }
        public string CODCRTMASK { get { return string.IsNullOrEmpty(CODCRT) ? CODCRT : UtilSIL.MascaraCartao(CODCRT, 17); } }
        public string VALTRA { get; set; }
        public string TVALOR { get; set; } /* PJ */
        public string PARCELA { get; set; } /* PJ */
        public string TPARCELA { get; set; } /* PJ */
        public string DAD { get; set; }
        public string CODCLI { get; set; }
        public string CPF { get; set; }
        public string NUMDEP { get; set; }
        public string PROCESSADA { get; set; }
        public string CONTSONDA { get; set; }
        public string CODCRE { get; set; }
        public string NOMCLI { get; set; }
        public string RAZSOC { get; set; }
        public string NOMUSU { get; set; }
        public string MAT { get; set; }
        public string CODFIL { get; set; }
        public string CODSET { get; set; }
        public string DATFECCRE { get; set; }
        public string NUMFECCRE { get; set; }
        public string DATPGTOCRE { get; set; }
        public string DATFECCLI { get; set; }
        public string NUMFECCLI { get; set; }
        public string DATPGTOCLI { get; set; }
        public string NUMCARG_VA { get; set; }
        public string NOMFAN { get; set; }
        public string CODCRENOMFAN { get { return (string.Format("{0} - {1}", CODCRE, NOMFAN)); } }
        public string DESTIPTRA { get; set; }
        public string CGC { get; set; }
        public string NOMSEG { get; set; }
        public string REDE { get; set; }
        public string ORIGEMOP { get; set; }
        public string NOMOPERADOR { get; set; }
        public string FLAG_AUT { get; set; }
        public string TIPTRANS { get; set; }
        public string DAD_JUST { get; set; }
        public object GetCollumnValue(string collumnName)
        {
            var ObjectType = GetType();
            var pinfo = ObjectType.GetProperty(collumnName);
            var value = pinfo.GetValue(this, null);
            return value;
        }
    }

    public class CTCARTVA
    {
        public string CODEMPRESA { get; set; }
        public string CODCARTAO { get; set; }
        public string STATUSU { get; set; }
        public string DTSTATUSU { get; set; }
        public string NOMEUSU { get; set; }
        public string NUMDEPEND { get; set; }
        public string CODCARTIT { get; set; }
        public string CPFTIT { get; set; }
        public string DTVALCART { get; set; }
        public string SENHA { get; set; }
        public string SALDOVA { get; set; }
        public string DTVAULT { get; set; }
        public string DTULTCONS { get; set; }
    }

    public class CARGA_AGUAR_LIB_PGTO
    {
        public Int32 CODCLI { get; set; }
        public string NOMCLIENTE { get; set; }
        public Int32 NUMCARGA { get; set; }
        public DateTime DATAUTORDT { get; set; }
        public int QTDUSU { get; set; }
        public decimal VALOR { get; set; }
        public decimal VAL2AVIA { get; set; }
        public decimal VALTAXSER { get; set; }
        public decimal TOTAL { get; set; }
        public decimal SALDOCONTAUTIL { get; set; }
    }
    public class CARGA_PENDENTE
    {
        public string DATA { get; set; }
        public string LOGIN { get; set; }
        public string NOME_ARQUIVO { get; set; }
        public string CLIENTE { get; set; }
        public string NUM_CARGA { get; set; }
        public string ORIGEM { get; set; }
        public string VALOR_ARQUIVO { get; set; }
        public string ETAPA { get; set; }
        public string NIVEL { get; set; }
        public string ERRO { get; set; }
        public string MENSAGEM { get; set; }
        public string BAIXOU_LOG { get; set; }
        public string ID_PROCESSO { get; set; }
    }

    public class LogCarga
    {
        public DateTime DATA { get; set; }
        public int CODCLI { get; set; }
        public int NUMCARG_VA { get; set; }
        public DateTime DTCARGA { get; set; }
        public int CODERRO { get; set; }
        public string MENSAGEM { get; set; }
        public string CPF { get; set; }
        public string STA { get; set; }
        public string DESTA { 
            get
            {
                switch (STA)
                {
                    case "00":
                        return "ATIVO";
                    case "01":
                        return "BLOQUEADO";
                    case "02":
                        return "CANCELADO";
                    case "06":
                        return "SUSPENSO";
                    default:
                        return "";
                }
            }
        }
        public decimal VALOR { get; set; }
    }

    public class CLIENTE_CARGA : ICloneable
    {
        public Int32 CODCLI { get; set; }
        public string NOMCLIENTE { get; set; }
        public Int32 NUMCARGA { get; set; }
        public DateTime DATAUTORDT { get; set; }
        private string _DATAUTOR;
        public string DATAUTOR
        {
            get
            {
                //if (DATAUTORDT != DateTime.MinValue && DATAUTORDT != DateTime.MaxValue)
                //    return DATAUTORDT.ToString(CultureInfo.InvariantCulture);
                //return _DATAUTOR;
                if (DATAUTORDT != DateTime.MinValue && DATAUTORDT != DateTime.MaxValue)
                    return DATAUTORDT.ToShortDateString();
                return string.Empty;
            }
            set { _DATAUTOR = value; }
        }
        public DateTime DATPROGDT { get; set; }
        public string DATPROG
        {
            get
            {
                if (DATPROGDT != DateTime.MinValue && DATPROGDT != DateTime.MaxValue)
                    return DATPROGDT.ToShortDateString();
                return string.Empty;
            }
        }
        public Boolean CARREGA { get; set; }
        public Boolean PROGRAMA { get; set; }
        public Boolean VENCIDA { get; set; }
        public string LIBVENC { get; set; }
        public Boolean EMPROCESSAMENTO { get; set; }
        public Boolean VIACONSULTA { get; set; }
        public Boolean PGTOANTECIPADO { get; set; }
        public Boolean DTPGTO { get; set; }
        public Boolean BLOQ { get; set; }
        public int QTDUSU { get; set; }
        public int CARGA_DIAS_EFET { get; set; }
        public string STACLI { get; set; }
        public string STATUS
        {
            get
            {
                if (BLOQ)
                    return "VERIFICAR CARGA";
                int totalDias = (DateTime.Parse(DateTime.Now.ToShortDateString()).Subtract(DateTime.Parse(DATAUTOR))).Days;
                if (totalDias > this.CARGA_DIAS_EFET && LIBVENC != "S" && PGTOANTECIPADO && !DTPGTO)
                    return "Aguardando pagamento da carga;Carga solicitada a mais de " + this.CARGA_DIAS_EFET + " dias";
                if (!PGTOANTECIPADO && totalDias > this.CARGA_DIAS_EFET && LIBVENC != "S")
                    return "Carga solicitada a mais de " + this.CARGA_DIAS_EFET + " dias";
                if (STACLI == "08")
                    return "Aguardando normalização";
                if (STACLI != "00" && STACLI != "08")
                    return "Cliente não está ativo";
                if (PGTOANTECIPADO && !DTPGTO)
                    return "Aguardando pagamento da carga";
                if (EMPROCESSAMENTO)
                    return "Processando...  " + PERCENTUAL + " %";                
                if (CARREGA)
                    return "Pronta para carregar";
                if (PROGRAMA && !VENCIDA)
                    return "Carga Programada";
                if (VENCIDA)
                    return "Data de programação vencida";
                return VIACONSULTA ? "Carga solicitada pelo cliente" : "Aguardando...";
            }
            set { if (value == null) throw new ArgumentNullException("value"); }
        }
        public int PERCENTUAL { get; set; }
        public decimal VALOR { get; set; }
        public int IDOPERCLIWEB { get; set; }
        public int IDLIBCARGOPR { get; set; }
        public string ROTULO { get; set; }
        public bool LIDA { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }

    public class OPERADOR_VA
    {
        public Int32 CODOPE { get; set; }
        public string LOGOPE { get; set; }
        public string NOMOPE { get; set; }
        public string DESCRICAO { get; set; }
    }

    public class RESTRICAOACESSO
    {
        public Int32 IDCOMP { get; set; }
        public Int32 IDPERFIL { get; set; }
        public string DESCRICAO { get; set; }
        public string TIPOCONTROLE { get; set; }
    }

    public class VOPERVAWS_CONSULTA
    {
        public Int32 ID_FUNC { get; set; }
        public string LOGIN { get; set; }
        public Int32 IDPERFIL { get; set; }
        public string DESCRICAO { get; set; }
        public string DETALHAMENTO { get; set; }
        public string NOME { get; set; }
        public string STA { get; set; }
        public string DESTA { get; set; }
    }

    public class BENEFICIOS_CONSULTA
    {
        public int CODBENEF { get; set; }
        public int TIPTRA { get; set; }
        public string NOMBENEF { get; set; }
        public string ABREVBENEF { get; set; }
        public string TRENOVA { get; set; }
        public string STATUS { get; set; }
        public string DESTA { get; set; }
    }

    public class TAXAS_CONSULTA
    {
        public Int32 CODTAXA { get; set; }
        public string ABREVTAXA { get; set; }
        public string NOMTAXA { get; set; }
        public string STATUS { get; set; }
        public string DESTA { get; set; }
        public string TRENOVA { get; set; }
        public string TIPO { get; set; }
        public string DESCTIPO { get; set; }
        public string SISTEMA { get; set; }
        public string DESCSISTEMA { get; set; }
        public string CENTRALIZADORA { get; set; }
        public string TAXADEFAULT { get; set; }
    }

    public class MODTAXA
    {
        public Int32 COD { get; set; }
        public Int32 CODTAXA { get; set; }
        public Int32 SISTEMA { get; set; }
        public string TIPOCARTAO
        {
            get
            {
                switch (SISTEMA)
                {
                    case 0: return "PÓS PAGO";
                    case 1: return "PRÉ PAGO";
                    default: return "";
                }
            }
        }
        public Int32 TIPO { get; set; }
        public string APLICACAO
        {
            get
            {
                if (TIPO == 1)
                    return "USUARIO";
                return TIPO == 3 ? "CLIENTE" : "CREDENCIADO";
            }
            set { if (value == null) throw new ArgumentNullException("value"); }
        }
        public Int32 TIPTRA { get; set; }
        public string NOMTAXA { get; set; }
        public string TAXAHAB { get; set; }
        public decimal VALOR { get; set; }
        public decimal VALORDEP { get; set; }
        public string COBFATABAIXO { get; set; }
        public decimal VALCOBFATABAIXO { get; set; }
        public Int32 NUMPARC { get; set; }
        public DateTime DTINC { get; set; }
        public DateTime DTINICIO { get; set; }
        public Int32 DIACOB { get; set; }
        public Int32 DIASPINICIO { get; set; }
        public string COBSEMCRED { get; set; }
        public Int32 PRIORIDADE { get; set; }
        public Int32 NUMULTPAC { get; set; }
        public DateTime DATRENOV { get; set; }
        public DateTime DATTAXA { get; set; }
        public string DESTA { get; set; }
        public decimal VALMINCRED { get; set; }
        public Int16 PGTOTAXA { get; set; }
        public string COBRAATV { get; set; }
        public string COBCANC { get; set; }
        public string COBCANCUTIL { get; set; }
        public string COBUTIL { get; set; }
        public string COBUTILGRUPO { get; set; }        
        public string INDIVIDUAL { get; set; }
        public string SENSISALDO { get; set; }
        public string CENTRALIZADORA { get; set; }
        public string TRENOVA { get; set; }
        public string TAXADEFAULT { get; set; }
        public string DESTRENOVA {
            get
            {
                switch (TRENOVA)
                {
                    case "U": return "ÚNICA";
                    case "A": return "ANUAL";
                    case "M": return "MENSAL";
                    case "F": return "FECHAMENTO";
                    default: return "";
                }
            }
        }
    }

    public class REDES
    {
        public string REDE { get; set; }
        public Int32 CODCRE { get; set; }
        public string SELECIONADA { get; set; }
        public string NOME { get; set; }
        public string TEMCODAFIL { get; set; }
        public string HABREDE { get; set; }
        public string CODAFILREDE { get; set; }
        public DateTime DATATUALIZ { get; set; }
        public string STATUSREDE { get; set; }
        public DateTime DATAREDE { get; set; }
    }

    public class FILTROSTRANS
    {
        public string HORAPERIODOINI { get; set; }
        public string HORAPERIODOFIM { get; set; }
    }

    public class TipoResposta
    {
        private string _descrResposta = string.Empty;
        public TipoResposta(string descricao, char value)
        {
            DESCR = descricao;
            VALUE = value;
        }
        public string DESCR
        {
            get { return _descrResposta; }
            set { _descrResposta = value; }
        }
        public char VALUE { get; set; }
    }

    [Serializable]
    public class MODREL
    {
        public string LINHAIMP { get; set; }
        public int TIP { get; set; }
    }

    public class RELATORIO
    {
        public int IDREL { get; set; }
        public string NOMREL { get; set; }
        public string DESCRICAO { get; set; }
        public string PROCREL { get; set; }
        public string TIPREL { get; set; }
        public string NAVIGATEURL { get; set; }
        public string SISTEMA { get; set; }
    }

    public class PARAMETRO
    {
        public PARAMETRO()
        {
            PARAMETRODINAMICO = true;
        }

        public int IDREL { get; set; }
        public string NOMPROC { get; set; }
        public string DISPOSICAO { get; set; }
        public int IDPAR { get; set; }
        public string DESPAR { get; set; }
        public string NOMPAR { get; set; }
        public string LABEL { get; set; }
        public string TIPO { get; set; }
        public int TAMANHO { get; set; }
        public string DEFAULT { get; set; }
        public string REQUERIDO { get; set; }
        public object VALOR { get; set; }
        public string HASDETAIL { get; set; }
        public string NOMREL { get; set; }
        public string EXECUTAVIAJOB { get; set; }
        public string SAIDA_ARQ_DIRETO { get; set; }
        public int ORDEM_TELA { get; set; }
        public int ORDEM_PROC { get; set; }
        public string NOM_FUNCTION { get; set; }
        public string PARAM_FUNCTION { get; set; }
        public string CHAMADA_FUNC_JS { get; set; }
        public bool PARAMETRODINAMICO { get; set; }
    }

    public class DETPAR
    {
        public string TEXT { get; set; }
        public string VALUE { get; set; }
        public string TIPO { get; set; }
    }

    [Serializable]
    public class FECHCRED_VA
    {
        public Int32 CODCRE { get; set; }
        public string CGC { get; set; }
        public string NOMFAN { get; set; }
        public string RAZSOC { get; set; }
        public string NUMBOR { get; set; }
        public DateTime DATBOR { get; set; }
        public decimal VALINF { get; set; }
        public int QTEINF { get; set; }
        public decimal VALAFE { get; set; }
        public int QTEAFE { get; set; }
        public decimal TAXADM { get; set; }
        public decimal VALTAXA { get; set; }
        public decimal ANUIDADE { get; set; }
        public decimal VALLIQ { get; set; }
        public DateTime DTINIFECH { get; set; }
        public DateTime DTFIMFECH { get; set; }
        public int NUMFECH { get; set; }
        public DateTime DATFECLOT { get; set; }
        public int PRAZO { get; set; }
        public DateTime DATPGTO { get; set; }
        public string BANCO { get; set; }
        public string AGENCIA { get; set; }
        public string CONTA { get; set; }
        public DateTime DATTRA { get; set; }
        public int NSUHOS { get; set; }
        public int NSUAUT { get; set; }
        public string CPF { get; set; }
        public int NUMDEP { get; set; }
        public string CODRTA { get; set; }
        public int TIPTRA { get; set; }
        public string CODCRT { get; set; }
        public decimal VALTRA { get; set; }
        public decimal TOTCREDVALINF { get; set; }
        public int TOTCREDQTEINF { get; set; }
        public decimal TOTCREDVALAFE { get; set; }
        public int TOTCREDQTEAFE { get; set; }
        public decimal TOTCREDVALTAXA { get; set; }
        public decimal TOTCREDVALLIQ { get; set; }
        public decimal TOTVALINF { get; set; }
        public int TOTQTEINF { get; set; }
        public decimal TOTVALAFE { get; set; }
        public int TOTQTEAFE { get; set; }
        public decimal TOTVALTAXA { get; set; }
        public decimal TOTVALLIQ { get; set; }
        public object GetCollumnValue(string collumnName)
        {
            var ObjectType = GetType();
            var pinfo = ObjectType.GetProperty(collumnName);
            var value = pinfo.GetValue(this, null);
            return value;
        }
    }

    public class CARGAC_VA
    {
        public int CODCLI { get; set; }
        public string NOMCLI { get; set; }
        public int NUMCARG_VA { get; set; }
        public string DTAUTORIZ { get; set; }
        public string DTPROG { get; set; }
        public int LIBSUP1 { get; set; }
        public int LIBSUP2 { get; set; }
        public string CODCRT { get; set; }
        public string CPF { get; set; }
        public string NOMUSU { get; set; }
        public decimal VCARGAUTO { get; set; }
        public decimal VALORTOTAL { get; set; }
        public int QTDUSU { get; set; }
        public double TAXSER { get; set; }
    }

    public class CARGA_EFETUADAS : CARGAC_VA
    {
        public string CGC { get; set; }
        public string DTCARGA { get; set; }
        public double VALOR { get; set; }
        public double VAL2AVIA { get; set; }
        public double VALSEG { get; set; }
        public double TAXADESAO { get; set; }
        public double ANUIMENS { get; set; }
        public int PRAPAG { get; set; }
        public string DTPAG { get; set; }
        public double TAXAADMPORCLIENTE { get; set; }
        public double TAXACRT { get; set; }
        public double TOTAL
        {
            get
            {
                return VALOR + TAXSER + VAL2AVIA + TAXADESAO;
            }
        }
        public double TOTALANALICO
        {
            get
            {
                return Convert.ToDouble(VCARGAUTO) + ANUIMENS + VALSEG + TAXACRT;
            }
        }
    }

    public class CARGAAUTO
    {
        public bool EXIBECARGAAUTO { get; set; }
        public bool HABCARGAUTO { get; set; }        
        public decimal MAXVALCARGAUTO { get; set; }
        public int TEMPCARGAUTO { get; set; }
    }

    public class TRANSACAO_VA : IComparable<TRANSACAO_VA>
    {
        public int CompareTo(TRANSACAO_VA trans)
        {
            return Convert.ToDateTime(DATTRA).Ticks.CompareTo(Convert.ToDateTime(trans.DATTRA).Ticks);
        }
        public int CODIGO { get; set; }
        public int CODAUX { get; set; }
        public string DESCRICAO { get; set; }
        public int CODCLI { get; set; }
        public int CODCRE { get; set; }
        public string DATTRA { get; set; }
        public string NUMHOST { get; set; }
        public string NUMAUT { get; set; }
        public string TIPTRA { get; set; }
        public decimal VALOR { get; set; }
        public decimal MEDIA { get; set; }
        public string MEDIAPC { get; set; }
        public char STATUS { get; set; }
        public string CODCRT { get; set; }
        public string CPF { get; set; }
        public int NUMDEP { get; set; }
        public string DATFECCRE { get; set; }
        public string ATV { get; set; }
        public string NUMFECRE { get; set; }
        public string DATPGTO { get; set; }
        public decimal PP { get; set; }
        public int TOTALTRANS { get; set; }
        public int QUANTATIV { get; set; }
        public int QUANTPEND { get; set; }
        public int QUANTREC { get; set; }
        public string TOTALGERAL { get; set; }
        public string MEDIAGERALSINTETICO { get; set; }
    }

    public class LOG
    {
        public LOG() { LISTALOG = new List<LOG>(); }
        public LOG(string id, string tipo, string descricao)
        {
            LISTALOG = new List<LOG>();
            ID = id;
            TIPO = tipo;
            DESCRICAOLOG = descricao;
        }
        public string ID { get; set; }
        public string TIPO { get; set; }
        public string DESCRICAOLOG { get; set; }
        public List<LOG> LISTALOG { get; set; }
        public void AddLog(LOG log) { LISTALOG.Add(log); }
        public List<LOG> getArquivoLog() { return LISTALOG; }
        public void removeRecordOBS()
        {
            if (LISTALOG == null) return;
            var count = LISTALOG.Count;
            while (count > 0)
            {
                LISTALOG.Remove(LISTALOG.Find(x => x.ID == "OBSERVACAO "));
                count--;
            }
        }
    }

    public class CONSULTA_VA
    {
        public short SISTEMA { get; set; }
        public int CODCONS { get; set; }
        public int CODCLI { get; set; }
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

                    case UtilSIL.TVALOR:
                        {
                            format = "VALOR TOTAL";
                            break;
                        }

                    case UtilSIL.PARCELA:
                        {
                            format = "PARCELA";
                            break;
                        }

                    case UtilSIL.TPARCELA:
                        {
                            format = "TOTAL PARCELAS";
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

                    case UtilSIL.DATFECCLI:
                        {
                            format = "DT. FECH. CLIENTE";
                            break;
                        }

                    case UtilSIL.NUMFECCLI:
                        {
                            format = "Nº FECH. CLIENTE";
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
        public int NUM_FECH_CLI_INI { get; set; }
        public int NUM_FECH_CLI_FIM { get; set; }
        public DateTime DATA_FECH_CLI_INI { get; set; }
        public DateTime DATA_FECH_CLI_FIM { get; set; }
        public int OPERADOR { get; set; }
        public string HORA_PERIODO_INI { get; set; }
        public string HORA_PERIODO_FIM { get; set; }
        public int CODCEN { get; set; }

        public string TIPOCARTAO { get { return SISTEMA == 0 ? "Pós Pago" : "Pré Pago"; } }

        public bool FILTROVAZIO
        {
            get
            {
                return string.IsNullOrEmpty(CPF_USUARIO) &&
                    DATA_FECH_CRED_FIM == DateTime.MinValue &&
                    DATA_FECH_CRED_INI == DateTime.MinValue &&
                    DATA_FECH_CLI_FIM == DateTime.MinValue &&
                    DATA_FECH_CLI_INI == DateTime.MinValue &&
                    PERIODO_INI == DateTime.MinValue &&
                    PERIODO_FIM == DateTime.MaxValue &&
                    string.IsNullOrEmpty(LISTA_CLI) &&
                    string.IsNullOrEmpty(LISTA_CRED) &&
                    string.IsNullOrEmpty(TIPO_RESPOSTA) &&
                    string.IsNullOrEmpty(SUBREDE) &&
                    string.IsNullOrEmpty(REDE) &&
                    string.IsNullOrEmpty(TIPO_TRANSACAO) &&
                    string.IsNullOrEmpty(MAT_USUARIO) &&
                    string.IsNullOrEmpty(NOME_USUARIO) &&
                    NUM_AUT_FIM == 0 &&
                    NUM_AUT_INI == 0 &&
                    string.IsNullOrEmpty(NUM_CARTAO) &&
                    NUM_FECH_CRED_FIM == 0 &&
                    NUM_FECH_CRED_INI == 0 &&
                    NUM_FECH_CLI_INI == 0 &&
                    NUM_FECH_CLI_FIM == 0 &&
                    NUM_HOST_FIM == 0 &&
                    NUM_AUT_INI == 0 && string.IsNullOrEmpty(INTERVALO_CLI_INI) && string.IsNullOrEmpty(INTERVALO_CLI_FIM) &&
                    string.IsNullOrEmpty(INTERVALO_CRED_INI) && string.IsNullOrEmpty(INTERVALO_CRED_FIM) &&
                    CODCEN == 0;
            }
        }
    }

    public class AFILIACAO
    {
        public string CODAFIL { get; set; }
        public string SIGUF0 { get; set; }
        public string RAZSOC { get; set; }
        public string NOMFAN { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string EMA { get; set; }
        public string ENDCRE { get; set; }
        public string CEP { get; set; }
        public string INSEST { get; set; }
        public string CGC { get; set; }
    }

    public class TRANSHABCKB
    {
        public string PDV { get; set; }
        public string POSMIC { get; set; }
        public string URA { get; set; }
        public string CENTRAL { get; set; }
        public string COMPRA { get; set; }
        public string COMPRAMED { get; set; }
        public string COMPRAPARC { get; set; }
        public string COMPRAMEDPARC { get; set; }
        public string COMPRACRTDIG { get; set; }
        public string COMPRASAQUE { get; set; }
    }

    public class LOCALIDADE
    {
        public int CODLOC { get; set; }
        public string NOMLOC { get; set; }
    }

    public class BAIRRO
    {
        public int CODBAI { get; set; }
        public string NOMBAI { get; set; }
    }

    public class UF
    {
        public string SIGUF0 { get; set; }
        public string NOMUF0 { get; set; }
    }

    public class ItensGenerico : IEnumerable
    {
        public ItensGenerico(string value, string text, int tamanhoColuna, int ordemTela = 0, short sistema = -1)
        {
            Value = value;
            Text = text;
            TamanhoColuna = tamanhoColuna;
            OrdemTela = ordemTela;
            Sistema = sistema; // -1 PJ/VA, 0 Somente PJ, 1 Somente VA
        }
        public string Value { get; set; }
        public string Text { get; set; }
        public int TamanhoColuna { get; set; }
        public int OrdemTela { get; set; }
        public short Sistema { get; set; }
        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)GetEnumerator(); }
        public ItensGenerico GetEnumerator() { return null; }
        #endregion
    }

    public class GRUPOCREDENCIADO
    {
        public int CODGRUPO { get; set; }
        public int CODCRE { get; set; }
        public string NOMFAN { get; set; }
        public string RAZSOC { get; set; }
    }

    public class BENEF_CLIENTE
    {
        public int CODBENEF { get; set; }
        public int CODCLI { get; set; }
        public string NOMBENEF { get; set; }
        public decimal VALTIT { get; set; }
        public decimal VALDEP { get; set; }
        public string COMPULSORIO { get; set; }
        public DateTime DTASSOC { get; set; }
        public string COBCANC { get; set; }
        public string SUBBENEF { get; set; }
        public int PERSUB { get; set; }
        public string CARENCIA { get; set; }
        public string VIGENCIA { get; set; }
        public string RENOVAUT { get; set; }
        public string GRUPO { get; set; }
        public string ID_OPERADOR { get; set; }
        public string ORIGEM_OPERADOR { get; set; }

    }
}
