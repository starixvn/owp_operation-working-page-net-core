using OWP.TicketEvent.Models;
namespace OWP.TicketEvent
{
    using Criteria;
    using Fanex.Data.Repository;

    public interface Repository
    {
        public Task<IList<TicketListModel>> GetTicketFromDb(int owpMatchId);

        public Task<IEnumerable<TicketAssignees>> QueryAssigneesByLeagueId(int owpMatchId);
    }

    public class RepositoryImp : Repository
    {
        private readonly IDynamicRepository repository;

        public RepositoryImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IList<TicketListModel>> GetTicketFromDb(int owpMatchId)
            => (await repository.FetchAsync<TicketListModel>(new GetTicketListCriteria
            {
                ActorId = 8,
                OwpMatchId = owpMatchId
            })).ToList();

        public async Task<IEnumerable<TicketAssignees>> QueryAssigneesByLeagueId(int owpMatchId)
            => (await repository.FetchAsync<TicketAssignees>(new QueryAssigneesLgaCriteria
            {
                OwpMatchId = owpMatchId
            })).ToList();
    }
}
