using OWP.PrematchAssignment.Models;

namespace OWP.PrematchAssignment.AutoAssignment.Services;

public interface PreprocessDataService
{
    QueryPreMatchLeagueViewModel Handle(QueryPreMatchLeagueViewModel model);
}

public class PreprocessDataServiceImp(
    ExcludeUnSupportedLeaguesService excludeService,
    GroupAndSortLeaguesService groupService) : PreprocessDataService
{
    public QueryPreMatchLeagueViewModel Handle(QueryPreMatchLeagueViewModel model)
    {
        //excludeService
        var filteredModel = excludeService.Handle(model);

        //groupService

        return filteredModel;
    }
}
