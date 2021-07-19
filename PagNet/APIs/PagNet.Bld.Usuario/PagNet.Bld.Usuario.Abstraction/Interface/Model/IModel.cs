using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.Usuario.Abstraction.Interface.Model
{
    public interface IFiltroModel
    {
        int codigoUsuario { get; set; }
        int codigoOperadora { get; set; }
        int codigoEmpresa { get; set; }
        string EmailUsuario { get; set; }
        string Login { get; set; }
    }
    public interface IUsuarioModel
    {
        int codigoUsuario { get; set; }
        int codigoOperadora { get; set; }
        string nomeUsuario { get; set; }
        string Login { get; set; }
        bool Administrador { get; set; }
        string Cpf { get; set; }
        string Email { get; set; }
        string nomeOperadora { get; set; }
        string codigoEmpresa { get; set; }
        string nomeEmpresa { get; set; }
        string Password { get; set; }
        string ConfirmPassword { get; set; }
    }
}
