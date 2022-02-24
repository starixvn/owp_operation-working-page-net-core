using MediatR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class TicketTypeRequest : IRequest<TicketTypeModel>
    {
    }

    public class TicketTypeHandler : IRequestHandler<TicketTypeRequest, TicketTypeModel>
    {
        private readonly Repository repository;

        public TicketTypeHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<TicketTypeModel> Handle(TicketTypeRequest request, CancellationToken cancellationToken)
        {
            var parentType = (await repository.GetTicketTypes()).ToList();
            var subType = (await repository.GetAllTicketSubTypes()).ToList();

            return new TicketTypeModel { Parent = parentType, Child = subType };
        }
    }
}