namespace OWP.Websites.App_Start;

using AutoAssignment;
using AutoAssignment.Services.InternalApi;
using AutoAssignment.Services.PublicApi;
using AutoAssignment.Services.SaveResponse;

public static class AutoAssignmentInjector
{
    public static void AddAutoAssignmentService(this IServiceCollection services)
    {
        services.AddScoped<ExtraTimeDetectorService, ExtraTimeDetectorServiceImp>();
        services.AddScoped<GetRequestService, GetRequestServiceImp>();
        services.AddScoped<ProviderDetectorService, ProviderDetectorServiceImp>();
        services.AddScoped<RealMatchService, RealMatchServiceImp>();
        services.AddScoped<RequestBuilder, RequestBuilderImp>();
        services.AddScoped<SaveRequestService, SaveRequestServiceImp>();
        services.AddScoped<SchedulingMatchBuilderService, SchedulingMatchBuilderServiceImp>();
        services.AddScoped<SabaMatchService, SabaMatchServiceImp>();
        services.AddScoped<LAUserService, LAUserServiceImp>();
        services.AddScoped<TraderUserService, TraderUserServiceImp>();
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<CreateRequestService, CreateRequestServiceImp>();
        services.AddScoped<LABuilderService, LABuilderServiceImp>();
        services.AddScoped<TraderBuilderService, TraderBuilderServiceImp>();
        services.AddScoped<ExcelPackageCreatorService, ExcelPackageCreatorServiceImp>();
        services.AddScoped<ExportService, ExportServiceImp>();
        services.AddScoped<MatchInfoBuilderService, MatchInfoBuilderServiceImp>();
        services.AddScoped<ProviderBuilderService, ProviderBuilderServiceImp>();
        services.AddScoped<LeagueSettingBuilderService, LeagueSettingBuilderServiceImp>();
    }
}