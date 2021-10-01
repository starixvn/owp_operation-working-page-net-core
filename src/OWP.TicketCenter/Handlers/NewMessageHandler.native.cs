using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class NewMessageRequest : IRequest<IList<int>>
    {
        public int TicketId { get; set; }

        public string Message { get; set; }

        public int CreatorId { get; set; }

        public int? FileId { get; set; }
    }

    public class NewMessageHandler : IRequestHandler<NewMessageRequest, IList<int>>
    {
        private readonly Repository repository;

        public NewMessageHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<int>> Handle(NewMessageRequest request, CancellationToken cancellationToken)
        {
            var messageIdsResponse = new List<int>();
            var message = new NewMessageRequestModel
            {
                TicketId = request.TicketId,
                Message = request.Message,
                CreatorId = request.CreatorId
            };
            var messageId = await repository.AddNewMessage(message);
            messageIdsResponse.Add(messageId);
            if (request.FileId.HasValue)
            {
                message.Files.Add(new()
                {
                    FileId = request.FileId,
                    Title = request.Message
                });
                var messageIds=await repository.AddFileInfo(message);
                messageIdsResponse.AddRange(messageIds);
            }

            return messageIdsResponse;
        }
    }
}
