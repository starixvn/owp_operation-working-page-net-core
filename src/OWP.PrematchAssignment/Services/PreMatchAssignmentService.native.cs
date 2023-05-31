using AutoMapper;
using Fanex.Caching;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using Newtonsoft.Json;
using OWP.Extensions;
using OWP.PrematchAssignment.Constants;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.Handlers;
using OWP.PrematchAssignment.Models;
using OWP.PreMatchAssignment.Criteria;
using OWP.PreMatchAssignment.Handlers;

namespace OWP.PrematchAssignment.Services;

public interface PreMatchAssignmentService
{
    Task<IEnumerable<TicketAssignee>> QueryAssigneesByLeagueId(int id);

    Task<InfoMatchQueried> GetInfoMatchById(int id);

    Task Update(UpdateLeagueAssignmentRequest request);

    Task Delete(UpdateLeagueAssignmentRequest request);

    Task UpdateStartTime(UpdateLeagueAssignmentRequest request);

    Task SaveOldPreMatchAssignment(List<int> preMatchId);

    Task<QueryPreMatchLeagueViewModel> QueryById(QueryPreMatchLeagueByIdRequest request);
}

public class PreMatchAssignmentServiceImp : PreMatchAssignmentService
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly INapSession currentSession;

    private readonly IMapper mapper;

    private readonly ICache cache;

    public PreMatchAssignmentServiceImp(
        IDynamicRepository dynamicRepository,
        INapSession currentSession,
        IMapper mapper,
        ICache cache)
    {
        this.dynamicRepository = dynamicRepository;
        this.currentSession = currentSession;
        this.mapper = mapper;
        this.cache = cache;
    }

    public async Task<IEnumerable<TicketAssignee>> QueryAssigneesByLeagueId(int id)
    {
        var criteria = new QueryAssigneePreMatchByPreLeagueIdCriteria
        {
            Id = id
        };

        criteria.ThrowExceptionIfInvalid();
        return await dynamicRepository.FetchAsync<TicketAssignee>(criteria);
    }

    public async Task<InfoMatchQueried> GetInfoMatchById(int id)
    {
        var criteria = new QueryInfoMatchByIdCriteria
        {
            Id = id
        };

        criteria.ThrowExceptionIfInvalid();
        return (await dynamicRepository.FetchAsync<InfoMatchQueried>(criteria)).FirstOrDefault();
    }

    public async Task Update(UpdateLeagueAssignmentRequest request)
    {
        var updateTasks = request.PrematchUpdated.Select(league =>
            {
                var criteria = mapper.Map<UpdateAssignmentCriteria>(league);
                criteria.ActorId = currentSession.UserId;
                return criteria;
            })
            .Select(updateCriteria => dynamicRepository.ExecuteAsync(updateCriteria))
            .Cast<Task>()
            .ToArray();

        await Task.WhenAll(updateTasks);
    }

    public async Task Delete(UpdateLeagueAssignmentRequest request)
    {
        var deleteTasks = request.PrematchIdDeleted.Select(leagueId => new DeleteLeagueCriteria
        {
            PreMatchLeagueId = leagueId,
            ActorId = currentSession.UserId
        })
            .Select(deleteCriteria => dynamicRepository.ExecuteAsync(deleteCriteria))
            .Cast<Task>()
            .ToArray();

        await Task.WhenAll(deleteTasks);
    }

    public async Task UpdateStartTime(UpdateLeagueAssignmentRequest request)
    {
        var updateStartTimeCriteria = new UpdateStartTimeCriteria
        {
            StartTime = request.StartTime,
            Date = request.Date,
            ActorId = currentSession.UserId
        };
        await dynamicRepository.ExecuteAsync(updateStartTimeCriteria);
    }

    public async Task SaveOldPreMatchAssignment(List<int> preMatchId)
    {
        var assignmentOldValue = await QueryAssignmentOldValue(preMatchId);
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(RedisKey.CACHE_EXPIRED_IN_30_MINS));
        var assignmentSnapshotInJson = JsonConvert.SerializeObject(assignmentOldValue);
        await cache.RemoveAsync(RedisKey.KEY_PREMATCH_ASSIGNMENT_OLD_VALUE_SNAPSHOT);
        await cache.SetAsync(RedisKey.KEY_PREMATCH_ASSIGNMENT_OLD_VALUE_SNAPSHOT, assignmentSnapshotInJson, cachePolicy);
    }

    private async Task<List<AssignmentSnapshot>> QueryAssignmentOldValue(List<int> preMatchId)
    {
        var assignmentOldValue = new List<AssignmentSnapshot>();
        foreach (var id in preMatchId)
        {
            var assignmentResult = await dynamicRepository.FetchAsync<AssignmentSnapshot>(new QueryAssignmentByPrematchLeagueIdCriteria
            {
                PreMatchLeagueId = id
            });
            assignmentOldValue.Add(assignmentResult.FirstOrDefault());
        }
        return assignmentOldValue;
    }

    public async Task<QueryPreMatchLeagueViewModel> QueryById(QueryPreMatchLeagueByIdRequest request)
    {
        var criteria = new QueryByPreMatchLeagueIdCriteria
        {
            ActorId = currentSession.UserId,
            PreMatchLeagueId = request.PreMatchLeagueId,
            StartTime = request.StartTime
        };

        var result = await dynamicRepository.FetchMultipleAsync<PreMatchLeague, InfoSiteMatch>(criteria);

        var viewModel = new QueryPreMatchLeagueViewModel
        {
            InfoSiteMatches = result.Item2,
            PreMatchLeagues = mapper.Map<IList<LeagueAssignment>>(result.Item1)
        };
        return viewModel;
    }
}