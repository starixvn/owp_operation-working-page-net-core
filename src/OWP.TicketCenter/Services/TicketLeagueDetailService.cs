using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    public interface TicketLeagueDetailService
    {
        Task<TicketLeagueDetailViewModel> GetTicketLeagueDetail(TicketLeagueDetailRequest request);
    }

    public class TicketLeagueDetailServiceImp : TicketLeagueDetailService
    {
        private readonly Repository repository;

        private readonly TicketDetailService ticketDetailService;

        private readonly IMapper mapper;

        public TicketLeagueDetailServiceImp(Repository repository, TicketDetailService ticketDetailService, IMapper mapper)
        {
            this.repository = repository;
            this.ticketDetailService = ticketDetailService;
            this.mapper = mapper;
        }

        public async Task<TicketLeagueDetailViewModel> GetTicketLeagueDetail(TicketLeagueDetailRequest request)
        {
            var ticketId = request.TicketId;
            var actorId = request.ActorId;

            var ticketLeague = await repository.GetTicketLeagueDetail(ticketId);
            var ticketFirst = ticketLeague.Any()
                ? ticketLeague.First()
                : new EventTicketLeagueModel();

            var ticketLeagueViewModel = mapper.Map<TicketLeagueDetailViewModel>(ticketFirst);
            var messages = ticketDetailService.BuildMessage(ticketLeague?.ToList());

            await repository.UpdateSingleRecipientReader(ticketId, true, actorId);

            var recipients = await repository.GetRecipientByTicketId(ticketId);
            var issueCreator = await repository.GetIssueCreator(ticketId);

            ticketLeagueViewModel.CreatorInfo = issueCreator;
            ticketLeagueViewModel.Messages = messages?.ToList();
            ticketLeagueViewModel.Recipients = recipients?.ToList();

            return ticketLeagueViewModel;
        }
    }
}
