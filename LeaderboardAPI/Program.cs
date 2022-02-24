/* Author: Phillip Pham
 * Date: 02/20/22
 * Description: Program entry point.
 */

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LeaderboardAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
