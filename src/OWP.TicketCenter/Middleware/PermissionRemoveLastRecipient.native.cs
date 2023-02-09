using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Middleware;

public class PermissionRemoveLastRecipient : IAsyncActionFilter
{
    private readonly PermissionService permissionService;

    public PermissionRemoveLastRecipient(PermissionService permissionService)
    {
        this.permissionService = permissionService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var param = context
            .ActionArguments
            .SingleOrDefault(p => p.Value is PermissionCheckedModel)
            .Value;

        if (param is PermissionCheckedModel model)
        {
            var hasPermission =
                await permissionService.HasPermissionRemoveLastRecipient(model.TicketId);

            if (hasPermission)
            {
                await next();
            }
            else
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new ContentResult
                {
                    Content = "Please select at least one recipient"
                };
            }
            return;
        }

        await next();
    }
}