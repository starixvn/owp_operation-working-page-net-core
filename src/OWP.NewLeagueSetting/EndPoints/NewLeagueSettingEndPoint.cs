
using OWP.NewLeagueSetting.Command;

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
        }).Accepts<HttpRequest>("multipart/form-data");

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

        groupEndpoint.MapPost("/editLeagueSetting", async (UpdateLeagueSettingRequest request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Json(result);
        });
    }
}
