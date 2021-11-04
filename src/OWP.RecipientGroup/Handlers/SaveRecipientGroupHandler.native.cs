namespace OWP.RecipientGroup.Handlers
{
    using MediatR;
    using Models;
    using OWP.RecipientGroup.Const;
    using OWP.RecipientGroup.Criteria;
    using Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class SaveRecipientGroupRequest : IRequest<string>
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public IEnumerable<Recipient> Recipients { get; set; }
    }

    public class SaveRecipientGroupHandler : IRequestHandler<SaveRecipientGroupRequest, string>
    {
        private readonly CommandRepository commandRepository;

        public SaveRecipientGroupHandler(CommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public async Task<string> Handle(SaveRecipientGroupRequest request, CancellationToken cancellationToken)
        {
            await commandRepository.SaveRecipientGroup(request);
            await InsertRecipientToMessageReader(request);
            await RemoveToMessageReader(request);
            return string.Empty;
        }

        public async Task InsertRecipientToMessageReader(SaveRecipientGroupRequest request)
        {
            var userId = request.Recipients
                .Where(r => r.RecipientGroupId == GroupConst.DefaultGroupIdForNewRecipientIndividual)
                .Select(x => x.RecipientId);

            if (userId.Any())
            {
                var criteria = new InsertMessageReaderCriteria
                {
                    GroupId = request.GroupId,
                    UserIds = string.Join(",", userId)
                };

                await commandRepository.InsertRecipientToMessageReader(criteria);
            }
        }

        public async Task RemoveToMessageReader(SaveRecipientGroupRequest request)
        {
            var userId = request.Recipients.Select(x => x.RecipientId);

            var criteria = new RemoveMessageReaderCriteria()
            {
                GroupId = request.GroupId,
                UserIds = string.Join(",", userId)
            };

            await commandRepository.RemoveRecipientToMessageReader(criteria);
        }
    }
}