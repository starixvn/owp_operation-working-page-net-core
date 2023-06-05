using Fanex.Data.Repository;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.Models;

namespace OWP.PrematchAssignment.Services;

public interface Repository
{
    Task<Dictionary<int, List<TicketLeagueBasicInfo>>> QueryAllLeaguesTicket(IEnumerable<int> owpLeagueIds, int actorId);

    Task<Dictionary<int, List<TicketMatchBasicInfo>>> QueryAllMatchesTicket(IEnumerable<int> infoSiteMatchIds, int actorId);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository repository;

    public RepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Dictionary<int, List<TicketLeagueBasicInfo>>> QueryAllLeaguesTicket(IEnumerable<int> owpLeagueIds, int actorId)
    {
        var criteria = new QueryAllLeaguesTicketCriteria
        {
            OWPLeagueIds = string.Join(",", owpLeagueIds.Distinct()),
            ActorId = actorId
        };

        var result = await repository.FetchAsync<TicketLeagueBasicInfo>(criteria);

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

        var result = await repository.FetchAsync<TicketMatchBasicInfo>(criteria);

        return result
            .GroupBy(x => x.InfoSiteMatchId)
            .ToDictionary(x => x.Key, x => x.ToList());
    }
}