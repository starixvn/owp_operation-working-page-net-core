using Fanex.Data.Repository;
using MediatR;
using OWP.EventAssignment.Criteria;
using OWP.EventAssignment.Models;

namespace OWP.EventAssignment.Handlers
{
    public class GetTicketListRequest : IRequest<IList<TicketListModel>>
    {
        public int OWPMatchId { get; set; }
    }

    public class GetTicketListHandler : IRequestHandler<GetTicketListRequest, IList<TicketListModel>>
    {
        private readonly IDynamicRepository repository;

        public async Task<IList<TicketListModel>> Handle(GetTicketListRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<IList<TicketListModel>> GetTicketFromDB(int OWPMatchId)
        => (await repository.FetchAsync<TicketListModel>(new GetTicketListCriteria
        {
            OWPMatchId = OWPMatchId
        })).ToList();
    }
}