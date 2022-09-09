using OWP.Enums;
using OWP.Maxbet.Helpers;
using OWP.Maxbet.Constants;
using OWP.Maxbet.Models;

namespace OWP.Maxbet.Builder;

public interface MaxbetBuilder
{
    Task<MaxbetSpread> BuildMaxbet(int owpMatchId);

    Task<MaxbetSpread> BuildMaxbetSpecialProduct(MinorMatchInfo specialProduct, int bettypeId);
}

public class MaxbetBuilderImp : MaxbetBuilder
{
    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly Dictionary<int, int> maxbetMapperFirstHalfHDP = new Dictionary<int, int>
            {
                {500_000,120_000 },
                {400_000,100_000 },
                {350_000,100_000 },
                {300_000,50_000 },
                {200_000,50_000 },
                {180_000,40_000 },
                {150_000,30_000 },
                {130_000,30_000 },
                {120_000,30_000 },
                {110_000,30_000 },
                {100_000,30_000 },
                {90_000,30_000 },
                {80_000,25_000 },
                {70_000,25_000 },
                {60_000,20_000 },
                {50_000,20_000 },
                {40_000,18_000 },
                {35_000,15_000 },
                {30_000,12_000 },
                {25_000,10_000 },
                {20_000,8_000 },
                {18_000,8_000 },
                {15_000,6_000 },
                {12_000,5_000 },
                {10_000,4_000 },
                {8_000,3_000 },
                {7_000,2_000 },
                {6_000,2_000 },
                {5_000,2_000 },
                {4_000,2_000 },
                {3_000,2_000 },
                {2_000,2_000 }
            };

    private readonly Dictionary<int, int> maxbetMapperFirstHalfOU = new Dictionary<int, int>
            {
                {500_000,150_000 },
                {400_000,120_000 },
                {350_000,120_000 },
                {300_000,60_000 },
                {200_000,60_000 },
                {180_000,50_000 },
                {150_000,40_000 },
                {130_000,40_000 },
                {120_000,40_000 },
                {110_000,40_000 },
                {100_000,40_000 },
                {90_000,40_000 },
                {80_000,30_000 },
                {70_000,30_000 },
                {60_000,25_000 },
                {50_000,25_000 },
                {40_000,20_000 },
                {35_000,18_000 },
                {30_000,15_000 },
                {25_000,12_000 },
                {20_000,10_000 },
                {18_000,10_000 },
                {15_000,8_000 },
                {12_000,6_000 },
                {10_000,5_000 },
                {8_000,4_000 },
                {7_000,4_000 },
                {6_000,3_000 },
                {5_000,3_000 },
                {4_000,2_000 },
                {3_000,2_000 },
                {2_000,2_000 }
            };

    private readonly Dictionary<int, Tuple<decimal, decimal>> maxbetBookingMapper = new Dictionary<int, Tuple<decimal, decimal>>
        {
            {500_000, new (10_000,5_000)},
            {400_000, new (10_000,5_000)},
            {350_000, new (10_000,5_000)},
            {300_000, new (10_000,5_000)},
            {200_000, new (10_000,5_000)},
            {180_000, new (10_000,5_000)},
            {150_000, new (5_000,3_000)},
            {130_000, new (5_000,3_000)},
            {120_000, new (5_000,3_000)},
            {110_000, new (5_000,3_000)},
            {100_000, new (5_000,3_000)},
            {90_000, new (3_000,2_000)},
            {80_000, new (3_000,2_000)},
            {70_000, new (3_000,2_000)},
            {60_000, new (3_000,2_000)},
            {50_000, new (3_000,2_000)},
            {40_000, new (3_000,2_000)},
            {35_000, new (3_000,2_000)},
            {30_000, new (3_000,2_000)},
            {25_000, new (3_000,2_000)},
            {20_000, new (3_000,2_000)},
            {18_000, new (3_000,2_000)},
            {15_000, new (3_000,2_000)},
            {12_000, new (3_000,2_000)},
            {10_000, new (3_000,2_000)},
            {8_000, new (3_000,2_000)},
            {7_000, new (3_000,2_000)},
            {6_000, new (3_000,2_000)},
            {5_000, new (3_000,2_000)},
            {4_000, new (3_000,2_000)},
            {3_000, new (3_000,2_000)},
        };

