using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    public class TicketLeagueDetailRequest : IRequest<TicketLeagueDetailViewModel>
    {
        public int TicketId { get; set; }
    }

    public class TicketLeagueDetailHandler: IRequestHandler<TicketLeagueDetailRequest, TicketLeagueDetailViewModel>
    {
        private readonly Repository repository;

        public TicketLeagueDetailHandler(Repository repository)
        {
            this.repository = repository;
        }
        public async Task<TicketLeagueDetailViewModel> Handle(TicketLeagueDetailRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
