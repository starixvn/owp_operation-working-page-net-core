using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.TaskList.Handlers
{
    using MediatR;

    public class UpdateReadyTradeStatusRequest : IRequest<string>
    {
        public int OWPMatchId { get; set; }
    }
    public class UpdateReadyTradeStatusHandler : IRequestHandler<UpdateReadyTradeStatusRequest, string>
    {
        private readonly Repository repository;

        public UpdateReadyTradeStatusHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<string> Handle(UpdateReadyTradeStatusRequest request, CancellationToken cancellation)
        {
            var date = await repository.UpdateTradeStatus(request.OWPMatchId);
            return date.ToString("hh:mm tt MMM dd, yyyy");
        }
    }
}
