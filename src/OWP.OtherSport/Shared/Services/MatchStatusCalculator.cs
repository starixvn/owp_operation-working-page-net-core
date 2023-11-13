using Fanex.Data.Repository;
using OWP.Extensions;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.Shared.Services;

public interface MatchStatusCalculator
{
    Task Map(Dictionary<int, MatchInfo> matchInfoDict);
}

public class MatchStatusCalculatorImp : MatchStatusCalculator
{
    private readonly IDynamicRepository dynamicRepository;

    public MatchStatusCalculatorImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task Map(Dictionary<int, MatchInfo> matchInfoDict)
    {
        await MapMatchStatusFromMainDb(matchInfoDict);

        foreach (var key in matchInfoDict.Keys)
        {
            var match = matchInfoDict[key];
            var nowDateTime = DateTime.Now.ToGmt8(); // NOSONAR

            match.MatchStatus = CalculateMatchStatus(match, nowDateTime);
        }
    }

    private static string CalculateMatchStatus(MatchInfo match, DateTime nowDateTime)
    {
        if (match.MatchStatus.Equals(MatchStatus.Completed, StringComparison.OrdinalIgnoreCase))
        {
            return MatchStatus.Completed;
        }

        if (match.DateTime.Date > nowDateTime.Date)
        {
            return MatchStatus.Early;
        }

        return nowDateTime >= match.DateTime ? MatchStatus.InPlay : MatchStatus.PreMatch;
    }

    private async Task MapMatchStatusFromMainDb(Dictionary<int, MatchInfo> matchInfoDict)
    {
        var adminSiteMatchIds = matchInfoDict.Values
            .Where(x => x.AdminsiteMatchId != null)
            .Select(x => x.AdminsiteMatchId).Distinct();
        var matchStatusFromMainDBs = await QueryMatchStatus(adminSiteMatchIds);

        foreach (var match in matchInfoDict.Keys.Select(key => matchInfoDict[key]))
        {
            if (match.AdminsiteMatchId == null)
            {
                match.MatchStatus = string.Empty;
            }
            else
            {
                var matchFromMainDb = matchStatusFromMainDBs.GetValue((int)match.AdminsiteMatchId, new EventStatus());
                match.MatchStatus = matchFromMainDb.MatchStatus;
            }
        }
    }

    public async Task<Dictionary<int, EventStatus>> QueryMatchStatus(IEnumerable<int?> adminsiteMatchIds)
    {
        var criteria = new GetEventStatusFromMainDBCriteria
        {
            AdminsiteMatchIds = string.Join(",", adminsiteMatchIds)
        };

        var result = await dynamicRepository.FetchAsync<EventStatus>(criteria);

        return result.ToDictionary(x => x.AdminsiteMatchId);
    }
}

public static class MatchStatus
{
    public const string InPlay = "In-play";
    public const string Completed = "Completed";
    public const string PreMatch = "Prematch";
    public const string Early = "Early";
}

public class GetEventStatusFromMainDBCriteria : CriteriaBase
{
    public string AdminsiteMatchIds { get; set; }

    public override string GetSettingKey() => "OWP_OSLiveGameAssignment_GetEventStatusFromMainDB";

    public override bool IsValid() => true;
}

public class EventStatus
{
    public int AdminsiteMatchId { get; set; }

    public string MatchStatus { get; set; } = string.Empty;
}