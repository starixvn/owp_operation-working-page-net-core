using MediatR;
using OWP.TaskList.Models;

namespace OWP.TaskList.Handlers
{
    using Services;

    public class TaskListTableRequest : IRequest<TaskListModel>
    {
        public DateTime EventDateTime { get; set; }
        public bool IsHideCompletedMatches { get; set; }
    }
    public class TaskListTableHandler : IRequestHandler<TaskListTableRequest, TaskListModel>
    {
        private readonly GetEventByDayService getEventByDayService;

        public TaskListTableHandler(GetEventByDayService getEventByDayService)
        {
            this.getEventByDayService = getEventByDayService;
        }

        public async Task<TaskListModel> Handle(TaskListTableRequest request, CancellationToken cancellationToken)
        {
            return await getEventByDayService.GetEventByDay(request.EventDateTime, request.IsHideCompletedMatches);
        }
    }
}
