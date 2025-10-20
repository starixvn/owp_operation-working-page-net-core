using Fanex.Data.Repository;
using OWP.Criteria;
using OWP.Models;

namespace OWP.Services;

public interface MaxbetSettingService
{
    Task<IList<MaxBetSettingModel>> QueryMaxbetSetting();
};

public class MaxbetSettingServiceImp(IDynamicRepository dynamicRepository) : MaxbetSettingService
{
    public async Task<IList<MaxBetSettingModel>> QueryMaxbetSetting()
    {
        var criteria = new QueryMaxbetSettingCriteria();
        var result = (await dynamicRepository.FetchAsync<MaxBetSettingModel>(criteria)).ToList();
        result.ForEach(x => { x.MapsFollowFTHDP(); });

        return result;
    }
}

