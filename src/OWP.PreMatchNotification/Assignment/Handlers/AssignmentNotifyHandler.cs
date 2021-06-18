using Fanex.Data.Repository;
using MediatR;
using OWP.PreMatchNotification.Assignment.Criteria;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Assignment.Services;
using OWP.PreMatchNotification.Constants;
using OWP.PreMatchNotification.Models;
using OWP.PreMatchNotification.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OWP.PreMatchNotification.Assignment.Handlers
{
    public class AssignmentNotifyRequest : IRequest<Unit>
    {
        public string redisKey { get; set; }

        public string SessionId { get; set; }
    }

    public class AssignmentNotifyHandler : IRequestHandler<AssignmentNotifyRequest, Unit>
    {
        private readonly CacheManipulatorCommon cacheManipulator;

        private readonly DynamicRepository dynamicRepository;

        private readonly MyAssignmentGenerator myAssignmentGenerator;

        private readonly MessageFactory messageFactory;

        private readonly CommandRepository commandRepository;

        private readonly NotifyServices notify;

        public AssignmentNotifyHandler(
            CacheManipulatorCommon cacheManipulator,
            DynamicRepository dynamicRepository,
            MyAssignmentGenerator myAssignmentGenerator,
            MessageFactory messageFactory,
            CommandRepository commandRepository,
            NotifyServices notify)
        {
            this.cacheManipulator = cacheManipulator;
            this.dynamicRepository = dynamicRepository;
            this.myAssignmentGenerator = myAssignmentGenerator;
            this.messageFactory = messageFactory;
            this.commandRepository = commandRepository;
            this.notify = notify;
        }

        public async Task<Unit> Handle(AssignmentNotifyRequest request, CancellationToken cancellationToken)
        {
            var messages = await GenerateMessages(request);
            var messageModels = messageFactory.Transform(messages);

            await commandRepository.InsertMessage(messageModels, request.SessionId);

            await notify.Notify(messageModels);

            return await Task.FromResult(Unit.Value);
        }

        private async Task<IList<ChangedValue>> GenerateMessages(AssignmentNotifyRequest request)
        {
            var changedValuesResult = new List<ChangedValue>();
            var oldSnapshot = await cacheManipulator.GetMultipleData<AssignmentSnapshot>(request.redisKey);
            var preMatchLeagueIdUpdated = oldSnapshot.Select(item => item.PreMatchLeagueId).ToList();
            var newSnapshot = await GetAssignmentNewSnapshot(preMatchLeagueIdUpdated);
            var prematchIdUpdated = newSnapshot.Select(p => p.PreMatchLeagueId);

            foreach (var prematchId in prematchIdUpdated)
            {
                var oldAssignment = oldSnapshot.First(snap => snap.PreMatchLeagueId == prematchId);
                var newAssignment = newSnapshot.First(snap => snap.PreMatchLeagueId == prematchId);
                var changedValues = myAssignmentGenerator.Create(oldAssignment, newAssignment);
                changedValuesResult.AddRange(changedValues);
            }

            return changedValuesResult;
        }

        private async Task<IList<AssignmentSnapshot>> GetAssignmentNewSnapshot(IList<int> prematchIds)
        {
            var assignmentOldValue = new List<AssignmentSnapshot>();
            foreach (var id in prematchIds)
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
