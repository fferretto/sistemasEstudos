using PagNet.Bld.PGTO.ABCBrasil.Abstraction.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Telenet.AspNetCore.Mvc.Core.ServiceHost;

namespace PagNet.Bld.PGTO.ABCBrasil.Web.Setup.Controllers
{
    public class PagamentoABCBrasilController : ServiceController<IPagamentoApp>
    {
        public PagamentoABCBrasilController(IPagamentoApp service)
            : base(service)
        { }
    }
}
