namespace OWP.WorkingShift.Repositories
{
    public interface WorkingShiftDefaultRepository
    {
        Task<ResultDataObject> UpdateDefaultShift(UpdateDefaultShiftModel model);

        Task<IList<WorkingShiftDefaultModel>> QueryByUser(string userIds);
    }

    public class WorkingShiftDefaultRepositoryImp: WorkingShiftDefaultRepository
    {
        private readonly IMapper mapper;
        private readonly IDynamicRepository dynamicRepository;
        private readonly INapSession currentSession;

        public WorkingShiftDefaultRepositoryImp(IMapper mapper, IDynamicRepository dynamicRepository, INapSession currentSession)
        {
            this.mapper = mapper;
            this.dynamicRepository = dynamicRepository;
            this.currentSession = currentSession;
        }

        public async Task<ResultDataObject> UpdateDefaultShift(UpdateDefaultShiftModel model)
        {
            var criteria = mapper.Map<UpdateDefaultShiftCriteria>(model);
            criteria.Actor = currentSession.UserId;
            
            await dynamicRepository.ExecuteAsync(criteria);
            return ResultDataObject.Ok;
        }

        public async Task<IList<WorkingShiftDefaultModel>> QueryByUser(string userIds)
        {
            var criteria = new QueryWorkingShiftDefaultByUserCriteria
            {
                UserIds = userIds
            };
            
            return (await dynamicRepository.FetchAsync<WorkingShiftDefaultModel>(criteria)).ToList();
        }
    }
}
