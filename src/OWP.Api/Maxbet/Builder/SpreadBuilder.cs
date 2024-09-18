using OWP.Api.Maxbet.Helpers;
using OWP.Api.Maxbet.Models;
using OWP.Enums;

namespace OWP.Api.Maxbet.Builder;

public interface SpreadBuilder
{
    Task<MaxbetSpread> BuildSpreadMainMatch(int owpMatchId);

    Task<MaxbetSpread> BuildSpreadSpecialProduct(int specialProductId);
}

public class SpreadBuilderImp(MaxbetRepository syncMaxbetRepository) : SpreadBuilder
{
    private const decimal SPREAD_BASE_COMPARER = (decimal)0.1;

    private const int LEAGUE_GROUP_MJ = 1;

    public async Task<MaxbetSpread> BuildSpreadSpecialProduct(int specialProductId)
    {
        var (spreads, leagueGroups) = await syncMaxbetRepository.GetSpreadSpecialProductById(specialProductId);
        var leagueGroupId = leagueGroups.FirstOrDefault()?.LeagueGroupId;
        return BuildSpread(spreads, leagueGroupId);
    }

    public async Task<MaxbetSpread> BuildSpreadMainMatch(int owpMatchId)
    {
        var (spreads, leagueGroups) = await syncMaxbetRepository.GetSpreadByOwpMatchId(owpMatchId);
        var leagueGroupId = leagueGroups.FirstOrDefault()?.LeagueGroupId;
        return BuildSpread(spreads, leagueGroupId);
    }

    public static MaxbetSpread BuildSpread(IEnumerable<SpreadModel> spreads, int? leagueGroupId)
    {
        spreads = spreads.Where(s => s.Spread != null);

        var spreadList = spreads.ToList();

        return leagueGroupId == LEAGUE_GROUP_MJ
            ? BuildSpreadSenderForLeagueGroupMJ(spreadList)
            : BuildSpreadSenderForLeagueGroupNotMJ(spreadList);
    }

    public static MaxbetSpread BuildSpreadSenderForLeagueGroupNotMJ(List<SpreadModel> spread)
    {
        var hasExistsFirstHalfHdp = spread.Exists(s => s.Bettype == (int)EnumBettype.FirstHalfHandicap);
        var hasExistsFirstHalfOu = spread.Exists(s => s.Bettype == (int)EnumBettype.FirstHalfOverUnder);

        var handicap = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.Handicap) ?? new SpreadModel();
        var overUnder = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.OverUnder) ?? new SpreadModel();

        if (!hasExistsFirstHalfHdp)
        {
            decimal? spread1H = null;
            decimal? pivot = (decimal)0.09;

            if (handicap.Spread > pivot)
            {
                spread1H = handicap.Spread;
            }

            if (handicap.Spread < pivot)
            {
                spread1H = (decimal)0.1;
            }

            if (handicap.Spread.Equals(pivot))
            {
                spread1H = (decimal)0.11;
            }
            spread.Add(
                new SpreadModel
                {
                    Bettype = (int)EnumBettype.FirstHalfHandicap,
                    Spread = spread1H
                }
            );
        }

        if (!hasExistsFirstHalfOu)
        {
            spread.Add(overUnder.Spread < SPREAD_BASE_COMPARER
                ? new SpreadModel
                {
                    Bettype = (int)EnumBettype.FirstHalfOverUnder,
                    Spread = SPREAD_BASE_COMPARER
                } :
                new SpreadModel
                {
                    Bettype = (int)EnumBettype.FirstHalfOverUnder,
                    Spread = overUnder.Spread
                });
        }

        return BuildSpreadSender(spread);
    }

    public static MaxbetSpread BuildSpreadSenderForLeagueGroupMJ(List<SpreadModel> spread)
    {
        var hasExistsFirstHalfHdp = spread.Exists(s => s.Bettype == (int)EnumBettype.FirstHalfHandicap);
        var hasExistsFirstHalfOu = spread.Exists(s => s.Bettype == (int)EnumBettype.FirstHalfOverUnder);

        var handicap = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.Handicap) ?? new SpreadModel();
        var overUnder = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.OverUnder) ?? new SpreadModel();
        if (!hasExistsFirstHalfHdp)
        {
            spread.Add(new SpreadModel
            {
                Bettype = (int)EnumBettype.FirstHalfHandicap,
                Spread = handicap.Spread
            });
        }
        if (!hasExistsFirstHalfOu)
        {
            spread.Add(new SpreadModel
            {
                Bettype = (int)EnumBettype.FirstHalfOverUnder,
                Spread = overUnder.Spread
            });
        }
        return BuildSpreadSender(spread);
    }

    public static MaxbetSpread BuildSpreadSender(List<SpreadModel> spread)
    {
        var handicap = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.Handicap) ?? new SpreadModel();
        var overUnder = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.OverUnder) ?? new SpreadModel();
        var firstHalfHandicap = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.FirstHalfHandicap) ?? new SpreadModel();
        var firstHalfOverUnder = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.FirstHalfOverUnder) ?? new SpreadModel();
        return new MaxbetSpread
        {
            Handicap = handicap.Spread,
            OverUnder = overUnder.Spread,
            FirstHalfHandicap = firstHalfHandicap.Spread,
            FirstHalfOverUnder = firstHalfOverUnder.Spread
        };
    }
}