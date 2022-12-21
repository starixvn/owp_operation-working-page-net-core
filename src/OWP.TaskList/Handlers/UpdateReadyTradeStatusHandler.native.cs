using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.TaskList.Handlers
{
    using MediatR;

    public class UpdateReadyTradeStatusRequest : IRequest<DateTime>
    {
        public int OWPMatchId { get; set; }
    }
    public class UpdateReadyTradeStatusHandler : IRequestHandler<UpdateReadyTradeStatusRequest, DateTime>
    {
        private readonly Repository repository;

        public UpdateReadyTradeStatusHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<DateTime> Handle(UpdateReadyTradeStatusRequest request, CancellationToken cancellation)
        {
            return await repository.UpdateTradeStatus(request.OWPMatchId);
        }
    }
}
