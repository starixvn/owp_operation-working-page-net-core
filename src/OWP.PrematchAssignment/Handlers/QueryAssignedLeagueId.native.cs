using Fanex.Data.Repository;
using MediatR;
using OWP.Extensions;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.Helper;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;

namespace OWP.PrematchAssignment.Handlers;

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

    private readonly Repository repository;

    public QueryAssignedLeagueIdHandler(IDynamicRepository dynamicRepository, Repository repository)
    {
        this.dynamicRepository = dynamicRepository;
        this.repository = repository;
    }

    public async Task<AssignedLeagueId> Handle(QueryAssignedLeagueIdRequest request, CancellationToken cancellationToken)
    {
        var prematchLeagueIds = await GetTodayPrematchLeague(isDay: request.IsDay);
        var owpLeagueNames = (await QueryAssignedLeagueNames(request: request, prematchLeagueIds: prematchLeagueIds)).ToList();
        var mainDbLeagueIds = (await QueryAssignedLeagueIdsFromMainDB(leagueNames: owpLeagueNames)).ToList();
        var unSupportedLeagueNames = new List<string>();

        if (owpLeagueNames.Count > mainDbLeagueIds.Count)
        {
            unSupportedLeagueNames = owpLeagueNames.Except(second: mainDbLeagueIds.Select(selector: x => x.LeagueName)).ToList();
        }

        return new AssignedLeagueId
        {
            LeagueIds = string.Join(separator: ",", values: mainDbLeagueIds.Select(selector: x => x.LeagueId)),
            UnSupportedLeagueNames = unSupportedLeagueNames,
            TodayLeagueCount = owpLeagueNames.Count
        };
    }

    public async Task<IEnumerable<int>> GetTodayPrematchLeague(bool isDay)
    {
        var eventDate = DateTime.Now.ToGmt8().Date;
        var startTime = await repository.QueryExistedStartTime(eventDate: eventDate);
        var criteria = isDay
            ? new QueryShiftPreMatchCriteria
            {
                EventDate = eventDate,
                EventFromDateTime = eventDate.AddHours(value: 12),
                EventToDateTime = eventDate.AddDays(value: 1).AddHours(value: 11).AddMinutes(value: 59).AddSeconds(value: 59),
                IsDay = true
            }
            : ShiftHelper.CreateShiftCriteria(eventDate: eventDate, isDay: false, startTime ?? new TimeSpan());

        var (prematchLeague, _) = await dynamicRepository.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(criteria: criteria);
        return prematchLeague.Select(selector: x => x.PreMatchLeagueId).Distinct();
    }

    public async Task<IEnumerable<string>> QueryAssignedLeagueNames(QueryAssignedLeagueIdRequest request, IEnumerable<int> prematchLeagueIds)
    {
        var criteria = new QueryAssignedLeagueNamesCriteria
        {
            ActorId = request.ActorId,
            BettypeId = request.BettypeId,
            IsTakeOver = request.IsTakeOver,
            IsDay = request.IsDay,
            PrematchLeagueIds = string.Join(separator: ",", values: prematchLeagueIds)
        };

        return await dynamicRepository.FetchAsync<string>(criteria: criteria);
    }

    public async Task<IEnumerable<LeagueInfo>> QueryAssignedLeagueIdsFromMainDB(IEnumerable<string> leagueNames)
    {
        var criteria = new QueryAssignedLeagueIdsFromMainDBCriteria
        {
            LeagueNames = string.Join(separator: ",", values: leagueNames)
        };

        return await dynamicRepository.FetchAsync<LeagueInfo>(criteria: criteria);
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