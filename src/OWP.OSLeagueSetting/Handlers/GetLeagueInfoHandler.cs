using Fanex.Data.Repository;
using MediatR;
using OWP.OSLeagueSetting.Models;

namespace OWP.OSLeagueSetting.Handlers;

public class GetLeagueInfoHandler
{
    public class Request : IRequest<object>
    {
        public string League { get; set; }

        public string StatusId { get; set; }

        public string CategoryId { get; set; }

        public string RoundId { get; set; }
    }

    public class Handler : IRequestHandler<Request, object>
    {
        private readonly IDynamicRepository repository;

        public Handler(IDynamicRepository repository) => this.repository = repository;

        public async Task<object> Handle(Request request, CancellationToken cancellationToken)
        {
            var leagueGeneralInfo = new List<LeagueGeneralInfo>
            {
                new LeagueGeneralInfo
                {
                    LeagueId=1,
                    Category="major",
                    LeagueName ="NBA Most 3 Points",
                    Active = true
                }
            };

            var maxbet = new List<MaxbetSpread>(){
                 new MaxbetSpread
            {
                LeagueId = 1,
                Ou = 12000,
                Hdp = 1000
            }};

            var spread = new List<MaxbetSpread>(){new MaxbetSpread
            {
                LeagueId = 1,
                Ou = 0.1m,
                Hdp = 0.3m
            }};

            var round = new List<Round>(){new Round
            {
                LeagueId = 1,
                Label = "Finals"
            }};
            return new
            {
                leagueGeneralInfo,
                maxbet,
                spread,
                round
            };
        }
    }
}