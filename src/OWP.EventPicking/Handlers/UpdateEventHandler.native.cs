
namespace OWP.EventPicking.Handlers;

public class UpdateEventsRequest : IRequest<Unit>
{
    public IList<UpdateEvents> Events { get; set; }
}

public class UpdateMaxbetHandler : IRequestHandler<UpdateEventsRequest, Unit>
{
    private readonly Repository repository;

    private readonly INapSession session;

    public UpdateMaxbetHandler(Repository repository, INapSession session)
    {
        this.repository = repository;
        this.session = session;
    }

    public async Task<Unit> Handle(UpdateEventsRequest request, CancellationToken cancellationToken)
    {
        var criteria = new UpdateMaxbetSpreadCriteria
        {
            InfoSiteMatchId = request.InfoSiteMatchId,
            MaxbetHdp = request.MaxbetHdp,
            NoOfHDP = request.NoOfHDP,
            SpreadHdp = request.SpreadHdp,
            SpreadOu = request.SpreadOu,
            ActorId = session.UserId
        };
         await repository.UpdateEvent(criteria);

         return Unit.Value;
    }

}