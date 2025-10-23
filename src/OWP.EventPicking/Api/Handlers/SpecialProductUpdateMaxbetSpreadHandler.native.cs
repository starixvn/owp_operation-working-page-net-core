

using Fanex.NAP.Client;
using Newtonsoft.Json;
using OWP.EventPicking.Api.Services;
using System.Linq;

namespace OWP.EventPicking.Api.Handlers;

public class UpdateEventsRequest : IRequest<Unit>
{
    public IList<UpdateEvents> Events { get; set; }
    public DateTime EventDate { get; set; }
}

public class UpdateMaxbetHandler : IRequestHandler<UpdateEventsRequest, Unit>
{
    private readonly Repository repository;
    private readonly INapSession session;
    private readonly CacheManipulator cacheManipulator;

    public UpdateMaxbetHandler(Repository repository, INapSession session, CacheManipulator cacheManipulator)
    {
        this.repository = repository;
        this.session = session;
        this.cacheManipulator = cacheManipulator;
    }

    public async Task<Unit> Handle(UpdateEventsRequest request, CancellationToken cancellationToken)
    {
        var cachedMBSpreads = await cacheManipulator.GetMBSpreadsByDate(request.EventDate);
        var logData = CompareAndGenerateRAndDInfo(request.Events, cachedMBSpreads);

        await UpdateMaxbetSpreadMainMatches(request.Events);
        await UpdateMaxbetSpreadMinorMatches(request.Events);

        var xml = XmlConverter.Serialize(logData);
        await repository.SaveEventLog(new SaveEditEventLogsCriteria { Xml = xml });

        await SaveUpdatedDataToCache(request.EventDate, request.Events, cachedMBSpreads);

        return Unit.Value;
    }

    private async Task UpdateMaxbetSpreadMinorMatches(IList<UpdateEvents> events)
    {
        var minorMatches = events.Where(e => e.IsChildMatch).ToList();
        var mapModel = minorMatches
            .Select(a => new SpecialProductMaxbetSpread
            {
                ParentId = a.InfoSiteMatchId,
                MinorBettypeId = a.MinorBettypeId,
                MaxbetHdp = a.MaxbetHdp,
                MaxbetOu = a.MaxbetOu,
                NoOfHDP = a.NoOfHDP,
                SpreadHdp = a.SpreadHdp,
                SpreadOu = a.SpreadOu
            })
            .ToList();
        var TargetJson = JsonConvert.SerializeObject(mapModel);
        var criteria = new SpecialProductUpdateMaxbetSpreadCriteria()
        {
            TargetJson = TargetJson,
            ActorId = session.UserId
        };
        await repository.ExecuteNonQuery(criteria);
    }

    private async Task UpdateMaxbetSpreadMainMatches(IList<UpdateEvents> events)
    {
        var mainMatches = events.Where(e => !e.IsChildMatch).ToList();
        var TargetJson = JsonConvert.SerializeObject(mainMatches);
        var criteria = new UpdateMaxbetSpreadCriteria
        {
            TargetJson = TargetJson,
            ActorId = session.UserId
        };
        await repository.UpdateEvent(criteria);
    }

    private List<LoggedChangeModel> CompareAndGenerateRAndDInfo(IList<UpdateEvents> requestEvents, List<EventMBSpread> cachedMBSpreads)
    {
        var rAndDInfos = new List<LoggedChangeModel>();

        foreach (var requestEvent in requestEvents)
        {
            var cachedEvent = cachedMBSpreads.FirstOrDefault(c => c.InfoSiteMatchId == requestEvent.InfoSiteMatchId 
                                                                  && c.MinorBettypeId == requestEvent.MinorBettypeId);

            if (cachedEvent == null)
            {
                continue;
            }

            CheckAndAddStringChange(rAndDInfos, requestEvent.InfoSiteMatchId, "SpreadHDP", formatSpread(requestEvent.SpreadHdp), formatSpread(cachedEvent.SpreadHdp));
            CheckAndAddStringChange(rAndDInfos, requestEvent.InfoSiteMatchId, "SpreadOU", formatSpread(requestEvent.SpreadOu), formatSpread(cachedEvent.SpreadOu));
            CheckAndAddStringChange(rAndDInfos, requestEvent.InfoSiteMatchId, "MB HDP", formatMaxbet(requestEvent.MaxbetHdp), formatMaxbet(cachedEvent.MaxbetHdp));
            CheckAndAddStringChange(rAndDInfos, requestEvent.InfoSiteMatchId, "MB OU", formatMaxbet(requestEvent.MaxbetOu), formatMaxbet(cachedEvent.MaxbetOU));
            CheckAndAddStringChange(rAndDInfos, requestEvent.InfoSiteMatchId, "No.HDP", requestEvent.NoOfHDP, cachedEvent.NoOfHDP);
        }
        
        return rAndDInfos;
    }
    
