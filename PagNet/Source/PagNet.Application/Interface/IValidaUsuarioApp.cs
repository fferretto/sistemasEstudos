using PagNet.Application.Models;
using System.Collections.Generic;

namespace PagNet.Application.Interface
{
    public interface IValidaUsuarioApp
    {
        List<MenuVMs> CarregaMenus(int cod_ope, string PerfilOperadora, string caminhoImagemDefault, bool PossuiNetCard, string login);
        string getCaminhoCss(int cod_ope, string PerfilOperadora, string CaminhoDefault, int codEmpresa);
        string getCaminhoLogo(int cod_ope, string PerfilOperadora, string CaminhoDefault, int codEmpresa);
        string getCaminhoIco(int cod_ope, string PerfilOperadora, string CaminhoDefault, int codEmpresa);
        bool ValidaBaseDados(int cod_ope, string BaseDados);


    }
}
