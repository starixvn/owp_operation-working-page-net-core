using MediatR;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.EventAssignment.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OWP.OtherSport.EventAssignment
{
    public class QueryModel : IRequest<Response>
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class Response
    {
        public Response()
        {
            EventAssignmentModels = new List<EventAssignmentModel>();
        }

        public List<EventAssignmentModel> EventAssignmentModels { get; set; }
    }
    public class Handler : IRequestHandler<QueryModel, Response>
    {
        private readonly CommandRepository commandRepository;
        public Handler(CommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public async Task<Response> Handle(QueryModel request, CancellationToken cancellationToken)
        {
            var result = new Response
            {
                EventAssignmentModels = await commandRepository.Query(request)
            };

            return result;
        }
    }
}
