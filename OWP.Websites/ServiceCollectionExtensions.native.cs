using AutoMapper;
using Fanex.Caching.Redis;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.AspNetCore;
using Fanex.Logging.Extensions.Database;
using Fanex.Logging.Sentry.RavenClient;
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
using OWP.PrematchAssignment;
using OWP.ProviderArrangement;
using OWP.ProviderVerification;
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
using OWP.TicketEvent;
using OWP.TicketTypeRecipient;
using SharpRaven;
using StackExchange.Redis;
using System.Reflection;
using OWP.Websites.App_Start;
using ILogger = Fanex.Logging.ILogger;

namespace OWP.Websites;

using Fanex.NAP.Client.Session;
using Microsoft.Extensions.FileProviders;
using OWP.AccountSetting.Profile;
using AccountSetting.QuickMessage;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new OtherSport.AutoMapperProfile());
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
            mc.AddProfile(new Event.LiveGameAssignment.Download.AutoMapperProfile());
            mc.AddProfile(new Application.AutoMapperProfile());
            mc.AddProfile(new WorkingShift.Common.AutoMapperProfile());
            mc.AddProfile(new DailyWorkingShift.AutoMapperProfile());
            mc.AddProfile(new MessageManagement.AutoMapperProfile());
            mc.AddProfile(new OddsCompiler.AutoMapperProfile());
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
        services.AddScoped<UserRepository, UserRepositoryImp>();
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
        services.AddNewLeagueSettingService();
        services.AddAccountSettingService();
        services.AddProviderVerificationService();
        services.AddApplicationInjector();
        services.AddWorkingShiftService();
        services.AddDailyWorkingShiftService();
        services.AddMessageManagementService();
        services.AddOddsCompilerService();
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
        services.AddMediatR(x => x.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("OWP.NewLeagueSetting")));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(ProfileController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(QuickMessageController).GetTypeInfo().Assembly));
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(ProviderVerificationController).GetTypeInfo().Assembly));
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

    public static IServiceCollection AddWebOptimizer(this IServiceCollection services)
    {
        services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddCssBundle("/styles/common",
                    "shared/lib/Fanex.Grid/Fanex.Grid.css",
                    "shared/lib/Fanex.Grid/Fanex.Pagination.css",
                    "shared/lib/fanex-multiselect/fanex.multiSelect.css",
                    "shared/lib/Materialize/materialize.css",
                    "shared/lib/CustomScrollbar/jquery.mCustomScrollbar.css",
                    "shared/lib/Select2/select2.css",
                    "shared/lib/jQuery-Wizard/wizard.css",
                    "shared/lib/Tipsy/tipsy.css",
                    "shared/lib/Colpick/colpick.css",
                    "shared/lib/SweetAlert2/sweetalert2.css",
                    "shared/lib/jQueryTagsinput/jquery.tagsinput.min.css",
                    "shared/lib/jquery-timepicker/jquery.timepicker.css",
                    "shared/lib/MsgBox/msgbox-0.0.1.css",
                    "shared/lib/SimplePagination/simplePagination.css",
                    "shared/lib/Toastr/toastr.css",
                    "shared/lib/Chosen/bootstrap-chosen.css",
                    "shared/owp/copy.css",
                    "shared/owp/owp.search-autocomplete.css",
                    "shared/themes/main.css",
                    "shared/themes/searchBlock.css"
                   );

                pipeline.AddCssBundle("/styles/fonts",
                    "shared/fonts/icon-OWP/style.css",
                     "shared/themes/fonts/OWP-v1.0/style.css",
                    "shared/themes/fonts/font-icon/style.css",
                    "shared/themes/fonts/flags/flag-icon.css");

                pipeline.AddJavaScriptBundle("scripts/core",
                    "shared/lib/jQuery/jquery-2.1.1.js",
                    "shared/lib/jquery.signalR-2.4.3.min.js");

                pipeline.AddJavaScriptBundle("scripts/common",
                    "shared/lib/JQuery/jquery-ui-1.10.4.js",
                    "shared/lib/JQuery/jquery.blockUI.2.66.min.js",
                    "shared/lib/Others/moment-latest.js",
                    "shared/lib/Chosen/chosen.jquery.js",
                    "shared/lib/MsgBox/msgbox-0.0.1.js",
                    "shared/lib/jquery-timepicker/jquery.timepicker.js",
                    "shared/lib/Others/namespace.js",
                    "shared/lib/js.cookie/js.cookie.js",

                    "shared/owp/common.js",

                    "shared/lib/Materialize/materialize.js",
                    "shared/lib/CustomScrollbar/jquery.mCustomScrollbar.concat.min.js",
                    "shared/lib/Select2/select2.min.js",
                    "shared/lib/jQuery-Wizard/jquery-wizard.js",
                    "shared/lib/Tipsy/jquery.tipsy.js",
                    "shared/lib/jQueryTagsinput/jquery.tagsinput.min.js",
                    "shared/lib/Inputmask/jquery.inputmask.bundle.js",
                    "shared/lib/jquery.shiftcheckbox.js",
                    "shared/lib/Colpick/colpick.js",
                    "shared/lib/SweetAlert2/sweetalert2.js",
                    "shared/lib/Fanex.Grid/Fanex.Grid.js",
                    "shared/lib/Fanex.Grid/Fanex.Pagination.js",
                    "shared/lib/SimplePagination/jquery.simplePagination.js",
                    "shared/lib/ContextMenu/bootstrap-contextmenu.js",
                    "shared/lib/fanex-multiselect/fanex.multiSelect.js",
                    "shared/lib/Toastr/toastr.js",
                    "shared/lib/Sortable/Sortable.js",
                    "shared/lib/favico/favico-0.3.10.min.js",
                    "shared/owp/navbar.js",
                    "shared/owp/main.js",
                    "shared/lib/modernizr.js",
                    "shared/owp/DocumentTitle/document-title.js",
                    "shared/owp/DocumentTitle/document-title-factory.js"
                );

                pipeline.AddCssBundle("Styles/EventInformation",
                    "shared/owp/TaskList/TaskList.css",
                    "shared/owp/EventManagement/Resources/EventInformation/EventInformation.css");

                pipeline.AddJavaScriptBundle("Scripts/EventInformation",
                    "shared/lib/jquery.floatThead/jquery.floatThead.min.js",
                    "shared/owp/TaskList/task-list-factory.js",
                    "shared/owp/EventManagement/Resources/EventInformation/*.js",
                    "shared/owp/EventManagement/Resources/Filter/*.js");

                pipeline.AddJavaScriptBundle("Scripts/WeeklyWorkingShift/IFrame",
                    "shared/lib/jQuery/jquery-2.1.1.min.js",
                    "shared/lib/jQuery/jquery-ui-1.10.4.min.js",
                    "shared/lib/MsgBox/msgbox-0.0.1.js",
                    "shared/owp/MsgBox/owp.search-autocomplete.js",
                    "shared/owp/MsgBox/common.js");

                pipeline.AddJavaScriptBundle("Scripts/WeeklyWorkingShift",
                    "shared/owp/weekly-working-shift/*.js");

                pipeline.AddCssBundle("Styles/WeeklyWorkingShift",
                    "shared/owp/weekly-working-shift/WorkingShift.css");

                pipeline.AddJavaScriptBundle("Scripts/DailyWorkingShift",
                    "shared/owp/daily-working-shift/*.js");

                pipeline.AddCssBundle("Styles/DailyWorkingShift",
                    "shared/themes/main.css",
                    "shared/owp/daily-working-shift/DailyWorkingShift.css");

                pipeline.AddJavaScriptBundle("Scripts/OddsCompiler",
                    "shared/owp/EventManagement/Resources/OddsCompiler/*.js");

                pipeline.AddJavaScriptBundle("Styles/OddsCompiler",
                    "shared/owp/EventManagement/Resources/OddsCompiler/OddsCompiler.css");

                pipeline.AddJavaScriptBundle("/css/re-checkin.js", "nap-auth/re-checkin.js");
            }
        );
        return services;
    }

}