
using Fanex.Data.Repository;
using OWP.Models;
using OWP.Criteria;

namespace OWP.Services;
public interface GetInfoSiteTimeService
{
    public Task<IEnumerable<InfoSiteTimeModel>> GetInfoSiteTime(string ids);
}


public class GetInfoSiteTimeServiceImp : GetInfoSiteTimeService
{
    private readonly IDynamicRepository dynamicRepository;
    public GetInfoSiteTimeServiceImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }
    public async Task<IEnumerable<InfoSiteTimeModel>> GetInfoSiteTime(string ids)
    {
        var criteria = new GetInfoSiteTimeCriteria
        {
            ParentMatchIds = ids
        };
        return await dynamicRepository.FetchAsync<InfoSiteTimeModel>(criteria);
    }
}