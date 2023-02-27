using Fanex.Data.Repository;
using OWP.Extensions;

namespace OWP.LiveDetail.OtherProvider;

public interface OtherProviderRepository
{
    Task<IEnumerable<OtherProvider>> GetAll();
}

public class OtherProviderRepositoryImp : OtherProviderRepository
{
    private readonly IDynamicRepository repository;

    public OtherProviderRepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<OtherProvider>> GetAll()
    {
        var criteria = new GetAllCriteria();
        criteria.ThrowExceptionIfInvalid();
        return await repository.FetchAsync<OtherProvider>(criteria);
    }
}