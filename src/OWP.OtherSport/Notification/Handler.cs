namespace OWP.OtherSport.Notification
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using OWP.OtherSport.Notification.Services;

    public class NotifyRequest : IRequest<bool>
    {
        public string SessionId { get; set; }

        public string EventDate { get; set; }
    }

    public class Handler : IRequestHandler<NotifyRequest, bool>
    {
        private readonly ChangedValueByCacheAndDatabase changedValueByCacheAndDatabase;
        private readonly AssigneeChangedValueFilter assigneeChangedValueFilter;
        private readonly WatcherChangedValueGenerator watcherChangedValueGenerator;
        private readonly ChangedValueAggregator changedValueAggregator;
        private readonly MessageFactory messageFactory;
        private readonly CommandRepository commandRepository;
        private readonly NotifyServices notify;

        public Handler(
            ChangedValueByCacheAndDatabase changedValueByCacheAndDatabase,
            AssigneeChangedValueFilter assigneeChangedValueFilter,
            WatcherChangedValueGenerator watcherChangedValueGenerator,
            ChangedValueAggregator changedValueAggregator,
            MessageFactory messageFactory,
            CommandRepository commandRepository,
            NotifyServices notify)
        {
            this.changedValueByCacheAndDatabase = changedValueByCacheAndDatabase;
            this.assigneeChangedValueFilter = assigneeChangedValueFilter;
            this.watcherChangedValueGenerator = watcherChangedValueGenerator;
            this.changedValueAggregator = changedValueAggregator;
            this.messageFactory = messageFactory;
            this.commandRepository = commandRepository;
            this.notify = notify;
        }

        public async Task<bool> Handle(NotifyRequest request, CancellationToken cancellationToken)
        {
            var messages = await changedValueByCacheAndDatabase.Create(request.SessionId, request.EventDate);
            var assigneeChangedValues = await assigneeChangedValueFilter.Filter(messages);
            var watcherChangedValues = await watcherChangedValueGenerator.Create(messages, assigneeChangedValues);
            var allChangedValues = assigneeChangedValues.Union(watcherChangedValues).ToList();

            var aggregatedChangedValues = changedValueAggregator.Aggregate(allChangedValues);
            var messageModels = messageFactory.Transform(aggregatedChangedValues);

            await commandRepository.InsertMessage(messageModels, request.SessionId);

            notify.Notify(messageModels);

            return await Task.FromResult(true);
        }
    }
}