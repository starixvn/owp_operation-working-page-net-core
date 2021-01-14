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
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
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
