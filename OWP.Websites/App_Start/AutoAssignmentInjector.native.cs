using Microsoft.Extensions.DependencyInjection;
using OWP.AutoAssignment;
using OWP.AutoAssignment.Services.PublicApi;
using OWP.AutoAssignment.Services.SaveResponse;

namespace OWP.Websites.App_Start
{
    using AutoAssignment.Services.InternalApi;

    public static class AutoAssignmentInjector
    {
        public static void InjectAutoAssignmentService(this IServiceCollection services)
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
            services.AddScoped<ExportExcelService, ExportExcelServiceImp>();
        }
    }
}
