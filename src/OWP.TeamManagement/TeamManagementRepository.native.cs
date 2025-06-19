using Fanex.Data.Repository;
using OWP.TeamManagement.Criteria;
using OWP.TeamManagement.Handlers;
using OWP.TeamManagement.Model;

namespace OWP.TeamManagement;

public interface TeamManagementRepository
{
    Task<IEnumerable<QueryTeamInfoResult>> GetTeamInfo(GetTeamInfoRequest request);
}

public class TeamManagementRepositoryImp(IDynamicRepository dynamicRepository) : TeamManagementRepository
{
    public async Task<IEnumerable<QueryTeamInfoResult>> GetTeamInfo(GetTeamInfoRequest request)
    {
        var criteria = new GetTeamInfoCriteria()
        {
            TeamName = request.TeamName,
            LeagueName = request.LeagueName,
            LeagueGroupId = request.LeagueGroupId
        };

        return (await dynamicRepository.FetchAsync<QueryTeamInfoResult>(criteria)).ToList();
    }
}