using OWP.NewLeagueSetting.Service;

namespace OWP.NewLeagueSetting.EndPoints.Middleware;

public class DuplicateActionFilter(DuplicateSettingService duplicateSettingService) : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = context.Arguments.SingleOrDefault(p => p is LeagueSettingRequest);

        if (result is not LeagueSettingRequest model)
        {
            return await next(context);
        }

        var isDuplicate = await duplicateSettingService.GetDuplicatedSettingId(model);
        if (isDuplicate)
        {
            return Results.BadRequest(new
            {
                Message = "A setting already exists. Please check again",
                DuplicatedLeagueName = model.League
            });
        }

        return await next(context);
    }
}