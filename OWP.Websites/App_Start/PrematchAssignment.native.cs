using Microsoft.Extensions.DependencyInjection;
using OWP.PreMatchAssignment.Misc;
using OWP.PreMatchAssignment.Services;

namespace OWP.Websites
{
    public static class PreMatchAssignmentInjector
    {
        public static void AddPreMatchAssignmentServices(this IServiceCollection services)
        {
            services.AddScoped<QueryWorkingShift, QueryWorkingShiftImp>();
            services.AddScoped<PreMatchAssignmentService, PreMatchAssignmentServiceImp>();
            services.AddScoped<QueryLogService, QueryLogServiceImp>();
            services.AddScoped<LogFactory, LogFactoryImp>();

            //services.AddScoped<NotificationService, NotificationServiceImp>();
            services.AddScoped<NoteService, NoteServiceImp>();
            services.AddScoped<ShiftAssignmentService, ShiftAssignmentServiceImp>();
        }
    }
}