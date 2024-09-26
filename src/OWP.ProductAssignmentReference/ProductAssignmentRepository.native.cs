using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Services;
using Newtonsoft.Json;
using OWP.Models;
using OWP.ProductAssignmentReference.Criteria;
using OWP.ProductAssignmentReference.Models;

namespace OWP.ProductAssignmentReference;

public interface ProductAssignmentRepository
{
    Task<IList<UserInfo>> QueryUser(QueryUserInfo queryInfo);

    Task UpdateUserInfo(UserInfoEditor info);

    Task<IList<ReferenceResource>> GetLevelBySectionId(int? sectionId);

    Task<IList<ReferenceResource>> GetMainBettypeBySectionId(int? sectionId);
}

public class ProductAssignmentRepositoryImp : ProductAssignmentRepository
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSettings napSettings;

    private readonly IUserService userService;

    public ProductAssignmentRepositoryImp(
        IDynamicRepository dynamicRepository,
        INapSettings napSettings,
        IUserService userService)
    {
        this.dynamicRepository = dynamicRepository;
        this.napSettings = napSettings;
        this.userService = userService;
    }

    public async Task<IList<UserInfo>> QueryUser(QueryUserInfo queryInfo)
    {
        var allUserActive = await userService.GetBySiteAsync(napSettings.SiteId, true);
        var criteria = new UserInfoCriteria
        {
            RoleId = queryInfo.RoleId,
            User = queryInfo.User,
            SectionId = queryInfo.SectionId,
            LocationId = queryInfo.LocationId,
            AllUserJson = JsonConvert.SerializeObject(allUserActive.Select(u => new
            {
                UserId = u.Id,
                IsActive = true
            }))
        };
        return (await dynamicRepository.FetchAsync<UserInfo>(criteria)).ToList();
    }

    public async Task<IList<ReferenceResource>> GetLevelBySectionId(int? sectionId)
    {
        var criteria = new GetLevelBySectionIdCriteria
        {
            SectionId = sectionId
        };

        return (await dynamicRepository.FetchAsync<ReferenceResource>(criteria)).ToList();
    }

    public async Task<IList<ReferenceResource>> GetMainBettypeBySectionId(int? sectionId)
    {
        var criteria = new GetMainBettypeBySectionIdCriteria
        {
            SectionId = sectionId
        };

        return (await dynamicRepository.FetchAsync<ReferenceResource>(criteria)).ToList();
    }

    public async Task UpdateUserInfo(UserInfoEditor info)
    {
        var userInfoEditorCriteria = new UserInfoEditorCriteria
        {
            ActorId = info.ActorId,
            Username = info.Username,
            UserId = info.UserId,
            Label = info.Label,
            Value = info.Value
        };
        await dynamicRepository.ExecuteAsync(userInfoEditorCriteria);
    }
}