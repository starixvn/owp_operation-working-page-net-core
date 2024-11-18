using Fanex.Data.Repository;
using OWP.Models;
using OWP.Criteria;
using Fanex.NAP.Client.Services;
using Fanex.NAP.Client.Configuration;
using Newtonsoft.Json;
namespace OWP.Services;

public interface UserRepository
{
    Task<User> GetUserInfoById(int userId);

    Task<IList<WeeklyWorkingShiftUser>> GetAllUsers();
}

public class UserRepositoryImp(
    IDynamicRepository repository,
    IUserService userService,
    INapSettings napSettings) : UserRepository
{
    public async Task<User> GetUserInfoById(int userId)
    {
        var criteria = new GetUserInfoByIdCriteria
        {
            UserId = userId
        };
        return await repository.GetAsync<User>(criteria);
    }

    public async Task<IList<WeeklyWorkingShiftUser>> GetAllUsers()
    {
        var allUserActive = await userService.GetBySiteAsync(napSettings.SiteId, true);
        var criteria = new GetAllUserCriteria
        {
            AllUserJson = JsonConvert.SerializeObject(allUserActive.Select(u => new
            {
                UserId = u.Id,
                IsActive = true
            }))
        };
        return (await repository.FetchAsync<WeeklyWorkingShiftUser>(criteria)).ToList();
    }
}
