using Fanex.Data.Repository;
using OWP.Constants;
using OWP.Extensions;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.Models;

namespace OWP.PrematchAssignment.Services;

public interface MaxbetSpreadService
{
    Task Build(IEnumerable<InfoSiteMatch> infoSiteMatches);
}

public class MaxbetSpreadServiceImp : MaxbetSpreadService
{
    private const int FLAG_CAN_FILL_MAXBET_SPREAD = 1;

    private readonly IDynamicRepository dynamicRepo;

    public MaxbetSpreadServiceImp(IDynamicRepository dynamicRepo)
    {
        this.dynamicRepo = dynamicRepo;
    }

    public async Task Build(IEnumerable<InfoSiteMatch> infoSiteMatches)
    {
        var infoSiteMatchList = infoSiteMatches.ToList();
        var adminSiteMatchIds = infoSiteMatchList.Select(info => info.AdminSiteMatchId);
        var maxbetSpreads = await GetMaxbetSpreads(adminSiteMatchIds.ToList());
        BuildMaxbetSpread(infoSiteMatchList, maxbetSpreads);
    }

    private async Task<IList<MaxbetSpread>> GetMaxbetSpreads(IEnumerable<int> adminSiteMatchIds)
        => (await dynamicRepo.FetchAsync<MaxbetSpread>(new GetMaxbetSpreadCriteria
        {
            AdminSiteMatchId = string.Join(",", adminSiteMatchIds)
        })).ToList();

    private static void BuildMaxbetSpread(IEnumerable<InfoSiteMatch> infoSiteMatches, IEnumerable<MaxbetSpread> maxbetSpreads)
    {
        var maxbetSpreadPair = maxbetSpreads
        .GroupBy(s => s.AdminSiteMatchId)
        .ToDictionary(s => s.Key, s => s.ToList());

        foreach (var infoSiteMatch in infoSiteMatches)
        {
            var maxbetSpreadValue = maxbetSpreadPair.GetValue(
                infoSiteMatch.AdminSiteMatchId,
                new List<MaxbetSpread>());

            BuildMaxbetSpreadHdp(maxbetSpreadValue, infoSiteMatch);
            BuildMaxbetSpreadOu(maxbetSpreadValue, infoSiteMatch);
        }
    }

    private static void BuildMaxbetSpreadHdp(IList<MaxbetSpread> maxbetSpreadValue, InfoSiteMatch infoSiteMatch)
    {
        var count = maxbetSpreadValue.Count(x => x.Bettype == Bettype.Handicap);
        if (count != FLAG_CAN_FILL_MAXBET_SPREAD)
        {
            return;
        }

        var maxbetHdp = maxbetSpreadValue.First(x => x.Bettype == Bettype.Handicap);
        infoSiteMatch.MaxBetHdp.Maxbet = maxbetHdp.Maxbet;
        infoSiteMatch.MaxBetHdp.Spread = maxbetHdp.Spread;
    }

    private static void BuildMaxbetSpreadOu(IList<MaxbetSpread> maxbetSpreadValue, InfoSiteMatch infoSiteMatch)
    {
        var count = maxbetSpreadValue.Count(x => x.Bettype == Bettype.OverUnder);
        if (count != FLAG_CAN_FILL_MAXBET_SPREAD)
        {
            return;
        }

        var maxbetOu = maxbetSpreadValue.First(x => x.Bettype == Bettype.OverUnder);
        infoSiteMatch.MaxBetOu.Maxbet = maxbetOu.Maxbet;
        infoSiteMatch.MaxBetOu.Spread = maxbetOu.Spread;
    }
}