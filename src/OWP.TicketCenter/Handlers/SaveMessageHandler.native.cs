namespace OWP.TicketCenter.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class SaveMessageRequest : IRequest<bool>
    {
        public int MessageId { get; set; }

        public bool IsSaved { get; set; }
    }

    public class SaveMessageHandler : IRequestHandler<SaveMessageRequest, bool>
    {
        private readonly Repository repository;

        public SaveMessageHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(SaveMessageRequest request, CancellationToken cancellationToken)
        {
            await repository.UpdateIsSavedMessage(request);

            return await Task.FromResult(true);
        }
    }
}