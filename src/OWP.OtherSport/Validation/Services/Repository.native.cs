namespace OWP.OtherSport.Validation.Services;

using Criteria;
using Fanex.Data.Repository;
using Models;

public interface Repository
{
    Task<List<Trader>> GetTraders(List<string> traderNames);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository repository;

    public RepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Trader>> GetTraders(List<string> traderNames)
    {
        var traderNamesInString = string.Join(",", traderNames);
        if (string.IsNullOrEmpty(traderNamesInString))
        {
            return new List<Trader>();
        }

        var criteria = new TraderByUsernameQueryCriteria
        {
            Username = traderNamesInString
        };

        return (await repository.FetchAsync<Trader>(criteria)).ToList();
    }
}