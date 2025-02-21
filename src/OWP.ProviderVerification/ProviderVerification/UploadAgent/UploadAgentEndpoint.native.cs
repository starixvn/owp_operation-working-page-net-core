using OWP.Helpers;

namespace OWP.ProviderVerification.ProviderVerification.UploadAgent;

public class UploadAgentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ProviderVerification/upload-agent/validate", async([FromServices]ISender sender,HttpRequest  httpRequest) =>
                {
                    if (!MultipartRequestHelper.IsMultipartContentType(httpRequest.ContentType))
                    {
                        return Results.StatusCode(StatusCodes.Status415UnsupportedMediaType);
                    }

                    if (httpRequest.Form.Files.Count != 1)
                    {
                        return Results.StatusCode(StatusCodes.Status400BadRequest);
                    }

                    await using var ms = new MemoryStream();
                    await httpRequest.Form.Files[0].CopyToAsync(ms);
                    var result= await sender.Send(new UploadAgentRequest(ms));
                    return Results.Ok(result);

                }

            ).RequireAuthorization()
        .WithName("UploadAgentValidator")
            .Produces<Unit>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("validate agent file")
            .WithDescription("validate agent file");
    }
}