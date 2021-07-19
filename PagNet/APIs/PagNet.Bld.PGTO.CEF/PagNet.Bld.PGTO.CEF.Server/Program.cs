using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PagNet.Bld.PGTO.CEF.Web.Setup;

namespace PagNet.Bld.PGTO.CEF.Server
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
