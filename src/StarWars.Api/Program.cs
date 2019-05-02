using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace testWebNet
{
#pragma warning disable CS1591 // Brak komentarza XML dla widocznego publicznie typu lub składowej

    public class Program

    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseNLog()
                .UseStartup<Startup>();
    }
}

#pragma warning restore CS1591 // Brak komentarza XML dla widocznego publicznie typu lub składowej