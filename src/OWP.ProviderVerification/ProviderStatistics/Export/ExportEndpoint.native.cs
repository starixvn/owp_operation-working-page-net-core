namespace OWP.ProviderVerification.ProviderStatistics.Export;

public class ExportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ProviderStatistics/export/by-day", async (DateTime fromDate, DateTime toDate, [FromServices] ISender sender) =>
        {
            using var excelPackage = await sender.Send(new ExportByDayRequest(fromDate, toDate, "ProviderStatistic.xlsx"));
            var fileName = $"Monthly_Total_Summaries_{fromDate:yyyyMMMMdd}_to_{toDate:yyyyMMMMdd}.xlsx";
            var bytes = await excelPackage.GetAsByteArrayAsync();
            return Results.File(bytes, "application/octet-stream", fileName);
        }
            ).RequireAuthorization()
            .WithName("ExportProviderStatisticByDay")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Export provider statistic by day")
            .WithDescription("Export provider statistic by day");

        app.MapGet("/api/ProviderStatistics/export/by-league", async (DateTime fromDate, DateTime toDate, [FromServices] ISender sender) =>
        {
            using var excelPackage = await sender.Send(new ExportByLeagueRequest(fromDate, toDate, "ProviderStatistic.xlsx"));
            var fileName = $"Soccer_Provider_Usage_Summaries_By_Leagues_{fromDate:yyyyMMMMdd}_to_{toDate:yyyyMMMMdd}.xlsx";
            var bytes = await excelPackage.GetAsByteArrayAsync();
            return Results.File(bytes, "application/octet-stream", fileName);
        }
            ).RequireAuthorization()
            .WithName("ExportProviderStatisticByLeague")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Export provider statistic by league")
            .WithDescription("Export provider statistic by league");
    }
}
