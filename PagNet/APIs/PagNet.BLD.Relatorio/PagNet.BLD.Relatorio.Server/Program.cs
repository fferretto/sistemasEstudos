using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PagNet.BLD.Relatorio.Web.Setup;

namespace PagNet.BLD.Relatorio.Server
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
