using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Services;
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
    private readonly IUserService userService;

    public RepositoryImp(IDynamicRepository dynamicRepository, INapSettings napSettings, IUserService userService)
    {
        this.dynamicRepository = dynamicRepository;
        this.napSettings = napSettings;
        this.userService = userService;
    }

    public async Task<MatchInfo> QueryMatchInfo(int owpMatchId)
    {
        var criteria = new QueryMatchInfoCriteria
        {
            OwpMatchId = owpMatchId
        };

        return (await dynamicRepository.FetchAsync<MatchInfo>(criteria)).FirstOrDefault();
    }

    public async Task<IEnumerable<UserInfo>> QueryTraderInfo(DateTime date)
    {
        var criteria = new QueryTraderInfoCriteria
        {
            EventDate = date
        };

        return await dynamicRepository.FetchAsync<UserInfo>(criteria);
    }

    public async Task<IList<AssignmentModel>> QueryAssignments(DateTime eventDate)
    {
        var criteria = new QueryAssignmentCriteria
        {
            EventDate = eventDate
        };
        var assignmentModel = await dynamicRepository.FetchAsync<AssignmentModel>(criteria);

        return assignmentModel.ToList();
    }

    public async Task<IEnumerable<int>> GetCustInfoFromDB02()
        => await dynamicRepository.FetchAsync<int>(new GetCustInfoFromDB02Criteria());

    public async Task<IEnumerable<int>> GetUserInfoFromNAPUserControl()
        => (await userService.GetBySiteAsync(napSettings.SiteId, true)).Select(u => u.Id);
}
