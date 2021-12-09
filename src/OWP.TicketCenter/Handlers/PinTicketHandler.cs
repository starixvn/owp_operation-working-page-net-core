namespace OWP.TicketCenter.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class PinTicketRequest : IRequest<Unit>
    {
        public int UserId { get; set; }

        public int TicketId { get; set; }

        public bool IsPinned { get; set; }
    }

    public class PinTicketHandler : IRequestHandler<PinTicketRequest, Unit>
    {
        private readonly Repository repository;

        public PinTicketHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(PinTicketRequest request, CancellationToken cancellationToken)
        {
            await repository.PinTicket(new Criteria.PinTicketCriteria { ActorId = request.UserId, TicketId = request.TicketId, IsPinned = request.IsPinned });

            return await Task.FromResult(Unit.Value);
        }
    }
}