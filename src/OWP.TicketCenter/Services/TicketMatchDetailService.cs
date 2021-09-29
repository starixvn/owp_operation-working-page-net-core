using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    public interface TicketMatchDetailService
    {
        Task<TicketMatchDetailViewModel> GetTicketMatchDetail(int ticketId);
    }

    public class TicketMatchDetailServiceImp : TicketMatchDetailService
    {
        private readonly Repository repository;

        private readonly TicketDetailService ticketDetailService;

        private readonly IMapper mapper;

        public TicketMatchDetailServiceImp(Repository repository, TicketDetailService ticketDetailService, IMapper mapper)
        {
            this.repository = repository;
            this.ticketDetailService = ticketDetailService;
            this.mapper = mapper;
        }

        public async Task<TicketMatchDetailViewModel> GetTicketMatchDetail(int ticketId)
        {
            var ticketMatch = await repository.GetTicketMatchDetail(ticketId);
            var ticketFirst = ticketMatch.Any()
                ? ticketMatch.First()
            : new EventTicketMatchModel();

            var ticketMatchViewModel = mapper.Map<TicketMatchDetailViewModel>(ticketFirst);
            var messages = ticketDetailService.BuildMessage(ticketMatch?.ToList());

            var recipients = await repository.GetRecipientByTicketId(ticketId);
            ticketMatchViewModel.Recipients = recipients?.ToList();
            ticketMatchViewModel.Messages = messages?.ToList();

            return ticketMatchViewModel;
        }
    }
}
