using PagNet.BLD.ContaCorrente2;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;


namespace PagNet.BLD.ContaCorrente.Web.Setup.ContextNegocio
{
    public class ContextApp : ServiceContextBase, IContextoApp
    {
        public ContextApp(IMessageTable messages)
            : base(1, messages)
        { }
    }
}
