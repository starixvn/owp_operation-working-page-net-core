namespace OWP.OtherSport.EventAssignment
{
    using MediatR;
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.EventAssignment.Services;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    public class QueryEventAssignmentRequest : IRequest<IEnumerable<EventAssignmentModel>>
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Employee { get; set; }

        public IList<string> Sports { get; set; }

        public IList<string> MatchStatuses { get; set; }
}
   
    public class Handler : IRequestHandler<QueryEventAssignmentRequest, IEnumerable<EventAssignmentModel>>
    {
        private readonly CommandRepository commandRepository;
        public Handler(CommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public async Task<IEnumerable<EventAssignmentModel>> Handle(QueryEventAssignmentRequest request, CancellationToken cancellationToken)
        {
            return await commandRepository.Query(request);          
        }
    }
}
