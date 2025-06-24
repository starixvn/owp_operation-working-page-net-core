using Fanex.Data.Repository;
using OWP.TeamManagement.Criteria;
using OWP.TeamManagement.Handlers;
using OWP.TeamManagement.Model;

namespace OWP.TeamManagement;

public interface TeamManagementRepository
{
    Task<IEnumerable<LeagueInfo>> GetTeamInfo(GetTeamInfoRequest request);
}

public class TeamManagementRepositoryImp(IDynamicRepository dynamicRepository) : TeamManagementRepository
{
    public async Task<IEnumerable<LeagueInfo>> GetTeamInfo(GetTeamInfoRequest request)
    {
        var criteria = new GetTeamInfoCriteria()
        {
            TeamName = request.TeamName,
            LeagueName = request.LeagueName,
            LeagueGroupId = request.LeagueGroupId
        };
        var rawData = await dynamicRepository.FetchAsync<QueryTeamInfoResult>(criteria);

        var leagues = rawData
            .GroupBy(r => new
            {
                r.LeagueId,
                r.LeagueName,
                r.LeagueGroupId,
                r.LeagueGroupName
            })
            .Select(g => new LeagueInfo
            {
                LeagueId = g.Key.LeagueId,
                LeagueName = g.Key.LeagueName,
                LeagueGroupId = g.Key.LeagueGroupId,
                LeagueGroupName = g.Key.LeagueGroupName,
                Teams = g
                    .GroupBy(t => t.TeamId)
                    .Select(tg => new TeamInfo
                    {
                        TeamId = tg.Key,
                        TeamName = tg.First().TeamName
                    })
                    .OrderBy(t => t.TeamName) 
                    .ToList()
            })
            .OrderBy(l => l.LeagueName)
            .ToList();

        return leagues;
    }
}