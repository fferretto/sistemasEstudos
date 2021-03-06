using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PagNet.BLD.ProjetoPadrao.Web.Setup;

namespace ProjetoPadrao
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
