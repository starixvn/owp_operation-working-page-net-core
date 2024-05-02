
using OWP.NewLeagueSetting.Data;
using OWP.NewLeagueSetting.Query;


namespace OWP.NewLeagueSetting.EndPoints;
public  class NewLeagueSettingEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("new-league", async (HttpRequest request, ISender sender) =>
        {
            await using var ms = new MemoryStream();
            await request.Form.Files[0].CopyToAsync(ms);
            return await sender.Send(new InitDataRequest(ms));
        }).RequireAuthorization().Accepts<HttpRequest>("multipart/form-data");

        app.MapGet("api/NewLeagueSetting/getAllListSettings", async (string league, ISender sender) =>
        {
            var result = await sender.Send(new QueryLeagueSettingRequest(league));
            return Results.Json(result);
        }).RequireAuthorization();
    }
}
