namespace OWP.LiveDetail.Television;

using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using OWP.Extensions;

public interface TelevisionRepository
{
    Task<IEnumerable<Television>> GetAll();
}

public class TelevisionRepositoryImp : TelevisionRepository
{
    private readonly IDynamicRepository repository;

    public TelevisionRepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Television>> GetAll()
    {
        var criteria = new GetAllCriteria();
        criteria.ThrowExceptionIfInvalid();

        return await repository.FetchAsync<Television>(criteria);
    }
}