    private readonly Dictionary<int, Tuple<int, int, int, int>> maxbetCornerMapper = new Dictionary<int, Tuple<int, int, int, int>>
        {
            {500_000, new (9_000,18_000, 4_000,8_000)},
            {400_000, new (9_000,18_000, 4_000,8_000)},
            {350_000, new (9_000,18_000, 4_000,8_000)},
            {300_000, new (9_000,18_000, 4_000,8_000)},
            {200_000, new (9_000,18_000, 4_000,8_000)},
            {180_000, new (9_000,18_000, 4_000,8_000)},
            {150_000, new (6_000,12_000, 3_000,6_000)},
            {130_000, new (6_000,12_000, 3_000,6_000)},
            {120_000, new (6_000,12_000, 3_000,6_000)},
            {110_000, new (6_000,12_000, 3_000,6_000)},
            {100_000, new (6_000,12_000, 3_000,6_000)},
            {90_000, new (6_000,12_000, 3_000,6_000)},
            {80_000, new (3_500,7_000, 2_000,3_000)},
            {70_000, new (3_500,7_000, 2_000,3_000)},
            {60_000, new (3_500,7_000, 2_000,3_000)},
            {50_000, new (3_500,7_000, 2_000,3_000)},
            {40_000, new (3_500,7_000, 2_000,3_000)},
            {35_000, new (2_500,5_000, 2_000,3_000)},
            {30_000, new (2_500,5_000, 2_000,3_000)},
            {25_000, new (2_500,5_000, 2_000,3_000)},
            {20_000, new (2_500,5_000, 2_000,3_000)},
            {18_000, new (2_500,5_000, 2_000,3_000)},
            {15_000, new (2_500,5_000, 2_000,3_000)},
            {12_000, new (2_500,5_000, 2_000,3_000)},
            {10_000, new (2_000,4_000,2_000,2_000)},
            {8_000, new (2_000,4_000,2_000,2_000)},
            {7_000, new (2_000,4_000,2_000,2_000)},
            {6_000, new (2_000,4_000,2_000,2_000)},
            {5_000, new (2_000,3_000,2_000,2_000)},
            {4_000, new (2_000,3_000,2_000,2_000)},
            {3_000, new (2_000,2_000,2_000,2_000)},
            {2_000, new (2_000,2_000,2_000,2_000)},
        };

    public MaxbetBuilderImp(MaxbetRepository syncMaxbetRepository)
    {
        this.syncMaxbetRepository = syncMaxbetRepository;
    }

    public async Task<MaxbetSpread> BuildMaxbet(int owpMatchId)
    {
        var maxbet = (await syncMaxbetRepository.GetMaxbetByOwpMatchId(owpMatchId)).ToList();

        if (!maxbet.Any())
        {
            return new MaxbetSpread();
        }

        if (maxbet.Count == SpecialBettype.NUMBER_FULL_BETTYPE)
        {
            return BuildMaxbetSender(maxbet);
        }

        var hasExistsOu = maxbet.Any(s => s.BettypeId == (int)EnumBettype.OverUnder);
        if (!hasExistsOu)
        {
            BuildMaxbetOverUnder(maxbet);
        }

        var hasExistsFirstHalfHdp = maxbet.Any(s => s.BettypeId == (int)EnumBettype.FirstHalfHandicap);
        if (!hasExistsFirstHalfHdp)
        {
            BuildMaxbetFirstHalfHandicap(maxbet);
        }

        var hasExistsFirstHalfOu = maxbet.Any(s => s.BettypeId == (int)EnumBettype.FirstHalfOverUnder);
        if (!hasExistsFirstHalfOu)
        {
            BuildMaxbetFirstHalfOverUnder(maxbet);
        }

        return BuildMaxbetSender(maxbet);
    }

