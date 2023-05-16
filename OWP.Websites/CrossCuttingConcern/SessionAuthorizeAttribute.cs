using Fanex.NAP.Client.AspNetCore.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OWP.Websites.CrossCuttingConcern;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class SessionAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter, IFilterMetadata
{
    public void OnAuthorization(AuthorizationFilterContext filterContext)
    {
        if (SkipAuthorization(filterContext))
        {
            return;
        }

        if (!IsAuthenticated(filterContext))
        {
            return;
        }

        if (!SessionIsValid())
        {
            RequestHelper.CloseConnection(filterContext);
        }
    }

    private static bool SkipAuthorization(AuthorizationFilterContext filterContext)
    {
        if (filterContext.ActionDescriptor.EndpointMetadata.Any((object em) => em.GetType() == typeof(AllowAnonymousAttribute)))
        {
            return true;
        }

        if ((filterContext.ActionDescriptor as ControllerActionDescriptor)?.EndpointMetadata.Any((object em) => em.GetType() == typeof(AllowAnonymousAttribute)) ?? false)
        {
            return true;
        }

        return false;
    }

    private static bool IsAuthenticated(AuthorizationFilterContext filterContext)
    {
        var identity = filterContext?.HttpContext?.User?.Identity;

        return identity != null && identity.IsAuthenticated;
    }

    protected bool SessionIsValid()
    {
        HttpContextAccessor accessor = new HttpContextAccessor();
        NapSession napSession = new NapSession(accessor);
        return !string.IsNullOrEmpty(napSession.UserName) && napSession.UserId > 0;
    }
}


internal static class RequestHelper
{
    public static void CloseConnection(AuthorizationFilterContext context)
    {
        if (IsAjaxRequest(context.HttpContext.Request))
        {
            HttpResponse response = context.HttpContext.Response;
            string text = "<center><b>You are not authorized to access the requested resource!</b></center>";
            response.StatusCode = 403;
            response.WriteAsync(text);
        }
        else
        {
            context.HttpContext.SignOutAsync("Cookies");
        }
    }

    public static bool IsAjaxRequest(HttpRequest request)
    {
        return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) || string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
    }
}