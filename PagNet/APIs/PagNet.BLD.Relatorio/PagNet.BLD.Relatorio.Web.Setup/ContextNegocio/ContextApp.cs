using PagNet.BLD.Relatorio.Job;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.BLD.Relatorio.Web.Setup.ContextNegocio
{
    public class ContextApp : ServiceContextBase, IContextoApp
    {
        public ContextApp(IMessageTable messages)
            : base(1, messages)
        { }
    }


}
