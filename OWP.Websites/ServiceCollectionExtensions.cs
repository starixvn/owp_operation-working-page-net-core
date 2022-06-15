using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Authorization;
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
using MediatR;
using Newtonsoft.Json;
using Olympus.SimpleDataApi;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;
using OWP.ApiSyncWorkingShift;
using OWP.FileUpload;
using OWP.Maxbet;
using OWP.OtherSport;
using OWP.OtherSport.Hubs;
using OWP.OvertimeReport;
using OWP.PreMatchAssignment;
using OWP.PreMatchReport;
using OWP.RecipientGroup;
using OWP.Services;
using OWP.SimpleDataApi;
using OWP.TicketCenter;
using OWP.TicketCenter.Hubs;
using OWP.TicketTypeRecipient;
using OWP.Websites.App_Start;
using RabbitMQ.Client;
using Sentry;
using StackExchange.Redis;
using OWP.RabbitMQ;
using OWP.PreMatchNotification;
using OWP.MatchCountLiveReport;
using OWP.RedisPubSub;
using Fanex.Caching;

namespace OWP.Websites;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddAutoMapper(this IServiceCollection services)
  {
    var mapperConfig = new MapperConfiguration(mc =>
    {
      mc.AddProfile(new OtherSport.AutoMapperProfile());
      mc.AddProfile(new TicketCenter.AutoMapperProfile());
      mc.AddProfile(new OvertimeReport.AutoMapperProfile());
      mc.AddProfile(new PreMatchReport.AutoMapperProfile());
      mc.AddProfile((new PrematchAssignment.AutoMapperProfile()));
      mc.AddProfile(new AutoAssignment.AutoMapperProfile());
    });

    var mapper = mapperConfig.CreateMapper();
    services.AddSingleton(mapper);

    services.AddMvc();

    return services;
  }

  public static IServiceCollection AddControllerAuthorization(this IServiceCollection services)
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

    return services;
  }

  public static IServiceCollection AddCookieAuthentication(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                  options.LoginPath = configuration.GetSection("AuthSettings:LoginPath").Value;
                  options.LogoutPath = configuration.GetSection("AuthSettings:LogoutPath").Value;
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(configuration.GetSection("AuthSettings:CookieTimeOut").Value));
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

    return services;
  }

  public static IServiceCollection AddCoreServices(this IServiceCollection services)
  {
    services.AddScoped<IDynamicRepository, DynamicRepository>();
    services.AddScoped<RefResourceService, RefResourceServiceImp>();
    services.AddSingleton<LogServiceWrapper, LogServiceWrapperImp>();
    services.AddSingleton<ConfigurationService, ConfigurationServiceImp>();
    services.AddScoped<NotifyEngine, NotifyEngineImp>();
    services.AddScoped<IHttpHandler, OWP.Services.HttpClientHandler>();
    services.AddTransient<ScopedDynamicRepository, ScopedDynamicRepositoryImp>();
    return services;
  }

  public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
  {
    Action<CorsPolicyBuilder> corsPolicy = builder =>
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
             };
    services.AddCors(options =>
    {
      options.AddPolicy("AllowOrigin", corsPolicy);
    });

    return services;
  }

  public static IServiceCollection AddFanexServices(this IServiceCollection services, IConfiguration configuration)
  {
    var connections = configuration.GetSection("ConnectionStrings")
                           .GetChildren()
                           .AsEnumerable()
                           .ToDictionary(
                                    x => x.GetValue<string>("Name"),
                                    y => new ConnectionConfiguration(y.GetValue<string>("Name"), y.GetValue<string>("ConnectionString"), y.GetValue<string>("ProviderName"))
                            );
    var storedProcedurePath = configuration.GetValue<string>("StoredProcedurePath");

    DbSettingProviderManager
      .StartNewSession()
      .Use(connections)
      .WithSqlServer()
          .UseDefaultDbSettingProvider(resourcePath: storedProcedurePath)
      .Run();

    LogManager
    .SetDefaultLogCategory(configuration.GetSection("LogCategory").Value)
    .Use(new List<ILogEngine>
    {
                new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(configuration.GetSection("SentryEngineOptions:Dsn").Value)
                })
    });

    ExceptionHandler
        .UseConfig(new LoggingConfiguration())
        .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

    return services;
  }

  public static IServiceCollection AddInternalServices(this IServiceCollection services)
  {
    services.AddAutoAssignmentService();
    services.AddSyncMaxbet();
    services.AddOtherSport();
    services.AddProductAssignment();
    services.AddSyncWorkingShift();
    services.AddPreMatchNotification();
    services.AddRecipientGroup();
    services.AddTicketCenterService();
    services.AddOvertimeReport();
    services.AddPreMatchAssignmentServices();
    services.AddPreMatchReportService();
    services.AddTicketTypeRecipientService();
    services.AddMatchLiveReportService();
    return services;
  }

  public static IServiceCollection AddMediator(this IServiceCollection services)
  {
    services.AddMediatR(typeof(MaxbetController).GetTypeInfo().Assembly);
    services.AddMediatR(typeof(OtherSportController).GetTypeInfo().Assembly);
    services.AddMediatR(typeof(SyncWorkingShiftController).GetTypeInfo().Assembly);
    services.AddMediatR(typeof(PrematchNotificationController).GetTypeInfo().Assembly);
    services.AddMediatR(typeof(RecipientGroupController).GetTypeInfo().Assembly);
    services.AddMediatR(typeof(TicketCenterController).GetTypeInfo().Assembly);
    services.AddMediatR(typeof(FileUploadController).GetTypeInfo().Assembly);
    services.AddMediatR(typeof(TicketTypeRecipientController).GetTypeInfo().Assembly);
    services.AddMediatR(typeof(OvertimeReportController).GetTypeInfo().Assembly);
    services.AddMediatR(typeof(PreMatchAssignmentController).GetTypeInfo().Assembly);
    services.AddMediatR(typeof(PreMatchReportController).GetTypeInfo().Assembly);
    services.AddMediatR(typeof(NotificationConsumer).GetTypeInfo().Assembly);
    services.AddMediatR(typeof(MatchCountLiveReportController).GetTypeInfo().Assembly);
    return services;
  }

  public static IServiceCollection AddNapServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddTransient<Fanex.Logging.ILogger, Logger>();
    services.AddSingleton(new UMClientConfiguration().Initialize(configuration));

    services.AddSingleton<INapSettings>(new NapSettings(configuration));
    services.AddSingleton<INapSession, NapSession>();

    services.AddSingleton<IGroupService, GroupService>();
    services.AddSingleton<IUserService, UserService>();
    services.AddSingleton<ISecurityService, SecurityService>();
    services.AddSingleton<ISiteService, SiteService>();

    services.AddSingleton<IMaintenanceService, MaintenanceService>();
    services.AddSingleton<IUMCacheService, UMCacheService>();
    services.AddSingleton<IUMRestClient, UMRestClient>();
    services.AddSingleton<IHttpContextHelper, HttpContextHelper>();

    return services;
  }

  public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
  {
    var redisConfig = configuration.GetSection("AuthSettings:RedisConfig").Value;
    var redis = ConnectionMultiplexer.Connect(redisConfig);

    services.AddSingleton<IConnectionMultiplexer>(redis);

    services.AddDataProtection()
        .PersistKeysToStackExchangeRedis(redis, "Fanex.OWP-ProtectionKeys");

    services.AddStackExchangeRedisCache(options =>
    {
      options.Configuration = redisConfig;
      options.InstanceName = $"Fanex.OWP.Session-";
    });

    services.AddRedisCache(configuration.GetSection("fanex.caching.redis"));

    return services;
  }

  public static IServiceCollection AddRedisPubSub(this IServiceCollection services, IConfiguration configuration){
    var chanel= configuration.GetSection("RedisChannel:NewLeague").Value;
    services.AddTransient<PubSubEvent, PubSubEventImp>();
    var sp = services.BuildServiceProvider();
    var pubSubEvent= sp.GetService<PubSubEvent>();
    pubSubEvent?.Subscribe(chanel);
    return services;
  }

  public static IServiceCollection AddRedisSession(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddSession(options =>
    {
      options.Cookie.IsEssential = true;
      options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(configuration.GetSection("AuthSettings:SessionTimeOut").Value));
      options.Cookie.Name = "Fanex.OWP.Session";
    });

    return services;
  }

  public static IServiceCollection AddSignalR(this IServiceCollection services, IConfiguration configuration)
  {
    var redisConfig = configuration.GetSection("AuthSettings:RedisConfig").Value;
    services.AddSignalR().AddStackExchangeRedis(redisConfig, options =>
    {
      options.Configuration.ChannelPrefix = "OWPNetCore";
    });
    services.AddSingleton<NewDataUploadNotifyHubImp>();
    services.AddSingleton<TicketCenterNotifyHubImp>();

    return services;
  }

  public static IServiceCollection AddSimpleDataApiModule(this IServiceCollection services)
  {
    services.AddTransient<SimpleCriteriaFactory, SimpleCriteriaFactoryImp>();
    services.AddTransient<SimpleFunctionPermissionFactory, SimpleFunctionPermissionFactoryImp>();
    services.AddTransient<SimpleMapperFactory, SimpleMapperFactoryImp>();

    return services;
  }

  public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddHostedService<NotificationConsumer>();
    return services;
  }
}