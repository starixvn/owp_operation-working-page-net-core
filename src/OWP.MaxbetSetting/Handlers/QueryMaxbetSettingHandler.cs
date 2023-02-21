namespace OWP.MaxbetSetting.Handlers;

using Criteria;
using Fanex.Data.Repository;
using MediatR;
using Models;

public class QueryMaxbetSettingRequest : IRequest<List<MaxBetSettingModel>> { }

public class QueryMaxbetSettingHandler : IRequestHandler<QueryMaxbetSettingRequest, List<MaxBetSettingModel>>
{
    private readonly IDynamicRepository dynamicRepository;

    public QueryMaxbetSettingHandler(IDynamicRepository dynamicRepository)
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
