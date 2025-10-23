using OWP.CQRS;
using OWP.Models;
using OWP.Services;

namespace OWP.MaxbetSetting.Handlers;

public record GetMaxbetSettingRequest(): Query<IList<MaxBetSettingModel>>;


public class GetMaxbetSettingHandler(MaxbetSettingService maxbetSettingService): QueryHandler<GetMaxbetSettingRequest, IList<MaxBetSettingModel>>
{
    public async Task<IList<MaxBetSettingModel>> Handle(GetMaxbetSettingRequest request, CancellationToken cancellationToken)
    {
        return await maxbetSettingService.QueryMaxbetSetting();
    }
}
