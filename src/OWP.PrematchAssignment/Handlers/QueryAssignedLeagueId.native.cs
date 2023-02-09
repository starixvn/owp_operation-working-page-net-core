using Fanex.Data.Repository;
using MediatR;
using OWP.Extensions;
using OWP.PreMatchAssignment.Criteria;
using OWP.PreMatchAssignment.Models;
using OWP.PreMatchAssignment.Services;

namespace OWP.PreMatchAssignment.Handlers;

public class QueryAssignedLeagueIdRequest : IRequest<AssignedLeagueId>
{
    public int ActorId { get; set; }

    public int BettypeId { get; set; }

    public bool IsTakeOver { get; set; }

    public bool IsDay { get; set; }
}

public class QueryAssignedLeagueIdHandler : IRequestHandler<QueryAssignedLeagueIdRequest, AssignedLeagueId>
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly ShiftAssignmentService shiftAssignmentService;

    public QueryAssignedLeagueIdHandler(IDynamicRepository dynamicRepository, ShiftAssignmentService shiftAssignmentService)
    {
        this.dynamicRepository = dynamicRepository;
        this.shiftAssignmentService = shiftAssignmentService;
    }

    public async Task<AssignedLeagueId> Handle(QueryAssignedLeagueIdRequest request, CancellationToken cancellationToken)
    {
        var prematchLeagueIds = await GetTodayPrematchLeague(request.IsDay);
        var owpLeageNames = await QueryAssignedLeagueNames(request, prematchLeagueIds);
        var mainDbLeagueIds = await QueryAssignedLeagueIdsFromMainDB(owpLeageNames);
        var unSupportedLeagueNames = new List<string>();

        if (owpLeageNames.Count() > mainDbLeagueIds.Count())
        {
            unSupportedLeagueNames = owpLeageNames.Except(mainDbLeagueIds.Select(x => x.LeagueName)).ToList();
        }

        return new AssignedLeagueId
        {
            LeagueIds = string.Join(",", mainDbLeagueIds.Select(x => x.LeagueId)),
            UnSupportedLeagueNames = unSupportedLeagueNames,
            TodayLeagueCount = owpLeageNames.Count()
        };
    }

    public async Task<IEnumerable<int>> GetTodayPrematchLeague(bool isDay)
    {
        var eventDate = DateTime.Now.Date.ToGmt8();
        var startTime = await shiftAssignmentService.QueryExistedStartTime(eventDate);
        var criteria = isDay
            ? new QueryShiftPreMatchCriteria
            {
                EventDate = eventDate,
                EventFromDateTime = eventDate.AddHours(12),
                EventToDateTime = eventDate.AddDays(1).AddHours(11).AddMinutes(59).AddSeconds(59),
                IsDay = isDay
            }
            : shiftAssignmentService.CreateNightShiftCriteria(eventDate, startTime, isDay);

        var (prematchLeague, _) = await dynamicRepository.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(criteria);
        return prematchLeague.Select(x => x.PreMatchLeagueId).Distinct();
    }

    public async Task<IEnumerable<string>> QueryAssignedLeagueNames(QueryAssignedLeagueIdRequest request, IEnumerable<int> prematchLeagueIds)
    {
        var criteria = new QueryAssignedLeagueNamesCriteria
        {
            ActorId = request.ActorId,
            BettypeId = request.BettypeId,
            IsTakeOver = request.IsTakeOver,
            IsDay = request.IsDay,
            PrematchLeagueIds = string.Join(",", prematchLeagueIds)
        };

        return await dynamicRepository.FetchAsync<string>(criteria);
    }

    public async Task<IEnumerable<LeagueInfo>> QueryAssignedLeagueIdsFromMainDB(IEnumerable<string> leagueNames)
    {
        var criteria = new QueryAssignedLeagueIdsFromMainDBCriteria
        {
            LeagueNames = string.Join(",", leagueNames)
        };

        return await dynamicRepository.FetchAsync<LeagueInfo>(criteria);
    }
}

public class AssignedLeagueId
{
    public string LeagueIds { get; set; }

    public List<string> UnSupportedLeagueNames { get; set; }

    public int TodayLeagueCount { get; set; }
}

public class LeagueInfo
{
    public int LeagueId { get; set; }

    public string LeagueName { get; set; }
}

public class QueryAssignedLeagueIdsFromMainDBCriteria : CriteriaBase
{
    public string LeagueNames { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_QueryAssignedLeagueIdFromMainDB";

    public override bool IsValid() => true;
}

public class QueryAssignedLeagueNamesCriteria : CriteriaBase
{
    public int ActorId { get; set; }

    public int BettypeId { get; set; }

    public bool IsTakeOver { get; set; }

    public bool IsDay { get; set; }

    public string PrematchLeagueIds { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_QueryAssignedLeagueNames";

    public override bool IsValid() => ActorId > 0;
}