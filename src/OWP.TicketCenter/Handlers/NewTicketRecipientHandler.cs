using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class NewTicketRecipientRequest : IRequest<bool>
    {
        public int TicketId { get; set; }

        public IList<Recipient> Recipient { get; set; }

    }

    public class NewTicketRecipientHandler : IRequestHandler<NewTicketRecipientRequest, bool>
    {
        private readonly Repository repository;
        public NewTicketRecipientHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewTicketRecipientRequest request, CancellationToken cancellationToken)
        {
            var recipientDic = request.Recipient
                .GroupBy(r => r.RecipientType)
                .ToDictionary(k => k.Key, x => x.ToList());
            foreach (var key in recipientDic.Keys)
            {
                var recipients = recipientDic[key];
                foreach (var recipient in recipients)
                {
                    var recipientIds = recipient.RecipientIds
                        .Select(r => r);
                    await repository.AddNewRecipients(request.TicketId, string.Join(',', recipientIds), recipient.RecipientType);
                }
            }
            return await Task.FromResult(true);
        }
    }
}
