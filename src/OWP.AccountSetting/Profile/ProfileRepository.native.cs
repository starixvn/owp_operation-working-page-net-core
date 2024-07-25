using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using OWP.AccountSetting.Profile.Criteria;

namespace OWP.AccountSetting.Profile;

public interface ProfileRepository
{
    Task<(string, string)> QueryUserProfileInfo(int? userId = null );
}

public class ProfileRepositoryImp : ProfileRepository
{
    private readonly IDynamicRepository repository;
    private readonly INapSession session;

    public ProfileRepositoryImp(IDynamicRepository repository, INapSession session)
    {
        this.repository = repository;
        this.session = session;
    }

    public async Task<(string, string)> QueryUserProfileInfo(int? userId = null)
    {
        var criteria = new GetUserProfileCriteria { UserId = userId ?? session.UserId };
        var result = await repository.FetchAsync<UserProfile>(criteria);
        var firstResult = result.FirstOrDefault();
        return (firstResult.Username, firstResult.SavedFileName);
    }
}

public class UserProfile
{
    public string Username { get; set; }

    public string SavedFileName { get; set; }
}
