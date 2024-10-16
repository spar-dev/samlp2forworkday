using System.Security.Cryptography.X509Certificates;
using Rsk.AspNetCore.Authentication.Saml2p;
using Serilog;
using SP.Monitoring;




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

string sSamlLicenseKey = builder.Configuration["App:RSKSamlLicense"] ?? string.Empty;
string sSamlIDPAddress = builder.Configuration["App:IDPMetaURL"] ?? string.Empty;
string sSamlMetaDataPath = builder.Configuration["App:MetaDataPath"] ?? string.Empty;

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "cookie";
        options.DefaultChallengeScheme = "idp";
    }).AddCookie("cookie")
    .AddSaml2p("idp", options =>
    {
        options.Licensee = "DEMO";
        options.LicenseKey = sSamlLicenseKey;
        options.NameIdClaimType = "sub";
        options.CallbackPath = "/signin-saml";
        options.SignInScheme = "cookie";

        options.IdentityProviderMetadataAddress = sSamlIDPAddress;

        options.ServiceProviderOptions = new SpOptions
        {
            EntityId = "RSKSaml",
            MetadataPath = sSamlMetaDataPath,
            SigningCertificate = new X509Certificate2("Resources\\MyTestSamlCert.pfx", "PYt4cwrMem9FxR6v"),
        };
    });

var app = builder.Build();



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
