using Fanex.Data.Repository;
using OWP.Extensions;

namespace OWP.LiveDetail.Agent;

public interface AgentRepository
{
    Task<IEnumerable<Agent>> GetAll();
}

public class AgentRepositoryImp : AgentRepository
{
    private readonly IDynamicRepository repository;

    public AgentRepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Agent>> GetAll()
    {
        var criteria = new GetAllCriteria();
        criteria.ThrowExceptionIfInvalid();

        return await repository.FetchAsync<Agent>(criteria);
    }
}