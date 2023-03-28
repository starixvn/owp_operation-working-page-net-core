using Fanex.Data.Repository;
using OWP.AssigneeSuggestionList.Criteria;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList;

public interface Repository
{
    Task<MatchInfo> QueryMatchInfo(int owpMatchId);

    Task<IEnumerable<UserInfo>> QueryTraderInfo(DateTime date);
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

    public async Task<IEnumerable<UserInfo>> QueryTraderInfo(DateTime date)
    {
        var criteria = new QueryTraderInfoCriteria
        {
            EventDate = date,
        };

        return await dynamicRepository.FetchAsync<UserInfo>(criteria);
    }
}
