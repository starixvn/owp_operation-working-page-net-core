namespace OWP.EventPicking.Api.Services;

public interface LogService
{
    Task<IList<LogModel>> GetLog(int infoSiteMatchId);

    Task SaveMBSpreadsToCache(DateTime eventDate, List<EventPickingModel> matchFiltered);
}

public class LogServiceImp(Repository repository, CacheManipulator cacheManipulator) : LogService
{
    public async Task<IList<LogModel>> GetLog(int infoSiteMatchId)
        => await repository.GetLog(infoSiteMatchId);

    public async Task SaveMBSpreadsToCache(DateTime eventDate, List<EventPickingModel> matchFiltered)
    {
        var mbSpreads = matchFiltered.Select(match => new EventMBSpread
        {
            InfoSiteMatchId = match.InfoSiteMatchId,
            MaxbetHdp = match.MaxbetHdp,
            MaxbetOU = match.MaxbetOu,
            SpreadHdp = match.SpreadHdp,
            SpreadOu = match.SpreadOu,
            IsChildMatch = match.IsChildMatch,
            MinorBettypeId = match.MinorBettypeId ?? 0,
            IsReferenceLeagueManagement = match.IsReferenceLeagueManagement
        }).ToList();

        await cacheManipulator.SaveMBSpreadsByDate(eventDate, mbSpreads);
    }
}