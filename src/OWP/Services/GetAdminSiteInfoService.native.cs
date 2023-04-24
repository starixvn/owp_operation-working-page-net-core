using Fanex.Data.Repository;
using OWP.Criteria;
using OWP.Models;

namespace OWP.Services;

public interface GetAdminSiteInfoService
{
    public Task<IEnumerable<AdminSiteOddsModel>> GetAdminSiteInfo();

    public Task<IEnumerable<AdminSiteIdModel>> GetAdminSiteMatchId(string ids);
}

public class GetAdminSiteInfoServiceImp : GetAdminSiteInfoService
{
    private readonly IDynamicRepository dynamicRepository;

    public GetAdminSiteInfoServiceImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<IEnumerable<AdminSiteOddsModel>> GetAdminSiteInfo()
    {
        var criteria = new GetAdminSiteInfoCriteria();
        return await dynamicRepository.FetchAsync<AdminSiteOddsModel>(criteria);
    }

    public async Task<IEnumerable<AdminSiteIdModel>> GetAdminSiteMatchId(string ids)
    {
        var criteria = new GetAdminSiteIdByOwpMatchIdCriteria
        {
            ParentMatchIds = ids
        };
        return await dynamicRepository.FetchAsync<AdminSiteIdModel>(criteria);
    }
}