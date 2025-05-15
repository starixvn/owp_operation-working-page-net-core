

using Fanex.NAP.Client;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Services;
using Microsoft.AspNetCore.Http;


namespace OWP.Application.Authorize;
public interface AuthorizeService
{
    Task<AuthorizeResult> Authorize(AuthorizeModel model);
}

public class AuthorizeServiceImp(
    INapSession session,
    IUserService userService,
    INapSettings napSettings,
    IHttpContextAccessor httpContextAccessor)
    : AuthorizeService
{
    public Task<AuthorizeResult> Authorize(AuthorizeModel model)
    {
        var policy = userService.GetPermission(session.UserId, napSettings.SiteId, model.FunctionKey);
        
        var result = new AuthorizeResult
        {
            UserId = session.UserId,
            Username = session.UserName,
            HasRead = policy >= 1,
            HasWrite = policy == 2,
            SiteId = napSettings.SiteId,
            RawPassword = httpContextAccessor.HttpContext?.Session.GetString("RawPassword")
        };
        return Task.FromResult(result);
    }
}
