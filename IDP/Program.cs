using IDP;
using IDP.Monitoring;
using Serilog;

/* Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up"); */
try
{
 var builder = WebApplication.CreateBuilder(args);
 var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddCommandLine(args)
            .Build();
    // Setup logging, tracing and metrics
    var logger = LoggerSetup.Init(builder);
    Log.Logger = logger;

    Log.Information("Starting the app");


    

    

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}