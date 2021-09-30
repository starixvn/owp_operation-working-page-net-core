using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{

    public class NewTicketIssueOtherRequest : NewTicketModelBase, IRequest<int>
    {

        public string Subject { get; set; }
    }

    public class NewTicketIssueOtherHandler : IRequestHandler<NewTicketIssueOtherRequest, int>
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        public NewTicketIssueOtherHandler(Repository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<int> Handle(NewTicketIssueOtherRequest request, CancellationToken cancellationToken)
        {
            var ticketId = await repository.BuildTicketId(request.TicketType);
            request.TicketId = ticketId;
            await repository.AddNewTicketIssueOther(request);
            var newMessage = mapper.Map<NewMessageRequestModel>(request);
            await repository.AddNewMessage(newMessage);
            await repository.AddFileInfo(newMessage);
            return ticketId;
        }
    }
}
