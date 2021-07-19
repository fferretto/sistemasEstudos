using PagNet.Api.Service.Interface.Model;
using System;
using PagNet.Interface.Helpers.HelperModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Api.Service.Model;
using System.Globalization;
using PagNet.Interface.Helpers;

namespace PagNet.Interface.Areas.ContasReceber.Models
{
    public class FiltroBorderoVM : IAPIFiltroBorderoModel
    {
        public bool acessoAdmin { get; set; }
        public int codigoCliente { get; set; }

        [Display(Name = "Código")]
        [Ajuda("Poderá ser utilizado o códgio ou CNPJ do cliente ")]
        public string filtroCliente { get; set; }

        [Display(Name = "Cliente")]
        public string nomeCliente { get; set; }

        [Display(Name = "Empresa")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }

        [Display(Name = "Data de Início")]
        [Ajuda("Data de Vencimento do Boleto")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string DataInicial { get; set; }

        [Display(Name = "Data de Térmno")]
        [Ajuda("Data de Vencimento do Boleto")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string DataFinal { get; set; }

        [Display(Name = "Código do Borderô")]
        public int codigoBordero { get; set; }

        [Display(Name = "Conta Corrente *")]
        [Ajuda("Conta que irá gerar os boletos")]
        public string codigoContaCorrente { get; set; }
        public string nomeContaCorrente { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }
        public string nmStatus { get; set; }

    }
    public class ResumoFatuasBorderoModel
    {
        public ResumoFatuasBorderoModel()
        {
            ListaBoletos = new List<DadosFaturaBorderoModel>();
        }
        public int codigoEmpresaBordero { get; set; }
        public int codigoContaCorrenteBordero { get; set; }

        [Display(Name = "Selecione os Itens para Incluir no Borderô")]
        public IList<DadosFaturaBorderoModel> ListaBoletos { get; set; }
        
        [Display(Name = "Faturas Selecionadas")]
        [Ajuda("Quantidade de faturas que serão incluídos neste borderô")]
        public int qtFaturasSelecionados { get; set; }

        [Display(Name = "Valor Borderô")]
        [Ajuda("Valor total a ser incluído neste borderô")]
        [InputAttrAux(Inicio = "R$")]
        public string ValorBordero { get; set; }


        internal static ResumoFatuasBorderoModel ToView(IAPIDadosBoletoModel item)
        {
            return new ResumoFatuasBorderoModel
            {
                codigoEmpresaBordero = item.codigoEmpresa,
                codigoContaCorrenteBordero = item.codigoContaCorrente,
                qtFaturasSelecionados = item.qtFaturasSelecionados,
                ValorBordero = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.ValorBordero),
                ListaBoletos = DadosFaturaBorderoModel.ToList(item.ListaBoletos)
            };
        }
    }
    public class DadosFaturaBorderoModel
    {
        public bool chkBoleto { get; set; }
        public int codigoFatura { get; set; }

        public int codigoCliente { get; set; }
        public string nomeCliente { get; set; }
        public string cnpj { get; set; }

        public string dataVencimento { get; set; }
        public string Valor { get; set; }
        public int QuantidadeFatura { get; set; }

        internal static IList<DadosFaturaBorderoModel> ToList<T>(ICollection<T> collection) where T : APIFaturasBorderoModel
        {
            return collection.Select(x => ToList(x)).ToList();
        }

        internal static DadosFaturaBorderoModel ToList(APIFaturasBorderoModel _bol)
        {
            return new DadosFaturaBorderoModel
            {
                chkBoleto = true,
                codigoFatura = _bol.codigoFatura,
                codigoCliente = _bol.codigoCliente,
                nomeCliente = _bol.nomeCliente,
                cnpj = Util.FormataCPFCnPj(_bol.cnpj),
                dataVencimento = _bol.dataVencimento.ToShortDateString(),
                Valor = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _bol.Valor)).Replace("R$", ""),
                QuantidadeFatura = _bol.QuantidadeFatura,
                
            };
        }
    }

}