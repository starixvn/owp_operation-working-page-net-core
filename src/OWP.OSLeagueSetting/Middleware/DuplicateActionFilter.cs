using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OWP.OSLeagueSetting.Models;
using OWP.OSLeagueSetting.Services;
using OWP.Services;

namespace OWP.OSLeagueSetting.Middleware;

public class DuplicateActionFilter : IAsyncActionFilter
{
    private readonly DuplicateSettingService duplicatedSettingService;

    public DuplicateActionFilter(DuplicateSettingService duplicatedSettingService)
    {
        this.duplicatedSettingService = duplicatedSettingService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var result = context.ActionArguments
            .SingleOrDefault(p => p.Value is SettingDetail)
            .Value;
        if (result is SettingDetail model)
        {
            var isDupLeagueSetting = await duplicatedSettingService.IsDuplicate(model);
            if (isDupLeagueSetting)
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new ContentResult
                {
                    Content = "A setting already exists. Please check again"
                };

                return;
            }

            await next();
        }
    }
}