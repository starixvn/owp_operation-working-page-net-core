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
        public class DownloadRequest : IRequest<FileModel>
        {
            public int FileId { get; set; }
        }

        public class Handler : IRequestHandler<DownloadRequest, FileModel>
        {

            private readonly IDynamicRepository dynamicRepo;

            public Handler(IDynamicRepository dynamicRepo)
            {
                this.dynamicRepo = dynamicRepo;
            }

            public async Task<FileModel> Handle(DownloadRequest request, CancellationToken cancellationToken)
            {
                var criteria = new DownloadCriteria
                {
                    FileId = request.FileId
                };

                var data = await dynamicRepo.FetchAsync<FileModel>(criteria);
                return data.FirstOrDefault();
            }
        }
    }
}
