using Duende.IdentityServer;
using Duende.IdentityServer.Configuration;
using IDP;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rsk.Saml.Configuration;
using Serilog;
using Rsk.Saml.Generators;

namespace IDP;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        string sDuendeLicenseKey = builder.Configuration["App:DuendeLicense"] ?? string.Empty;
        string sSamlLicenseKey = builder.Configuration["App:RSKSamlLicense"] ?? string.Empty;
        builder.Services.AddRazorPages();

        var isBuilder = builder.Services.AddIdentityServer(options =>
            {
                options.LicenseKey = sDuendeLicenseKey;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
                options.KeyManagement.Enabled = true;
                options.KeyManagement.SigningAlgorithms = new[]
                {
                    new SigningAlgorithmOptions("RS256") {UseX509Certificate = true}
                };
            })
            .AddTestUsers(TestUsers.Users)
            .AddSamlPlugin(options =>
            {
                options.Licensee = "DEMO";
                options.LicenseKey =sSamlLicenseKey;
                options.WantAuthenticationRequestsSigned = false;
            })
            .AddInMemoryServiceProviders(Config.ServiceProviders);

        // in-memory, code config
        isBuilder.AddInMemoryIdentityResources(Config.IdentityResources);
        isBuilder.AddInMemoryApiScopes(Config.ApiScopes);
        isBuilder.AddInMemoryClients(Config.Clients);


        

builder.Services.AddAuthentication();
builder.Services.AddTransient<ISamlNameIdGenerator, CustomNameIdGenerator>();  

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseIdentityServerSamlPlugin();
        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}