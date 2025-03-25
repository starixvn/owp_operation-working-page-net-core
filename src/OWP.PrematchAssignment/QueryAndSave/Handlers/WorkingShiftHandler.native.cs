using MediatR;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class QueryWorkingShiftRequest : IRequest<IEnumerable<WorkingShift>>
{
    public DateTime Date { get; set; }
}

public class WorkingShiftHandler : IRequestHandler<QueryWorkingShiftRequest, IEnumerable<WorkingShift>>
{
    private readonly QueryWorkingShift queryWorkingShift;

    public WorkingShiftHandler(QueryWorkingShift queryWorkingShift)
    {
        this.queryWorkingShift = queryWorkingShift;
    }

    public async Task<IEnumerable<WorkingShift>> Handle(QueryWorkingShiftRequest request, CancellationToken cancellationToken)
    {
        var result = await queryWorkingShift.QueryWorkingShift(request.Date);

        return result;
    }
}