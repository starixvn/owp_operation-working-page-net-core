namespace OWP.AssignmentUpload.SyncInfoSite;

using Fanex.Data.Repository;
using InfositeIdSync.Criteria;
using Models;

public interface SyncInfoSiteRepository
{
    Task<List<InfositeMatch>> QueryInfositeMatch(DateTime eventDate);
}

public class SyncInfoSiteRepositoryImp : SyncInfoSiteRepository
{
    private readonly IDynamicRepository repository;

    public SyncInfoSiteRepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<InfositeMatch>> QueryInfositeMatch(DateTime eventDate)
    {
        var criteria = new QueryInfositeMatchCriteria
        {
            EventDate = eventDate
        };

        var matches = await repository.FetchAsync<InfositeMatch>(criteria);
        return matches.ToList();
    }
}