using AutoMapper;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Services;
using Newtonsoft.Json;
using OWP.Criteria;
using OWP.Extensions;

namespace OWP.LiveDetail.User;

public interface UserRepository
{
    Task<IEnumerable<UserSuggestionEntity>> GetSuggestion(GetSuggestionModel model);

    Task<IEnumerable<UserInfoModel>> GetAll();

    Task<UserInfoModel> GetUserInfo(string name);
}

public class UserRepositoryImp(
    IDynamicRepository repository,
    INapSettings napSettings,
    IUserService userService) : UserRepository
{
    public async Task<IEnumerable<UserSuggestionEntity>> GetSuggestion(GetSuggestionModel model)
    {
        var allUserActive = await userService.GetBySiteAsync(napSettings.SiteId, null);
        var mapper = new MapperConfiguration(cfg => cfg.CreateMap<GetSuggestionModel, GetSuggestionCriteria>().ForAllMembers(opt => opt.Condition(src => src != null))).CreateMapper();

        var criteria = mapper.Map<GetSuggestionCriteria>(model);
        criteria.AllUserJson = JsonConvert.SerializeObject(allUserActive.Select(u => new
        {
            UserId = u.Id

        }));
        criteria.ThrowExceptionIfInvalid();
        return await repository.FetchAsync<UserSuggestionEntity>(criteria);
    }

    public async Task<IEnumerable<UserInfoModel>> GetAll()
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
        criteria.ThrowExceptionIfInvalid();
        return await repository.FetchAsync<UserInfoModel>(criteria);
    }

    public async Task<UserInfoModel> GetUserInfo(string name)
    {
        var criteria = new GetInfoByNameCriteria
        {
            Name = name
        };
        criteria.ThrowExceptionIfInvalid();
        return await repository.GetAsync<UserInfoModel>(criteria);
    }
}