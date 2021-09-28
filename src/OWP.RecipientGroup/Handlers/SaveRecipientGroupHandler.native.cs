namespace OWP.RecipientGroup.Handlers
{
    using MediatR;
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class SaveRecipientGroupRequest : IRequest<string>
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public IEnumerable<RecipientGroup> Recipients { get; set; }
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
            return string.Empty;
        }
    }
}