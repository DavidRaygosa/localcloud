using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostserver = CreateHostBuilder(args).Build();
            using(var ambientence=hostserver.Services.CreateScope()){
                var services = ambientence.ServiceProvider;
                try{
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var context = services.GetRequiredService<DataBaseContext>();
                    context.Database.Migrate();
                } catch(Exception e){
                    var logging = services.GetRequiredService<ILogger<Program>>();
                    logging.LogError(e, "Ocurrio un error en la migracion");
                }  
            }
            var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseUrls("http://*:5000")
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build();
            hostserver.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(15);
                    });
                });
    }
}
