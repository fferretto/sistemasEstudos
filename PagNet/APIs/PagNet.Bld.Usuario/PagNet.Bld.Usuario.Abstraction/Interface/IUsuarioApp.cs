using PagNet.Bld.Usuario.Abstraction.Interface.Model;
using PagNet.Bld.Usuario.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.Usuario.Abstraction.Interface
{
    public interface IUsuarioApp
    {
        UsuarioModel GetUsuario(IFiltroModel filtro);
        RetornoModel Salvar(IUsuarioModel model);
        RetornoModel Desativar(IFiltroModel filtro);
        List<UsuarioModel> GetAllUsuarioByCodOpe(IFiltroModel filtro);
        List<UsuarioModel> GetAllUsuarioByCodEmpresa(IFiltroModel filtro);
        RetornoValidaUsuario ValidaLoginRecuperarSenha(IFiltroModel filtro);
        RetornoModel SalvaAlteracaoSenha(IUsuarioModel model);
    }
}
