using MediatR;

namespace OWP.Event.TaskList.Handlers;

public class UpdateReadyTradeStatusRequest : IRequest<string>
{
    public int OWPMatchId { get; set; }
}
public class UpdateReadyTradeStatusHandler : IRequestHandler<UpdateReadyTradeStatusRequest, string>
{
    private readonly TaskListRepository taskListRepository;

    public UpdateReadyTradeStatusHandler(TaskListRepository taskListRepository)
    {
        this.taskListRepository = taskListRepository;
    }

    public async Task<string> Handle(UpdateReadyTradeStatusRequest request, CancellationToken cancellation)
    {
        var date = await taskListRepository.UpdateTradeStatus(request.OWPMatchId);
        return date.ToString("hh:mm tt MMM dd, yyyy");
    }
}