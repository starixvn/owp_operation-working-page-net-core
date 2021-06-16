namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using System;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Services;
    using OWP.PreMatchNotification.Services;
    using OWP.PreMatchNotification.Models;
    using System.Collections.Generic;

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

        private readonly ChangedValueGenerator changedValueGenerator;

        private readonly ChangedValueAggregator changedValueAggregator;

        private readonly MessageFactory messageFactory;

        private readonly CommandRepository commandRepository;

        private readonly NotifyServices notify;

        public MatchInfoNotifyHandler(
            CacheManipulator cacheManipulator,
            Repository queryRepository,
            ChangedValueGenerator changedValueGenerator,
            ChangedValueAggregator changedValueAggregator,
            MessageFactory messageFactory,
            CommandRepository commandRepository,
            NotifyServices notify)
        {
            this.cacheManipulator = cacheManipulator;
            this.queryRepository = queryRepository;
            this.changedValueGenerator = changedValueGenerator;
            this.changedValueAggregator = changedValueAggregator;
            this.messageFactory = messageFactory;
            this.commandRepository = commandRepository;
            this.notify = notify;
        }

        public async Task<Unit> Handle(MatchInfoNotifyRequest request, CancellationToken cancellationToken)
        {
            var oldSnapshot = await cacheManipulator.GetOldInfoSiteMatchSnapshot(request.SessionId);
            var newSnapshot = await queryRepository.GetInfoSiteMatchNewSnapshot(request.FromDate, request.ToDate);
            var messages = changedValueBySnapshot.Create(oldSnapshot, newSnapshot);

            await cacheManipulator.DeleteOldInfoSiteMatchSnapshot(request.SessionId);

            return await Task.FromResult(Unit.Value);
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

        private async Task<IList<ChangedValue>> GenerateMessages(MatchInfoNotifyRequest request)
        {
            var oldSnapshot = await cacheManipulator.GetOldInfoSiteMatchSnapshot(request.SessionId);
            var newSnapshot = await queryRepository.GetInfoSiteMatchNewSnapshot(request.FromDate, request.ToDate);
            var preMatchAssignees = await queryRepository.GetPreMatchAssignee();
            var messages = changedValueGenerator.Create(oldSnapshot, newSnapshot, preMatchAssignees);

            await cacheManipulator.DeleteOldPreMatchLeagueSnapshot(request.SessionId);

            return messages.ToList();
        }
    }
}