

using Fanex.NAP.Client;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Services;

namespace OWP.Application.Authorize;
public interface AuthorizeService
{
    Task<AuthorizeResult> Authorize(AuthorizeModel model);
}

public class AuthorizeServiceImp : AuthorizeService
{
    private readonly IUserService userService;
    private readonly INapSession session;
    private readonly INapSettings napSettings;

    public AuthorizeServiceImp(INapSession session, IUserService userService, INapSettings napSettings)
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
