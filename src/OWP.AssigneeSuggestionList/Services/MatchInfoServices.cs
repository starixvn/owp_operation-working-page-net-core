using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Handlers;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;

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
        // TODO
        var matchInfo = await repository.QueryMatchInfo(request.OwpMatchId);
        matchInfo.BettypeId = request.BetTypeId;
        matchInfo.FilterDate = request.FilterDate;
        matchInfo.KOTime = GetSuggestionTime(request, matchInfo);
        UpdateEndTime(matchInfo);
        UpdateSingleMatch(matchInfo);

        return matchInfo;
    }

    private static DateTime GetSuggestionTime(QueryTraderSuggestionRequest request, MatchInfo matchInfo)
    {
        if (!request.OtherTime.HasValue)
        {
            return matchInfo.KOTime;
        }

        const int twelfthHour = 12;
        var eventDate = request.OtherTime.Value.Hour < twelfthHour ? request.FilterDate.AddDays(1) : request.FilterDate;

        return eventDate.Date.Add(request.OtherTime.Value.TimeOfDay);
    }

    public void UpdateEndTime(MatchInfo matchInfo)
    {
        matchInfo.EndTime = matchInfo.IsSaba
            ? matchInfo.KOTime.AddHours(1)
            : matchInfo.KOTime.Add(GetEndTimeForRealMatch(matchInfo));
    }

    public void UpdateSingleMatch(MatchInfo matchInfo)
    {
        matchInfo.IsSingleMatch = SingleMatchHelper.IsSingleMatch(matchInfo.MaxbetHDP,matchInfo.LeagueName);
    }

    public TimeSpan GetEndTimeForRealMatch(MatchInfo matchInfo)
        => matchInfo.IsMainMatch
            ? GetEndTimeForFtMatch(matchInfo)
            : GetEndTimeForCornerBooking(matchInfo);

    public TimeSpan GetEndTimeForFtMatch(MatchInfo matchInfo)
    {
        var etpkType = ETPKHelper.DetectExtraTime(new AssignmentModel
        {
            Remark = matchInfo.Remark,
            AssignmentNote = matchInfo.AssignmentNote
        });
        return matchInfo.BettypeId is Bettype.FirstHalfHandicap or Bettype.FirstHalfOverUnder
              ? new TimeSpan(1, 0, 0)
              : EndTimeHelper.CalculateFtMatchEndTime(etpkType);
    }

    public static TimeSpan GetEndTimeForCornerBooking(MatchInfo matchInfo)
    {
        var endTime = new TimeSpan(2, 0, 0);

        if (matchInfo.MinorBettypeId != null && !BetTypeSpecial.BetTypes.Contains(matchInfo.MinorBettypeId.Value))
        {
            return endTime;
        }

        var etpkType = ETPKHelper.DetectExtraTime(new AssignmentModel
        {
            Remark = matchInfo.Remark,
            AssignmentNote = matchInfo.AssignmentNote
        });
        endTime = EndTimeHelper.CalculateCornerMatchEndTime(etpkType);

        return endTime;
    }
}