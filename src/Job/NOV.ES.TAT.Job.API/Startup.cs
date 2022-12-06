using Elastic.Apm.NetCoreAll;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Enrichers;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Sinks.Elasticsearch;

namespace NOV.ES.TAT.Job.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = GetElasticLogger();
        }
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
            .AddCustomMvc()
            .AddHttpContextAccessor()
            .AddCustomAuthentication(Configuration)
            .AddCustomAuthorization()
            .AddHealthChecks(Configuration)
            .AddHttpContextAccessor()
            .AddCustomDbContext(Configuration)
            .AddCustomSwagger(Configuration)
            .RegistorMediatR()
            .AddAutoMapper()
            .AddDomainCoreDependecy()
            .AddPolly(Configuration)
            .AddFluentValidator(Configuration);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Job V1");
                });
            app.UseAllElasticApm(Configuration);
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapGet("/_proto/", async ctx =>
                {
                    ctx.Response.ContentType = "text/plain";
                    using var fs = new FileStream(Path.Combine(env.ContentRootPath, "Proto", "basket.proto"), FileMode.Open, FileAccess.Read);
                    using var sr = new StreamReader(fs);
                    while (!sr.EndOfStream)
                    {
                        var line = await sr.ReadLineAsync();
                        if (line != "/* >>" || line != "<< */")
                        {
                            await ctx.Response.WriteAsync(line);
                        }
                    }
                });
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });

        }
        private Serilog.ILogger GetElasticLogger()
        {
            return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
            .Enrich.WithProperty("Application", Program.AppName)
            .Enrich.FromLogContext()
            //.Enrich.WithExceptionDetails()
            .Enrich.With(new ThreadIdEnricher())
            .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
            .WithDefaultDestructurers()
            .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() }))
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .WriteTo.Debug()
            .Enrich.WithProperty("Environment", $"{Configuration["ASPNETCORE_ENVIRONMENT"]}")
            .WriteTo.Elasticsearch(ConfigureElasticSink()).CreateLogger();
        }
        private ElasticsearchSinkOptions ConfigureElasticSink()
        {
            return new ElasticsearchSinkOptions(new Uri(Configuration["ElasticSearchUrl"]))
            {
                ModifyConnectionSettings = x => x.BasicAuthentication(Configuration["ElasticSearchUserName"], Configuration["ElasticSearchPassword"]),
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                IndexFormat = Configuration["ElasticSearchIndex"]
            };
        }
    }
}
