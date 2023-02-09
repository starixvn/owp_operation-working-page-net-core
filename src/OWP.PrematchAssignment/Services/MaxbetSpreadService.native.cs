namespace OWP.PreMatchAssignment.Services;

using Criteria;
using Extensions;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using Models;
using OWP.Constants;
using PrematchAssignment.Models;

public interface MaxbetSpreadService
{
    Task Build(IEnumerable<InfoSiteMatch> infoSiteMatches);
}

public class MaxbetSpreadServiceImp : MaxbetSpreadService
{
    private const int FLAG_CAN_FILL_MAXBET_SPREAD = 1;

    private readonly IDynamicRepository dynamicRepo;

    public MaxbetSpreadServiceImp(INapSession napSession, IDynamicRepository dynamicRepo)
    {
        this.dynamicRepo = dynamicRepo;
    }

    public async Task Build(IEnumerable<InfoSiteMatch> infoSiteMatches)
    {
        var adminSiteMatchIds = infoSiteMatches.Select(info => info.AdminSiteMatchId);
        var maxbetSpreads = await GetMaxbetSpreads(adminSiteMatchIds.ToList());
        BuildMaxbetSpread(infoSiteMatches, maxbetSpreads);
    }

    private async Task<IList<MaxbetSpread>> GetMaxbetSpreads(IList<int> adminSiteMatchIds)
    => (await dynamicRepo.FetchAsync<MaxbetSpread>(new GetMaxbetSpreadCriteria
    {
        AdminSiteMatchId = string.Join(",", adminSiteMatchIds)
    })).ToList();

    private void BuildMaxbetSpread(IEnumerable<InfoSiteMatch> infoSiteMatches, IList<MaxbetSpread> maxbetSpreads)
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

    private void BuildMaxbetSpreadHdp(IList<MaxbetSpread> maxbetSpreadValue, InfoSiteMatch infoSiteMatch)
    {
        var count = maxbetSpreadValue.Count(x => x.Bettype == Bettype.Handicap);
        if (count == FLAG_CAN_FILL_MAXBET_SPREAD)
        {
            var maxbetHdp = maxbetSpreadValue.First(x => x.Bettype == Bettype.Handicap);
            infoSiteMatch.MaxBetHdp.Maxbet = maxbetHdp?.Maxbet;
            infoSiteMatch.MaxBetHdp.Spread = maxbetHdp?.Spread;
        }
    }

    private void BuildMaxbetSpreadOu(IList<MaxbetSpread> maxbetSpreadValue, InfoSiteMatch infoSiteMatch)
    {
        var count = maxbetSpreadValue.Count(x => x.Bettype == Bettype.OverUnder);
        if (count == FLAG_CAN_FILL_MAXBET_SPREAD)
        {
            var maxbetOu = maxbetSpreadValue.First(x => x.Bettype == Bettype.OverUnder);
            infoSiteMatch.MaxBetOu.Maxbet = maxbetOu?.Maxbet;
            infoSiteMatch.MaxBetOu.Spread = maxbetOu?.Spread;
        }
    }
}