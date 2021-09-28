using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OWP.TicketCenter.ViewModels;
using OWP.Utils;

namespace OWP.TicketCenter.Services
{
    public interface TicketDetailService
    {
        IList<MessageDetail> BuildMessage<T>(IList<T> ticketSource);
    }

    public class TicketDetailServiceImp : TicketDetailService
    {
        private readonly IMapper mapper;

        public TicketDetailServiceImp(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public IList<MessageDetail> BuildMessage<T>(IList<T> ticketSource)
        {
            var message = ticketSource
                .Select(t => mapper.Map<MessageDetail>(t))
                .DistinctBy(m => new { m.UserId, m.Username, m.CreatedDate, m.MessageId, m.Message })
                .OrderBy(m=>m.CreatedDate);
            return message.ToList();
        }
    }
}
