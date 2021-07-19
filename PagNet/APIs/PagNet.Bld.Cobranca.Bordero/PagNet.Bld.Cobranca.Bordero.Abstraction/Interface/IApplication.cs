using PagNet.Bld.Cobranca.Bordero.Abstraction.Interface.Model;
using PagNet.Bld.Cobranca.Bordero.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.Bld.Cobranca.Bordero.Abstraction.Interface
{
    public interface IApplication
    {
        DadosBoletoModel BuscaFaturas(IFiltroBorderoModel filtro);
        List<DadosBorderoModel> ListaBorderos(IFiltroBorderoModel filtro);
        RetornoModel Salvar(IDadosBoletoModel model);
        RetornoModel Cancelar(int codigoBordero);
    }
}
