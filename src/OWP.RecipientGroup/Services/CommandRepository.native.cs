namespace OWP.RecipientGroup.Services
{
    using Criteria;
    using Fanex.Data.Repository;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public interface CommandRepository
    {
        Task<IEnumerable<TicketGroup>> GetRecipientGroup();
    }

    public class CommandRepositoryImp : CommandRepository
    {
        private readonly IDynamicRepository repository;

        public CommandRepositoryImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<TicketGroup>> GetRecipientGroup()
        {
            var criteria = new GetRecipientGroupCriteria();

            var result = await repository.FetchAsync<RecipientGroup>(criteria);

            return result.GroupBy(x => new { x.GroupId, x.GroupName }, (key, value) =>
                new TicketGroup
                {
                    GroupId = key.GroupId,
                    GroupName = key.GroupName,
                    Recipients = value.ToList()
                }
            );
        }
    }
}
