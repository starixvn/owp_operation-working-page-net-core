using Fanex.Data.Repository;
using MediatR;
using OWP.MaxbetSetting.Criteria;
using OWP.MaxbetSetting.Models;

namespace OWP.MaxbetSetting.Handlers;

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
        var criteria = new QueryMaxbetSettingCriteria { };

        return (await dynamicRepository.FetchAsync<MaxBetSettingModel>(criteria)).ToList();
    }
}
