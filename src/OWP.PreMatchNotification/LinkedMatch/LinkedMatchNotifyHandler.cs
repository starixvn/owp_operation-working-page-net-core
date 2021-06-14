namespace OWP.PreMatchNotification.LinkedMatch
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Services;
    using System;
    using System.Linq;

    public class LinkedMatchNotifyRequest : IRequest<Unit>
    {
        public string SessionId { get; set; }

        public DateTime EventDate { get; set; }
    }

    public class LinkedMatchNotifyHandler : IRequestHandler<LinkedMatchNotifyRequest, Unit>
    {
        private readonly CacheManipulator cacheManipulator;
        private readonly Repository queryRepository;
        private readonly ChangedLinkedMatchDetector changedValueBySnapshot;

        public LinkedMatchNotifyHandler(
            CacheManipulator cacheManipulator,
            Repository queryRepository,
            ChangedLinkedMatchDetector changedValueBySnapshot)
        {
            this.cacheManipulator = cacheManipulator;
            this.queryRepository = queryRepository;
            this.changedValueBySnapshot = changedValueBySnapshot;
        }

        public async Task<Unit> Handle(LinkedMatchNotifyRequest request, CancellationToken cancellationToken)
        {
            var oldMatches = await cacheManipulator.GetOldLGAMatchSnapshot(request.SessionId, request.EventDate);
            var newMatches = await queryRepository.GetLGAMatchNewSnapshot(request.EventDate);
            var prematchLeagues = await queryRepository.GetInfositePrematch(request.EventDate);
            
            var messages = changedValueBySnapshot.Create(oldMatches, newMatches, prematchLeagues);

            await cacheManipulator.DeleteAssignmentSnapshot(request.SessionId, request.EventDate);

            return await Task.FromResult(Unit.Value);
        }
    }
}