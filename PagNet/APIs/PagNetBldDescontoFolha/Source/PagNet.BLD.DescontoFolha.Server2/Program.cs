using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PagNet.BLD.DescontoFolha.Web.Setup2;

namespace PagNet.BLD.DescontoFolha.Server2
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
