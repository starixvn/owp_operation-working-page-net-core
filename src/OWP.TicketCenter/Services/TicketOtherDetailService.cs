using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    
    public interface TicketOtherDetailService
    {
        Task<TicketOtherDetailViewModel> GetTicketOtherDetail(int ticketId);
    }

    public class TicketOtherDetailServiceImp : TicketOtherDetailService
    {
        private readonly Repository repository;

        private readonly TicketDetailService ticketDetailService;

        private readonly IMapper mapper;

        public TicketOtherDetailServiceImp(Repository repository, TicketDetailService ticketDetailService, IMapper mapper)
        {
            this.repository = repository;
            this.ticketDetailService = ticketDetailService;
            this.mapper = mapper;
        }

        public async Task<TicketOtherDetailViewModel> GetTicketOtherDetail(int ticketId)
        {
            var ticketOther = await repository.GetTicketOtherDetail(ticketId);
            var ticketFirst = ticketOther.First();
            var ticketOtherViewModel = mapper.Map<TicketOtherDetailViewModel>(ticketFirst);
            var message = ticketDetailService.BuildMessage(ticketOther.ToList());
            ticketOtherViewModel.Messages = message.ToList();
            return ticketOtherViewModel;
        }
    }
}
