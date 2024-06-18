using OWP.WorkingShift.Common.Criteria;
using OWP.WorkingShift.Common.Models;

namespace OWP.WorkingShift.Common.Repositories;

public interface WorkingShiftOrderRepository
{
    Task<ResultDataObject> UpdateEmployeeWorkingShiftOrder(UpdateEmployeeWorkingShiftOrderModel model);
}

public class WorkingShiftOrderRepositoryImp : WorkingShiftOrderRepository
{
    private readonly IMapper mapper;
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession currentSession;

    public WorkingShiftOrderRepositoryImp(IMapper mapper, IDynamicRepository dynamicRepository, INapSession currentSession)
    {
        this.mapper = mapper;
        this.dynamicRepository = dynamicRepository;
        this.currentSession = currentSession;
    }

    public async Task<ResultDataObject> UpdateEmployeeWorkingShiftOrder(UpdateEmployeeWorkingShiftOrderModel model)
    {
        var criteria = mapper.Map<UpdateEmployeeWorkingShiftOrderCriteria>(model);
        criteria.ActorId = currentSession.UserId;

        await dynamicRepository.ExecuteAsync(criteria);

        return new ResultDataObject
        {
            Code = (int)ErrorCode.Ok
        };
    }
}
