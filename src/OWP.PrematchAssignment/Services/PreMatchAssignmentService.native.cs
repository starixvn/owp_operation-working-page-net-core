using Fanex.NAP.Client.Helpers.Web;

namespace OWP.PreMatchAssignment.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Web.Session;
    using Handlers;
    using Models;
    using Newtonsoft.Json;
    using OWP.PreMatchAssignment.Constants;
    using Utils;

    public interface PreMatchAssignmentService
    {
        Task Update(UpdateLeagueAssignmentRequest request);

        Task Delete(UpdateLeagueAssignmentRequest request);

        Task UpdateStartTime(UpdateLeagueAssignmentRequest request);

        Task<IEnumerable<Message>> QueryIssueMessages(int preMatchIssueId, bool isShowAllMessage);

        Task<IEnumerable<Message>> QueryIssueMessagesAfterInsert(int? owpLeagueId, int? infoSiteMatchId);

        Task CreateNewIssue(CreateNewIssueRequest request);

        Task PostNewMessage(PostNewMessageRequest request);

        Task<QueryPreMatchLeagueViewModel> QueryById(QueryPreMatchLeagueByIdRequest request);

        Task<IList<string>> QueryNotificationReceivers(int preMatchIssueId);

        Task<PreMatchIssueInfo> QueryPreMatchIssueInfo(int preMatchIssueId);

        Task SaveOldPreMatchAssignment(List<int> preMatchId);
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

        public async Task<IEnumerable<Message>> QueryIssueMessages(int preMatchIssueId, bool isShowAllMessage)
        {
            var criteria = new QueryIssueMessagesCriteria
            {
                PreMatchIssueId = preMatchIssueId,
                ActorId = currentSession.UserId
            };

            var result = await dynamicRepository.FetchMultipleAsync<Message, DateTime>(criteria);
            var messageList = isShowAllMessage
                              ? result.Item1.ToList()
                              : ShowMessageIn24Hours(result.Item1, result.Item2.FirstOrDefault());

            return messageList;
        }

        public static IEnumerable<Message> ShowMessageIn24Hours(IEnumerable<Message> data, DateTime currentTime)
        {
            var last24Hours = currentTime.AddHours(-24);
            return data.Where(x => x.CreatedDate >= last24Hours);
        }

        public async Task<IEnumerable<Message>> QueryIssueMessagesAfterInsert(int? owpLeagueId, int? infoSiteMatchId)
        {
            var criteria = new QueryIssueMessagesAfterInsertCriteria
            {
                InfoSiteMatchId = infoSiteMatchId,
                OWPLeagueId = owpLeagueId,
                ActorId = currentSession.UserId
            };

            return await dynamicRepository.FetchAsync<Message>(criteria);
        }

        public async Task CreateNewIssue(CreateNewIssueRequest request)
        {
            var criteria = new InsertNewIssueCriteria
            {
                ActorId = currentSession.UserId,
                InfoSiteMatchId = request.InfoSiteMatchId,
                Message = request.Message,
                OWPLeagueId = request.OWPLeagueId,
                FileId = request.FileId
            };

            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task PostNewMessage(PostNewMessageRequest request)
        {
            var criteria = new InsertNewMessageCriteria
            {
                ActorId = currentSession.UserId,
                Message = request.Message,
                PreMatchIssueId = request.PreMatchIssueId,
                FileId = request.FileId
            };

            await dynamicRepository.ExecuteAsync(criteria);
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

        public async Task<IList<string>> QueryNotificationReceivers(int preMatchIssueId)
        {
            var criteria = new GetNotificationReceiversCriteria
            {
                ActorId = currentSession.UserId,
                PreMatchIssueId = preMatchIssueId
            };
            var result = await dynamicRepository.FetchAsync<string>(criteria);
            return result.ToList();
        }

        public async Task<PreMatchIssueInfo> QueryPreMatchIssueInfo(int preMatchIssueId)
        {
            var criteria = new QueryPreMatchIssueInfoCriteria
            {
                PreMatchIssueId = preMatchIssueId
            };
            criteria.ThrowExceptionIfInvalid();

            var result = await dynamicRepository.FetchAsync<PreMatchIssueInfo>(criteria);
            return result.FirstOrDefault();
        }

        public async Task SaveOldPreMatchAssignment(List<int> preMatchId)
        {
            var assignmentOldValue = await QueryAssignmentOldValue(preMatchId);
            var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(RedisKey.CACHE_EXPIRED_IN_DAYS));
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
    }
}