    public async Task<MaxbetSpread> BuildMaxbetSpecialProduct(MinorMatchInfo specialProduct, int bettypeId)
    {
        var maxbet = (await syncMaxbetRepository.GetMaxbetByOwpMatchId(specialProduct.MinorMatchId)).ToList();
        var maxbetMainMatch = (await syncMaxbetRepository.GetMaxbetByOwpMatchId(specialProduct.ParentMatchId)).ToList();

        switch (bettypeId)
        {
            case SpecialBettype.BETTYPE_ID_NO_OF_CORNERS:
                return BuildMaxbetNoOfCorner(maxbet, maxbetMainMatch);

            case SpecialBettype.BETTYPE_ID_NEXT_CORNERS:
                return BuildMaxbetNextCorner(maxbet, maxbetMainMatch);

            case SpecialBettype.BETTYPE_ID_NEXT_BOOKINGS:
                return BuildMaxbetNextBooking(maxbet, maxbetMainMatch);

            case SpecialBettype.BETTYPE_ID_TOTAL_BOOKINGS:
                return BuildMaxbetTotalBooking(maxbet, maxbetMainMatch);

            default:
                return BuildMaxbetSender(maxbet);
        }
    }

    private MaxbetSpread BuildMaxbetNextCorner(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch)
    {
        var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
        if (!hasExistsHdp)
        {
            var handicapMainMatch = GetHandicapMainMatch(maxbetMainMatch);

            var valueBettype = default(Tuple<int, int, int, int>);
            var hasExistsHdpMainMatch = handicapMainMatch.Value.HasValue && maxbetCornerMapper.TryGetValue((int)handicapMainMatch.Value, out valueBettype);

            maxbet.Add(MaxbetHelper.Build(hasExistsHdpMainMatch, (int)EnumBettype.Handicap, valueBettype?.Item2, handicapMainMatch.Value));
        }

        return BuildMaxbetSender(maxbet);
    }

    private MaxbetSpread BuildMaxbetNextBooking(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch)
    {
        var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
        if (!hasExistsHdp)
        {
            var handicapMainMatch = GetHandicapMainMatch(maxbetMainMatch);

            var valueBettype = default(Tuple<decimal, decimal>);
            var hasExistsHdpMainMatch = handicapMainMatch.Value.HasValue && maxbetBookingMapper.TryGetValue((int)handicapMainMatch.Value, out valueBettype);

            maxbet.Add(MaxbetHelper.Build(hasExistsHdpMainMatch, (int)EnumBettype.Handicap, valueBettype?.Item1, handicapMainMatch.Value));
        }

        return BuildMaxbetSender(maxbet);
    }

    private MaxbetSpread BuildMaxbetNoOfCorner(List<MaxbetModel> maxbet,
               List<MaxbetModel> maxbetMainMatch)
    {
        var handicapMainMatch = GetHandicapMainMatch(maxbetMainMatch);
        var canUpdateValueBettype = MaxbetHelper.CanUpdateValueBettype(maxbet);

        var valueBettype = default(Tuple<int, int, int, int>);
        var hasExistsValueBettype = handicapMainMatch.Value.HasValue && maxbetCornerMapper.TryGetValue((int)handicapMainMatch.Value, out valueBettype);

        if (canUpdateValueBettype.Hdp)
        {
            maxbet.Add(MaxbetHelper.Build(hasExistsValueBettype, (int)EnumBettype.Handicap, valueBettype?.Item1, handicapMainMatch.Value));
        }

        if (canUpdateValueBettype.Ou)
        {
            maxbet.Add(MaxbetHelper.Build(hasExistsValueBettype, (int)EnumBettype.OverUnder, valueBettype?.Item2, handicapMainMatch.Value));
        }

        if (canUpdateValueBettype.FirstHalfHdp)
        {
            maxbet.Add(MaxbetHelper.Build(hasExistsValueBettype, (int)EnumBettype.FirstHalfHandicap, valueBettype?.Item3, handicapMainMatch.Value));
        }

        if (canUpdateValueBettype.FirstHalfOu)
        {
            maxbet.Add(MaxbetHelper.Build(hasExistsValueBettype, (int)EnumBettype.FirstHalfOverUnder, valueBettype?.Item4, handicapMainMatch.Value));
        }

        return BuildMaxbetSender(maxbet);
    }

