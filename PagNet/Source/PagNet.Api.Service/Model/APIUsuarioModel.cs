using PagNet.Api.Service.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Model
{
    public class FiltroModel : IFiltroModel
    {
        public int codigoUsuario { get; set; }
        public int codigoOperadora { get; set; }
        public int codigoEmpresa { get; set; }
        public string EmailUsuario { get; set; }
        public string Login { get; set; }
    }
    public class UsuarioModel : IUsuarioModel
    {
        public int codigoUsuario { get; set; }
        public int codigoOperadora { get; set; }
        public string nomeUsuario { get; set; }
        public string Login { get; set; }
        public bool Administrador { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string nomeOperadora { get; set; }
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class RetornoValidaUsuario
    {
        public bool Sucesso { get; set; }
        public string msgResultado { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
    }
}
