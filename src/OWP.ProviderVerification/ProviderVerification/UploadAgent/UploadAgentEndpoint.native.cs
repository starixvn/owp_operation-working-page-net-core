
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
                    var result= await sender.Send(new UploadAgentValidationRequest(ms, Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture)));
                    return Results.Ok(result);

                }

            ).RequireAuthorization()
        .WithName("UploadAgentValidator")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Validate agent file")
            .WithDescription("Validate agent file");

        app.MapPost("/api/ProviderVerification/upload-agent/submit", async ([FromServices] ISender sender, SubmitAgentRequest request) =>
                {
                    var result = await sender.Send(request);
                    return Results.Ok(result);
                }

            ).RequireAuthorization()
            .WithName("SubmitAgent")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Submit agent file")
            .WithDescription("Submit agent file");
    }

   
}