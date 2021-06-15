namespace OWP.PreMatchNotification.NewLeague.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using OWP.PreMatchNotification.Models;
    using OWP.PreMatchNotification.NewLeague.Services;
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

        private readonly ChangedValueAggregator changedValueAggregator;

        private readonly MessageFactory messageFactory;

        private readonly CommandRepository commandRepository;

        private readonly NotifyServices notify;

        public NewLeagueNotifyHandler(
            CacheManipulator cacheManipulator,
            Repository queryRepository,
            NewLeagueValueGenerator newLeagueGenerator,
            ChangedValueAggregator changedValueAggregator,
            MessageFactory messageFactory,
            CommandRepository commandRepository,
            NotifyServices notify)
        {
            this.cacheManipulator = cacheManipulator;
            this.queryRepository = queryRepository;
            this.newLeagueGenerator = newLeagueGenerator;
            this.changedValueAggregator = changedValueAggregator;
            this.messageFactory = messageFactory;
            this.commandRepository = commandRepository;
            this.notify = notify;
        }

        public async Task<Unit> Handle(NewLeagueNotifyRequest request, CancellationToken cancellationToken)
        {
            var messages = await GenerateMessages(request);
            var aggregatedChangedValues = changedValueAggregator.Aggregate(messages);
            var messageModels = messageFactory.Transform(aggregatedChangedValues);

            await commandRepository.InsertMessage(messageModels, request.SessionId);

            await notify.Notify(messageModels);

            return await Task.FromResult(Unit.Value);
        }

        private async Task<IList<ChangedValue>> GenerateMessages(NewLeagueNotifyRequest request)
        {
            var oldSnapshot = await cacheManipulator.GetOldPreMatchLeagueSnapshot(request.SessionId);
            var newSnapshot = await queryRepository.GetPreMatchLeagueNewSnapshot(request.FromDate, request.ToDate);
            var preMatchAssignees = await queryRepository.GetPreMatchAssignee();
            var messages = newLeagueGenerator.Create(oldSnapshot, newSnapshot, preMatchAssignees);

            await cacheManipulator.DeleteOldPreMatchLeagueSnapshot(request.SessionId);

            return messages.ToList();
        }
    }
}