    private void CheckAndAddNumberChange(List<LoggedChangeModel> rAndDInfos, int infoSiteMatchId, string fieldName, decimal? newValue, decimal? oldValue)
    {
        if (HasNumberChanged(newValue, oldValue))
        {
            rAndDInfos.Add(CreateRAndDInfo(infoSiteMatchId, $"Update <b>{fieldName}<b/>", 
                oldValue?.ToString() ?? "", newValue?.ToString() ?? ""));
        }
    }
    
    private void CheckAndAddStringChange(List<LoggedChangeModel> rAndDInfos, int infoSiteMatchId, string fieldName, string newValue, string oldValue)
    {
        if ((newValue ?? "") != (oldValue ?? ""))
        {
            rAndDInfos.Add(CreateRAndDInfo(infoSiteMatchId, $"Update the <b>{fieldName}<b/>", 
                oldValue ?? "", newValue ?? ""));
        }
    }
    
    private bool HasNumberChanged(decimal? newValue, decimal? oldValue)
    {
        if (newValue == null && oldValue == null) return false;
        if (newValue == null || oldValue == null) return true;
        return Math.Abs(newValue.Value - oldValue.Value) > 0.0001m; // Small tolerance for decimal comparison
    }
    
    private LoggedChangeModel CreateRAndDInfo(int infoSiteMatchId, string fieldName, string oldValue, string newValue)
    {
        return new LoggedChangeModel
        {
            InfoSiteMatchId = infoSiteMatchId,
            UpdateTypeId = null,
            ModifiedBy = session.UserName,
            Action = fieldName,
            OldValue = oldValue,
            NewValue = newValue,
            UpDateTime = DateTime.Now.AddHours(12)
        };
    }
    
    private async Task SaveUpdatedDataToCache(DateTime eventDate, IList<UpdateEvents> requestEvents, List<EventMBSpread> cachedMBSpreads)
    {
        // Create a copy of the existing cached data
        var updatedCacheData = cachedMBSpreads.ToList();
        
        // Update only the events that were modified
        foreach (var requestEvent in requestEvents)
        {
            var existingEvent = updatedCacheData.FirstOrDefault(c => c.InfoSiteMatchId == requestEvent.InfoSiteMatchId
                                                                     && c.MinorBettypeId == requestEvent.MinorBettypeId);
            
            if (existingEvent != null)
            {
                // Update existing event in cache
                existingEvent.SpreadHdp = requestEvent.SpreadHdp;
                existingEvent.SpreadOu = requestEvent.SpreadOu;
                existingEvent.MaxbetHdp = requestEvent.MaxbetHdp;
                existingEvent.MaxbetOU = requestEvent.MaxbetOu;
                existingEvent.NoOfHDP = requestEvent.NoOfHDP;
                existingEvent.IsChildMatch = requestEvent.IsChildMatch;
                existingEvent.MinorBettypeId = requestEvent.MinorBettypeId;
                existingEvent.IsReferenceLeagueManagement = false; // Mark as updated
            }
            else
            {
                // Add new event to cache
                updatedCacheData.Add(new EventMBSpread
                {
                    InfoSiteMatchId = requestEvent.InfoSiteMatchId,
                    SpreadHdp = requestEvent.SpreadHdp,
                    SpreadOu = requestEvent.SpreadOu,
                    MaxbetHdp = requestEvent.MaxbetHdp,
                    MaxbetOU = requestEvent.MaxbetOu,
                    NoOfHDP = requestEvent.NoOfHDP,
                    IsChildMatch = requestEvent.IsChildMatch,
                    MinorBettypeId = requestEvent.MinorBettypeId,
                    IsReferenceLeagueManagement = false
                });
            }
        }
        
        await cacheManipulator.SaveMBSpreadsByDate(eventDate, updatedCacheData);
    }

    private string  formatSpread(decimal? spread)
    {
        return spread.HasValue ? $"{Math.Round(spread.Value*100)}%" : string.Empty;
    }

    private string formatMaxbet(decimal? maxbet)
    {
        return maxbet.HasValue ? $"{Math.Round(maxbet.Value/1000)}K" : string.Empty;
    }
}