
using Fanex.NAP.Client;

namespace OWP.WorkingShift.Common.Repositories;

public interface WorkingShiftPublishRepository
{
    Task<ResultDataObject> PublishSection(PublishWeeklyWorkingShiftModel model);

    Task<IList<SectionPublishWorkingShiftModel>> GetSectionPublishWorkingShift(DateTime fromDate, DateTime toDate, string sectionIds);

    Task<IList<Models.WorkingShiftModel>> GetWeeklyWorkingShiftBySection(WorkingShiftQueryBySectionModel queryModel);
}

public class WorkingShiftPublishRepositoryImp : WorkingShiftPublishRepository
{
    private readonly IMapper mapper;
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession currentSession;

    public WorkingShiftPublishRepositoryImp(IMapper mapper, IDynamicRepository dynamicRepository, INapSession currentSession)
    {
        this.mapper = mapper;
        this.dynamicRepository = dynamicRepository;
        this.currentSession = currentSession;
    }

    public async Task<IList<SectionPublishWorkingShiftModel>> GetSectionPublishWorkingShift(DateTime fromDate, DateTime toDate, string sectionIds)
    {
        var criteria = new GetSectionWorkingShiftPublishByWeekCriteria
        {
            FromDate = fromDate,
            ToDate = toDate,
            SectionIds = sectionIds
        };


        return (await dynamicRepository.FetchAsync<SectionPublishWorkingShiftModel>(criteria)).ToList();
    }

    public async Task<ResultDataObject> PublishSection(PublishWeeklyWorkingShiftModel model)
    {
        var criteria = mapper.Map<PublishWeeklyWorkingShiftCriteria>(model);

        criteria.ActorId = currentSession.UserId;


        await dynamicRepository.ExecuteAsync(criteria);

        return new ResultDataObject
        {
            Code = (int)ErrorCode.Ok,
            Data = model
        };
    }
    public async Task<IList<Models.WorkingShiftModel>> GetWeeklyWorkingShiftBySection(WorkingShiftQueryBySectionModel queryModel)
    {
        var criteria = mapper.Map<QueryWorkingShiftBySectionCriteria>(queryModel);
        return (await dynamicRepository.FetchAsync<Models.WorkingShiftModel>(criteria)).ToList();
    }

}