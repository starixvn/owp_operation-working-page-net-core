

using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Services;

namespace OWP.Application.Authorize;
public interface Service
{
    Task<AuthorizeResult> Authorize(AuthorizeModel model);
}

public class ServiceImp : Service
{
    private readonly IUserService userService;
    private readonly INapSession session;
    private readonly INapSettings napSettings;

    public ServiceImp(INapSession session, IUserService userService, INapSettings napSettings)
    {
        this.session = session;
        this.userService = userService;
        this.napSettings = napSettings;
    }

    public Task<AuthorizeResult> Authorize(AuthorizeModel model)
    {
        var policy = userService.GetPermission(session.UserId, napSettings.SiteId, model.FunctionKey);

        var result = new AuthorizeResult
        {
            UserId = session.UserId,
            Username = session.UserName,
            HasRead = policy >= 1,
            HasWrite = policy == 2,
            SiteId = napSettings.SiteId
        };
        return Task.FromResult(result);
    }
}
