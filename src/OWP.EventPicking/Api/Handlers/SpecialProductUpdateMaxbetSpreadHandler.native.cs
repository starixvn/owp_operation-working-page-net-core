

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
        
        var TargetJson = JsonConvert.SerializeObject(request.Events);
        var criteria = new UpdateMaxbetSpreadCriteria
        {
            TargetJson = TargetJson,
            ActorId = session.UserId
        };
        await repository.UpdateEvent(criteria);

        var xml = XmlConverter.Serialize(logData);
        await repository.SaveEventLog(new SaveEditEventLogsCriteria { Xml = xml });

        await SaveUpdatedDataToCache(request.EventDate, request.Events, cachedMBSpreads);

        return Unit.Value;
    }

    private List<LoggedChangeModel> CompareAndGenerateRAndDInfo(IList<UpdateEvents> requestEvents, List<EventMBSpread> cachedMBSpreads)
    {
        var rAndDInfos = new List<LoggedChangeModel>();

        foreach (var requestEvent in requestEvents)
        {
            var cachedEvent = cachedMBSpreads.FirstOrDefault(c => c.InfoSiteMatchId == requestEvent.InfoSiteMatchId);

            if (cachedEvent == null)
            {
                continue;
            }

            CheckAndAddStringChange(rAndDInfos, requestEvent.InfoSiteMatchId, "SpreadHDP", formatSpread(requestEvent.SpreadHdp), formatSpread(cachedEvent.SpreadHdp));
            CheckAndAddStringChange(rAndDInfos, requestEvent.InfoSiteMatchId, "SpreadOU", formatSpread(requestEvent.SpreadOu), formatSpread(cachedEvent.SpreadOu));
            CheckAndAddStringChange(rAndDInfos, requestEvent.InfoSiteMatchId, "MB", formatMaxbet(requestEvent.MaxbetHdp), formatMaxbet(cachedEvent.MaxbetHdp));
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
            rAndDInfos.Add(CreateRAndDInfo(infoSiteMatchId, $"Update <b>{fieldName}<b/>", 
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
            var existingEvent = updatedCacheData.FirstOrDefault(c => c.InfoSiteMatchId == requestEvent.InfoSiteMatchId);
            
            if (existingEvent != null)
            {
                // Update existing event in cache
                existingEvent.SpreadHdp = requestEvent.SpreadHdp;
                existingEvent.SpreadOu = requestEvent.SpreadOu;
                existingEvent.MaxbetHdp = requestEvent.MaxbetHdp;
                existingEvent.NoOfHDP = requestEvent.NoOfHDP;
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
                    NoOfHDP = requestEvent.NoOfHDP,
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