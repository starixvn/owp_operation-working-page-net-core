using OWP.OSLeagueSetting.Constants;
using OWP.OSLeagueSetting.Factory;
using OWP.OSLeagueSetting.Models;

namespace OWP.OSLeagueSetting.Services;

public interface DuplicateSettingService
{
    Task<bool> IsDuplicate(SettingDetail model);
}

public class DuplicateSettingServiceImp : DuplicateSettingService
{
    private readonly Repository repository;

    private readonly SettingRuleCreator creator;

    public DuplicateSettingServiceImp(Repository repository, SettingRuleCreator creator)
    {
        this.repository = repository;
        this.creator = creator;
    }

    public async Task<bool> IsDuplicate(SettingDetail model)
    {
        var leagueSettings = await repository.GetLeagueSettingByLeagueName(model.LeagueSettingId,model.LeagueName);
        var leagueCombines = CombineLeagueSetting(leagueSettings).ToList();
        if (!leagueCombines.Any())
        {
            return false;
        }
        var roundRule = creator.Create(DuplicateType.Round);
        var offerLineRule = creator.Create(DuplicateType.OfferLine);
        var betDelayRule = creator.Create(DuplicateType.BetDelay);
        var providerRule = creator.Create(DuplicateType.Provider);
        var rangeTimeRule = creator.Create(DuplicateType.RangeTime);
        var ipTvRule = creator.Create(DuplicateType.IPTV);
        var remarkRule = creator.Create(DuplicateType.Remark);
        roundRule.SetNextRule(offerLineRule)
            .SetNextRule(betDelayRule)
            .SetNextRule(providerRule)
            .SetNextRule(rangeTimeRule)
            .SetNextRule(ipTvRule)
            .SetNextRule(remarkRule);

        return roundRule.IsDuplicate(model, leagueCombines);
    }

    private IEnumerable<DuplicateSettingModel> CombineLeagueSetting(IEnumerable<LeagueSettingBase> leagueSettings)
    {
        var leagueDict = leagueSettings.GroupBy(x => x.LeagueSettingId)
            .ToDictionary(g => g.Key, g => g.ToList());
        foreach (var key in leagueDict.Keys)
        {
            var leagueSettingById = leagueDict[key];
            yield return new DuplicateSettingModel
            {
                LeagueSettingId = key,
                Remark = leagueSettingById.First().Remark,
                BetDeLayIds = leagueSettingById.Select(x => x.BetDeLayId).Distinct().ToList(),
                OfferLinesIds = leagueSettingById.Select(x => x.OfferLineId).Distinct().ToList(),
                ProviderIds = leagueSettingById.Select(x => x.ProviderId).Distinct().ToList(),
                RoundIds = leagueSettingById.Select(x => x.RoundId).Distinct().ToList(),
                KOFromTime = leagueSettingById.First().KOFromTime,
                KOToTime = leagueSettingById.First().KOToTime,
                IsIptvOnly = leagueSettingById.First().IsIptvOnly
            };
        }
    }
}