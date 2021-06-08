namespace OWP.PreMatchNotification
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using OWP.PreMatchNotification.Services;
    using System;

    public class NotifyRequest : IRequest<Unit>
    {
        public string SessionId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class NotifyHandler : IRequestHandler<NotifyRequest, Unit>
    {
        private readonly ChangedValueByCacheAndDatabase changedValueByCacheAndDatabase;
        private readonly Repository repository;

        public NotifyHandler(
            ChangedValueByCacheAndDatabase changedValueByCacheAndDatabase,
            Repository repository)
        {
            this.changedValueByCacheAndDatabase = changedValueByCacheAndDatabase;
            this.repository = repository;
        }

        public async Task<Unit> Handle(NotifyRequest request, CancellationToken cancellationToken)
        {
            var messages = await changedValueByCacheAndDatabase.Create(request.SessionId, request.FromDate, request.ToDate);

            return await Task.FromResult(Unit.Value);
        }
    }
}