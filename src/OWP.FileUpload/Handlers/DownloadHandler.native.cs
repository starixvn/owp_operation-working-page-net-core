using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using MediatR;
using OWP.FileUpload.Criteria;
using OWP.FileUpload.Models;

namespace OWP.FileUpload.Handlers
{
    public class DownloadHandler
    {
        public class DownloadRequest : IRequest<File>
        {
            public int FileId { get; set; }
        }

        public class Handler : IRequestHandler<DownloadRequest, File>
        {

            private readonly IDynamicRepository dynamicRepo;

            public Handler(IDynamicRepository dynamicRepo)
            {
                this.dynamicRepo = dynamicRepo;
            }

            public async Task<File> Handle(DownloadRequest request, CancellationToken cancellationToken)
            {
                var criteria = new DownloadCriteria
                {
                    FileId = request.FileId
                };

                var data = await dynamicRepo.FetchAsync<File>(criteria);
                return data.FirstOrDefault();
            }
        }
    }
}
