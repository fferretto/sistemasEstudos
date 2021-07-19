using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PagNet.Bld.PGTO.ABCBrasil.Web.Setup;

namespace PagNet.Bld.PGTO.ABCBrasil.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
