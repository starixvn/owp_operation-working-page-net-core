using Fanex.Data.Repository;
using OWP.Criteria;
using OWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.Services;
public interface GetAdminSiteInfoService
{
    public Task<IEnumerable<AdminSiteInfoModel>> GetAdminSiteInfo(string ids);
}

public class GetAdminSiteInfoServiceImp : GetAdminSiteInfoService
{
    private readonly IDynamicRepository dynamicRepository;
    public GetAdminSiteInfoServiceImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }
    public async Task<IEnumerable<AdminSiteInfoModel>> GetAdminSiteInfo(string ids)
    {
        var criteria = new GetAdminSiteInfoCriteria
        {
            ParentMatchIds = ids
        };
        return await dynamicRepository.FetchAsync<AdminSiteInfoModel>(criteria);
    }
}