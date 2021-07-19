using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface
{
    public interface IAPIUsuario
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
