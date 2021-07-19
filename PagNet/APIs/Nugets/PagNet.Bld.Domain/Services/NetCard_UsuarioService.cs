using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services.Common;
using PagNet.Bld.Domain.Interface.Repository;
using System.Linq;
using System.Collections.Generic;

namespace PagNet.Bld.Domain.Services
{
    public class NETCARD_USUARIOPOSService : ServiceBase<NETCARD_USUARIOPOS>, INETCARD_USUARIOPOSService
    {
        private readonly INETCARD_USUARIOPOSRepository _repPos;

        public NETCARD_USUARIOPOSService(INETCARD_USUARIOPOSRepository repPos)
            : base(repPos)
        {
            _repPos = repPos;
        }

        public NETCARD_USUARIOPOS BuscaUsuarioPosByCPF(string cpf)
        {
            var dados = _repPos.Get(x => x.CPF == cpf && x.NUMDEP == 0).FirstOrDefault();
            return dados;
        }

        public NETCARD_USUARIOPOS BuscaUsuarioPosByMatricula(string matricula)
        {
            var dados = _repPos.Get(x => x.MAT == matricula && x.NUMDEP == 0).FirstOrDefault();
            return dados;
        }

        public List<NETCARD_USUARIOPOS> BuscaUsuarioTodosPosByCliente(int codigoCliente)
        {
            var dados = _repPos.Get(x => x.CODCLI == codigoCliente && x.NUMDEP == 0).ToList();
            return dados;
        }

    }
    public class NETCARD_USUARIOPREService : ServiceBase<NETCARD_USUARIOPRE>, INETCARD_USUARIOPREService
    {
        private readonly INETCARD_USUARIOPRERepository _repPre;

        public NETCARD_USUARIOPREService(INETCARD_USUARIOPRERepository repPre)
            : base(repPre)
        {
            _repPre = repPre;
        }

        public NETCARD_USUARIOPRE BuscaUsuarioPreByCPF(string cpf)
        {
            var dados = _repPre.Get(x => x.CPF == cpf && x.NUMDEP == 0).FirstOrDefault();
            return dados;
        }

        public NETCARD_USUARIOPRE BuscaUsuarioPreByMatricula(string matricula)
        {
            var dados = _repPre.Get(x => x.MAT == matricula && x.NUMDEP == 0).FirstOrDefault();
            return dados;
        }
    }

}
