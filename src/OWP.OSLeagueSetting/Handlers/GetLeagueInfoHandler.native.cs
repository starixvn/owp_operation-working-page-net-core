namespace OWP.OSLeagueSetting.Handlers;

using MediatR;
using Models;
using Services;

public class GetLeagueInfoHandler
{
    public class Request : IRequest<IEnumerable<LeagueSettingModel>>
    {
        public string League { get; set; }

        public string StatusId { get; set; }

        public string CategoryId { get; set; }

        public string RoundId { get; set; }
    }

    public class Handler : IRequestHandler<Request, IEnumerable<LeagueSettingModel>>
    {
        private readonly GetLeagueSettingService getLeagueSettingService;

        public Handler(GetLeagueSettingService getLeagueSettingService) =>
            this.getLeagueSettingService = getLeagueSettingService;

        public async Task<IEnumerable<LeagueSettingModel>> Handle(Request request, CancellationToken cancellationToken)
            => await getLeagueSettingService.GetLeagueSetting(request);
    }
}