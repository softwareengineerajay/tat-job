using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityModel.AspNetCore.OAuth2Introspection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NOV.ES.Framework.Core.CQRS.Behaviors;
using NOV.ES.Framework.Core.CQRS.Commands;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.Framework.Core.Data;
using NOV.ES.TAT.Common.FeatureToggle.Service;
using NOV.ES.TAT.Common.UserPermissions.Service;
using NOV.ES.TAT.Job.API.Constant;
using NOV.ES.TAT.Job.API.Filters;
using NOV.ES.TAT.Job.API.Validators;
using NOV.ES.TAT.Job.Infrastructure;
using NOV.ES.TAT.Job.Interfaces;
using NOV.ES.TAT.Job.Service;
using Polly;
using Polly.Extensions.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace NOV.ES.TAT.Job.API
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddControllers(
                options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                    options.Filters.Add(typeof(GlobalAuthorizationFilter));
                }
            ).AddNewtonsoftJson(
                joptions =>
                {
                    joptions.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    joptions.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    joptions.SerializerSettings.Converters.Add(new StringEnumConverter());
                }
            );
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("*"));
            });
            return services;
        }
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            return services;
        }
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(OAuth2IntrospectionDefaults.AuthenticationScheme)
            .AddOAuth2Introspection(options =>
            {
                options.Authority = configuration["Authority"];
                options.ClientId = configuration["ClientId"];

                options.Events = new OAuth2IntrospectionEvents()
                {
                    OnTokenValidated = context =>
                    {
                        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadToken(context.SecurityToken);
                        context.HttpContext.Items["user_email_id"] = ((JwtSecurityToken)jwtSecurityToken)
                        .Claims.FirstOrDefault(c => c.Type == "sub").Value;
                        context.HttpContext.Items["access_token"] = (JwtSecurityToken)jwtSecurityToken;
                        return Task.CompletedTask;
                    }
                };
            })
            .AddJwtBearer(ConstantsProperty.IDENTITY_AUTH_SCHEME, options =>
            {
                /// local url from rancher for internal identity server
                /// http://tat-identity-server.trackatool-dev.svc.cluster.local
                options.Authority = configuration["InternalIdentity"];
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
                options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        context.HttpContext.Items["x-internal-identity"] = "true";
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Get", poblicBuilder => poblicBuilder
                     .RequireAuthenticatedUser()
                     .AddAuthenticationSchemes("client1"));
                options.AddPolicy("Update", poblicBuilder => poblicBuilder
                     .RequireAuthenticatedUser()
                     .RequireClaim("Permission", "canEdit")
                     .RequireRole("update")
                     .RequireRole("SuperUser")
                     .AddAuthenticationSchemes("client1"));
                options.AddPolicy("Create", poblicBuilder => poblicBuilder
                     .RequireAuthenticatedUser()
                     .RequireClaim("Permission", "canAdd")
                     .RequireRole("create")
                     .RequireRole("SuperUser")
                     .AddAuthenticationSchemes("client1"));
                options.AddPolicy("Delete", poblicBuilder => poblicBuilder
                     .RequireAuthenticatedUser()
                     .RequireClaim("Permission", "canDelete")
                     .RequireRole("delete")
                     .RequireRole("SuperUser")
                     .AddAuthenticationSchemes("client1"));

            });
            return services;
        }
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<JobDBContext>(options =>
            {
                options.UseSqlServer(configuration["TrackAToolDbConnString"],
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            },
            ServiceLifetime.Scoped
            );

            return services;
        }
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TAT Job REST API",
                    Version = "v1",
                    Description = "The Job Service REST API"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                          new OpenApiSecurityScheme()
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                    }
                });
            });

            return services;
        }
        public static IServiceCollection AddDomainCoreDependecy(this IServiceCollection services)
        {

            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IQueryBus, QueryBus>();
            services.AddScoped<BaseContext, JobDBContext>();
            services.AddScoped<IRequestManager, RequestManager>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            services.AddScoped<INovJobCommandRepository, NovJobCommandRepository>();
            services.AddScoped<INovJobQueryRepository, NovJobQueryRepository>();
            services.AddScoped<INovJobService, NovJobService>();

            services.AddScoped<IJobSnapShotCommandRepository, JobSnapShotCommandRepository>();
            services.AddScoped<IJobSnapShotQueryRepository, JobSnapShotQueryRepository>();
            services.AddScoped<IJobSnapShotService, JobSnapShotService>();

            services.AddScoped<INovJobDetailsQueryRepository, NovJobDetailsQueryRepository>();
            services.AddScoped<INovJobDetailsService, NovJobDetailsService>();

            services.AddScoped<IUsageDetailsQueryRepository, UsageDetailsQueryRepository>();
            services.AddScoped<IUsageDetailsService, UsageDetailsService>();

            services.AddScoped<IFieldTransferDetailsQueryRepository, FieldTransferDetailsQueryRepository>();
            services.AddScoped<IFieldTransferDetailsService, FieldTransferDetailsService>();

            services.AddScoped<ISalesOrderDetailsQueryRepository, SalesOrderDetailsQueryRepository>();
            services.AddScoped<ISalesOrderDetailsService, SalesOrderDetailsService>();

            return services;
        }
        public static IServiceCollection RegistorMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);

            return services;
        }
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
        public static IServiceCollection AddPolly(this IServiceCollection services, IConfiguration configuration)
        {
            #region PollyImplementation

            services.AddHttpClient<IUserProfileService, UserProfileService>(client =>
            {
                client.BaseAddress = new Uri(configuration["UPM_Base_url"]);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(Convert.ToInt32(configuration["HandlerLifetimeInMinutes"])))
            .AddPolicyHandler(GetWaitRetryPolicy(configuration));

            services.AddHttpClient<IFeatureToggleService, FeatureToggleService>(client =>
            {
                client.BaseAddress = new Uri(configuration["UPM_Base_url"]);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(Convert.ToInt32(configuration["HandlerLifetimeInMinutes"])))
            .AddPolicyHandler(GetWaitRetryPolicy(configuration));
            #endregion
            return services;
        }

        public static IServiceCollection AddFluentValidator(this IServiceCollection services, IConfiguration configuration)
        {
            #region FluentValidation            
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<NovJobValidator>());
            #endregion
            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetWaitRetryPolicy(IConfiguration configuration)
        {
            return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(Convert.ToInt32(configuration["RetryCount"])
            , retryAttempt => TimeSpan.FromSeconds(Convert.ToInt32(configuration["RetryCount"])));
        }
    }
}
