using Fanex.Data.Repository;
using OWP.TicketSetting.TicketEvent.Criteria;
using OWP.TicketSetting.TicketEvent.Handlers;
using OWP.TicketSetting.TicketEvent.Models;

namespace OWP.TicketSetting.TicketEvent;

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
            OwpMatchId = request.OwpMatchId
        })).ToList();

    public async Task<IEnumerable<TicketAssignees>> QueryAssigneesByLeagueId(int owpMatchId)
        => (await repository.FetchAsync<TicketAssignees>(new QueryAssigneesLgaCriteria
        {
            OwpMatchId = owpMatchId
        })).ToList();
}