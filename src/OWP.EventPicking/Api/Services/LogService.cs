using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Services;

public interface LogService
{
    Task<IList<LogModel>> GetLog(int infoSiteMatchId);
}

public class LogServiceImp : LogService
{
    private readonly Repository repository;

    public LogServiceImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<LogModel>> GetLog(int infoSiteMatchId)
    => await repository.GetLog(infoSiteMatchId);
}