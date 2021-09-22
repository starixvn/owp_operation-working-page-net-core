using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;
using OWP.Utils;

namespace OWP.TicketCenter.Handlers
{
    public class NewTicketRequest : IRequest<IList<EventInfoSender>>
    {
        public string Keyword { get; set; }
    }

    public class NewTicketHandler : IRequestHandler<NewTicketRequest, IList<EventInfoSender>>
    {
        private readonly Repository repository;

        public NewTicketHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<EventInfoSender>> Handle(NewTicketRequest request, CancellationToken cancellationToken)
        {
            var eventInfo = await repository.GetEventInfo((request.Keyword));
            var eventInfoDic = eventInfo
                .GroupBy(e => new { e.LeagueName })
                .ToDictionary(x => x.Key, x => x.ToList());

            var eventInfoSender = new List<EventInfoSender>();
            foreach (var key in eventInfoDic.Keys)
            {
                var userViewModel = eventInfoDic[key]
                    .Select(x => new UserViewModel
                    {
                        UserId = x.UserId,
                        Username = x.Username
                    })?.DistinctBy(x=>new {x.UserId, x.Username});

                var leagueViewModel = eventInfoDic[key]
                    .Select(i => new EventInfoViewModel
                    {
                        LeagueName = key.LeagueName,
                        EventDateTimeGmt8 = i.EventDateTimeGmt8,
                        IsLeague = i.IsLeague,
                        EventName = i.EventName,
                    })
                    .DistinctBy(l => new
                    {
                        l.LeagueName,
                        l.EventDateTimeGmt8,
                        l.IsLeague,
                        l.EventName
                    });

                eventInfoSender.Add(new EventInfoSender
                {
                    EventInfoViewModel = leagueViewModel.ToList(),
                    UserViewModel = userViewModel.ToList()
                });
            }

            return eventInfoSender;
        }
    }
}

