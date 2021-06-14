namespace OWP.PreMatchNotification.LinkedMatch.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Services;
    using System;

    public class LinkedMatchNotifyRequest : IRequest<Unit>
    {
        public string SessionId { get; set; }

        public DateTime EventDate { get; set; }
    }

    public class LinkedMatchNotifyHandler : IRequestHandler<LinkedMatchNotifyRequest, Unit>
    {
        private readonly CacheManipulator cacheManipulator;
        private readonly Repository queryRepository;
        private readonly ChangedMatchChangedValueGenerator changedValueBySnapshot;

        public LinkedMatchNotifyHandler(
            CacheManipulator cacheManipulator,
            Repository queryRepository,
            ChangedMatchChangedValueGenerator changedValueBySnapshot)
        {
            this.cacheManipulator = cacheManipulator;
            this.queryRepository = queryRepository;
            this.changedValueBySnapshot = changedValueBySnapshot;
        }

        public async Task<Unit> Handle(LinkedMatchNotifyRequest request, CancellationToken cancellationToken)
        {
            var oldSnapshot = await cacheManipulator.GetOldLGAMatchSnapshot(request.SessionId, request.EventDate);
            var newSnapshot = await queryRepository.GetInfoSiteMatchNewSnapshot(request.FromDate, request.ToDate);
            var messages = changedValueBySnapshot.Create(oldSnapshot, newSnapshot);

            await cacheManipulator.DeleteOldInfoSiteMatchSnapshot(request.SessionId);

            return await Task.FromResult(Unit.Value);
        }
    }
}