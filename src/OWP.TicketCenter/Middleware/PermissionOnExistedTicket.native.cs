using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Middleware;

public class PermissionOnExistedTicket : IAsyncActionFilter
{
    private readonly PermissionService permissionService;

    public PermissionOnExistedTicket(PermissionService permissionService)
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
            var hasExistedTicket =
                await permissionService.HasExistedTicket(model.TicketId);

            if (hasExistedTicket)
            {
                await next();
            }
            else
            {
                context.HttpContext.Response.StatusCode = 203;
                context.Result = new ContentResult
                {
                    Content = "This ticket was deleted, and cannot view any more"
                };
            }

            return;
        }

        await next();
    }
}