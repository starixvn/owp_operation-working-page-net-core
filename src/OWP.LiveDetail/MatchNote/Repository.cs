namespace OWP.LiveDetail.MatchNote
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fanex.Data.Repository;
    using Fanex.NAP.Client.Helpers.Web;
    using OWP.Extensions;

    public interface MatchNoteRepository
    {
        Task<IEnumerable<MatchNoteModel>> Query(QueryModel model);

        Task<CreateResult> Create(CreateModel model);

        Task<string> Update(UpdateModel model);

        Task<string> Delete(DeleteModel model);
    }

    public class MatchNoteRepositoryImp : MatchNoteRepository
    {
        private readonly IMapper mapper;
        private readonly IDynamicRepository repository;
        private readonly INapSession session;

        public MatchNoteRepositoryImp(IMapper mapper, IDynamicRepository repository, INapSession session)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.session = session;
        }

        public async Task<IEnumerable<MatchNoteModel>> Query(QueryModel model)
        {
            var criteria = mapper.Map<QueryCriteria>(model);
            criteria.ThrowExceptionIfInvalid();
            return await repository.FetchAsync<MatchNoteModel>(criteria);
        }

        public async Task<CreateResult> Create(CreateModel model)
        {
            var criteria = mapper.Map<CreateCriteria>(model);
            criteria.UserId = session.UserId;
            criteria.ThrowExceptionIfInvalid();
            await repository.ExecuteCommandAsync(criteria);
            return new CreateResult
            {
                SessionId = criteria.SessionId,
                MatchNoteId = criteria.MatchNoteId,
                Username = session.UserName
            };
        }

        public async Task<string> Update(UpdateModel model)
        {
            var criteria = mapper.Map<UpdateCriteria>(model);
            criteria.UserId = session.UserId;
            criteria.ThrowExceptionIfInvalid();
            await repository.ExecuteCommandAsync(criteria);
            return criteria.SessionId;
        }

        public async Task<string> Delete(DeleteModel model)
        {
            var criteria = mapper.Map<DeleteCriteria>(model);
            criteria.UserId = session.UserId;
            criteria.ThrowExceptionIfInvalid();
            await repository.ExecuteCommandAsync(criteria);
            return criteria.SessionId;
        }
    }
}