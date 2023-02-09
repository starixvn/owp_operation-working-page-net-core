using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using OWP.ProductAssignmentReference.Criteria;
using OWP.ProductAssignmentReference.Models;

namespace OWP.ProductAssignmentReference;

public interface ProductAssignmentRepository
{
    Task<IList<UserInfo>> QueryUser(QueryUserInfo queryInfo);

    Task UpdateUserInfo(UserInfoEditor info);

    Task<IEnumerable<ReferenceResource>> GetResourceByName(string name);
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
            Section = queryInfo.Section,
            SiteId = napSettings.SiteId
        };
        return (await dynamicRepository.FetchAsync<UserInfo>(criteria)).ToList();
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

    public async Task<IEnumerable<ReferenceResource>> GetResourceByName(string name)
        => await dynamicRepository.FetchAsync<ReferenceResource>(
            new GetRefResourceByNameCriteria
            {
                RefName = name
            });
}