using Fanex.Data.Repository;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.QueryAndSave.Models;

namespace OWP.PrematchAssignment.QueryAndSave.Services;

public interface Repository
{
    Task<Dictionary<int, List<TicketLeagueBasicInfo>>> QueryAllLeaguesTicket(IEnumerable<int> owpLeagueIds, int actorId);

    Task<Dictionary<int, List<TicketMatchBasicInfo>>> QueryAllMatchesTicket(IEnumerable<int> infoSiteMatchIds, int actorId);

    Task<List<LinkedMatch>> QueryLinkedOwpMatch(List<int> infositeIds, DateTime eventDate);

    Task<TimeSpan?> QueryExistedStartTime(DateTime eventDate);

    Task<(List<PreMatchLeague>, List<InfoSiteMatch>)> QueryDayShiftPreMatchAssignments(QueryShiftPreMatchCriteria criteria);

    Task<IEnumerable<OddStatus>> QueryOddStatusFromMainDb(IEnumerable<InfoSiteMatch> infositeMatches);

    Task<IEnumerable<string>> GetEarlyOfferedLeagues(DateTime eventDate);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;

    public RepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<Dictionary<int, List<TicketLeagueBasicInfo>>> QueryAllLeaguesTicket(IEnumerable<int> owpLeagueIds, int actorId)
    {
        var criteria = new QueryAllLeaguesTicketCriteria
        {
            OWPLeagueIds = string.Join(",", owpLeagueIds.Distinct()),
            ActorId = actorId
        };

        var result = await dynamicRepository.FetchAsync<TicketLeagueBasicInfo>(criteria);

        return result
            .GroupBy(x => x.OWPLeagueId)
            .ToDictionary(x => x.Key, x => x.ToList());
    }

    public async Task<Dictionary<int, List<TicketMatchBasicInfo>>> QueryAllMatchesTicket(IEnumerable<int> infoSiteMatchIds, int actorId)
    {
        var criteria = new QueryAllMatchesTicketCriteria
        {
            InfoSiteMatchIds = string.Join(",", infoSiteMatchIds.Distinct()),
            ActorId = actorId
        };

        var result = await dynamicRepository.FetchAsync<TicketMatchBasicInfo>(criteria);

        return result
            .GroupBy(x => x.InfoSiteMatchId)
            .ToDictionary(x => x.Key, x => x.ToList());
    }

    public async Task<List<LinkedMatch>> QueryLinkedOwpMatch(List<int> infositeIds, DateTime eventDate)
    {
        var criteria = new QueryLinkedOWPMatchCriteria
        {
            InfositeMatchIds = string.Join(",", infositeIds),
            EventDate = eventDate
        };

        var result = await dynamicRepository.FetchAsync<LinkedMatch>(criteria);

        return result.ToList();
    }

    public async Task<TimeSpan?> QueryExistedStartTime(DateTime eventDate)
    {
        var criteria = new GetStartTimeCriteria
        {
            EventDate = eventDate
        };
        var result = (await dynamicRepository.FetchAsync<string>(criteria)).ToList();

        return result.Any() ? TimeSpan.Parse(result.FirstOrDefault()) : null;
    }

    public async Task<(List<PreMatchLeague>, List<InfoSiteMatch>)> QueryDayShiftPreMatchAssignments(QueryShiftPreMatchCriteria criteria)
    {
        var (preMatchLeagues, infoSiteMatches) = await dynamicRepository.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(criteria);

        return (preMatchLeagues.ToList(), infoSiteMatches.ToList());
    }

    public async Task<IEnumerable<OddStatus>> QueryOddStatusFromMainDb(IEnumerable<InfoSiteMatch> infositeMatches)
    {
        var adminSiteMatchIds = infositeMatches.Select(x => x.AdminSiteMatchId).Distinct();
        var criteria = new QueryOddStatusFromMainDBCriteria
        {
            AdminSiteMatchIds = string.Join(",", adminSiteMatchIds)
        };

        return await dynamicRepository.FetchAsync<OddStatus>(criteria);
    }

    public async Task<IEnumerable<string>> GetEarlyOfferedLeagues(DateTime eventDate)
    {
        var criteria = new GetEarlyOfferedLeaguesCriteria
        {
            EventDate = eventDate
        };
        return await dynamicRepository.FetchAsync<string>(criteria);
    }
}