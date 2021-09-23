namespace OWP.FileUpload.Handlers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Criterias;
    using Fanex.Data.Repository;
    using MediatR;
    using File = Models.File;

    public class UploadHandler
    {
        public class UploadCommand : IRequest<File>
        {
            public byte[] FileStream { get; set; }

            public string FileName { get; set; }
        }

        public class Handler : IRequestHandler<UploadCommand, File>
        {

            private readonly IDynamicRepository dynamicRepo;

            public Handler(IDynamicRepository dynamicRepo)
            {
                this.dynamicRepo = dynamicRepo;
            }

            public async Task<File> Handle(UploadCommand request, CancellationToken cancellationToken)
            {
                var criteria = new UploadCriteria
                {
                    FileContent = request.FileStream,
                    Title = request.FileName
                };

                var result = await dynamicRepo.FetchAsync<int>(criteria);

                return new File
                {
                    FileId = result.FirstOrDefault(),
                    Title = request.FileName
                };
            }
        }
    }
}
