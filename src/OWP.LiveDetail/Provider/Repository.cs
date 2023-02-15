namespace OWP.LiveDetail.Provider;

using Extensions;
using Fanex.Data.Repository;

public interface ProviderRepository
{
    Task<IEnumerable<Provider>> GetAll();
}

public class ProviderRepositoryImp : ProviderRepository
{
    private readonly IDynamicRepository repository;

    public ProviderRepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Provider>> GetAll()
    {
        var criteria = new GetAllCriteria();
        criteria.ThrowExceptionIfInvalid();
        return await repository.FetchAsync<Provider>(criteria);
    }
}