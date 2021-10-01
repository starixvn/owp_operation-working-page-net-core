using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class NewTicketIssueLeagueRequest : NewTicketModelBase, IRequest<int>
    {
        public int OwpLeagueId { get; set; }

    }

    public class NewTicketIssueLeagueHandler : IRequestHandler<NewTicketIssueLeagueRequest, int>
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        public NewTicketIssueLeagueHandler(Repository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<int> Handle(NewTicketIssueLeagueRequest request, CancellationToken cancellationToken)
        {
            var ticketId = await repository.BuildTicketId(request.TicketType);
            request.TicketId = ticketId;
            await repository.AddNewTicketIssueLeague(request);
            await repository.InsertIssueCreator(ticketId, request.CreatorId);

            var messages = new List<NewMessageRequestModel>();
            foreach (var file in request.Files)
            {
                messages.Add(new NewMessageRequestModel()
                {
                    TicketId = ticketId,
                    CreatorId = request.CreatorId,
                    Message = file.Title,
                    FileId = file.FileId
                });
            }
            var newMessage = mapper.Map<NewMessageRequestModel>(request);
            messages.Add(newMessage);

            foreach (var message in messages)
            {
                await repository.AddNewMessage(message);
            }
            return ticketId;
        }
    }
}
