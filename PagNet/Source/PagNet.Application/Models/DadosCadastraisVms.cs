
using System;
using System.ComponentModel.DataAnnotations;
using PagNet.Application.Helpers;

namespace PagNet.Application.Models
{
    public class ClienteVM
    {
        public ClienteVM()
        {
            Endereco = new EnderecoVM();
        }

        public virtual EnderecoVM Endereco { get; set; }

        public int CodCliente { get; set; }
        public string Btn { get; set; }
        public bool bVinculaOperadora { get; set; }

        [Display(Name = "Nome do Cliente")]
        [Required(ErrorMessage = "Informe o nome da empresa")]
        [Ajuda("Nome da empresa que efetuará os pagamentos")]
        [StringLength(60)]
        public string nmCliente { get; set; }

        [Display(Name = "CPF / CNPJ")]
        [Ajuda("CPF ou CNPJ da empresa")]
        public string CpfCnpj { get; set; }
        
        [Display(Name = "Habilitar Geração de Boleto")]
        [Ajuda("Libera Opção de gerar boletos de pagamentos.")]
        public bool bBoleto { get; set; }

        [Display(Name = "Habilitar Pagamento")]
        [Ajuda("Libera a opção de realizar pagamentos de títulos e tributos.")]
        public bool bPagamento { get; set; }


        [Display(Name = "Status do Cliente")]
        public bool bAtivo { get; set; }

        [Display(Name = "Caminho do Arquivo")]
        [Ajuda("Dentro deste arquivo será criado automaticamente as pastas que armazenarão os arquivos de remessa, retorno e boletos gerados.")]
        [Required(ErrorMessage = "Informe um caminho para os arquivos gerados.")]
        public string CaminhoArquivo { get; set; }

        [Display(Name = "Nome do Banco de Dados do Cliente")]
        [Ajuda("Será o banco de dados que o cliente utilizará para armazenar as informações deles.")]
        [Required(ErrorMessage = "Informe o nome do banco de dados.")]
        public string nmBancoDados { get; set; }

        [Display(Name = "Código da Operadora")]
        [Ajuda("Este código será utilizado para transações realizadas dentro do NetCard e Modulo de Consulta.")]
        public short codOpe { get; set; }

        public string statusClliente { get; set; }

        internal static ClienteVM ToViewAPI(APIClienteVM Api)
        {
            return new ClienteVM()
            {
                CodCliente = Api.CodCliente,
                nmCliente = Api.nmCliente.Trim(),
                CpfCnpj = Geral.FormataCPFCnPj(Api.CpfCnpj.Trim()),
                bBoleto = Api.bBoleto,
                bPagamento = Api.bPagamento,
                bAtivo = Api.bAtivo,
                statusClliente = (Api.bAtivo) ? "Ativo" : "Desativado",
                nmBancoDados = Api.BD_Pag,
                CaminhoArquivo = Api.CaminhoArquivo,
                codOpe = Api.codOpe,
                bVinculaOperadora = (Api.codOpe > 0) ? true : false,

                Endereco = EnderecoVM.ToViewAPI(Api)
            };
        }
    

    }
    public class APIClienteVM
    {
        public int CodCliente { get; set; }
        public string nmCliente { get; set; }
        public string CpfCnpj { get; set; }
        public string LogradouroEndereco { get; set; }
        public int LogradouroNumero { get; set; }
        public string LogradouroComplemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }

        public string BD_Pag { get; set; }
        public string CaminhoArquivo { get; set; }

        public short codOpe { get; set; }
        public bool bBoleto { get; set; }
        public bool bPagamento { get; set; }
        public bool bAtivo { get; set; }


        internal static APIClienteVM ToEntity(ClienteVM model)
        {
            return new APIClienteVM
            {
                CodCliente = model.CodCliente,
                nmCliente = model.nmCliente.ToString(),
                CpfCnpj = Geral.RemoveCaracteres(model.CpfCnpj),
                LogradouroEndereco = model.Endereco.Endereco.ToString(),
                LogradouroNumero = Convert.ToInt32(model.Endereco.Numero),
                LogradouroComplemento = model.Endereco.Complemento ?? "",
                Bairro = model.Endereco.LocalidadeBairroDescricao.ToString(),
                Cidade = model.Endereco.LocalidadeMunicipioDescricao.ToString(),
                UF = model.Endereco.LocalidadeUfDescricao.ToString(),
                CEP = model.Endereco.Cep.ToString(),
                bAtivo = model.bAtivo,
                bBoleto = model.bBoleto,
                bPagamento = model.bPagamento,

                BD_Pag = model.nmBancoDados,
                CaminhoArquivo = model.CaminhoArquivo,
                codOpe = model.codOpe,
            };
        }
    }
    public class APICliente_NetPagVM
    {
        public int codCliente_NetPag { get; set; }
        public string nmCliente_NetPag { get; set; }
        public int? codOpe { get; set; }
        public bool bBoleto { get; set; }
        public bool bPagamento { get; set; }
    }
    public class Cliente_NetPagVM
    {
        public Cliente_NetPagVM(APICliente_NetPagVM x)
        {
            codCliente_NetPag = x.codCliente_NetPag;
            nmCliente_NetPag = x.nmCliente_NetPag.Trim();
            bBoleto = (x.bBoleto) ? "Sim" : "Não";
            bPagamento = (x.bPagamento) ? "Sim" : "Não";
        }

        public int codCliente_NetPag { get; set; }

        [Display(Name = "Nome do Cliente")]
        public string nmCliente_NetPag { get; set; }

        [Display(Name = "Gera Boleto")]
        public string bBoleto { get; set; }

        [Display(Name = "Faz Pagamento")]
        public string bPagamento { get; set; }

    }
}
