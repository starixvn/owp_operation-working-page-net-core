
namespace OWP.ProviderVerification.ProviderVerification.Export;
public class ExportEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ProviderVerification/export/difference", async (DateTime eventDate, string sessionId, [FromServices] ISender sender) =>
        {
                    using var excelPackage = await sender.Send(new ExportCheckDifferenceRequest(eventDate, sessionId, "ProviderVerificationDifference.xlsx"));
                    var fileName = $"ProviderVerification - CheckDifferenceProviderId {eventDate:ddMMMMyyyy}.xlsx";
                    var bytes = await excelPackage.GetAsByteArrayAsync();
                    return Results.File(bytes, "application/octet-stream", fileName);
        }
            ).RequireAuthorization()
            .WithName("ExportDifference")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Export difference from adminsite provider match id with agent file")
            .WithDescription("Export difference from adminsite provider match id with agent file");

        app.MapGet("/api/ProviderVerification/export", async (DateTime startDate, DateTime endDate,
                    bool onlyShowIsMismatchEvent, string confirmProvider, string lSTBookProvider,
                    [FromServices] ISender sender) =>
                {
                    using var excelPackage = await sender.Send(new ExportRequest(startDate, endDate, onlyShowIsMismatchEvent, confirmProvider, lSTBookProvider, "ProviderVerification.xlsx"));
                    var fileName = $"ProviderVerification {DateTime.Today:ddMMMMyyyy}.xlsx";
                    var bytes = await excelPackage.GetAsByteArrayAsync();
                    return Results.File(bytes, "application/octet-stream", fileName);
                }
            ).RequireAuthorization()
            .WithName("Export")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Export provider match id")
            .WithDescription("Export provider match id");
    }
}
