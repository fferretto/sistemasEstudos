using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.BLD.ProjetoPadrao.Web.Setup.ContextNegocio
{
    public class ContextApp : ServiceContextBase, IContextoAntecipacaoApp
    {
        public ContextApp(IMessageTable messages)
            : base(1, messages)
        { }
    }
}
