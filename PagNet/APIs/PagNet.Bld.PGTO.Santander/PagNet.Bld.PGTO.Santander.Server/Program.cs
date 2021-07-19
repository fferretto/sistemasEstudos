using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PagNet.Bld.PGTO.Santander.Web.Setup;

namespace PagNet.Bld.PGTO.Santander.Server
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
