using Newtonsoft.Json;

namespace OWP.EventPicking.Api.Services;

public interface MaxbetSpreadChildMatchBuilder
{
    Task<IList<ChildMatchMaxbetSpread>> BuildMaxbetSpreadChildMatch(IList<int> infositeMatchId);
}

public class MaxbetSpreadChildMatchBuilderImp(IDynamicRepository repository): MaxbetSpreadChildMatchBuilder
{
    public async Task<IList<ChildMatchMaxbetSpread>> BuildMaxbetSpreadChildMatch(IList<int> infositeMatchId)
    {
        return (await repository.FetchAsync<ChildMatchMaxbetSpread>(new GetMaxbetSpreadChildMatchCriteria()
        {
            InfositeMatchIds = string.Join(",",infositeMatchId)
        })).ToList();
    }
}
