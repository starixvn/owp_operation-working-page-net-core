namespace OWP.Services;

using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Models.ValueObject;
using Fanex.NAP.Client.Services;
using Fanex.NAP.Core.Business;
using Models;

public interface PermissionService
{
    bool HasRight(string functionKey, Constant.Policy policy);

    IList<FunctionPolicy> GetPermissions(string parentFunctionKey);

    PermissionModel GetPermissionModel(string functionKey);
}

public class PermissionServiceImp : PermissionService
{
    private readonly IUserService userService;

    private readonly INapSession currentSession;

    private readonly INapSettings napSettings;

    public PermissionServiceImp(IUserService userService, INapSession currentSession, INapSettings napSettings)
    {
        this.userService = userService;
        this.currentSession = currentSession;
        this.napSettings = napSettings;
    }

    public bool HasRight(string functionKey, Constant.Policy policy)
    {
        if (currentSession.UserId <= 0)
        {
            return false;
        }

        var functionPolicy = userService.GetPermission(
            currentSession.UserId,
            napSettings.SiteId,
            functionKey);
        return functionPolicy >= (int)policy;
    }

    public IList<FunctionPolicy> GetPermissions(string parentFunctionKey)
    {
        if (currentSession.UserId <= 0)
        {
            return new List<FunctionPolicy>();
        }

        return userService.GetPermissions(
            currentSession.UserId,
            napSettings.SiteId,
            parentFunctionKey,
            napSettings.Environment).ToList();
    }

    public PermissionModel GetPermissionModel(string functionKey)
    {
        if (currentSession.UserId <= 0)
        {
            return new PermissionModel();
        }

        var functionPolicy = userService.GetPermission(
            currentSession.UserId,
            napSettings.SiteId,
            functionKey);
        return new PermissionModel(functionPolicy >= (int)Constant.Policy.Read, functionPolicy >= (int)Constant.Policy.Host);
    }
}