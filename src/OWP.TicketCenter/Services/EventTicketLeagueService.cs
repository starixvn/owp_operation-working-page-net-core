using AutoMapper;
using OWP.TicketCenter.Builder;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    public interface EventTicketLeagueService
    {
        Task<IList<TicketLeagueDetailViewModel>> GetAllLeague(QueryEventTicketsRequest request);
    }

    public class EventTicketLeagueServiceImp : EventTicketLeagueService
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        public EventTicketLeagueServiceImp(Repository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<TicketLeagueDetailViewModel>> GetAllLeague(QueryEventTicketsRequest request)
        {
            var ticketLeague = await repository.GetEventTicketLeague(request);

            var leagueDic = ticketLeague
                .GroupBy(t => t.TicketId)
                .ToDictionary(t => t.Key, v => v.ToList());

            var leaguesViewModel = new List<TicketLeagueDetailViewModel>();
            foreach (var key in leagueDic.Keys)
            {
                var leagueFirst = leagueDic[key].First();
                var leagueViewModel = mapper.Map<TicketLeagueDetailViewModel>(leagueFirst);

                var messageModel = BuildMessageModels(leagueDic[key]);
                var messageDetail = TwoLastCommentBuilder.GetTwoLastComment(messageModel.ToList());

                leagueViewModel.Messages = messageDetail;
                leagueViewModel.IsRead = leagueDic[key].All(l => l.IsRead);

                leaguesViewModel.Add(leagueViewModel);
                leagueViewModel.EventType = (int)EventType.League;
            }
            return leaguesViewModel;
        }

        private IEnumerable<MessageResponseModel> BuildMessageModels(IEnumerable<EventTicketLeagueModel> models)
        => models
            .Select(x => mapper.Map<MessageResponseModel>(x))
            .ToList();
    }
}