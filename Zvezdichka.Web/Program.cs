using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Zvezdichka.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
//                .UseSetting("https_port","8080")
                .UseStartup<Startup>()
                .Build();
    }
}