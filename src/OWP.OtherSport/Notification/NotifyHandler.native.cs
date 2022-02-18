namespace OWP.OtherSport.Notification
{
    
    
    using MediatR;
    using OWP.OtherSport.Notification.Services;

    public class NotifyRequest : IRequest<bool>
    {
        public string SessionId { get; set; }

        public string EventDate { get; internal set; }
    }

    public class NotifyHandler : IRequestHandler<NotifyRequest, bool>
    {
        private readonly ChangedValueByCacheAndDatabase changedValueByCacheAndDatabase;
        private readonly AssigneeChangedValueFilter assigneeChangedValueFilter;
        private readonly ChangedValueAggregator changedValueAggregator;
        private readonly MessageFactory messageFactory;
        private readonly CommandRepository commandRepository;
        private readonly NotifyServices notify;

        public NotifyHandler(
            ChangedValueByCacheAndDatabase changedValueByCacheAndDatabase,
            AssigneeChangedValueFilter assigneeChangedValueFilter,
            ChangedValueAggregator changedValueAggregator,
            MessageFactory messageFactory,
            CommandRepository commandRepository,
            NotifyServices notify)
        {
            this.changedValueByCacheAndDatabase = changedValueByCacheAndDatabase;
            this.assigneeChangedValueFilter = assigneeChangedValueFilter;
            this.changedValueAggregator = changedValueAggregator;
            this.messageFactory = messageFactory;
            this.commandRepository = commandRepository;
            this.notify = notify;
        }

        public async Task<bool> Handle(NotifyRequest request, CancellationToken cancellationToken)
        {
            var messages = await changedValueByCacheAndDatabase.Create(request.SessionId, request.EventDate);
            var assigneeChangedValues = await assigneeChangedValueFilter.Filter(messages);

            var aggregatedChangedValues = changedValueAggregator.Aggregate(assigneeChangedValues);
            var messageModels = messageFactory.Transform(aggregatedChangedValues);

            await commandRepository.InsertMessage(messageModels, request.SessionId);

            await notify.Notify(messageModels);

            return await Task.FromResult(true);
        }
    }
}
