using OWP.TaskList;
using OWP.TaskList.Services;

namespace OWP.Websites;

public static class TaskListInjector
{
    public static void AddTaskListService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<GetEventByDayService, GetEventByDayServiceImp>();
        services.AddScoped<GetAdminSiteMatchIdService, GetAdminSiteMatchIdServiceImp>();
    }
}

