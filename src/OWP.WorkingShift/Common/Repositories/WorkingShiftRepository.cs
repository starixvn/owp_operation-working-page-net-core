
using Fanex.NAP.Client;
using WorkingShiftModel = OWP.WorkingShift.Common.Models.WorkingShiftModel;

namespace OWP.WorkingShift.Common.Repositories;

public interface WorkingShiftRepository
{
    Task<ResultDataObject> CopyWholeShifts(CopyWholeShiftsModel model);

    Task<ResultDataObject> DeleteWholeShifts(DeleteWholeShiftsModel model);

    Task<ResultDataObject> RemoveSingleShift(RemoveSingleShiftModel model);

    Task<ResultDataObject> CopySingleShift(CopySingleShiftModel model);

    Task<ResultDataObject> UpdateWorkingShiftCode(UpdateWorkingShiftCodeModel model);

    Task<ResultDataObject> CopyWorkingShiftWeeklyPrevious(CopyWorkingShiftWeeklyPreviousModel model);

    Task<IList<WorkingShiftModel>> QueryByUser(WorkingShiftQueryByUserModel model);
}

public class WorkingShiftRepositoryImp : WorkingShiftRepository
{
    private readonly IMapper mapper;
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession currentSession;

    public WorkingShiftRepositoryImp(IMapper mapper, IDynamicRepository dynamicRepository, INapSession currentSession)
    {
        this.mapper = mapper;
        this.dynamicRepository = dynamicRepository;
        this.currentSession = currentSession;
    }

    public async Task<ResultDataObject> CopyWholeShifts(CopyWholeShiftsModel model)
    {
        var criteria = mapper.Map<CopyWholeShiftsCriteria>(model);
        criteria.ActorId = currentSession.UserId;

        await dynamicRepository.ExecuteAsync(criteria);
        return ResultDataObject.Ok;
    }

    public async Task<ResultDataObject> DeleteWholeShifts(DeleteWholeShiftsModel model)
    {
        var criteria = mapper.Map<DeleteWholeShiftsCriteria>(model);
        criteria.ActorId = currentSession.UserId;

        await dynamicRepository.ExecuteAsync(criteria);
        return ResultDataObject.Ok;
    }

    public async Task<ResultDataObject> CopySingleShift(CopySingleShiftModel model)
    {
        var criteria = mapper.Map<CopySingleShiftCriteria>(model);
        criteria.RefShiftId = model.RefShiftId > 0 ? model.RefShiftId : null;
        criteria.ActorId = currentSession.UserId;

        await dynamicRepository.ExecuteAsync(criteria);
        return ResultDataObject.Ok;
    }

    public async Task<ResultDataObject> RemoveSingleShift(RemoveSingleShiftModel model)
    {
        var criteria = mapper.Map<RemoveSingleShiftCriteria>(model);
        criteria.ActorId = currentSession.UserId;

        await dynamicRepository.ExecuteAsync(criteria);
        return ResultDataObject.Ok;
    }

    public async Task<ResultDataObject> UpdateWorkingShiftCode(UpdateWorkingShiftCodeModel model)
    {
        var criteria = new UpdateWorkingShiftCodeCriteria
        {
            ActorId = currentSession.UserId,
            EmployeeId = model.EmployeeId,
            ShiftId = model.ShiftId,
            ShiftDate = model.ShiftDate,
            RefShiftId = model.RefShiftId,
            IsOvertime = model.IsOvertime
        };

        await dynamicRepository.ExecuteAsync(criteria);
        return ResultDataObject.Ok;
    }

    public async Task<ResultDataObject> CopyWorkingShiftWeeklyPrevious(CopyWorkingShiftWeeklyPreviousModel model)
    {
        var criteria = mapper.Map<CopyWorkingShiftWeeklyPreviousCriteria>(model);
        criteria.ActorId = currentSession.UserId;

        await dynamicRepository.ExecuteAsync(criteria);
        return ResultDataObject.Ok;
    }

    public async Task<IList<WorkingShiftModel>> QueryByUser(WorkingShiftQueryByUserModel model)
    {
        var criteria = mapper.Map<QueryWorkingShiftByUserCriteria>(model);

        return (await dynamicRepository.FetchAsync<WorkingShiftModel>(criteria)).ToList();
    }
}