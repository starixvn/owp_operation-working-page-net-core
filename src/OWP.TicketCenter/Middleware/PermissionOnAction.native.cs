namespace OWP.TicketCenter.Middleware;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Services;

public class PermissionOnAction : IAsyncActionFilter
{
    private readonly PermissionService permissionService;

    public PermissionOnAction(PermissionService permissionService)
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
                await permissionService.HasPermission(
                    model.TicketId,
                    model.ActorId);

            if (hasPermission)
            {
                await next();
            }
            else
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new ContentResult
                {
                    Content = "You have been removed from the recipient list, and cannot view this ticket any more"
                };
            }
            return;
        }

        await next();
    }
}
