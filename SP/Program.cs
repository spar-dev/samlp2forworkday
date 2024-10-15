using Rsk.AspNetCore.Authentication.Saml2p;

var builder = WebApplication.CreateBuilder(args);

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
            MetadataPath = sSamlMetaDataPath
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
