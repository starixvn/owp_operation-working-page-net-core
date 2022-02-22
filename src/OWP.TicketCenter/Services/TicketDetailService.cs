using AutoMapper;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    public interface TicketDetailService
    {
        IEnumerable<MessageDetailViewModel> BuildMessage<T>(IList<T> ticketSource);
    }

    public class TicketDetailServiceImp : TicketDetailService
    {
        private readonly IMapper mapper;

        public TicketDetailServiceImp(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public IEnumerable<MessageDetailViewModel> BuildMessage<T>(IList<T> ticketSource)
        {
            var messages = ticketSource
                .Select(t => mapper.Map<MessageDetailViewModel>(t))
                .DistinctBy(m => new { m.UserId, m.Username, m.CreatedDate, m.MessageId, m.Message })
                .OrderBy(m => m.CreatedDate);
            return messages;
        }
    }
}