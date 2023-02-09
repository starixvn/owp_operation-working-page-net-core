using MediatR;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers;

public class GetTicketEventOptionLGARequest : IRequest<IList<EventLGAOptionInfoSender>>
{
    public string Keyword { get; set; }
}

public class GetEventOptionLGAHandler : IRequestHandler<GetTicketEventOptionLGARequest, IList<EventLGAOptionInfoSender>>
{
    private readonly Repository repository;

    public GetEventOptionLGAHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<EventLGAOptionInfoSender>> Handle(GetTicketEventOptionLGARequest request, CancellationToken cancellationToken)
    {
        var eventInfo = await repository.GetEventOptionLGAInfo((request.Keyword));
        var eventInfoDic = eventInfo
            .GroupBy(e => new { e.LeagueName })
            .ToDictionary(x => x.Key, x => x.ToList());

        var eventInfoSender = new List<EventLGAOptionInfoSender>();
        foreach (var key in eventInfoDic.Keys)
        {
            var eventViewModel = eventInfoDic[key]
                .Select(i => new EventLGAOptionInfoViewModel
                {
                    OwpMatchId = i.OwpMatchId,
                    OwpLeagueId = i.OwpLeagueId,
                    LeagueName = key.LeagueName,
                    HomeName = i.HomeName,
                    HomeId = i.HomeId,
                    AwayName = i.AwayName,
                    AwayId = i.AwayId,
                    EventDateTimeGmt8 = i.EventDateTimeGmt8,
                    IsLeague = i.IsLeague,
                })
                .DistinctBy(l => new
                {
                    l.OwpMatchId,
                    l.OwpLeagueId,
                    l.LeagueName,
                    l.HomeId,
                    l.HomeName,
                    l.AwayId,
                    l.AwayName,
                    l.EventDateTimeGmt8,
                    l.IsLeague,
                });

            eventInfoSender.Add(new EventLGAOptionInfoSender
            {
                EventInfoViewModel = eventViewModel.ToList(),
                UserViewModel = new List<UserViewModel>()
            });
        }

        return eventInfoSender;
    }
}