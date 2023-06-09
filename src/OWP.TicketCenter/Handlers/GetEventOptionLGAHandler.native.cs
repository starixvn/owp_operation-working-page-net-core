using MediatR;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers;

public class GetTicketEventOptionLgaRequest : IRequest<IList<EventLgaOptionInfoSender>>
{
    public string Keyword { get; set; }
}

public class GetEventOptionLgaHandler : IRequestHandler<GetTicketEventOptionLgaRequest, IList<EventLgaOptionInfoSender>>
{
    private readonly Repository repository;

    public GetEventOptionLgaHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<EventLgaOptionInfoSender>> Handle(GetTicketEventOptionLgaRequest request,
        CancellationToken cancellationToken)
    {
        var eventInfo = await repository.GetEventOptionLgaInfo(request.Keyword);

        var eventInfoSender = new List<EventLgaOptionInfoSender>
        {
            new()
            {
                EventInfoViewModel = eventInfo.Select(i => new EventLgaOptionInfoViewModel
                    {
                        OwpMatchId = i.OwpMatchId,
                        OwpLeagueId = i.OwpLeagueId,
                        LeagueName = i.LeagueName,
                        HomeName = i.HomeName,
                        HomeId = i.HomeId,
                        AwayName = i.AwayName,
                        AwayId = i.AwayId,
                        EventDateTimeGmt8 = i.EventDateTimeGmt8,
                        IsLeague = i.IsLeague
                    }
                ).OrderBy(x => x.LeagueName).ThenBy(x => x.EventDateTimeGmt8).ToList(),
                UserViewModel = new List<UserViewModel>()
            }
        };

        return eventInfoSender;
    }
}