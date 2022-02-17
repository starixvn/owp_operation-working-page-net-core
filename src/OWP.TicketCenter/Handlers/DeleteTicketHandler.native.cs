namespace OWP.TicketCenter.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class DeleteTicketRequest : IRequest<bool>
    {
        public int UserId { get; set; }

        public int TicketId { get; set; }
    }

    public class DeleteTicketHandler : IRequestHandler<DeleteTicketRequest, bool>
    {
        private readonly Repository repository;

        public DeleteTicketHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(DeleteTicketRequest request, CancellationToken cancellationToken)
        {
            await repository.DeleteTicket(new Criteria.DeleteTicketCriteria { UserId = request.UserId, TicketId = request.TicketId, EventDate = DateTime.Now });

            return await Task.FromResult(true);
        }
    }
}