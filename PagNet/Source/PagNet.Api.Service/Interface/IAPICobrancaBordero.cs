using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface
{
    public interface IAPICobrancaBordero
    {
        APIDadosBoletoModel BuscaFaturas(IAPIFiltroBorderoModel filtro);
        List<APIDadosBorderoModel> ListaBorderos(IAPIFiltroBorderoModel filtro);
        RetornoModel Salvar(IAPIDadosBoletoModel model);
        RetornoModel Cancelar(int codigoBordero);
    }
}
