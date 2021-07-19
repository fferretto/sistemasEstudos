using System.Web;
using System.Web.Mvc;

namespace PagNet.Bld.PGTO.CobrancaBancaria.EmissaoArqu
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
