using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PagNet.Bld.PGTO.CobrancaBancaria.Web.Setup;

namespace PagNet.Bld.PGTO.CobrancaBancaria.Server
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
