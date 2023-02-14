namespace OWP.ProductAssignmentReference;

using Criteria;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using OWP.Models;

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

    public ProductAssignmentRepositoryImp(
        IDynamicRepository dynamicRepository,
        INapSettings napSettings)
    {
        this.dynamicRepository = dynamicRepository;
        this.napSettings = napSettings;
    }

    public async Task<IList<UserInfo>> QueryUser(QueryUserInfo queryInfo)
    {
        var criteria = new UserInfoCriteria
        {
            RoleId = queryInfo.RoleId,
            User = queryInfo.User,
                SectionId = queryInfo.SectionId,
                LocationId = queryInfo.LocationId,
                SiteId = napSettings.SiteId,

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