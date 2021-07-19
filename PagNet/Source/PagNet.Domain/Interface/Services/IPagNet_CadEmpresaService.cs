﻿using PagNet.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_CadEmpresaService : IServiceBase<PAGNET_CADEMPRESA>
    {
        Task<PAGNET_CADEMPRESA> ConsultaEmpresaById(int CodEmpresa);
        Task<PAGNET_CADEMPRESA> ConsultaEmpresaBySubRede(int codSubRede);
        Task<PAGNET_CADEMPRESA> ConsultaEmpresaByCNPJ(string CNPJ);
        Task<List<PAGNET_CADEMPRESA>> GetAllempresas();
        void AtualizaEmpresa(PAGNET_CADEMPRESA emp);
        void InserirEmpresa(PAGNET_CADEMPRESA emp);
        object[][] GetEmpresa();
    }
}
