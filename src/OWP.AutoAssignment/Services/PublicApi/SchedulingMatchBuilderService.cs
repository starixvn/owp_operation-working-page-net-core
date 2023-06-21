using OWP.AutoAssignment.Models.PublicApi;

namespace OWP.AutoAssignment.Services.PublicApi;

public interface SchedulingMatchBuilderService
{
    Task<SchedulingMatch> GetMatchesInfo(DateTime eventDate);
}

public class SchedulingMatchBuilderServiceImp : SchedulingMatchBuilderService
{
    private readonly Repository repository;

    private readonly SabaMatchService sabaMatchService;

    private readonly RealMatchService realMatchService;

    public SchedulingMatchBuilderServiceImp(
        Repository repository,
        SabaMatchService sabaMatchService,
        RealMatchService realMatchService
    )
    {
        this.repository = repository;
        this.realMatchService = realMatchService;
        this.sabaMatchService = sabaMatchService;
    }

    public async Task<SchedulingMatch> GetMatchesInfo(DateTime eventDate)
    {
        var schedulingMatch = new SchedulingMatch();
        var matchQueries = await repository.GetMatchByDate(eventDate);

        var realMatchIds = SelectMatchIds(matchQueries.ToList(), false);
        var sabaMatchIds = SelectMatchIds(matchQueries.ToList(), true);

        var sabaMatch = await sabaMatchService.BuildSabaMatchInfo(sabaMatchIds);
        var realMatch = await realMatchService.BuildRealMatchInfo(realMatchIds);

        schedulingMatch.Saba.AddRange(sabaMatch);
        schedulingMatch.Real.AddRange(realMatch);
        return schedulingMatch;
    }

    internal IEnumerable<SchedulingMatch> BuildListSchedulingMatch(object mainMatches, object matches)
    {
        throw new NotImplementedException();
    }

    private IList<MatchQueried> SelectMatchIds(IEnumerable<MatchQueried> matchQueries, bool isSaba)
        => matchQueries
            .Where(m => m.IsSaba == isSaba)
            .ToList();
}