using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using OWP.AccountSetting.Profile.Criteria;

namespace OWP.AccountSetting.Profile;

public interface ProfileRepository
{
    Task<(string, string)> QueryUserProfileInfo();
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

    public async Task<(string, string)> QueryUserProfileInfo()
    {
        var criteria = new GetUserProfileCriteria { UserId = session.UserId };
        var result = await repository.FetchAsync<(string, string)>(criteria);

        return result.FirstOrDefault();
    }
}
