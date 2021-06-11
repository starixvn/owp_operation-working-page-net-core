namespace OWP.PreMatchNotification.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using OWP.PreMatchNotification.Services;

    public class NewLeagueNotifyRequest : IRequest<Unit>
    {
        public string SessionId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class NewLeagueNotifyHandler : IRequestHandler<NewLeagueNotifyRequest, Unit>
    {
        private readonly CacheManipulator cacheManipulator;
        private readonly Repository queryRepository;
        private readonly NewLeagueValueGenerator newLeagueGenerator;

        public NewLeagueNotifyHandler(
            CacheManipulator cacheManipulator,
            Repository queryRepository,
            NewLeagueValueGenerator newLeagueGenerator)
        {
            this.cacheManipulator = cacheManipulator;
            this.queryRepository = queryRepository;
            this.newLeagueGenerator = newLeagueGenerator;
        }

        public async Task<Unit> Handle(NewLeagueNotifyRequest request, CancellationToken cancellationToken)
        {
            var oldSnapshot = await cacheManipulator.GetOldPreMatchLeagueSnapshot(request.SessionId);
            var newSnapshot = await queryRepository.GetPreMatchLeagueNewSnapshot(request.FromDate, request.ToDate);
            var messages = newLeagueGenerator.Create(oldSnapshot, newSnapshot);

            await cacheManipulator.DeleteOldPreMatchLeagueSnapshot(request.SessionId);

            return await Task.FromResult(Unit.Value);
        }
    }
}