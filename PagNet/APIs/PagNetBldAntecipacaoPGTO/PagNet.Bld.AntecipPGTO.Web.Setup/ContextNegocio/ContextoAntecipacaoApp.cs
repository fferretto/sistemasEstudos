using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.Bld.AntecipPGTO.Web.Setup.ContextNegocio
{
    public class ContextoAntecipacaoApp : ServiceContextBase, IContextoAntecipacaoApp
    {
        public ContextoAntecipacaoApp(IMessageTable messages)
            : base(1, messages)
        { }
    }
}
