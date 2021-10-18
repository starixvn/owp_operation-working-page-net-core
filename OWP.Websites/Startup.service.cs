using OWP.TicketCenter;
using OWP.FileUpload;
using OWP.TicketCenter.Hubs;
namespace OWP.Websites
{
    using ApiSyncWorkingShift;
    using App_Start;
    using AutoMapper;
    using Fanex.Caching.Redis;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Data.SqlServer;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.AspNetCore;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Sentry;
    using Fanex.NAP.Client.AspNetCore.Configuration;
    using Fanex.NAP.Client.AspNetCore.Session;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Maxbet;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Olympus.SimpleDataApi;
    using ONEbook.UM.AspNetCore;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;
    using OtherSport;
    using OvertimeReport;
    using TicketCenter= OWP.TicketCenter;
    using OWP.OtherSport.Hubs;
    using PreMatchNotification;
    using RecipientGroup;
    using Sentry;
    using Services;
    using SimpleDataApi;
    using StackExchange.Redis;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using TicketTypeRecipient;

    public partial class Startup
    {
        private const string allowOrigin = "AllowOrigin";

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(options =>
                        {
                            options.LoginPath = Configuration.GetSection("AuthSettings:LoginPath").Value;
                            options.LogoutPath = Configuration.GetSection("AuthSettings:LogoutPath").Value;
                            options.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(Configuration.GetSection("AuthSettings:CookieTimeOut").Value));
                            options.Cookie = new CookieBuilder
                            {
                                Name = "Fanex.OWP.Cookie",
                                SecurePolicy = CookieSecurePolicy.SameAsRequest
                            };
                            options.Events.OnSigningOut = context =>
                            {
                                context.Response.Redirect(options.LoginPath);
                                return Task.CompletedTask;
                            };
                        });
        }

        private static void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                }
            );
        }

        private static void ConfigureCORS(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(allowOrigin,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44380",
                                            "https://localhost:64193",
                                            "http://localhost:64193",
                                            "http://localhost:57420",
                                            "https://owp.nexdev.net",
                                            "https://owp-netcore-v1.nexdev.net",
                                            "https://owp-netcore-v2.nexdev.net",
                                            "https://owp-netcore-v3.nexdev.net",
                                            "https://owp.starixuat.com",
                                            "https://newowp1.starixuat.com",
                                            "https://newowp2.starixuat.com",
                                            "https://newowp3.starixuat.com",
                                            "https://owp.oneops.net",
                                            "https://newowp.oneops.net",
                                            "https://owp1.oneops.net",
                                            "http://localhost:3000",
                                            "https://owp.starixuat.com"
                                            )
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials();
                    });
            });
        }

        private void ConfigureRedis(IServiceCollection services)
        {
            var redisConfig = Configuration.GetSection("AuthSettings:RedisConfig").Value;
            var redis = ConnectionMultiplexer.Connect(redisConfig);

            services.AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "Fanex.OWP-ProtectionKeys");

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfig;
                options.InstanceName = $"Fanex.OWP.Session-";
            });

            services.AddRedisCache(Configuration.GetSection("fanex.caching.redis"));
        }

        private void ConfigureSession(IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(Configuration.GetSection("AuthSettings:SessionTimeOut").Value));
                options.Cookie.Name = "Fanex.OWP.Session";
            });
        }

        private static void ConfigInternalService(IServiceCollection services)
        {
            services.InjectSyncMaxbetService();
            services.InjectOtherSportService();
            services.InjectProductAssignment();
            services.InjectSyncWorkingShift();
            services.InjectPreMatchNotification();
            services.InjectRecipientGroup();
            services.InjectTicketCenterService();
            services.InjectOvertimeReport();
        }

        private void ConfigureNapServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ILogger, Logger>();
            services.AddSingleton(new UMClientConfiguration().Initialize(Configuration));

            services.AddSingleton<INapSettings>(new NapSettings(Configuration));
            services.AddSingleton<INapSession, NapSession>();

            services.AddSingleton<IGroupService, GroupService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ISecurityService, SecurityService>();
            services.AddSingleton<ISiteService, SiteService>();

            services.AddSingleton<IMaintenanceService, MaintenanceService>();
            services.AddSingleton<IUMCacheService, UMCacheService>();
            services.AddSingleton<IUMRestClient, UMRestClient>();
            services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
        }

        private void ConfigureFanex()
        {
            var connections = Configuration.GetSection("ConnectionStrings")
                                   .GetChildren()
                                   .AsEnumerable()
                                   .ToDictionary(
                                            x => x.GetValue<string>("Name"),
                                            y => new ConnectionConfiguration(y.GetValue<string>("Name"), y.GetValue<string>("ConnectionString"), y.GetValue<string>("ProviderName"))
                                    );

            DbSettingProviderManager
              .StartNewSession()
              .Use(connections)
              .WithSqlServer()
              .UseDefaultDbSettingProvider(resourcePath: "~/App_Data/Fanex.Data")
              .Run();

            LogManager
            .SetDefaultLogCategory(Configuration.GetSection("LogCategory").Value)
            .Use(new List<ILogEngine>
            {
                new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(Configuration.GetSection("SentryEngineOptions:Dsn").Value)
                })
            });

            ExceptionHandler
                .UseConfig(new LoggingConfiguration())
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);
        }

        private static void ConfigureCore(IServiceCollection services)
        {
            services.AddScoped<IDynamicRepository, DynamicRepository>();
            services.AddSingleton<LogServiceWrapper, LogServiceWrapperImp>();
            services.AddSingleton<ConfigurationService, ConfigurationServiceImp>();
        }

        private void ConfigureSignalR(IServiceCollection services)
        {
            var redisConfig = Configuration.GetSection("AuthSettings:RedisConfig").Value;
            services.AddSignalR().AddStackExchangeRedis(redisConfig, options =>
            {
                options.Configuration.ChannelPrefix = "OWPNetCore";
            });
            services.AddSingleton<NewDataUploadNotifyHubImp>();
            services.AddSingleton<NewMessageIncomeNotifyHubImp>();
        }
        private static void ConfigureMediator(IServiceCollection services)
        {
            services.AddMediatR(typeof(MaxbetController).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(OtherSportController).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(SyncWorkingShiftController).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(PreMatchNotificationController).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(RecipientGroupController).GetTypeInfo().Assembly);
             services.AddMediatR(typeof(TicketCenterController).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(FileUploadController).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(TicketTypeRecipientController).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(OvertimeReportController).GetTypeInfo().Assembly);
        }

        private static void InitializeSimpleDataApiModule(IServiceCollection services)
        {
            services.AddTransient<SimpleCriteriaFactory, SimpleCriteriaFactoryImp>();
            services.AddTransient<SimpleFunctionPermissionFactory, SimpleFunctionPermissionFactoryImp>();
            services.AddTransient<SimpleMapperFactory, SimpleMapperFactoryImp>();
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new OtherSport.AutoMapperProfile());
                mc.AddProfile(new TicketCenter.AutoMapperProfile());
                mc.AddProfile(new OvertimeReport.AutoMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();
        }
    }
}