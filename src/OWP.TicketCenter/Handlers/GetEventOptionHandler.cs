using MediatR;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    public class GetTicketEventOptionRequest : IRequest<IList<EventCheckingOptionInfoSender>>
    {
        public string Keyword { get; set; }
    }

    public class GetEventOptionHandler : IRequestHandler<GetTicketEventOptionRequest, IList<EventCheckingOptionInfoSender>>
    {
        private readonly Repository repository;

        public GetEventOptionHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<EventCheckingOptionInfoSender>> Handle(GetTicketEventOptionRequest request, CancellationToken cancellationToken)
        {
            var eventInfo = await repository.GetEventOptionInfo((request.Keyword));
            var eventInfoDic = eventInfo
                .GroupBy(e => new { e.LeagueName })
                .ToDictionary(x => x.Key, x => x.ToList());

            var eventInfoSender = new List<EventCheckingOptionInfoSender>();
            foreach (var key in eventInfoDic.Keys)
            {
                var userViewModel = eventInfoDic[key]
                    .Where(x => x.HasAssignee)
                    .Select(x => new UserViewModel
                    {
                        RecipientId = x.RecipientId,
                        UserName = x.UserName,
                        NickName = x.NickName
                    })?.DistinctBy(x => new { x.RecipientId, x.UserName, x.NickName });

                var eventViewModel = eventInfoDic[key]
                    .Select(i => new EventCheckingOptionInfoViewModel
                    {
                        InfoSiteMatchId = i.InfoSiteMatchId,
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
                        l.InfoSiteMatchId,
                        l.OwpLeagueId,
                        l.LeagueName,
                        l.HomeId,
                        l.HomeName,
                        l.AwayId,
                        l.AwayName,
                        l.EventDateTimeGmt8,
                        l.IsLeague,
                    });

                eventInfoSender.Add(new EventCheckingOptionInfoSender
                {
                    EventInfoViewModel = eventViewModel.ToList(),
                    UserViewModel = userViewModel.ToList()
                });
            }

            return eventInfoSender;
        }
    }
}