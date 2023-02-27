using OWP.AutoAssignment.Constant;
using OWP.AutoAssignment.Models.PublicApi;

namespace OWP.AutoAssignment.Services.PublicApi;

public interface SabaMatchService
{
    Task<IList<SabaMatchSender>> BuildSabaMatchInfo(IList<MatchQueried> matchesQueried);
}

public class SabaMatchServiceImp : SabaMatchService
{
    private readonly Repository repository;

    public SabaMatchServiceImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<SabaMatchSender>> BuildSabaMatchInfo(IList<MatchQueried> matchesQueried)
    {
        var matchIds = matchesQueried.Select(i => i.InfositeMatchId).ToList();
        var matchInfos = await GetSabaMatchInfo(matchIds);
        var maxBetInfos = (await GetSabaMaxbet(matchIds)).ToList();

        return matchInfos.Select(i => new SabaMatchSender
        {
            MatchId = i.InfositeId,
            KOTime = i.KOTime.ToString(AutoConstant.FormatDateTime),
            SabaType = GetSabaType(matchesQueried, i.InfositeId),
            LeagueSettingId = GetMaxBetValue(maxBetInfos, i.InfositeId)?.LeagueSettingId,
            MaxBet = GetMaxBetValue(maxBetInfos, i.InfositeId)?.MaxBetFTHDP,
            AwayId = i.AwayId,
            HomeId = i.HomeId
        }).ToList();
    }

    private static int? GetSabaType(IList<MatchQueried> matchesQueried, int infositeId)
        => matchesQueried
            .FirstOrDefault(i => i.InfositeMatchId == infositeId)?
            .SabaType;

    private MaxBetOfFTHDP GetMaxBetValue(List<MaxBetOfFTHDP> maxbetInfos, int infositeId)
        => maxbetInfos
            .FirstOrDefault(m => m.InfoSiteMatchId == infositeId);

    public async Task<IEnumerable<MaxBetOfFTHDP>> GetSabaMaxbet(IList<int> matchIds)
        => await repository.GetMaxMaxbetFTHDP(matchIds);

    public async Task<IEnumerable<SabaMatchCommonInfo>> GetSabaMatchInfo(IList<int> matchIds)
        => await repository.GetSabaMatchCommonInfoByIds(matchIds);
}