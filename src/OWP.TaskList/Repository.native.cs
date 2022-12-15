namespace OWP.TaskList;

using Criteria;
using Fanex.Data.Repository;
using Models;

public interface Repository
{
    Task<IEnumerable<MatchInfo>> GetTaskList(GetTaskListCriteria criteria);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;

    public RepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<IEnumerable<MatchInfo>> GetTaskList(GetTaskListCriteria criteria)
    {
        return (await dynamicRepository.FetchAsync<MatchInfo>(criteria)).ToArray();
    }
}