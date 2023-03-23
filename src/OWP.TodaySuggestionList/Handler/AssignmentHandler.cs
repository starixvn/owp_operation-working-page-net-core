using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using MediatR;
using OWP.TodaySuggestionList.Criteria;
using OWP.TodaySuggestionList.Models;

namespace OWP.TodaySuggestionList.Handler
{
    public class AssignmentRequest : IRequest<Unit>
    {
        public DateTime EventDate { get; set; }
    }

    public class AssignmentHandler : IRequestHandler<AssignmentRequest, Unit>
    {
        private readonly DynamicRepository repository;

        public AssignmentHandler(DynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(AssignmentRequest request, CancellationToken cancellationToken)
        {
            var (assignmentModel, maxbetModel) = await repository.FetchMultipleAsync<MaxbetModel, AssignmentModel>(new QueryAssignmentCriteria
            {
                EventDate = request.EventDate
            });
            return Unit.Value;
        }
    }
}