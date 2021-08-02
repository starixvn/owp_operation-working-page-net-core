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

        private readonly EventAssignmentBuilder eventAssignmentBuilder;

        public Handler(CommandRepository commandRepository, EventAssignmentBuilder assignmentBuilder)
        {
            this.commandRepository = commandRepository;
            this.eventAssignmentBuilder = assignmentBuilder;
        }

        public async Task<IEnumerable<EventAssignmentModel>> Handle(QueryEventAssignmentRequest request, CancellationToken cancellationToken)
        {
            var assignmentSnapshot = await commandRepository.Query(request);
            return eventAssignmentBuilder.Build(assignmentSnapshot, request.Employee);
        }
    }
}
