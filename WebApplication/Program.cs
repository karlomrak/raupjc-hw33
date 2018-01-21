using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
             BuildWebHost(args).Run();

           
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((context, logger) =>
                {
                    var cnnstr = context.Configuration.GetConnectionString("DefaultConnection");


                logger.MinimumLevel.Error()
                    .Enrich.FromLogContext()
                    .WriteTo.MSSqlServer(
                         cnnstr,
                         "Errors",
                            autoCreateSqlTable: true);

                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        logger.WriteTo.RollingFile("error-log.txt");
                    }

                    
                })
                .Build();
    }
}
