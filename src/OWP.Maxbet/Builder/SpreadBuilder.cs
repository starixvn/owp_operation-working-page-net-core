using OWP.Enums;
using OWP.Maxbet.Helpers;
using OWP.Maxbet.Models;
using OWP.Services;

namespace OWP.Maxbet.Builder
{
  public interface SpreadBuilder
  {
    Task<MaxbetSpread> BuildSpreadMainMatch(int owpMatchId);

    Task<MaxbetSpread> BuildSpreadSpecialProduct(int specialProductId);
  }

  public class SpreadBuilderImp : SpreadBuilder
  {
    private const decimal spreadBaseline = (decimal)0.1;

    private const int leagueGroupMJ = 1;

    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly LogServiceWrapper logServiceWrapper;

    public SpreadBuilderImp(MaxbetRepository syncMaxbetRepository, LogServiceWrapper logServiceWrapper)
    {
      this.syncMaxbetRepository = syncMaxbetRepository;
      this.logServiceWrapper = logServiceWrapper;
    }

    public async Task<MaxbetSpread> BuildSpreadSpecialProduct(int specialProductId)
    {
      var (spreads, leagueGroups) = await syncMaxbetRepository.GetSpreadSpecialProductById(specialProductId);
      await logServiceWrapper.Log("======BuildSpreadSpecialProduct====", spreads, leagueGroups);
      var leagueGroupId = leagueGroups.FirstOrDefault()?.LeagueGroupId;
      return BuildSpread(spreads, leagueGroupId);
    }

    public async Task<MaxbetSpread> BuildSpreadMainMatch(int owpMatchId)
    {
      var (spreads, leagueGroups) = await syncMaxbetRepository.GetSpreadByOwpmatchId(owpMatchId);
      var leagueGroupId = leagueGroups.FirstOrDefault()?.LeagueGroupId;
      return BuildSpread(spreads, leagueGroupId);
    }

    public static MaxbetSpread BuildSpread(IEnumerable<SpreadModel> spreads, int? leagueGroupId)
    {
      spreads = spreads.Where(s => s.Spread != null);

      var spreadList = spreads.ToList();

      return leagueGroupId == leagueGroupMJ
          ? BuildSpreadSenderForLeagueGroupMJ(spreadList)
          : BuildSpreadSenderForLeagueGroupNotMJ(spreadList);
    }

    public static MaxbetSpread BuildSpreadSenderForLeagueGroupNotMJ(List<SpreadModel> spread)
    {
      var hasExistsFirstHalfHdp = spread.Any(s => s.Bettype == (int)EnumBettype.FirstHalfHandicap);
      var hasExistsFirstHalfOu = spread.Any(s => s.Bettype == (int)EnumBettype.FirstHalfOverUnder);

      var handicap = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.Handicap) ?? new SpreadModel();
      var overUnder = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.OverUnder) ?? new SpreadModel();

      if (!hasExistsFirstHalfHdp)
      {
        spread.Add(handicap?.Spread < spreadBaseline ?
            new SpreadModel
            {
              Bettype = (int)EnumBettype.FirstHalfHandicap,
              Spread = spreadBaseline
            } :
            new SpreadModel
            {
              Bettype = (int)EnumBettype.FirstHalfHandicap,
              Spread = handicap.Spread
            }
        );
      }

      if (!hasExistsFirstHalfOu)
      {
        spread.Add(overUnder?.Spread < spreadBaseline
            ? new SpreadModel
            {
              Bettype = (int)EnumBettype.FirstHalfOverUnder,
              Spread = spreadBaseline
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
      var hasExistsFirstHalfHdp = spread.Any(s => s.Bettype == (int)EnumBettype.FirstHalfHandicap);
      var hasExistsFirstHalfOu = spread.Any(s => s.Bettype == (int)EnumBettype.FirstHalfOverUnder);

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
      var overunder = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.OverUnder) ?? new SpreadModel();
      var firstHalfHandicap = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.FirstHalfHandicap) ?? new SpreadModel();
      var firstHalfOverUnder = MaxbetHelper.FindSpreadByBettype(spread, (int)EnumBettype.FirstHalfOverUnder) ?? new SpreadModel();
      return new MaxbetSpread
      {
        Handicap = handicap.Spread,
        OverUnder = overunder.Spread,
        FirstHalfHandicap = firstHalfHandicap.Spread,
        FirstHalfOverUnder = firstHalfOverUnder.Spread
      };
    }
  }
}