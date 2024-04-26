
namespace OWP.NewLeagueSetting.Data;

public class InitDataEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("new-league", async (HttpRequest request, ISender sender) =>
        {
            await using var ms = new MemoryStream();
            await request.Form.Files[0].CopyToAsync(ms);
            return await sender.Send(new InitDataRequest(ms));
        }).RequireAuthorization().Accepts<HttpRequest>("multipart/form-data");
    }
}