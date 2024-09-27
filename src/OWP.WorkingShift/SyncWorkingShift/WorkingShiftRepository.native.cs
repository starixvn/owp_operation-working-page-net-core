
using Fanex.NAP.Client.Services;
using Newtonsoft.Json;
using OWP.WorkingShift.DailyWorkingShift.Criteria;
using WorkingShiftModel = OWP.WorkingShift.SyncWorkingShift.Models.WorkingShiftModel;

namespace OWP.WorkingShift.SyncWorkingShift;

public interface WorkingShiftRepository
{
    Task<IEnumerable<WorkingShiftModel>> Query(QueryWorkingShiftRequest request);

    Task<IEnumerable<WorkingShiftHRPPModel>> QueryWorkingShiftHRPP(QueryWorkingShiftToHRPPRequest request);
}

public class WorkingShiftRepositoryImp : WorkingShiftRepository
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSettings napSettings;
    private readonly IUserService userService;

    public WorkingShiftRepositoryImp(IDynamicRepository dynamicRepository, INapSettings napSettings, IUserService userService)
    {
        this.dynamicRepository = dynamicRepository;
        this.napSettings = napSettings;
        this.userService = userService;
    }

    public async Task<IEnumerable<WorkingShiftModel>> Query(QueryWorkingShiftRequest request)
    {
        var allUserActive = (await userService.GetBySiteAsync(napSettings.SiteId, true)).ToList();
       
        return await dynamicRepository.FetchAsync<WorkingShiftModel>(new GetEmployeeWorkingShiftCriteria
        {
            FromDate = request.From,
            ToDate = request.To,
            AllUserJson = JsonConvert.SerializeObject(allUserActive.Select(u => new
            {
                UserId = u.Id,
                IsActive = true
            }))
        });
    }


    public async Task<IEnumerable<WorkingShiftHRPPModel>> QueryWorkingShiftHRPP(QueryWorkingShiftToHRPPRequest request)
    {
        var allUserActive = (await userService.GetBySiteAsync(napSettings.SiteId, true)).ToList();

        return await dynamicRepository.FetchAsync<WorkingShiftHRPPModel>(new GetEmployeeWorkingShiftHRPPCriteria
        {
            FromDate = request.From,
            ToDate = request.To,
            AllUserJson = JsonConvert.SerializeObject(allUserActive.Select(u => new
            {
                UserId = u.Id,
                IsActive = true
            }))
        });
    }
}