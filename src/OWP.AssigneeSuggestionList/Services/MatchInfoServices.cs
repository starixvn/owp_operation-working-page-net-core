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
        await UpdateEndTime(matchInfo);
        UpdateSingleMatch(matchInfo);

        return matchInfo;
    }

    public async Task UpdateEndTime(MatchInfo matchInfo)
    {
        matchInfo.EndTime = matchInfo.IsSaba
            ? matchInfo.EventDateTime.AddHours(1)
            : matchInfo.EventDateTime.Add(await GetEndTimeForRealMatch(matchInfo));
    }

    public void UpdateSingleMatch(MatchInfo matchInfo)
    {
        matchInfo.IsSingleMatch = matchInfo.MaxbetHDP > 12 || IsSingleMatchSpecialCase(matchInfo);
    }

    private bool IsSingleMatchSpecialCase(MatchInfo matchInfo)
    {
        return matchInfo switch
        {
            _ when matchInfo.LeagueName.IsContains("Fantasy ") => true,
            _ when matchInfo.LeagueName.IsContains("FUTSAL")
                   && !matchInfo.LeagueName.IsContains("SABA ") => true,
            _ when matchInfo.MaxbetHDP == 10 && (matchInfo.SpreadHDP == 8 || matchInfo.SpreadHDP == 10)
                => true,
            _ when matchInfo.MaxbetHDP == 12 && (matchInfo.SpreadHDP == 8 || matchInfo.SpreadHDP == 10)
                => true,
            _ => false
        };
    }

    public async Task<TimeSpan> GetEndTimeForRealMatch(MatchInfo matchInfo)
        => matchInfo.IsMainMatch ? GetEndTimeForFtMatch(matchInfo)
            : await GetEndTimeForCornerBooking(matchInfo);

    public TimeSpan GetEndTimeForFtMatch(MatchInfo matchInfo)
        => matchInfo.BettypeId == Bettype.FirstHalfHandicap ||
           matchInfo.BettypeId == Bettype.FirstHalfOverUnder
            ? new TimeSpan(1, 0, 0)
            : EndTimeHelper.CalculateFtMatchEndTime(matchInfo.Remark);

    public async Task<TimeSpan> GetEndTimeForCornerBooking(MatchInfo matchInfo)
    {
        var endTime = new TimeSpan(2, 0, 0);

        if (matchInfo.BettypeId is Bettype.NextCorner or Bettype.NextBooking or Bettype.Corner or Bettype.TotalBooking)
        {
            var parentMatch = await repository.QueryParentMatchInfo(new List<int> { matchInfo.OwpMatchId });
            var remark = parentMatch.FirstOrDefault(x => x.OwpMatchId == matchInfo.ParentMatchId)?.Remark;
            endTime = EndTimeHelper.CalculateCornerMatchEndTime(remark);
        }

        return endTime;
    }
}