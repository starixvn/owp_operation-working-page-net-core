using MediatR;
using OWP.TaskList.Models;
using OWP.TaskList.Services;

namespace OWP.TaskList.Handlers;

public class TaskListTableRequest : IRequest<TaskListModel>
{
    public DateTime EventDateTime { get; set; }
}
public class TaskListTableHandler : IRequestHandler<TaskListTableRequest, TaskListModel>
{
    private readonly GetEventByDayService getEventByDayService;

    public TaskListTableHandler(GetEventByDayService getEventByDayService)
    {
        this.getEventByDayService = getEventByDayService;
    }

    public async Task<TaskListModel> Handle(TaskListTableRequest request, CancellationToken cancellationToken)
    => await getEventByDayService.GetEventByDay(request.EventDateTime);
}

