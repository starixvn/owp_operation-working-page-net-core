namespace OWP.ProviderVerification.ProviderStatistic.Export;
public class ExportEndpoint(IMediator sender) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ProviderStatistic/export/by-day", async (DateTime fromDate, DateTime toDate) =>
        {
            using var excelPackage = await sender.Send(new QueryByDayRequest(fromDate,toDate));
            // TODO

        }
            ).RequireAuthorization()
            .WithName("ExportProviderStatistic")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Export provider statistic by day")
            .WithDescription("Export provider statistic by day");

        app.MapGet("/api/ProviderStatistic/export/by-league", async (DateTime fromDate, DateTime toDate) =>
                {
                    using var excelPackage = await sender.Send(new QueryByLeagueRequest(fromDate, toDate));
                    // TODO

                }
            ).RequireAuthorization()
            .WithName("ExportProviderStatistic")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Export provider statistic by league")
            .WithDescription("Export provider statistic by league");
    }
}
