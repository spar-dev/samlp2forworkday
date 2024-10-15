using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace SP.Monitoring
{
    public class LoggerSetup
    {
        public static Logger Init(WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration()
           // .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            builder.Services.AddSingleton(logger);
            builder.Host.UseSerilog(logger);
            return logger;
        }
    }
}