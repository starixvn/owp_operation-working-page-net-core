
using Fanex.NAP.Client.Services;
using Newtonsoft.Json;

namespace OWP.WorkingShift.SyncWorkingShift;

public interface WorkingShiftRepository
{
    Task<IEnumerable<EmployeeWorkingShiftModel>> QueryWorkingShift(QueryEmployeeWorkingShiftRequest request);

    Task<IEnumerable<WorkingShiftHRPPModel>> QueryWorkingShiftHRPP(QueryWorkingShiftToHRPPRequest request);
}

public class WorkingShiftRepositoryImp(
    IDynamicRepository dynamicRepository,
    INapSettings napSettings,
    IUserService userService) : WorkingShiftRepository
{
    public async Task<IEnumerable<EmployeeWorkingShiftModel>> QueryWorkingShift(QueryEmployeeWorkingShiftRequest request)
    {
        var users = await userService.GetBySiteAsync(napSettings.SiteId, true);
        var criteria = new GetEmployeeWorkingShiftHRMCriteria
        {
            LocationCode = request.LocationCode,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            AllUserJson = JsonConvert.SerializeObject(users.Select(u => new
            {
                UserId = u.Id,
                IsActive = true
            }))
        };

        return await dynamicRepository.FetchAsync<EmployeeWorkingShiftModel>(criteria);
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