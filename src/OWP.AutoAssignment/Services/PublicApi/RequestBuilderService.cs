namespace OWP.AutoAssignment.Services.PublicApi;

using Enum;
using Models.InternalApi;
using Models.PublicApi;

public interface RequestBuilder
{
    Task<IEnumerable<RequestSender>> BuildRequestSenderModel(IEnumerable<RequestModel> request);
}

public class RequestBuilderImp : RequestBuilder
{
    private SchedulingMatch schedulingMatch = new();

    private IList<SchedulingLAUser> schedulingLAUser = new List<SchedulingLAUser>();

    private IList<SchedulingTraderInfo> traderInfos = new List<SchedulingTraderInfo>();

    private readonly Repository repository;
    private readonly SchedulingMatchBuilderService matchService;
    private readonly LAUserService laService;
    private readonly TraderUserService traderService;

    public RequestBuilderImp(
        Repository repository,
        SchedulingMatchBuilderService matchService,
        LAUserService laService,
        TraderUserService traderService)
    {
        this.repository = repository;
        this.matchService = matchService;
        this.laService = laService;
        this.traderService = traderService;
    }

    public async Task<IEnumerable<RequestSender>> BuildRequestSenderModel(IEnumerable<RequestModel> request)
    {
        var sender = new List<RequestSender>();
        foreach (var req in request)
        {
            schedulingMatch = await matchService.GetMatchesInfo(req.EventDate);
            schedulingLAUser = await laService.GetLAUsersInfo(req.EventDate);
            traderInfos = await traderService.GetTradersInfo(req.EventDate);
            await BuildRequestSenderModel(req, schedulingMatch, sender);
        }
        return sender;
    }

    private async Task BuildRequestSenderModel(
        RequestModel request,
        SchedulingMatch schedulingMatch,
        IList<RequestSender> sender)
    {
        var realMatch = schedulingMatch.Real;
        var sabaMatch = schedulingMatch.Saba;

        var hasSuspender = schedulingLAUser.Any() || traderInfos.Any();
        var hasMatch = realMatch.Any() || sabaMatch.Any();
        if (hasMatch && hasSuspender)
        {
            sender.Add(BuildRequestSender(request, "New"));
            await UpdateStatusRequest(request, StatusRequest.Running);
            await SaveEventSender(request, realMatch, sabaMatch);
        }
        else
        {
            await UpdateStatusRequest(request, StatusRequest.Success);
        }
    }

    private async Task UpdateStatusRequest(RequestModel requestModel, StatusRequest status)
        => await repository.UpdateStatusRequest(new StatusRequestModel
        {
            RequestId = requestModel.RequestId,
            Status = status
        });

    private RequestSender BuildRequestSender(RequestModel model, string action)
    {
        return new RequestSender
        {
            Action = action,
            RequestId = model.RequestId,
            DataHolder = BuildDataHolder(),
            EventDate = model.EventDate
        };
    }

    private DataHolder BuildDataHolder()
        => new()
        {
            Matches = schedulingMatch,
            Suspenders = schedulingLAUser,
            Traders = traderInfos
        };

    private async Task SaveEventSender(RequestModel requestModel, IList<RealMatchSender> realMatches, IList<SabaMatchSender> sabaMatches)
    {
        var events = new List<AutoAssignmentHistory>();
        var realHistory = realMatches.Select(i => new AutoAssignmentHistory
        {
            InfoSiteMatchId = i.MatchId,
            LeagueSettingId = i.LeagueSettingId,
            RequestId = requestModel.RequestId,
            MaxBetFTHDP = i.MaxBetMainMatch,
            MaxBetNextCorner = i.MaxBetNextCorner,
            MaxBetNoOfCorner = i.MaxBetNoOfCorner
        });
        var sabaHistory = sabaMatches.Select(x => new AutoAssignmentHistory
        {
            InfoSiteMatchId = x.MatchId,
            MaxBetFTHDP = x.MaxBet,
            LeagueSettingId = x.LeagueSettingId,
            RequestId = requestModel.RequestId
        });
        events.AddRange(realHistory);
        events.AddRange(sabaHistory);

        await repository.SaveEventSender(events);
    }
}