using Fanex.Data.Repository;
using OWP.TodaySuggestionList.Criteria;
using OWP.TodaySuggestionList.Models;

namespace OWP.TodaySuggestionList;

public interface Repository
{
    Task<MatchInfo> QueryMatchInfo(int owpMatchId);
}

public class RepositoryImp
{
    private readonly IDynamicRepository dynamicRepository;

    public RepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<MatchInfo> QueryMatchInfo(int owpMatchId)
    {
        var criteria = new QueryMatchInfoCriteria
        {
            OwpMatchId = owpMatchId,
        };

        return (await dynamicRepository.FetchAsync<MatchInfo>(criteria)).FirstOrDefault();
    }
}
