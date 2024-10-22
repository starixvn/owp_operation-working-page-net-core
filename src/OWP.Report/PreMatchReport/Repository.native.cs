using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Services;
using OWP.Report.PreMatchReport.Criteria;
using OWP.Report.PreMatchReport.Models;

namespace OWP.Report.PreMatchReport;

public interface Repository
{
    Task<IEnumerable<QueryPrematchReportModel>> GetReportForLeague(QueryModel queryModel);

    Task<IEnumerable<QueryPrematchReportModel>> GetReportForMatches(QueryModel queryModel);

    Task<IEnumerable<UserInfo>> GetUnAssigned(List<int> userIds, string keyWord);

    Task<IEnumerable<int>> GetUserIdFromDb02();

    Task<IEnumerable<int>> GetUserIdFromNapUser();
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

    public async Task<IEnumerable<QueryPrematchReportModel>> GetReportForLeague(QueryModel queryModel)
    {
        var criteria = new GetReportForLeagueCriteria
        {
            FromDate = queryModel.FromDate,
            ToDate = queryModel.ToDate,
            KeyWord = queryModel.KeyWord
        };
        return await dynamicRepository.FetchAsync<QueryPrematchReportModel>(criteria);
    }

    public async Task<IEnumerable<QueryPrematchReportModel>> GetReportForMatches(QueryModel queryModel)
    {
        var criteria = new GetReportForMatchesCriteria
        {
            FromDate = queryModel.FromDate,
            ToDate = queryModel.ToDate,
            KeyWord = queryModel.KeyWord
        };
        return await dynamicRepository.FetchAsync<QueryPrematchReportModel>(criteria);
    }

    public async Task<IEnumerable<UserInfo>> GetUnAssigned(List<int> userIds, string keyWord)
    {
        var criteria = new GetUnAssignedUserCriteria
        {
            UserIds = string.Join(',', userIds),
            KeyWord = keyWord
        };
        return (await dynamicRepository.FetchAsync<UserInfo>(criteria)).ToList();
    }

    public async Task<IEnumerable<int>> GetUserIdFromDb02()
    => await dynamicRepository.FetchAsync<int>(new GetUserIdFromDb02Criteria());

    public async Task<IEnumerable<int>> GetUserIdFromNapUser()
        => (await userService.GetBySiteAsync(napSettings.SiteId, true)).Select(u => u.Id);
}