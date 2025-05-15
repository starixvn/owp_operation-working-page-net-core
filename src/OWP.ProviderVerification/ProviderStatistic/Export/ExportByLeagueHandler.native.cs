namespace OWP.ProviderVerification.ProviderStatistic.Export;

public record QueryByLeagueRequest(DateTime FromDate, DateTime ToDate) : Query<ExcelPackage>{}


public class QueryByLeagueHandler(IDynamicRepository repository) : QueryHandler<QueryByLeagueRequest, ExcelPackage>
{
   

    public async Task<ExcelPackage> Handle(QueryByLeagueRequest request, CancellationToken cancellationToken)
    {
        
        // TODO
        throw new NotImplementedException();
    }
}