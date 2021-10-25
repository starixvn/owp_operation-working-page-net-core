using System.Linq;
using OWP.TicketCenter.Notification.Services;

namespace OWP.TicketCenter.Notification
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class NewMessageIncomeRequest : IRequest<bool>
    {

        public int TicketId { get; set; }
    }

    public class NewMessageIncomeHandler : IRequestHandler<NewMessageIncomeRequest, bool>
    {
        private readonly Repository repository;

        private readonly NotifyServices notify;

        public NewMessageIncomeHandler(
            Repository repository,
            NotifyServices notify)
        {
            this.repository = repository;
            this.notify = notify;
        }

        public async Task<bool> Handle(NewMessageIncomeRequest request, CancellationToken cancellationToken)
        {
            var userIds = await repository.GetUserIdsByTicketId(request.TicketId);
            var userIdsString = userIds.Select(u => u.ToString());

            await notify.NotifyNewMessageIncome(userIdsString.ToList());
            return await Task.FromResult(true);
        }
    }
}
