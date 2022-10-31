namespace OWP.TicketCenter.Handlers
{
    using MediatR;
    using OWP.TicketCenter.Middleware.Models;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteTicketRequest : PermissionCheckedModel, IRequest<bool>
    {

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
            await repository.DeleteTicket(new Criteria.DeleteTicketCriteria { UserId = request.ActorId, TicketId = request.TicketId, EventDate = DateTime.Now });

            return await Task.FromResult(true);
        }
    }
}