using System;
using System.Collections.Generic;
using System.Text;

namespace NetCard.Bld.Relatorio.Abstraction.Interface
{
    public interface IParametrosApp
    {
        string id_login { get; }
        string tp_usu { get; }
        int cod_ope { get; }
        string BdNetCard { get; }

        string GetConnectionString();
        string GetConnectionStringConcentrador();
    }
}
