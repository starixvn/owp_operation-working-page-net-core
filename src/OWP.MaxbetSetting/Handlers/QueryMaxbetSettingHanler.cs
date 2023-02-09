namespace OWP.MaxbetSetting.Handlers;

using Criteria;
using Fanex.Data.Repository;
using MediatR;
using Models;

public class QueryMaxbetSettingRequest : IRequest<List<MaxBetSettingModel>> { }

public class QueryMaxbetSettingHanler : IRequestHandler<QueryMaxbetSettingRequest, List<MaxBetSettingModel>>
{
    private readonly IDynamicRepository dynamicRepository;

    public QueryMaxbetSettingHanler(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<List<MaxBetSettingModel>> Handle(QueryMaxbetSettingRequest request, CancellationToken cancellationToken)
    {
        var criteria = new QueryMaxbetSettingCriteria();
        var result = (await dynamicRepository.FetchAsync<MaxBetSettingModel>(criteria)).ToList();
        result.ForEach(x => { x.MapsFollowFTHDP(); });

        return result;
    }
}
