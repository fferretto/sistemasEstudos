using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PagNet.Bld.PGTO.Brasil.Web.Setup;

namespace PagNet.Bld.PGTO.Brasil.Server
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
