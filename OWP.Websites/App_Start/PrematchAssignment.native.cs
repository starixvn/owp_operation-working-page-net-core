using OWP.PrematchAssignment.QueryAndSave.Misc;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.Websites;

public static class PreMatchAssignmentInjector
{
    public static void AddPreMatchAssignmentServices(this IServiceCollection services)
    {
        services.AddScoped<QueryWorkingShift, QueryWorkingShiftImp>();
        services.AddScoped<PreMatchAssignmentService, PreMatchAssignmentServiceImp>();
        services.AddScoped<QueryLogService, QueryLogServiceImp>();
        services.AddScoped<LogFactory, LogFactoryImp>();

        services.AddScoped<NoteService, NoteServiceImp>();
        services.AddScoped<MaxbetSpreadService, MaxbetSpreadServiceImp>();
        services.AddScoped<ShiftAssignmentService, ShiftAssignmentServiceImp>();
        services.AddScoped<Repository, RepositoryImp>();
    }
}
