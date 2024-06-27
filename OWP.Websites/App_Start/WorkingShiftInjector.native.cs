using OWP.WorkingShift.Common.Repositories;
using OWP.WorkingShift.Common.Services;
using OWP.WorkingShift.ViewLog.Services;
using OWP.WorkingShift.WeeklyWorkingShift.Services;
using DailyWorkingShift = OWP.WorkingShift.DailyWorkingShift;
using WeeklyWorkingShift = OWP.WorkingShift.WeeklyWorkingShift;

namespace OWP.Websites;

public static class WorkingShiftInjector
{
    public static void AddWorkingShiftService(this IServiceCollection services)
    {
        services.AddTransient<WorkingShiftColorRepository, RepositoryWorkingShiftColorImp>();
        services.AddTransient<WorkingShiftTimezoneSettingRepository, WorkingShiftTimezoneSettingRepositoryImp>();
        services.AddTransient<WorkingShiftPublishRepository, WorkingShiftPublishRepositoryImp>();
        services.AddTransient<WorkingShiftDefaultRepository, WorkingShiftDefaultRepositoryImp>();
        services.AddTransient<WorkingShiftOrderRepository, WorkingShiftOrderRepositoryImp>();
        services.AddTransient<WorkingShiftRepository, WorkingShiftRepositoryImp>();
        services.AddTransient<ModelService, ModelServiceImp>();
    }

    public static void AddDailyWorkingShiftService(this IServiceCollection services)
    {
        services.AddTransient<DailyWorkingShift.Repositories.Repository, DailyWorkingShift.Repositories.RepositoryImp>();
        services.AddTransient<DailyWorkingShift.Services.ModelService, DailyWorkingShift.Services.ModelServiceImp>();
    }

    public static void AddWeeklyWorkingShiftService(this IServiceCollection services)
    {
        services.AddTransient<WeeklyWorkingShift.Repositories.Repository, WeeklyWorkingShift.Repositories.RepositoryImp>();
        services.AddTransient<ImportService, ImportServiceImp>();
        services.AddTransient<ExportService, ExportServiceImp>();
        services.AddTransient<ViewLogService, ViewLogServiceImp>();
    }

    public static void AddViewLogService(this IServiceCollection services)
    {
        services.AddTransient<LogService, LogServiceImp>();
        services.AddTransient<TemplateService, TemplateServiceImp>();

    }
}
