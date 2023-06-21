using OWP.Enums;
using OWP.Maxbet.Constants;
using OWP.Maxbet.Helpers;
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

        var maxBetMainMatchDefault = (await syncMaxbetRepository.GetMaxbetSettingDefault()).ToList();

        var hasExistsOu = maxbet.Any(s => s.BettypeId == (int)EnumBettype.OverUnder);
        if (!hasExistsOu)
        {
            BuildMaxbetOverUnder(maxbet, maxBetMainMatchDefault);
        }

        var hasExistsFirstHalfHdp = maxbet.Any(s => s.BettypeId == (int)EnumBettype.FirstHalfHandicap);
        if (!hasExistsFirstHalfHdp)
        {
            BuildMaxbetFirstHalfHandicap(maxbet, maxBetMainMatchDefault);
        }

        var hasExistsFirstHalfOu = maxbet.Any(s => s.BettypeId == (int)EnumBettype.FirstHalfOverUnder);
        if (!hasExistsFirstHalfOu)
        {
            BuildMaxbetFirstHalfOverUnder(maxbet, maxBetMainMatchDefault);
        }

        return BuildMaxbetSender(maxbet);
    }

    public async Task<MaxbetSpread> BuildMaxbetSpecialProduct(MinorMatchInfo specialProduct, int bettypeId)
    {
        var maxbet = (await syncMaxbetRepository.GetMaxbetByOwpMatchId(specialProduct.MinorMatchId)).ToList();
        var maxbetMainMatch = (await syncMaxbetRepository.GetMaxbetByOwpMatchId(specialProduct.ParentMatchId)).ToList();
        var maxBetSpecialDefault = (await syncMaxbetRepository.GetMaxbetSettingDefault()).ToList();

        switch (bettypeId)
        {
            case SpecialBettype.BETTYPE_ID_NO_OF_CORNERS:
                return BuildMaxbetNoOfCorner(maxbet, maxbetMainMatch, maxBetSpecialDefault);

            case SpecialBettype.BETTYPE_ID_NEXT_CORNERS:
                return BuildMaxbetNextCorner(maxbet, maxbetMainMatch, maxBetSpecialDefault);

            case SpecialBettype.BETTYPE_ID_NEXT_BOOKINGS:
                return BuildMaxbetNextBooking(maxbet, maxbetMainMatch, maxBetSpecialDefault);

            case SpecialBettype.BETTYPE_ID_TOTAL_BOOKINGS:
                return BuildMaxbetTotalBooking(maxbet, maxbetMainMatch, maxBetSpecialDefault);

            default:
                return BuildMaxbetSender(maxbet);
        }
    }

    private MaxbetSpread BuildMaxbetNextCorner(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch, List<MaxbetSetting> maxbetSettingDefault)
    {
        var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
        if (!hasExistsHdp)
        {
            var handicapMainMatch = GetHandicapMainMatch(maxbetMainMatch);

            var valueBettype = maxbetSettingDefault.FirstOrDefault(x => x.FT_HDP == handicapMainMatch.Value);

            maxbet.Add(MaxbetHelper.Build((int)EnumBettype.Handicap, valueBettype?.NextCorner_FT_HDP, handicapMainMatch.Value));
        }

        return BuildMaxbetSender(maxbet);
    }

    private MaxbetSpread BuildMaxbetNextBooking(List<MaxbetModel> maxbet, List<MaxbetModel> maxbetMainMatch, List<MaxbetSetting> maxbetSettingDefault)
    {
        var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
        if (!hasExistsHdp)
        {
            var handicapMainMatch = GetHandicapMainMatch(maxbetMainMatch);

            var valueBettype = maxbetSettingDefault.FirstOrDefault(x => x.FT_HDP == handicapMainMatch.Value);

            maxbet.Add(MaxbetHelper.Build((int)EnumBettype.Handicap, valueBettype?.NextBooking_FT_HDP, handicapMainMatch.Value));
        }

        return BuildMaxbetSender(maxbet);
    }

    private MaxbetSpread BuildMaxbetNoOfCorner(List<MaxbetModel> maxbet,
            List<MaxbetModel> maxbetMainMatch, IEnumerable<MaxbetSetting> maxbetSettingDefault)
    {
        var handicapMainMatch = GetHandicapMainMatch(maxbetMainMatch);
        var canUpdateValueBettype = MaxbetHelper.CanUpdateValueBettype(maxbet);

        var valueBettype = maxbetSettingDefault.FirstOrDefault(x => x.FT_HDP == handicapMainMatch.Value);

        if (canUpdateValueBettype.Hdp)
        {
            maxbet.Add(MaxbetHelper.Build((int)EnumBettype.Handicap, valueBettype?.NoOfCorner_FT_HDP, handicapMainMatch.Value));
        }

        if (canUpdateValueBettype.Ou)
        {
            maxbet.Add(MaxbetHelper.Build((int)EnumBettype.OverUnder, valueBettype?.NoOfCorner_FT_OU, handicapMainMatch.Value));
        }

        if (canUpdateValueBettype.FirstHalfHdp)
        {
            maxbet.Add(MaxbetHelper.Build((int)EnumBettype.FirstHalfHandicap, valueBettype?.NoOfCorner_1H_HDP, handicapMainMatch.Value));
        }

        if (canUpdateValueBettype.FirstHalfOu)
        {
            maxbet.Add(MaxbetHelper.Build((int)EnumBettype.FirstHalfOverUnder, valueBettype?.NoOfCorner_1H_OU, handicapMainMatch.Value));
        }

        return BuildMaxbetSender(maxbet);
    }

    private MaxbetSpread BuildMaxbetTotalBooking(
        List<MaxbetModel> maxbet,
        List<MaxbetModel> maxbetMainMatch,
        IEnumerable<MaxbetSetting> maxbetSettingDefault)
    {
        var handicapMainMatch = GetHandicapMainMatch(maxbetMainMatch);
        var canUpdateValueBettype = MaxbetHelper.CanUpdateValueBettype(maxbet);

        var valueBettype = maxbetSettingDefault.FirstOrDefault(x => x.FT_HDP == handicapMainMatch.Value);

        if (canUpdateValueBettype.Hdp)
        {
            maxbet.Add(MaxbetHelper.Build((int)EnumBettype.Handicap, valueBettype?.TotalBooking_FT_HDP, handicapMainMatch.Value));
        }

        if (canUpdateValueBettype.Ou)
        {
            maxbet.Add(MaxbetHelper.Build((int)EnumBettype.OverUnder, valueBettype?.TotalBooking_FT_OU, handicapMainMatch.Value));
        }

        if (canUpdateValueBettype.FirstHalfHdp)
        {
            maxbet.Add(MaxbetHelper.Build((int)EnumBettype.FirstHalfHandicap, valueBettype?.TotalBooking_1H_HDP, handicapMainMatch.Value));
        }

        if (canUpdateValueBettype.FirstHalfOu)
        {
            maxbet.Add(MaxbetHelper.Build((int)EnumBettype.FirstHalfOverUnder, valueBettype?.TotalBooking_1H_OU, handicapMainMatch.Value));
        }

        return BuildMaxbetSender(maxbet);
    }

    public void BuildMaxbetOverUnder(List<MaxbetModel> maxbet, IList<MaxbetSetting> maxbetSettings)
    {
        var handicap = maxbet.FirstOrDefault(m => m.BettypeId == (int)EnumBettype.Handicap) ?? new MaxbetModel();
        var maxbetFtOu = maxbetSettings.FirstOrDefault(x => x.FT_HDP == handicap.Value)?.FT_OU;

        var maxbetModel = MaxbetHelper.Build((int)EnumBettype.OverUnder, maxbetFtOu, handicap.Value);

        maxbet.Add(maxbetModel);
    }

    public void BuildMaxbetFirstHalfOverUnder(List<MaxbetModel> maxbet, IList<MaxbetSetting> maxbetSettings)
    {
        var handicap = maxbet.FirstOrDefault(m => m.BettypeId == (int)EnumBettype.Handicap) ?? new MaxbetModel();

        var valueFirstHalfOverUnder = maxbetSettings.FirstOrDefault(x => x.FT_HDP == handicap.Value)?.FirstHalf_OU;

        var maxbetModel = MaxbetHelper.Build((int)EnumBettype.FirstHalfOverUnder, valueFirstHalfOverUnder, handicap.Value);

        maxbet.Add(maxbetModel);
    }

    public void BuildMaxbetFirstHalfHandicap(List<MaxbetModel> maxbet, IList<MaxbetSetting> maxbetSettings)
    {
        var handicap = maxbet.FirstOrDefault(m => m.BettypeId == (int)EnumBettype.Handicap) ?? new MaxbetModel();

        var valueFirstHalfHandicap = maxbetSettings.FirstOrDefault(x => x.FT_HDP == handicap.Value)?.FirstHalf_HDP;

        var maxbetModel = MaxbetHelper.Build((int)EnumBettype.FirstHalfHandicap, valueFirstHalfHandicap, handicap.Value);
        maxbet.Add(maxbetModel);
    }

    public static MaxbetSpread BuildMaxbetSender(List<MaxbetModel> maxbet)
    {
        var handicap = MaxbetHelper.FindMaxbetByBettype(maxbet, (int)EnumBettype.Handicap) ?? new MaxbetModel();
        var overUnder = MaxbetHelper.FindMaxbetByBettype(maxbet, (int)EnumBettype.OverUnder) ?? new MaxbetModel();
        var firstHalfHandicap = MaxbetHelper.FindMaxbetByBettype(maxbet, (int)EnumBettype.FirstHalfHandicap) ?? new MaxbetModel();
        var firstHalfOverUnder = MaxbetHelper.FindMaxbetByBettype(maxbet, (int)EnumBettype.FirstHalfOverUnder) ?? new MaxbetModel();

        return new MaxbetSpread
        {
            Handicap = handicap.Value,
            OverUnder = overUnder.Value,
            FirstHalfHandicap = firstHalfHandicap.Value,
            FirstHalfOverUnder = firstHalfOverUnder.Value
        };
    }

    private MaxbetModel GetHandicapMainMatch(IEnumerable<MaxbetModel> maxbetMainMatch)
        => maxbetMainMatch.FirstOrDefault(m => m.BettypeId.Equals((int)EnumBettype.Handicap)) ?? new MaxbetModel();
}