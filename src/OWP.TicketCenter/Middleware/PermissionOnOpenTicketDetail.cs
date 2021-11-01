using Fanex.NAP.Client.Helpers.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Services;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.TicketCenter.Middleware
{
    public class PermissionOnOpenTicketDetail : IAsyncActionFilter
    {
        private readonly PermissionService permissionService;

        private readonly INapSession currentSession;

        public PermissionOnOpenTicketDetail(PermissionService permissionService, INapSession currentSession)
        {
            this.permissionService = permissionService;
            this.currentSession = currentSession;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context
                .ActionArguments
                .SingleOrDefault(p => p.Value is OnClickIssueNotificationRequest)
                .Value;

            if (param is OnClickIssueNotificationRequest model)
            {
                var hasPermission =
                    await permissionService.HasPermission(model.TicketId, currentSession.UserId);

                if (hasPermission)
                {
                    await next();
                }
                else
                {
                    context.HttpContext.Response.StatusCode = 400;
                    context.Result = new ContentResult
                    {
                        Content = "Can not delete last recipient"
                    };
                }
                return;
            }

            await next();
        }
    }
}