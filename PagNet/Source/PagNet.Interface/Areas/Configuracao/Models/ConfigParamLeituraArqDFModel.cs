using PagNet.Api.Service.Interface.Model;
using PagNet.Interface.Helpers.HelperModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace PagNet.Interface.Areas.Configuracao.Models
{
    public class ConfigParamLeituraArquivoVM : IAPIConfigParamLeituraArquivoVM
    {
        public bool acessoAdmin { get; set; }
        public int codigoCliente { get; set; }
        public bool IsCPF { get; set; }
        public int codigoArquivoDescontoFolha { get; set; }
        public int codigoFormaVerificacao { get; set; }
        public int codigoParamUsuario { get; set; }
        public int codigoParamValor { get; set; }
        public string extensaoArquivoRET { get; set; }


        [Display(Name = "Código/CNPJ do Cliente")]
        [Ajuda("Informe o Código ou CPF/CNPJ do cliente")]
        [Required(ErrorMessage = "Obrigatório informar o cliente")]
        [StringLength(20)]
        public string filtroCliente { get; set; }

        [Display(Name = "Nome do Cliente")]
        [Ajuda("Nome do Cliente que irá receber o boleto para pagamento")]
        [StringLength(100)]
        public string nomeCliente { get; set; }

        [Display(Name = "Empresa *")]
        [Ajuda("Filtrar os borderôs por Empresa")]
        public string codigoEmpresa { get; set; }
        public string nmEmpresa { get; set; }

        [Display(Name = "Forma de ler o arquivo *")]
        public string codigoFormaVerificacaoArq { get; set; }
        public string nmFormaVerificacaoArq { get; set; }

        [Display(Name = "Extensão do Arquivo")]
        [Ajuda("Informe qual a extensão do arquivo de retorno da prefeitura")]
        [Required(ErrorMessage = "Obrigatório informar a extensão do arquivo")]
        public string extensaoArquivo { get; set; }
        public string NomeextensaoArquivo { get; set; }
        
        [Display(Name = "Linha Inicial para ler o arqivo")]
        [Ajuda("informe a linha que inicia a primeira informação relacionado ao usuário")]
        [StringLength(3)]
        public string linhaInicial { get; set; }

        [Display(Name = "Posição inicial do CPF")]
        [Ajuda("Posição onde inicial a informação relacionada ao CPF")]
        public int posicaoInicialCPF { get; set; }

        [Display(Name = "Posição final do CPF")]
        [Ajuda("Posição onde final a informação relacionada ao CPF")]
        public int posicaoFinalCPF { get; set; }

        [Display(Name = "Posição inicial da Matricula")]
        [Ajuda("Posição onde inicial a informação relacionada a Matricula")]
        public int posicaoInicialMatricula { get; set; }

        [Display(Name = "Posição final da Matricula")]
        [Ajuda("Posição onde final a informação relacionada a Matricula")]
        public int posicaoFinalMatricula { get; set; }

        [Display(Name = "Posição inicial do Valor")]
        [Ajuda("Posição onde inicial a informação relacionada ao Valor")]
        public int posicaoInicialValor { get; set; }

        [Display(Name = "Posição final do Valor")]
        [Ajuda("Posição onde final a informação relacionada ao Valor")]
        public int posicaoFinalValor { get; set; }


        internal static ConfigParamLeituraArquivoVM ToView(IAPIConfigParamLeituraArquivoVM md)
        {
            return new ConfigParamLeituraArquivoVM
            {
                IsCPF = md.IsCPF,
                codigoCliente = md.codigoCliente,
                extensaoArquivo = md.extensaoArquivoRET,
                extensaoArquivoRET = md.extensaoArquivoRET,
                linhaInicial = md.linhaInicial,
                posicaoInicialCPF = md.posicaoInicialCPF,
                posicaoFinalCPF = md.posicaoFinalCPF,
                posicaoInicialMatricula = md.posicaoInicialMatricula,
                posicaoFinalMatricula = md.posicaoFinalMatricula,
                posicaoInicialValor = md.posicaoInicialValor,
                posicaoFinalValor = md.posicaoFinalValor,
                codigoArquivoDescontoFolha = md.codigoArquivoDescontoFolha,
                codigoFormaVerificacao = md.codigoFormaVerificacao,
                codigoParamUsuario = md.codigoParamUsuario,
                codigoParamValor = md.codigoParamValor,
                nomeCliente = md.nomeCliente

            };
        }
        
    }
    public class FiltroDescontoFolhaVM : IAPIFiltroDescontoFolhaVM
    {
        public int codigoCliente { get; set; }
        public string maticulaUsuario { get; set; } = "";
        public int numeroLote { get; set; }
        public string CPF { get; set; } = "";
        public DateTime dataVencimento { get; set; } = DateTime.Now;
        public bool renovarSaldo { get; set; }
        public string status { get; set; } = "";
        public string CaminhoArquivo { get; set; } = "";
        public int codFatura { get; set; }
        public int codigoConfigArquivo { get; set; }
    }
}
