
using OWP.NewLeagueSetting.Command;

namespace OWP.NewLeagueSetting.EndPoints;
public class NewLeagueSettingEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var groupEndpoint = app.MapGroup("api/NewLeagueSetting").RequireAuthorization();
        groupEndpoint.MapPost("/new-league", async (HttpRequest request, ISender sender) =>
        {
            await using var ms = new MemoryStream();
            await request.Form.Files[0].CopyToAsync(ms);
            return await sender.Send(new InitDataRequest(ms));
        });

        groupEndpoint.MapGet("/getAllListSettings", async (string league, ISender sender) =>
        {
            var result = await sender.Send(new QueryLeagueSettingRequest(league));
            return Results.Json(result);
        });

        groupEndpoint.MapGet("/getLeagueSettingDetail", async (int leagueSettingId, ISender sender) =>
        {
            var result = await sender.Send(new QueryLeagueSettingDetailRequest(leagueSettingId));
            return Results.Json(result);
        });

        groupEndpoint.MapGet("/get-league-setting-by-owpLeagueId", async (int owpLeagueId, ISender sender) =>
        {
            var result = await sender.Send(new GetLeagueSettingByLeagueIdRequest(owpLeagueId));
            return Results.Json(result);
        });

        groupEndpoint.MapPost("/editLeagueSetting", async (UpdateLeagueSettingRequest request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Json(result);
        });
    }
}
