namespace OWP.FileUpload.Handlers
{
    using Criteria;
    using Fanex.Data.Repository;
    using MediatR;
    using OWP.FileUpload.Models;

    public class UploadHandler
    {
        public class UploadCommand : IRequest<FileModel>
        {
            public byte[] FileStream { get; set; }

            public string FileName { get; set; }
        }

        public class Handler : IRequestHandler<UploadCommand, FileModel>
        {
            private readonly IDynamicRepository dynamicRepo;

            public Handler(IDynamicRepository dynamicRepo)
            {
                this.dynamicRepo = dynamicRepo;
            }

            public async Task<FileModel> Handle(UploadCommand request, CancellationToken cancellationToken)
            {
                var criteria = new UploadCriteria
                {
                    FileContent = request.FileStream,
                    Title = request.FileName
                };

                var result = await dynamicRepo.FetchAsync<int>(criteria);

                return new FileModel
                {
                    FileId = result.FirstOrDefault(),
                    Title = request.FileName
                };
            }
        }
    }
}