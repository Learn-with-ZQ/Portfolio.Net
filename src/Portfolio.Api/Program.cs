using Microsoft.Extensions.FileProviders;
using Portfolio.Api.Extensions;
using Portfolio.Application;
using Portfolio.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddPortfolioSwagger();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AdminPortal", policy =>
        // Dev: allow any localhost / 127.0.0.1 origin regardless of port, so the
        // admin (portfolio-admin) and public (portfolio-web) dev servers work no
        // matter which port ng serve lands on. No credentials/cookies are used
        // (auth is a JWT Bearer header), so a permissive local origin check is safe.
        policy.SetIsOriginAllowed(origin =>
                  Uri.TryCreate(origin, UriKind.Absolute, out var uri)
                  && (uri.Host == "localhost" || uri.Host == "127.0.0.1"))
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UsePortfolioSwagger();

app.UseSerilogRequestLogging();
app.UseCors("AdminPortal");

// Serve public document files straight from a physical folder (no download API).
// IMPORTANT: point this at a PUBLIC-ONLY folder. Never expose identity documents
// (CNIC, passport, domicile) — keep those outside this path entirely.
var docRoot = app.Configuration["Documents:PhysicalRoot"];
var docRequestPath = app.Configuration["Documents:RequestPath"] ?? "/files";
if (!string.IsNullOrWhiteSpace(docRoot))
{
    var fullDocRoot = Path.GetFullPath(docRoot);
    if (Directory.Exists(fullDocRoot))
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(fullDocRoot),
            RequestPath = docRequestPath
        });
        Log.Information("Serving public documents from {Root} at {Path}", fullDocRoot, docRequestPath);
    }
    else
    {
        Log.Warning("Documents:PhysicalRoot '{Root}' does not exist; document serving disabled.", fullDocRoot);
    }
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Starting Portfolio API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Portfolio API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
