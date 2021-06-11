namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using System;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Services;

    public class MatchInfoNotifyRequest : IRequest<Unit>
    {
        public string SessionId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class MatchInfoNotifyHandler : IRequestHandler<MatchInfoNotifyRequest, Unit>
    {
        private readonly CacheManipulator cacheManipulator;
        private readonly Repository queryRepository;
        private readonly ChangedMatchChangedValueGenerator changedValueBySnapshot;

        public MatchInfoNotifyHandler(
            CacheManipulator cacheManipulator,
            Repository queryRepository,
            ChangedMatchChangedValueGenerator changedValueBySnapshot)
        {
            this.cacheManipulator = cacheManipulator;
            this.queryRepository = queryRepository;
            this.changedValueBySnapshot = changedValueBySnapshot;
        }

        public async Task<Unit> Handle(MatchInfoNotifyRequest request, CancellationToken cancellationToken)
        {
            var oldSnapshot = await cacheManipulator.GetOldInfoSiteMatchSnapshot(request.SessionId);
            var newSnapshot = await queryRepository.GetInfoSiteMatchNewSnapshot(request.FromDate, request.ToDate);
            var messages = changedValueBySnapshot.Create(oldSnapshot, newSnapshot);

            await cacheManipulator.DeleteOldInfoSiteMatchSnapshot(request.SessionId);

            return await Task.FromResult(Unit.Value);
        }
    }
}