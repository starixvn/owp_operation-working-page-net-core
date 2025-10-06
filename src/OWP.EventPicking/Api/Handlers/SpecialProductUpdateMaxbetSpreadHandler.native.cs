

using Fanex.NAP.Client;
using Newtonsoft.Json;
using OWP.EventPicking.Api.Services;

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
        
        var rAndDInfos = CompareAndGenerateRAndDInfo(request.Events, cachedMBSpreads);
        
        var TargetJson = JsonConvert.SerializeObject(request.Events);
        var criteria = new UpdateMaxbetSpreadCriteria
        {
            TargetJson = TargetJson,
            ActorId = session.UserId
        };
        await repository.UpdateEvent(criteria);

        return Unit.Value;
    }

    private List<RAndDInfo> CompareAndGenerateRAndDInfo(IList<UpdateEvents> requestEvents, List<EventMBSpread> cachedMBSpreads)
    {
        var rAndDInfos = new List<RAndDInfo>();

        foreach (var requestEvent in requestEvents)
        {
            var cachedEvent = cachedMBSpreads.FirstOrDefault(c => c.InfoSiteMatchId == requestEvent.InfoSiteMatchId);

            if (cachedEvent == null)
            {
                continue;
            }

            if (HasValueChanged(requestEvent.SpreadHdp, cachedEvent.SpreadHdp))
            {
                rAndDInfos.Add(CreateRAndDInfo(requestEvent.InfoSiteMatchId, "Update <b>SpreadHdp<b/>",
                    cachedEvent.SpreadHdp?.ToString() ?? "",
                    requestEvent.SpreadHdp?.ToString() ?? ""));
            }

            if (HasValueChanged(requestEvent.SpreadOu, cachedEvent.SpreadOu))
            {
                rAndDInfos.Add(CreateRAndDInfo(requestEvent.InfoSiteMatchId, "Update <b>SpreadOu<b/>",
                    cachedEvent.SpreadOu?.ToString() ?? "",
                    requestEvent.SpreadOu?.ToString() ?? ""));
            }

            if (HasValueChanged(requestEvent.MaxbetHdp, cachedEvent.MaxbetHdp))
            {
                rAndDInfos.Add(CreateRAndDInfo(requestEvent.InfoSiteMatchId, "Update <b>MaxbetHdp<b/>",
                    cachedEvent.MaxbetHdp?.ToString() ?? "",
                    requestEvent.MaxbetHdp?.ToString() ?? ""));
            }
            
            if(requestEvent.NoOfHDP != cachedEvent.NoOfHDP)
            {
                rAndDInfos.Add(CreateRAndDInfo(requestEvent.InfoSiteMatchId, "Update <b>NoOfHDP<b/>",
                    cachedEvent.NoOfHDP,
                    requestEvent.NoOfHDP));
            }
        }
        
        return rAndDInfos;
    }
    
    private bool HasValueChanged(decimal? newValue, decimal? oldValue)
    {
        if (newValue == null && oldValue == null) return false;
        if (newValue == null || oldValue == null) return true;
        return Math.Abs(newValue.Value - oldValue.Value) > 0.0001m; // Small tolerance for decimal comparison
    }
    
    private RAndDInfo CreateRAndDInfo(int infoSiteMatchId, string fieldName, string oldValue, string newValue)
    {
        return new RAndDInfo
        {
            InfositeId = infoSiteMatchId,
            UpdateTypeId = 0,
            RAndDUpdate = fieldName,
            OldValue = oldValue,
            NewValue = newValue,
            Group = "MaxBet/Spread Update"
        };
    }
    
}