using Fanex.Data.Repository;
using OWP.OtherSport.Validation.Criteria;
using OWP.OtherSport.Validation.Models;

namespace OWP.OtherSport.Validation.Services;

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