using MediatR;
using Microsoft.EntityFrameworkCore;
using NOV.ES.TAT.Job.API;
using NOV.ES.TAT.Job.Infrastructure;
using Serilog;

#pragma warning disable S1481 // Unused local variables should be removed
var configuration = GetConfiguration();
#pragma warning restore S1481 // Unused local variables should be removed

try
{
    var host = CreateWebHostBuilder(args);

    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<JobDBContext>();
            context.Database.Migrate(); // apply all migrations
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred when updating DB.");
        }
    }
    host.Run();

    return 0;
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

IHost CreateWebHostBuilder(string[] args) =>
   Host.CreateDefaultBuilder(args)
   .ConfigureServices(services =>
       {
           services.Configure<HostOptions>(hostOptions =>
           {
               hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
           });
       })
       .ConfigureWebHostDefaults(webBuilder =>
       {
           webBuilder
             .UseStartup<Startup>()
             .UseSerilog();
       })
       .Build();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

#pragma warning disable S3903 // Types should be defined in named namespaces
public partial class Program
#pragma warning restore S3903 // Types should be defined in named namespaces
{
    protected Program() { }
    private static string Namespace = typeof(Startup).Namespace;
    public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}