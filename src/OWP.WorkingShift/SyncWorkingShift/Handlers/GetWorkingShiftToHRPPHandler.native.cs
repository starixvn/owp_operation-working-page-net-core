using MediatR;

namespace OWP.WorkingShift.SyncWorkingShift.Handlers;

public class QueryWorkingShiftToHRPPRequest : IRequest<IEnumerable<WorkingShiftHRPPModel>>
{
    public DateTime From { get; set; }

    public DateTime To { get; set; }
}

public class QueryWorkingShiftToHRPPHandler : IRequestHandler<QueryWorkingShiftToHRPPRequest, IEnumerable<WorkingShiftHRPPModel>>
{
    private readonly WorkingShiftRepository commandRepository;

    public QueryWorkingShiftToHRPPHandler(WorkingShiftRepository commandRepository)
    {
        this.commandRepository = commandRepository;
    }

    public async Task<IEnumerable<WorkingShiftHRPPModel>> Handle(QueryWorkingShiftToHRPPRequest request, CancellationToken cancellationToken)
        => await commandRepository.QueryWorkingShiftHRPP(request);
}