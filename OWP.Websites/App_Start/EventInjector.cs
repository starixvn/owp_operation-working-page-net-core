using OWP.Event.EventShared;
using OWP.Event.EventShared.Services;
using OWP.Event.LiveGameAssignment;
using OWP.Event.LiveGameAssignment.Download.Repositories;
using OWP.Event.LiveGameAssignment.Download.Service.Implement;
using OWP.Event.LiveGameAssignment.EventPicking;
using OWP.Event.LiveGameAssignment.EventPicking.Service;
using OWP.Event.LiveGameAssignment.Services;
using OWP.Event.LiveGameAssignment.User;
using OWP.Event.LiveGameList;
using OWP.Event.LiveGameList.Services;
using OWP.Event.TaskList;
using OWP.Event.TaskList.Services;
using OWP.Services;

namespace OWP.Websites;

public static class EventInjector
{
    public static void AddEventService(this IServiceCollection services)
    {
        services.AddScoped<EventRepository, EventRepositoryImp>();
        services.AddScoped<EventInformationService, EventInformationServiceImp>();
        services.AddScoped<AdminSiteMatchServices, AdminSiteMatchServicesImp>();
        services.AddScoped<InfoSiteTimeServices, InfoSiteTimeServicesImp>();

        // Task List
        services.AddScoped<GetEventByDayService, GetEventByDayServiceImp>();
        services.AddScoped<TaskListRepository, TaskListRepositoryImp>();

        // Live Game List
        services.AddScoped<GetEventByDateService, GetEventByDateServiceImp>();
        services.AddScoped<LiveGameListRepository, LiveGameListRepositoryImp>();

        // Live Game Assignment
        services.AddScoped<SyncOffVRService, SyncOffVRServiceImp>();
        services.AddScoped<EventAssignmentPermission, EventAssignmentPermissionImp>();
        services.AddScoped<LiveGameAssignmentRepository, LiveGameAssignmentRepositoryImp>();
        services.AddScoped<IEventAssignmentService, EventAssignmentService>();
        services.AddScoped<ExcelService, ExcelServiceImp>();
        services.AddScoped<BuildEventInfoService, BuildEventInfoServiceImp>();
        services.AddScoped<EventAssignmentExportFilter, EventAssignmentExportFilterImp>();
        services.AddScoped<ExcelService, ExcelServiceImp>();
        services.AddScoped<LeagueSettingNewFormatService, LeagueSettingNewFormatServiceImp>();
        services.AddScoped<LeagueSettingService, LeagueSettingServiceImp>();
        services.AddScoped<EventPickingService, EventPickingServiceImp>();
        services.AddScoped<AccessCounter, AccessCounterImp>();
        services.AddScoped<LstNoteMapperService, LstNoteMapperServiceImp>();
        services.AddScoped<OrderMatchService, OrderMatchServiceImp>();
        services.AddScoped<IEventAssignmentRepository, EventAssignmentRepository>();
        services.AddScoped<LeagueSettingRepository, LeagueSettingRepositoryImp>();
        services.AddScoped<UserRepository, UserRepositoryImp>();
    }
}