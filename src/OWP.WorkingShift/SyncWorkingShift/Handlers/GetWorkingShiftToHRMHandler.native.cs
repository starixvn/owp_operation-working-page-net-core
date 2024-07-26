using MediatR;
using WorkingShiftModel = OWP.WorkingShift.SyncWorkingShift.Models.WorkingShiftModel;

namespace OWP.WorkingShift.SyncWorkingShift.Handlers;

public class QueryWorkingShiftRequest : IRequest<IEnumerable<WorkingShiftModel>>
{
    public DateTime From { get; set; }

    public DateTime To { get; set; }
}

public class Handler : IRequestHandler<QueryWorkingShiftRequest, IEnumerable<WorkingShiftModel>>
{
    private readonly WorkingShiftRepository commandRepository;

    public Handler(WorkingShiftRepository commandRepository)
    {
        this.commandRepository = commandRepository;
    }

    public async Task<IEnumerable<WorkingShiftModel>> Handle(QueryWorkingShiftRequest request, CancellationToken cancellationToken)
    {
        var workingShiftModels = (await commandRepository.Query(request)).ToList();
        foreach (var shift in workingShiftModels)
        {
            shift.RecalculateShiftCode();
        }
        return workingShiftModels;
    }
}