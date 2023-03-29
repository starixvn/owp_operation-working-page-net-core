using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using OWP.AssigneeSuggestionList.Criteria;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList;

public interface Repository
{
    Task<MatchInfo> QueryMatchInfo(int owpMatchId);

    Task<IEnumerable<UserInfo>> QueryTraderInfo(DateTime date);

    Task<IEnumerable<int>> GetUserInfoFromNAPUserControl();

    Task<IEnumerable<int>> GetCustInfoFromDB02();

    Task<IList<AssignmentModel>> QueryAssignments(DateTime eventDate);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSettings napSettings;

    public RepositoryImp(IDynamicRepository dynamicRepository, INapSettings napSettings)
    {
        this.dynamicRepository = dynamicRepository;
        this.napSettings = napSettings;
    }

    public async Task<MatchInfo> QueryMatchInfo(int owpMatchId)
    {
        var criteria = new QueryMatchInfoCriteria
        {
            OwpMatchId = owpMatchId,
        };

        return (await dynamicRepository.FetchAsync<MatchInfo>(criteria)).FirstOrDefault();
    }

    public async Task<IEnumerable<UserInfo>> QueryTraderInfo(DateTime date)
    {
        var criteria = new QueryTraderInfoCriteria
        {
            EventDate = date,
        };

        return await dynamicRepository.FetchAsync<UserInfo>(criteria);
    }

    public async Task<IList<AssignmentModel>> QueryAssignments(DateTime eventDate)
    {
        var assignmentModel = await dynamicRepository.FetchAsync<AssignmentModel>(new QueryAssignmentCriteria
        {
            EventDate = eventDate
        });
        return assignmentModel.ToList();
    }

    public async Task<IEnumerable<int>> GetCustInfoFromDB02()
        => await dynamicRepository.FetchAsync<int>(new GetCustInfoFromDB02Criteria());

    public async Task<IEnumerable<int>> GetUserInfoFromNAPUserControl()
        => await dynamicRepository.FetchAsync<int>(new GetUserInfoFromNAPUserControlCriteria { SiteId = napSettings.SiteId });
}
