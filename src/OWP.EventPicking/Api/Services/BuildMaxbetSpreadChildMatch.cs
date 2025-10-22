using Newtonsoft.Json;

namespace OWP.EventPicking.Api.Services;

public interface BuildMaxbetSpreadChildMatch
{
    Task<IList<ChildMatchMaxbetSpread>> Execute(IList<SyncMaxbetSpreadChildMatchModel> childMatchInfo);
}

public class BuildMaxbetSpreadChildMatchImp(IDynamicRepository repository): BuildMaxbetSpreadChildMatch
{
    public async Task<IList<ChildMatchMaxbetSpread>> Execute(IList<SyncMaxbetSpreadChildMatchModel> childMatchInfo)
    {
        return (await repository.FetchAsync<ChildMatchMaxbetSpread>(new GetMaxbetSpreadChildMatchCriteria()
        {
            ChildMatchInfo = JsonConvert.SerializeObject(childMatchInfo)
        })).ToList();
    }
}
