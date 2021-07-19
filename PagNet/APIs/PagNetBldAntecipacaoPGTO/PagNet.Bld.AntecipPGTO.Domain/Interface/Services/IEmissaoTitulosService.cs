using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Services.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.AntecipPGTO.Domain.Interface.Services
{
    public interface IEmissaoTitulosService : IServiceBase<PAGNET_EMISSAO_TITULOS>
    {
        void IncluiLog(PAGNET_EMISSAO_TITULOS Titulo, int codUsuario, string Justificativa);

        void AtualizaTitulo(PAGNET_EMISSAO_TITULOS Titulo);

        PAGNET_EMISSAO_TITULOS BuscaTituloByID(int CODTITULO);
        List<PAGNET_EMISSAO_TITULOS> BustaTitulosAVencer(int codEmpresa, int codigoFavorecido, DateTime APartirDe);

    }
}
