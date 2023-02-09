using OWP.TicketEvent.Models;
namespace OWP.TicketEvent;

using Criteria;
using Fanex.Data.Repository;
using OWP.TicketEvent.Handlers;

public interface Repository
{
    public Task<IList<TicketListModel>> GetTicketFromDb(GetTicketListRequest request);

    public Task<IEnumerable<TicketAssignees>> QueryAssigneesByLeagueId(int owpMatchId);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository repository;

    public RepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }
    public async Task<IList<TicketListModel>> GetTicketFromDb(GetTicketListRequest request)
        => (await repository.FetchAsync<TicketListModel>(new GetTicketListCriteria
        {
            ActorId = request.ActorId,
            OwpMatchIds = request.OwpMatchId.ToString()
        })).ToList();

    public async Task<IEnumerable<TicketAssignees>> QueryAssigneesByLeagueId(int owpMatchId)
        => (await repository.FetchAsync<TicketAssignees>(new QueryAssigneesLgaCriteria
        {
            OwpMatchId = owpMatchId
        })).ToList();
}