using Fanex.Data.Repository;
using OWP.Criteria;
using OWP.Models;

namespace OWP.Services;

public interface MaxbetSettingService
{
    Task<IList<MaxBetSettingModel>> QueryMaxbetSetting();
};

public class MaxbetSettingServiceImp : MaxbetSettingService
{
    private readonly IDynamicRepository dynamicRepository;

    public MaxbetSettingServiceImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }
    public async Task<IList<MaxBetSettingModel>> QueryMaxbetSetting()
    {
        var criteria = new QueryMaxbetSettingCriteria();
        var result = (await dynamicRepository.FetchAsync<MaxBetSettingModel>(criteria)).ToList();
        result.ForEach(x => { x.MapsFollowFTHDP(); });

        return result;
    }
}

