using MediatR;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;

namespace OWP.PrematchAssignment.Handlers;

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