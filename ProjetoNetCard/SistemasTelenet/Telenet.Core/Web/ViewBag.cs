// -----------------------------------------------------------------------------
// Telenet Tecnologia e Serviços em Rede
// Autor: Alexandre Chestter
// Data: 16/09/2019
// -----------------------------------------------------------------------------

#pragma warning disable 1591

using System.Collections.Generic;

namespace Telenet.Core.Web
{
    public class ViewBag : Dictionary<string, object>, IViewBag
    { }
}

#pragma warning restore 1591
