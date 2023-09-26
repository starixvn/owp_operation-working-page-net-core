using MediatR;
using OWP.Kpi.LA.Models;
using OWP.Kpi.LA.Services;

namespace OWP.Kpi.LA.Handlers;

public class UpdateTargetLevelRequest : IRequest<Unit>
{
    public List<TargetLevel> TargetLevel { get; set; }
}

public class UpdateTargetLevelHandler : IRequestHandler<UpdateTargetLevelRequest, Unit>
{
    private readonly Repository repository;

    public UpdateTargetLevelHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<Unit> Handle(UpdateTargetLevelRequest request, CancellationToken cancellationToken)
    {
        await repository.UpdateTargetLevel(request.TargetLevel);

        return Unit.Value;
    }
}
