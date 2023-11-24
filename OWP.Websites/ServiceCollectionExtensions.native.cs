using AutoMapper;
using Fanex.Caching.Redis;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.AspNetCore;
using Fanex.Logging.Extensions.Database;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Authorization;
using Newtonsoft.Json;
using Olympus.SimpleDataApi;
using OWP.ApiSyncWorkingShift;
using OWP.AssigneeSuggestionList;
using OWP.AssignmentUpload;
using OWP.EventPicking;
using OWP.FileUpload;
using OWP.Kpi.LA;
using OWP.LeagueManagement;
using OWP.Maxbet;
using OWP.MaxbetSetting;
using OWP.OSLeagueSetting;
using OWP.OtherSport;
using OWP.OtherSport.Hubs;
using OWP.PrematchAssignment;
using OWP.ProviderArrangement;
using OWP.RabbitMQ;
using OWP.RecipientGroup;
using OWP.Report.LAReport;
using OWP.Report.MatchCountLiveReport;
using OWP.Report.OvertimeReport;
using OWP.Report.PreMatchReport;
using OWP.Report.TraderReport;
using OWP.Services;
using OWP.SimpleDataApi;
using OWP.TicketCenter;
using OWP.TicketCenter.Hubs;
using OWP.TicketEvent;
using OWP.TicketTypeRecipient;
using SharpRaven;
using StackExchange.Redis;
using System.Reflection;
using AutoMapperProfile = OWP.OtherSport.AutoMapperProfile;
using ILogger = Fanex.Logging.ILogger;

namespace OWP.Websites;

using Fanex.NAP.Client.Session;
using Microsoft.Extensions.FileProviders;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutoMapperProfile());
            mc.AddProfile(new TicketCenter.AutoMapperProfile());
            mc.AddProfile(new Report.OvertimeReport.AutoMapperProfile());
            mc.AddProfile(new Report.PreMatchReport.AutoMapperProfile());
            mc.AddProfile(new PrematchAssignment.AutoMapperProfile());
            mc.AddProfile(new AutoAssignment.AutoMapperProfile());
            mc.AddProfile(new OSLeagueSetting.AutoMapperProfile());
            mc.AddProfile(new Report.AutoMapperProfile());
            mc.AddProfile(new LeagueManagement.AutoMapperProfile());
            mc.AddProfile(new MaxbetSetting.AutoMapperProfile());
            mc.AddProfile(new LiveDetail.AutoMapperProfile());
            mc.AddProfile(new AssignmentUpload.AutoMapperProfile());
            mc.AddProfile(new AssigneeSuggestionList.AutoMapperProfile());
            mc.AddProfile(new Kpi.AutoMapperProfile());
        });

        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

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
            });

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
        services.AddTransient<ScopedDynamicRepository, ScopedDynamicRepositoryImp>();
        services.AddScoped<CheckIsLiveStatusServices, CheckIsLiveStatusServicesImp>();
        services.AddScoped<GetAdminSiteMatchIdService, GetAdminSiteMatchIdServiceImp>();
        services.AddScoped<PermissionService, PermissionServiceImp>();
        services.AddScoped<GetInfoSiteTimeService, GetInfoSiteTimeServiceImp>();
        services.AddScoped<AccessCounter, AccessCounterImp>();
        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        static void corsPolicy(CorsPolicyBuilder builder)
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
                                "http://localhost:3001",
                                "https://owp.starixuat.com"
                                )
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
        services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin", corsPolicy);
        });

        return services;
    }

    public static IServiceCollection AddFanexServices(this IServiceCollection services, IConfiguration configuration)
    {
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
        services.AddRecipientGroup();
        services.AddTicketCenterService();
        services.AddReportService();
        services.AddPreMatchAssignmentServices();
        services.AddTicketTypeRecipientService();
        services.AddOSLeagueSetting();
        services.AddLeagueManagementService();
        services.AddLiveDetailService();
        services.AddTicKetEventService();
        services.AddAssignmentUploadService();
        services.AddEventPicking();
        services.AddResourceManagement();
        services.AddUserManagement();
        services.AddSuggestionList();
        services.AddEventService();
        services.AddKpiService();
        return services;
    }

    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(MaxbetController).Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(MaxbetController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(EventPickingController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(MaxbetSettingController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(OtherSportController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(SyncWorkingShiftController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(RecipientGroupController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(TicketCenterController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(FileUploadController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(TicketTypeRecipientController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(OvertimeReportController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(PreMatchAssignmentController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(PreMatchReportController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(MatchCountLiveReportController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(OSLeagueController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(LAReportController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(TraderReportController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(LeagueApiController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(TicketEventController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(AssignmentUploadingApiController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Event.TaskList.TaskListController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(ProviderArrangementController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(AssigneeSuggestionController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(LAKpiController).GetTypeInfo().Assembly));
        return services;
    }

    public static IServiceCollection AddNapServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<ILogger, Logger>();
        services.AddSingleton<INapSettings>(new NapSettings(configuration));
        services.AddSingleton<INapSession, NapSession>();
        services.AddSingleton<IGroupService, GroupService>();
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<ISecurityService, SecurityService>();
        services.AddSingleton<ISiteService, SiteService>();

        return services;
    }

    public static IServiceCollection AddOnebookUMServices(this IServiceCollection services, IConfiguration configuration)
    {
        OnebookUMInitializer.InitializeUMClientManager(configuration);
        OnebookUMInitializer.RegisterServiceCollection(services);

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRedisCache(configuration.GetSection("fanex.caching.redis"));

        var redisConfig = configuration.GetSection("AuthSettings:RedisConfig").Value;
        var redisMultiplexer = ConnectionMultiplexer.Connect(redisConfig);

        services.AddDataProtection().PersistKeysToStackExchangeRedis(redisMultiplexer, "Fanex.OWP-ProtectionKeys");

        services.AddSingleton(redisMultiplexer);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfig;
            options.InstanceName = "Fanex.OWP.Session-";
        });

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
        var channelPrefix = configuration.GetSection("AuthSettings:ChannelPrefix").Value;
        services.AddSignalR()
        .AddStackExchangeRedis(redisConfig, options =>
        {
            options.Configuration.ChannelPrefix = new RedisChannel(channelPrefix, RedisChannel.PatternMode.Auto);
        });
        services.AddSingleton<NewDataUploadNotifyHubImp>();
        services.AddSingleton<TicketCenterNotifyHubImp>();

        return services;
    }

    public static IServiceCollection AddSimpleDataApiModule(this IServiceCollection services)
    {
        SimpleDataApiConfigurationHandler.Instance().SetActorIdParameterName("ActorId");
        services.AddTransient<SimpleCriteriaFactory, SimpleCriteriaFactoryImp>();
        services.AddTransient<SimpleFunctionPermissionFactory, SimpleFunctionPermissionFactoryImp>();
        services.AddTransient<SimpleMapperFactory, SimpleMapperFactoryImp>();

        return services;
    }

    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        services.AddSingleton<MessageClient, MessageClientImp>();

        return services;
    }

    public static WebApplication UseTicketPathStaticFiles(this WebApplication app)
    {
        var env = app.Environment;

        var ticketPaths = Path.Combine(
            env.ContentRootPath,
            "App_Data",
            "Tickets"
        );
        if (!Directory.Exists(ticketPaths))
        {
            Directory.CreateDirectory(ticketPaths);
        }
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(ticketPaths),
            RequestPath = "/Tickets"
        });

        return app;
    }

}