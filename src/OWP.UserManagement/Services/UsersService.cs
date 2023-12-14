using Fanex.NAP.Client.Helpers.Web;

namespace OWP.UserManagement.Services;

using System.Threading.Tasks;
using Models;
using OWP.Models;
using OWP.UserManagement.Criteria;

public interface UsersService
{
    Task<IList<User>> GetUserPagingAsync(QueryModel model);

    Task UpdateUserInfo(UpdateUserInfo info);
}

public class UsersServiceImp : UsersService
{
    private readonly ModelService modelService;
    private readonly Repository repository;
    private readonly INapSession currentSession;

    public UsersServiceImp(ModelService modelService, Repository repository, INapSession currentSession)
    {
        this.modelService = modelService;
        this.repository = repository;
        this.currentSession = currentSession;
    }

    public async Task<IList<User>> GetUserPagingAsync(QueryModel model)
    {
        model.Permission = modelService.BuildPermissionViewModel();
        return await repository.GetUserPagingAsync(model);
    }

    public async Task UpdateUserInfo(UpdateUserInfo info)
    {
        var criteria = new UpdateUserCriteria()
        {
            ColumnName = info.ColumnName,
            UserId = info.UserId,
            Value = info.Value,
            ActorId = currentSession.UserId
        };
        await repository.UpdateUserInfo(criteria);
        info.ErrCode = criteria.ErrCode;
        info.ErrMsg = criteria.ErrMsg;
    }

}