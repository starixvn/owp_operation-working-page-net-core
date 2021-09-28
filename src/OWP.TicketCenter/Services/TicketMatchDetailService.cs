using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    public interface TicketMatchDetail
    {
        Task<TicketMatchDetailViewModel> GetTicketMatchDetail(int ticketId);
    }

    public class TicketMatchDetailImp : TicketMatchDetail
    {
        private readonly Repository repository;

        private readonly TicketDetailService ticketDetailService;

        private readonly IMapper mapper;

        public TicketMatchDetailImp(Repository repository, TicketDetailService ticketDetailService, IMapper mapper)
        {
            this.repository = repository;
            this.ticketDetailService = ticketDetailService;
            this.mapper = mapper;
        }

        public async Task<TicketMatchDetailViewModel> GetTicketMatchDetail(int ticketId)
        {
            var ticketMatch = await repository.GetTicketMatchDetail(ticketId);
            var ticketFirst = ticketMatch.First();
            var ticketMatchViewModel = mapper.Map<TicketMatchDetailViewModel>(ticketFirst);
            var message = ticketDetailService.BuildMessage(ticketMatch.ToList());
            ticketMatchViewModel.Messages = message.ToList();
            return ticketMatchViewModel;
        }
    }
}
