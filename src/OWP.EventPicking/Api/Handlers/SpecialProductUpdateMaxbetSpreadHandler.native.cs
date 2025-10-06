

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

            CheckAndAddNumberChange(rAndDInfos, requestEvent.InfoSiteMatchId, "SpreadHdp", requestEvent.SpreadHdp, cachedEvent.SpreadHdp);
            CheckAndAddNumberChange(rAndDInfos, requestEvent.InfoSiteMatchId, "SpreadOu", requestEvent.SpreadOu, cachedEvent.SpreadOu);
            CheckAndAddNumberChange(rAndDInfos, requestEvent.InfoSiteMatchId, "MaxbetHdp", requestEvent.MaxbetHdp, cachedEvent.MaxbetHdp);
            CheckAndAddStringChange(rAndDInfos, requestEvent.InfoSiteMatchId, "NoOfHDP", requestEvent.NoOfHDP, cachedEvent.NoOfHDP);
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
        var newCacheData = requestEvents.Select(requestEvent => new EventMBSpread
        {
            InfoSiteMatchId = requestEvent.InfoSiteMatchId,
            SpreadHdp = requestEvent.SpreadHdp,
            SpreadOu = requestEvent.SpreadOu,
            MaxbetHdp = requestEvent.MaxbetHdp,
            IsReferenceLeagueManagement = false // Default value for updated events
        }).ToList();
        
        await cacheManipulator.SaveMBSpreadsByDate(eventDate, newCacheData);
    }
    
}