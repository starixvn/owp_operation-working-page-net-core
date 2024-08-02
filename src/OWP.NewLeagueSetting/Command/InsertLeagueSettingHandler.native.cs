using AutoMapper;

namespace OWP.NewLeagueSetting.Command;

public class InsertLeagueSettingRequest : LeagueSettingRequest, Command<int> { }

public class InsertLeagueSettingHandler(Repository repository, INapSession session, IMapper mapper) : CommandHandler<InsertLeagueSettingRequest, int>
{
    public async Task<int> Handle(InsertLeagueSettingRequest request, CancellationToken cancellationToken)
    {
        StandardizedLeagueInfo.BuildMaxbet(request, request.MaxbetWD_HDP, request.MaxbetWK_HDP);
        StandardizedLeagueInfo.BuildNoHdp(request, request.WD_NoOfHDP, request.WK_NoOfHDP);
        StandardizedLeagueInfo.BuildSpread(request, request.Spread);
        var criteria = mapper.Map<InsertLeagueSettingCriteria>(request);
        //await repository.InsertLeagueSetting(criteria, session.UserId);
        return criteria.LeagueSettingId;
    }
}
