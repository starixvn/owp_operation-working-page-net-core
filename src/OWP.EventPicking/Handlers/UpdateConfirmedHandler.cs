using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using OWP.EventPicking.Criteria;

namespace OWP.EventPicking.Handlers;

public class UpdateConfirmedRequest : IRequest<Unit>
{
    public IList<string> InfoSiteMatchIds { get; set; }

    public bool Confirmed { get; set; }
}

public class UpdateConfirmedHandler : IRequestHandler<UpdateConfirmedRequest, Unit>
{
    private readonly Repository repository;

    private readonly INapSession session;

    public UpdateConfirmedHandler(Repository repository, INapSession session)
    {
        this.repository = repository;
        this.session = session;
    }

    public async Task<Unit> Handle(UpdateConfirmedRequest request, CancellationToken cancellationToken)
    {
        await repository.UpdateConfirmed(new UpdateConfirmedMatchCriteria
        {
            InfoSiteMatchIds = request.InfoSiteMatchIds,
            ModifiedBy = session.UserName,
            Confirmed = request.Confirmed
        });
        return Unit.Value;
    }
}