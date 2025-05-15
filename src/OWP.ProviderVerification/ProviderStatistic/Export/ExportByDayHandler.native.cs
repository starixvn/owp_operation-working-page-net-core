
using OWP.ProviderVerification.ProviderStatistic.Criteria;
using OWP.ProviderVerification.ProviderStatistic.Models;

namespace OWP.ProviderVerification.ProviderStatistic.Export;

public record QueryByDayRequest(DateTime FromDate, DateTime ToDate) : Query<ExcelPackage>{}


public class QueryByDayHandler(IDynamicRepository repository) : QueryHandler<QueryByDayRequest, ExcelPackage>
{
   

    public async Task<ExcelPackage> Handle(QueryByDayRequest request, CancellationToken cancellationToken)
    {
        var provider = await GetProviderFromDb(request);
        var providerStatistic=CalculateProviderStatistic(provider);
        // TODO
        throw new NotImplementedException();
    }

    private async Task<IList<ProviderStatisticDb>> GetProviderFromDb(QueryByDayRequest request)
    {
        return (await repository.FetchAsync<ProviderStatisticDb>(new SumProviderByDayCriteria
        {
            FromDate = request.FromDate,
            ToDate = request.ToDate
        })).ToList();

    }

    private static IList<ProviderStatisticInfo> CalculateProviderStatistic(IList<ProviderStatisticDb> providerSource)
    {
        var providerMapEventDate= providerSource.GroupBy(x => x.EventDate)
            .ToDictionary(x => x.Key, x => x);
        var providerStatistic = providerMapEventDate.Select(provider => new ProviderStatisticInfo()
            {
                _365Gv = provider.Value.FirstOrDefault(x => x.AgentId == 1)?.Count,
                Betrada = provider.Value.FirstOrDefault(x => x.AgentId == 2)?.Count,
                Bg = provider.Value.FirstOrDefault(x => x.AgentId == 3)?.Count,
                Fc = provider.Value.FirstOrDefault(x => x.AgentId == 4)?.Count,
                Img = provider.Value.FirstOrDefault(x => x.AgentId == 5)?.Count,
                Rb2 = provider.Value.FirstOrDefault(x => x.AgentId == 6)?.Count,
                Rts = provider.Value.FirstOrDefault(x => x.AgentId == 8)?.Count,
                Slr = provider.Value.FirstOrDefault(x => x.AgentId == 9)?.Count,
                Woo = provider.Value.FirstOrDefault(x => x.AgentId == 10)?.Count,
                Streaming = provider.Value.First(x => x.AgentId == 11)?.Count,
                LeagueOrDay = provider.Key.ToString("MMM-dd (ddd)")
        })
            .ToList();
        return providerStatistic.OrderBy(p => p.EventDate).ToList();
    }
}