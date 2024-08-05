using OWP.Constants;
using OWP.NewLeagueSetting.Command;
using OWP.NewLeagueSetting.EndPoints.Middleware;
using OWP.Services;

namespace OWP.NewLeagueSetting.EndPoints;
public class NewLeagueSettingEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var groupEndpoint = app.MapGroup("api/NewLeagueSetting").RequireAuthorization();
        app.MapPost("api/NewLeagueSetting/new-league", async (HttpRequest request, ISender sender) =>
        {
            await using var ms = new MemoryStream();
            await request.Form.Files[0].CopyToAsync(ms);
            return await sender.Send(new InitDataRequest(ms));
        }).Accepts<HttpRequest>("multipart/form-data")
            .AllowAnonymous();

        groupEndpoint.MapGet("/getAllListSettings", async (string league, bool isEqualLeague, ISender sender) =>
        {
            var result = await sender.Send(new QueryLeagueSettingRequest(league, isEqualLeague));
            return Results.Json(result);
        });

        groupEndpoint.MapGet("/getLeagueSettingDetail", async (int leagueSettingId, ISender sender) =>
        {
            var result = await sender.Send(new QueryLeagueSettingDetailRequest(leagueSettingId));
            return Results.Json(result);
        });

        groupEndpoint.MapPost("/update", async (UpdateLeagueSettingRequest request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Json(result);
        }).AddEndpointFilter<DuplicateActionFilter>();

        groupEndpoint.MapPost("/insert", async (InsertLeagueSettingRequest request, ISender sender, AccessCounter accessCounter) =>
        {
            await accessCounter.UpdateCounter(AccessCounterConst.AddNewLeagueSetting);
            var leagueSettingId = await sender.Send(request);
            return Results.Ok(new { leagueSettingId });
        }).AddEndpointFilter<DuplicateActionFilter>();

        groupEndpoint.MapPost("/remove", async (RemoveLeagueSettingRequest request, ISender sender) =>
        {
            await sender.Send(request);
            return Results.Ok();
        });
    }
}
