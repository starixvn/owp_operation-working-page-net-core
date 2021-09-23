using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.ViewModels;
using OWP.Utils;

namespace OWP.TicketCenter.Handlers
{
    public class GetTicketEventOptionRequest : IRequest<IList<EventOptionInfoSender>>
    {
        public string Keyword { get; set; }
    }

    public class GetTicketEventOptionHandler : IRequestHandler<GetTicketEventOptionRequest, IList<EventOptionInfoSender>>
    {
        private readonly Repository repository;

        public GetTicketEventOptionHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<EventOptionInfoSender>> Handle(GetTicketEventOptionRequest eventOptionRequest, CancellationToken cancellationToken)
        {
            var eventInfo = await repository.GetEventOptionInfo((eventOptionRequest.Keyword));
            var eventInfoDic = eventInfo
                .GroupBy(e => new { e.LeagueName })
                .ToDictionary(x => x.Key, x => x.ToList());

            var eventInfoSender = new List<EventOptionInfoSender>();
            foreach (var key in eventInfoDic.Keys)
            {
                var userViewModel = eventInfoDic[key]
                    .Select(x => new UserViewModel
                    {
                        UserId = x.UserId,
                        Username = x.Username
                    })?.DistinctBy(x=>new {x.UserId, x.Username});

                var leagueViewModel = eventInfoDic[key]
                    .Select(i => new EventOptionInfoViewModel
                    {
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
                        l.OwpLeagueId,
                        l.LeagueName,
                        l.HomeId,
                        l.HomeName,
                        l.AwayId,
                        l.AwayName,
                        l.EventDateTimeGmt8,
                        l.IsLeague,
                    });

                eventInfoSender.Add(new EventOptionInfoSender
                {
                    EventInfoViewModel = leagueViewModel.ToList(),
                    UserViewModel = userViewModel.ToList()
                });
            }

            return eventInfoSender;
        }
    }
}

