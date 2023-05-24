using Fanex.Data.Repository;

namespace OWP.ResourceManagement;

using Criteria;
using Models;
using System.Threading.Tasks;

public interface Repository
{
    Task<RefResourceModel> Insert(RefResourceModel model);

    Task<RefResourceModel> Update(RefResourceModel model);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;

    public RepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<RefResourceModel> Insert(RefResourceModel model)
    {
        var criteria = new InsertCriteria { RefName = model.RefName, Value = model.Value };

        await dynamicRepository.ExecuteAsync(criteria);
        model.Id = criteria.ResourceId;

        return model;
    }

    public async Task<RefResourceModel> Update(RefResourceModel model)
    {
        var criteria = new UpdateCriteria { ResourceId = model.Id, RefName = model.RefName, Value = model.Value };

        await dynamicRepository.ExecuteAsync(criteria);

        return model;
    }
}