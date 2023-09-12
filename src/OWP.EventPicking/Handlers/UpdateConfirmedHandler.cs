using MediatR;
using OWP.EventPicking.Criteria;

namespace OWP.EventPicking.Handlers;

public class UpdateConfirmedRequest : IRequest<Unit>
{
    public IList<string> InfoSiteMatchIds { get; set; }

    public string ModifiedBy { get; set; }

    public bool Confirmed { get; set; }
}

public class UpdateConfirmedHandler : IRequestHandler<UpdateConfirmedRequest, Unit>
{
    private readonly Repository repository;

    public UpdateConfirmedHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<Unit> Handle(UpdateConfirmedRequest request, CancellationToken cancellationToken)
    {
        await repository.UpdateConfirmed(new UpdateConfirmedMatchCriteria
        {
            InfoSiteMatchIds = request.InfoSiteMatchIds,
            ModifiedBy = request.ModifiedBy,
            Confirmed = request.Confirmed
        });
        return Unit.Value;
    }
}