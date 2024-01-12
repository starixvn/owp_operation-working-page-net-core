using OWP.Event.EventShared;
using OWP.Event.EventShared.Services;
using OWP.Event.LiveGameAssignment;
using OWP.Event.LiveGameAssignment.Services;
using OWP.Event.LiveGameList;
using OWP.Event.LiveGameList.Services;
using OWP.Event.TaskList;
using OWP.Event.TaskList.Services;

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
    }
}