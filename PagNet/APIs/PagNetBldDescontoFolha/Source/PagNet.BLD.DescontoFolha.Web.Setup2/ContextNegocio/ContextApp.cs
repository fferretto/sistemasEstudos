using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.BLD.DescontoFolha.Web.Setup2.ContextNegocio
{
    public class ContextApp : ServiceContextBase, IContextoApp
    {
        public ContextApp(IMessageTable messages)
            : base(1, messages)
        { }
    }
}
