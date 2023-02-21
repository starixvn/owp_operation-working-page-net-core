using OWP.OtherSport.Submit.Constants;

namespace OWP.OtherSport.EventAssignment.Services;

using System.Globalization;
using Extensions;
using Models;
using Submit.Models;

public interface MatchSettingBuilder
{
    public void Build(List<EventAssignmentModel> eventAssignmentModels, Dictionary<int, List<MatchSetting>> matchSettings);
}

public class MatchSettingBuilderImp : MatchSettingBuilder
{
    public void Build(List<EventAssignmentModel> eventAssignmentModels, Dictionary<int, List<MatchSetting>> matchSettings)
    {
        foreach (var eventAssignmentModel in eventAssignmentModels)
        {
            var matchSettingsByOWPMatchId = matchSettings.GetValue(eventAssignmentModel.OSMatchId, new List<MatchSetting>());

            CreateMatchSetting(matchSettingsByOWPMatchId, eventAssignmentModel.TraderMaxbetModels, eventAssignmentModel.Sport);
            SortMatchSettings(eventAssignmentModel.TraderMaxbetModels);
        }
    }

    public void CreateMatchSetting(List<MatchSetting> matchSettings, List<TraderMaxBetModel> traderMaxbetModels, string sport)
    {
        foreach (var matchSetting in matchSettings)
        {
            var bettypeName = matchSetting.BetTypeName;
            var isMinorBettype = MinorBettypesRelationships.ContainsKey(bettypeName);
            var mainBettypeName = isMinorBettype
                ? MinorBettypesRelationships.GetValue(bettypeName)
                : bettypeName;

            var eventAssignmentByBettype = traderMaxbetModels.FirstOrDefault(x => x.BetTypeName == mainBettypeName);
            if (eventAssignmentByBettype != null)
            {
                var isSpecialBettype = MainBettypeHasMinorBettypes.ContainsKey(new Tuple<string, string>(bettypeName, sport));
                eventAssignmentByBettype.MatchSettings.Add(new MatchSettingModel
                {
                    BetTypeName = isMinorBettype || isSpecialBettype ? DisplayBettypes[bettypeName] : string.Empty,
                    MaxBet = matchSetting.MaxBet,
                    Spread = FormatSpread(matchSetting.Spread)
                });
            }
        }
    }

    public string FormatSpread(decimal? spread)
    {
        return spread.HasValue ? $"{(spread.Value * 100).ToString("0.##", CultureInfo.InvariantCulture)}%" : string.Empty;
    }

    public void SortMatchSettings(List<TraderMaxBetModel> traderMaxbetModels)
    {
        foreach (var traderMaxbetModel in traderMaxbetModels)
        {
            traderMaxbetModel.MatchSettings = traderMaxbetModel.MatchSettings.OrderBy(x => x.Order).ToList();
        }
    }

    private readonly Dictionary<Tuple<string, string>, int> MainBettypeHasMinorBettypes = new()
    {
        { new Tuple<string, string>(Bettype.FTOUOE, Sport.Basketball), 0 },
        { new Tuple<string, string>(Bettype.FTOUOE, Sport.Netball), 0 },
        { new Tuple<string, string>(Bettype.SetWinnerHDPOE, Sport.Volleyball), 0 },
        { new Tuple<string, string>(Bettype.FirstHOUOE, Sport.Netball), 0 },
        { new Tuple<string, string>(Bettype.FirstQ3QOUOE, Sport.Netball), 0 },
        { new Tuple<string, string>(Bettype.MatchWinnerHDPOU, Sport.Snooker), 0 },
        { new Tuple<string, string>(Bettype.FramesWinnerHDPOUOE, Sport.Snooker), 0 }
    };

    private readonly Dictionary<string, string> MinorBettypesRelationships = new()
    {
        { Bettype.FTOE, Bettype.FTOUOE },
        { Bettype.SetWinnerOE, Bettype.SetWinnerHDPOE },
        { Bettype.FirstHOE, Bettype.FirstHOUOE },
        { Bettype.FirstQ3QOE, Bettype.FirstQ3QOUOE },
        { Bettype.MatchWinnerOU, Bettype.MatchWinnerHDPOU },
        { Bettype.FramesWinnerOU, Bettype.FramesWinnerHDPOUOE },
        { Bettype.FramesWinnerOE, Bettype.FramesWinnerHDPOUOE }
    };

    private readonly Dictionary<string, string> DisplayBettypes = new()
    {
        { Bettype.FTOE, MinorBettype.OE },
        { Bettype.SetWinnerOE, MinorBettype.OE },
        { Bettype.FirstHOE, MinorBettype.OE },
        { Bettype.FirstQ3QOE, MinorBettype.OE },
        { Bettype.MatchWinnerOU, MinorBettype.OU },
        { Bettype.FramesWinnerOU, MinorBettype.OU },
        { Bettype.FramesWinnerOE, MinorBettype.OE },
        { Bettype.FTOUOE, MinorBettype.OU },
        { Bettype.SetWinnerHDPOE, MinorBettype.HDP },
        { Bettype.FirstHOUOE, MinorBettype.OU },
        { Bettype.FirstQ3QOUOE, MinorBettype.OU },
        { Bettype.MatchWinnerHDPOU, MinorBettype.HDP },
        { Bettype.FramesWinnerHDPOUOE, MinorBettype.HDP }
    };
}