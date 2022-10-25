namespace OWP.LiveDetail.MatchSetting
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fanex.Data.Repository;
    using Fanex.NAP.Client.Helpers.Web;
    using OWP.Extensions;

    public interface MatchSettingRepository
    {
        Task<IEnumerable<MatchSetting>> Query(QueryModel model);

        Task<CreateResult> Create(CreateModel model);

        Task<ResponseResult> Update(UpdateModel model);

        Task<ResponseResult> Delete(DeleteModel model);

    }

    public class MatchSettingRepositoryImp : MatchSettingRepository
    {
        private readonly IMapper mapper;
        private readonly INapSession session;
        private readonly IDynamicRepository repository;

        public MatchSettingRepositoryImp(IMapper mapper, IDynamicRepository repository, INapSession session)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.session = session;
        }

        public async Task<IEnumerable<MatchSetting>> Query(QueryModel model)
        {
            var criteria = mapper.Map<QueryCriteria>(model);
            return await repository.FetchAsync<MatchSetting>(criteria);
        }

        public async Task<CreateResult> Create(CreateModel model)
        {
            var criteria = mapper.Map<CreateCriteria>(model);
            await repository.ExecuteAsync(criteria);
            var output = new CreateResult
            {
                Code = 0,
                Message = "OK",
                SessionId = criteria.SessionId
            };
            return await Task.FromResult(output);
        }

        public async Task<ResponseResult> Delete(DeleteModel model)
        {
            var criteria = mapper.Map<DeleteCriteria>(model);
            await repository.ExecuteAsync(criteria);
            var output = new ResponseResult
            {
                Code = 0,
                Message = "Ok",
                SessionId = criteria.SessionId
            };

            return await Task.FromResult(output);
        }

        public async Task<ResponseResult> Update(UpdateModel model)
        {
            var criteria = mapper.Map<UpdateCriteria>(model);
            criteria.UserId = session.UserId;
            var result = await repository.ExecuteCommandAsync(criteria);
            var output = new ResponseResult
            {
                Code = result.Code,
                Message = result.Message,
                SessionId = criteria.SessionId
            };

            return await Task.FromResult(output);
        }
    }
}