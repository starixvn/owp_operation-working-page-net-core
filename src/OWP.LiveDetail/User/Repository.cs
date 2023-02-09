namespace OWP.LiveDetail.User;

using AutoMapper;
using Extensions;
using Fanex.Data.Repository;

public interface UserRepository
{
    Task<IEnumerable<UserSuggestionEntity>> GetSuggestion(GetSuggestionModel model);

    Task<IEnumerable<UserInfoModel>> GetAll();

    Task<UserInfoModel> GetUserInfo(string name);
}

public class UserRepositoryImp : UserRepository
{
    private readonly IDynamicRepository repository;

    public UserRepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<UserSuggestionEntity>> GetSuggestion(GetSuggestionModel model)
    {
        var mapper = new MapperConfiguration(cfg => cfg.CreateMap<GetSuggestionModel, GetSuggestionCriteria>().ForAllMembers(opt => opt.Condition(src => src != null))).CreateMapper();

        var criteria = mapper.Map<GetSuggestionCriteria>(model);
        criteria.ThrowExceptionIfInvalid();
        return await repository.FetchAsync<UserSuggestionEntity>(criteria);
    }

    public async Task<IEnumerable<UserInfoModel>> GetAll()
    {
        var criteria = new GetUserListCriteria();
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