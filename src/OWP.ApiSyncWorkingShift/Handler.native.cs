using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.ApiSyncWorkingShift.Models;

namespace OWP.ApiSyncWorkingShift
{
    public class QueryWorkingShiftRequest : IRequest<IEnumerable<WorkingShiftModel>>
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }

    public class Handler : IRequestHandler<QueryWorkingShiftRequest, IEnumerable<WorkingShiftModel>>
    {
        private readonly WorkingShiftRepository commandRepository;

        public Handler(WorkingShiftRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public async Task<IEnumerable<WorkingShiftModel>> Handle(QueryWorkingShiftRequest request, CancellationToken cancellationToken)
        {
            var workingShiftModelInternal= await commandRepository.Query(request);
            foreach (var shift in workingShiftModelInternal)
            {
                shift.RecalculateShiftCode();
            }
            return workingShiftModelInternal;
        }
    }
}
