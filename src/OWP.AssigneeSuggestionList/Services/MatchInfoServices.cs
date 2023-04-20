using OWP.AssigneeSuggestionList.Handlers;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;
using OWP.Extensions;

namespace OWP.AssigneeSuggestionList.Services;
public interface MatchInfoServices
{
    Task<MatchInfo> GetMatchInfo(QueryTraderSuggestionRequest request);
}

public class MatchInfoServicesImp : MatchInfoServices
{
    private readonly Repository repository;

    public MatchInfoServicesImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<MatchInfo> GetMatchInfo(QueryTraderSuggestionRequest request)
    {
        var matchInfo = await repository.QueryMatchInfo(request.OwpMatchId);
        matchInfo.BettypeId = request.BetTypeId;
        matchInfo.FilterDate = request.FilterDate;
        UpdateEndTime(matchInfo);
        UpdateSingleMatch(matchInfo);

        return matchInfo;
    }

    public void UpdateEndTime(MatchInfo matchInfo)
    {
        matchInfo.EndTime = matchInfo.IsSaba
            ? matchInfo.KOTime.AddHours(1)
            : matchInfo.KOTime.Add(GetEndTimeForRealMatch(matchInfo));
    }

    public void UpdateSingleMatch(MatchInfo matchInfo)
    {
        matchInfo.IsSingleMatch = matchInfo.MaxbetHDP > 12_000 || IsSingleMatchSpecialCase(matchInfo);
    }

    private static bool IsSingleMatchSpecialCase(MatchInfo matchInfo)
        => matchInfo switch
        {
            _ when matchInfo.LeagueName.IsContains("Fantasy ") => true,
            _ when matchInfo.LeagueName.IsContains("FUTSAL")
                   && !matchInfo.LeagueName.IsContains("SABA ") => true,
            _ when matchInfo.MaxbetHDP == 10_000 && matchInfo.SpreadHDP is (decimal)0.08 or (decimal)0.10
                => true,
            _ when matchInfo.MaxbetHDP == 12_000 && matchInfo.SpreadHDP is (decimal)0.08 or (decimal)0.10
                => true,
            _ => false
        };

    public TimeSpan GetEndTimeForRealMatch(MatchInfo matchInfo)
        => matchInfo.IsMainMatch
            ? GetEndTimeForFtMatch(matchInfo)
            : GetEndTimeForCornerBooking(matchInfo);

    public TimeSpan GetEndTimeForFtMatch(MatchInfo matchInfo)
        => matchInfo.BettypeId is Bettype.FirstHalfHandicap or Bettype.FirstHalfOverUnder
            ? new TimeSpan(1, 0, 0)
            : EndTimeHelper.CalculateFtMatchEndTime(matchInfo.Remark);

    public TimeSpan GetEndTimeForCornerBooking(MatchInfo matchInfo)
    {
        var endTime = new TimeSpan(2, 0, 0);

        if (matchInfo.MinorBettypeId is Bettype.NextCorner or Bettype.NextBooking or Bettype.Corner or Bettype.TotalBooking)
        {
            endTime = EndTimeHelper.CalculateCornerMatchEndTime(matchInfo.Remark);
        }

        return endTime;
    }
}