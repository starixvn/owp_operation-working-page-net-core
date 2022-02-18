namespace OWP.RecipientGroup.Services
{
    using Criteria;
    using Fanex.Data.Repository;
    using Handlers;
    using Models;

    public interface CommandRepository
    {
        Task<IEnumerable<RecipientGroupModel>> GetRecipientGroup();

        Task SaveRecipientGroup(SaveRecipientGroupRequest model);

        Task InsertRecipientToMessageReader(InsertMessageReaderCriteria criteria);

        Task RemoveRecipientToMessageReader(RemoveMessageReaderCriteria criteria);
    }

    public class CommandRepositoryImp : CommandRepository
    {
        private readonly IDynamicRepository repository;

        public CommandRepositoryImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<RecipientGroupModel>> GetRecipientGroup()
        {
            var criteria = new GetRecipientGroupCriteria();

            return await repository.FetchAsync<RecipientGroupModel>(criteria);
        }

        public async Task InsertRecipientToMessageReader(InsertMessageReaderCriteria criteria)
            => await repository.ExecuteAsync(criteria);

        public async Task RemoveRecipientToMessageReader(RemoveMessageReaderCriteria criteria)
            => await repository.ExecuteAsync(criteria);

        public async Task SaveRecipientGroup(SaveRecipientGroupRequest model)
        {
            var criteria = new SaveRecipientGroupCommand
            {
                GroupId = model.GroupId,
                GroupName = model.GroupName,
                Recipients = string.Join(',', model.Recipients.Select(x => x.RecipientId))
            };

            await repository.ExecuteAsync(criteria);
        }
    }
}