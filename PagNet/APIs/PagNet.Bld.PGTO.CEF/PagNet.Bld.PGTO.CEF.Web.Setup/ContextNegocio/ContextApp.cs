using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.Bld.PGTO.CEF.Web.Setup.ContextNegocio
{
    public class ContextApp : ServiceContextBase, IContextoApp
    {
        public ContextApp(IMessageTable messages)
            : base(1, messages)
        { }
    }
}