    private MaxbetSpread BuildMaxbetTotalBooking(
        List<MaxbetModel> maxbet,
        List<MaxbetModel> maxbetMainMatch)
    {
        var handicapMainMatch = GetHandicapMainMatch(maxbetMainMatch);
        var canUpdateValueBettype = MaxbetHelper.CanUpdateValueBettype(maxbet);

        var valueBettype = default(Tuple<decimal, decimal>);
        var hasExistsValueBettype = handicapMainMatch.Value.HasValue && maxbetBookingMapper.TryGetValue((int)handicapMainMatch.Value, out valueBettype);

        if (canUpdateValueBettype.Hdp)
        {
            maxbet.Add(MaxbetHelper.Build(hasExistsValueBettype, (int)EnumBettype.Handicap, valueBettype?.Item1, handicapMainMatch.Value));
        }

        if (canUpdateValueBettype.Ou)
        {
            maxbet.Add(MaxbetHelper.Build(hasExistsValueBettype, (int)EnumBettype.OverUnder, valueBettype?.Item1, handicapMainMatch.Value));
        }

        if (canUpdateValueBettype.FirstHalfHdp)
        {
            maxbet.Add(MaxbetHelper.Build(hasExistsValueBettype, (int)EnumBettype.FirstHalfHandicap, valueBettype?.Item2, handicapMainMatch.Value));
        }

        if (canUpdateValueBettype.FirstHalfOu)
        {
            maxbet.Add(MaxbetHelper.Build(hasExistsValueBettype, (int)EnumBettype.FirstHalfOverUnder, valueBettype?.Item2, handicapMainMatch.Value));
        }

        return BuildMaxbetSender(maxbet);
    }

    public void BuildMaxbetOverUnder(List<MaxbetModel> maxbet)
    {
        var handicap = maxbet.FirstOrDefault(m => m.BettypeId == (int)EnumBettype.Handicap) ?? new MaxbetModel();

        maxbet.Add(new MaxbetModel
        {
            BettypeId = (int)EnumBettype.OverUnder,
            Value = handicap?.Value
        });
    }

    public void BuildMaxbetFirstHalfOverUnder(List<MaxbetModel> maxbet)
    {
        var handicap = maxbet.FirstOrDefault(m => m.BettypeId == (int)EnumBettype.Handicap) ?? new MaxbetModel();

        int valueFirstHalfOverUnder = 0;
        var existsValueFirstHalfOverunder = handicap.Value.HasValue
            && maxbetMapperFirstHalfOU.TryGetValue((int)handicap.Value, out valueFirstHalfOverUnder);

        var maxbetModel = MaxbetHelper.Build(existsValueFirstHalfOverunder, (int)EnumBettype.FirstHalfOverUnder, valueFirstHalfOverUnder, handicap.Value);
        maxbet.Add(maxbetModel);
    }

    public void BuildMaxbetFirstHalfHandicap(List<MaxbetModel> maxbet)
    {
        var handicap = maxbet.FirstOrDefault(m => m.BettypeId == (int)EnumBettype.Handicap) ?? new MaxbetModel();

        int valueFirstHalfHandicap = 0;
        var existsValueFirstHalfHandicap = handicap.Value.HasValue
            && maxbetMapperFirstHalfHDP.TryGetValue((int)handicap.Value, out valueFirstHalfHandicap);

        var maxbetModel = MaxbetHelper.Build(existsValueFirstHalfHandicap, (int)EnumBettype.FirstHalfHandicap, valueFirstHalfHandicap, handicap.Value);
        maxbet.Add(maxbetModel);
    }

    public static MaxbetSpread BuildMaxbetSender(List<MaxbetModel> maxbet)
    {
        var handicap = MaxbetHelper.FindMaxbetByBettype(maxbet, (int)EnumBettype.Handicap) ?? new MaxbetModel();
        var overunder = MaxbetHelper.FindMaxbetByBettype(maxbet, (int)EnumBettype.OverUnder) ?? new MaxbetModel();
        var firstHalfHandicap = MaxbetHelper.FindMaxbetByBettype(maxbet, (int)EnumBettype.FirstHalfHandicap) ?? new MaxbetModel();
        var firstHalfOverUnder = MaxbetHelper.FindMaxbetByBettype(maxbet, (int)EnumBettype.FirstHalfOverUnder) ?? new MaxbetModel();

        return new MaxbetSpread
        {
            Handicap = handicap.Value,
            OverUnder = overunder.Value,
            FirstHalfHandicap = firstHalfHandicap.Value,
            FirstHalfOverUnder = firstHalfOverUnder.Value
        };
    }

    private MaxbetModel GetHandicapMainMatch(List<MaxbetModel> maxbetMainMatch)
        => maxbetMainMatch.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap)) ?? new MaxbetModel();
}