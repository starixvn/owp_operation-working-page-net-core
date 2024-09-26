using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using OWP.Report.LAReport.Criteria;
using OWP.Report.LAReport.Models;
using OWP.Report.Models;
using Fanex.NAP.Client.Services;
using Newtonsoft.Json;

namespace OWP.Report.LAReport.Services;

public interface QueryRepository
{
    Task<IEnumerable<LAReportModel>> Query(QueryModel model);
}

public class QueryRepositoryImp : QueryRepository
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly INapSettings napSettings;

    private readonly IUserService userService;
    public QueryRepositoryImp(IDynamicRepository dynamicRepository, INapSettings napSettings, IUserService userService)
    {
        this.dynamicRepository = dynamicRepository;
        this.napSettings = napSettings;
        this.userService = userService;
    }

    public async Task<IEnumerable<LAReportModel>> Query(QueryModel model)
    {
        var allUser = await userService.GetBySiteAsync(napSettings.SiteId, null);
        var allUserActive = await userService.GetBySiteAsync(napSettings.SiteId, true);
        var criteria = new GetLAReportCriteria
        {
            FromDate = model.FromDate,
            ToDate = model.ToDate,
            ActiveUser = model.ActiveUser,
            ResignUser = model.ResignUser,
            AllUserJson = JsonConvert.SerializeObject(allUser.Select(u => new
            {
                UserId = u.Id,
                IsActive = allUserActive.Any(a => a.Id == u.Id)
            }))
        };

        return await dynamicRepository.FetchAsync<LAReportModel>(criteria);
    }

}